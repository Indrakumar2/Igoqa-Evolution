using AutoMapper;
using System.Linq;

namespace Evolution.Automapper.Resolver.MongoSearch
{
    public class SearchTextFormatResolver : IMemberValueResolver<object, object, string, string>
    {
        public string Resolve(object source, object destination, string sourceMember, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(sourceMember))
                return null;

            if(sourceMember.Contains("'") || sourceMember.Contains('"')) //def 1397 #4 fix
                CheckIsSingleOrDoubleQuoteExistes(ref sourceMember); //IGO qc 863 :special char issue fix

            if (sourceMember.Split(" + ").Count() > 1)
                return string.Format("'{0}'", string.Join("' '", sourceMember.Split(" + "))); // to do mongo "AND" text search ex: "'text1' 'text2'"
            else if (sourceMember.Split(" - ")?.Count() > 1)
                return string.Format("{0}", sourceMember.Replace(" - ", " "));// to do mongo "OR" text search ex: "text1 text2"
            else
                return  string.Format("\"{0}\"", sourceMember) ;// To do full text search in MONGO DB
        }
        /// <summary>
        /// Append same word by replacing special char and search for both the words. 
        /// As there are chances of having world with these charecters . But user wont be able to enter these character while searching in 
        /// application .
        /// </summary>
        /// <param name="source"></param>
        private void CheckIsSingleOrDoubleQuoteExistes(ref string source)
        {
            source = string.Format("{0} - {1}", source, source.Replace("'", "’").Replace('"', '”'));
        }
    }
}