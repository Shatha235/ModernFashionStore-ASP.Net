using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Services;
using Org.BouncyCastle.Asn1.X9;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IEmailService _emailService;


        public AdminController(ModelContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.FirstOrDefault(u=>u.Userid == userId);
            decimal totalSales = _context.Orders.Sum(o => o.Totalprice);
            // Calculate total customers
            int totalCustomers = _context.Logininfos.Where(u=>u.Userroleid == 2).Count();
            // Calculate total products
            int totalProducts = _context.Products.Count();
            int totalReveiws = _context.Testimonials.Count();
            int totalOrders = _context.Orders.Count();
            int totalContact = _context.Contactrequests.Count();
            ViewData["SalesTarget"] = CalculateSalesTargetFromOrders();
            ViewData["TotalSales"] = totalSales;
            ViewData["TotalCustomers"] = totalCustomers;
            ViewData["TotalProducts"] = totalProducts;
            ViewData["TotalReviews"] = totalReveiws;
            ViewData["TotalOrders"] = totalOrders;
            ViewData["TotalContact"] = totalContact;

            var orders = _context.Orders.ToList();
            var orderDetail = _context.Orderdetails.Include(od => od.Product).ToList();
            var salesByCountry = _context.Orders
                .Include(o => o.User)
                .GroupBy(o => o.User.Usercity)
                .Select(group => new SalesByCountryViewModel
                {
                    Country = group.Key,
                    TotalOrders = group.Count()
                })
                .ToList();


            var model = Tuple.Create<User, IEnumerable<SalesByCountryViewModel> , IEnumerable<Order>, IEnumerable<Orderdetail>>(user , salesByCountry , orders , orderDetail);

            return View(model);
        }


        private int CalculateSalesTargetFromOrders()
        {
            // Retrieve order data for the current month
            DateTime currentMonth = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            // Query your database to get order data
            var orderCount = _context.Orders
                .Where(o => o.Orderdate >= firstDayOfMonth && o.Orderdate < firstDayOfNextMonth)
                .Count();

            // Calculate the target based on the number of orders
            int target = (int)(orderCount * 1.1); // Aim for a 10% increase

            return target;
        }

        public async Task<IActionResult> OrderList(DateTime? startDate, DateTime? endDate)
        {
            // Query to get all orders
            var query = _context.Orders.AsQueryable();

            // Filter based on startDate and endDate
            if (startDate.HasValue)
            {
                query = query.Where(o => o.Orderdate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(o => o.Orderdate <= endDate.Value);
            }
            // Get all orders with related information
            var ordersWithDetails = await query
               .Include(o => o.User)
               .Include(o => o.Orderdetails).ThenInclude(od => od.Product).ThenInclude(p => p.Productattributes).ThenInclude(pa => pa.Color)
               .Include(o => o.Orderdetails).ThenInclude(od => od.Product).ThenInclude(p => p.Productattributes).ThenInclude(pa => pa.Size)
               .ToListAsync();

            // Construct the list of OrderViewModels
            List<OrderViewModel> orderViewModels = ordersWithDetails.Select(order => new OrderViewModel
            {
                Order = order,
                CouponCode = order.Coupon?.Couponcode,
                CouponDiscount = order.Coupon?.Discountpercentage,
                OrderDetails = order.Orderdetails.Select(od => new OrderDetailViewModel
                {
                    OrderDetail = od,
                    Product = od.Product,
                    // Assuming each ProductAttributes has a Color and Size
                    // You need to decide how to handle multiple attributes (e.g., take the first one or a specific one)
                    ColorName = od.Product.Productattributes.FirstOrDefault()?.Color?.Colorname,
                    SizeName = od.Product.Productattributes.FirstOrDefault()?.Size?.Sizename
                }).ToList()
            }).ToList();

            return View(orderViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(decimal orderId, string newStatus)
        {
            //var order = await _context.Orders.FindAsync(orderId);
            var order = _context.Orders.Where(u => u.Orderid == orderId).FirstOrDefault(); 
            var userId = _context.Orders.FirstOrDefault(o => o.Orderid == orderId).Userid;
            var user = _context.Users
                .FirstOrDefault(u => u.Userid == userId);
            if (order == null)
            {
                return NotFound();
            }

            // Add any additional validation you need here
            order.Orderstatus = newStatus;
            if (newStatus == "delivered")
            {
                var userEmail = _context.Logininfos.FirstOrDefault(u => u.Userid == user.Userid).Email;
                var subject = "Your Order Has Been Delivered";
                // Get the absolute URL for the feedback form
                var feedbackUrl = Url.Action("TrackOrder", "Home", new { orderId = order.Orderid }, Request.Scheme);

                // Get the absolute URL for the login page with the returnUrl
                var loginUrl = Url.Action("SignIn", "SigninAndSignup", new { returnUrl = feedbackUrl }, Request.Scheme);

                var message = $"Dear {user.Fname} {user.Lname},\n\nYour order with ID {order.Orderid} has been delivered.\n\nThank you for shopping with us!\n\nDon't forget to give us your feedback: {loginUrl}";
                await _emailService.SendEmailAsync(userEmail, subject, message);

                await _emailService.SendEmailAsync(userEmail, subject, message);

            }

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Orderid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        private bool OrderExists(decimal orderId)
        {
            return _context.Orders.Any(e => e.Orderid == orderId);
        }
        public IActionResult Customers()
        {
            var users = _context.Users
                .Include(u => u.Logininfos)
                .Include(u => u.Userreviews)
                    .ThenInclude(r => r.Product)
                .Where(u => u.Logininfos.Any(li => li.Userroleid == 2)) 
                .ToList();

            return View(users);
        }


        public IActionResult Reviews()
        {
            var reveiws = _context.Testimonials.ToList();

            return View(reveiws);
        }

        public IActionResult Contacts()
        {
            var contact = _context.Contactrequests
                .ToList();

            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReveiwStatus(decimal testId, string newStatus)
        {
            var reveiw = await _context.Testimonials.FindAsync(testId);
            if (reveiw == null)
            {
                return NotFound();
            }

            // Add any additional validation you need here
            reveiw.Approvalstatus = newStatus;

            try
            {
                _context.Update(reveiw);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(reveiw.Testimonialid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate, DateTime? startDate1, DateTime? endDate1)
        //{
        //    var checkUser = HttpContext.Session.GetString("AdminName");

        //    if (!string.IsNullOrEmpty(checkUser))
        //    {
        //        var user = _context.UserLogins.FirstOrDefault(u => u.UserName == checkUser);

        //        if (user != null)
        //        {
        //            var userIdd = user.CustomerId;
        //            var admin = _context.Customers.FirstOrDefault(c => c.Id == userIdd);

        //            if (admin != null)
        //            {
        //                ViewBag.Admin = admin;
        //                if (user.RoleId == 1)
        //                {

        //                    var ordersInfo = _context.OrderInfos.ToList();
        //                    var customer = _context.Customers.ToList();
        //                    var product_customer = _context.ProductCustomers.ToList();
        //                    var addrsses = _context.Addresses.ToList();
        //                    var products = _context.Products.ToList();

        //                    //-----------------------------------------------
        //                    var multiTable1 = from pc in product_customer
        //                                      join a in addrsses on pc.AddressId equals a.Id
        //                                      select new OrdersJoin(_context)
        //                                      {
        //                                          Order = pc,
        //                                          Address = a
        //                                      };


        //                    var modelContext = multiTable1.ToList();

        //                    if (startDate != null && endDate != null)
        //                    {
        //                        modelContext = modelContext.Where(x =>
        //                            x.Order.OrderDate.Value.Date >= startDate && x.Order.OrderDate.Value.Date <= endDate)
        //                            .ToList();
        //                    }
        //                    else if (startDate == null && endDate != null)
        //                    {
        //                        modelContext = modelContext.Where(x =>
        //                            x.Order.OrderDate.Value.Date <= endDate)
        //                            .ToList();
        //                    }
        //                    else if (startDate != null && endDate == null)
        //                    {
        //                        modelContext = modelContext.Where(x =>
        //                           (x.Order.OrderDate.Value.Date >= startDate))
        //                            .ToList();
        //                    }

        //                    //--------------------------------------------------------------------

        //                    var multiTable = from p in products
        //                                     join oi in ordersInfo on p.Id equals oi.ProductId
        //                                     select new
        //                                     {
        //                                         OrderInfo = oi,
        //                                         Products = p
        //                                     };

        //                    var modelContext1 = multiTable.Select(result => new OrdersJoin(_context)
        //                    {
        //                        OrderInfo = new List<OrderInfo> { result.OrderInfo },
        //                        Products = new List<Product>(),
        //                    }).ToList();

        //                    var filteredContext1 = modelContext1.ToList(); // Create a new collection for filtering

        //                    if (startDate1 != null && endDate1 != null)
        //                    {
        //                        filteredContext1 = filteredContext1
        //                            .Where(x =>
        //                                x.OrderInfo.Any(oi =>
        //                                    oi.Order.OrderDate.Value.Date >= startDate1 && oi.Order.OrderDate.Value.Date <= endDate1))
        //                            .ToList();
        //                    }
        //                    else if (startDate1 == null && endDate1 != null)
        //                    {
        //                        filteredContext1 = filteredContext1
        //                            .Where(x =>
        //                                x.OrderInfo.Any(oi => oi.Order.OrderDate.Value.Date <= endDate1))
        //                            .ToList();
        //                    }
        //                    else if (startDate1 != null && endDate1 == null)
        //                    {
        //                        filteredContext1 = filteredContext1
        //                            .Where(x =>
        //                                x.OrderInfo.Any(oi =>
        //                                    oi.Order.OrderDate.Value.Date >= startDate1))
        //                            .ToList();
        //                    }

        //                    // Now, use the filteredContext1 for further processing or displ

        //                    var ordersDetails = Tuple.Create<IEnumerable<OrdersJoin>, IEnumerable<OrdersJoin>>(filteredContext1, modelContext);
        //                    return View(ordersDetails);

        //                }
        //            }
        //        }
        //        return View();
        //    }
        //    else
        //    {
        //        var adminName = HttpContext.Session.GetString("AdminName");

        //        if (!string.IsNullOrEmpty(adminName))
        //        {
        //            HttpContext.Session.Remove("AdminName");
        //        }

        //        HttpContext.SignOutAsync("MyCustomAuthScheme");
        //        return RedirectToAction("Login", "LoginAndRegister");
        //    }
        //}


    }
}
