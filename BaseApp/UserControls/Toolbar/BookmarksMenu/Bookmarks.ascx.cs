using App_Code.SessionStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class UserControls_Menu_BookmarksMenu_Bookmarks : System.Web.UI.UserControl
{
    public event BookmarkChangedEventHandler HiddenFieldChanged;
    public delegate void BookmarkChangedEventHandler(object sender, BookmarkEventArgs e);

    public event BookmarkAddedEventHandler BookmarkAdded;
    public delegate void BookmarkAddedEventHandler(object sender, BookmarkEventArgs e);

    public event NotificationCheckerEventHandler NotificationChecker;
    public delegate void NotificationCheckerEventHandler(object sender);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hdnUser.Value = LoginSession.UserGuid;

            CheckBookmarks();
        }
    }

    protected void upnlBookmarkMenu_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(hdnNewBookmark.Value))
        {
            if (hdnNewBookmark.Value != "BookmarkIsDeletedCheckPls")
            {
                Bookmark bookmark = Bookmark.FromJSON(hdnNewBookmark.Value);
                AddNewBookMark(bookmark);
                CheckBookmarks();
                hdnAddedBookmark.Value = string.Empty;
                upnlBookmarkMenu.Update();
            }
            else
            {
                CheckBookmarks();
                hdnNewBookmark.Value = string.Empty;
                upnlBookmarkMenu.Update();
            }
        }

    }

    public void CheckBookmarks()
    {
        List<Bookmark> lstBookmarks = DbConnMenu.GetBookmarks(UserName);

        rmBookmarks.Items[0].Items.Clear();

        RadMenuItem rmiAddBookmark = new RadMenuItem();
        //rmiAddBookmark.Text = "Добавить закладку";  //GetWindowName()
        rmiAddBookmark.Text = this.GetLocalResourceObject("AddBookmark").ToString();
        rmiAddBookmark.Attributes.Add("onclick", "getStateForBookmark(); toOpen=true; ShowBookmarkPannel(toOpen); SetTitleToBookmark(GetWindowTitle());");

        rmBookmarks.Items[0].Items.Add(rmiAddBookmark);

        RadMenuItem rmiSeparator = new RadMenuItem();
        rmiSeparator.IsSeparator = true;
        rmBookmarks.Items[0].Items.Add(rmiSeparator);

        foreach (Bookmark bookmark in lstBookmarks)
        {
            RadMenuItem tmpRMI = new RadMenuItem();
            tmpRMI.Text = bookmark.AppTitle;
            tmpRMI.Value = bookmark.AppName;
            tmpRMI.Attributes.Add("idBookmark", bookmark.LinkKey.ToString ());
            tmpRMI.Attributes.Add("onclick", "javascript: sendEvent('LOAD_STATE', '" + bookmark.StateID.ToString().Replace("\r\n", "") + "', '" + bookmark.AppName + "'); ");
            tmpRMI.Attributes.Add("oncontextmenu", "javascript: setHiddenField('" + bookmark.LinkKey.ToString().Replace("\r\n", "") + "'); showCMBookmarkMenu(event); return false;");
            rmBookmarks.Items[0].Items.Add(tmpRMI);
            if (this.HiddenFieldChanged != null)
            {
                BookmarkEventArgs a = new BookmarkEventArgs();
                a.IsChanged = true;
                this.HiddenFieldChanged(this, a);
            }
        }
    }

    public string UserName
    {
        get { return LoginSession.UserName; }
        set { LoginSession.UserName = value; }
    }

    public void AddNewBookMark(Bookmark bookmark)
    {
        if (this.BookmarkAdded != null)
        {
            BookmarkEventArgs a = new BookmarkEventArgs();
            a.NewBookmark = bookmark;
            this.BookmarkAdded(this, a);
        }
    }

    protected void cmCMBookmark_ItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
        if (e.Item.Text == GetLocalResourceObject("rmiDeleteResource1.Text").ToString())
        {
            DbConnMenu.DeleteBookmark(Convert.ToInt32(hdnMenuItemSelected.Value));
            CheckBookmarks();
        }
    }
    //protected void rmNotification_OnItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    //{
    //    //foreach (RadMenuItem item in rmNotification.Items[0].Items)
    //    //{
    //    //    string jndcjn = item.Attributes["key"].ToString();
    //    //    DbConnMenu.DeliveredNotification(item.Attributes["key"].ToString());
    //    //}
    //}
}

public class BookmarkEventArgs : EventArgs
{
    // this is a string value I will set using a dropdownlist
    public bool IsChanged { get; set; }
    public Bookmark NewBookmark { get; set; }
    public string str { get; set; }
}

public class BookmarkBox : ITemplate
{
    System.Web.UI.WebControls.Button btnDelete = new System.Web.UI.WebControls.Button();
    System.Web.UI.WebControls.Label lblCaption = new System.Web.UI.WebControls.Label();

    public void InstantiateIn(System.Web.UI.Control container)
    {
        btnDelete.ID = "btnDelete";
        btnDelete.Text = "Hello";
        btnDelete.DataBinding += new EventHandler(btnDelete_DataBinding);

        lblCaption.ID = "lblCaption";
        container.Controls.Add(btnDelete);
    }

    public string Caption
    {
        get { return lblCaption.Text; }
        set { lblCaption.Text = value; }
    }

    private void btnDelete_DataBinding(object sender, EventArgs e)
    {
        Button target = (Button)sender;
        RadMenuItem item = (RadMenuItem)target.BindingContainer;

        string itemText = (string)DataBinder.Eval(item, "Text");
        target.Text = itemText;
    }

}

class TextBoxTemplate : ITemplate
{
    public void InstantiateIn(Control container)
    {
        Label label1 = new Label();
        label1.ID = "ItemLabel";
        label1.Text = "Text";
        label1.Font.Size = 13;
        label1.Font.Bold = true;
        label1.DataBinding += new EventHandler(label1_DataBinding);
        container.Controls.Add(label1);
    }
    private void label1_DataBinding(object sender, EventArgs e)
    {
        Label target = (Label)sender;
        RadMenuItem item = (RadMenuItem)target.BindingContainer;
        string itemText = (string)DataBinder.Eval(item, "Text");
        target.Text = itemText;
    }
}



