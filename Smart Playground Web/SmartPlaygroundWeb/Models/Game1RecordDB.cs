using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPlaygroundWeb.Models
{
    public class Game1RecordDB
    {
        private int kidId;
        private int score;
        private int wrongColor;
        private int boardHit;
        private DateTime timestamp;

        public int KidId { get => kidId; set => kidId = value; }
        public int Score { get => score; set => score = value; }

        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public int WrongColor { get => wrongColor; set => wrongColor = value; }
        public int BoardHit { get => boardHit; set => boardHit = value; }
    }
}