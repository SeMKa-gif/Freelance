using System;

namespace Kyrsovai
{
    public class Message
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderTitle { get; set; }
        public string From { get; set; }
        public string FromEmail { get; set; }
        public string To { get; set; }
        public string ToEmail { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}