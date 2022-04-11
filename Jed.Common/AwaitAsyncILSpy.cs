using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jed.Common
{
    public class AwaitAsyncILSpy
    {
        public static void Show()
        {
            Console.WriteLine($"Start {Thread.CurrentThread.ManagedThreadId:00}");
            Async();
            Console.WriteLine($"aaa2 {Thread.CurrentThread.ManagedThreadId:00}");
        }
        public static async void Async()
        {
            Console.WriteLine($"ddd5 {Thread.CurrentThread.ManagedThreadId:00}");

            await Task.Run(() =>
            {
                Thread.Sleep(500); 
                Console.WriteLine($"bbb3 {Thread.CurrentThread.ManagedThreadId:00}");
            });

            Console.WriteLine($"ccc4 {Thread.CurrentThread.ManagedThreadId:00}");
        }
    }
}
