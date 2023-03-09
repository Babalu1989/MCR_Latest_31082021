using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    SimpleBL OBJbL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["LOGIN_TYPE"] != null)
                GetMenuData_StatusWise(Convert.ToString(Session["LOGIN_TYPE"]));
            else
                divMenu.Visible = false;
        }
    }

    private void GetMenuData_StatusWise(string _sRoleCode)
    {
        DataTable _dtMenuData = new DataTable();
        _dtMenuData = OBJbL.GetMenuData_RoleWise(_sRoleCode, Session["COMPANY"].ToString()); //AllClassQuery.GetMenuData_RoleWise(_sRoleCode);

        if (_dtMenuData.Rows.Count > 0)
        {
            DataRow[] drowpar = _dtMenuData.Select("PARENT_ID=" + 0);

            foreach (DataRow dr in drowpar)
            {
                Menu_One.Items.Add(new MenuItem(dr["PAGE_TITLE"].ToString(), dr["PAGE_ID"].ToString(), "", dr["NAVIGATE_URL"].ToString()));
            }

            foreach (DataRow dr in _dtMenuData.Select("PARENT_ID >" + 0))
            {
                MenuItem mnu = new MenuItem(dr["PAGE_TITLE"].ToString(), dr["PAGE_ID"].ToString(), "", dr["NAVIGATE_URL"].ToString());
                Menu_One.FindItem(dr["PARENT_ID"].ToString()).ChildItems.Add(mnu);
            }

            MenuItemCollection menuItems = Menu_One.Items;

            MenuItem adminItem = new MenuItem();

            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.ChildItems.Count == 0)
                {
                    adminItem = menuItem;
                    menuItems.Remove(adminItem);
                    break;
                }
            }
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.ChildItems.Count == 0)
                {
                    adminItem = menuItem;
                    menuItems.Remove(adminItem);
                    break;
                }
            }
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.ChildItems.Count == 0)
                {
                    adminItem = menuItem;
                    menuItems.Remove(adminItem);
                    break;
                }
            }
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.ChildItems.Count == 0)
                {
                    adminItem = menuItem;
                    menuItems.Remove(adminItem);
                    break;
                }
            }            
        }
    }

    protected void btnImage_Click(object sender, ImageClickEventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
        Response.Redirect("../Default.aspx");
    }
    protected void btnHome_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
    }
}
