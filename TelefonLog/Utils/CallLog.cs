using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelefonLog.Utils
{
    
    class CallLog
    {
        public string CName { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }

        public CallLog(string name, string text, string time)
        {
            this.CName = name;
            this.Text = text;
            this.Time = time;
            
        }
    }
}
