using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required]
        public string TagName { get; set; }

        public ICollection<TitleTag> TitleTags { get; set; }
    }
}
