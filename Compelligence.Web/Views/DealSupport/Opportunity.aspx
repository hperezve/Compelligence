<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/frontendsite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>
    Compelligence - Import Opportunities
    
    </title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
   <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/thickbox.css") %>" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script> 
    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script> 
    <script src="<%= Url.Content("~/Scripts/jquery.bgiframe.min.js") %>" type="text/javascript"></script> 
    <script src="<%= Url.Content("~/Scripts/jquery.ajaxQueue.js") %>" type="text/javascript"></script> 
    <script src="<%= Url.Content("~/Scripts/thickbox-compressed.js") %>" type="text/javascript"></script> 
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>

    <%--//Filter table--%>

    <script src="<%= Url.Content("~/Scripts/jquery.json.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.cookiejar.pack.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.tableFilter.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Rating.js") %>" type="text/javascript">    </script>    
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript">    </script> 
       
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
    
<style type="text/css">
 	
 	#contentleft
  {
 	 width: 70%; 	 
  }
  
   td
        {
          border: 1px solid #CCCCCC;
          
        }
 	 
</style>  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <%IList<SalesForceOpportunity> opportunities = (IList<SalesForceOpportunity>)ViewData["Opportunities"]; %>
    <br />
    <div id="ErrorMessage" style="height:20px;display:none"><%=(string)ViewData["ErrorMessage"]%></div>
    <div id="MessageBox"></div>
    <div>
        <table id="sftable" class="filtered">
            <thead>
                <tr>
                    <th style="width: 130px;">
                    <%--Id  --%>
                    <asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:LabelResource, DealSupportOpportunityId%>" />
                        
                    </th>
                    <th style="width: 350px;">
                    
                    <%--Name  --%>
                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ Resources:LabelResource, DealSupportOpportunityName%>" />
                        
                    </th>
                    <th style="width: 195px; ">
                    
                    <%--Description  --%>
                    <asp:Literal ID="Literal3"  runat="server" Text="<%$ Resources:LabelResource, DealSupportOpportunityDescription%>" />
                        
                    </th>
                    <th style="width: 150px;">
                    
                     <%--LeadSource  --%>
                    <asp:Literal ID="Literal4"  runat="server" Text="<%$ Resources:LabelResource, DealSupportOpportunityLeadSource%>" />
                        
                    </th>
                    <th style="width: 150px;">
                    
                    
                    <%--OwnerId--%>
                    <asp:Literal ID="Literal5"  runat="server" Text="<%$ Resources:LabelResource, DealSupportOpportunityOwnerId%>" />
                        
                    </th>
                    <th style="width: 50px;">
                    
                    <%--Action--%>
                    <asp:Literal ID="Literal6"  runat="server" Text="<%$ Resources:LabelResource, DealSupportOpportunityAction%>" />
                    
                        
                    </th>
                </tr>
            </thead>
            <tbody>
                <%foreach (SalesForceOpportunity o in opportunities)
                  { %>
                <tr>
                    <td>
                        <%=o.Id  %>
                    </td>
                    <td>
                        <%=o.Name  %>
                    </td>
                    <td>
                        <%=o.Description %>
                    </td>
                   <td>
                        <%=o.LeadSource %>
                    </td>
                    <td>
                        <%=o.OwnerId  %>
                    </td>
                    <td>
                        <input type="button" value="Add" onclick="location.href='<%=Url.Action("AddOpportunity","DealSupport",new {Id=o.Id}) %>';return false" />
                    </td>
                </tr>
                <%} %>
            </tbody>
        </table>
    </div>


    <script type="text/javascript">
        $(document).ready(function() {

            // Initialise Plugin
            var options = {
                matchingRow: function(state, tr, textTokens) {
                    if (!state) { return true; }
                    return state.value != true || tr.children('td:eq(2)').text() == 'yes';
                }
            };

            $('#sftable').tableFilter(options);
            $("#DealContent").css("overflow", "auto");
            $("#sftable input[type='text']").css("width", "98%");

        });
</script>
<script type="text/javascript">
    $(function() {
        var message = $("#ErrorMessage").text();
        if (message.toString().length > 0)
            MessageBox("Error", message);
    });
    
</script>

  
</asp:Content>
<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
<% Html.RenderPartial("Options"); %>
</asp:Content>
