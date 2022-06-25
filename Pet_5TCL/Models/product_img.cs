namespace Pet_5TCL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class product_img
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product_img()
        {
            orders_item = new HashSet<orders_item>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int id { get; set; }

        [Key]
        [StringLength(256)]
        [Required]
        public string masp { get; set; }

        [Required]
        [StringLength(256)]
        public string tensp { get; set; }

        [Required]
        [StringLength(256)]
        public string maloai { get; set; }
        [Required]
        public int soluong { get; set; }
        [Required]
        public DateTime ngaycapnhat { get; set; }
        [Required]
        public decimal dongia { get; set; }

        [StringLength(256)]
        [Required]
        public string mota { get; set; }
        [Required]
        public int active { get; set; }
        [Required]
        public int saleactive { get; set; }
        [Required]
        public int sale { get; set; }

        [StringLength(256)]
        public string image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orders_item> orders_item { get; set; }

        public virtual products_type products_type { get; set; }
        [NotMapped]
        public HttpPostedFileBase uploadFile { get; set; }


    }
}
