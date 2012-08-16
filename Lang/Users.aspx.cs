using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using Data;

namespace Lang
{
    public partial class Users : System.Web.UI.Page
    {
        public List<User> users = new List<User>();

        protected void Page_Load(object sender, EventArgs e)
        {
            users = UserManager.GetAllUsers();
        }

        [WebMethod]
        public static void UpdateUserName(int userid, string value)
        {
            var user = UserManager.GetUser(userid);
            user.name = value;
            UserManager.UpdateUser(user);
        }

        [WebMethod]
        public static void UpdateUserPassword(int userid, string value)
        {
            var user = UserManager.GetUser(userid);
            user.password = value;
            UserManager.UpdateUser(user);
        }

        [WebMethod]
        public static void UpdateUserRole(int userid, bool value)
        {
            var user = UserManager.GetUser(userid);
            user.role = value ? Role.ADMIN : Role.USER;
            UserManager.UpdateUser(user);
        }

        [WebMethod]
        public static void DeleteUser(int userid)
        {
            UserManager.DeleteUser(userid);
        }
    }
}