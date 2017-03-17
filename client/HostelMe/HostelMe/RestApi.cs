using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HostelMe
{
    public class RestApi
    {
        public RestApi(){}

        public async Task<string> GetDataAsync()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(Constants.RestUrl));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Answer: " + ex.ToString());
            }
            return null;
        }
    }
}
