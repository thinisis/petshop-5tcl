using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Pet_5TCL.Models;
using PagedList;
using System.IO;

namespace Pet_5TCL.Controllers
{
    public class AdminController : Controller
    {
        DataContext accDB = new DataContext();
        DataContext dataDB = new DataContext();
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
                    ViewBag.OrderCount = GetOrderCount();
                    ViewBag.UserCount = GetUserCount();
                    ViewBag.ProductCount = GetProductCount();
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
            if(acc.password != collection["password"] && collection["password"] != "")
            {
                acc.password = GetMD5(collection["password"]);
            }
            acc.email = collection["email"];
            acc.phone = collection["phone"];
            acc.admin = int.Parse(collection["admin"]);
            acc.name = collection["name"];
            acc.active = int.Parse(collection["active"]);
            accDB.SaveChanges();
            return RedirectToAction("AccountManager", "Admin");
        }

        public ActionResult ListProduct(int ?page)
        {
            if (Session["Admin"] != null)
            {
                if (page == null) page = 1;
                List<product> lstproducts = dataDB.products.ToList();
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(lstproducts.ToPagedList(pageNum, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ListCategory(int? page)
        {
            if (Session["Admin"] != null)
            {
                if (page == null) page = 1;
                List<products_type> lstproducts_type = dataDB.products_type.ToList();
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(lstproducts_type.ToPagedList(pageNum, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ListOrder(int ?page)
        {
            if (Session["Admin"] != null)
            {
                if (page == null) page = 1;
                List<order> lstorder = dataDB.orders.ToList();
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(lstorder.ToPagedList(pageNum, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(products_type prdt)
        {
            if(prdt != null)
            {
                try
                {
                    dataDB.products_type.Add(prdt);
                    dataDB.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
            return RedirectToAction("ListCategory", "Admin");
        }



        [NonAction]
        public SelectList ToSelectList(List<products_type> lstprdt)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            for(int i = 0; i < lstprdt.Count; i++)
            {
                list.Add(new SelectListItem()
                {
                    Text = lstprdt[i].tenloai.ToString(),
                    Value = lstprdt[i].maloai.ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }


        [HttpGet]
        public ActionResult NewProduct()
        {

            if (Session["Admin"] != null)
            {
                List<products_type> lstprdt = dataDB.products_type.ToList();
                ViewBag.ListPrDType = ToSelectList(lstprdt);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult NewCategory()
        {

            if (Session["Admin"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult NewCategory(products_type product)
        { 
                if(product != null)
            {
                dataDB.products_type.Add(product);
                dataDB.SaveChanges();
                ViewBag.msgSuccess = "Thao tác thành công!";
                return RedirectToAction("ListCategory","Admin");
            }
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult NewProduct(product_img product)
        {
            if (Session["Admin"] != null)
            {
                product prd = new product();
                prd.ngaycapnhat = DateTime.Now;
                prd.tensp = product.tensp;
                prd.masp = product.masp;
                prd.mota = product.mota;
                prd.maloai = product.products_type.maloai;
                prd.active = product.active;
                prd.sale = product.sale;
                prd.saleactive = product.saleactive;
                prd.dongia = product.dongia;
                prd.soluong = product.soluong;
                try
                {
                    if (product.uploadFile != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/images"), product.uploadFile.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        else
                        {
                            prd.image = "Content/images/" + product.uploadFile.FileName;
                            product.uploadFile.SaveAs(path);
                        } 
                    }
                    dataDB.products.Add(prd);
                    dataDB.SaveChanges();
                    return RedirectToAction("ListProduct");
                }
                catch (Exception ex)
                {
                    ViewBag.MsgFail = ex.Message;
                    return ListProduct(null);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ProductDelete(string id)
        {
            product pd = dataDB.products.FirstOrDefault(p => p.masp == id);
            if(pd != null)
            {
                dataDB.products.Remove(pd);
                dataDB.SaveChanges();
                ViewBag.msgSuccess = "Thao tác thành công";
                return RedirectToAction("ListProduct", "Admin");
            }
            ViewBag.msgFail = "Thao tác thất bại";
            return RedirectToAction("ListProduct", "Admin");

        }

        public ActionResult OrderDelete(string id)
        {
            order pd = dataDB.orders.FirstOrDefault(p => p.madon == id);
            if (pd != null)
            {
                dataDB.orders.Remove(pd);
                dataDB.SaveChanges();
                ViewBag.msgSuccess = "Thao tác thành công";
                return RedirectToAction("ListOrder", "Admin");
            }
            ViewBag.msgFail = "Thao tác thất bại";
            return RedirectToAction("ListOrder", "Admin");

        }

        public ActionResult ProductEdit(string id)
        {
            if (Session["Admin"] != null)
            {
                product pr = dataDB.products.FirstOrDefault(p => p.masp == id);
                if (pr == null)
                {
                    return RedirectToAction("ProductEdit", "Admin");
                }
                List<products_type> lstprdt = dataDB.products_type.ToList();
                ViewBag.ListPrDType = ToSelectList(lstprdt);
                return View(pr);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult ProductEdit(product_img product)
        {
            product prd = dataDB.products.FirstOrDefault(p => p.masp == product.masp);
            prd.ngaycapnhat = DateTime.Now;
            prd.tensp = product.tensp;
            prd.mota = product.mota;
            prd.maloai = product.products_type.maloai;
            prd.active = product.active;
            prd.sale = product.sale;
            prd.saleactive = product.saleactive;
            prd.dongia = product.dongia;
            prd.soluong = product.soluong;
            try
            {
                if (product.uploadFile != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/images"), product.uploadFile.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    else
                    {
                        prd.image = "Content/images/" + product.uploadFile.FileName;
                        product.uploadFile.SaveAs(path);
                    }
                }
                dataDB.SaveChanges();
                return RedirectToAction("ListProduct", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.MsgFail = ex.Message;
                return RedirectToAction("ListProduct", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CategoryDetele(string id)
        {
            if (Session["Admin"] != null)
            {
                products_type pdt = dataDB.products_type.FirstOrDefault(p => p.maloai == id);
                if (pdt != null)
                {
                    List<product> lstproducts = dataDB.products.Where(p => p.maloai == pdt.maloai).ToList();
                    for (int i = 0; i < lstproducts.Count; i++)
                    {
                        dataDB.products.Remove(lstproducts[i]);
                    }
                    dataDB.products_type.Remove(pdt);
                    dataDB.SaveChanges();
                    ViewBag.msgSuccess = "Thao tác thành công";
                    return RedirectToAction("ListCategory", "Admin");
                }
            }
            ViewBag.msgFail = "Thao tác thất bại";
            return RedirectToAction("ListCategory", "Admin");

        }

        public ActionResult CategoryEdit(string id)
        {
            if (Session["Admin"] != null)
            {
                if (Session["Admin"] != null)
                {
                    products_type pdt = dataDB.products_type.FirstOrDefault(p => p.maloai == id);
                    if (pdt == null)
                    {
                        return RedirectToAction("ListCategory", "Admin");
                    }
                    return View(pdt);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult CategoryEdit(products_type pdt)
        {
            if (Session["Admin"] != null)
            {
                products_type pdte = dataDB.products_type.FirstOrDefault(p => p.maloai == pdt.maloai);
                try
                {
                    pdte.tenloai = pdt.tenloai;
                    dataDB.SaveChanges();
                    ViewBag.msgSuccess = "Thao tác thành công";
                    return RedirectToAction("ListCategory", "Admin");
                }
                catch (Exception ex)
                {
                    ViewBag.MsgFail = ex.Message;
                    return ListCategory(null);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public int GetProductCount()
        {
            return dataDB.products.Count();
        }

        public int GetOrderCount()
        {
            return dataDB.orders.Count();
        }

        public int GetUserCount()
        {
            return accDB.accounts.Count();
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