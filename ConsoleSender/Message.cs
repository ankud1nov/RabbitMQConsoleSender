using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSender
{
    class Message
    {
        int number;
        DateTime timeSend;
        string comment;
        string text;
        string hashSumm;
        public Message()
        {
            number = 1;
            comment = "";
            SetParams();
        }
        public Message(string comment)
        {
            number = 1;
            this.comment = comment;
            SetParams();
        }
        public void Next()
        {
            number += 1;
            SetParams();
        }
        public void Next(string comment)
        {
            number += 1;
            this.comment = comment;
            SetParams();
        }
        public string GetMessage()
        {
            return text;
        }
        void SetParams()
        {
            timeSend = DateTime.Now;
            text = $"{number}. \"{timeSend}\" Комментарий: {comment}";
            hashSumm = $"{number}{timeSend}{comment}";
        }
    }
}
