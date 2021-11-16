using App_Code.SessionStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_Toolbar_Toolbar : System.Web.UI.UserControl
{
    public string ModuleName { get; set; }
    protected string addedBookmarkOkMessage;
    protected string addedBookmarkNotOkMessage;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            redmineMenu.ModuleName = ModuleName;
            hdnUser.Value = LoginSession.UserGuid;
            userMenu.UserName = LoginSession.UserName;
            var localResource = GetLocalResourceObject("lbUsrFio");
            //if (localResource != null)
            //{
            //    laUserFio.Text = LoginSession.UserFIO;
            //}
            //else
            //{
            //    laUserFio.Text = LoginSession.UserFIO;
            //}
               

            //Получаем значения из ресурсов локализации
            addedBookmarkOkMessage = this.GetLocalResourceObject("addedBookmarkOkMessageResource1.Text").ToString();
            addedBookmarkNotOkMessage = this.GetLocalResourceObject("addedBookmarkNotOkMessageResource1.Text").ToString();
        }
    }

    protected void upnlUserMenu_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(hdnNewBookmark.Value))
        {
            if (hdnNewBookmark.Value != "BookmarkIsDeletedCheckPls")
            {
                Bookmark bookmark = Bookmark.FromJSON(hdnNewBookmark.Value);
                BookmarkMenu.AddNewBookMark(bookmark);
                BookmarkMenu.CheckBookmarks();
                hdnAddedBookmark.Value = string.Empty;
                upnlUserMenu.Update();
            }
            else
            {
                BookmarkMenu.CheckBookmarks();
                hdnNewBookmark.Value = string.Empty;
                upnlUserMenu.Update();
            }
        }

    }
}