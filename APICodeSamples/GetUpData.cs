using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization; //Must include System.Web.Extensions as a referance.
using APICodeSamples.Classes; //Centrify_API_Interface class location.

namespace APICodeSamples
{
    class GetUpData
    {
        public static string CentLoginURL = "https://cloud.centrify.com/security/login";
        public static string CentGetAppsURL = "https://pod.centrify.com/uprest/GetUPData";
        public static string CentPodURL = "https://pod.centrify.com";
        public static string CentRunAppURL = "https://pod.centrify.com/run?appkey=";

        //Hard coded as an example only. A UI should assist the user in login.
        public static string UserName = "user@domain.com";
        public static string Password = "userPass";

        public void GetApps()
        {
            var jss = new JavaScriptSerializer();

            string loginJSON = "{user:'" + UserName + "', password:'" + Password + "'}";
            Centrify_API_Interface centLogin = new Centrify_API_Interface().MakeRestCall(CentLoginURL, loginJSON);
            Dictionary<string, dynamic> loginData = jss.Deserialize<Dictionary<string, dynamic>>(centLogin.returnedResponse);

            string appsJSON = @"{""force"":""True""}";
            Centrify_API_Interface centGetApps = new Centrify_API_Interface().MakeRestCall(CentGetAppsURL, appsJSON, centLogin.returnedCookie);           
            Dictionary<string, dynamic> getAppsData = jss.Deserialize<Dictionary<string, dynamic>>(centGetApps.returnedResponse);

            var dApps = getAppsData["Result"]["Apps"];
            string strAuth = loginData["Result"]["Auth"];

            int iCount = 0;

            foreach (var app in dApps)
            {
                string strDisplayName = app["DisplayName"];
                string strAppKey = app["AppKey"];
                string strIcon = app["Icon"];

                AddUrls(strAppKey, strDisplayName, strIcon, iCount, strAuth);

                iCount++;
            }
        }

        protected void AddUrls(string strAppKey, string strName, string strIcon, int count, string strAuth)
        {
            //Sample code to dynamically display apps on a ASP.net page.
            /*HyperLink link = new HyperLink();
            link.ID = "CentrifyApp" + count;
            link.NavigateUrl = CentRunAppURL + strAppKey + "&Auth=" + strAuth;
            link.Text = strName;
            link.ImageUrl = CentPodURL + strIcon;
            link.ImageHeight = 75;
            link.ImageWidth = 75;


            if (count % 7 == 0)
            {
                Apps.Controls.Add(new LiteralControl("<br />"));
            }
            else
            {
                Apps.Controls.Add(new LiteralControl("&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;"));

            }

            Apps.Controls.Add(link);*/

            Console.WriteLine("App Name: " + strName + " App Key: " + strAppKey + " Icon Location: " + strIcon);
        }
    }
}
