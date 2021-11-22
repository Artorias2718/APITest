using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITest.Utilities
{
    public class Utility
    {
        /// <summary>
        /// Serializes an object into a JSON string
        /// </summary>
        /// <param name="oObj"></param>
        /// <returns></returns>
        public static string JsonSerializeObject(object i_oObj)
        {
            string sTaskPayload = "";

            JsonSerializerSettings jSettings = new JsonSerializerSettings();
            jSettings.NullValueHandling = NullValueHandling.Ignore;

            string jJson = JsonConvert.SerializeObject(i_oObj, Formatting.None, jSettings);
            jJson = jJson.Replace(@"\\n", @"\n");

            sTaskPayload = jJson;

            return sTaskPayload;
        }

        /// <summary>
        /// Reserialize a JSON string into a JSON string in case
        /// you'd like to format it.
        /// </summary>
        /// <param name="oObj"></param>
        /// <returns></returns>
        public static string JsonSerializeObject(string i_oObj, bool indent = false)
        {
            JsonSerializerSettings jSettings = new JsonSerializerSettings();
            jSettings.NullValueHandling = NullValueHandling.Ignore;


            JToken oJson = JToken.Parse(i_oObj);
            string sJson = JsonConvert.SerializeObject(oJson, indent ? Formatting.Indented : Formatting.None);

            return sJson;
        }
    }
}
