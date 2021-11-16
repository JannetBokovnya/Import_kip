using System;
using System.Collections.Generic;
using Telerik.Web.UI.Skins;

public partial class UserControls_Panel_InfoPanel : System.Web.UI.UserControl
{
    public event EventHandler BookmarkDeleted = delegate {};

    public string GetListBoxControlId()
    {
        return radLstBox.ClientID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        radLstBox.CheckBoxes = IsCheckBox;
        cmCMBookmarkPanel.Items[0].Attributes.Add("onclick", "deleteBookmarkCM()");
    }

    public string Caption
    {
        get { return lblCaption.Text; }
        set { lblCaption.Text = value; }
    }

    public bool IsCheckBox { get; set; }

    public void CheckActiveWindows(List<string> lstWindows, List<string> lstTitles)
    {
        //для закладок
        radLstBox.Items.Clear();
        if (lstWindows.Count == lstTitles.Count)
        {
            Telerik.Web.UI.RadListBoxItem tmpListBoxItem;
            for (int i = 0; i < lstTitles.Count; i++)
            {
                tmpListBoxItem = new Telerik.Web.UI.RadListBoxItem() { Text = lstTitles[i], Value = lstWindows[i] };
                tmpListBoxItem.Attributes.Add("onmouseup", "javascript:sendEvent('OPEN_WINDOW', '{}', '" + lstWindows[i] + "');");
                radLstBox.Items.Add(tmpListBoxItem);
               
            }
        }
    }

    public void GetUserBookmarks(string strUser)
    {
        radLstBox.Items.Clear();
        List<Bookmark> lstBookmarks = DbConnMenu.GetBookmarks(strUser);
        foreach (Bookmark bookmark in lstBookmarks)
        {
            Telerik.Web.UI.RadListBoxItem tmpRMI = new Telerik.Web.UI.RadListBoxItem();
            tmpRMI.Text = bookmark.AppTitle;
            tmpRMI.Value = bookmark.AppName;
            tmpRMI.Attributes.Add("idBookmark", bookmark.StateID.ToString());
            //убрала, т.к. теперь обрабатываем клик в onclientselectedindexchanged ="OnClientItemClickNotification" - т.е. общее для уведомлений и закладок
            tmpRMI.Attributes.Add("onclick", "javascript: sendEvent('LOAD_STATE', '" + bookmark.StateID.ToString().Replace("\r\n", "") + "', '" + bookmark.AppName + "'); ");
            tmpRMI.Attributes.Add("oncontextmenu", "javascript: setBMHiddenField('" + bookmark.LinkKey.ToString().Replace("\r\n", "") + "'); showCMBookmarkPanelMenu(event); return false;");
            tmpRMI.Attributes.Add("cState", bookmark.StateID.ToString().Replace("\r\n", ""));
            tmpRMI.Attributes.Add("cAppName", bookmark.AppName);
            radLstBox.Items.Add(tmpRMI);
        }


       


    }

    public void AddUserBookmarks(Bookmark bookmark)
    {
        Telerik.Web.UI.RadListBoxItem tmpRMI = new Telerik.Web.UI.RadListBoxItem();
        tmpRMI.Text = bookmark.AppTitle;
        tmpRMI.Value = bookmark.AppName;
        tmpRMI.Attributes.Add("idBookmark", bookmark.StateID.ToString());

        radLstBox.Items.Insert(0,tmpRMI);
    }

    public string GetNumberElements()
    {
        return radLstBox.Items.Count.ToString();
    }

    public void AddElement(string text)
    {
        Telerik.Web.UI.RadListBoxItem tmpRMI = new Telerik.Web.UI.RadListBoxItem();
        tmpRMI.Text = text;
        radLstBox.Items.Add(tmpRMI);

    }

    protected void cmCMBookmark_ItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
        if (e.Item.Text == "Удалить")
        {
            DbConnMenu.DeleteBookmark(Convert.ToInt32(hdnBMMenuItemSelected.Value));
            //GetUserBookmarks(App_Code.SessionStorage.LoginSession.UserGuid);
            if (this.BookmarkDeleted != null)
            {
                this.BookmarkDeleted(this, EventArgs.Empty);
            }

        }
    }

}