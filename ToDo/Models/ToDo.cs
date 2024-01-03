using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDo.Models
{
    public class ToDo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        public string? Desc { get; set; }
        public bool? Complete { get; set; } = false;
        [JsonIgnore]
        public int UserId { get; set; }
    }
}