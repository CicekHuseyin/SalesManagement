using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesManagement.Models
{
    [Table("Purchase")]
    public partial class Purchase
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("PRODUCT_ID")]
        public int? ProductId { get; set; }
        [Column("QUANTITY")]
        public double? Quantity { get; set; }
        [Column("PRICE")]
        public double? Price { get; set; }
        [Column("AMOUNT")]
        public double? Amount { get; set; }
        [Column("DATE", TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column("CUSTOMER_ID")]
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Purchases")]
        public virtual Customer? Customer { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Purchases")]
        public virtual Product? Product { get; set; }
    }
}
