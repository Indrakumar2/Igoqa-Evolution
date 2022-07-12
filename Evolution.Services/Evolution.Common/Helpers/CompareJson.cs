using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

namespace Evolution.Common.Helpers
{
    public static class CompareJson
    { 
        public static string CompareObjects(string sourceJsonString, string targetJsonString)
        {
            Dictionary<string, object> diffValue = new Dictionary<string, object>();
            JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(sourceJsonString);
            JObject targetJObject = JsonConvert.DeserializeObject<JObject>(targetJsonString);
            if (!JToken.DeepEquals(sourceJsonString, targetJsonString))
            {
                List<string> auditConstantToNotCompare = new List<string>() { "Evo Id", "Charge Expense Type", "Contact Type" };   // These needs to be removed once we stop syncing data
                foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
                {
                    JProperty targetProp = targetJObject.Property(sourceProperty.Key);
                    if (!auditConstantToNotCompare.Contains(sourceProperty.Key)) // These needs to be removed once we stop syncing data
                    {
                        if (!JToken.DeepEquals(sourceProperty.Value, targetProp?.Value))
                        {
                            diffValue.Add(sourceProperty.Key, new string[] { WebUtility.HtmlDecode(sourceProperty.Value?.ToString()), WebUtility.HtmlDecode(targetProp?.Value?.ToString()) });
                        }
                    }
                    else
                    {
                        diffValue.Add(sourceProperty.Key, new string[] { WebUtility.HtmlDecode(sourceProperty.Value?.ToString()), WebUtility.HtmlDecode(targetProp?.Value?.ToString()) });
                    }
                }
            }
            return JsonConvert.SerializeObject(diffValue);
        }

        public static string CompareVisitObject(string sourceJsonString, string targetJsonString, List<string> auditConstantToNotCompare)
        {
            Dictionary<string, object> diffValue = new Dictionary<string, object>();
            JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(sourceJsonString);
            JObject targetJObject = JsonConvert.DeserializeObject<JObject>(targetJsonString);
            if (!JToken.DeepEquals(sourceJsonString, targetJsonString))
            {
                if (auditConstantToNotCompare?.Count > 0)
                    auditConstantToNotCompare.AddRange(new List<string>() { "Evo Id", "Charge Expense Type", "Contact Type" });
                else
                    auditConstantToNotCompare = new List<string>() { "Evo Id", "Charge Expense Type", "Contact Type" };

                foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
                {
                    JProperty targetProp = targetJObject.Property(sourceProperty.Key);
                    // These needs to be removed once we stop syncing data

                    if (!auditConstantToNotCompare.Contains(sourceProperty.Key)) // These needs to be removed once we stop syncing data
                    {
                        if (!JToken.DeepEquals(sourceProperty.Value, targetProp?.Value))
                        {
                            diffValue.Add(sourceProperty.Key, new string[] { WebUtility.HtmlDecode(sourceProperty.Value?.ToString()), WebUtility.HtmlDecode(targetProp?.Value?.ToString()) });
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(diffValue);
        }
    }
}
