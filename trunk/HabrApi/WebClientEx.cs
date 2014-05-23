using System;
using System.Net;

namespace HabrApi
{
    /// <summary>
    /// Extended WebClient.
    /// </summary>
    public class WebClientEx : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest) base.GetWebRequest(address);
            if (request != null)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }
            return request;
        }
    }
}