using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace TodoListApp.Controllers
{
    public class TodoController : Controller
    {
        private static List<TodoItem> items = new List<TodoItem>();

        public IActionResult Index()
        {
            return View(items);
        }

        [HttpPost]
        public IActionResult Add(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                item.Id = items.Count > 0 ? items.Max(i => i.Id) + 1 : 1;
                items.Add(item);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                items.Remove(item);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Toggle(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.IsCompleted = !item.IsCompleted;
            }
            return RedirectToAction("Index");
        }
    }
}