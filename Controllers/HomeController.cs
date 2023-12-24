using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using OnlineStore.Models;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OnlineStore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Net.Mime;
using Org.BouncyCastle.Asn1.X509;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public HomeController(ILogger<HomeController> logger, ModelContext context , IEmailService emailService, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var categoryType = _context.Categorytypes.ToList();
            var productType = _context.Producttypes.ToList();
            var product = _context.Products.ToList();
            var coupon = _context.Coupons.ToList();
            var test = _context.Testimonials.Where(t => t.Approvalstatus == "Approved").ToList();
            var users = _context.Users.Include(u => u.Logininfos).ToList();

            var CategoryTypeAndProductType = Tuple.Create<IEnumerable<Categorytype>, IEnumerable<Producttype>, IEnumerable<Product>, IEnumerable<Coupon>, IEnumerable<Testimonial> , IEnumerable<User>>(categoryType, productType, product, coupon,test, users);
            return View(CategoryTypeAndProductType);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult AddTestnoimal()
        {
            return View();
        }

        public IActionResult AboutUS()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult MyProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                var MyProfileUrl = Url.Action("MyProfile", "Home", Request.Scheme);
                return RedirectToAction("SignIn", "SigninAndSignup", new { returnUrl = MyProfileUrl }, Request.Scheme);

            }
            var userInfo= _context.Users.Where(u=>u.Userid==userId).FirstOrDefault();
            var userLoginInfo = _context.Logininfos.Where(u => u.Userid == userId).FirstOrDefault();
            var newModel = Tuple.Create<User, Logininfo>(userInfo, userLoginInfo);

            return View(newModel);
        }

        public IActionResult EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            var userInfo = _context.Users.Where(u => u.Userid == userId).FirstOrDefault();
            var userLoginInfo = _context.Logininfos.Where(u => u.Userid == userId).FirstOrDefault();
            var model = Tuple.Create<User, Logininfo>(userInfo, userLoginInfo);


            return View(model);
        }


        public IActionResult MyOrders()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Query the database for orders, including order details, products, and their attributes
            var ordersWithDetails = _context.Orders
                                            .Include(order => order.Coupon)
                                            .Where(order => order.Userid == userId)
                                            .Select(order => new
                                            {
                                                Order = order,
                                                OrderDetails = order.Orderdetails
                                                                    .Select(detail => new
                                                                    {
                                                                        Detail = detail,
                                                                        Product = detail.Product,
                                                                        Attribute = detail.Product.Productattributes.FirstOrDefault(), 
                                                                    })
                                            })
                                            .ToList();

            // Project the result into a view model or dynamic object as needed for the view
            var viewModel = ordersWithDetails.Select(o => new OrderViewModel
            {
                Order = o.Order,
                CouponCode = o.Order.Coupon != null ? o.Order.Coupon.Couponcode : null, 
                CouponDiscount = o.Order.Coupon != null ? o.Order.Coupon.Discountpercentage : (decimal?)null,
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailViewModel
                {
                    OrderDetail = od.Detail,
                    Product = od.Product,
                    ColorName = od.Attribute != null ? _context.Productcolors.FirstOrDefault(c => c.Colorid == od.Attribute.Colorid).Colorname : null,
                    SizeName = od.Attribute != null ? _context.Productsizes.FirstOrDefault(s => s.Sizeid == od.Attribute.Sizeid).Sizename : null
                }).ToList()
            }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> TrackOrder(int orderId)
        {

            if (orderId==0)
            {
                // Optionally, add a message to ViewData or ViewBag to display to the user
                ViewData["ErrorMessage"] = "Order ID is required.";
                return View("TrackOrder"); // Assuming "TrackOrder" is the view where the search form is located
            }

            // You may want to ensure that the orderId belongs to the current logged-in user
            int? userId = HttpContext.Session.GetInt32("UserId");

            var orderWithDetails = await _context.Orders
                .Include(order => order.Coupon)
                .Where(order => order.Orderid == orderId && order.Userid == userId)
                .Select(order => new
                {
                    Order = order,
                    OrderDetails = order.Orderdetails
                                        .Select(detail => new
                                        {
                                            Detail = detail,
                                            Product = detail.Product,
                                            Attribute = detail.Product.Productattributes.FirstOrDefault(),
                                        })
                })
                .FirstOrDefaultAsync();

            if (orderWithDetails == null)
            {
                // Optionally, add a message to ViewData or ViewBag to display to the user
                ViewData["ErrorMessage"] = "Order not found or you do not have permission to view it.";
                return View("TrackOrder"); // Assuming "TrackOrder" is the view where the search form is located
            }

            var viewModel = new OrderViewModel
            {
                Order = orderWithDetails.Order,
                CouponCode = orderWithDetails.Order.Coupon != null ? orderWithDetails.Order.Coupon.Couponcode : null,
                CouponDiscount = orderWithDetails.Order.Coupon != null ? orderWithDetails.Order.Coupon.Discountpercentage : (decimal?)null,
                OrderDetails = orderWithDetails.OrderDetails.Select(od => new OrderDetailViewModel
                {
                    OrderDetail = od.Detail,
                    Product = od.Product,
                    ColorName = od.Attribute != null ? _context.Productcolors.FirstOrDefault(c => c.Colorid == od.Attribute.Colorid)?.Colorname : null,
                    SizeName = od.Attribute != null ? _context.Productsizes.FirstOrDefault(s => s.Sizeid == od.Attribute.Sizeid)?.Sizename : null
                }).ToList()
            };

            return View("TrackOrder", viewModel);

        }


        public IActionResult WishList()
        {
            var returnUrl = HttpContext.Request.Path.ToString();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {                // User is not authenticated; you can redirect them to the sign-in page or take another action.

                var WishListUrl = Url.Action("WishList", "Home", Request.Scheme);
                return RedirectToAction("SignIn", "SigninAndSignup", new { returnUrl = WishListUrl }, Request.Scheme);
        
            }


            var wishListItem = _context.Favoriteitems.Where(x => x.Userid == userId).ToList();
            var products = _context.Products.ToList();
            var UserProduct = from w in wishListItem
                              join p in products
                              on w.Productid equals p.Productid
                              select new UserFavItem
                              {
                                  FavItem = w,
                                  Products = p
                              };

            return View(UserProduct);
        }

        public IActionResult CartList()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // User is not authenticated; you can redirect them to the sign-in page or take another action.

                var CartListUrl = Url.Action("CartList", "Home", Request.Scheme);
                return RedirectToAction("SignIn", "SigninAndSignup", new { returnUrl = CartListUrl }, Request.Scheme);
            }

            var cartListItem = _context.Shoppingcarts.Where(x => x.Userid == userId).ToList();
            var products = _context.Products.ToList();
            var category = _context.Categories.ToList();
            var color = _context.Productcolors.ToList();
            var size = _context.Productsizes.ToList();

            var UserProduct = from c in cartListItem
                              join p in products on c.Productid equals p.Productid
                              join ca in category on p.Categoryid equals ca.Categoryid
                              join co in color on c.Colorid equals co.Colorid into colorJoin
                              from co in colorJoin.DefaultIfEmpty()
                              join s in size on c.Sizeid equals s.Sizeid into sizeJoin
                              from s in sizeJoin.DefaultIfEmpty()
                              select new UserShoppingCartItem
                              {
                                  CartItem = c,
                                  Products = p,
                                  Categories = ca,
                                  Colors = co, 
                                  Sizes = s   
                              };

            return View(UserProduct);
        }

        public IActionResult ProcessToCheckOutOrder()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var cartListItem = _context.Shoppingcarts.Where(x => x.Userid == userId).ToList();
            var products = _context.Products.ToList();
            var category = _context.Categories.ToList();
            var color = _context.Productcolors.ToList();
            var size = _context.Productsizes.ToList();
            var UserProduct = from c in cartListItem
                              join p in products on c.Productid equals p.Productid
                              join ca in category on p.Categoryid equals ca.Categoryid
                              join co in color on c.Colorid equals co.Colorid into colorJoin
                              from co in colorJoin.DefaultIfEmpty()
                              join s in size on c.Sizeid equals s.Sizeid into sizeJoin
                              from s in sizeJoin.DefaultIfEmpty()
                              select new UserShoppingCartItem
                              {
                                  CartItem = c,
                                  Products = p,
                                  Categories = ca,
                                  Colors = co,
                                  Sizes = s
                              };
            var user = _context.Users.Where(u => u.Userid == userId).FirstOrDefault();
            var newModel = Tuple.Create<IEnumerable<UserShoppingCartItem>, User>(UserProduct, user);


            return View(newModel);
        }

        public async Task<IActionResult> DeleteWishlist()
        {
            // Get the currently authenticated user
            var user = HttpContext.Session.GetInt32("UserId");

            if (user != null)
            {
                int userId = user.Value;

                var wishlistItems = _context.Favoriteitems.Where(w => w.Userid == userId).ToList();

                // Remove wishlist items
                _context.Favoriteitems.RemoveRange(wishlistItems);

                _context.SaveChanges();

                return RedirectToAction("Wishlist");
            }
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> DeleteCartlist()
        {
            // Get the currently authenticated user
            var user = HttpContext.Session.GetInt32("UserId");

            if (user != null)
            {
                int userId = user.Value;

                var carlistItems = _context.Shoppingcarts.Where(w => w.Userid == userId).ToList();

                // Remove wishlist items
                _context.Shoppingcarts.RemoveRange(carlistItems);

                _context.SaveChanges();

                return RedirectToAction("Cartlist");
            }
            return RedirectToAction("Login");
        }

        public IActionResult RemoveFromWishlist(int id)
        {
            var user = HttpContext.Session.GetInt32("UserId");
            int userId = user.Value;
            var wishlistItems = _context.Favoriteitems.Where(w => w.Userid == userId && w.Productid == id);

            _context.Favoriteitems.RemoveRange(wishlistItems);
            _context.SaveChanges();
            return RedirectToAction("Wishlist");
        }

        public IActionResult RemoveFromCartlist(int id)
        {
            var user = HttpContext.Session.GetInt32("UserId");
            int userId = user.Value;
            var carlistItems = _context.Shoppingcarts.Where(s => s.Userid == userId && s.Shoppingcartid == id);

            _context.Shoppingcarts.RemoveRange(carlistItems);
            _context.SaveChanges();
            return RedirectToAction("Cartlist");
        }


        public IActionResult GetProductByCategoryType(int id)
        {
            var products = _context.Products.Where(p => p.Categoryid == id)
                .Join(_context.Producttypes,
               p => p.Producttypeid, pt => pt.Typeid,
               (Product, ProductType) => new ProductWithType
               {
                   ProductTypeName = ProductType.Typename,
                   Product = Product

               }).ToList();
            return View(products);
        }

        public IActionResult GetProductByProductType(int id)
        {
            var products = _context.Products.Where(p => p.Producttypeid == id)
                .ToList();
            return View(products);
        }

        public async Task<IActionResult> ProductDetails(decimal? id )
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            // Fetch the product details
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Fetch images, attributes, reviews, and ratings for the product
            var images = _context.Productimages.Where(img => img.Productid == id).ToList();
            var attributes = _context.Productattributes.Where(attr => attr.Productid == id).ToList();
            var reviews = _context.Userreviews.Where(review => review.Productid == id).ToList();
            var ratings = _context.Ratings.Where(rating => rating.Productid == id).ToList();
            var users = _context.Users.ToList();
            var colors = _context.Productcolors.ToList();
            var sizes = _context.Productsizes.ToList();
            // Create a ViewModel to package all the data
            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Images = images,
                Attributes = attributes,
                Reviews = reviews,
                Ratings = ratings,
                Users = users,
                Colors = colors,
                Sizes = sizes
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddReview(int id, [Bind("Rating, UserReview")] ProductDetailsViewModel model)
        {
            // Check if the user is authenticated based on your session setup
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // User is not authenticated; you can redirect them to the sign-in page or take another action.
                var ReturnUrl = Url.Action("ProductDetails", "Home", new { id= id }, Request.Scheme);
                return RedirectToAction("SignIn", "SigninAndSignup", new { returnUrl = ReturnUrl }, Request.Scheme);
            }

            try
            {
                // Retrieve the user's ID from the session
                decimal userDecId = userId.Value;

                var newReview = new Userreview()
                {
                    Productid = id,
                    Userid = userDecId,
                    Comments = model.UserReview,
                    Reviewdate = DateTime.Now
                };

                var newRating = new Rating()
                {
                    Productid = id,
                    Userid = userDecId,
                    Rating1 = model.Rating,
                    Ratingdate = DateTime.Now

                };

                _context.Userreviews.Add(newReview);
                _context.Ratings.Add(newRating);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("ProductDetails", "Home", new { id = id });
        }


        [HttpPost]
        public async Task<IActionResult> AddToFavorite(int id)
        {
            // Check if the user is authenticated based on your session setup
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // User is not authenticated; you can redirect them to the sign-in page or take another action.
                var ReturnUrl = Url.Action("ProductDetails", "Home", new { id = id }, Request.Scheme);
                return RedirectToAction("SignIn", "SigninAndSignup", new { returnUrl = ReturnUrl }, Request.Scheme);
            }

            try
            {
                // Check if the product already exists in the user's favorite items
                bool productAlreadyExists = _context.Favoriteitems.Any(f => f.Userid == userId && f.Productid == id);

                if (!productAlreadyExists)
                {
                    // Product does not exist in the favorite items; add it
                    var newFav = new Favoriteitem
                    {
                        Userid = userId.Value,
                        Productid = id
                    };

                    _context.Favoriteitems.Add(newFav);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("ProductDetails", "Home", new { id = id });
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productid, int quantity, int colorid, int sizeid, int categoryid)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "User is not authenticated" });
            }

            try
            {
                if (categoryid == 2) // Clothes
                {
                    if (colorid == 0 || sizeid == 0)
                    {
                        return Json(new { success = false, message = "Please select both color and size." });
                    }

                    var productAlreadyExists = _context.Shoppingcarts
            .FirstOrDefault(f => f.Userid == userId && f.Productid == productid &&
                                 (categoryid == 2 ? (f.Colorid == colorid && f.Sizeid == sizeid) : true));
                    if (productAlreadyExists!=null)
                    {
                        productAlreadyExists.Quantity += 1;
                        await _context.SaveChangesAsync(); 
                        return Json(new { success = false, message = "Item is already in the cart! The quantity has been updated!" });
                    }

                    var newCart = new Shoppingcart
                    {
                        Userid = userId.Value,
                        Productid = productid,
                        Quantity = quantity,
                        Colorid = colorid,
                        Sizeid = sizeid
                    };

                    _context.Shoppingcarts.Add(newCart);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Item is added successfully!" });
                }
                else if (categoryid == 1) // Acessories
                {
                    var productAlreadyExists = _context.Shoppingcarts
            .FirstOrDefault(f => f.Userid == userId && f.Productid == productid);
                    if (productAlreadyExists!=null)
                    {
                        productAlreadyExists.Quantity += 1;
                        await _context.SaveChangesAsync(); 
                        return Json(new { success = false, message = "Item is already in the cart! The quantity has been updated! " });
                    }

                    var newCart = new Shoppingcart
                    {
                        Userid = userId.Value,
                        Productid = productid,
                        Quantity = quantity,
                        Colorid = null,
                        Sizeid = null

                    };

                    _context.Shoppingcarts.Add(newCart);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Item is added successfully!" });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "An error occurred while adding the product to the cart." });
            }

            return Json(new { success = false, message = "Invalid category or data provided!" });
        }

        private string GenerateInvoiceContent(Order order, List<UserShoppingCartItem> cartItems, Coupon appliedCoupon)
        {
            var invoiceBuilder = new StringBuilder();

            invoiceBuilder.AppendLine($"Order ID: {order.Orderid}");
            invoiceBuilder.AppendLine($"Order Date: {order.Orderdate:dd MMM yyyy}");

            if (appliedCoupon != null)
            {
                invoiceBuilder.AppendLine($"Total Price Before Discount: {order.Totalprice * (1 - appliedCoupon.Discountpercentage / 100)}");
                invoiceBuilder.AppendLine($"Total Price After Discount: {order.Totalprice}");
                invoiceBuilder.AppendLine($"Coupon Code: {appliedCoupon.Couponcode}");
                invoiceBuilder.AppendLine($"Coupon Discount: {appliedCoupon.Discountpercentage}%");

            }
            else
            {
                invoiceBuilder.AppendLine($"Total Price : {order.Totalprice}");

            }

            invoiceBuilder.AppendLine("Order Details:");
            foreach (var item in cartItems)
            {
                invoiceBuilder.AppendLine($"Product: {item.Products.Name}, Quantity: {item.CartItem.Quantity}, Price: {item.Products.Price}");
                if (!string.IsNullOrEmpty(item.Sizes?.Sizename))
                {
                    invoiceBuilder.AppendLine($"Size: {item.Sizes.Sizename}");
                }
            }

            return invoiceBuilder.ToString();
        }

        public byte[] GeneratePdfInvoice(Order order, List<UserShoppingCartItem> cartItems, Coupon appliedCoupon)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Creating a table for the order and coupon details
                PdfPTable orderTable = new PdfPTable(2); // 2 columns
                orderTable.AddCell("Order ID");
                orderTable.AddCell(order.Orderid.ToString());
                orderTable.AddCell("Order Date");
                orderTable.AddCell(order.Orderdate.ToString("dd MMM yyyy"));

                if (appliedCoupon != null)
                {
                    orderTable.AddCell("Total Price Before Discount");
                    orderTable.AddCell((order.Totalprice / (1 - appliedCoupon.Discountpercentage / 100)).ToString());
                    orderTable.AddCell("Total Price After Discount");
                    orderTable.AddCell(order.Totalprice.ToString());
                    orderTable.AddCell("Coupon Code");
                    orderTable.AddCell(appliedCoupon.Couponcode);
                    orderTable.AddCell("Coupon Discount");
                    orderTable.AddCell(appliedCoupon.Discountpercentage.ToString() + "%");
                }
                else
                {
                    orderTable.AddCell("Total Price");
                    orderTable.AddCell(order.Totalprice.ToString());
                }
                pdfDoc.Add(orderTable);

                // Adding a space
                pdfDoc.Add(new Paragraph(" "));

                // Creating a table for cart items
                PdfPTable cartTable = new PdfPTable(4); // 4 columns for Product, Quantity, Price, Size
                cartTable.AddCell("Product");
                cartTable.AddCell("Quantity");
                cartTable.AddCell("Price");
                cartTable.AddCell("Size");

                foreach (var item in cartItems)
                {
                    cartTable.AddCell(item.Products.Name);
                    cartTable.AddCell(item.CartItem.Quantity.ToString());
                    cartTable.AddCell(item.Products.Price.ToString());
                    cartTable.AddCell(item.Sizes?.Sizename ?? "N/A"); // Handle null sizes
                }
                pdfDoc.Add(cartTable);

                pdfDoc.Close();
                return stream.ToArray();
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddOrder([Bind("Cardnumber, Expirydate, Cvv")] Staticcard inputCard, string couponName)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                var ReturnUrl = Url.Action("ProcessToCheckOutOrder", "Home", Request.Scheme);
                return RedirectToAction("SignIn", "SigninAndSignup", new { returnUrl = ReturnUrl }, Request.Scheme);
            }

            var cartListItem = _context.Shoppingcarts.Where(x => x.Userid == userId).ToList();
            var products = _context.Products.ToList();
            var category = _context.Categories.ToList();
            var color = _context.Productcolors.ToList();
            var size = _context.Productsizes.ToList();
            //get the userCart
            var userCart = cartListItem.Select(cartItem => new UserShoppingCartItem
            {
                CartItem = cartItem,
                Products = products.FirstOrDefault(p => p.Productid == cartItem.Productid),
                Categories = category.FirstOrDefault(c => c.Categoryid == cartItem.Product.Categoryid), 
                Colors = color.FirstOrDefault(c => c.Colorid == cartItem.Colorid), 
                Sizes = size.FirstOrDefault(s => s.Sizeid == cartItem.Sizeid) 
            }).ToList();

            var email = _context.Logininfos.Where(u => u.Userid == userId).FirstOrDefault().Email;

            using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Check the card details
                        var userCard = _context.Staticcards
                            .FirstOrDefault(card => card.Cardnumber == inputCard.Cardnumber && card.Cvv == inputCard.Cvv && card.Expirydate.Value.Date == inputCard.Expirydate.Value.Date);

                        if (userCard == null)
                        {
                        return Json(new { success = false, message = "Card details are incorrect!" });
                    }

                    // 2. Verify balance
                    decimal grandTotalPrice = UserShoppingCartItem.CalculateGrandPrice((IEnumerable<UserShoppingCartItem>)userCart);

                        // 3. Adjust the total order price if a valid coupon is used
                        Coupon appliedCoupon = null;
                        if (!string.IsNullOrEmpty(couponName))
                        {
                            appliedCoupon = _context.Coupons.FirstOrDefault(c => c.Couponcode == couponName && c.Startdate <= DateTime.Now && c.Enddate >= DateTime.Now);
                            if (appliedCoupon != null)
                            {
                                decimal discountAmount = grandTotalPrice * appliedCoupon.Discountpercentage / 100;
                                grandTotalPrice -= discountAmount;
                            }
                        }

                        if (userCard.Balance < grandTotalPrice)
                        {
                        return Json(new { success = false, message = "Insufficient funds!" });
                        }

                        // 4. Deduct the order price from the card's balance
                        userCard.Balance -= grandTotalPrice;
                        _context.Update(userCard);
                    await _context.SaveChangesAsync();  


                    // 5. Insert the order details in the order table
                    var order = new Order
                        {
                            Userid = userId.Value,
                            Totalprice = grandTotalPrice,
                            Orderdate = DateTime.Now,
                            Couponid = appliedCoupon?.Couponid,
                            Orderstatus = "Pending"
                        };
                        _context.Orders.Add(order);
                        await _context.SaveChangesAsync();

                        //add order details for each cart item
                        foreach (var cartItem in userCart)
                        {
                            var orderDetail = new Orderdetail
                            {
                                Orderid = order.Orderid, 
                                Productid = cartItem.Products.Productid, 
                                Quantity = (decimal)cartItem.CartItem.Quantity
                            };

                            _context.Orderdetails.Add(orderDetail);

                        // Update the product quantity
                        var productToUpdate = _context.Products.Find(cartItem.Products.Productid);
                        if (productToUpdate != null)
                        {
                            // Convert cartItem quantity to the same type as productToUpdate.Quantity if they differ
                            decimal quantityToDeduct = (decimal)cartItem.CartItem.Quantity; 
                            productToUpdate.Quantity -= quantityToDeduct;

                            // Check if the product is out of stock
                            if (productToUpdate.Quantity <= 0)
                            {
                                productToUpdate.Quantity = 0; // Ensure quantity doesn't go negative
                                productToUpdate.Productstatus = "Out of Stock";
                            }

                            _context.Update(productToUpdate);
                        }
                    }

                        await _context.SaveChangesAsync();


                    string invoiceContent = GenerateInvoiceContent(order, userCart, appliedCoupon);
                    byte[] pdfBytes = GeneratePdfInvoice(order, userCart, appliedCoupon);

                    if (pdfBytes == null)
                    {
                        return Json(new { success = false, message = "PDF generation returned null." });
                        // Handle the error scenario
                    }
                    using (MemoryStream ms = new MemoryStream(pdfBytes))
                    { 
                        // Reset the memory stream position to the beginning
                        ms.Position = 0;

                        Attachment attachment = new Attachment(ms, "OrderInvoice.pdf", MediaTypeNames.Application.Pdf);
                        attachment.ContentDisposition.FileName = "OrderInvoice.pdf"; // Optional, for setting file name

                        var trackOrderUrl = Url.Action("TrackOrder", "Home", Request.Scheme);

                        // Get the absolute URL for the login page with the returnUrl
                        var loginUrl = Url.Action("SignIn", "SigninAndSignup", new { returnUrl = trackOrderUrl }, Request.Scheme);

                        // Send confirmation email with attachment
                        var userEmail = email;
                        var subject = "Order Confirmation";
                        var emailMessage = $"Your order with ID {order.Orderid} has been successfully placed.\n\nBig thanks from everyone at She & He Store for choosing to shop with us! You can track you order here : {loginUrl}";
                        await _emailService.SendEmailWithAttachmentAsync(userEmail, subject, emailMessage, attachment);
                    }
                    // clear the shopping cart
                    await DeleteCartlist();

                        // Commit the transaction
                        transaction.Commit();

                    return Json(new { success = true, message = "Order placed successfully!" });

                    }
                    catch (Exception ex)
                    {
                    // Log the error
                    return Json(new { success = false, message = $"Error occurred in AddOrder: {ex.Message}, Stack Trace: {ex.StackTrace}" });

                    if (ex.InnerException != null)
                    {
                        return Json(new { success = false, message = $"Inner Exception: {ex.InnerException.Message}" });
                    }

                    // Rollback the transaction
                    transaction.Rollback();

                    // For debugging purposes, rethrow the exception to get more details
                    // Remove this line in production code
                    throw;
                }
                }

        }

        [HttpPost]
        public async Task<IActionResult> AddTestnoimal(string feedback )
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("SignIn", "SigninAndSignup");
            }

            var user = _context.Users.Where(u=>u.Userid == userId).FirstOrDefault();
            var userInfo = _context.Logininfos.Where(u => u.Userid == userId).FirstOrDefault();

            try
            {

                var newFed = new Testimonial
                {
                    Name = user.Fname + " "+ user.Lname,
                    Email = userInfo.Email  ,
                    Feedback = feedback , 
                    Submissiondate = DateTime.Now
                };

                    _context.Testimonials.Add(newFed);
                    await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }

            return Json(new { success = true, message = "Your input is a gift that keeps us moving forward. Thank you for trusting us and for taking the time to share your experience. We're here because of you, and we're better with you." });
        }


        [HttpPost]
        public async Task<IActionResult> AddContact(string Name , string Email , string Message)
        {

            try
            {

                var newCon = new Contactrequest
                {
                    Name = Name,
                    Email = Email,
                    Message = Message,
                    Submissiondate = DateTime.Now
                };

                _context.Contactrequests.Add(newCon);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }

            return Json(new { success = true, message = "Your Message sent" });
        }

        [HttpPost]
        public async Task<IActionResult> EditQuantity( int productId , int newQuantity)
        {

            try
            {
                var cartItem = _context.Shoppingcarts
                    .Where(x => x.Productid == productId).FirstOrDefault();
                cartItem.Quantity = newQuantity;
                _context.Shoppingcarts.Update(cartItem);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "The Quantity is updated" });


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile([Bind("Fname,Lname,Birthdate,Gender,Usercity,Phonenumber,ImageFile")] User user, string Email , string NewPassword, string ConfirmNewPassword)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var ActiveUser = _context.Users.FirstOrDefault(u => u.Userid == userId); 
            if(ActiveUser!=null)
            {
                // If the user uploaded a new profile picture, handle the file upload
                if (user.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(fileStream);
                    }

                    // Assuming you want to replace the old image
                    user.Imagepath = fileName;
                }

                ActiveUser.Fname = user.Fname;
                ActiveUser.Lname = user.Lname;
                ActiveUser.Birthdate = user.Birthdate;
                ActiveUser.Gender = user.Gender;
                ActiveUser.Usercity = user.Usercity;
                ActiveUser.Phonenumber = user.Phonenumber;
                ActiveUser.Imagepath = user.Imagepath;

                // Update the user's details
                _context.Users.Update(ActiveUser);
                _context.SaveChanges();

                // Update login info if the email was changed
                var loginInfo = await _context.Logininfos.FirstOrDefaultAsync(l => l.Userid == userId);
                if (Email != null)
                {
                    loginInfo.Email = Email;
                    _context.Logininfos.Update(loginInfo);
                    _context.SaveChanges();
                }

                if (NewPassword != null && ConfirmNewPassword != null)
                {
                    if (NewPassword == ConfirmNewPassword)
                    {

                        loginInfo.Password = NewPassword; // Ideally, you should hash this password
                        _context.Logininfos.Update(loginInfo);
                        _context.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("", "New passwords do not match.");
                        return View(user);
                    }
                }

                return RedirectToAction("MyProfile");

            }
            return View(); 

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}