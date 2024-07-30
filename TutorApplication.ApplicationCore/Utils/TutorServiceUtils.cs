using System.Globalization;
using TutorApplication.SharedModels.Models;
using TutorApplication.SharedModels.Responses;

namespace TutorApplication.ApplicationCore.Utils
{
	public static class TutorServiceUtils
	{

		public static Dictionary<string, List<MemoResponse>> ConvertMemosToWeekChapters(this IEnumerable<Memo> memos)
		{
			var memoResponse = memos.Select(e => new MemoResponse() { Date = ConvertDateStringToDate(e.Date), BookInfo = e.BookInfo, Time = e.Time, Type = e.Type });
			IEnumerable<MemoResponse> orderedMemos = memoResponse.OrderBy(e => e.Date);

			int numberOfDaysInAWeek = 7;
			int prevDay = 0;
			int prevDaysLeft = 0;
			int week = 0;
			var Weeks = new Dictionary<string, List<MemoResponse>>();
			orderedMemos.ToList().ForEach(memo =>
			{
				int dayOfweek = (int)memo.Date.DayOfWeek + 1;
				int dayOftheYear = memo.Date.DayOfYear;
				int daysLeft = numberOfDaysInAWeek - dayOfweek;

				if ((prevDay + prevDaysLeft) < dayOftheYear)
				{
					week++;
				}

				if (!Weeks.ContainsKey(week.ToString()))
				{
					Weeks.Add(week.ToString(), new List<MemoResponse> { memo });
				}
				else
				{
					Weeks[week.ToString()].Add(memo);
				}

				prevDay = dayOftheYear;
				prevDaysLeft = daysLeft;
			});
			return Weeks;
		}

		public static DateTime ConvertDateStringToDate(string? dateString = "1/12/2024")
		{
			string format = "d/M/yyyy";
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
