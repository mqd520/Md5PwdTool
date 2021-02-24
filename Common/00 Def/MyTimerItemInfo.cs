using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// MyTimerItemInfo
    /// </summary>
    public class MyTimerItemInfo
    {
        public string Id { get; set; }

        public int Timeout { get; set; } = 0;

        public int Repeat { get; set; } = 0;

        public Int64 CreateTime { get; set; } = 0;

        public Action<int, int> CompletedHandler { get; set; } = null;
    }
}
