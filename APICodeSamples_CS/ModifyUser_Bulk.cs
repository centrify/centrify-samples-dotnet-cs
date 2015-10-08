//Expected CSV format 
//Username,AccountEnabled,AccountStatus,NewPass,ConfirmNewPass
//testuser@domain.com,true,Locked,newpass1,newpass1


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Web.Script.Serialization; //Must include System.Web.Extensions as a referance.
using APICodeSamples.Classes; //Centrify_API_Interface class location.

namespace APICodeSamples
{
    class ModifyUser_Bulk
    {
        public static string CentLoginURL = "https://cloud.centrify.com/security/login";
        public static string CentCreateUserURL = "https://pod.centrify.com/cdirectoryservice/createuser";
        public static string CentQueryURL = "https://pod.centrify.com/RedRock/query";
        public static string CentModifyUserURL = "https://pod.centrify.com/cdirectoryservice/changeuser";
        public static string CentSetUserURL = "https://pod.centrify.com/cdirectoryservice/SetUserState";
        public static string CentSetPassURL = "https://pod.centrify.com/UserMgmt/ResetUserPassword";

        public static string UserName = "user@domain.com";
        public static string Password = "userPass";

        public string strUuid = "";

        public void ModifyUser_Bulk()
        {
            List<string[]> modifyUsers = ParseCSV(@"c:\newUsers.csv");

            string loginJSON = "{user:'" + UserName + "', password:'" + Password + "'}";
            Centrify_API_Interface centLogin = new Centrify_API_Interface().MakeRestCall(CentLoginURL, loginJSON);

            foreach (var user in modifyUsers)
            {
                string strModUserName = (string)user.GetValue(0);
                //A User GUID is needed to modify a user so the user will first need to be queried unless the GUID is hardcoded
                string strQueryJSON = @"{""Script"":""select * from dsusers where SystemName = '" + strModUserName + @"'""}";

                Centrify_API_Interface centQueryUser = new Centrify_API_Interface().MakeRestCall(CentQueryURL, strQueryJSON, centLogin.returnedCookie);
                var jssFindUser = new JavaScriptSerializer();
                Dictionary<string, dynamic> centQueryUser_Dict = jssFindUser.Deserialize<Dictionary<string, dynamic>>(centQueryUser.returnedResponse);

                if (centQueryUser_Dict["success"].ToString() == "True")
                {
                    Console.WriteLine("User Found:");

                    ArrayList centFindUser_Results = centQueryUser_Dict["Result"]["Results"];
                    dynamic centFindUser_Results_Column = centFindUser_Results[0];
                    Dictionary<string, dynamic> centFindUser_Results_Row = centFindUser_Results_Column["Row"];

                    strUuid = centFindUser_Results_Row["InternalName"];
                }
                else
                {
                    Console.WriteLine("Error Running Query: " + centQueryUser_Dict["Message"].ToString());
                }

                if (strUuid != "")
                {
                    bool bDisableAccount = (bool)user.GetValue(1);
                    string strState = (string)user.GetValue(2);
                    string strNewPass = (string)user.GetValue(3);
                    string strConfirmNewPass = (string)user.GetValue(4);

                    string strModifyUserJSON = @"{""ID"":""" + strUuid + @""", ""enableState"":" + bDisableAccount + @",""state"":""" + strState + @"""}";
                    Centrify_API_Interface centSetUser = new Centrify_API_Interface().MakeRestCall(CentSetUserURL, strModifyUserJSON);
                    var jss = new JavaScriptSerializer();
                    Dictionary<string, dynamic> centSetUser_Dict = jss.Deserialize<Dictionary<string, dynamic>>(centSetUser.returnedResponse);

                    if (centSetUser_Dict["success"].ToString() == "True" && centSetUser_Dict["success"].ToString() == "True")
                    {
                        if (strNewPass != null)
                        {
                            string strSetPassJSON = @"{""ID"":""" + strUuid + @""",""ConfrimPassword"":""" + strConfirmNewPass + @""",""newPassword"":""" + strNewPass + @"""}";
                            Centrify_API_Interface centSetPass = new Centrify_API_Interface().MakeRestCall(CentSetPassURL, strSetPassJSON);
                            Dictionary<string, dynamic> centSetPass_Dict = jss.Deserialize<Dictionary<string, dynamic>>(centSetPass.returnedResponse);

                            if (centSetPass_Dict["success"].ToString() == "True")
                            {
                                Console.WriteLine("User Updated.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to Set Password: " + centSetPass.returnedResponse);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to Modify user: " + centSetUser.returnedResponse);
                    }
                }
            }
        }
        public List<string[]> ParseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return parsedData;
        }
    }
}
