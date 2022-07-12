using System.Collections.Generic;
using System.Linq;

namespace Evolution.Common.Extensions
{
    public static class IListExtension
    {
        public static void AddRange<T>(this IList<T> source, IList<T> args) where T : class
        {
            args?.ToList().ForEach(x => source?.Add(x));
        }

        public static IList<string> ToString<T>(this IList<T> source)
        {
          var result= source.ToList<T>().ConvertAll<string>(delegate (T i)
            {
                return i.ToString();
            });
            return result;

        }

    }
}
