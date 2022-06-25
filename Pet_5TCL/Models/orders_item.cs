namespace Pet_5TCL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class orders_item
    {
        public int id { get; set; }

        [Required]
        [StringLength(256)]
        public string madon { get; set; }

        [Required]
        [StringLength(256)]
        public string masp { get; set; }

        public int soluong { get; set; }

        public decimal dongia { get; set; }

        public virtual order order { get; set; }

        public virtual product product { get; set; }
    }
}
