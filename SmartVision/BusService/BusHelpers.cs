using System;
using System.Collections.Generic;
using System.Linq;

namespace BusService
{
    public class BusHelpers
    {
        public static string GetBusLocation(Bus bus)
        {
            string location = BusLocation(bus);

            if (location.Contains("Bus is currently standing at this location"))
                return location;

            return string.Format(
                "Your missing person was spotted between these stops: {0}. Accurate location: {1}, {2}", location,
                bus.Latitude, bus.Longitude);
        }

        internal static string GetRouteName(string busNumber)
        {
            var lines = Resource.routes.Split('\n');
            string result = lines.FirstOrDefault(x => x.Contains(busNumber));
            return result.Split(',')[2];
        }

        private static string BusLocation(Bus bus)
        {
            string routeId;

            switch (bus.BusType)
            {
                case BusType.Trolley:
                    routeId = String.Format("vilnius_trol_" + bus.BusNumber);
                    break;
                case BusType.Bus when bus.BusNumber.Contains("G"):
                    routeId = String.Format("vilnius_expressbus_" + bus.BusNumber);
                    break;
                case BusType.Bus when bus.BusNumber.Contains("N"):
                    routeId = String.Format("vilnius_nightbus_" + bus.BusNumber);
                    break;
                default:
                    routeId = String.Format("vilnius_bus_" + bus.BusNumber);
                    break;
            }

            var trips = Resource.trips.Split('\n');

            var firstPossibleTripId = Convert.ToInt32(trips.First(x => x.Contains(routeId)).Split(',')[2]);
            var lastPossibleTripId = Convert.ToInt32(trips.Last(x => x.Contains(routeId)).Split(',')[2]);

            var allTimes = Resource.stop_times.Split('\n').Where(x => Convert.ToInt32(x.Split(',')[0]) >= firstPossibleTripId && Convert.ToInt32(x.Split(',')[0]) <= lastPossibleTripId).ToList();

            var possibleStops = new List<Tuple<int, int>>();

            for (int i = 1; i < allTimes.Count; i++)
            {
                var departureTime = TimeSpan.Parse(allTimes[i - 1].Split(',')[2]);
                var arrivalTime = TimeSpan.Parse(allTimes[i].Split(',')[1]);

                if (IsInRange(bus.BusTime, departureTime, arrivalTime) && IsSameTrip(Convert.ToInt32(allTimes[i - 1].Split(',')[0]), Convert.ToInt32(allTimes[i].Split(',')[0])))
                {
                    possibleStops.Add(new Tuple<int, int>(Convert.ToInt32(allTimes[i - 1].Split(',')[3]), Convert.ToInt32(allTimes[i].Split(',')[3])));
                }
            }

            return MostProbableStops(bus, possibleStops);
        }

        private static bool IsInRange(TimeSpan timeSeen, TimeSpan departure, TimeSpan arrival)
        {
            return (timeSeen >= departure && timeSeen <= arrival);
        }

        private static bool IsSameTrip(int previousStopTripId, int nextStopTripId)
        {
            return (previousStopTripId == nextStopTripId);
        }

        private static string MostProbableStops(Bus bus, List<Tuple<int, int>> possibleStopList)
        {
            if (bus.RouteId.Equals(""))
            {
                return $"Bus is currently standing at this location: {bus.Latitude},{bus.Longitude}";
            }

            var allRegularStops = Resource.stops.Split('\n');

            string previousStop;
            string nextStop;

            List<double> regularStopsDifferences = new List<double>();

            foreach (var stops in possibleStopList)
            {
                previousStop = allRegularStops.First(x => Convert.ToInt32(x.Split(',')[0]) == stops.Item1);
                nextStop = allRegularStops.First(x => Convert.ToInt32(x.Split(',')[0]) == stops.Item2);

                var previousStopAbsoluteDifference = Math.Abs(bus.Latitude - Convert.ToDouble(previousStop.Split(',')[4])) +
                                                        Math.Abs(bus.Longitude - Convert.ToDouble(previousStop.Split(',')[5]));

                var nextStopAbsoluteDifference = Math.Abs(bus.Latitude - Convert.ToDouble(nextStop.Split(',')[4])) +
                                                    Math.Abs(bus.Longitude - Convert.ToDouble(nextStop.Split(',')[5]));

                regularStopsDifferences.Add((previousStopAbsoluteDifference > nextStopAbsoluteDifference) ? nextStopAbsoluteDifference : previousStopAbsoluteDifference);
            }

            int mostProbableRegularStopsIndexes = Min(regularStopsDifferences);
            var endStops = Resource.EndStops.Split('\n');
            int mostProbableEndStopIndex = 0;
            List<double> endStopsDifferences = new List<double>();

            foreach (var stops in endStops)
            {
                endStopsDifferences.Add(Math.Abs(bus.Latitude - Convert.ToDouble(stops.Split(',')[1])) +
                                        Math.Abs(bus.Longitude - Convert.ToDouble(stops.Split(',')[2])));
            }

            mostProbableEndStopIndex = Min(endStopsDifferences);

            if (regularStopsDifferences[mostProbableRegularStopsIndexes] >
                endStopsDifferences[mostProbableEndStopIndex])
            {
                return endStops[mostProbableEndStopIndex];

            }

            previousStop = allRegularStops.First(x => Convert.ToInt32(x.Split(',')[0]) == possibleStopList[mostProbableRegularStopsIndexes].Item1);
            nextStop = allRegularStops.First(x => Convert.ToInt32(x.Split(',')[0]) == possibleStopList[mostProbableRegularStopsIndexes].Item2);

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