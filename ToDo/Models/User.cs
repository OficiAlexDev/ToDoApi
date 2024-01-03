using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ToDo.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Email { get; set; }  
        [Required]
        public required string Password { get; set; } 
        [ForeignKey("UserId")]
        [JsonIgnore]
        public ICollection<ToDo>? ToDos { get; set; }
    }
}