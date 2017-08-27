using System;
using System.Collections.Generic;
using System.Linq;

namespace SaveMyHome.Helpers
{
    public static class House
    {
        public static int ApartmentsAmount { get; } = 71;
        public static int ApartmentsWithinFloor { get; } = 6;
        public static int ApartmentsWithinFirstFloor { get; } = 5;
        public static int AlertRangeVertical { get; } = 2;
        public static int AlertRangeHorizontal { get; } = 1;

        public static int FloorsCount => (int)Math.Ceiling((decimal)ApartmentsAmount / ApartmentsWithinFloor);
        public static IList<int> GetApartmentsByFloor(int floor)
        {
            var aparts = new List<int>();
            int currApart = 0;

            for (int i = ApartmentsWithinFloor - 1; i >= 0; i--)
            {
                currApart = floor * ApartmentsWithinFloor - 1 - i;

                if (currApart == 0)
                    continue;

                aparts.Add(currApart);
            }

            return aparts;
        }
        public static IList<int> GetApartmentsByFloor(IEnumerable<int> apartments, int floor) 
        {
            var aparts = new List<int>();
            int currApart = 0;

            for (int i = ApartmentsWithinFloor - 1; i >= 0; i--)
            {
                currApart = floor * ApartmentsWithinFloor - 1 - i;

                if (currApart == 0)
                    continue;

                if (apartments!= null && apartments.Contains(currApart))
                    aparts.Add(currApart);
            }

            return aparts;
        }
        public static List<int> GetFloorsByApartments(IEnumerable<int> numbers)
        {
            List<int> floors = new List<int>();
            foreach (var number in numbers) 
                floors.Add((int)Math.Ceiling((decimal)number / ApartmentsAmount * FloorsCount));
            
            return floors.Distinct().ToList();
        }
    }
}