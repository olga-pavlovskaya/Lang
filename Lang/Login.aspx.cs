using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Lang
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = LoginUser.UserName,
                pass = LoginUser.Password;
            
            
            var user = Data.UserManager.GetUser(username, pass);
            if (user != null)
            {
                if (Request.QueryString["ReturnUrl"] != null)
                {

                    FormsAuthentication.RedirectFromLoginPage(username, false);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                }
            }
            else
            {
                
                LoginUser.FailureText = "Invalid username and password combination";
            }
        }
    }
}
