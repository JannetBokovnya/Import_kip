using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_Panel_ContextNavigation : System.Web.UI.UserControl
{
    public event EventHandler ContextNavigationChanged;

    private const string IdThread = "NoThread";
    private const string IdMg = "NoMG";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            Dictionary<string, string> lstMg = DbConnMenu.GetListMG();

            foreach (KeyValuePair<string, string> mg in lstMg)
            {
                ddlMG.Items.Add(new RadComboBoxItem(mg.Value, mg.Key));
            }
            if (ddlMG.Items.Count == 1)
            {
                ddlMG.Items [0].Selected = true;
                FillThread();
            }
        }
    }

    protected void ddlMG_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillThread();
    }

    private void FillThread()
    {
        //очистить 
        SetNullThread();
        //получить перечень всех ниток

        if (ddlMG.SelectedValue != IdMg)
        {
            List<ThreadMG> lstThread = DbConnMenu.GetListThread(ddlMG.SelectedValue);
            foreach (ThreadMG thread in lstThread)
            {
                //ddlThread.Items.Add(new RadComboBoxItem(thread.NameThread, thread.IdMG + ";" + thread.NameThreadShorten));
                ddlThread.Items.Add(new RadComboBoxItem(thread.NameThreadShorten, thread.IdMG + ";" + thread.NameThreadShorten));
            }
        }
    }

    protected void ddlThread_SelectedIndexChanged(object sender, EventArgs e)
    {
        //получить перечень всех ниток
        if (ddlThread.SelectedValue.Split(';')[0] != IdThread)
        {
            Dictionary<string, string> lstKm = DbConnMenu.GetListKmStartEnd(ddlThread.SelectedValue.Split(';')[0]);

            txtKmStart.Text = lstKm["KmStart"].Replace(",",".");
            txtKmEnd.Text = lstKm["KmEnd"].Replace(",", ".");
        }
        else
        {
            SetNullKm();
        }
    }

    protected void btnSetConNav_Click(object sender, EventArgs e)
    {
        try
        {
            if ((ddlMG.SelectedValue != IdMg) && (ddlThread.SelectedValue != IdThread) && !String.IsNullOrEmpty(txtKmStart.Text) && !String.IsNullOrEmpty(txtKmEnd.Text))
            {
                ContextNavigation cn = new ContextNavigation(ddlMG.SelectedValue, ddlMG.SelectedItem.Text,
                    ddlThread.SelectedValue.Split(';')[0], ddlThread.SelectedItem.Text, ddlThread.SelectedValue.Split(';')[1],
                    txtKmStart.Text, txtKmEnd.Text);
                //записать в cookies
                HttpCookie cookie = Request.Cookies["ContextNavigation"] ?? new HttpCookie("ContextNavigation");

                cookie.Value = HttpUtility.UrlEncode(cn.ToJSON(), System.Text.Encoding.ASCII);
               
                Response.Cookies.Add(cookie);
                //послать контекст навигации
                if (ContextNavigationChanged != null)
                {
                    ContextNavigationChanged(this, EventArgs.Empty);
                }
            }
        }
        catch
        { }
    }

    private void SetNullKm()
    {
        //установить начальные значения
        txtKmStart.Text = "";
        txtKmEnd.Text = "";
    }

    private void SetNullThread()
    {
        ddlThread.Items.Clear();

        SetNullKm();
    }

}