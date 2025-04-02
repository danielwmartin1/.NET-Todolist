using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsEditing { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }
}