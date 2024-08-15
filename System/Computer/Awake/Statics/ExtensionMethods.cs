// Source Microsoft Powertoys
// License MIT
// https://github.com/microsoft/PowerToys


namespace FrApp42.System.Computer.Awake.Statics
{
    internal static class ExtensionMethods
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            ArgumentNullException.ThrowIfNull(target);
            ArgumentNullException.ThrowIfNull(source);

            foreach (var element in source)
            {
                target.Add(element);
            }
        }

        public static string ToHumanReadableString(this TimeSpan timeSpan)
        {
            // Get days, hours, minutes, and seconds from the TimeSpan
            int days = timeSpan.Days;
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;

            // Format the string based on the presence of days, hours, minutes, and seconds
            return $"{days:D2} {hours:D2} {minutes:D2} {seconds:D2}";
        }
    }
}
