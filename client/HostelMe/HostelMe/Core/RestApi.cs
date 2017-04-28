using System;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace HostelMe
{
    internal static class Extensions
    {
        internal static async Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request, int msec)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            using (token.Register(() => request.Abort(), useSynchronizationContext: false))
            {
                new Task(() =>
                {
                    Task.Delay(msec).ContinueWith(_ => 
                    cancelTokenSource.Cancel());                    
                }).Start();

                WebResponse response;
                try
                {
                    response = await request.GetResponseAsync().ConfigureAwait(false);
                    return (HttpWebResponse)response;
                }
                catch (Exception ex)
                {
                    if (token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(ex.Message, ex, token);
                    }
                    throw; // cancellation hasn't been requested, rethrow the original WebException
                }
            }
        }
    }
        
    public sealed class RestApi
    {
        public async Task<string> GetDataAsync()
        {           
            string uriStr = Constants.RestUrl;
            string version = DB.GetLastDBVersion();
            if (version != null && version.Length != 0)
            {
                uriStr = string.Format("{0}?{1}={2}", uriStr, Constants.Version, version);
            }
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(uriStr));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                // Send the request to the server and wait for the response:
                using (WebResponse response = await Extensions.GetResponseAsync(request, Constants.RequestDelay))
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        string data = reader.ReadToEnd();
                        Log.log.WriteLine("Request status: DATA loaded");
                        return data;
                    }
                }
            }
            catch(OperationCanceledException cancelEx)
            {
                Log.log.WriteLine("Request status: aborted after 10 sec\n" + cancelEx.ToString());
            }
            catch (Exception ex)
            {
                Log.log.WriteLine("Answer: " + ex.ToString());
            }
            return null;
        }
    }
}
