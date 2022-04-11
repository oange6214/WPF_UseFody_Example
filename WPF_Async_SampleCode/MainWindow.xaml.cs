using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        /// 
        /// 一、
        /// 非同步的回調和狀態參數
        /// 非同步等待三種方式
        /// 獲取非同步的返回值
        /// 
        /// 二、
        /// .NETFramework 多版本多執行緒對比
        /// 多執行緒最近實踐 Task 應對多執行緒應用場景
        /// 多執行緒進階思考
        /// 
        /// 三、
        /// 局部變數和執行緒安全問題解讀
        /// Lock 關鍵字使用全解析
        /// 執行緒安全解決方案總覽
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

        /// <summary>
        /// 1、非同步的回調和狀態參數
        /// 2、非同步等待三種方式
        /// 3、獲取非同步的返回值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AsyncAdvanced_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine($"********* Async_Click 非同步方法 Start {Thread.CurrentThread.ManagedThreadId} *********");

            // 用戶點擊按鈕，希望業務操作要做，但是不要卡界面，可以使用非同步多執行緒

            Action<string> action = UpdateDB;

            #region 同步、非同步

            //action.Invoke("AsyncAdvanced_Click 1");
            //action.BeginInvoke("AsyncAdvanced_Click 2", null, null);

            #endregion

            #region 非同步回調

            // 不允許沒有監控的項目上線，需要在業務操作後記錄下日誌

            //action.BeginInvoke("AsyncAdvanced_Click 2", arg =>
            //{
            //    Console.WriteLine(arg.AsyncState);
            //    Console.WriteLine($"AsyncAdvanced_Click 操作已經完成了... {Thread.CurrentThread.ManagedThreadId}");
            //}, "sunday");

            #endregion

            #region IsCompleted 等待（IAsyncResult）

            {// Ex3-1
                //// 用戶必須確定操作完成，才能返回---上傳文件，只有成功之後才能預覽---需要等到任務計算完成後才能給用戶返回
                ////action.BeginInvoke("文件上傳", null, null);
                ////Console.WriteLine("完成文件預覽，綁定到界面");


                //// 希望一方面文件上傳<完成後才預覽；另一方面，還希望有個進度提示---只有主執行緒才能操作界面
                //IAsyncResult asyncResult = action.BeginInvoke("文件上傳", null, null);  // 啟動子執行緒完成計算

                //int i = 0;
                //while(!asyncResult.IsCompleted) // IsCompleted 是一個屬性，用來描述非同步動作是否完成；其實一開始就是 false，非同步動作完成後回去修改這個屬性為 true
                //{
                //    // 下面範例是模擬上傳。在真實開發中，一開始可以讀取文件的 size，然後就直接獲取已經上傳好的 size，就在做個比例就是進度。
                //    if(i < 9)
                //    {
                //        ShowConsoleAndView($"當前文件上傳進度為 {++i * 10}%...");
                //    }
                //    else
                //    {
                //        ShowConsoleAndView($"當前文件上傳進度為 99.999%...");
                //    }
                //    Thread.Sleep(200);    // 放在這邊最少延遲 200ms
                //}

                //Console.WriteLine("完成文件上傳，執行預覽，綁定到界面");
                //// 界面不會即時更新，因為主執行緒再忙，忙完才會更新---那要如何才能即時更新？當然就是讓主執行緒閒下來，其他操作都由子執行緒完成就行了
            }

            {// Ex3-2
                // 更新 UI
                //Action actionRect = UpdateRect;
                ////actionRect.Invoke();                    // 可以更新 UI
                //actionRect.BeginInvoke(null, null);     // 透過子執行緒不能更新 UI
                //Console.WriteLine("UpdateRect 執行完成");
            }

            #endregion

            #region 訊息量（AsyncWaitHandle.WaitOne）解決順序問題

            //IAsyncResult asyncResult = action.BeginInvoke("調用接口", null, null);

            //// 應用場景：當中間還有其他執行的時候... 
            //Console.WriteLine("Do Something Else...");
            //Console.WriteLine("Do Something Else...");
            //Console.WriteLine("Do Something Else...");

            ////asyncResult.AsyncWaitHandle.WaitOne();  // 阻塞當前執行緒，直到收到訊息量，從 asyncResult 發出，無延遲

            ////asyncResult.AsyncWaitHandle.WaitOne(-1);    // 一直等待

            //asyncResult.AsyncWaitHandle.WaitOne(1000);  // 等待 1000ms，超過就過去了，這個是用來做「超時」控制。在微服務架構，一個操作需要調用 5 個接口，如果某個接口很慢，會影響整個流程，可以做超時控制，超時就換接口 或者放棄 或者給個結果。

            //Console.WriteLine("接口調用成功，必須是真實的成功...");

            #endregion

            #region EndInvoke

            {
                //// 調用接口，需要返回值
                //int iResult1 = RemoteService();

                //Func<int> func = RemoteService;
                //int iResult2 = func.Invoke();

                //IAsyncResult asyncResult = func.BeginInvoke(null, null);    // 非同步調用結果，描述非同步操作的
                //int iResult3 = func.EndInvoke(asyncResult);
            }

            // 如果想要獲取非同步調用的真實返回值，只能使用 EndInvoke

            {
                //Func<string> func2 = () => DateTime.Now.ToString();
                //string sResult = func2.EndInvoke(func2.BeginInvoke(null, null));
            }

            {
                //Func<string, string> func2 = (s) => $"1 + {s}";
                //string sResult = func2.EndInvoke(func2.BeginInvoke("Json", null, null));
            }

            {
                //Func<int> func = RemoteService;

                //IAsyncResult asyncResult = func.BeginInvoke(arg =>
                //{
                //    int iResult = func.EndInvoke(arg);  // EndInvoke 可以放在主執行緒，也可以放在 回調，每個非同步操作，只能調用一次 EndInvoke
                //}, null);
            }

            #endregion


            Console.WriteLine($"********* Async_Click 非同步方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            Console.WriteLine();
        }

        /// <summary>
        /// .NetFramework 有 N 多個版本，就有 M 多個多執行緒的使用方式
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mutiple_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine($"********* Mutiple_Click 方法 Start {Thread.CurrentThread.ManagedThreadId} *********");

            #region .NETFramework 1.0、1.1（如今用的少了）Thread

            //ThreadStart threadStart = () =>
            //{
            //    Console.WriteLine($"This is Thread Start {Thread.CurrentThread.ManagedThreadId}");
            //    Thread.Sleep(2000);
            //    Console.WriteLine($"This is Thread End {Thread.CurrentThread.ManagedThreadId}");
            //};
            //Thread thread = new Thread(threadStart);

            ////thread.Start();
            ////thread.Resume();
            ////thread.Join();
            ////thread.Abort();
            ////thread.IsBackground = true;
            ////Thread.ResetAbort();

            //// Thread 的 API 特別豐富，可以很彈性的控制，但是很難做好

            //// Thread 有以下問題：
            //// 因為執行緒資源是作業系統管理的，響應並不靈敏，所以不好控制。
            //// Tread 啟動執行緒是沒有控制的，可能導致死機。

            #endregion

            #region .NETFramework 2.0（新的 CLR）ThreadPool（大多程式都運用此方法）

            //// 池化資源管理設計思想，執行緒是一種資源，之前每次要用執行緒，就去申請一個執行緒，使用之後，釋放掉；
            //// 池化就是做一個容器，容器提前申請 5 個執行緒，程序需要使用執行緒，直接找容器獲取，用完後在放回容器（利用控制狀態），避免頻繁的申請和銷毀；
            //// 容器自己還會根據限制數量去申請和釋放；
            //// 優點：
            ////  1、執行緒重複用
            ////  2、可以限制最大執行緒數量
            //// 缺點：
            ////  1、API 太少
            ////  2、執行緒等待順序控制特別弱（MRE），影響實戰運用

            //WaitCallback waitCallback = arg =>
            //{
            //    Console.WriteLine($"This is ThreadPool Start {Thread.CurrentThread.ManagedThreadId}");
            //    Thread.Sleep(2000);
            //    Console.WriteLine($"This is ThreadPool End {Thread.CurrentThread.ManagedThreadId}");
            //};

            //ThreadPool.QueueUserWorkItem(waitCallback);

            #endregion

            #region .NETFramework 3.0 Task 被稱之為多執行緒的最佳實踐

            //// 1、Task 執行緒全部是執行緒池
            //// 2、擁有豐富的 API

            //Action action = () =>
            //{
            //    Console.WriteLine($"This is Task Start {Thread.CurrentThread.ManagedThreadId}");
            //    Thread.Sleep(2000);
            //    Console.WriteLine($"This is Task End {Thread.CurrentThread.ManagedThreadId}");
            //};

            //Task task = new Task(action);
            //task.Start();

            #endregion

            #region .NETFramework 4.0 Task.Run

            //Task.Run(() =>
            //{
            //    Console.WriteLine($"This is Task.Run Start {Thread.CurrentThread.ManagedThreadId}");
            //    Thread.Sleep(2000);
            //    Console.WriteLine($"This is Task.Run End {Thread.CurrentThread.ManagedThreadId}");
            //});

            #endregion

            #region Parallel（大多運用在客戶端）

            {
                //// Parallel 可以啟動多執行緒，主執行緒也參與計算，節約一個執行緒
                //// 可以透過 ParallelOptions 輕鬆控制最大併發數量

                //Parallel.Invoke(() =>
                //{
                //    Console.WriteLine($"This is Parallel Start 1 {Thread.CurrentThread.ManagedThreadId}");
                //    Thread.Sleep(2000);
                //    Console.WriteLine($"This is Parallel End 1 {Thread.CurrentThread.ManagedThreadId}");
                //}, () =>
                //{
                //    Console.WriteLine($"This is Parallel Start 2 {Thread.CurrentThread.ManagedThreadId}");
                //    Thread.Sleep(2000);
                //    Console.WriteLine($"This is Parallel End 2 {Thread.CurrentThread.ManagedThreadId}");
                //}, () =>
                //{
                //    Console.WriteLine($"This is Parallel Start 3 {Thread.CurrentThread.ManagedThreadId}");
                //    Thread.Sleep(2000);
                //    Console.WriteLine($"This is Parallel End 3 {Thread.CurrentThread.ManagedThreadId}");
                //}, () =>
                //{
                //    Console.WriteLine($"This is Parallel Start 4 {Thread.CurrentThread.ManagedThreadId}");
                //    Thread.Sleep(2000);
                //    Console.WriteLine($"This is Parallel End 4 {Thread.CurrentThread.ManagedThreadId}");
                //});
            }

            #endregion


            Console.WriteLine($"********* Mutiple_Click 方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            Console.WriteLine();
        }

        /// <summary>
        /// Task 解析（多執行緒開發，大多人使用）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Task_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine($"********* Task_Click 方法 Start {Thread.CurrentThread.ManagedThreadId} *********");

            Console.WriteLine("Jed 又接到一個案子");
            Console.WriteLine("溝通大概需求和標準，談妥價格...");
            Console.WriteLine("簽合約，先收取 50% 的費用");    // 一定要收，很多會搞到後面不給錢
            Console.WriteLine("需求分析-框架搭建-模塊切分...");
            Console.WriteLine("資料庫初步設計");
            Console.WriteLine("高級班挑選人選，組建開發團隊");    // 團隊大加分錢
            Console.WriteLine("開始工作");

            // 除了明白情境，還要知道執行緒問題，有嚴格時間限制的，先後順序的，只能單執行緒
            // 下面用多執行緒是為了提升效率，因為這些任務是可以獨立併發執行的
            // 一個資料庫查詢需要 10s，能不能多執行緒優化？（不行，這是不可分割的任務
            // 一個操作要查詢資料庫、要調用接口、要讀磁碟檔案。（可以多執行緒，因為任務彼此不干擾

            List<Task> tasks = new List<Task>
            {
                Task.Run(() => this.Coding("李五", "Portal")),
                Task.Run(() => this.Coding("林一", "Client")),
                Task.Run(() => this.Coding("王三", "WeChat")),
                Task.Run(() => this.Coding("陳二", "Service")),
                Task.Run(() => this.Coding("香七", "SqlServer")),
                Task.Run(() => this.Coding("粘六", "WebApi"))
            };

            #region 正常流程（會阻塞操作界面）

            //Task.WaitAny(tasks.ToArray());  // 阻塞當前執行緒，直到任一任務結束---主執行緒被阻塞，所以卡界面
            //Console.WriteLine("項目里程碑達成，收取 20% 的費用");

            //// 既需要多執行緒來提升性能，又需要在多執行緒全部完成後才能執行的操作。
            //Task.WaitAll(tasks.ToArray());  // 阻塞當前執行緒，直到全部任務結束---主執行緒被阻塞，所以卡界面


            //Console.WriteLine("項目驗收交付後，支付剩餘的全部費用");

            //Console.WriteLine($"********* Task_Click 方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            //Console.WriteLine();

            #endregion

            #region Task.Run 解決阻塞（不建議做法）

            // 盡量不要執行緒嵌入執行緒，容易出問題。
            // 子執行緒完成的，不能直接操作界面

            //Task.Run(() =>
            //{
            //    Task.WaitAny(tasks.ToArray());  // 阻塞當前執行緒，直到任一任務結束---主執行緒被阻塞，所以卡界面
            //    Console.WriteLine("項目里程碑達成，收取 20% 的費用");

            //    // 既需要多執行緒來提升性能，又需要在多執行緒全部完成後才能執行的操作。
            //    Task.WaitAll(tasks.ToArray());  // 阻塞當前執行緒，直到全部任務結束---主執行緒被阻塞，所以卡界面


            //    Console.WriteLine("項目驗收交付後，支付剩餘的全部費用");

            //    Console.WriteLine($"********* Task_Click 方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            //    Console.WriteLine();
            //});

            #endregion

            #region TaskFactory 解決阻塞（建議）

            //TaskFactory taskFactory = new TaskFactory();

            //// 等待任一任務完成後，啟動一個新的 task 來完成後緒動作
            //taskFactory.ContinueWhenAny(tasks.ToArray(), ca =>
            //{
            //    Console.WriteLine($"{ca} 第一個完成，獲取紅包獎勵 {Thread.CurrentThread.ManagedThreadId}");
            //});

            //// 等待全部任務完成後，啟動一個新的 task 來完成後緒動作
            //taskFactory.ContinueWhenAll(tasks.ToArray(), tArray =>
            //{
            //    Console.WriteLine($"慶功宴，部屬聯調測試 {Thread.CurrentThread.ManagedThreadId}");
            //});

            //// *** Countinue 的後緒執行緒，可能是新執行緒，可能是剛完成任務的執行緒，還可能是同一個執行緒，不可能是主執行緒 ***
            //// *** 執行緒是不可預測，動作先後都可能的 ***

            //Task.WaitAny(tasks.ToArray());  // 阻塞當前執行緒，直到任一任務結束---主執行緒被阻塞，所以卡界面
            //Console.WriteLine("項目里程碑達成，收取 20% 的費用");

            //// 既需要多執行緒來提升性能，又需要在多執行緒全部完成後才能執行的操作。
            //Task.WaitAll(tasks.ToArray());  // 阻塞當前執行緒，直到全部任務結束---主執行緒被阻塞，所以卡界面

            //Console.WriteLine("項目驗收交付後，支付剩餘的全部費用");

            //Console.WriteLine($"********* Task_Click 方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            //Console.WriteLine();

            #endregion

            #region TaskFactory 解決 ContinueWhenAll 沒有順序問題（ContinueWhenAny、ContinueWhenAll、WaitAny、WaitAll 可以解決 90% 多執行緒控制問題）

            TaskFactory taskFactory = new TaskFactory();

            // 等待任一任務完成後，啟動一個新的 task 來完成後緒動作
            taskFactory.ContinueWhenAny(tasks.ToArray(), ca =>
            {
                Console.WriteLine($"{ca} 第一個完成，獲取紅包獎勵 {Thread.CurrentThread.ManagedThreadId}");
            });

            // 等待全部任務完成後，啟動一個新的 task 來完成後緒動作
            // [解決點] task.Factory 會返回 Task 類型，把它加入 task 清單中，在後面的 WaitAll 即可卡控
            tasks.Add(
                taskFactory.ContinueWhenAll(tasks.ToArray(), tArray =>
                {
                    Console.WriteLine($"慶功宴，部屬聯調測試 {Thread.CurrentThread.ManagedThreadId}");
                })
            );

            // *** Countinue 的後緒執行緒，可能是新執行緒，可能是剛完成任務的執行緒，還可能是同一個執行緒，不可能是主執行緒 ***
            // *** 執行緒是不可預測，動作先後都可能的 ***

            Task.WaitAny(tasks.ToArray());  // 阻塞當前執行緒，直到任一任務結束---主執行緒被阻塞，所以卡界面
            Console.WriteLine("項目里程碑達成，收取 20% 的費用");

            // 既需要多執行緒來提升性能，又需要在多執行緒全部完成後才能執行的操作。
            Task.WaitAll(tasks.ToArray());  // 阻塞當前執行緒，直到全部任務結束---主執行緒被阻塞，所以卡界面

            Console.WriteLine("項目驗收交付後，支付剩餘的全部費用");

            Console.WriteLine($"********* Task_Click 方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            Console.WriteLine();

            #endregion

        }

        /// <summary>
        /// 執行緒安全解析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreadSafe_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine($"********* Task_Click 方法 Start {Thread.CurrentThread.ManagedThreadId} *********");


            #region for 建立 Task 疑問
            //for (int i = 0; i < 5; i++)
            //{
            //    int j = i;
            //    //Thread.Sleep(20);
            //    Task.Run(() =>
            //    {
            //        Console.WriteLine($"This is {i} {j} Start... {Thread.CurrentThread.ManagedThreadId}");
            //        Thread.Sleep(2000);
            //        Console.WriteLine($"This is {i} {j} End... {Thread.CurrentThread.ManagedThreadId}");
            //    });
            //}
            #endregion

            #region 什麼是執行緒安全問題？

            //// 多執行緒去訪問同一個集合，有問題嗎？一般是沒有問題的
            //// 執行緒安全問題都是出現在修改一個物件的

            //List<int> lists = new List<int>();

            //for (int i = 0; i < 10000; i++) // 多執行緒後，結果就變成小於 10000---就是資料丟失了
            //{
            //    Task.Run(() =>
            //    {
            //        lists.Add(i);
            //    });
            //}

            //// 多執行緒安全問題：一段程式碼，單執行緒和多執行緒 執行的結果不一致，就表明有執行緒安全問題
            //// List 是個陣列結構，在記憶體上是連續擺放的，假如同一時刻，去增加一個資料，都是操作同一個記憶體位置，2 個 CPU 同時發了命令，記憶體先執行一個在執行一個，就出現覆蓋了。

            //Thread.Sleep(3000);
            //Console.WriteLine(lists.Count);

            #endregion

            #region 解決執行緒安全問題

            //List<int> lists = new List<int>();

            //for (int i = 0; i < 10000; i++)
            //{
            //    Task.Run(() =>
            //    {
            //        lock (LOCK)
            //        {
            //            lists.Add(i);
            //        }
            //    });
            //}

            //// 加 lock 就能解決執行緒安全問題---本質就是「單執行緒化」---lock 就是保證方法區塊任意時刻只有一個執行緒能進去，其他執行緒就排隊。
            //// Lock 原理---語法糖---等同 Monitor ---鎖定一個記憶體引用地址---所以不能是值類型---也不能是 null---因為 Lock 就是佔據引用，所以需要一個引用

            //Thread.Sleep(3000);
            //Console.WriteLine(lists.Count);

            #endregion

            #region LOCK 相關測試

            #region Lock Static（可以併發）

            {
                //TestLock.Show();

                //for (int i = 0; i < 5; i++)
                //{
                //    int j = i;

                //    Task.Run(() =>
                //    {
                //        //lock (TestLock.TestLock_LOCK)   // 省事，共用 Lock
                //        lock (LOCK)  // 主程序與方法是併發的
                //        {
                //            Console.WriteLine($"This is {i} {j} MainShow Start... {Thread.CurrentThread.ManagedThreadId}");
                //            Thread.Sleep(2000);
                //            Console.WriteLine($"This is {i} {j} MainShow End... {Thread.CurrentThread.ManagedThreadId}");
                //        }
                //    });
                //}
                //// 如果共用一個鎖，就會出現相互阻塞
                //// 鎖不同，才能併發
            }

            #endregion

            #region Lock Class---多實例（可以併發）

            {
                //TestLock testLock1 = new TestLock();
                //testLock1.ShowTemp(1);

                //TestLock testLock2 = new TestLock();
                //testLock2.ShowTemp(2);

                // 不同的實例裡面，都是不同的字段，所以可以併發。
            }

            #endregion

            #region Lock String（不可以併發）

            {
                //TestLock testLock1 = new TestLock();
                //testLock1.ShowString(1);

                //for (int i = 0; i < 5; i++)
                //{
                //    int j = i;

                //    Task.Run(() =>
                //    {
                //        lock (LOCK_String)
                //        {
                //            Console.WriteLine($"This is {i} {j} MainShow Start... {Thread.CurrentThread.ManagedThreadId}");
                //            Thread.Sleep(2000);
                //            Console.WriteLine($"This is {i} {j} MainShow End... {Thread.CurrentThread.ManagedThreadId}");
                //        }
                //    });
                //}
                //// 鎖定的是記憶體引用---字串是共享記憶體位置---堆裡面只有一個「努力向上」
            }

            #endregion

            #region Lock Generic（1 和 2 不能併發、 2 和 3 可以併發）

            {
                //TestLockGeneric<int>.Show(1);
                //TestLockGeneric<int>.Show(2);
                //TestLockGeneric<TestLock>.Show(3);
                //// 1 和 2 不能併發，因為是相同的變量---泛型類，在類型參數相同時，是同一個類。
                //// 2 和 3 可以併發，因為是不同的變量---泛型類，在類型參數不同時，是不同的類。
            }

            #endregion

            #region Lock This（可以併發）

            {
                //TestLock testLock1 = new TestLock();
                //testLock1.ShowThis(1);

                //TestLock testLock2 = new TestLock();
                //testLock2.ShowThis(2);
            }

            #endregion

            #region 主執行緒與實例（不可以併發）

            {
                //TestLock testLock1 = new TestLock();
                //testLock1.ShowThis(1);

                //for (int i = 0; i < 5; i++)
                //{
                //    int j = i;
                //    Task.Run(() =>
                //    {
                //        lock (testLock1)    // 鎖 TestLock 物件
                //        {
                //            Console.WriteLine($"This is {i} {j} Main Start... {Thread.CurrentThread.ManagedThreadId}");
                //            Thread.Sleep(2000);
                //            Console.WriteLine($"This is {i} {j} Main End... {Thread.CurrentThread.ManagedThreadId}");
                //        }
                //    });
                //}
            }

            #endregion

            #region Lock 遞歸（不會死鎖）

            {
                new TestLock().ShowThisAnother(1);
            }

            #endregion

            #endregion


            Console.WriteLine($"********* Task_Click 方法 End {Thread.CurrentThread.ManagedThreadId} *********");
            Console.WriteLine();
        }

        #region Field

        private static readonly object LOCK = new object();
        private static readonly string LOCK_String = "努力向上";

        #endregion


        #region Private Methods
        // 解決執行緒安全問題（標準做法）

        /// <summary>
        /// 模擬 Coding
        /// </summary>
        /// <param name="text"></param>
        private void Coding(string name, string project)
        {
            Console.WriteLine("********* Coding Start {0} {1} {2} {3}*********",
                name,
                Thread.CurrentThread.ManagedThreadId.ToString("00"),
                DateTime.Now.ToString("HHmmss:fff"),
                project);

            long lReuslt = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                lReuslt += i;
            }

            Console.WriteLine("********* Coding End {0} {1} {2} {3}*********",
                name,
                Thread.CurrentThread.ManagedThreadId.ToString("00"),
                DateTime.Now.ToString("HHmmss:fff"),
                project);
        }

        /// <summary>
        /// 模擬遠程接口
        /// </summary>
        /// <returns></returns>
        private int RemoteService()
        {
            long lReuslt = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                lReuslt += i;
            }
            return DateTime.Now.Day;
        }

        /// <summary>
        /// 更新 UI Label
        /// </summary>
        /// <param name="text"></param>
        private void ShowConsoleAndView(string text)
        {
            Console.WriteLine(text);
            lblProcessing.Content = text;
        }

        /// <summary>
        /// 更新界面 Rect
        /// </summary>
        private void UpdateRect()
        {
            Console.WriteLine("********* UpdateRect Start {0} {1} *********",
                Thread.CurrentThread.ManagedThreadId.ToString("00"),
                DateTime.Now.ToString("HHmmss:fff"));

            try
            {
                Canvas.SetLeft(rect, 50);
            }
            catch (Exception ex)
            {
                Console.WriteLine("子執行緒無法控制主 UI 執行緒");
            }

            Console.WriteLine("********* UpdateRect Start {0} {1} *********",
                Thread.CurrentThread.ManagedThreadId.ToString("00"),
                DateTime.Now.ToString("HHmmss:fff"));
        }

        /// <summary>
        /// 模擬更新資料庫
        /// </summary>
        /// <param name="text"></param>
        private void UpdateDB(string text)
        {
            Console.WriteLine("********* UpdateDB Start {0} {1} {2} *********",
                text,
                Thread.CurrentThread.ManagedThreadId.ToString("00"),
                DateTime.Now.ToString("HHmmss:fff"));

            long lReuslt = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                lReuslt += i;
            }

            Console.WriteLine("********* UpdateDB End {0} {1} {2} *********",
                text,
                Thread.CurrentThread.ManagedThreadId.ToString("00"),
                DateTime.Now.ToString("HHmmss:fff"));
        }

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
