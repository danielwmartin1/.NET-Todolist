using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace TodoListApp.Controllers
{
    public class TodoController : Controller
    {
        private static List<TodoItem> _todoItems = new List<TodoItem>();

        public IActionResult Index()
        {
            IEnumerable<TodoItem> model = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Task 1", IsCompleted = false },
                new TodoItem { Id = 2, Title = "Task 2", IsCompleted = true }
            };
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                item.Id = _todoItems.Count + 1;
                _todoItems.Add(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _todoItems.FirstOrDefault(t => t.Id == item.Id);
                if (existingItem != null)
                {
                    existingItem.Title = item.Title;
                    existingItem.Description = item.Description;
                    existingItem.IsCompleted = item.IsCompleted;
                    existingItem.DueDate = item.DueDate;
                }
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult Delete(int id)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _todoItems.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                _todoItems.Remove(item);
            }
            return RedirectToAction("Index");
        }
    }
}