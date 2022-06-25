using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pet_5TCL.Models;
using PagedList;

namespace Pet_5TCL.Controllers
{
    public class HomeController : Controller
    {
        DataContext dataDB = new DataContext();
        public ActionResult Index()
        {
            @ViewData["Title"] = "Trang chủ";
            IEnumerable<product> prd = dataDB.products.ToList().Take(5);
            IEnumerable<product> lprd = dataDB.products.OrderByDescending(x => x.id);
            TempData["ListProduct"] = prd;
            TempData["LastProduct"] = lprd;
            return View();
        }

        public ActionResult Details(string id)
        {
            product prd = dataDB.products.FirstOrDefault(p => p.masp == id);
            if(prd == null)
            {
                return RedirectToAction("Index");
            }
            TempData["Product"] = prd;
            return View(prd);
        }

        public ActionResult Shop(int ?page)
        {
            if (page == null) page = 1;
            List<product> products = dataDB.products.ToList();
            ViewBag.ProductNumber = products.Count();
            int pageSize = 8;
            int pageNum = page ?? 1;
            return View(products.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Test()
        {

            return View();
        }
    }
}