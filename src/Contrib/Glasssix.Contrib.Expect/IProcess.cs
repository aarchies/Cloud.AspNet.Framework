using System.Diagnostics;
using System.IO;

namespace Glasssix.Contrib.Expect
{
    interface IProcess
    {
        ProcessStartInfo StartInfo { get; }
        StreamReader StandardOutput { get; }
        StreamReader StandardError { get; }
        StreamWriter StandardInput { get; }

        void Start();
    }
}
