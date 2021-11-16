using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Telerik.Web.UI;

public partial class UserControls_Menu_OpenedModulesMenu_OpenedModulesMenu : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DbConnMenu.LoadLeftMenuItems(ref rmOpenedModulesMain);
        }
    }

    public void CheckActiveWindows(List<InfoOpenModule> lstOpenModules)
    {
        if (lstOpenModules.Count > 0)
        {
            //находим активные окна

            foreach (Telerik.Web.UI.RadMenuItem rmItem in rmOpenedModulesMain.Items)
            {
                var resOpenedModulesActually = from openmodule in lstOpenModules
                                               where rmItem.Attributes["SubModules"].Contains(";" + openmodule.Window + ";")
                                               select openmodule;
                if (resOpenedModulesActually.Count() != rmItem.Items.Count)
                {
                    if (resOpenedModulesActually.Count() == 0)
                    {
                        //то мы проверяем или оно активное
                        if (rmItem.CssClass == rmItem.Attributes["CssActive"])
                        {
                            rmItem.CssClass = rmItem.Attributes["CssPassive"];
                            rmItem.Items.Clear();
                        }
                    }
                    else
                    {
                        if (rmItem.CssClass == rmItem.Attributes["CssPassive"])
                        {
                            rmItem.CssClass = rmItem.Attributes["CssActive"];
                        }
                        rmItem.Items.Clear();
                        foreach (InfoOpenModule opActually in resOpenedModulesActually)
                        {
                            rmItem.Items.Add(new RadMenuItem() { Text = opActually.Title, Value = opActually.Window });
                            rmItem.Items[rmItem.Items.Count - 1].Attributes.Add("onclick", "javascript:sendEvent('OPEN_WINDOW', '{}', '" + opActually.Window + "');");
                        }
                    }
                }

            }
        }
        else
        {
            var resActiveItems = from rmItem in rmOpenedModulesMain.Items
                                 where rmItem.CssClass == rmItem.Attributes["CssActive"]
                                 select rmItem;
            foreach (RadMenuItem rmItem in resActiveItems)
            {
                rmItem.CssClass = rmItem.Attributes["CssPassive"];
                rmItem.Items.Clear();
            }
        }
    }
}
