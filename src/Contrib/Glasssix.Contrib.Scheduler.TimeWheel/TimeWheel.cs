using Glasssix.Contrib.Scheduler.TimeWheel.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Scheduler.TimeWheel
{
    public class TimeWheel
    {
        int minuteSlot, secondSlot = 0;
        DateTime wheelTime { get { return new DateTime(1, 1, 1, 0, minuteSlot, secondSlot); } }
        Dictionary<int, ConcurrentQueue<WheelTask>> hoursTaskQueue ,minuteTaskQueue, secondTaskQueue;
        public void Start()
        {
            new Timer(Callback, null, 0, 1000);
            hoursTaskQueue = new Dictionary<int, ConcurrentQueue<WheelTask>>();
            minuteTaskQueue = new Dictionary<int, ConcurrentQueue<WheelTask>>();
            secondTaskQueue = new Dictionary<int, ConcurrentQueue<WheelTask>>();
            Enumerable.Range(0, 60).ToList().ForEach(x =>
            {
                hoursTaskQueue.Add(x, new ConcurrentQueue<WheelTask>());
                minuteTaskQueue.Add(x, new ConcurrentQueue<WheelTask>());
                secondTaskQueue.Add(x, new ConcurrentQueue<WheelTask>());
            });
        }


        public async Task AddTaskAsync(double hours, double minute, double second, Func<Task> handler)
        {
            Console.WriteLine("添加任务");
            var handTime = wheelTime.AddHours(hours).AddMinutes(minute).AddSeconds(second);
            if (handTime.Minute != wheelTime.Minute)
                minuteTaskQueue[handTime.Minute].Enqueue(new WheelTask(handTime.Second, handler));
            else
            {
                if (handTime.Second != wheelTime.Second)
                    secondTaskQueue[handTime.Second].Enqueue(new WheelTask(handler));
                else
                {
                    await handler();
                }
            }
        }


        async void Callback(object o)
        {
             Console.WriteLine("回调");
            bool minuteExecuteTask = false;
            if (secondSlot != 59)
                secondSlot++;
            else
            {
                secondSlot = 0;
                minuteExecuteTask = true;
                if (minuteSlot != 59)
                    minuteSlot++;
                else
                {
                    minuteSlot = 0;
                }
            }
            if (minuteExecuteTask || secondTaskQueue[secondSlot].Any())
                await ExecuteTask(minuteExecuteTask);
        }

        async Task ExecuteTask(bool minuteExecuteTask)
        {
            if (minuteExecuteTask)
                while (minuteTaskQueue[minuteSlot].Any())
                    if (minuteTaskQueue[minuteSlot].TryDequeue(out WheelTask task))
                        secondTaskQueue[task.Second].Enqueue(task);
            if (secondTaskQueue[secondSlot].Any())
                while (secondTaskQueue[secondSlot].Any())
                    if (secondTaskQueue[secondSlot].TryDequeue(out WheelTask task))
                        await task.Handle();
        }
    }

}
