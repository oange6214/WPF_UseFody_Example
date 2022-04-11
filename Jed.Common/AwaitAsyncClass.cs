using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jed.Common
{
    /// <summary>
    /// await/async：出現在 C#5.0（.NetFramework 在 4.5 以上）
    ///              它是一個語法糖，不是一個全新的非同步多執行緒使用方式
    ///              （語法糖：就是編譯器提供的新功能）
    ///              本身並不會產生新的執行緒，但是依託於 Task 而存在，所以程序執行時，也是有多執行緒的
    ///              
    /// async 可以隨意添加，可以不用 await
    /// await 只能出現在 task 前面，但是方法必須聲明 async，不能單獨出現
    /// 
    /// await/async 之後，原本沒有返回值的，可以返回 Task
    ///                  原本返回 T 類型的，可以返回 Task<T>
    ///                  一般來說，盡量不要用 返回 void，因為不能用 await
    ///                  
    /// TASK 要練熟         
    /// 1、語法熟捻
    /// 2、執行順序要輕楚
    /// 3、執行緒 ID
    /// </summary>
    public class AwaitAsyncClass
    {
        public void Show()
        {
            Console.WriteLine($"This Main Start {Thread.CurrentThread.ManagedThreadId}");
            {
                //NormalNoReturn();
            }
            {
                //NoReturnAsync();
            }
            {
                //ReturnTaskAsync();
            }
            {
                //ReturnLongAsync();
            }
            {
                ReturnTask2Async();
            }

            Console.WriteLine($"This Main End {Thread.CurrentThread.ManagedThreadId}"); // 多執行緒併發的方式
        }

        /// <summary>
        /// 沒有返回值，在方法裡面開啟一個 Task 執行緒
        /// </summary>
        public void NormalNoReturn()
        {
            Console.WriteLine($"This NormalNoReturn Start {Thread.CurrentThread.ManagedThreadId}");
            Task task = Task.Run(() =>
            {
                Console.WriteLine($"This NormalNoReturn Task Start {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"This NormalNoReturn Task End {Thread.CurrentThread.ManagedThreadId}");
            });

            task.ContinueWith(t => Console.WriteLine($"This NormalNoReturn End {Thread.CurrentThread.ManagedThreadId}"));
        }

        /// <summary>
        /// 沒有返回值---在方法裡面開啟了一個 Task 執行緒
        /// </summary>
        public async void NoReturnAsync()
        {
            Console.WriteLine($"This NoReturnAsync Start {Thread.CurrentThread.ManagedThreadId}");   // 調用執行緒 執行
            Task task = Task.Run(() =>
            {
                Console.WriteLine($"This NoReturnAsync Task Start {Thread.CurrentThread.ManagedThreadId}");  // Task 的子執行緒完成
                Thread.Sleep(2000);
                Console.WriteLine($"This NoReturnAsync Task End {Thread.CurrentThread.ManagedThreadId}");    // Task 的子執行緒完成
            });             // 調用執行緒發起，啟動新執行緒執行內部動作
            await task;     // 調用執行緒回去忙自己的事情
            Console.WriteLine($"This NoReturnAsync End {Thread.CurrentThread.ManagedThreadId}");     // Task 的子執行緒完成（如果沒有 await 應該會是調用線程執行）
        }

        public async Task ReturnTaskAsync()
        {
            Console.WriteLine($"This ReturnTaskAsync Start {Thread.CurrentThread.ManagedThreadId}");
            Task task = Task.Run(() =>
            {
                Console.WriteLine($"This ReturnTaskAsync Task Start {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"This ReturnTaskAsync Task End {Thread.CurrentThread.ManagedThreadId}");
            });
            await task;
            Console.WriteLine($"This ReturnTaskAsync End {Thread.CurrentThread.ManagedThreadId}");

            // 可以認為，加了 await 就等同於將 await 後面的程式碼，包裝成一個回調---其實回調的執行緒具備多種可能性
        }

        public async Task ReturnTask2Async()
        {
            Console.WriteLine($"This ReturnTask2_1Async Start {Thread.CurrentThread.ManagedThreadId}");
            Task task = Task.Run(() =>
            {
                Console.WriteLine($"This ReturnTask2_1Async Task Start {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"This ReturnTask2_1Async Task End {Thread.CurrentThread.ManagedThreadId}");
            });
            await task;
            Console.WriteLine($"This ReturnTask2_1Async End {Thread.CurrentThread.ManagedThreadId}");

            // 可以認為，加了 await 就等同於將 await 後面的程式碼，包裝成一個回調---其實回調的執行緒具備多種可能性

            Console.WriteLine($"This ReturnTask2_2Async Start {Thread.CurrentThread.ManagedThreadId}");

            await Task.Run(() =>
            {
                Console.WriteLine($"This ReturnTask2_2Async Task Start {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"This ReturnTask2_2Async Task End {Thread.CurrentThread.ManagedThreadId}");
            });

            Console.WriteLine($"This ReturnTask2_2Async End {Thread.CurrentThread.ManagedThreadId}");

            // 如果有多層的 await，這個執行順序，就會明白了

            Console.WriteLine($"This ReturnTask2_3Async Start {Thread.CurrentThread.ManagedThreadId}");

            await Task.Run(() =>
            {
                Console.WriteLine($"This ReturnTask2_3Async Task Start {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"This ReturnTask2_3Async Task End {Thread.CurrentThread.ManagedThreadId}");
            });

            Console.WriteLine($"This ReturnTask2_3Async End {Thread.CurrentThread.ManagedThreadId}");
            // 可以同步編寫的形式去寫非同步
        }



        public async Task<long> ReturnLongAsync()
        {
            Console.WriteLine($"This ReturnLongAsync Start {Thread.CurrentThread.ManagedThreadId}");

            long result = 0;
            await Task.Run(() =>
            {
                Console.WriteLine($"This ReturnLongAsync Task Start {Thread.CurrentThread.ManagedThreadId}");

                for(int i = 0; i < 1000000000; i++)
                {
                    result += i;
                }

                Console.WriteLine($"This ReturnLongAsync Task End {Thread.CurrentThread.ManagedThreadId}");

                return result;
            });

            Console.WriteLine($"This ReturnLongAsync End {Thread.CurrentThread.ManagedThreadId}");
            return result;
        }
    }
}
