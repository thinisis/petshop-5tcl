namespace Pet_5TCL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product()
        {
            orders_item = new HashSet<orders_item>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        [StringLength(256)]
        public string masp { get; set; }

        [Required]
        [StringLength(256)]
        public string tensp { get; set; }

        [Required]
        [StringLength(256)]
        public string maloai { get; set; }

        public int soluong { get; set; }

        public DateTime ngaycapnhat { get; set; }

        public decimal dongia { get; set; }

        [StringLength(256)]
        public string mota { get; set; }

        public int active { get; set; }

        public int saleactive { get; set; }

        public int sale { get; set; }

        [StringLength(256)]
        public string image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orders_item> orders_item { get; set; }

        public virtual products_type products_type { get; set; }
    }
}
