using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BusService
{
    public class Bus
    {
        public BusType BusType { get; }
        public string BusNumber { get; }
        public string RouteId { get; }
        public int BusId { get; }
        public string RouteName { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public TimeSpan BusTime { get; }

        public Bus(int busId, DateTime date)
        {
            // Get data of all buses
            var allBuses = new List<string>();
            using (var client = new WebClient())
            {
                string result = client.DownloadString("https://www.stops.lt/vilnius/gps_full.txt");
                allBuses = result.Split('\n').ToList();
            }

            // First line is a header so remove it
            allBuses.RemoveAt(0);

            try
            {
                // Find a bus in a list
                var busData = allBuses.First(x => Convert.ToInt32(x.Split(',')[3]) == busId);
                var busDataColumns = busData.Split(',');

                BusType = busDataColumns[0] == "Autobusai" ? BusType.Bus : BusType.Trolley;
                BusNumber = busDataColumns[1];
                RouteId = busDataColumns[2];
                BusId = Convert.ToInt32(busDataColumns[3]);
                Latitude = Convert.ToDouble(busDataColumns[5]) / 1000000;
                Longitude = Convert.ToDouble(busDataColumns[4]) / 1000000;
                RouteName = BusHelpers.GetRouteName(BusNumber);
                if (!busDataColumns[9].Equals(""))
                {
                    BusTime = date.AddSeconds(-Convert.ToInt32(busDataColumns[9])).TimeOfDay;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong Bus ID provided");
            }
        }
    }
}