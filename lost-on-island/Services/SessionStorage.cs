using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace lost_on_island.Services
{
    public class SessionStorage<T> : ISessionStorage<T> where T : new()
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionStorage(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T LoadOrCreate(string key)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var value = session.GetString(key);

            if (string.IsNullOrEmpty(value))
            {
                return new T();
            }
            Console.WriteLine(value);
            return JsonSerializer.Deserialize<T>(value);
        }

        public void Save(string key, T value)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var serializedValue = JsonSerializer.Serialize(value);
            session.SetString(key, serializedValue);
        }

       
    }
}
