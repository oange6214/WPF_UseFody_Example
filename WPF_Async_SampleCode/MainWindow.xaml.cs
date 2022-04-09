using System;
using System.Threading;
using System.Windows;

namespace WPF_Async_SampleCode
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 計算機概論：
        /// 處理序：程序在服務器上運行時，佔據的計算資源合集，稱之為處理緒
        ///        處理緒之煎不會相互干擾---處理緒之間的通訊比較困難（分佈式）
        /// 
        /// 執行緒：程序執行的最小單位，響應操作的最小執行流，
        ///        執行緒也包含自己的計算資源，
        ///        執行緒是屬於處理序的，一個處理緒可以有多個執行緒
        ///        
        /// 多執行緒：一個處理序裡面有多個執行緒並行處理。
        /// 
        /// c# 多執行緒 Thread 類：
        /// 一個封裝類，是 .NetFramework 對執行緒物件的抽象封裝
        /// 通過 Thread 去完成的操作，最終是透過作業系統請求，得到的執行流
        /// 
        /// CurrentThread：當前執行緒---任何操作執行都是執行緒完成，表示執行當前程序的執行緒
        /// ManagedThreadId：是 .Net 平台給 Thread 起的名字，是一個 int 整數值（盡量不重複）
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// 一、
        /// 同步單執行緒方法會卡 UI 界面-----主（UI）執行緒忙於計算，所以不能響應
        /// 非同步多執行緒方法不卡 UI 界面-----計算任務給子執行緒，主（UI）執行緒已經閒置，就可以嚮應操作
        /// CS 開發：按鈕後能不卡死---上傳文件不卡死
        /// BS 開發：用戶註冊、發信件 

        /// 二、
        /// 同步單執行緒方法慢---只有一個執行緒計算
        /// 非同步多執行緒方法快---有多個執行緒計算
        /// 
        /// 多執行緒就是資源換性能，並不是線性增加的
        /// A、多執行緒的協調管理額外成本
        /// B、資源也有上限的
        /// 執行緒並不是越多越好

        /// 三、無序性---不可預測性
        /// 啟動無序：幾乎同一時間向作業系統請求執行緒，也是需要 CPU 處理的請求
        ///          因為執行緒是作業系統資源，CLR 只能去申請，具體順序是無法掌控的
        /// 
        /// 執行時間不確定：同一個執行緒同一個任務耗時也可能不同
        ///                這與作業系統的調度策略有關系，CPU 分片（每秒分拆 1000 份，使其看起來有並行處理）
        ///                任務執行過程就看運氣---執行緒優先級可以影響作業系統的調度順序
        ///                
        /// 結束無序：以上狀況加起來
        /// 

        /// *** 使用多執行緒時，不要透過延時等方式去掌控順序，不要試圖「複雜的多模式」掌控順序 ***


        /// <summary>
        /// 同步
        /// 同步單執行緒方法：按順序執行，每次調用完成後才能進入下一行，是同一個執行緒執行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sync_Click(object sender, RoutedEventArgs e) 
        {
            Console.WriteLine();
            Console.WriteLine($"********* Sync_Click 同步方法 Start {Thread.CurrentThread.ManagedThreadId} *********");

            {// Ex1
                //for (int i = 0; i < 5; i++)
                //{
                //    string name = string.Format("{0}_{1}",
                //        "Sync_Click",
                //        i);
                //    DoSomethingLong(name);
                //}
            }

            {// Ex2
                Action<string> action = DoSomethingLong;

                for(int i = 0; i < 5; i++)
                {
                    string name = string.Format("{0}_{1}",
                        "Sync_Click",
                        i);
                    action.Invoke(name);
                }
            }

            Console.WriteLine($"********* Sync_Click 同步方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            Console.WriteLine();
        }


        /// <summary>
        /// 任何的非同步多執行緒，都無法離開 Delegate---Lambda---Action/Func
        /// 委託的非同步調用
        /// 非同步多執行緒：發起調用，不等待結束就直接進入下一行；
        ///                動作會由一個新執行緒來執行（子執行緒）； （並行了
        ///                
        /// 多執行緒程式碼不難，困難在於用好；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Async_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine($"********* Async_Click 非同步方法 Start {Thread.CurrentThread.ManagedThreadId} *********");

            {// Ex1
                //Action<string> action = DoSomethingLong;
                //action.Invoke("Async_Click 1");
                //action("Async_Click 2");
                //action.BeginInvoke("Async_Click 3", null, null);
            }

            {// Ex2
                Action<string> action = DoSomethingLong;

                for(int i = 0; i < 5; i++)
                {
                    string name = $"Async_Click {i}";
                    action.BeginInvoke(name, null, null);
                }
            }


            Console.WriteLine($"********* Async_Click 非同步方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            Console.WriteLine();
        }

        #region Private Methods

        /// <summary>
        /// 顯示執行時間（開始、結束）、進行整數累加，製造出 CPU bound
        /// </summary>
        /// <param name="text"></param>
        private void DoSomethingLong(string text)
        {
            Console.WriteLine("********* DoSomethingLong Start {0} {1} {2} *********",
                text,
                Thread.CurrentThread.ManagedThreadId.ToString("00"),
                DateTime.Now.ToString("HHmmss:fff"));

            long lReuslt = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                lReuslt += i;
            }

            Console.WriteLine("********* DoSomethingLong End {0} {1} {2} *********",
                text,
                Thread.CurrentThread.ManagedThreadId.ToString("00"),
                DateTime.Now.ToString("HHmmss:fff"));
        }

        #endregion
    }
}
