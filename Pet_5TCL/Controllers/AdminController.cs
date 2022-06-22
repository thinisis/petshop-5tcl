using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Pet_5TCL.Models;

namespace Pet_5TCL.Controllers
{
    public class AdminController : Controller
    {
        DataContext accDB = new DataContext();
        // GET: Admin
        public ActionResult Index()
        {
            ViewData["Title"] = "Admin Panel";
            if (Session["LoginSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["Admin"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.UserCount = GetUserCount();
                    return View();
                }
            }

            return View();
        }

        public ActionResult AccountManager()
        {
            if (Session["Admin"] != null)
            {
                @ViewData["Title"] = "Quản lý tài khoản";
                List<account> accounts = accDB.accounts.ToList();
                if(accounts.Count != 0)
                {
                    return View(accounts);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccountEdit(int? id)
        {
            if (Session["Admin"] != null)
            {
                account acc = accDB.accounts.FirstOrDefault(p => p.id == id);
                if(acc != null)
                {
                    ViewBag.AccUser = "Chỉnh sửa tài khoản " + acc.username;
                    ViewBag.AccID = "ID của tài khoản: " + acc.id;
                    return View(acc);
                }
                else
                {
                    return RedirectToAction("AccountManager", "Admin");
                }
            }
            return RedirectToAction("Index", "Home");
            
        }
        [HttpPost]
        public ActionResult AccountEdit(FormCollection collection, int id)
        {
            account acc = accDB.accounts.FirstOrDefault(p => p.id == id);
            acc.password = GetMD5(collection["password"]);
            acc.email = collection["email"];
            acc.phone = collection["phone"];
            acc.admin = int.Parse(collection["admin"]);
            acc.name = collection["name"];
            acc.active = int.Parse(collection["active"]);
            accDB.SaveChanges();
            return RedirectToAction("AccountManager", "Admin");
        }

        public int GetUserCount()
        {
            int b = accDB.accounts.Count(p => p.id != null);
            return b;
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}