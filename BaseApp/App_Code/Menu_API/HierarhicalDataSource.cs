using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Summary description for HierarhicalDataSource
/// </summary>
public class HierarchicalDataSource : List<LeftMenuItem>
{
    // This list holds all the items that come from the web service result
    private List<LeftMenuItem> unsortedList = new List<LeftMenuItem>();

    public List<Telerik.Web.UI.RadMenuItem> exampleMenuList = new List<Telerik.Web.UI.RadMenuItem>();

    public HierarchicalDataSource(List<LeftMenuItem> result)
    {
        // Create a new instance of the web service and get the data from the table
        unsortedList = result;
        // Get all the first level nodes. In our case it is only one - House M.D.
        var rootNodes = this.unsortedList.Where(x => x.ParentId == "");

        // Foreach root node, get all its children and add the node to the HierarchicalDataSource.
        // see bellow how the FindChildren method works
        foreach (LeftMenuItem node in rootNodes)
        {
            Telerik.Web.UI.RadMenuItem tmp = new Telerik.Web.UI.RadMenuItem();
            this.FindChildren(node, tmp);
            this.Add(node);
            //tmp.HorizontalAlignment = HorizontalAlignment.Left;
            //tmp.Header = node;

            tmp.Text = node.NodeText;
            tmp.Value = node.NodeUrl;
            //tmp.NavigateUrl = node.NodeUrl;
            tmp.Attributes.Add("onmouseup", node.NodeUrl);
            exampleMenuList.Add(tmp);
        }
    }

    public List<Telerik.Web.UI.RadMenuItem> TopMenuItems()
    {
        List<Telerik.Web.UI.RadMenuItem> res = new List<Telerik.Web.UI.RadMenuItem>();
        var rootNodes = this.unsortedList.Where(x => x.ParentId == "");
        foreach (LeftMenuItem node in rootNodes)
        {
            Telerik.Web.UI.RadMenuItem tmp = new Telerik.Web.UI.RadMenuItem();
            tmp.ToolTip = node.NodeText;
            tmp.Value = node.NodeUrl;
            tmp.CssClass = node.NodeStyle;
            tmp.ImageUrl = "~/Images/empty_foreground.png";
            tmp.Attributes.Add("CssPassive", tmp.CssClass);
            tmp.Attributes.Add("CssActive", tmp.CssClass + "Active");
            //надо найти всех деток
            var childNodes = this.unsortedList.Where(x => x.ParentId == node.NodeId);
            string strModules = ";"+node.NodeUrl+";";
            foreach (LeftMenuItem childNode in childNodes)
            {
                strModules += childNode.NodeUrl + ";";
            }
            tmp.Attributes.Add("SubModules", strModules);
            res.Add(tmp);           
        }
        return res;
    }

    private void FindChildren(LeftMenuItem item, Telerik.Web.UI.RadMenuItem tmp)
    {
        // find all the children of the item
        var children = unsortedList.Where(x => x.ParentId == item.NodeId && x.NodeId != item.NodeId);

        // add the child to the item's children collection and call the FindChildren recursively, in case the child has children
        foreach (LeftMenuItem child in children)
        {
            item.Children.Add(child);
            Telerik.Web.UI.RadMenuItem tmpR = new Telerik.Web.UI.RadMenuItem();
            //tmpR.Header = child;
            tmpR.Text = child.NodeText;
            tmpR.Value = child.NodeUrl;
            if (!String.IsNullOrEmpty(child.NodeUrl))
                tmpR.Attributes.Add("onclick", child.NodeUrl);

            tmp.Items.Add(tmpR);

            FindChildren(child, tmpR);
        }
    }
}

//переделала класс одной ячейки под себя
public class LeftMenuItem
{
    public string NodeText { get; private set; }

    public string NodeUrl { get; private set; }

    public string NodeStyle { get; private set; }

    public string ParentId { get; private set; }

    public string NodeId { get; private set; }

    public List<LeftMenuItem> Children { get; private set; }

    public LeftMenuItem(string nodeText, string nodeURL, string nodeStyle, string nodeID, string parentID)
    {
        this.NodeText = nodeText;
        this.NodeUrl = nodeURL;
        this.NodeStyle = nodeStyle;
        this.NodeId = nodeID;
        this.ParentId = parentID;

        this.Children = new List<LeftMenuItem>();
    } 
}

