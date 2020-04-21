
namespace SmartPlaygroundWeb.Models
{
    public class UserAtZone
    {
        string name;
        string image;
        int zoneId;

        public UserAtZone(string name, string image, int zoneId)
        {
            this.Name = name;
            this.Image = image;
            this.ZoneId = zoneId;
        }

        public string Name { get => name; set => name = value; }
        public string Image { get => image; set => image = value; }
        public int ZoneId { get => zoneId; set => zoneId = value; }
    }
}