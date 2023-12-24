using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.ViewComponents
{
    public class MiniCartViewComponent : ViewComponent
    {
        private readonly ModelContext _context;

        public MiniCartViewComponent(ModelContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? userId =  HttpContext.Session.GetInt32("UserId");
            var cartListItem = await _context.Shoppingcarts.Where(x => x.Userid == userId).ToListAsync();
            var products = await _context.Products.ToListAsync();
            var category = await _context.Categories.ToListAsync();
            var color = await _context.Productcolors.ToListAsync();
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

        private IViewComponentResult PartialView(string v, IEnumerable<UserShoppingCartItem> userProduct)
        {
            throw new NotImplementedException();
        }
    }
}
