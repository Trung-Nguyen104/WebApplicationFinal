using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        dbMoviesDataContext db = new dbMoviesDataContext();
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, Admin a)
        {
            var name = collection["ad_name"];
            var password = collection["password"];
            var confirmpassword = collection["confirmpassword"];
            var email = collection["email"];
            var phone = collection["phone"];
            if (String.IsNullOrEmpty(confirmpassword))
            {
                ViewData["enterpassword"] = "Please enter password to confirm";
            }
            else
            {
                if (!password.Equals(confirmpassword))
                {
                    ViewData["samepassword"] = "Password not same";
                }
                else
                {
                    a.ad_name = name;
                    a.password = password;
                    a.email = email;
                    db.Admins.InsertOnSubmit(a);
                    db.SubmitChanges();
                    Session["email"] = email;
                    Session["password"] = password;
                    if (Session["email"] != null && Session["password"] != null)
                    {
                        var email_after_register = Session["email"];
                        var password_after_register = Session["password"];
                        Admin a_after_register = db.Admins.SingleOrDefault(n => n.email == email as string && n.password == password as string);
                        Session["Admin"] = a_after_register;
                    }
                    return RedirectToAction("Index", "Admin");
                }
            }
            return this.Register();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var email = collection["email"];
            var password = collection["password"];
            Admin a = db.Admins.SingleOrDefault(n => n.email == email && n.password == password);
            if (a != null)
            {
                ViewBag.ThongBao = "Login Successful";
                Session["Admin"] = a;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.ThongBao = "Email or password is incorrect";
            }
            return this.Login();
        }
        public ActionResult Index()
        {
            var all_movie = from movies in db.Movies select movies;
            return View(all_movie);
        }
        public ActionResult Detail(int id)
        {
            var movie_Detail = db.Movies.Where(m => m.movie_id == id).FirstOrDefault();
            return View(movie_Detail);
        }
        public ActionResult Edit(int id)
        {
            var movie_Edit = db.Movies.FirstOrDefault(m => m.movie_id == id);
            return View(movie_Edit);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var movie_id = db.Movies.FirstOrDefault(m => m.movie_id == id);
            var m_name = collection["movie_name"];
            var m_poster = collection["poster"];
            var m_director = collection["director"];
            var m_price = Convert.ToDecimal(collection["price"]);
            var m_release_date = Convert.ToDateTime(collection["release_date"]);
            movie_id.movie_id = id;
            if (string.IsNullOrEmpty(m_name))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                movie_id.movie_name = m_name;
                movie_id.poster = m_poster;
                movie_id.director = m_director;
                movie_id.price = m_price;
                movie_id.release_date = m_release_date;
                UpdateModel(movie_id);
                db.SubmitChanges();
                return RedirectToAction("Index", "Admin");
            }
            return this.Edit(id);
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Movie m)
        {
            var m_name = collection["movie_name"];
            var m_poster = collection["poster"];
            var m_director = collection["director"];
            var m_price = Convert.ToDecimal(collection["price"]);
            var m_release_date = Convert.ToDateTime(collection["release_date"]);
            if (string.IsNullOrEmpty(m_name))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                m.movie_name = m_name;
                m.poster = m_poster;
                m.director = m_director;
                m.price = m_price;
                m.release_date = m_release_date;
                db.Movies.InsertOnSubmit(m);
                db.SubmitChanges();
                return RedirectToAction("Index", "Admin");
            }
            return this.Create();
        }
        public ActionResult Delete(int id)
        {
            var movie_Del = db.Movies.FirstOrDefault(m => m.movie_id == id);
            return View(movie_Del);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var movie_Del = db.Movies.FirstOrDefault(m => m.movie_id == id);
            db.Movies.DeleteOnSubmit(movie_Del);
            db.SubmitChanges();
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult Account_Info(int id)
        {
            var ad = db.Admins.First(c => c.ad_id == id);
            return View(ad);
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}