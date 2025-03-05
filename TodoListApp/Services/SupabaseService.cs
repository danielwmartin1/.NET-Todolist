using Supabase;
using TodoListApp.Models;

namespace TodoListApp.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _client;

        public SupabaseService(IConfiguration configuration)
        {
            var url = configuration["Supabase:Url"];
            var apiKey = configuration["Supabase:ApiKey"];
            _client = new Supabase.Client(url, apiKey);
        }

        public async Task<List<TodoItem>> GetTodoItemsAsync()
        {
            var response = await _client.From<TodoItem>().Get();
            return response.Models;
        }

        public async Task<TodoItem> AddTodoItemAsync(TodoItem item)
        {
            var response = await _client.From<TodoItem>().Insert(item);
            return response.Models.FirstOrDefault();
        }

        public async Task<TodoItem> UpdateTodoItemAsync(TodoItem item)
        {
            var response = await _client.From<TodoItem>().Update(item);
            return response.Models.FirstOrDefault();
        }

        public async Task DeleteTodoItemAsync(int id)
        {
            await _client.From<TodoItem>().Delete().Match(new { Id = id });
        }
    }
}
