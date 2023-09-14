using System.IO;

namespace Glasssix.Contrib.Expect
{
    class ProcessAdapter : IProcess
    {
        private System.Diagnostics.Process process;

        public ProcessAdapter(System.Diagnostics.Process process)
        {
            this.process = process;
        }

        public System.Diagnostics.ProcessStartInfo StartInfo
        {
            get { return process.StartInfo; }
        }

        public StreamReader StandardOutput
        {
            get { return process.StandardOutput; }
        }

        public StreamReader StandardError
        {
            get { return process.StandardError; }
        }

        public StreamWriter StandardInput
        {
            get { return process.StandardInput; }
        }

        public void Start()
        {
            process.Start();
        }
    }
}
