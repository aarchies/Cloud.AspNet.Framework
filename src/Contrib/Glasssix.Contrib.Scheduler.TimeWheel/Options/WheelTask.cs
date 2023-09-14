using System;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Scheduler.TimeWheel.Options
{
    public class WheelTask
    {


        public WheelTask()
        {

        }

        public WheelTask(Func<Task> handle)
        {
            Handle = handle;
        }


        public WheelTask(int second, Func<Task> handle)
        {
            Second = second;
            Handle = handle;
        }

        //public T Data { get; set; }
        public Func<Task> Handle { get; set; }
        public int Second { get; set; }

    }
}