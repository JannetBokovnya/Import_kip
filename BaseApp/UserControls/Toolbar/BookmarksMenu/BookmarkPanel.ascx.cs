using System;
using System.Web.UI.WebControls;

public partial class UserControls_Panel_Bookmark : System.Web.UI.UserControl
{
    public event EventHandler BookmarkAdded;
    private Bookmark _bookmark;
    public Bookmark AddedBookmark { get { return _bookmark; } set { _bookmark = value; } }

    public event EventHandler PageTitleUpdated;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnAddBookmark_Click(object sender, EventArgs e)
    {
        try
        {
            _bookmark = Bookmark.FromJSON ((Page.Master.FindControl ("hdnNewBookmark") as HiddenField).Value);
            _bookmark.AppTitle = txtComment.Text;

            DbConnMenu.AddBookmark (_bookmark);
            if (BookmarkAdded != null)
            {
                BookmarkAdded (this, EventArgs.Empty);
            }
        }
        catch
        {
            
        }
    }

}

