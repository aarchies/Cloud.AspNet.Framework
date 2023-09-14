namespace Glasssix.Contrib.Data
{
    public interface IGlasssixFactory<out TService> where TService : class
    {
        TService Create();

        TService Create(string name);
    }
}