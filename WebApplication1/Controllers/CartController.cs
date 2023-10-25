using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        dbMoviesDataContext db = new dbMoviesDataContext();
        public List<Cart> getCart()
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }
        public ActionResult AddCart(int id)
        {
            List<Cart> listCart = getCart();
            Cart cart = listCart.Find(p => p.movie_id == id);
            if (cart == null)
            {
                cart = new Cart(id);
                listCart.Add(cart);
            }
            else
            {
                cart.iquantity++;
            }
            return RedirectToAction("Index", "Home", ViewBag.sumMovieQuantity = sumMovieQuantity());
        }
        public int sumQuantity()
        {
            int sum = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                sum = listCart.Sum(m => m.iquantity);
            }
            return sum;
        }
        public int sumMovieQuantity()
        {
            int sum = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                sum = listCart.Count;
                Session["sumMovieQuantity"] = sum;
            }
            return sum;
        }
        public double Total()
        {
            double tt = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                tt = listCart.Sum(n => n.total);
            }
            return tt;
        }
        public ActionResult Cart()
        {
            List<Cart> listCart = getCart();
            ViewBag.sumQuantity = sumQuantity();
            ViewBag.sumMovieQuantity = sumMovieQuantity();
            ViewBag.Total = Total();
            return View(listCart);
        }
        public ActionResult CartDelete(int id)
        {
            List<Cart> listCart = getCart();
            Cart movie = listCart.SingleOrDefault(p => p.movie_id == id);
            if (movie != null)
            {
                listCart.Remove(movie);
                Session["sumMovieQuantity"] = listCart.Count;
                return RedirectToAction("Cart", "Cart");
            }
            return RedirectToAction("Cart", "Cart");
        }
        public ActionResult CartUpdate(int id, FormCollection collection)
        {
            List<Cart> lstCart = getCart();
            Cart movie = lstCart.SingleOrDefault(n => n.movie_id == id);
            if (movie != null)
            {
                movie.iquantity = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("Cart", "Cart");
        }
        public ActionResult AllCartDelete()
        {
            List<Cart> listCart = getCart();
            listCart.Clear();
            return RedirectToAction("Cart", "Cart");
        }

        [HttpGet]
        public ActionResult PlaceOrder()
        {
            if (Session["Customer"] == null || Session["Customer"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Book");
            }
            List < Cart > listCart = getCart();
            ViewBag.sumQuantity = sumQuantity();
            ViewBag.Total = Total();
            return View(listCart);
        }
        public ActionResult PlaceOrder(FormCollection collection)
        {
            Buy b = new Buy();
            Customer c = (Customer)Session["Customer"];
            Movie m = new Movie();
            List<Cart> listCart = getCart();
            b.cus_id = c.cus_id;
            b.buy_date = DateTime.Now;
            db.Buys.InsertOnSubmit(b);
            db.SubmitChanges();
            foreach (var item in listCart)
            {
                buy_detail buy_Detail = new buy_detail();
                buy_Detail.buy_id = b.buy_id;
                buy_Detail.movie_id = item.movie_id;
                buy_Detail.quantity = item.iquantity;
                buy_Detail.price = Convert.ToDecimal(item.total);
                db.SubmitChanges();
                db.buy_details.InsertOnSubmit(buy_Detail);
            }
            db.SubmitChanges();
            Session["Cart"] = null;
            return RedirectToAction("ConfirmOrder", "Cart");
        }
        public ActionResult ConfirmOrder()
        {
            return View();
        }
    }
}