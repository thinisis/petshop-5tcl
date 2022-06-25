namespace Pet_5TCL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public account()
        {
            orders = new HashSet<order>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        [StringLength(256)]
        public string username { get; set; }

        [Required]
        [StringLength(256)]
        public string password { get; set; }

        [Required]
        [StringLength(256)]
        public string email { get; set; }

        [Required]
        [StringLength(256)]
        public string phone { get; set; }

        public int admin { get; set; }

        public int active { get; set; }

        public DateTime created { get; set; }

        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> orders { get; set; }
    }
}
