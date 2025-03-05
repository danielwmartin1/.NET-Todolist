using Postgrest.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models
{
    [Table("todo_items")]
    public class TodoItem
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public required string Title { get; set; }

        [Column("is_completed")]
        public bool IsCompleted { get; set; }
    }
}