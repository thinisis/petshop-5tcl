using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Pet_5TCL.Models;

namespace Pet_5TCL.Models
{
    public class Cart
    {
        DataContext myData = new DataContext();
        public string masp { get; set; }
        public string tensp { get; set; }
        public string image { get; set; }
        public int total { get; set; }
        public Double giaban { get; set; }
        public int giamgia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien
        {
            get { if (giamgia != 0) return ((iSoluong * giaban) - ((iSoluong * giaban)*(giamgia * 0.01))); else return (iSoluong * giaban); }
        }
        public Cart()
        { }
            public Cart(string id)
        {
            masp = id; 
            product prd = myData.products.Single(n => n.masp == masp); 
            tensp = prd.tensp; 
            image = prd.image;
            
            if(prd.saleactive == 1)
            {
                giamgia = prd.sale;
            }
            else
            {
                giamgia = 0;
            }
            total = prd.soluong;
            giaban = double.Parse(prd.dongia.ToString()); 
            iSoluong = 1;
        }
    }
}