using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization; //Must include System.Web.Extensions as a referance.
using APICodeSamples.Classes; //Centrify_API_Interface class location.

namespace APICodeSamples
{
    class CreateUser
    {
        public static string CentLoginURL = "https://cloud.centrify.com/security/login";
        public static string CentCreateUserURL = "https://pod.centrify.com/cdirectoryservice/createuser";

        public static string UserName = "user@domain.com";
        public static string Password = "userPass";

        public static string NewUserName = "newUser";
        public static string NewPassword = "newPass";

        public void DoCreateUser()
        {
            string loginJSON = "{user:'" + UserName + "', password:'" + Password + "'}";
            Centrify_API_Interface centLogin = new Centrify_API_Interface().MakeRestCall(CentLoginURL, loginJSON);

            string strCreateUserJSON = "{Name:'" + NewUserName + "', Mail:'" + NewUserName + "', Password:'" + NewPassword + "'}";
            Centrify_API_Interface centCreateUser = new Centrify_API_Interface().MakeRestCall(CentCreateUserURL, strCreateUserJSON, centLogin.returnedCookie);
            var jssAdvanceAuthPoll = new JavaScriptSerializer();
            Dictionary<string, dynamic> centCreateUser_Dict = jssAdvanceAuthPoll.Deserialize<Dictionary<string, dynamic>>(centCreateUser.returnedResponse);

            if (centCreateUser_Dict["Message"].ToString() != null)
            {
                Console.WriteLine("Error: " + centCreateUser_Dict["Message"].ToString());
            }
            else
            {
                Console.WriteLine("Create User Successful.");
            }
        }
    }
}
