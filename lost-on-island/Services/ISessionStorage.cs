namespace lost_on_island.Services
{
    public interface ISessionStorage<T>
    {
        T LoadOrCreate(string key);
        void Save(string key, T value);
    }

}
