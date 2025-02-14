using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTelegram
{
    public class MessageItem
    {
        public string Nickname { get; set; }
        public string Message { get; set; }

        public MessageItem(string nickname, string message)
        {
            Nickname = nickname;
            Message = message;
        }
    }
}
