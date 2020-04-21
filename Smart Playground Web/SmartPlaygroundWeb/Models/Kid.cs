using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPlaygroundWeb.Models
{
    public class Kid
    {
        private int kidId;
        private string email;
        private DateTime timestamp;
        private string timestampEnd;
        private string contactNumber;
        private string contactRelationship;
        private DateTime registereDate;
        private string note;
        private string currentStation;
        private string name;
        private string zone;
        private string gender;

        public string Email { get => email; set => email = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public string TimestampEnd { get => timestampEnd; set => timestampEnd = value; }
        public string ContactNumber { get => contactNumber; set => contactNumber = value; }
        public string ContactRelationship { get => contactRelationship; set => contactRelationship = value; }
        public DateTime RegistereDate { get => registereDate; set => registereDate = value; }
        public string Note { get => note; set => note = value; }
        public string CurrentStation { get => currentStation; set => currentStation = value; }
        public string Name { get => name; set => name = value; }
        public string Zone { get => zone; set => zone = value; }
        public string Gender { get => gender; set => gender = value; }
        public int KidId { get => kidId; set => kidId = value; }
    }
}