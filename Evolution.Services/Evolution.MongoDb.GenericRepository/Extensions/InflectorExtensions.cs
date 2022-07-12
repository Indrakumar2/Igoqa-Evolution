using Evolution.MongoDb.GenericRepository.Utils;
using System.Text.RegularExpressions;

namespace Evolution.MongoDb.GenericRepository.Extensions
{
    public static class InflectorExtensions
    {     
        public static string Pluralize(this string word, bool inputIsKnownToBeSingular = true)
        {
            return Vocabularies.Default.Pluralize(word, inputIsKnownToBeSingular);
        }
             
        public static string Singularize(this string word, bool inputIsKnownToBePlural = true)
        {
            return Vocabularies.Default.Singularize(word, inputIsKnownToBePlural);
        }
        
        public static string Pascalize(this string input)
        {
            return Regex.Replace(input, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
        }
        
        public static string Camelize(this string input)
        {
            var word = Pascalize(input);
            return word.Substring(0, 1).ToLower() + word.Substring(1);
        }
        
        public static string Underscore(this string input)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(input, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])", "$1_$2"), @"[-\s]", "_").ToLower();
        }
        
        public static string Dasherize(this string underscoredWord)
        {
            return underscoredWord.Replace('_', '-');
        }
        
        public static string Hyphenate(this string underscoredWord)
        {
            return Dasherize(underscoredWord);
        }
    }
}
