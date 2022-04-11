using System;
using System.Threading;
using System.Threading.Tasks;

namespace WPF_Async_SampleCode
{
    /// <summary>
    /// 普通類
    /// </summary>
    public class TestLock
    {
        public static readonly object TestLock_LOCK = new object();    // 靜態 LOCK
        private readonly object TestLock_LOCKTemp = new object();      // LOCK
        private readonly string TestLock_LOCKString = "努力向上";      // LOCK

        private int _num = 0;


        /// <summary>
        /// 靜態方法
        /// </summary>
        public static void Show()
        {
            for (int i = 0; i < 5; i++)
            {
                int j = i;

                Task.Run(() =>
                {
                    lock (TestLock_LOCK)
                    {
                        Console.WriteLine($"This is {i} {j} Show Start... {Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"This is {i} {j} Show End... {Thread.CurrentThread.ManagedThreadId}");
                    }
                });
            }
        }

        /// <summary>
        /// 普通方法
        /// </summary>
        public void ShowTemp(int index)
        {
            for (int i = 0; i < 5; i++)
            {
                int j = i;

                Task.Run(() =>
                {
                    lock (TestLock_LOCKTemp)
                    {
                        Console.WriteLine($"This is  {i} {j} ShowTemp {index} Start... {Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"This is  {i} {j} ShowTemp {index} End... {Thread.CurrentThread.ManagedThreadId}");
                    }
                });
            }
        }

        /// <summary>
        /// 普通方法，LOCK 為 string 類型
        /// </summary>
        public void ShowString(int index)
        {
            for (int i = 0; i < 5; i++)
            {
                int j = i;

                Task.Run(() =>
                {
                    lock (TestLock_LOCKString)
                    {
                        Console.WriteLine($"This is  {i} {j} ShowString {index} Start... {Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"This is  {i} {j} ShowString {index} End... {Thread.CurrentThread.ManagedThreadId}");
                    }
                });
            }
        }

        /// <summary>
        /// 普通方法，LOCK 為 this （實例自己）
        /// </summary>
        /// <param name="index"></param>
        public void ShowThis(int index)
        {
            for (int i = 0; i < 5; i++)
            {
                int j = i;

                Task.Run(() =>
                {
                    lock (this) // this 是當前實例
                    {
                        Console.WriteLine($"This is  {i} {j} ShowThis {index} Start... {Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"This is  {i} {j} ShowThis {index} End... {Thread.CurrentThread.ManagedThreadId}");
                    }
                });
            }
        }

        public void ShowThisAnother(int index)
        {
            for (int i = 0; i < 5; i++)
            {
                _num++;
                int j = i;
                lock (this) // this 是當前實例
                {
                    Console.WriteLine($"This is  {i} {j} ShowThisAnother {index} Start... {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"This is  {i} {j} ShowThisAnother {index} End... {Thread.CurrentThread.ManagedThreadId}");

                    if(_num < 5)
                    {
                        ShowThisAnother(index);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

    }

    /// <summary>
    /// 泛型類
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TestLockGeneric<T>
    {
        public static readonly object TestLock_LOCK = new object();    // 靜態 LOCK

        public static void Show(int index)
        {
            for (int i = 0; i < 5; i++)
            {
                int j = i;

                Task.Run(() =>
                {
                    lock (TestLock_LOCK)
                    {
                        Console.WriteLine($"This is {i} {j} TestLockGeneric.Show {index} Start... {Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"This is {i} {j} TestLockGeneric.Show {index} End... {Thread.CurrentThread.ManagedThreadId}");
                    }
                });
            }
        }
    }
}
