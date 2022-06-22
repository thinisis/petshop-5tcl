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
    public class AccountController : Controller
    {
        DataContext accDB = new DataContext();
        // GET: Account
        public ActionResult Login()
        {

            if (Session["LoginSession"] == null)
            {
                @ViewData["Title"] = "Đăng nhập";
                return View();
            }
            else
            {
                return Content("<script>alert('Bạn đă đăng nhập, không thể đăng nhập nữa!'); window.location.href='/';</script>");
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (Session["LoginSession"] == null)
            {
                if (username == null || password == null)
                {
                    ViewBag.msgFail = "Vui lòng nhập đầy đủ thông tin!";
                    return Login();
                }
                else
                {
                    account acc = accDB.accounts.FirstOrDefault(a => a.username == username);
                    if (acc == null)
                    {
                        ViewBag.msgFail = "Tài khoản không tồn tại!";
                        return Login();
                    }
                    else
                    {
                        if (acc.password != GetMD5(password))
                        {
                            ViewBag.msgFail = "Mật khẩu không chính xác!";
                            return Login();
                        }
                        else
                        {
                            if (ActiveStatus(username) == true)
                            {
                                SetSession(username);
                                AdminSession(username);
                            }
                            else
                            {
                                ViewBag.msgFail = "Tài khoản đã bị khoá! Vui lòng liên hệ admin để biết thêm chi tiết!";
                                return Login();
                            }
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return Content("<script>alert('Bạn đă đăng nhập, không thể đăng nhập nữa!'); window.location.href='/';</script>");
            }
            
        }


        public ActionResult Reg()
        {
            ViewData["Title"] = "Đăng ký";
            if (GetSession() == true)
            {
                return Content("<script>alert('Bạn đă đăng nhập, vui lòng đăng xuất để tiếp tục!'); window.location.href='/';</script>");
            }
            else
            {

                return View("Reg");
            }
        }

        [HttpPost]
        public ActionResult Reg(FormCollection collection)
        {
            if(GetSession() == true)
            {
                return Content("<script>alert('Bạn đă đăng nhập, vui lòng đăng xuất để tiếp tục!'); window.location.href='/';</script>");
            }
            else
            {
                account newacc = new account();
                newacc.username = collection["username"];
                if (collection["password"] != collection["repassword"])
                {
                    ViewBag.msgFail = "Mật khẩu nhập lại không đúng";
                    return Reg();
                }
                else
                {
                    newacc.password = GetMD5(collection["password"]);
                }
                newacc.email = collection["email"];
                newacc.created = DateTime.Now;
                newacc.admin = 0;
                newacc.active = 1;
                newacc.name = collection["name"];
                newacc.phone = collection["phone"];
                try
                {
                    accDB.accounts.Add(newacc);
                    accDB.SaveChangesAsync();
                    ViewBag.msgSuccess = "Tạo tài khoản thành công!";
                    return Reg();
                }
                catch(Exception ex)
                {
                    ViewBag.msgFail = "Không xác định!";
                    return Reg();
                }
                
            }
        }

        public ActionResult Logout()
        {
            EmptySession();
            return RedirectToAction("Index","Home");
        }

        public bool GetSession() //Lay session dang nhap
        {
            if (Session["LoginSession"] == null)
            {
                return false;
            }
            return true;
        }
        public void SetSession(string obj)
        {
            Session["LoginSession"] = obj;
        }

        public void EmptySession()
        {
            Session["LoginSession"] = null;
            Session["Admin"] = null;
        }

        public void AdminSession(string Username)
        {
            if((accDB.accounts.FirstOrDefault(a => a.username == Username && a.admin == 1)) != null)
            {
                Session["Admin"] = Username;
            }
        }

        public bool ActiveStatus(string Username)
        {
            account acc = accDB.accounts.FirstOrDefault(p => p.username == Username);
            if(acc != null)
            {
                if(acc.active == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        //Mã hoá MD5 
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