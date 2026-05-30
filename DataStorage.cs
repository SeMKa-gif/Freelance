using System.Collections.Generic;

namespace Kyrsovai
{
    public class DataStorage
    {
        public List<User> Users { get; set; }
        public List<Order> Orders { get; set; }
        public string CurrentUserEmail { get; set; }
    }
}