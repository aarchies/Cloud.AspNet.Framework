using System.Threading.Tasks;

namespace Glasssix.Contrib.Expect
{
    public interface ISpawnable
    {
        void Init();

        void Write(string command);

        string Read();

        Task<string> ReadAsync();
    }
}
