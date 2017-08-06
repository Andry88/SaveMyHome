using System;
using System.Collections.Generic;

namespace SaveMyHome.Models
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
            var apartments = new List<int>();
            int currApart = 0;

            for (int i = ApartmentsWithinFloor - 1; i >= 0; i--)
            {
                currApart = floor * ApartmentsWithinFloor - 1 - i;

                if (currApart == 0)
                    continue;

                apartments.Add(currApart);
            }

            return apartments;
        }
    }
}