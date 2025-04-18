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
            return View(items);
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
    }
}