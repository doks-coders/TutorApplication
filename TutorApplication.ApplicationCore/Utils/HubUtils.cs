using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorApplication.ApplicationCore.Utils
{
	public static class HubUtils
	{
		public static string GetGroupName(string UserName, string RecieverName, string isGroup)
		{
			if (isGroup == "true") return $"{RemoveStrangeCharacters(RecieverName)}";

			int o = string.CompareOrdinal(UserName, RecieverName);

			if (o < 0)
			{
				return RemoveStrangeCharacters($"{RecieverName}-{UserName}");
			}

			return RemoveStrangeCharacters($"{UserName}-{RecieverName}");
		}

		private static string RemoveStrangeCharacters(string str)
		{
			return System.Text.RegularExpressions.Regex.Replace(str, @"[^a-zA-Z0-9]", "");
		}
	}
}
