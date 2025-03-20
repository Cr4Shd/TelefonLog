using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelefonLog.Utils
{
    
    class CallLog
    {
        public string CallBackNumber { get; set; }
        public string DateTime { get; set; }
        public string CName { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public int IsMedical { get; set; }
        public string CallBound { get; set; }
        public string CallID { get; private set; }

        /// <summary>
        /// Konstruktor für neue CallLog Elemente
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="time"></param>
        /// <param name="numb"></param>
        /// <param name="dateTime"></param>
        /// <param name="ismed"></param>
        /// <param name="bound"></param>
        public CallLog(string name, string text, string time, string numb, string dateTime, int ismed, string bound)
        {
            this.CName = name;
            this.Text = text;
            this.Time = time;
            this.CallBackNumber = numb;
            this.DateTime = dateTime;
            this.IsMedical = ismed;
            this.CallBound = bound;
            this.CallID = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Konstruktor für Elemente aus der Datenbank
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="time"></param>
        /// <param name="numb"></param>
        /// <param name="dateTime"></param>
        /// <param name="ismed"></param>
        /// <param name="bound"></param>
        /// <param name="callid"></param>
        public CallLog(string name, string text, string time, string numb, string dateTime, int ismed, string bound, string callid)
        {
            this.CName = name;
            this.Text = text;
            this.Time = time;
            this.CallBackNumber = numb;
            this.DateTime = dateTime;
            this.IsMedical = ismed;
            this.CallBound = bound;
            this.CallID = callid;
        }
    }
}
