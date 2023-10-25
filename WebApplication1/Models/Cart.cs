using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Cart
    {
        dbMoviesDataContext db = new dbMoviesDataContext();
        public int movie_id { get; set; }
        [Display(Name ="Movie Name")]
        public string movie_name { get; set; }
        [Display(Name ="Poster")]
        public string poster { get; set; }
        [Display(Name ="Release Date")]
        public DateTime? release_date { get; set; }
        [Display(Name ="Price")]
        public Double price { get; set; }
        [Display(Name = "Quantity")]
        public int iquantity { get; set; }
        [Display(Name ="Total")]
        public Double total
        {
            get { return iquantity * price; }
        }
        public Cart(int id) 
        {
            movie_id = id;
            Movie movie = db.Movies.FirstOrDefault(m => m.movie_id == movie_id);
            movie_name = movie.movie_name;
            poster = movie.poster;
            release_date = movie.release_date;
            price = double.Parse(movie.price.ToString());
            iquantity = 1;
        }
    }
}