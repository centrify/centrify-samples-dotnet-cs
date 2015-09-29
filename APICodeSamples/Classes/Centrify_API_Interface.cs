using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace APICodeSamples.Classes
{
    class Centrify_API_Interface
    {
        public CookieCollection returnedCookie { get; set; }
        public String returnedResponse { get; set; }

        public Centrify_API_Interface()
        {

        }

        public Centrify_API_Interface MakeRestCall(string CentEndPoint, string JSON_Payload, CookieCollection cookies = null)
        {
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(CentEndPoint);

            request.Method = "POST";
            request.ContentLength = 0;
            request.ContentType = "application/json";
            request.Headers.Add("X-CENTRIFY-NATIVE-CLIENT", "1");

            request.CookieContainer = new CookieContainer();

            if (cookies != null)
            {
                request.CookieContainer.Add(cookies);
            }

            if (!string.IsNullOrEmpty(JSON_Payload))
            {
                var encoding = new System.Text.UTF8Encoding();
                var bytes = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(JSON_Payload);
                request.ContentLength = bytes.Length;

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            using (var response = (System.Net.HttpWebResponse)request.GetResponse())
            {
                var responseValue = string.Empty;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                        using (var reader = new System.IO.StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                }

                returnedCookie = response.Cookies;
                returnedResponse = responseValue;

                return this;
            }
        }
    }
}
