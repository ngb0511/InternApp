using Base.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Intergration.Utilities
{
    public static class OutPutApi
    {
        public static List<T> OutPutList<T>(RequestResponse body)
        {
            if (body != null && body.StatusCode == Code.OK)
            {
                if (body.Content != null)
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<List<T>>(body.Content);
                        if (result != null)
                            return result;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error deserializing JSON content.", ex);
                    }
                }
            }
            return new List<T>();
        }

        public static T OutPut<T>(RequestResponse body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body), "RequestResponse body is null.");
            }

            if (body.StatusCode != Code.OK)
            {
                throw new InvalidOperationException($"Unexpected status code: {body.StatusCode}");
            }

            if (body.Content == null)
            {
                throw new InvalidOperationException("RequestResponse content is null.");
            }

            try
            {
                var result = JsonConvert.DeserializeObject<T>(body.Content);

                if (result == null)
                {
                    throw new InvalidOperationException("Deserialized result is null.");
                }

                return result;
            }
            catch (JsonException ex)
            {
                throw new Exception("Error deserializing JSON content.", ex);
            }
        }
    }
}
