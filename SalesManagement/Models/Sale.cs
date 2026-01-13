using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesManagement.Models
{
    public partial class Sale
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("PRODUCT_ID")]
        public int? ProductId { get; set; }
        [Column("QUANTITY")]
        public double? Quantity { get; set; }
        [Column("SALESPRICE")]
        public double? Salesprice { get; set; }
        [Column("DATE", TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column("AMOUNT")]
        public double? Amount { get; set; }
        [Column("CUSTOMER_ID")]
        public int? CustomerId { get; set; }
        [Column("LISTPRICE")]
        public double? Listprice { get; set; }
        [Column("DISCOUNTRATE")]
        public double? Discountrate { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Sales")]
        public virtual Customer? Customer { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Sales")]
        public virtual Product? Product { get; set; }
    }
}
