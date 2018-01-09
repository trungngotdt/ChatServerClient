using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message
{
    [Serializable]
    public class MessageStruct
    {
        private string contents;
        private string address;
        private string sender;

        public string Contents { get => contents; set => contents = value; }
        public string Address { get => address; set => address = value; }
        public string Sender { get => sender; set => sender = value; }

        public MessageStruct()
        {
            Contents = null;
            Address = null;
            Sender = null;
        }
        public MessageStruct(string content,string address,string sender)
        {
            this.Address = address;
            this.Contents = content;
            this.Sender = sender;
        }
    }
}
