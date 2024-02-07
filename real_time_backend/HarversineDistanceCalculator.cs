using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace real_time_backend
{
    public static class HarversineDistanceCalculator
    {
        public static decimal Calculate(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            int EARTH_RADIUS = 6371;
            static double degreesToRadians(double angle) => angle * (Math.PI / 180);

            double deltaLatitude = degreesToRadians(latitude1 - latitude2);
            double deltaLongitude = degreesToRadians(longitude1 - longitude2);

            var squareOfHalfChordLenght = Math.Pow(Math.Sin(deltaLatitude / 2), 2) +
                Math.Cos(degreesToRadians(latitude1)) * Math.Cos(degreesToRadians(latitude2)) *
                Math.Pow(Math.Sin(deltaLongitude / 2), 2);

            var angularDistance = 2 * Math.Atan2(Math.Sqrt(squareOfHalfChordLenght), Math.Sqrt(1 - squareOfHalfChordLenght));

            return (decimal)(angularDistance * EARTH_RADIUS);
        }
    }
}