using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace TodoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private static List<TodoItem> todoItems = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Test Todo 1", IsCompleted = false },
            new TodoItem { Id = 2, Title = "Test Todo 2", IsCompleted = true }
        };

        // GET: api/todo
        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetTodoItems()
        {
            return Ok(todoItems);
        }

        // GET: api/todo/{id}
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetTodoItem(int id)
        {
            var todoItem = todoItems.FirstOrDefault(item => item.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return Ok(todoItem);
        }

        // POST: api/todo
        [HttpPost]
        public ActionResult<TodoItem> CreateTodoItem(TodoItem todoItem)
        {
            todoItem.Id = todoItems.Count > 0 ? todoItems.Max(item => item.Id) + 1 : 1;
            todoItems.Add(todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // PUT: api/todo/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateTodoItem(int id, TodoItem updatedTodoItem)
        {
            var todoItem = todoItems.FirstOrDefault(item => item.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }
            todoItem.Title = updatedTodoItem.Title;
            todoItem.IsCompleted = updatedTodoItem.IsCompleted;
            return NoContent();
        }

        // DELETE: api/todo/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTodoItem(int id)
        {
            var todoItem = todoItems.FirstOrDefault(item => item.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }
            todoItems.Remove(todoItem);
            return NoContent();
        }
    }
}