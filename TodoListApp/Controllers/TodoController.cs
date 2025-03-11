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
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS TodoItems (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        IsCompleted INTEGER NOT NULL
                    )";
                command.ExecuteNonQuery();
            }
        }

        public IActionResult Index()
        {
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
            return View(items);
        }

        [HttpPost]
        public IActionResult Add(TodoItem item)
        {
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
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM TodoItems WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Toggle(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE TodoItems SET IsCompleted = NOT IsCompleted WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}