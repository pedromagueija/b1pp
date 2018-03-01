namespace B1PP
{
    using System;

    internal static class Ensure
    {
        public static void NotNullOrEmpty(string parameterName, string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException($@"{parameterName} cannot be null or empty.");
        }
    }
}