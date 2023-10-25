using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StatisticController : Controller
    {
        dbMoviesDataContext db = new dbMoviesDataContext();
        public ActionResult StatisticMovie()
        {
            List<SumSaleQuantity> list = 
                (from a in db.Movies join b in db.buy_details on a.movie_id equals b.movie_id
                     group b by b.movie_id into g select new SumSaleQuantity
                           {movieName = g.FirstOrDefault().Movie.movie_name, sumQuantity = g.Sum(x =>x.quantity)}).ToList();
            return View(list);
        }
    }
}