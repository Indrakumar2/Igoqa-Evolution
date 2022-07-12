namespace Evolution.Common.Extensions
{
    public static class BoolExtension
    {
        public static string ToYesNo(this bool source) => (source == true ? "Yes" : "No");

        public static string ToYesNo(this bool? source) => (source == null ? "No" : (source == true ? "Yes" : "No"));
    }
}
