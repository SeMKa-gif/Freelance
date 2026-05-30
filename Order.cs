using System;

namespace Kyrsovai
{
    public class Order
    {
        public string OrderTitle { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int Price { get; set; }
        public double Rating { get; set; }
        public string ImagePath { get; set; }
        public string Author { get; set; }
    }
}
