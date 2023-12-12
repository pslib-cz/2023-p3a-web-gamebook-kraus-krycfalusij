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
                var newState = new T();
                Save(key, newState);
                return newState;
            }

            return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }

        public void Save(string key, T value)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var serializedValue = System.Text.Json.JsonSerializer.Serialize(value);
            session.SetString(key, serializedValue);
        }
    }


}
