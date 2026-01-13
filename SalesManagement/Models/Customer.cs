using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesManagement.Models
{
    [Table("Customer")]
    public partial class Customer
    {
        public Customer()
        {
            Purchases = new HashSet<Purchase>();
            Sales = new HashSet<Sale>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("CUSTOMERTITLE")]
        [StringLength(50)]
        public string? Customertitle { get; set; }
        [Column("CUSTOMERNUMBER")]
        [StringLength(50)]
        public string? Customernumber { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<Purchase> Purchases { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
