using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPlaygroundWeb.Models
{
    public class Game2Record
    {
        private int kidId;
        private int score;
        private int missHit;
        private int power;
        private string timestamp;

        public int KidId { get => kidId; set => kidId = value; }
        public int Score { get => score; set => score = value; }
        public int MissHit { get => missHit; set => missHit = value; }
        public int Power { get => power; set => power = value; }
        public string Timestamp { get => timestamp; set => timestamp = value; }
    }
}