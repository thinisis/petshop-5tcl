namespace Pet_5TCL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order()
        {
            orders_item = new HashSet<orders_item>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        [StringLength(256)]
        public string madon { get; set; }

        [Required]
        [StringLength(256)]
        public string username { get; set; }

        [Required]
        [StringLength(256)]
        public string address { get; set; }

        public decimal tongtien { get; set; }

        public DateTime ngaytao { get; set; }

        public int thanhtoan { get; set; }

        public int giaohang { get; set; }

        public int hoanthanh { get; set; }

        public virtual account account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orders_item> orders_item { get; set; }
    }
}
