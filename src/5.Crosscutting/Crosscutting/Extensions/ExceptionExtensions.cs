namespace System
{
    public static class ExceptionExtensions
    {
        public static string GetFullMessage(this Exception exception)
        {
            var innerMessage = "";
            if (exception.InnerException != null)
                innerMessage = " || Inner exception: " + GetFullMessage(exception.InnerException);

            return exception.Message  + " " + exception.StackTrace + innerMessage;
        }

        public static string GetFullMessage(this AggregateException aggregateException)
        {
            var globalMessage = "";

            foreach (var exception in aggregateException.InnerExceptions)
            {
                var innerMessage = "";

                if (exception.InnerException != null)
                    innerMessage = " || Inner exception: " + GetFullMessage(exception.InnerException);

                globalMessage += exception.Message + innerMessage + Environment.NewLine;
            }

            return globalMessage;
        }
    }
}
