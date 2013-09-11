using System;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AllUserWebPart14.AllUserWebs
{
    [ToolboxItemAttribute(false)]
    public class AllUserWebs : WebPart
    {
        private bool _error = false;
        private string _myProperty = null;

        private DataList dtList = new DataList();
        private DataTable dtSites = new DataTable();
        private String searchString = "";
        private DataGrid dgSites = new DataGrid();
        private int _repeatCount = 3;

        #region WebpartProperties
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [System.ComponentModel.Category("My Property Group")]
        [WebDisplayName("MyProperty")]
        [WebDescription("Meaningless Property")]
        public string MyProperty
        {
            get
            {
                if (_myProperty == null)
                {
                    _myProperty = "";
                }
                return _myProperty;
            }
            set { _myProperty = value; }
        }
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [System.ComponentModel.Category("Search Settings")]
        [WebDisplayName("Search String")]
        [WebDescription("String used to limit site collections by URL")]
        public string SearchString
        {
            get
            {
                if (searchString == null)
                {
                    searchString = "";
                }
                return searchString;
            }
            set { searchString = value; }
        }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [System.ComponentModel.Category("Search Settings")]
        [WebDisplayName("Repeat Columns")]
        [WebDescription("How many columns wide")]
        public int RepeatColumns
        {
            get
            {
                if (_repeatCount == null)
                {
                    _repeatCount = 3;
                }
                return _repeatCount;
            }
            set { _repeatCount = value; }
        }

        #endregion

        private string getSubWebs(String surl)
        {
            String sites = String.Empty;
            using (SPSite site = new SPSite(surl))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPWebCollection webCollection = web.GetSubwebsForCurrentUser();
                    foreach (SPWeb webCol in webCollection)
                    {
                        DataRow dRow = dtSites.NewRow();
                        if (!(webCol.NoCrawl) & !(webCol.Title.Equals("News")) & !(webCol.Title.Equals("Sites")) & !(webCol.Title.Equals("Reports")) & !(webCol.Title.Equals("Document Center")) & !(webCol.Title.Equals("Search")))
                        {

                            dRow["SiteName"] = webCol.Title;
                            dRow["SiteURL"] = webCol.Url;
                            dRow["SiteDesc"] = webCol.Description;
                            dtSites.Rows.Add(dRow);
                            sites = sites + "<tr><td><a href='" + webCol.Url + "'>link</a></td><td>" + webCol.Title + "</td></tr>";
                        }
                    }
                }
            }
            return sites;
        }

        private void buildSites()
        {
            SPSite sps = SPContext.Current.Site;
            String sites = "<table>";
            for (int siteCount = 0; siteCount < sps.WebApplication.Sites.Count; siteCount++)
            {
                SPUser spu = SPContext.Current.Web.CurrentUser;
                using (SPSite spsc = sps.WebApplication.Sites[siteCount])
                {
                    using (SPWeb rootWeb = spsc.RootWeb)
                    {
                        if (rootWeb.DoesUserHavePermissions(SPBasePermissions.ViewPages) == true)
                        {
                            if (spsc.Url.Contains(searchString) || searchString.Length == 0)
                            {
                                if (!(rootWeb.Description.Contains("Placeholder")))
                                {
                                    DataRow dRow = dtSites.NewRow();
                                    dRow["SiteURL"] = spsc.Url;
                                    dRow["SiteName"] = spsc.RootWeb.Title;
                                    dRow["SiteDesc"] = spsc.RootWeb.Description;
                                    dtSites.Rows.Add(dRow);
                                }
                                sites = sites + "<tr><td>" + spsc.Url + "</td><td>" + spsc.RootWeb.Title + "</td></tr>";
                                sites = sites + getSubWebs(spsc.Url);
                            }
                        }
                    }


                }

            }
        }
        // Start if Generated Code



        public AllUserWebs()
        {
            this.ExportMode = WebPartExportMode.All;
        }

        /// <summary>
        /// Create all your controls here for rendering.
        /// Try to avoid using the RenderWebPart() method.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (!_error)
            {
                try
                {

                    base.CreateChildControls();
                    dtSites.Columns.Add("SiteName");
                    dtSites.Columns.Add("SiteURL");
                    dtSites.Columns.Add("SiteDesc");
                    dtSites.DefaultView.Sort = "SiteName ASC";
                    buildSites();
                    // Your code here...
                    dtList.ItemTemplate = new AllUserWebs_Webpart.WebPartCode.SiteLinkRender();
                    dtList.RepeatDirection = RepeatDirection.Vertical;
                    dtList.BorderStyle = BorderStyle.Solid;
                    dtList.ItemStyle.Width = 1;
                    dtList.ItemStyle.BorderStyle = BorderStyle.Inset;
                    dtList.RepeatColumns = _repeatCount;

                    dtList.DataSource = dtSites.DefaultView;
                    dtList.DataBind();


                    // Your code here...

                    this.Controls.Add(new LiteralControl(this.MyProperty));
                    this.Controls.Add(dtList);
                    this.Controls.Add(dgSites);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

            
        /// <summary>
        /// Ensures that the CreateChildControls() is called before events.
        /// Use CreateChildControls() to create your controls.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!_error)
            {
                try
                {
                    base.OnLoad(e);
                    this.EnsureChildControls();

                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

         private void HandleException(Exception ex)
        {
            this._error = true;
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }
    }

        }

