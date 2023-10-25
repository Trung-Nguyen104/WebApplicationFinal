using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        dbMoviesDataContext db = new dbMoviesDataContext();
        public ActionResult Order(int id)
        {
            List<Orders> list = (from a in db.Buys
                                 join b in db.buy_details on a.buy_id equals b.buy_id
                                 where a.cus_id == id
                                 group b by b.buy_id into g select new Orders
                                 {
                                     orderId = g.FirstOrDefault().Buy.buy_id,
                                     orderDate = g.FirstOrDefault().Buy.buy_date,
                                     total = g.Sum(x => x.price)
                                 }).ToList();
            return View(list);
        }
        public ActionResult OrderDetail(int id)
        {
            List<OrderDetail> list = (from a in db.Movies
                                 join b in db.buy_details on a.movie_id equals b.movie_id
                                 where b.buy_id == id
                                 group b by b.movie_id into g select new OrderDetail
                                 {
                                     buyId = id,
                                     movieName = g.FirstOrDefault().Movie.movie_name,
                                     quantity = g.FirstOrDefault().quantity,
                                     price = g.FirstOrDefault().price
                                 }).ToList();
            return View(list);
        }
    }
}