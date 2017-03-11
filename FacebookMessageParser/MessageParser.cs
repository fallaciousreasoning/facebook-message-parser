using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace FacebookMessageParser
{
    public class MessageParser
    {
        public IEnumerable<Message> Parse(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var document = htmlDoc.DocumentNode;

            var messageHeaders = document.SelectNodes("//div[@class='message']");
            var result = (from header in messageHeaders select new Message
            {
                Text = header.SelectSingleNode("./following-sibling::p")?.InnerText.Trim(),
                Sender = header.SelectSingleNode("(./div/span)[1]").InnerText.Trim(),
                SentDate = header.SelectSingleNode("(./div/span)[2]").InnerText.Trim()
            });
           return result.ToList();
        }
    }
}
