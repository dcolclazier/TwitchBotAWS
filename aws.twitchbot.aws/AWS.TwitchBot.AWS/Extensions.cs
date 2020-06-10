using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AWS.TwitchBot.AWS
{
    public static class Extensions
    {

        public static string ToJsonString(this object that) => JsonConvert.SerializeObject(that, Formatting.Indented);
        public static bool IsNullOrEmpty(this string that) => string.IsNullOrEmpty(that);

        public static void ActIfNull(this object that, Action action)
        {
            if(that == null)
            {
                action?.Invoke();
            }
        }

        public static bool IsInteger(this string that, out int results)
        {
            if (that.IsNullOrEmpty())
            {
                results = 0;
                return false;
            }
            return int.TryParse(that, out results);
        }
    }
}
