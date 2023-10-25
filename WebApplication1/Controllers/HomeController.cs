using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        dbMoviesDataContext db = new dbMoviesDataContext();
        public ActionResult Index()
        {
            var all_movie = from movies in db.Movies select movies;
            return View(all_movie);
        }
        public ActionResult Detail(int id)
        {
            var movie_detail = db.Movies.First(p => p.movie_id == id);
            return View(movie_detail);
        }
    }
}