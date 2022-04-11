using Jed.Common;
using System;

namespace Console_AwaitAsync
{
    /// <summary>
    /// 一、await/async 語法和使用
    /// 二、反編譯 IL 解讀和狀態機模式
    /// 三、使用 awit/async 的建議
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                {
                    //new AwaitAsyncClass().Show();
                }
                {
                    AwaitAsyncILSpy.Show();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }
    }
}
