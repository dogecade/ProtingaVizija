namespace Objects.Buses
{
    public class AvailableBus
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public AvailableBus(string type, string number, string id)
        {
            Id = id;
            type = (type == "Troleibusai") ? "troleibusas. Id: " + Id : "autobusas. Id: " + Id;
            Name = number + " " + type;
        }
    }
}