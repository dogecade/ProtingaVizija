using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace BusService
{
    public class BusHelpers
    {
        public static Bus GetBusData(int rowNumber)
        {
            var allBuses = new List<string>();
            using (var client = new WebClient())
            {
                string result = client.DownloadString("https://www.stops.lt/vilnius/gps.txt");
                allBuses = result.Split('\n').ToList();
            }

            var busData = allBuses[rowNumber - 1].Split(',');

            return new Bus(Convert.ToInt32(busData[0]), busData[1], Convert.ToDouble(busData[3]) / 1000000, Convert.ToDouble(busData[2]) / 1000000);
        }

        public static string GetRouteName(string busNumber)
        {
            var lines = Resource.routes.Split('\n');
            string result = lines.FirstOrDefault(x => x.Contains(busNumber));
            return result.Split(',')[2];
        }

        public static string BusStops(Bus bus, DateTime timeSeen)
        {
            string routeId;

            if (bus.busType == BusType.Trolley)
                routeId = String.Format("vilnius_trol_" + bus.busNumber);

            else if (bus.busType == BusType.Bus && bus.busNumber.Contains("G"))
                routeId = String.Format("vilnius_expressbus_" + bus.busNumber);

            else if (bus.busType == BusType.Bus && bus.busNumber.Contains("N"))
                routeId = String.Format("vilnius_nightbus_" + bus.busNumber);

            else
                routeId = String.Format("vilnius_bus_" + bus.busNumber);

            var trips = Resource.trips.Split('\n');

            var firstPossibleTripId = Convert.ToInt32(trips.First(x => x.Contains(routeId)).Split(',')[2]);
            var lastPossibleTripId = Convert.ToInt32(trips.Last(x => x.Contains(routeId)).Split(',')[2]);

            var allTimes = Resource.stop_times.Split('\n').Where(x => Convert.ToInt32(x.Split(',')[0]) >= firstPossibleTripId && Convert.ToInt32(x.Split(',')[0]) <= lastPossibleTripId).ToList();

            var possibleStops = new List<Tuple<int, int>>();

            for (int i = 1; i < allTimes.Count; i++)
            {
                var departureTime = TimeSpan.Parse(allTimes[i - 1].Split(',')[2]);
                var arrivalTime = TimeSpan.Parse(allTimes[i].Split(',')[1]);

                if (IsInRange(timeSeen, departureTime, arrivalTime))
                {
                    possibleStops.Add(new Tuple<int, int>(Convert.ToInt32(allTimes[i - 1].Split(',')[3]), Convert.ToInt32(allTimes[i].Split(',')[3])));
                }
            }

            return MostProbableStops(bus, possibleStops);
        }

        private static bool IsInRange(DateTime timeSeen, TimeSpan departure, TimeSpan arrival)
        {
            return (timeSeen.TimeOfDay >= departure && timeSeen.TimeOfDay <= arrival);
        }

        private static string MostProbableStops(Bus bus, List<Tuple<int, int>> possibleStopList)
        {
            var allStops = Resource.stops.Split('\n');

            string previousStop;
            string nextStop;

            double previousStopAbsoluteDifference;
            double nextStopAbsoluteDifference;

            List<double> differenceList = new List<double>();

            foreach (var stops in possibleStopList)
            {
                previousStop = allStops.First(x => Convert.ToInt32(x.Split(',')[0]) == stops.Item1);
                nextStop = allStops.First(x => Convert.ToInt32(x.Split(',')[0]) == stops.Item2);

                previousStopAbsoluteDifference = Math.Abs(bus.latitude - Convert.ToDouble(previousStop.Split(',')[4])) +
                                                 Math.Abs(bus.longitude - Convert.ToDouble(previousStop.Split(',')[5]));

                nextStopAbsoluteDifference = Math.Abs(bus.latitude - Convert.ToDouble(nextStop.Split(',')[4])) +
                                                 Math.Abs(bus.longitude - Convert.ToDouble(nextStop.Split(',')[5]));

                differenceList.Add(previousStopAbsoluteDifference + nextStopAbsoluteDifference);
            }

            int mostProbableStops = Min(differenceList);

            previousStop = allStops.First(x => Convert.ToInt32(x.Split(',')[0]) == possibleStopList[mostProbableStops].Item1);
            nextStop = allStops.First(x => Convert.ToInt32(x.Split(',')[0]) == possibleStopList[mostProbableStops].Item2);

            return string.Format(previousStop.Split(',')[2] + " - " + nextStop.Split(',')[2]);
        }

        private static int Min(List<double> list)
        {
            int minIndex = 0;

            for (int i = 1; i < list.Count; i++)
            {
                if (list[minIndex] > list[i])
                    minIndex = i;
            }

            return minIndex;
        }
    }
}