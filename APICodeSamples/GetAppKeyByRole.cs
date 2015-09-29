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
    class GetAppKeyByRole
    {
        public static string CentLoginURL = "https://cloud.centrify.com/security/login";
        public static string CentGetRoleAppsURL = "https://pod.centrify.com/SaasManage/GetRoleApps?role=";

        public static string UserName = "user@domain.com";
        public static string Password = "userPass";

        public List<string> GetAppKey(string strRole)
        {
            string loginJSON = "{user:'" + UserName + "', password:'" + Password + "'}";
            Centrify_API_Interface centLogin = new Centrify_API_Interface().MakeRestCall(CentLoginURL, loginJSON);

            string UpdateAppJson = "";
            Centrify_API_Interface centGetAppKey = new Centrify_API_Interface().MakeRestCall(CentGetRoleAppsURL + strRole, UpdateAppJson, centLogin.returnedCookie);
            var jssFindUser = new JavaScriptSerializer();
            Dictionary<string, dynamic> centGetAppKey_Dict = jssFindUser.Deserialize<Dictionary<string, dynamic>>(centGetAppKey.returnedResponse);

            ArrayList aApps = centGetAppKey_Dict["Result"];

            List<string> appKeys = new List<string>();
            int iCount = 0;

            foreach (Dictionary<string, dynamic> row in aApps)
            {
                appKeys[iCount] = row["ID"];
            }

            return appKeys;
        }
    }
}
