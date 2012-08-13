using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}