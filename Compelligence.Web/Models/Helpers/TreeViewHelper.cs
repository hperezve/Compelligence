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

    public static class TreeViewHelper
    {

        public static string BuildHeaderScript(this HtmlHelper helper)
        {
            StringBuilder buildHeaderScript = new StringBuilder();
            buildHeaderScript.AppendLine("  var disableItems = function(id){");
            buildHeaderScript.AppendLine("      $(id).unbind('click', id.click);");
            buildHeaderScript.AppendLine("  } ");

            buildHeaderScript.AppendLine("  var enabled_disabled = function(){");
            buildHeaderScript.AppendLine("      $('#tree_Workspace').click(); $('#tree_Workspace').click();"); // buildHeaderScript.Append("      $('#' + id).click(); $('#' + id).click();"); 
            buildHeaderScript.AppendLine("      $('#tree_Environment').click(); $('#tree_Environment').click();");
            buildHeaderScript.AppendLine("      $('#tree_Tools').click(); $('#tree_Tools').click();");
            buildHeaderScript.AppendLine("      $('#tree_Research').click(); $('#tree_Research').click();");
            buildHeaderScript.AppendLine("      $('#tree_Reports').click(); $('#tree_Reports').click();");
            buildHeaderScript.AppendLine("      $('#tree_Admin').click(); $('#tree_Admin').click();");
            buildHeaderScript.AppendLine("  } ");

            buildHeaderScript.AppendLine("  var enableClassSelected = function(id) {");
            buildHeaderScript.AppendLine("      $(id).addClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("  } ");

            buildHeaderScript.AppendLine("  var disableAllClassSelected = function() {");

            buildHeaderScript.AppendLine("      $('#tree_Workspace_Dashboard').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Project').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Reports').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Deal').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Event').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Feedback').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Survey').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Calendar').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Newsletter').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Objective').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Workspace_Kit').removeClass(\"onSelectedItem\");");

            buildHeaderScript.AppendLine("      $('#tree_Environment_Industry').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Environment_Competitor').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Environment_Product').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Environment_Customer').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Environment_Supplier').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Environment_Partner').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Environment_Library').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Environment_Criterias').removeClass(\"onSelectedItem\");");
            //buildHeaderScript.AppendLine("      $('#tree_Environment_CriteriaGroup').removeClass(\"onSelectedItem\");");
            //buildHeaderScript.AppendLine("      $('#tree_Environment_CriteriaSet').removeClass(\"onSelectedItem\");");
            //buildHeaderScript.AppendLine("      $('#tree_Environment_Criteria').removeClass(\"onSelectedItem\");");

            buildHeaderScript.AppendLine("      $('#tree_Tools_WinLoss').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Tools_Nugget').removeClass(\"onSelectedItem\");");

            buildHeaderScript.AppendLine("      $('#tree_Research_Source').removeClass(\"onSelectedItem\");");

            buildHeaderScript.AppendLine("      $('#tree_Reports_Environment').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Reports_Admin').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Reports_Workspace').removeClass(\"onSelectedItem\");");


            buildHeaderScript.AppendLine("      $('#tree_Admin_User').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Admin_Team').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Admin_Template').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Admin_Website').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Admin_ContentType').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Admin_LibraryType').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("      $('#tree_Admin_LibraryExternalSource').removeClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("  } ");

            buildHeaderScript.AppendLine("  var refreshItems = function(id) {  ");
            buildHeaderScript.AppendLine("      disableAllClassSelected();");
            buildHeaderScript.AppendLine("      $(id).addClass(\"onSelectedItem\");");
            buildHeaderScript.AppendLine("  } ");



            buildHeaderScript.AppendLine("var WorkspaceRefresh = function(id) {");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_DashboardContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_ProjectContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_DealContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_EventContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_SurveyContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_CalendarContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_NewsletterContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_WinLossAnalysisContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_ActionHistoryContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_ObjectiveContent'); ");
            buildHeaderScript.AppendLine("    WorkspaceSubtabs.setActiveTab('WorkspaceTab_KitContent'); ");

            buildHeaderScript.AppendLine("}");
            buildHeaderScript.AppendLine("var EnvironmentRefresh = function() {");                        
            buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_IndustryContent');");
            buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CompetitorContent');");
            buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_ProductContent');");
            buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CustomerContent');");
            buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_SupplierContent');");
            buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_PartnerContent');");
            buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_LibraryContent');");
            buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriasContent');");
            //buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaGroupContent');");
            //buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaSetContent');");
            //buildHeaderScript.AppendLine("    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaContent');");
            buildHeaderScript.AppendLine("}");

            buildHeaderScript.AppendLine("var ToolsRefresh = function() {");
            buildHeaderScript.AppendLine("    ToolsSubtabs.setActiveTab('ToolsTab_WinLossContent');");
            buildHeaderScript.AppendLine("    ToolsSubtabs.setActiveTab('ToolsTab_NuggetContent');");
            buildHeaderScript.AppendLine("}");

            buildHeaderScript.AppendLine("var ResearchRefresh = function() {");
            buildHeaderScript.AppendLine("   // $('#tree_Research_Source').click();");
            buildHeaderScript.AppendLine("}");

            buildHeaderScript.AppendLine("var ReportsRefresh = function() {");
            buildHeaderScript.AppendLine("    //ReportsSubtabs.setActiveTab('ReportsTab_ReportsWorkspaceContent'); ");
            buildHeaderScript.AppendLine("    //ReportsSubtabs.setActiveTab('ReportsTab_ReportsEnvironmentContent'); ");
            buildHeaderScript.AppendLine("    //ReportsSubtabs.setActiveTab('ReportsTab_ReportsAdminContent'); ");

            buildHeaderScript.AppendLine("}");

            buildHeaderScript.AppendLine("var AdminRefresh = function() {");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_UserContent'); ");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_TeamContent'); ");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_TemplateContent');");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_WebsiteContent');");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_WorkflowContent');");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_ContentTypeContent');");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_LibraryTypeContent');");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_LibraryExternalSourceContent');");
            buildHeaderScript.AppendLine("    AdminSubtabs.setActiveTab('AdminTab_CustomFieldContent');");
            buildHeaderScript.AppendLine("}");

            return buildHeaderScript.ToString();
            
        }

        public static string BuildNodeScript(this HtmlHelper helper, IList listNode,params String[] content)
        {
            StringBuilder buildNodeScript = new StringBuilder();
            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];

            for (int i = 0; i < listNode.Count; i++)
            {
                StringBuilder  scriptNode = new StringBuilder("");

                string[] nodeParameters = (string[])listNode[i];
                string nodeId = nodeParameters[0];
                string nodeLabel = nodeParameters[1];
                string access = nodeParameters[2];

                if (!string.IsNullOrEmpty(access))
                {
                    if (RoleManager.GetInstance().AllowAccess(userId, access))
                    {
                        scriptNode.AppendLine("$('#tree_" + nodeLabel + "').click(function() { BackEndTabs.setActiveTab('AdminTabs_" + nodeId + "Tab');");
                        if ( content.Length>0 )
                          scriptNode.AppendLine(content[i]);
                        scriptNode.AppendLine("});");
                    }
                }
                buildNodeScript.AppendLine(scriptNode.ToString());
            }

            return buildNodeScript.ToString();
        }

        public static string BuildSubNodeScript(this HtmlHelper helper, IList listSubNode)
        {
            StringBuilder buildSubNodeScript = new StringBuilder();
            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];

            for (int i = 0; i < listSubNode.Count; i++)
            {
                StringBuilder scriptSubNode = new StringBuilder("");

                string[] subNodeParameters = (string[])listSubNode[i];
                string subNodeId = subNodeParameters[0];
                string subNodeLabel = subNodeParameters[1];
                string nodeId = subNodeParameters[2];
                string nodeLabel = subNodeParameters[3];
                string access = subNodeParameters[4];

                if (!string.IsNullOrEmpty(access))
                {
                    if (RoleManager.GetInstance().AllowAccess(userId, access))
                    {
                        string bug = nodeId.Equals("Reports") ? "Reports" : "";
                        scriptSubNode.AppendLine("$('#tree_"+nodeId+"_"+subNodeId+"').click(function() {");
                        scriptSubNode.AppendLine("    "+nodeId+"Refresh();");
                        scriptSubNode.AppendLine("    enabled_disabled();");
                        scriptSubNode.AppendLine("    refreshItems('#tree_"+nodeId+"_"+subNodeId+"');");
                        scriptSubNode.AppendLine("    BackEndTabs.setActiveTab('AdminTabs_"+nodeId+"Tab');");
                        scriptSubNode.AppendLine("    "+nodeId+"Subtabs.setActiveTab('"+nodeId+"Tab_"+bug+subNodeId+"Content');");
                        scriptSubNode.AppendLine("    disableItems('#tree_"+nodeId+"_"+subNodeId+"');");
                        scriptSubNode.AppendLine("});");

                        //scriptSubNode.AppendLine("$('#tree_" + nodeLabel + "_" + subNodeLabel + "').click(function() {");
                        //scriptSubNode.AppendLine(nodeId+"Refresh(); ");
                        //scriptSubNode.AppendLine("enableddisabled();");
                        //scriptSubNode.AppendLine("refreshItems('#tree_" + nodeLabel + "_" + subNodeLabel + "'); ");
                        //scriptSubNode.AppendLine("BackEndTabs.setActiveTab('AdminTabs_" + nodeId + "Tab'); ");
                        //scriptSubNode.AppendLine(nodeLabel + "SubTabs.setActiveTab('" + nodeId + "Tab_" + subNodeId + "Content');");
                        //scriptSubNode.AppendLine("disableItems('#tree_" + nodeLabel + "_" + subNodeLabel + "');");
                        //scriptSubNode.AppendLine("});");



                    }
                }
                buildSubNodeScript.AppendLine(scriptSubNode.ToString());


            }
            return buildSubNodeScript.ToString();
        }

        public static string BuildTreeScript(this HtmlHelper helper)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);

            StringBuilder TreeRoot = new StringBuilder();

            IList listNode = new ArrayList();
            IList listSubNode = new ArrayList();


            TreeRoot.Append(" <script type='text/javascript' >");

            //Add Header functions
            TreeRoot.AppendLine(BuildHeaderScript(helper));
            TreeRoot.Append("   $(function() {");

            //TreeRoot.Append("       $('#TreeviewMenu').treeview();");

            listNode = new ArrayList();
            listNode.Add(new string[] { "Workspace" /*NODE ID*/, "Workspace" /*NODE LABEL*/, "TAB-WRK" /*ACCESS*/});
            IList listWorkSpace = new ArrayList();
            listWorkSpace.Add(new string[] { "Dashboard"/*SUBNODE ID*/, "Dashboard"/*SUB LABEL*/, "Workspace" /*NODE ID*/, "Workspace"/*NODE LABEL*/, "SUBTAB-WRK-DSH" /*ACCESS*/});
            listWorkSpace.Add(new string[] { "Project", "Project", "Workspace", "Workspace", "SUBTAB-WRK-PRJ" });
            listWorkSpace.Add(new string[] { "Deal", "Deal Support", "Workspace", "Workspace", "SUBTAB-WRK-DLS" });
            listWorkSpace.Add(new string[] { "Event", "Events", "Workspace", "Workspace", "SUBTAB-WRK-EVT" });
            listWorkSpace.Add(new string[] { "Survey", "Survey", "Workspace", "Workspace", "SUBTAB-WRK-SRV" });
            listWorkSpace.Add(new string[] { "Calendar", "Calendar", "Workspace", "Workspace", "SUBTAB-WRK-CAL" });
            listWorkSpace.Add(new string[] { "Newsletter", "Newsletter", "Workspace", "Workspace", "SUBTAB-WRK-NWL" });
            listWorkSpace.Add(new string[] { "Objective", "Objectives", "Workspace", "Workspace", "SUBTAB-WRK-OBJ" });
            listWorkSpace.Add(new string[] { "Kit", "KITS", "Workspace", "Workspace", "SUBTAB-WRK-KIT" });

            string TreeWorkSpace = BuildNodeScript(helper, listNode, BuildSubNodeScript(helper, listWorkSpace));
            TreeRoot.AppendLine(TreeWorkSpace);

            listNode = new ArrayList();
            listNode.Add(new string[] { "Environment", "Environment", "TAB-ENV" });

            listSubNode = new ArrayList();
            
            
            listSubNode.Add(new string[] { "Industry", "Industries", "Environment", "Environment", "SUBTAB-ENV-IND" });
            listSubNode.Add(new string[] { "Competitor", "Competitors", "Environment", "Environment", "SUBTAB-ENV-CMP" });
            listSubNode.Add(new string[] { "Product", "Products/Offering", "Environment", "Environment", "SUBTAB-ENV-PRD" });
            listSubNode.Add(new string[] { "Customer", "Customers", "Environment", "Environment", "SUBTAB-ENV-CST" });
            listSubNode.Add(new string[] { "Supplier", "Suppliers", "Environment", "Environment", "SUBTAB-ENV-SPL" });
            listSubNode.Add(new string[] { "Partner", "Partners", "Environment", "Environment", "SUBTAB-ENV-PRT" });
            listSubNode.Add(new string[] { "Library", "Libraries", "Environment", "Environment", "SUBTAB-ENV-LBR" });
            listSubNode.Add(new string[] { "Criterias", "Criterias", "Environment", "Environment", "SUBTAB-ENV-CRG" });
            //listSubNode.Add(new string[] { "CriteriaGroup", "CriteriaGroup", "Environment", "Environment", "SUBTAB-ENV-CRG" });
            //listSubNode.Add(new string[] { "CriteriaSet", "Set", "Environment", "Environment", "SUBTAB-ENV-CRS" });
            //listSubNode.Add(new string[] { "Criteria", "Criteria", "Environment", "Environment", "SUBTAB-ENV-CRI" });
            string TreeEnvironment = BuildNodeScript(helper, listNode,BuildSubNodeScript(helper, listSubNode));
            TreeRoot.AppendLine(TreeEnvironment);

            listNode = new ArrayList();
            listNode.Add(new string[] { "Tools", "Tools", "TAB-TLS" });

            listSubNode = new ArrayList();
            listSubNode.Add(new string[] { "WinLoss", "Win Loss", "Tools", "Tools", "SUBTAB-TLS-WNL" });
            listSubNode.Add(new string[] { "Nugget", "Nugget", "Tools", "Tools", "SUBTAB-TLS-NGT" });
            string TreeTools = BuildNodeScript(helper, listNode, BuildSubNodeScript(helper, listSubNode));
            TreeRoot.AppendLine(TreeTools);

            listNode = new ArrayList();
            listNode.Add(new string[] { "Research", "Research", "TAB-RSC" });
            listSubNode = new ArrayList();
            listSubNode.Add(new string[] { "Source", "Source", "Research", "Research", "SUBTAB-RSC-SRC" });
            string TreeResearch = BuildNodeScript(helper, listNode, BuildSubNodeScript(helper, listSubNode));
            TreeRoot.AppendLine(TreeResearch);

            listNode = new ArrayList();
            listNode.Add(new string[] { "Reports", "Reports", "TAB-REP" });

            listSubNode = new ArrayList();
            listSubNode.Add(new string[] { "Workspace"  , "Workspace"  , "Reports", "Reports", "SUBTAB-REP-WRK" });
            listSubNode.Add(new string[] { "Environment", "Environment", "Reports", "Reports", "SUBTAB-REP-ENV" });
            listSubNode.Add(new string[] { "Admin"      , "Admin"      , "Reports", "Reports", "SUBTAB-REP-ADM" });
            string TreeReports = BuildNodeScript(helper, listNode, BuildSubNodeScript(helper, listSubNode));
            TreeRoot.AppendLine(TreeReports);

            listNode = new ArrayList();
            listNode.Add(new string[] { "Admin", "Admin", "TAB-ADM" });
            listSubNode = new ArrayList();
            listSubNode.Add(new string[] { "User"       , "Users"       , "Admin", "Admin", "SUBTAB-ADM-USR" });
            listSubNode.Add(new string[] { "Team"       , "Teams"       , "Admin", "Admin", "SUBTAB-ADM-TEA" });
            listSubNode.Add(new string[] { "Template"   , "Templates"   , "Admin", "Admin", "SUBTAB-ADM-TMP" });
            listSubNode.Add(new string[] { "Website"    , "Website"     , "Admin", "Admin", "SUBTAB-ADM-WBS" });
            listSubNode.Add(new string[] { "ContentType", "Content Type", "Admin", "Admin", "SUBTAB-ADM-CTY" });
            listSubNode.Add(new string[] { "LibraryType", "Library Type", "Admin", "Admin", "SUBTAB-ADM-LBR" });
            listSubNode.Add(new string[] { "LibraryExternalSource", "Library External Source", "Admin", "Admin", "SUBTAB-ADM-LES" });
            string TreeAdmin = BuildNodeScript(helper, listNode,BuildSubNodeScript(helper, listSubNode));
            TreeRoot.AppendLine(TreeAdmin);


            listNode = new ArrayList();
            listNode.Add(new string[] { "Search", "Search", "TAB-SRC" });
            string TreeSearch = BuildNodeScript(helper, listNode);
            TreeRoot.AppendLine(TreeSearch);

            TreeRoot.Append("   });");

            TreeRoot.Append(" </script> ");

            return TreeRoot.ToString();
        }


        public static string BuildTreeDivs(this HtmlHelper helper)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder TreeDiv = new StringBuilder();

            IList listNode = new ArrayList();
            listNode.Add(new string[] { "Workspace" /*NODE ID*/, "Workspace" /*NODE LABEL*/, "TAB-WRK" /*ACCESS*/});

            IList listWorkSpace = new ArrayList();
            listWorkSpace.Add(new string[] { "Dashboard"/*SUBNODE ID*/, "Dashboard"/*SUB LABEL*/, "Workspace" /*NODE ID*/, "Workspace"/*NODE LABEL*/, "SUBTAB-WRK-DSH" /*ACCESS*/});
            listWorkSpace.Add(new string[] { "Project", "Project", "Workspace", "Workspace", "SUBTAB-WRK-PRJ" });
            listWorkSpace.Add(new string[] { "Deal", "Deal Support", "Workspace", "Workspace", "SUBTAB-WRK-DLS" });
            listWorkSpace.Add(new string[] { "Event", "Events", "Workspace", "Workspace", "SUBTAB-WRK-EVT" });
            listWorkSpace.Add(new string[] { "Survey", "Survey", "Workspace", "Workspace", "SUBTAB-WRK-SRV" });
            listWorkSpace.Add(new string[] { "Calendar", "Calendar", "Workspace", "Workspace", "SUBTAB-WRK-CAL" });
            listWorkSpace.Add(new string[] { "Newsletter", "Newsletter", "Workspace", "Workspace", "SUBTAB-WRK-NWL" });
            listWorkSpace.Add(new string[] { "Objective", "Objectives", "Workspace", "Workspace", "SUBTAB-WRK-OBJ" });
            listWorkSpace.Add(new string[] { "Kit", "KITS", "Workspace", "Workspace", "SUBTAB-WRK-KIT" });
            TreeDiv.AppendLine( BuildTreeNode(helper, listNode, BuildTreeSubNode(helper, listWorkSpace, "Workspace")) );
            
            listNode = new ArrayList();
            listNode.Add(new string[] { "Environment", "Environment", "TAB-ENV" });
            IList listEnvironment = new ArrayList();
            
            
            listEnvironment.Add(new string[] { "Industry", "Industries", "Environment", "Environment", "SUBTAB-ENV-IND" });
            listEnvironment.Add(new string[] { "Competitor", "Competitors", "Environment", "Environment", "SUBTAB-ENV-CMP" });
            listEnvironment.Add(new string[] { "Product", "Products/Offering", "Environment", "Environment", "SUBTAB-ENV-PRD" });
            listEnvironment.Add(new string[] { "Customer", "Customers", "Environment", "Environment", "SUBTAB-ENV-CST" });
            listEnvironment.Add(new string[] { "Supplier", "Suppliers", "Environment", "Environment", "SUBTAB-ENV-SPL" });
            listEnvironment.Add(new string[] { "Partner", "Partners", "Environment", "Environment", "SUBTAB-ENV-PRT" });
            listEnvironment.Add(new string[] { "Library", "Libraries", "Environment", "Environment", "SUBTAB-ENV-LBR" });
            listEnvironment.Add(new string[] { "Criterias", "Criterias", "Environment", "Environment", "SUBTAB-ENV-CRG" });
            //listEnvironment.Add(new string[] { "CriteriaGroup", "CriteriaGroup", "Environment", "Environment", "SUBTAB-ENV-CRG" });
            //listEnvironment.Add(new string[] { "CriteriaSet", "Set", "Environment", "Environment", "SUBTAB-ENV-CRS" });
            //listEnvironment.Add(new string[] { "Criteria", "Criteria", "Environment", "Environment", "SUBTAB-ENV-CRI" });
            TreeDiv.AppendLine(BuildTreeNode(helper, listNode, BuildTreeSubNode(helper, listEnvironment, "Environment")));

            listNode = new ArrayList();
            listNode.Add(new string[] { "Tools", "Tools", "TAB-TLS" });

            IList listTools = new ArrayList();
            listTools.Add(new string[] { "WinLoss", "Win Loss", "Tools", "Tools", "SUBTAB-TLS-WNL" });
            listTools.Add(new string[] { "Nugget", "Nugget", "Tools", "Tools", "SUBTAB-TLS-NGT" });
            TreeDiv.AppendLine(BuildTreeNode(helper,listNode,BuildTreeSubNode(helper, listTools, "Tools")));

            listNode = new ArrayList();
            listNode.Add(new string[] { "Research", "Research", "TAB-RSC" });

            IList listResearch = new ArrayList();
            listResearch.Add(new string[] { "Source", "Source", "Research", "Research", "SUBTAB-RSC-SRC" });
            TreeDiv.AppendLine(BuildTreeNode(helper,listNode,BuildTreeSubNode(helper, listResearch, "Research")) );

            listNode = new ArrayList();
            listNode.Add(new string[] { "Reports", "Reports", "TAB-REP" });

            IList listReports = new ArrayList();
            listReports.Add(new string[] { "Workspace"  , "Workspace"  , "Reports", "Reports", "SUBTAB-REP-WRK" });
            listReports.Add(new string[] { "Environment", "Environment", "Reports", "Reports", "SUBTAB-REP-ENV" });
            listReports.Add(new string[] { "Admin"      , "Admin"      , "Reports", "Reports", "SUBTAB-REP-ADM" });
            TreeDiv.AppendLine(BuildTreeNode(helper, listNode, BuildTreeSubNode(helper, listReports, "Reports")));

            listNode = new ArrayList();
            listNode.Add(new string[] { "Admin", "Admin", "TAB-ADM" });

            IList listAdmin = new ArrayList();
            listAdmin.Add(new string[] { "User", "Users"        , "Admin", "Admin", "SUBTAB-ADM-USR" });
            listAdmin.Add(new string[] { "Team", "Teams"        , "Admin", "Admin", "SUBTAB-ADM-TEA" });
            listAdmin.Add(new string[] { "Template", "Templates", "Admin", "Admin", "SUBTAB-ADM-TMP" });
            listAdmin.Add(new string[] { "Website", "Website"   , "Admin", "Admin", "SUBTAB-ADM-WBS" });
            listAdmin.Add(new string[] { "ContentType", "Content Type", "Admin", "Admin", "SUBTAB-ADM-CTY" });
            listAdmin.Add(new string[] { "LibraryType", "Library Type", "Admin", "Admin", "SUBTAB-ADM-LBR" });
            listAdmin.Add(new string[] { "LibraryExternalSource", "Library External Source", "Admin", "Admin", "SUBTAB-ADM-LES" });
            TreeDiv.AppendLine(BuildTreeNode(helper, listNode, BuildTreeSubNode(helper, listAdmin, "Admin")));

            listNode = new ArrayList();
            listNode.Add(new string[] { "Search", "Search", "TAB-SRC" });

            TreeDiv.AppendLine(BuildTreeNode(helper, listNode,string.Empty));

            return TreeDiv.ToString();
        }

        public static string BuildTreeNode(this HtmlHelper helper, IList listNode, string content)
        {
            StringBuilder buildNodeScript = new StringBuilder();
            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];

            for (int i = 0; i < listNode.Count; i++)
            {
                string scriptNode = "";

                string[] nodeParameters = (string[])listNode[i];
                string nodeId = nodeParameters[0];
                string nodeLabel = nodeParameters[1];
                string access = nodeParameters[2];

                if (!string.IsNullOrEmpty(access))
                {
                    if (RoleManager.GetInstance().AllowAccess(userId, access))
                    {
                        scriptNode = TreeDiv(nodeId, nodeLabel, content);
                    }
                }
                buildNodeScript.AppendLine(scriptNode);
            }

            return buildNodeScript.ToString();
        }
        public static string BuildTreeSubNode(this HtmlHelper helper, IList listSubNode, string parent)
        {
            StringBuilder buildNodeScript = new StringBuilder();
            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];

            for (int i = 0; i < listSubNode.Count; i++)
            {
                string scriptNode = "";

                string[] subNodeParameters = (string[])listSubNode[i];
                string subNodeId = subNodeParameters[0];
                string subNodeLabel = subNodeParameters[1];
                string nodeId = subNodeParameters[2];
                string nodeLabel = subNodeParameters[3];
                string access = subNodeParameters[4];

                if (!string.IsNullOrEmpty(access))
                {
                    if (RoleManager.GetInstance().AllowAccess(userId, access))
                    {
                        scriptNode = TreeSubDiv(subNodeId, subNodeLabel, nodeId,i==0 ? true : false);
                    }
                }
                buildNodeScript.AppendLine(scriptNode);
            }

            return buildNodeScript.ToString();
        }

        private static string TreeSubDiv(string id, string label, string parent, params bool[] close)
        {
            StringBuilder Script = new StringBuilder();
            bool isCssClose = false;
            if (close.Length > 0 && close[0] == true)
                isCssClose = true;
            if (isCssClose)
                Script.AppendLine(" <li class=\"closed\"><span class=\"subfolder\" id=\"tree_" + parent + "_" + id + "\">" + label + "</span></li>");
            else
                Script.AppendLine(" <li><span class=\"subfolder\" id=\"tree_" + parent + "_" + id + "\">" + label + "</span></li>");
            return Script.ToString();
        }

        private static string TreeDiv(string id, string label, params string[] content)
        {
            StringBuilder Script = new StringBuilder();
            Script.AppendLine("<li class=\"closed\"><span class=\"folder\" id=\"tree_" + id + "\">" + label + "</span>");
            
            Script.AppendLine("<ul>");
            if (content.Length > 0)
            {
                Script.AppendLine(content[0]);
            }

            Script.AppendLine("</ul>");
            Script.AppendLine("</li>");

            return Script.ToString();

        }
    }

}