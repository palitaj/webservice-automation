using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WSAutomation
{
    /// <summary>
    /// Summary description for WSAutomation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSAutomation : System.Web.Services.WebService
    {
        Class.ClassEmail sendemail = new Class.ClassEmail();
        [WebMethod]
        public string Journey2ThankyouRegister(string JBkey, string email, string phone, string firstname, string lastname, string dealer_name, string branch_name, string model)
        {
            return sendemail.ThankyouEmail2(JBkey, email, phone, firstname, lastname, dealer_name, branch_name, model);
        }
    }
}
