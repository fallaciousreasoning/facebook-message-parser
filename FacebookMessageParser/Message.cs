using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookMessageParser
{
    public class Message
    { 
        public string Thread { get; set; }
        public string Text { get; set; }
        public string Sender { get; set; }
        public string SentDate { get; set; }
    }
}
