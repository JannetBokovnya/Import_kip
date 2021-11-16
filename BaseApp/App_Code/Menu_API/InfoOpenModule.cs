using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InfoOpenModule
/// </summary>
public class InfoOpenModule
{
    private string window;
    private string title;
    private bool isChecked;

    public string Window { get { return window; } set { window = value; } }
    public string Title { get { return title; } set { title = value; } }
    public bool IsChecked { get { return isChecked; } set { isChecked = value; } }
	
    public InfoOpenModule(string window, string title)
	{
        this.window = window;
        this.title = title;
        this.isChecked = false;
    }
}