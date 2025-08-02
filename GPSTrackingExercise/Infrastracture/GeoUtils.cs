﻿namespace GPSTrackingExercise.Infrastracture
{
    public static class GeoUtils
    {
        public static double CalculateDistanceMeters(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371000; // Radius of Earth in meters
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public static double DegreesToRadians(double deg) => deg * Math.PI / 180;
    }
}
