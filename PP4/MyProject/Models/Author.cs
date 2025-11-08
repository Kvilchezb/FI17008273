using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        public string AuthorName { get; set; }

        public ICollection<Title> Titles { get; set; }
    }
}
