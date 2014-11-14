using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Compelligence.Domain.Entity;
using Compelligence.Security.Managers;

namespace Compelligence.Web.Models.Helpers
{
    public static class RoleHelper
    {
        public static string HtmlInputButton(this HtmlHelper helper, string value, string onclick, string access)
        {
            string htmlInputButton = "";

            if (string.IsNullOrEmpty(access))
            {
                return htmlInputButton;
            }

            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];

            if (RoleManager.GetInstance().AllowAccess(userId, access))
            {
                htmlInputButton = "<input type=\"button\" value=\"" + value + "\" onclick=\"" + onclick + "\" />";
            }

            return htmlInputButton;
        }

        private static string ScriptTabs(this HtmlHelper helper, IList listTabs)
        {
            StringBuilder scriptTabs = new StringBuilder();

            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];

            for (int i = 0; i < listTabs.Count; i++)
            {
                string scriptTab = "";

                string[] tabParameters = (string[])listTabs[i];
                string tabId = tabParameters[0];
                string tabLabel = tabParameters[1];
                string access = tabParameters[2];

                if (!string.IsNullOrEmpty(access))
                {
                    if (RoleManager.GetInstance().AllowAccess(userId, access))
                    {
                        scriptTab = "                    { contentEl: '" + tabId + "Tab', title: '" + tabLabel + "', id: 'AdminTabs_" + tabId + "Tab',";
                        scriptTab += ("                     listeners: { activate: function() { document.getElementById('breadcrumb').innerHTML='" + tabId + "';");

                        foreach (string[] others in listTabs)  //Need Include Utility.js
                        {
                            scriptTab += ("TreeNodeCollapse('tree_" + others[0] + "');");
                        }
                        scriptTab += ("TreeNodeExpand('tree_" + tabId + "');");
                        scriptTab += ("                                                                      }}");
                        scriptTab += "}";
                    }
                }

                if (!string.IsNullOrEmpty(scriptTab))
                {
                    if (!string.IsNullOrEmpty(scriptTabs.ToString()))
                    {
                        scriptTabs.Append(",");
                    }
                    scriptTabs.AppendLine(scriptTab);
                }
            }

