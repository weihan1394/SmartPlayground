using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPlaygroundWeb.Models
{
    public class HighestRecord
    {
        string name;
        int score;
        DateTime timestamp;

        public string Name { get => name; set => name = value; }
        public int Score { get => score; set => score = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
    }
}