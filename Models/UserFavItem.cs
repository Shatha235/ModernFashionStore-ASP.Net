using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineStore.Models
{
    public class UserFavItem
    {
        public Favoriteitem FavItem { get; set; }

        public Product Products { get; set; }


    }
}
