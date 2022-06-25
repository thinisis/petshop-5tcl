using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pet_5TCL.Models;

namespace Pet_5TCL.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        DataContext data = new DataContext();
        public List<Cart> GetCart()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                lstCart = new List<Cart>(); 
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }
        public ActionResult AddItem(string id, string strURL)
        {
            if (Session["LoginSession"] != null)
            {
                List<Cart> lstCart = GetCart();
                Cart sanpham = lstCart.Find(n => n.masp == id);
                if (sanpham == null)
                {
                    sanpham = new Cart(id);
                    lstCart.Add(sanpham);
                    Session["TongSanPham"] = TongSoLuong();
                    return Redirect(strURL);
                }
                else
                {
                    sanpham.iSoluong++;
                    Session["TongSanPham"] = TongSoLuong();
                    return Redirect(strURL);
                }
            }
            ViewBag.msgFail = "Bạn chưa đăng nhập";
            return RedirectToAction("Login", "Account");
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>; 
            if (lstCart != null)
            {
                tsl = lstCart.Sum(n => n.iSoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>; if (lstCart != null)
            {
                tsl = lstCart.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0; 
            List<Cart> lstCart = Session["Cart"] as List<Cart>; 
            if (lstCart != null)
            {
                tt = lstCart.Sum(n => n.dThanhtien);
            }
            return tt;
        }
        public ActionResult Cart()
        {
            if (Session["LoginSession"] != null)
            {
                List<Cart> lstCart = GetCart();
                Session["TongSanPham"] = TongSoLuong();
                ViewBag.TongTien = TongTien();
                return View(lstCart);
            }
            return RedirectToAction("Index", "Home");
            
        }
        public ActionResult CartPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham(); 
            return PartialView();
        }
        public ActionResult RemoveItem(string id)
        {
            List<Cart> lstCart = GetCart(); 
            Cart sanpham = lstCart.SingleOrDefault(n => n.masp == id); 
            if (sanpham != null)
            {
                lstCart.RemoveAll(n => n.masp == id);
                Session["TongSanPham"] = TongSoLuong();
                return RedirectToAction("Cart");
            }
            return RedirectToAction("Cart");
        }
        public ActionResult UpdateCart(string id, FormCollection collection)
        {
            List<Cart> lstCart = GetCart();
            Cart sanpham = lstCart.SingleOrDefault(n => n.masp == id);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["soluong"].ToString());
                Session["TongSanPham"] = TongSoLuongSanPham();

            }
            return RedirectToAction("Cart");
        }
        public ActionResult EmptyCart()
        {
            Session["TongSanPham"] = null;
            Session["Cart"] = null;
            List<Cart> lstCart = GetCart();
            lstCart.Clear(); 
            return RedirectToAction("Cart");
        }

        public ActionResult Checkout()
        {
            if (Session["LoginSession"] != null)
            {
                List<Cart> lstCart = GetCart();
                ViewBag.TongTien = TongTien();
                return View(lstCart);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Checkout(string address)
        {
                order order = new order();
                List<Cart> lstCart = GetCart();
                order.ngaytao = DateTime.Now;
                order.madon = DateTime.Now.ToString("ddMMyyyyHHmmss"+"DH");
                order.username = Session["LoginSession"].ToString();
                order.address = address;
                order.tongtien = (decimal)TongTien();
                order.thanhtoan = 0;
                order.giaohang = 0;
                order.hoanthanh = 0;
                data.orders.Add(order);
                for (int i = 0; i < lstCart.Count; i++)
                {
                    orders_item item = new orders_item();
                    item.madon = order.madon;
                    item.masp = lstCart[i].masp;
                    item.soluong = lstCart[i].iSoluong;
                    item.dongia = (decimal)lstCart[i].giaban;
                    data.orders_item.Add(item);
                    
                }
                data.SaveChangesAsync();
                EmptyCart();
            ViewBag.MaDon = order.madon;
            return RedirectToAction("Result","Cart");

        }

        public ActionResult Result()
        {
            
            return View();
        }
    }
}