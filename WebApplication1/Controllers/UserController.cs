using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        dbMoviesDataContext db = new dbMoviesDataContext();
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, Customer c)
        {
            var name = collection["cus_name"];
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
                    c.cus_name = name;
                    c.password = password;
                    c.email = email;
                    c.phone = phone;
                    db.Customers.InsertOnSubmit(c);
                    db.SubmitChanges();
                    Session["email"] = email;
                    Session["password"] = password;
                    if (Session["email"] != null && Session["password"] != null)
                    {
                        var email_after_register = Session["email"];
                        var password_after_register = Session["password"];
                        Customer c_after_register = db.Customers.SingleOrDefault(n => n.email == email as string && n.password == password as string);
                        Session["Customer"] = c_after_register;
                    }
                    return RedirectToAction("Index", "Home");
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
            Customer c = db.Customers.SingleOrDefault(n => n.email == email && n.password == password);
            if (c != null)
            {
                ViewBag.ThongBao = "Login Successful";
                Session["Customer"] = c;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Email or password is incorrect";
            }
            return this.Login();
        }

        public ActionResult Account_Info(int id)
        {
            var customer = db.Customers.First(c => c.cus_id == id);
            return View(customer);
        }

        public ActionResult LogOut() 
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