            return scriptTabs.ToString();
        }

        private static string ScriptSubTabs(this HtmlHelper helper, IList listSubTabs)
        {
            StringBuilder scriptSubTabs = new StringBuilder();

            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);

            for (int i = 0; i < listSubTabs.Count; i++)
            {
                StringBuilder scriptSubTab = new StringBuilder();

                string[] subTabParameters = (string[])listSubTabs[i];
                string subTabId = subTabParameters[0];
                string subTabLabel = subTabParameters[1];
                string tabId = subTabParameters[2];
                string access = subTabParameters[3];

                if (!string.IsNullOrEmpty(access))
                {
                    if (RoleManager.GetInstance().AllowAccess(userId, access))
                    {
                        if (tabId.Equals("Report"))
                        {
                            scriptSubTab.Append("                { contentEl: 'Reports" + subTabId + "Content', title: '" + subTabLabel + "', listeners: { activate: function() { document.getElementById('breadcrumb').innerHTML='<u>" + tabId + "</u> > " + subTabId + "';loadContent('" + new StringBuilder(url.Action(subTabId, tabId)).ToString() + "', '#Reports" + subTabId + "Content', '" + tabId + "'); } }, id: 'ReportsTab_Reports" + subTabId + "Content' }");
                        }
                        else
                        {
                            scriptSubTab.Append("                    { contentEl: '" + subTabId + "Content',");
                            scriptSubTab.Append("                          title: '" + subTabLabel + "',");
                            scriptSubTab.Append("                      listeners: { activate: function() { document.getElementById('breadcrumb').innerHTML='<u>" + tabId + "</u> > " + subTabId + "';");
                            if (string.Compare(access, "SUBTAB-WRK-DSH") == 0)
                            {
                                //scriptSubTab.Append(" alert($('#" + subTabId + "Content').is(':empty') + ' - ' + $('#" + subTabId + "Content').html()); ");
                                //scriptSubTab.Append(" if($('#" + subTabId + "Content').is(':empty')) { ");
                               // scriptSubTab.Append(" if(!isDashboardLoaded) { ");
                                scriptSubTab.Append("   loadContent('" + url.Action("Index", subTabId) + "', '#" + subTabId + "Content', '" + tabId + "'); ");
                                scriptSubTab.Append("   isDashboardLoaded = true; ");
                                //scriptSubTab.Append(" } ");
                            }
                            else
                            {
                                scriptSubTab.Append(" loadContent('" + url.Action("Index", subTabId) + "', '#" + subTabId + "Content', '" + tabId + "'); ");
                            }
                            scriptSubTab.Append(" refreshItems('#tree_" + tabId + "_" + subTabId + "');");
                            scriptSubTab.Append(" TreeNodeExpand('tree_" + tabId + "');");
                            scriptSubTab.Append(" $('#tree_" + tabId + "_" + subTabId + "').addClass(\"onSelectedItem\");");
                            scriptSubTab.Append(" } }, ");
                            scriptSubTab.Append("                             id: '" + tabId + "Tab_" + subTabId + "Content' }");
                        }
                    }
                }

                if (scriptSubTab.Length > 0)
                {
                    if (scriptSubTabs.Length > 0)
                    {
                        scriptSubTabs.Append(",");
                    }
                    scriptSubTabs.AppendLine(scriptSubTab.ToString());
                }
            }

            return scriptSubTabs.ToString();
        }

        public static string ScriptTabsSubTabs(this HtmlHelper helper, ClientCompany clientCompany)
        {
            bool IsDashboardTabVisible = false;
            bool IsCalendarTabVisible = false;
            if (clientCompany != null)
            {
                if (clientCompany.ShowDashboardTab.Equals("TRUE") || clientCompany.ShowDashboardTab.Equals("True") || clientCompany.ShowDashboardTab.Equals("true"))
                {
                    IsDashboardTabVisible = true;
                }
                if (clientCompany.ShowCalendarTab.Equals("TRUE") || clientCompany.ShowCalendarTab.Equals("True") || clientCompany.ShowCalendarTab.Equals("true"))
                {
                    IsCalendarTabVisible = true;
                }
            }
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);

            StringBuilder tabSubTab = new StringBuilder();

            IList listTabs = new ArrayList();
            IList listSubTabs = new ArrayList();

            tabSubTab.AppendLine("    <script type=\"text/javascript\">");
            tabSubTab.AppendLine("      var isDashboardLoaded = false; ");
            tabSubTab.AppendLine("        $(function() {");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("            BackEndTabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'AdminTabs',");
            tabSubTab.AppendLine("                id: 'AdminTabs_t',");
            tabSubTab.AppendLine("                activeTab: 0,");
            //tabSubTab.AppendLine("                width: 100%;,");
            tabSubTab.AppendLine("                enableTabScroll: true,");
            tabSubTab.AppendLine("                enablerowmove:true,");//true
            tabSubTab.AppendLine("                defaults: { autoHeight: true , autoWidth: true  },");
            tabSubTab.AppendLine("                listeners: {");
            tabSubTab.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            tabSubTab.AppendLine("                        }");
            tabSubTab.AppendLine("                },");
            tabSubTab.AppendLine("                items: [");

            listTabs = new ArrayList();
            //          (new string[] {    TAB ID    ,   TAB LABEL  ,   ACCESS  });
            listTabs.Add(new string[] { "Workspace", "Workspace", "TAB-WRK" });
            listTabs.Add(new string[] { "Environment", "Environment", "TAB-ENV" });
            listTabs.Add(new string[] { "Tools", "Tools", "TAB-TLS" });
            //listTabs.Add(new string[] { "Research", "Research", "TAB-RSC" });
            listTabs.Add(new string[] { "Reports", "Reports", "TAB-REP" });
            listTabs.Add(new string[] { "Admin", "Admin", "TAB-ADM" });
            listTabs.Add(new string[] { "Search", "Search", "TAB-SRC" });
            tabSubTab.AppendLine(ScriptTabs(helper, listTabs));
            tabSubTab.AppendLine("                ]");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("            WorkspaceSubtabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'WorkspaceTab',");
            tabSubTab.AppendLine("                id: 'WorkspaceTabPanel',");
            tabSubTab.AppendLine("                activeTab: 0,");
            //tabSubTab.AppendLine("                width: auto,");
            tabSubTab.AppendLine("                frame: true,");
            tabSubTab.AppendLine("                split:true,");//true
            tabSubTab.AppendLine("                enableTabScroll: true,");
            tabSubTab.AppendLine("                defaults: { autoHeight: true, autoWidth: true },");
            tabSubTab.AppendLine("                listeners: {");
            tabSubTab.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            tabSubTab.AppendLine("                            if (currentTab != null && currentTab.contentEl != 'DashboardContent') {");
            tabSubTab.AppendLine("                                $('#' + currentTab.contentEl).empty();");
            tabSubTab.AppendLine("                            }");
            tabSubTab.AppendLine("                        }");
            tabSubTab.AppendLine("                },");
            tabSubTab.AppendLine("                items: [");

            listSubTabs = new ArrayList();
            //             (new string[] {  SUBTAB ID ,  SUBTAB LABEL ,   TAB ID   ,     ACCESS       });
            if (IsDashboardTabVisible)
            {
                listSubTabs.Add(new string[] { "Dashboard", "Dashboard", "Workspace", "SUBTAB-WRK-DSH" });
            }
            listSubTabs.Add(new string[] { "Objective", "Objectives", "Workspace", "SUBTAB-WRK-OBJ" });
            listSubTabs.Add(new string[] { "Kit", "KITS", "Workspace", "SUBTAB-WRK-KIT" });
            listSubTabs.Add(new string[] { "Project", "Project", "Workspace", "SUBTAB-WRK-PRJ" });
            listSubTabs.Add(new string[] { "News", "News", "Workspace", "SUBTAB-WRK-MKT" });
            listSubTabs.Add(new string[] { "Event", "Events", "Workspace", "SUBTAB-WRK-EVT" });
            listSubTabs.Add(new string[] { "Deal", "Deal Support", "Workspace", "SUBTAB-WRK-DLS" });
            listSubTabs.Add(new string[] { "Newsletter", "Newsletter", "Workspace", "SUBTAB-WRK-NWL" });            
            listSubTabs.Add(new string[] { "Survey", "Survey", "Workspace", "SUBTAB-WRK-SRV" });
            if (IsCalendarTabVisible)
            {
                listSubTabs.Add(new string[] { "Calendar", "Calendar", "Workspace", "SUBTAB-WRK-CAL" });
            }

            tabSubTab.AppendLine(ScriptSubTabs(helper, listSubTabs));

            tabSubTab.AppendLine("                ]");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(1);");
            tabSubTab.AppendLine("            EnvironmentSubtabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'EnvironmentTab',");
            tabSubTab.AppendLine("                id: 'EnvironmentTabPanel',");
            //tabSubTab.AppendLine("                autoWidth: true,");
            tabSubTab.AppendLine("                frame: true,");
            tabSubTab.AppendLine("                enableTabScroll: true,");
            tabSubTab.AppendLine("                defaults: { autoHeight: true, autoWidth: true  },");
            tabSubTab.AppendLine("                listeners: {");
            tabSubTab.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            tabSubTab.AppendLine("                            if (currentTab != null && newTab.contentEl != currentTab.contentEl) {");
            tabSubTab.AppendLine("                                $('#' + currentTab.contentEl).empty();");
            tabSubTab.AppendLine("                            }");
            tabSubTab.AppendLine("                        }");
            tabSubTab.AppendLine("                },");
            tabSubTab.AppendLine("                items: [");

            listSubTabs = new ArrayList();
            //             (new string[] {    SUBTAB ID   ,    SUBTAB LABEL    ,    TAB ID    ,     ACCESS       });                        
            listSubTabs.Add(new string[] { "Industry", "Industries", "Environment", "SUBTAB-ENV-IND" });
            listSubTabs.Add(new string[] { "Competitor", "Competitors", "Environment", "SUBTAB-ENV-CMP" });
            listSubTabs.Add(new string[] { "Product", "Products/Offering", "Environment", "SUBTAB-ENV-PRD" });
            listSubTabs.Add(new string[] { "Customer", "Customers", "Environment", "SUBTAB-ENV-CST" });
            //listSubTabs.Add(new string[] { "Supplier", "Suppliers", "Environment", "SUBTAB-ENV-SPL" });
            //listSubTabs.Add(new string[] { "Partner", "Partners", "Environment", "SUBTAB-ENV-PRT" });
            listSubTabs.Add(new string[] { "Library", "Libraries", "Environment", "SUBTAB-ENV-LBR" });
            //listSubTabs.Add(new string[] { "Criterias", "Criterias", "Environment", "SUBTAB-ENV-CRG" });
            listSubTabs.Add(new string[] { "MarketType", "Market Type", "Environment", "SUBTAB-ENV-MKT" });


            //agregado Tools

            tabSubTab.AppendLine(ScriptSubTabs(helper, listSubTabs));
            tabSubTab.AppendLine("                ]");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(0);");
            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(2);");
            tabSubTab.AppendLine("            ToolsSubtabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'ToolsTab',");
            //tabSubTab.AppendLine("                autoWidth: true,");
            tabSubTab.AppendLine("                id: 'ToolsTabPanel',");
            //tabSubTab.AppendLine("                autoWidth: true,");
            tabSubTab.AppendLine("                frame: true,");
            tabSubTab.AppendLine("                enableTabScroll: true,");
            tabSubTab.AppendLine("                defaults: { autoHeight: true , autoWidth: true },");
            tabSubTab.AppendLine("                listeners: {");
            tabSubTab.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            tabSubTab.AppendLine("                            if (currentTab != null && newTab.contentEl != currentTab.contentEl) {");
            tabSubTab.AppendLine("                                $('#' + currentTab.contentEl).empty();");
            tabSubTab.AppendLine("                            }");
            tabSubTab.AppendLine("                        }");
            tabSubTab.AppendLine("                },");
            tabSubTab.AppendLine("                items: [");

            listSubTabs = new ArrayList();
            //             (new string[] { SUBTAB ID, SUBTAB LABEL,TAB ID,     ACCESS       });
            listSubTabs.Add(new string[] { "Swot", "SWOT", "Tools", "SUBTAB-TLS-SWO" });
            //listSubTabs.Add(new string[] { "WinLoss", "Win Loss", "Tools", "SUBTAB-TLS-WNL" });
            listSubTabs.Add(new string[] { "Trend", "Trend", "Tools", "SUBTAB-TLS-TRN" });
            //listSubTabs.Add(new string[] { "ProjectArchive", "Archive Projects", "Tools", "SUBTAB-TLS-PRJ" });
            listSubTabs.Add(new string[] { "NewsScoring", "News Scoring", "Tools", "SUBTAB-TLS-NSG" });            
            //listSubTabs.Add(new string[] { "Positioning", "Positioning", "Tools", "SUBTAB-TLS-PTN" });
            //listSubTabs.Add(new string[] { "WinLossAnalysis", "Win/Loss Analysis", "Tools", "SUBTAB-TLS-WLY" });
            listSubTabs.Add(new string[] { "ComparinatorCriteriaUploader", "Criteria Uploader", "Tools", "SUBTAB-TLS-CCU" });
            listSubTabs.Add(new string[] { "Source", "Source", "Tools", "SUBTAB-TLS-SRC" });//SUBTAB-RSC-SRC
            //listSubTabs.Add(new string[] { "Criteria", "Comparinator", "Tools", "SUBTAB-TLS-CRT" });
            
            // Research subtab
            //tabSubTab.AppendLine(ScriptSubTabs(helper, listSubTabs));
            //tabSubTab.AppendLine("                ]");
            //tabSubTab.AppendLine("            });");
            //tabSubTab.AppendLine("");
            //tabSubTab.AppendLine("            BackEndTabs.setActiveTab(0);");
            //tabSubTab.AppendLine("            BackEndTabs.setActiveTab(3);");
            //tabSubTab.AppendLine("            ResearchSubtabs = new Ext.TabPanel({");
            //tabSubTab.AppendLine("                renderTo: 'ResearchTab',");
            //tabSubTab.AppendLine("                id: 'ResearchTabPanel',");
            /////tabSubTab.AppendLine("                autoWidth: true,");
            //tabSubTab.AppendLine("                enableTabScroll: true,");
            //tabSubTab.AppendLine("                defaults: { autoHeight: true, autoWidth: true  },");
            //tabSubTab.AppendLine("                listeners: {");
            //tabSubTab.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            //tabSubTab.AppendLine("                            if (currentTab != null && newTab.contentEl != currentTab.contentEl) {");
            //tabSubTab.AppendLine("                                $('#' + currentTab.contentEl).empty();");
            //tabSubTab.AppendLine("                            }");
            //tabSubTab.AppendLine("                        }");
            //tabSubTab.AppendLine("                },");
            //tabSubTab.AppendLine("                items: [");

            //listSubTabs = new ArrayList();
            //listSubTabs.Add(new string[] { "Source", "Source", "Research", "SUBTAB-RSC-SRC" });
            
            // Report subtab            
            tabSubTab.AppendLine(ScriptSubTabs(helper, listSubTabs));
            tabSubTab.AppendLine("                ]");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(0);");
            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(4);");
            tabSubTab.AppendLine("            ReportsSubtabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'ReportsTab',");
            tabSubTab.AppendLine("                id: 'ReportsTabPanel',");
            //tabSubTab.AppendLine("                autoWidth: true,");
            tabSubTab.AppendLine("                frame: true,");
            tabSubTab.AppendLine("                enableTabScroll: true,");
            tabSubTab.AppendLine("                defaults: { autoHeight: true , autoWidth: true },");
            tabSubTab.AppendLine("                listeners: {");
            tabSubTab.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            tabSubTab.AppendLine("                            if (currentTab != null && newTab.contentEl != currentTab.contentEl) {");
            tabSubTab.AppendLine("                                $('#' + currentTab.contentEl).empty();");
            tabSubTab.AppendLine("                            }");
            tabSubTab.AppendLine("                        }");
            tabSubTab.AppendLine("                },");
            tabSubTab.AppendLine("                items: [");

            listSubTabs = new ArrayList();
            //             (new string[] { SUBTAB ID, SUBTAB LABEL,TAB ID,     ACCESS       });
            listSubTabs.Add(new string[] { "Workspace", "Workspace", "Report", "SUBTAB-REP-WRK" });
            listSubTabs.Add(new string[] { "Environment", "Environment", "Report", "SUBTAB-REP-ENV" });
            listSubTabs.Add(new string[] { "Admin", "Admin", "Report", "SUBTAB-REP-ADM" });
            listSubTabs.Add(new string[] { "Event", "Event", "Report", "SUBTAB-REP-EVN" });
            listSubTabs.Add(new string[] { "Dynamic", "Dynamic", "Report", "SUBTAB-REP-TBR" });
            listSubTabs.Add(new string[] { "General", "General", "Report", "SUBTAB-REP-GEN" });
            listSubTabs.Add(new string[] { "Comparison", "Comparison", "Report", "SUBTAB-REP-COM" });

            tabSubTab.AppendLine(ScriptSubTabs(helper, listSubTabs));

            tabSubTab.AppendLine("                ]");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(0);");
            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(5);");
            tabSubTab.AppendLine("            AdminSubtabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'AdminTab',");
            tabSubTab.AppendLine("                id: 'AdminTabPanel',");
            tabSubTab.AppendLine("                autoWidth: true,");
            tabSubTab.AppendLine("                enableTabScroll: true,");
            tabSubTab.AppendLine("                frame: true,");
            tabSubTab.AppendLine("                defaults: { autoHeight: true},");
            tabSubTab.AppendLine("                listeners: {");
            tabSubTab.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            tabSubTab.AppendLine("                            if (currentTab != null && newTab.contentEl != currentTab.contentEl) {");
            tabSubTab.AppendLine("                                $('#' + currentTab.contentEl).empty();");
            tabSubTab.AppendLine("                            }");
            tabSubTab.AppendLine("                        }");
            tabSubTab.AppendLine("                },");
            tabSubTab.AppendLine("                items: [");

            listSubTabs = new ArrayList();
            //             (new string[] {   SUBTAB ID  , SUBTAB LABEL  , TAB ID ,     ACCESS       });
            listSubTabs.Add(new string[] { "User", "Users", "Admin", "SUBTAB-ADM-USR" });
            listSubTabs.Add(new string[] { "Team", "Teams", "Admin", "SUBTAB-ADM-TEA" });
            listSubTabs.Add(new string[] { "Template", "Templates", "Admin", "SUBTAB-ADM-TMP" });
            listSubTabs.Add(new string[] { "Website", "Website", "Admin", "SUBTAB-ADM-WBS" });
            listSubTabs.Add(new string[] { "ContentType", "Content Type", "Admin", "SUBTAB-ADM-CTY" });
            listSubTabs.Add(new string[] { "LibraryType", "Library Type", "Admin", "SUBTAB-ADM-LBR" });
            listSubTabs.Add(new string[] { "EventType", "Event Type", "Admin", "SUBTAB-ADM-ETY" });
            listSubTabs.Add(new string[] { "LibraryExternalSource", "Library External Source", "Admin", "SUBTAB-ADM-LES" });
            listSubTabs.Add(new string[] { "CustomField", "Custom Field", "Admin", "SUBTAB-ADM-CSF" });
            listSubTabs.Add(new string[] { "ConfigurationGeneral", "Configurations", "Admin", "SUBTAB-ADM-CNS" });
            tabSubTab.AppendLine(ScriptSubTabs(helper, listSubTabs));

            tabSubTab.AppendLine("                ]");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("            ");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(0);");
            tabSubTab.AppendLine("            SearchSubtabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'SearchTab',");
            tabSubTab.AppendLine("                id: 'SearchTabPanel',");
            //tabSubTab.AppendLine("                autoWidth: true,");
            tabSubTab.AppendLine("                frame: true,");
            tabSubTab.AppendLine("                defaults: { autoHeight: true , autoWidth: true  }");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("            ");
            tabSubTab.AppendLine("            loadContent('" + new StringBuilder(url.Action("Index", "InternalSearch")).ToString() + "', '#SearchTab', 'Search');");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("        });");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("    </script>");

            return tabSubTab.ToString();
        }

        public static string ScriptTabsSubTabsSmall(this HtmlHelper helper)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);

            StringBuilder tabSubTab = new StringBuilder();

            IList listTabs = new ArrayList();
            IList listSubTabs = new ArrayList();

            tabSubTab.AppendLine("    <script type=\"text/javascript\">");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("        $(function() {");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("            BackEndTabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'AdminTabs',");
            tabSubTab.AppendLine("                id: 'AdminTabs_t',");
            tabSubTab.AppendLine("                activeTab: 0,");
            //tabSubTab.AppendLine("                width: 100%,");
            tabSubTab.AppendLine("                enableTabScroll: true,");
            tabSubTab.AppendLine("                enablerowmove:true,");//true
            tabSubTab.AppendLine("                defaults: { autoHeight: true , autoWidth: true },");
            tabSubTab.AppendLine("                listeners: {");
            tabSubTab.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            tabSubTab.AppendLine("                        }");
            tabSubTab.AppendLine("                },");
            tabSubTab.AppendLine("                items: [");

            listTabs = new ArrayList();
            //          (new string[] {    TAB ID    ,   TAB LABEL  ,   ACCESS  });
            listTabs.Add(new string[] { "Workspace", "Workspace", "TAB-WRK" });
            listTabs.Add(new string[] { "Environment", "Environment", "TAB-ENV" });
            //listTabs.Add(new string[] { "Tools", "Tools", "TAB-TLS" });
            //listTabs.Add(new string[] { "Research", "Research", "TAB-RSC" });
            listTabs.Add(new string[] { "Reports", "Reports", "TAB-REP" });
            listTabs.Add(new string[] { "Admin", "Admin", "TAB-ADM" });
            listTabs.Add(new string[] { "Search", "Search", "TAB-SRC" });
            tabSubTab.AppendLine(ScriptTabs(helper, listTabs));
            tabSubTab.AppendLine("                ]");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("");


            listSubTabs = new ArrayList();
            //             (new string[] {  SUBTAB ID ,  SUBTAB LABEL ,   TAB ID   ,     ACCESS       });
            listSubTabs.Add(new string[] { "Dashboard", "Dashboard", "Workspace", "SUBTAB-WRK-DSH" });
            listSubTabs.Add(new string[] { "Project", "Project", "Workspace", "SUBTAB-WRK-PRJ" });
            //listSubTabs.Add(new string[] { "Deal", "Deal Support", "Workspace", "SUBTAB-WRK-DLS" });
            //listSubTabs.Add(new string[] { "Event", "Events", "Workspace", "SUBTAB-WRK-EVT" });
            listSubTabs.Add(new string[] { "Survey", "Survey", "Workspace", "SUBTAB-WRK-SRV" });
            //listSubTabs.Add(new string[] { "Calendar", "Calendar", "Workspace", "SUBTAB-WRK-CAL" });
            //listSubTabs.Add(new string[] { "Newsletter", "Newsletter", "Workspace", "SUBTAB-WRK-NWL" });

            tabSubTab.AppendLine(MakeSubTab("WorkspaceSubtabs", "WorkspaceTab", "WorkspaceTabPanel", ScriptSubTabsSmall(helper, listSubTabs)));

            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(1);");

            listSubTabs = new ArrayList();
            //             (new string[] {    SUBTAB ID   ,    SUBTAB LABEL    ,    TAB ID    ,     ACCESS       });
            listSubTabs.Add(new string[] { "Objective", "Objectives", "Environment", "SUBTAB-ENV-OBJ" });
            listSubTabs.Add(new string[] { "Kit", "KITS", "Environment", "SUBTAB-ENV-KIT" });
            listSubTabs.Add(new string[] { "Industry", "Industries", "Environment", "SUBTAB-ENV-IND" });
            listSubTabs.Add(new string[] { "Competitor", "Competitors", "Environment", "SUBTAB-ENV-CMP" });
            listSubTabs.Add(new string[] { "Product", "Products/Offering", "Environment", "SUBTAB-ENV-PRD" });
            //listSubTabs.Add(new string[] { "Customer", "Customers", "Environment", "SUBTAB-ENV-CST" });
            //listSubTabs.Add(new string[] { "Supplier", "Suppliers", "Environment", "SUBTAB-ENV-SPL" });
            //listSubTabs.Add(new string[] { "Partner", "Partners", "Environment", "SUBTAB-ENV-PRT" });
            listSubTabs.Add(new string[] { "Library", "Libraries", "Environment", "SUBTAB-ENV-LBR" });
            //listSubTabs.Add(new string[] { "Criterias", "Criterias", "Environment", "SUBTAB-ENV-CRG" });
            listSubTabs.Add(new string[] { "MarketType", "Market Type", "Environment", "SUBTAB-ENV-MKT" });
            tabSubTab.AppendLine(MakeSubTab("EnvironmentSubtabs", "EnvironmentTab", "EnvironmentTabPanel", ScriptSubTabsSmall(helper, listSubTabs)));

            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(0);");

            listSubTabs = new ArrayList();
            //             (new string[] { SUBTAB ID, SUBTAB LABEL,TAB ID,     ACCESS       });
            listSubTabs.Add(new string[] { "Workspace", "Workspace", "Report", "SUBTAB-REP-WRK" });
            listSubTabs.Add(new string[] { "Environment", "Environment", "Report", "SUBTAB-REP-ENV" });
            listSubTabs.Add(new string[] { "Admin", "Admin", "Report", "SUBTAB-REP-ADM" });
            listSubTabs.Add(new string[] { "Event", "Event", "Report", "SUBTAB-REP-EVN" });
            listSubTabs.Add(new string[] { "Dynamic", "Dynamic", "Report", "SUBTAB-REP-TBR" });
            listSubTabs.Add(new string[] { "General", "General", "Report", "SUBTAB-REP-GEN" });
            listSubTabs.Add(new string[] { "Comparison", "Comparison", "Report", "SUBTAB-REP-COM" });
            tabSubTab.AppendLine(MakeSubTab("ReportsSubtabs", "ReportsTab", "ReportsTabPanel", ScriptSubTabsSmall(helper, listSubTabs)));

            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(0);");


            listSubTabs = new ArrayList();
            //             (new string[] {   SUBTAB ID  , SUBTAB LABEL  , TAB ID ,     ACCESS       });
            listSubTabs.Add(new string[] { "User", "Users", "Admin", "SUBTAB-ADM-USR" });
            listSubTabs.Add(new string[] { "Team", "Teams", "Admin", "SUBTAB-ADM-TEA" });
            listSubTabs.Add(new string[] { "Template", "Templates", "Admin", "SUBTAB-ADM-TMP" });
            listSubTabs.Add(new string[] { "Website", "Website", "Admin", "SUBTAB-ADM-WBS" });
            listSubTabs.Add(new string[] { "ContentType", "Content Type", "Admin", "SUBTAB-ADM-CTY" });
            listSubTabs.Add(new string[] { "LibraryType", "Library Type", "Admin", "SUBTAB-ADM-LBR" });
            //listSubTabs.Add(new string[] { "EventType", "Event Type", "Admin", "SUBTAB-ADM-ETY" });
            listSubTabs.Add(new string[] { "LibraryExternalSource", "Library External Source", "Admin", "SUBTAB-ADM-LES" });
            listSubTabs.Add(new string[] { "CustomField", "Custom Field", "Admin", "SUBTAB-ADM-CSF" });
            tabSubTab.AppendLine(MakeSubTab("AdminSubtabs", "AdminTab", "AdminTabPanel", ScriptSubTabsSmall(helper, listSubTabs)));

            tabSubTab.AppendLine("            BackEndTabs.setActiveTab(0);");
            tabSubTab.AppendLine("            SearchSubtabs = new Ext.TabPanel({");
            tabSubTab.AppendLine("                renderTo: 'SearchTab',");
            tabSubTab.AppendLine("                id: 'SearchTabPanel',");
            //tabSubTab.AppendLine("                autoWidth: true,");
            tabSubTab.AppendLine("                frame: true,");
            tabSubTab.AppendLine("                defaults: { autoHeight: true , autoWidth: true  }");
            tabSubTab.AppendLine("            });");
            tabSubTab.AppendLine("            ");
            tabSubTab.AppendLine("            loadContent('" + new StringBuilder(url.Action("Index", "InternalSearch")).ToString() + "', '#SearchTab', 'Search');");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("        });");
            tabSubTab.AppendLine("");
            tabSubTab.AppendLine("    </script>");

            return tabSubTab.ToString();
        }

        static string MakeSubTab(string jsObject, string jsTarget, string jsId)
        {
            return MakeSubTab(jsObject, jsTarget, jsId, string.Empty);
        }
        static string MakeSubTab(string jsObject, string jsTarget, string jsId, string jsContent)
        {
            //jsObject=WorkspaceSubtabs
            //jsTarget=WorkspaceTab
            //jsId=WorkspaceTabPanel

            StringBuilder result = new StringBuilder();

            result.AppendLine("            " + jsObject + " = new Ext.TabPanel({");
            result.AppendLine("                renderTo: '" + jsTarget + "',");
            result.AppendLine("                id: '" + jsId + "',");
            result.AppendLine("                activeTab: 0,");
            //result.AppendLine("                autoWidth: true,");
            result.AppendLine("                frame: true,");
            result.AppendLine("                split:true,");
            result.AppendLine("                enableTabScroll: true,");
            result.AppendLine("                defaults: { autoHeight: true , autoWidth: true  },");
            result.AppendLine("                listeners: {");
            result.AppendLine("                        beforetabchange: function(tabPanel, newTab, currentTab) {");
            result.AppendLine("                            if (currentTab != null) {");
            result.AppendLine("                                $('#' + currentTab.contentEl).empty();");
            result.AppendLine("                            }");
            result.AppendLine("                        }");
            result.AppendLine("                },");
            result.AppendLine("                items: [");
            result.AppendLine(jsContent);
            result.AppendLine("                       ]");
            result.AppendLine("              });");
            result.AppendLine("");
            return result.ToString();
        }
        static string ScriptSubTabsSmall(this HtmlHelper helper, IList listSubTabs)
        {
            StringBuilder scriptSubTabs = new StringBuilder();

            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);

            for (int i = 0; i < listSubTabs.Count; i++)
            {
                string scriptSubTab = "";

                string[] subTabParameters = (string[])listSubTabs[i];
                string subTabId = subTabParameters[0];
                string subTabLabel = subTabParameters[1];
                string tabId = subTabParameters[2];
                string access = subTabParameters[3];

                if (!string.IsNullOrEmpty(access))
                {
                    if (RoleManager.GetInstance().AllowAccess(userId, access))
                    {
                        scriptSubTab += ("                    { contentEl: '" + subTabId + "Content',");
                        scriptSubTab += ("                          title: '" + subTabLabel + "',");
                        scriptSubTab += ("                      listeners: { activate: function() { document.getElementById('breadcrumb').innerHTML='<u>" + tabId + "</u> > " + subTabId + "';");
                        scriptSubTab += ("loadContent('" + url.Action("Index", subTabId) + "', '#" + subTabId + "Content', '" + tabId + "'); ");
                        scriptSubTab += ("refreshItems('#tree_" + tabId + "_" + subTabId + "');");
                        scriptSubTab += ("TreeNodeExpand('tree_" + tabId + "');");
                        scriptSubTab += ("$('#tree_" + tabId + "_" + subTabId + "').addClass(\"onSelectedItem\");");
                        scriptSubTab += (" } }, ");
                        scriptSubTab += ("                             id: '" + tabId + "Tab_" + subTabId + "Content' }");
                        if (tabId.Equals("Report"))
                        {
                            scriptSubTab = "                { contentEl: 'Reports" + subTabId + "Content', title: '" + subTabLabel + "', listeners: { activate: function() { document.getElementById('breadcrumb').innerHTML='<u>" + tabId + "</u> > " + subTabId + "';loadContent('" + new StringBuilder(url.Action(subTabId, tabId)).ToString() + "', '#Reports" + subTabId + "Content', '" + tabId + "'); } }, id: 'ReportsTab_Reports" + subTabId + "Content' }";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(scriptSubTab))
                {
                    if (!string.IsNullOrEmpty(scriptSubTabs.ToString()))
                    {
                        scriptSubTabs.Append(",");
                    }
                    scriptSubTabs.AppendLine(scriptSubTab);
                }
            }

            return scriptSubTabs.ToString();
        }


    }
}