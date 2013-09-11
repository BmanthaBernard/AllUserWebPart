using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllUserWebs_Webpart.WebPartCode
{
    class SiteLinkRender : ITemplate
    {
        void ITemplate.InstantiateIn(Control container)
        {

            HyperLink itemlbl = new HyperLink();
            itemlbl.Width = 110;
            itemlbl.DataBinding += new EventHandler(itemlbl_DataBinding);
            container.Controls.Add(itemlbl);

        }

        void itemlbl_DataBinding(object sender, EventArgs e)
        {
            // Label lbldata = (Label)sender;
            HyperLink hlSiteLink = (HyperLink)sender;
            DataListItem container = (DataListItem)hlSiteLink.NamingContainer;
            //  lbldata.Text = Convert.ToString(DataBinder.Eval(((DataListItem)container).DataItem, "SiteName"));
            hlSiteLink.NavigateUrl = Convert.ToString(DataBinder.Eval(((DataListItem)container).DataItem, "SiteURL"));
            hlSiteLink.ToolTip = Convert.ToString(DataBinder.Eval(((DataListItem)container).DataItem, "SiteDesc"));
            hlSiteLink.Text = Convert.ToString(DataBinder.Eval(((DataListItem)container).DataItem, "SiteName"));

        }

    }
}
