using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesManagement.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            Purchases = new HashSet<Purchase>();
            Sales = new HashSet<Sale>();
            Stocks = new HashSet<Stock>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("CATEGORY_ID")]
        public int? CategoryId { get; set; }
        [Column("NAME")]
        [StringLength(150)]
        public string? Name { get; set; }
        [Column("IMAGE_SRC")]
        [StringLength(100)]
        public string? ImageSrc { get; set; }
        [Column("SALESPRICE")]
        public double? Salesprice { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Products")]
        public virtual Category? Category { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Purchase> Purchases { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Sale> Sales { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
