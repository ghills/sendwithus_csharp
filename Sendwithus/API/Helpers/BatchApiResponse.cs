﻿using System.Web.Script.Serialization;

namespace Sendwithus
{
    /// <summary>
    /// sendwithus BatchApiResponse class
    /// </summary>
    public class BatchApiResponse
    {
        public string path { get; set; }
        public int status_code { get; set; }
        public string method { get; set; }
        public object body { private get ; set; }

        /// <summary>
        /// Converts the batch API response body into an object of the given type
        /// </summary>
        /// <typeparam name="T">The type to convert the body to</typeparam>
        /// <returns>An object of the given type, based on the contents of the batch API response body</returns>
        public T GetBody<T>()
        {
            var serializer = new JavaScriptSerializer();
            var bodyString = serializer.Serialize(body);
            return (T)serializer.Deserialize(bodyString, typeof(T));
        }
    }
}
