using System;

namespace Kyrsovai
{
    public class Bid
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderTitle { get; set; }
        public string FreelancerName { get; set; }
        public string FreelancerEmail { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // "pending", "accepted", "rejected"
    }
}
