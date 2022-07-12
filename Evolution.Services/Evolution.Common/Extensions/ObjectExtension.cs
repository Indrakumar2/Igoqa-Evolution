using Evolution.Common.Enums;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;


namespace Evolution.Common.Extensions
{
    public static class ObjectExtension
    {
        public static T Populate<T>(this object source) where T : class
        {
            try
            {
                if (source is T)
                {
                    return (T)source;
                }
                else
                    return (T)Convert.ChangeType(source, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }


        public static string ToText(this object source)
        {
            System.Reflection.PropertyInfo[] _propertyInfos = source?.GetType().GetProperties();


            if (_propertyInfos != null)
            {
                var sb = new System.Text.StringBuilder();


                foreach (var info in _propertyInfos)
                {
                    var value = info.GetValue(source, null) ?? "(null)";
                    sb.AppendLine(info.Name + ": " + value.ToString());
                } 
                return sb.ToString();
            }
            else
                return string.Empty;
        }


        public static object Clone(this object objSource)
        {
            //Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);
            //Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                if (property.CanWrite)
                {
                    //check whether property type is value type, enum or string type
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType == typeof(String))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, objPropertyValue.Clone(), null);
                        }
                    }
                }
            }
            return objTarget;
        }


        public static void SetPropertyValue<T>(this T source, string propertyName, object propertValue) where T : class
        {
            source.SetPropertyValueOfObject(propertyName, propertValue);
        }

        public static void SetPropertyValue<T>(this T source, IList<KeyValuePair<string,object>> propertyNameValue) where T : class
        {
            propertyNameValue.ToList().ForEach(x=>source.SetPropertyValueOfObject(x.Key, x.Value));
        }

        public static void SetPropertyValue<T>(this IList<T> source, IList<KeyValuePair<string, object>> propertyNameValue) where T : class
        {
            source?.ToList()?.ForEach(item=> propertyNameValue.ToList().ForEach(x => item.SetPropertyValueOfObject(x.Key, x.Value)));
        }


        public static void SetPropertyValue<T>(this IList<T> source, string propertyName, object propertValue) where T : class
        {
            source?.ToList()?.ForEach(x => x.SetPropertyValueOfObject(propertyName, propertValue));
        }


        private static void SetPropertyValueOfObject(this object source, string propertyName, object propertValue)
        {
            try
            {
                System.Reflection.PropertyInfo propertyInfo = source?.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(source, propertValue, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static object GetPropertyValue(this object source, string propertyName)
        {
            object value = null;
            try
            {
                System.Reflection.PropertyInfo propertyInfo = source?.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    value = propertyInfo.GetValue(source);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return value;
        }


        public static bool IsAnyCollectionPropertyContainValue<T>(this T source, params string[] propertyToBeExcludes) where T : class
        {
            bool result = false;
            try
            {
                var properties = source?.GetType().GetProperties()?.ToList();
                properties?.ForEach(x =>
                {
                    bool isValidationRequired = true;
                    if (propertyToBeExcludes.Length > 0 && propertyToBeExcludes.Contains(x.Name))
                        isValidationRequired = false;


                    if (isValidationRequired)
                    {
                        var type = x?.PropertyType.Name;
                        if (type != null && type.ToLower().Contains("icollection"))
                        {
                            var value = x.GetValue(source);
                            if ((int?)value.GetType().GetProperty("Count").GetValue(value) > 0)
                                result = true;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        public static String Serialize<T>(this T source, SerializationType type, bool ignoreNullValueProperty = false) where T : class
        {
            if (type == SerializationType.XML)
                return SerializeToXml(source);
            else if (type == SerializationType.JSON)
                return SerializeToJson(source, ignoreNullValueProperty);
            else
                return string.Empty;
        }


        public static T[] ToArray<T>(this T source) where T : class
        {
            return new T[] { source };
        }


        public static T DeSerialize<T>(this string source, SerializationType type) where T : class
        {
            if (type == SerializationType.XML)
                return null;
            else if (type == SerializationType.JSON)
                return DeSerializeToJson<T>(source);
            else
                return null;
        }


        private static string SerializeToXml<T>(T source) where T : class
        {
            string xml = null;
            using (StringWriter sw = new StringWriter())
            {


                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(sw, source);
                try
                {
                    xml = sw.ToString();


                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return xml;
        }


        private static string SerializeToJson<T>(T source, bool ignoreNullValueProperty = false) where T : class
        {
            if (ignoreNullValueProperty)
                return JsonConvert.SerializeObject(source, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            else
                return JsonConvert.SerializeObject(source, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

       /// <summary>
       /// For Audit JSON Serialize
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="source"></param>
       /// <param name="ignoreNullValueProperty"></param>
       /// <returns></returns>
        private static string AuditSerializeToJson<T>(T source, bool ignoreNullValueProperty = false) where T : class
        {
            if (ignoreNullValueProperty)
                return JsonConvert.SerializeObject(source, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    //ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            else
                return JsonConvert.SerializeObject(source, new JsonSerializerSettings
                {
                    //ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

        private static T DeSerializeToJson<T>(string source) where T : class
        {
            return JsonConvert.DeserializeObject<T>(source);
        }


        public static T XMLToObject<T>(string xml)
        {


            var serializer = new XmlSerializer(typeof(T));


            using (var textReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(textReader))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }
        public static T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }


        public static bool IsList(this object o)
        {
            if (o == null) return false;
            return o is IList &&
            o.GetType().IsGenericType &&
            o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsEnumerable(this object o)
        {
            if (o == null) return false;
            return o is IEnumerable &&
            o.GetType().IsGenericType &&
            o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));
        }


        public static bool IsDictionary(this object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
            o.GetType().IsGenericType &&
            o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }


        public static string SerializeAttribute<T, CustAttribute>(this T source, string attributePropertyName)
        where T : class
        where CustAttribute : class
        {
            try
            {
                Dictionary<string, object> _dict = new Dictionary<string, object>();
                List<string> lstString = null;
                if (source != null && (source.IsList() || source.IsEnumerable()))
                {
                    lstString = new List<string>();
                    foreach (var item in (IList)source)
                    {
                        lstString.Add(item.SerializeAttribute<object, CustAttribute>(attributePropertyName));
                    }
                    return SerializeToJson(lstString);
                }
                else
                {
                    PropertyInfo[] props = source.GetType().GetProperties();
                    object[] attrs = null;
                    CustAttribute attrTyp = null;
                    object prpValue = null;
                    string attrName = string.Empty;

                    foreach (PropertyInfo prop in props)
                    {
                        attrs = prop.GetCustomAttributes(typeof(CustAttribute), false);
                        foreach (object attr in attrs)
                        {
                            attrTyp = attr as CustAttribute;

                            if (attrTyp != null)
                            {
                                attrName = ((CustAttribute)attrTyp).GetPropertyValue(attributePropertyName).ToString();

                                if (((typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) || typeof(IList).IsAssignableFrom(prop.PropertyType)) && !typeof(string).IsAssignableFrom(prop.PropertyType)) || prop.PropertyType.Name.Contains("ICollection"))
                                {
                                    lstString = new List<string>();
                                    var prpItems = source.GetPropertyValue(prop.Name);
                                    if (prpItems != null)
                                    {
                                        foreach (var item in (IList)prpItems)
                                        {
                                            lstString.Add(item.SerializeAttribute<object, CustAttribute>(attributePropertyName));
                                        }
                                    }

                                    prpValue = string.Format("[{0}]", string.Join(',', lstString));//replace
                                }
                                else
                                    prpValue = source.GetPropertyValue(prop.Name);

                                if (attr.GetType() == typeof(AuditNameAttribute))
                                    prpValue = FormatPropertyValue((AuditNameAttribute)attr, prpValue);

                               _dict.Add(attrName, System.Net.WebUtility.HtmlEncode(prpValue?.ToString()));
                            }
                        }
                    }
                    return SerializeToJson(_dict);//replace
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Audit JSON Serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="CustAttribute"></typeparam>
        /// <param name="source"></param>
        /// <param name="attributePropertyName"></param>
        /// <returns></returns>
        public static string AuditSerializeAttribute<T, CustAttribute>(this T source, string attributePropertyName)
        where T : class
        where CustAttribute : class
        {
            try
            {
                Dictionary<string, object> _dict = new Dictionary<string, object>();
                List<string> lstString = null;
                if (source != null && (source.IsList() || source.IsEnumerable()))
                {
                    lstString = new List<string>();
                    foreach (var item in (IList)source)
                    {
                        lstString.Add(item.AuditSerializeAttribute<object, CustAttribute>(attributePropertyName));
                    }
                    return AuditSerializeToJson(lstString);
                }
                else
                {
                    PropertyInfo[] props = source.GetType().GetProperties();
                    object[] attrs = null;
                    CustAttribute attrTyp = null;
                    object prpValue = null;
                    string attrName = string.Empty;

                    foreach (PropertyInfo prop in props)
                    {
                        attrs = prop.GetCustomAttributes(typeof(CustAttribute), false);
                        foreach (object attr in attrs)
                        {
                            attrTyp = attr as CustAttribute;

                            if (attrTyp != null)
                            {
                                attrName = ((CustAttribute)attrTyp).GetPropertyValue(attributePropertyName).ToString();

                                if (((typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) || typeof(IList).IsAssignableFrom(prop.PropertyType)) && !typeof(string).IsAssignableFrom(prop.PropertyType)) || prop.PropertyType.Name.Contains("ICollection"))
                                {
                                    lstString = new List<string>();
                                    var prpItems = source.GetPropertyValue(prop.Name);
                                    if (prpItems != null)
                                    {
                                        foreach (var item in (IList)prpItems)
                                        {
                                            lstString.Add(item.AuditSerializeAttribute<object, CustAttribute>(attributePropertyName));
                                        }
                                    }

                                    prpValue = string.Format("[{0}]", string.Join(',', lstString));//replace
                                }
                                else
                                    prpValue = source.GetPropertyValue(prop.Name);

                                if (attr.GetType() == typeof(AuditNameAttribute))
                                    prpValue = FormatPropertyValue((AuditNameAttribute)attr, prpValue);

                                _dict.Add(attrName, System.Net.WebUtility.HtmlEncode(prpValue?.ToString()));
                            }
                        }
                    }
                    return AuditSerializeToJson(_dict);//replace

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static object FormatPropertyValue(AuditNameAttribute attr, object prpvalue)
        {
            if (!string.IsNullOrEmpty(attr.Format) && attr.FormatDataType != AuditNameformatDataType.None && attr.FormatDataType == AuditNameformatDataType.DateTime && prpvalue!=null)
            {
                switch (attr.FormatDataType)
                {
                    case AuditNameformatDataType.DateTime:
                        prpvalue = Convert.ToDateTime(prpvalue).ToString(attr.Format.ToString(), CultureInfo.InvariantCulture);
                        break;
                }
            }
            return prpvalue;
        }

        public static string ReplaceJsonProp(string dictionary, Dictionary<string, string> values)
        {
            var output = values.Aggregate(dictionary, (current, replacement) => current.Replace(replacement.Key, replacement.Value));
            return output;
        }

        public static bool HasItems<T>(this IEnumerable<T> source)
        {
            return source?.Any() ?? false;
        }

        public static MemoryCacheEntryOptions CacheExpiry(double absoluteExpiration, double slidingExpiration)
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(absoluteExpiration), //Gets or sets an absolute expiration date for the cache entry.
                Priority = CacheItemPriority.Normal,
                SlidingExpiration = TimeSpan.FromMinutes(slidingExpiration) // Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed
            };
        }

    }
}

