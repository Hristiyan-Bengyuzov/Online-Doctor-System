namespace OnlineDoctorSystem.Web.Infrastructure.Utilities
{
	public static class Haversine
	{
		public static double CalculateHaversineDistance(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
		{
			const double EARTH_RADIUS_KM = 6371.0;

			double deltaLatitude = (endLatitude - startLatitude) * Math.PI / 180.0;
			double deltaLongitude = (endLongitude - startLongitude) * Math.PI / 180.0;

			double havHalfDeltaLat = Math.Sin(deltaLatitude / 2.0) * Math.Sin(deltaLatitude / 2.0);
			double havHalfDeltaLon = Math.Sin(deltaLongitude / 2.0) * Math.Sin(deltaLongitude / 2.0);
			double cosStartLat = Math.Cos(startLatitude * Math.PI / 180.0);
			double cosEndLat = Math.Cos(endLatitude * Math.PI / 180.0);

			double a = havHalfDeltaLat + cosStartLat * cosEndLat * havHalfDeltaLon;
			double centralAngle = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

			double distance = EARTH_RADIUS_KM * centralAngle;

			return distance;
		}
	}
}
