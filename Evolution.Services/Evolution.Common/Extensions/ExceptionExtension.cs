using System.Text;

namespace Evolution.Common.Extensions
{
    public static class ExceptionExtension
    {
        public static string ToFullString(this System.Exception e, int level = int.MaxValue)
        {
            var sb = new StringBuilder();
            var exception = e;
            var counter = 1;
            while (exception != null && counter <= level)
            {   
                sb.AppendLine($"{counter}-> Message: {exception.Message}");
                sb.AppendLine($"{counter}-> Source: {exception.Source}");
                sb.AppendLine($"{counter}-> Stack Trace: {exception.StackTrace}");

                exception = exception.InnerException;
                counter++;
            }

            return sb.ToString();
        }
    }
}
