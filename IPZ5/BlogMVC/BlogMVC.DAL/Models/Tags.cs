using System.ComponentModel.DataAnnotations.Schema;

namespace BlogMVC.DAL.Models
{
    public class Tags
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
