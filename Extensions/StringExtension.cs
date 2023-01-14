namespace BugTracker.Extensions
{
	public static class StringExtension
	{
		public static string Truncate(this string value, int maxChars)
		{
			return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
		}
	}
}
