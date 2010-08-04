    public static class DateTimeEx
    {
        public static bool IsOverTimeout(this DateTime dt, int seconds)
        {
            var diff = dt - DateTime.Now;
            var ts = diff.TotalSeconds;

            return ts > seconds;
        }
    }