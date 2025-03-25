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
            Console.WriteLine("[DEBUG] Index action called. Fetching all TodoItems...");
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
            Console.WriteLine($"[DEBUG] Fetched {items.Count} TodoItems.");

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Json(items);
            }

            return View(items);
        }

        [HttpPost]
        public IActionResult Add(TodoItem item)
        {
            Console.WriteLine($"[DEBUG] Add action called. Title={item.Title}, IsCompleted={item.IsCompleted}");
            if (ModelState.IsValid)
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO TodoItems (Title, IsCompleted) VALUES (@Title, @IsCompleted)";
                    command.Parameters.AddWithValue("@Title", item.Title);
                    command.Parameters.AddWithValue("@IsCompleted", item.IsCompleted);
                    var rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"[DEBUG] TodoItem added successfully. Title={item.Title}");
                    }
                    else
                    {
                        Console.WriteLine($"[ERROR] Failed to add TodoItem. Title={item.Title}");
                    }
                }
            }
            else
            {
                Console.WriteLine("[ERROR] ModelState is invalid. TodoItem not added.");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Console.WriteLine($"[DEBUG] Delete action called. Id={id}");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM TodoItems WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"[DEBUG] TodoItem with Id={id} deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"[ERROR] TodoItem with Id={id} not found.");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Toggle(int id)
        {
            Console.WriteLine($"[DEBUG] Toggle action called. Id={id}");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE TodoItems SET IsCompleted = NOT IsCompleted WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"[DEBUG] TodoItem with Id={id} toggled successfully.");
                }
                else
                {
                    Console.WriteLine($"[ERROR] TodoItem with Id={id} not found.");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, string title)
        {
            Console.WriteLine($"[DEBUG] Update action called. Id={id}, Title={title}");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE TodoItems SET Title = @Title WHERE Id = @Id";
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Id", id);

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"[DEBUG] TodoItem with Id={id} updated successfully.");
                }
                else
                {
                    Console.WriteLine($"[ERROR] TodoItem with Id={id} not found or update failed.");
                }
            }
            return RedirectToAction("Index");
        }
    }
}