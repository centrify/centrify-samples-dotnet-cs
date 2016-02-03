using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization; //Must include System.Web.Extensions as a referance.
using APICodeSamples.Classes; //Centrify_API_Interface class location.

namespace APICodeSamples
{
    class ResetAppCredsForUser
    {
        public static string CentLoginURL = "https://cloud.centrify.com/security/login";
        public static string CentUpdateAppURL = "https://pod.centrify.com/saasManage/UpdateApplicationDE?_RowKey=";

        public static string UserName = "user@domain.com";
        public static string Password = "userPass";

        public void UpdateApp(string strNewUserName, string strNewPass, string strAppKey)
        {
            string loginJSON = "{user:'" + UserName + "', password:'" + Password + "'}";
            Centrify_API_Interface centLogin = new Centrify_API_Interface().MakeRestCall(CentLoginURL, loginJSON);

            string UpdateAppJson = @"{""UserNameStrategy"":""Fixed"",""Password"":""" + strNewPass + @""",""UserNameArg"":""" + strNewUserName + @"""}";
            Centrify_API_Interface centUpdateApp = new Centrify_API_Interface().MakeRestCall(CentUpdateAppURL + strAppKey, UpdateAppJson, centLogin.returnedCookie);
        }
    }
}
