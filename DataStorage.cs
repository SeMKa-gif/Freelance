using System.Collections.Generic;

namespace Kyrsovai
{
    public class DataStorage
    {
        public List<User> Users { get; set; }
        public List<Order> Orders { get; set; }
        public List<Bid> Bids { get; set; }
        public List<Message> Messages { get; set; }
        public string CurrentUserEmail { get; set; }
    }
}