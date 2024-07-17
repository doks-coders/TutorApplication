using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.ApplicationCore.Utils
{
	public static class CourseServiceUtils
	{
		public static DateTime GetTodayFromTimeStamp(long timestamp)
		{
			DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
			DateTime dateTime = dateTimeOffset.UtcDateTime;
			return GetDayStartFromDate(dateTime);
		}
		public static DateTime GetDayStartFromDate(DateTime dateTime)
		{
			dateTime = dateTime
				.AddHours(-1 * dateTime.Hour)
				.AddMinutes(-1 * dateTime.Minute)
				.AddSeconds(-1 * dateTime.Second);
			return dateTime;
		}

		public static long ConvertDateTimeToStamp(DateTime dateTime)
		{
			DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
			return dateTimeOffset.ToUnixTimeMilliseconds();
		}

		public static DateTime ConvertDateStringToDate(string? dateString = "1/12/24")
		{
			string format = "d/M/yyyy"; // Define the format as day/month/two-digit year
			CultureInfo provider = CultureInfo.InvariantCulture;
			try
			{
				DateTime date = DateTime.ParseExact(dateString, format, provider);
				return date;
			}
			catch (FormatException)
			{
				Console.WriteLine($"Unable to parse '{dateString}' with the format '{format}'.");
				return default;
			}
		}

	}
}
