using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public override string? ToString()
        {
            return Name;
        }
    }
}
