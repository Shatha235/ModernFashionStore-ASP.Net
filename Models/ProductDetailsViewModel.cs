using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Models
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Productimage> Images { get; set; }
        public List<Productattribute> Attributes { get; set; }
        public List<Userreview> Reviews { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<User> Users { get; set; }
        public List<Productcolor> Colors { get; set; }
        public List<Productsize> Sizes { get; set; }
        // Calculate the average rating
        public double AverageRating
        {
            get
            {
                if (Ratings != null && Ratings.Any())
                {
                    return Math.Round(Ratings.Average(r => ((double)r.Rating1)) , 2);
                }

                return 0; // You can return a default value when there are no ratings.
            }
        }

        // Properties for user review and rating input
        public string UserReview { get; set; }
        public decimal Rating { get; set; }
    }

}

