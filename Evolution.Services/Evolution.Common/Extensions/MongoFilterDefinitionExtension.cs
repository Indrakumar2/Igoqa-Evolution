using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Evolution.Common.Extensions
{
    public static class MongoFilterDefinitionExtension
    {
        /// <summary>
        /// Use this to get filter query with $text search , Provided "Text" index is created on mongo DB.
        /// NOTE: Without creating "text" index, If text search is used Mongo DB will through an error.
        /// </summary> 
        /// <param name="entity">Object containing filter properties</param>
        /// <param name="excludePropertyNames">PropertyNames that should be excluded in filter query.</param>
        /// <param name="textIndexSearchProperty"> Property Name to which "Text" index is created in Mongo DB.</param>
        /// <param name="textSearchLanguage"> Language that should be used for text search eg:"en,es"</param>
        /// <returns>FilterDefinition query </returns>
        public static FilterDefinition<T> ToMongoFilterDefinition<T>(this T entity, IList<string> excludePropertyNames = null, string textIndexSearchProperty = "",string textSearchLanguage= "none") where T : class
        {
            FilterDefinition<T> filterQuery = null; 
            IList<FilterDefinition<T>> arryOfBuilder = new List<FilterDefinition<T>>();

            var modelProrties = MongoFilterDefinitionExtension.GetProperties<T>(entity);
            if (excludePropertyNames?.Count > 0)
                modelProrties = modelProrties?.Where(x => !excludePropertyNames.Contains(x.Key)).ToList();

            foreach (KeyValuePair<string, KeyValuePair<object, string>> entityProperty in modelProrties)
            {
                if (MongoFilterDefinitionExtension.IsValueApplicableForFilter(entityProperty.Value))
                {
                    if (string.Equals(textIndexSearchProperty, entityProperty.Key?.ToString(), StringComparison.InvariantCultureIgnoreCase) && Type.GetTypeCode(entityProperty.Value.Key.GetType()) == TypeCode.String)
                    {
                        TextSearchOptions textSearchOptions = new TextSearchOptions() {
                            CaseSensitive = false,
                            DiacriticSensitive = false,
                            Language = textSearchLanguage,
                        };
                        arryOfBuilder.Add(Builders<T>.Filter.Text(entityProperty.Value.Key.ToString(), textSearchOptions));
                    }
                    else {
                        arryOfBuilder.Add(Builders<T>.Filter.Eq(entityProperty.Key?.ToString(), entityProperty.Value.Key));
                    }
                }
            }
            if (arryOfBuilder?.Count > 1)
            {
                filterQuery = arryOfBuilder[0];
                for (int i = 1; i < arryOfBuilder?.Count; i++)
                {
                    filterQuery = filterQuery & arryOfBuilder[i];
                }
            }

            return filterQuery;
        }

        private static IList<KeyValuePair<string, KeyValuePair<object, string>>> GetProperties<T>(T entity) where T : class
        {
            var result = new List<KeyValuePair<string, KeyValuePair<object, string>>>();
            if (entity != null)
            {
                var type = entity.GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in properties)
                {
                    var selfValue = type.GetProperty(prop.Name).GetValue(entity, null);
                    if (selfValue != null && !prop.PropertyType.Name.Contains("ICollection"))
                    {
                        KeyValuePair<object, string> valueOperator = new KeyValuePair<object, string>();
                        if (prop.PropertyType.GenericTypeArguments.Length == 1)
                            valueOperator = new KeyValuePair<object, string>(selfValue, prop.PropertyType.GenericTypeArguments[0].Name);
                        else
                            valueOperator = new KeyValuePair<object, string>(selfValue, prop.PropertyType.Name);

                        var propItem = new KeyValuePair<string, KeyValuePair<object, string>>(prop.Name, valueOperator);
                        result.Add(propItem);
                    }
                }
            }
            return result;
        }

        private static bool IsValueApplicableForFilter(KeyValuePair<object, string> obj)
        {
            if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Int16 && (Int16)obj.Key == 0)
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Int32 && (int)obj.Key == 0)
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Int64 && (Int64)obj.Key == 0)
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.String && string.IsNullOrWhiteSpace((string)obj.Key))
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.String && (string.IsNullOrWhiteSpace((string)obj.Key) || (string)obj.Key == "\0"))
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.DateTime && Convert.ToDateTime(obj.Key) == default(DateTime))
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Decimal && (Decimal)obj.Key == 0)
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Byte && (Byte)obj.Key == 0)
                return false;
            else
                return true;
        }
    }
    
}

