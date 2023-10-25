using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SearchController : Controller
    {
        dbMoviesDataContext db = new dbMoviesDataContext();
        public ActionResult Search(string searchString)
        {
            var all_movie = from movies in db.Movies select movies;
            if (!string.IsNullOrEmpty(searchString)) all_movie = (IOrderedQueryable<Movie>)
            all_movie.Where(m => m.movie_name.Contains(searchString));
            return View(all_movie);
        }
    }
}