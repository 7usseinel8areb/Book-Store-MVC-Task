using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBook.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [NotMapped]
        public IFormFile ImageFile { get; set; } = default!;
        public string Image {  get; set; }= string.Empty;
    }
}
