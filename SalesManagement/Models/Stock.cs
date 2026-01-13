using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesManagement.Models
{
    [Table("Stock")]
    public partial class Stock
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("PRODUCT_ID")]
        public int? ProductId { get; set; }
        [Column("QUANTITY")]
        public double? Quantity { get; set; }
        [Column("DATE", TypeName = "datetime")]
        public DateTime? Date { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("Stocks")]
        public virtual Product? Product { get; set; }
    }
}
