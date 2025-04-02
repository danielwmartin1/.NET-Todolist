using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using TodoListApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace TodoListApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly string connectionString = "Data Source=todo.db";
        private static List<TodoItem> _todoItems = new List<TodoItem>();

        public TodoController()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Database connection opened.");
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS TodoItems (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        IsCompleted INTEGER NOT NULL
                    )";
                command.ExecuteNonQuery();
                Console.WriteLine("Ensured TodoItems table exists.");
            }
        }

        public IActionResult Index()
        {
            Console.WriteLine("Fetching all TodoItems...");
            List<TodoItem> items = new List<TodoItem>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Title, IsCompleted FROM TodoItems";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new TodoItem
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            IsCompleted = reader.GetBoolean(2)
                        });
                    }
                }
            }
            Console.WriteLine($"Fetched {items.Count} TodoItems.");

            // Check if the request is for JSON data
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Json(items); // Return JSON data
            }

            return View(items); // Return the view for regular requests
        }

        [HttpPost]
        public IActionResult Add(TodoItem item)
        {
            Console.WriteLine($"Adding new TodoItem: Title={item.Title}, IsCompleted={item.IsCompleted}");
            if (ModelState.IsValid)
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO TodoItems (Title, IsCompleted) VALUES (@Title, @IsCompleted)";
                    command.Parameters.AddWithValue("@Title", item.Title);
                    command.Parameters.AddWithValue("@IsCompleted", item.IsCompleted);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("TodoItem added successfully.");
            }
            else
            {
                Console.WriteLine("ModelState is invalid. TodoItem not added.");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Console.WriteLine($"Deleting TodoItem with Id={id}");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM TodoItems WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("TodoItem deleted successfully.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Toggle(int id)
        {
            Console.WriteLine($"Toggling completion status for TodoItem with Id={id}");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE TodoItems SET IsCompleted = NOT IsCompleted WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("TodoItem completion status toggled successfully.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Todo/Edit")]
        public IActionResult Edit(int id, string title)
        {
            Console.WriteLine($"Editing TodoItem with Id={id}, New Title={title}");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE TodoItems SET Title = @Title WHERE Id = @Id";
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("TodoItem updated successfully.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Todo/StartEdit")]
        public IActionResult StartEdit(int id)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                item.IsEditing = true;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, string title)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                item.Title = title;
                item.IsEditing = false;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Todo/CancelEdit")]
        public IActionResult CancelEdit(int id)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                item.IsEditing = false;
            }
            return RedirectToAction("Index");
        }
    }
}