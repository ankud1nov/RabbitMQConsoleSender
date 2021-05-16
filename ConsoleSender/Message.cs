using System;
using System.IO;

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
            number = LastNumber();
            comment = "";
            SetParams();
        }
        public Message(string comment)
        {
            number = LastNumber();
            this.comment = comment;
            SetParams();
        }
        public void Next()
        {
            WriteNextNumber(number);
            number = number + 1;
            SetParams();
        }
        public void Next(string comment)
        {
            WriteNextNumber(number);
            number = number+1;
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
            text = $"{number}. \"{timeSend}\" Комментарий: {comment} Предыдщий хэш: \"{hashSumm}\"";
            hashSumm = $"{number}{timeSend}{comment}";
        }
        int LastNumber()
        {
            var path = "lastNumber";
            int number = 1;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    number = Convert.ToInt32(sr.ReadToEnd());
                }                
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(number);
                }
            }
            return number;
        }
        void WriteNextNumber(int number)
        {
            var path = "lastNumber";
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(number + 1);
            }
        }
    }
}
