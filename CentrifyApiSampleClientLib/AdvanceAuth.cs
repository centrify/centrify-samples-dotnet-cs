using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization; //Must include System.Web.Extensions as a referance.
using APICodeSamples.Classes; //Centrify_API_Interface class location.

namespace APICodeSamples
{
    class AdvanceAuth
    {
        public String ResultSummery { get; set; }
        public Dictionary<string, dynamic> AdvanceAuth_Dict { get; set; }

        public static string CentAdvanceAuthURL = "https://cloud.centrify.com/Security/AdvanceAuthentication";

        public AdvanceAuth DoAdvanceAuth(string strSessionId, string strTenantId, bool bRememberMe, string strMechId, string strAction, string strSecondMechId = null, string strSecondAction = null)
        {
            string strAdvanceAuthJSON;

            //Multiple operations can be made at once
            if (strSecondMechId != null)
            {
                strAdvanceAuthJSON = @"{""TenantId"":""" + strTenantId + @""",""SessionId"":""" + strSessionId + @""",""PersistentLogin"":" + bRememberMe.ToString().ToLower() + @",""MultipleOperations"":[{""MechanismId"":""" + strSecondMechId + @""",""Action"":""" + strSecondAction + @"""},{""MechanismId"":""" + strMechId + @""",""Action"":""" + strAction + @"""}]}";
            }
            //Or Single Operations can be made
            else
            {
                strAdvanceAuthJSON = @"{""TenantId"":""" + strTenantId + @""",""SessionId"":""" + strSessionId + @""",""PersistentLogin"":" + bRememberMe.ToString().ToLower() + @",""MechanismId"":""" + strMechId + @""",""Action"":""" + strAction + @"""}";
            }

            Centrify_API_Interface centAdvanceAuth = new Centrify_API_Interface().MakeRestCall(CentAdvanceAuthURL, strAdvanceAuthJSON);
            var jssAdvanceAuth = new JavaScriptSerializer();
            Dictionary<string, dynamic> centAdvanceAuth_Dict = jssAdvanceAuth.Deserialize<Dictionary<string, dynamic>>(centAdvanceAuth.returnedResponse);

            ResultSummery = centAdvanceAuth_Dict["Result"]["Summary"].ToString();
            AdvanceAuth_Dict = centAdvanceAuth_Dict;

            return this;

        }
    }
}
