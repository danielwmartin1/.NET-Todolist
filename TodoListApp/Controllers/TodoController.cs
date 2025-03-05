using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using TodoListApp.Services;

namespace TodoListApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly SupabaseService _supabaseService;

        public TodoController(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _supabaseService.GetTodoItemsAsync();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                await _supabaseService.AddTodoItemAsync(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                await _supabaseService.UpdateTodoItemAsync(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _supabaseService.DeleteTodoItemAsync(id);
            return RedirectToAction("Index");
        }
    }
}