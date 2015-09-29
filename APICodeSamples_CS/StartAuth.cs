using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Web.Script.Serialization; //Must include System.Web.Extensions as a referance.
using APICodeSamples.Classes; //Centrify_API_Interface class location.

namespace APICodeSamples
{
    class StartAuth
    {
        public String TenantId { get; set; }
        public String SessionId { get; set; }
        public ArrayList Challenges { get; set; }

        public static string CentStartAuthURL = "https://cloud.centrify.com/Security/StartAuthentication";

        //Hard coded as an example only. 
        public static string UserName = "user@domain.com";
        public static string Password = "userPass";

        public StartAuth StartAuth()
        {
            string strStartAuthJSON = @"{""User"":""" + UserName + @""", ""Version"":""1.0""}";
            Centrify_API_Interface centStartAuth = new Centrify_API_Interface().MakeRestCall(CentStartAuthURL, strStartAuthJSON);
            var jss = new JavaScriptSerializer();
            Dictionary<string, dynamic> centStartAuth_Dict = jss.Deserialize<Dictionary<string, dynamic>>(centStartAuth.returnedResponse);

            TenantId = centStartAuth_Dict["Result"]["TenantId"];
            SessionId = centStartAuth_Dict["Result"]["SessionId"];
            Challenges = centStartAuth_Dict["Result"]["Challenges"];

            return this;
        }
    }
}
