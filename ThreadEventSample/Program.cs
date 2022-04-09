using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadEventSample
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var pub = new Publisher();
            var sub1 = new Subscriber("sub1", pub);
            var sub2 = new Subscriber("sub2", pub);
            var sub3 = new Subscriber("sub3", pub);
            var sub4 = new Subscriber("sub4", pub);
            var sub5 = new Subscriber("sub5", pub);

            // 呼叫方法 喚起事件
            Task.Run(() =>
            {
                pub.DoSomething();
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
    }

    /// <summary>
    /// 發佈事件類（屬於發佈者）
    /// </summary>
    public class Publisher
    {
        public event EventHandler<string> RaiseCustomEvent;

        public void DoSomething()
        {
            OnRaiseCustomEvent("Event triggered");

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

    /// <summary>
    /// 訂閱事件類（屬於訂閱者）
    /// </summary>
    public class Subscriber
    {
        private readonly string _sub;

        public Subscriber(string sub, Publisher pub)
        {
            _sub = sub;

            // 訂閱事件
            pub.RaiseCustomEvent += CustomEvent;
        }

        // 定義 引發事件時 要採取的動作
        public void CustomEvent(object sender, string e)
        {
            String msg = null;
            Thread thread = Thread.CurrentThread;

            msg = String.Format("[{0}] - {1} Thread information\n", _sub, e) +
                    String.Format("   Background: {0}\n", thread.IsBackground) +
                    String.Format("   Thread Pool: {0}\n", thread.IsThreadPoolThread) +
                    String.Format("   Thread ID: {0}\n", thread.ManagedThreadId);
            Console.WriteLine(msg);
        }
    }
}
