//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Collections;
//using System.Web.Script.Serialization; //Must include System.Web.Extensions as a referance.
//using APICodeSamples.Classes; //Centrify_API_Interface class location.

//namespace APICodeSamples
//{
//    class ModifyUser
//    {
//        public static string CentLoginURL = "https://cloud.centrify.com/security/login";
//        public static string CentCreateUserURL = "https://pod.centrify.com/cdirectoryservice/createuser";
//        public static string CentQueryURL = "https://pod.centrify.com/RedRock/query";
//        public static string CentModifyUserURL = "https://pod.centrify.com/cdirectoryservice/changeuser";
//        public static string CentSetUserURL = "https://pod.centrify.com/cdirectoryservice/SetUserState";
//        public static string CentSetPassURL = "https://pod.centrify.com/UserMgmt/ResetUserPassword";

//        public static string UserName = "user@domain.com";
//        public static string Password = "userPass";

//        //What to change on the account. This would typically be populated by a UI.
//        public static bool bDisableAccount = false; 
//        public static string strState = "Locked";
//        public static string strNewPass = "newpass1";
//        public static string strConfirmNewPass = "newpass1";

//        public string strUuid = "";

//        public void DoModifyUser()
//        {
//            string loginJSON = "{user:'" + UserName + "', password:'" + Password + "'}";
//            Centrify_API_Interface centLogin = new Centrify_API_Interface().MakeRestCall(CentLoginURL, loginJSON);

//            //A User GUID is needed to modify a user so the user will first need to be queried unless the GUID is hardcoded
//            string strQueryJSON = @"{""Script"":""select * from dsusers where SystemName = '" + UserName + @"';"",""Args"":{""PageNumber"":1,""PageSize"":10000,""Limit"":10000,""SortBy"":"""",""direction"":""False"",""Caching"":-1}}";

//            Centrify_API_Interface centQueryUser = new Centrify_API_Interface().MakeRestCall(CentQueryURL, strQueryJSON, centLogin.returnedCookie);
//            var jssFindUser = new JavaScriptSerializer();
//            Dictionary<string, dynamic> centQueryUser_Dict = jssFindUser.Deserialize<Dictionary<string, dynamic>>(centQueryUser.returnedResponse);

//            if (centQueryUser_Dict["success"].ToString() == "True")
//            {
//                Console.WriteLine("User Found:");

//                ArrayList centFindUser_Results = centQueryUser_Dict["Result"]["Results"];
//                dynamic centFindUser_Results_Column = centFindUser_Results[0];
//                Dictionary<string, dynamic> centFindUser_Results_Row = centFindUser_Results_Column["Row"];

//                strUuid = centFindUser_Results_Row["InternalName"];
//            }
//            else
//            {
//                Console.WriteLine("Error Running Query: " + centQueryUser_Dict["Message"].ToString());
//            }

//            if (strUuid != "")
//            {
//                string strModifyUserJSON = @"{""ID"":""" + strUuid + @""", ""enableState"":" + bDisableAccount + @",""state"":""" + strState + @"""}";
//                Centrify_API_Interface centSetUser = new Centrify_API_Interface().MakeRestCall(CentSetUserURL, strModifyUserJSON);
//                var jss = new JavaScriptSerializer();
//                Dictionary<string, dynamic> centSetUser_Dict = jss.Deserialize<Dictionary<string, dynamic>>(centSetUser.returnedResponse);

//                if (centSetUser_Dict["success"].ToString() == "True")
//                {
//                    if (strNewPass != null)
//                    {
//                        string strSetPassJSON = @"{""ID"":""" + strUuid + @""",""ConfrimPassword"":""" + strConfirmNewPass + @""",""newPassword"":""" + strNewPass + @"""}";
//                        Centrify_API_Interface centSetPass = new Centrify_API_Interface().MakeRestCall(CentSetPassURL, strSetPassJSON);
//                        Dictionary<string, dynamic> centSetPass_Dict = jss.Deserialize<Dictionary<string, dynamic>>(centSetPass.returnedResponse);

//                        if (centSetPass_Dict["success"].ToString() == "True")
//                        {
//                            Console.WriteLine("User Updated.");
//                        }
//                        else
//                        {
//                            Console.WriteLine("Failed to Set Password: " + centSetPass.returnedResponse);
//                        }
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Failed to Modify user: " + centSetUser.returnedResponse);
//                }
//            }
//        }
//    }
//}
