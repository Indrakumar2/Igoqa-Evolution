namespace Evolution.Common.Extensions
{
    public static class IsNullExtension
    {
        public static string IsEmptyReturnNull(this string source) => (source == "" ? null : source); //Added to return null if value empty - For Data Sync issue

    }
}
