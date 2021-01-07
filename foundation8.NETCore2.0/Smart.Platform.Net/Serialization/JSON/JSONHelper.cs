using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Smart.Platform.Net.Serialization.JSON
{
    /// <summary>
    /// A helper class for JSON.
    /// </summary>
    public class JSONHelper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONHelper"/> class.
        /// </summary>
        private JSONHelper() { }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Deserializes the automatic json.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns></returns>
        public static DataJSONWrapper DeserializeToJSON(String jsonString)
        {
            DataJSONWrapper r = new DataJSONWrapper();

            if (!string.IsNullOrEmpty(jsonString))
            {
                // TODO: Deprecated in .NET Core 1.0
                //JavaScriptSerializer s = new JavaScriptSerializer();
                //r = s.Deserialize<DataJSONWrapper>(jsonString);

                r = JsonConvert.DeserializeObject<DataJSONWrapper>(jsonString);
            }

            return r;
        }

        /// <summary>
        /// Serializes from json.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <returns></returns>
        public static String SerializeFromJSON(DataJSONWrapper jsonObject)
        {
            String r = string.Empty;


            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DataJSONWrapper));

                serializer.WriteObject(ms, jsonObject);

                r = System.Text.Encoding.UTF8.GetString((ms.ToArray()));
            }

            return r;
        }

        /// <summary>
        /// Decodes the json string.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns></returns>
        public static String DecodeJSONString(String jsonString)
        {
            String r = string.Empty;

            // Decode % encoding
            r = jsonString.Replace("%22", "\\\"");  // %22 = double quote

            return r;
        }

        #endregion
    }
}
