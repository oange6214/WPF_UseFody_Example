using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadEventNotPS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pub = new Publisher();

            string[] subs = new string[]
            {
                "sub1",
                "sub2",
                "sub3",
                "sub4",
                "sub5"
            };

            pub.RaiseCustomEvent += CustomEvent;

            // 呼叫方法 喚起事件
            Task.Run(() =>
            {
                for(int i = 0; i < subs.Length; i++)
                {
                    pub.DoSomething(i.ToString(), subs[i]); ;
                }
            });


            Thread thread = Thread.CurrentThread;
            string msg = null;

            msg = String.Format("MAIN Thread information\n") +
                String.Format("   Background: {0}\n", thread.IsBackground) +
                String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
                String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            Console.WriteLine(msg);

            Console.ReadLine();
        }
        // 定義 引發事件時 要採取的動作
        public static void CustomEvent(object sender, string e)
        {
            String msg = null;
            Thread thread = Thread.CurrentThread;

            msg = String.Format("{0} in Main Thread information\n",  e) +
                    String.Format("   Background: {0}\n", thread.IsBackground) +
                    String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
                    String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            Console.WriteLine(msg);
        }
    }


    /// <summary>
    /// 發佈事件類（屬於發佈者）
    /// </summary>
    public class Publisher
    {
        public event EventHandler<string> RaiseCustomEvent;

        public void DoSomething(string i, string sub)
        {
            Task.Run(() =>
            {
                OnRaiseCustomEvent($"[{i}]{sub} - Event triggered");
            });

            Thread thread = Thread.CurrentThread;
            string msg = null;

            msg = String.Format("Publisher Thread information\n") +
                String.Format("   Background: {0}\n", thread.IsBackground) +
                String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
                String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            Console.WriteLine(msg);
        }

        protected virtual void OnRaiseCustomEvent(string e)
        {
            EventHandler<string> raiseEvent = RaiseCustomEvent;

            try
            {
                raiseEvent?.Invoke(this, e + $" at {DateTime.Now}");
                Console.WriteLine("Try-Catch\n\n");
            }
            catch (Exception ex)
            {

            }


            Thread thread = Thread.CurrentThread;
            string msg = null;

            msg = String.Format("OnRaiseCustomEvent Method Thread information\n") +
                String.Format("   Background: {0}\n", thread.IsBackground) +
                String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
                String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            Console.WriteLine(msg);
        }
    }
}
