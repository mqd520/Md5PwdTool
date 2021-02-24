using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Common
{
    /// <summary>
    /// MyTimer
    /// </summary>
    public class MyTimer
    {
        #region Fields
        protected object _obj = new object();
        #endregion


        #region Properties
        protected Timer Timer { get; set; } = new Timer();

        /// <summary>
        /// Get or Set Items
        /// </summary>
        protected IDictionary<string, MyTimerItemInfo> Items { get; set; } = new Dictionary<string, MyTimerItemInfo>();

        /// <summary>
        /// Get or Set Accuracy
        /// </summary>
        public int Accuracy { get; set; } = 100;
        #endregion


        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accuracy">Accuracy</param>
        public MyTimer(int accuracy = 100)
        {
            Accuracy = accuracy;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Init
        /// </summary>
        public void Init()
        {
            Timer.AutoReset = false;
            Timer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            Stop();
        }

        public void Start()
        {
            Timer.Interval = Accuracy;
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }

        /// <summary>
        /// CreateTimerId
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="repeat"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string CreateTimerId(int timeout, int repeat = 0, Action<int, int> action = null)
        {
            string id = Guid.NewGuid().ToString();
            var item = new MyTimerItemInfo
            {
                Id = id,
                Repeat = repeat,
                Timeout = timeout,
                CreateTime = CommonTool.GetMiscoTimeStamp(),
                CompletedHandler = action
            };

            lock (_obj)
            {
                Items.Add(id, item);
            }

            return id;
        }


        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="id"></param>
        public void Remove(string id)
        {
            lock (_obj)
            {
                foreach (var item in Items)
                {
                    if (item.Key == id)
                    {
                        Items.Remove(item);

                        break;
                    }
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var ls = new List<dynamic>();
            var timestamp = CommonTool.GetMiscoTimeStamp();

            lock (_obj)
            {
                var ls1 = new List<string>();
                foreach (var item in Items)
                {
                    int n1 = 0;
                    Int64 n = timestamp - item.Value.CreateTime;
                    if (n > item.Value.Timeout)
                    {
                        n1 = (int)(n / item.Value.Timeout);
                        if (n % item.Value.Timeout != 0)
                        {
                            n1++;
                        }
                    }

                    if (n1 > 0)
                    {
                        ls.Add(new { Timeout = item.Value.Timeout, Count = n, Handler = item.Value.CompletedHandler });

                        if (n1 >= item.Value.Repeat)
                        {
                            ls1.Add(item.Key);
                        }
                    }
                }

                foreach (var item in ls1)
                {
                    Items.Remove(item);
                }
            }

            try
            {
                foreach (var item in ls)
                {
                    var fun = item.Handler as Action<int, int>;
                    if (fun != null)
                    {
                        fun.Invoke((int)item.Timeout, (int)item.Count);
                    }
                }
            }
            catch (Exception e1)
            {
                ConsoleHelper.WriteLine(
                    ELogCategory.Fatal,
                    string.Format("MyTimer.Timer_Elapsed Exception: {0}", e1.Message),
                    true,
                    e: e1
                );
            }

            Timer.Start();
        }
        #endregion
    }
}
