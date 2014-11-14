<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Compelligence.Domain.Entity.Views.UserProfileView>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   <title>List Users Of Company</title>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%= Url.Content("~/Content/Styles/tinytable.style.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/Site.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/StyleSorterTabla.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/jquery-latest.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/jquery.tablesorter.js") %>" type="text/javascript"></script>

   <script type="text/javascript">
       $(function() {
           $("table").tablesorter({ debug: true });
       });
	</script>
	<script type="text/javascript">
	    var updateEditHelp = function(urlAction, id) {
	        var value = '';
	        if ($('#EditHelp_' + id).is(':checked')) {
	            value=true.toString();
	         }
	        else {
	            value = false.toString();
	        }
	        var parameters = { id: id, editHelp: value };
	        if (value != undefined && value != null && value.length > 0) {
	            $.post(urlAction, parameters);
	        }
	    };
	    
	    var redirectoToThisAction = function(urlAction, firstParameter, secondParameter) {
	        var xmlhttp;
	        var results = null;
	        $.get(
            urlAction,
            null,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        var newResult = results.replace('"', '');
                        newResult = newResult.replace('"', '');
                        var httpPosition = newResult.indexOf("http://");
                        if (httpPosition == -1) {
                            self.location.href = 'http://' + newResult;
                        }
                        else {
                            self.location.href = newResult;
                        }
                    }
                }
            });	        
	    };
	</script>	
    <script type="text/javascript">
	        var x;
	        x = $(document);
	        x.ready(StartEvents);
	        function StartEvents() {
	            var x;
	            x = $("th");
	            x.mouseover(EnterMouse);
	            x.mouseout(ExitMouse);
	        }
	        function EnterMouse() 
	        {
	            $(this).css("color", "#FFFFFF");
	            $(this).css("background-color", "#464647");

	        }
	        function ExitMouse() 
	        {
	            $(this).css("color", "#000000");
	            $(this).css("background-color", "#EEEFEF");
	        }
    </script>
    <div style="text-align: right;">
        <%=Html.ActionLink("Return to List All Client Companies", "List", "MaintenanceAccount")%>        
    </div>    
    <h2>
        List Users Of <%= ViewData["CompanyName"].ToString()%> Company</h2>
        <h3 style="color: rgb(68, 68, 68);">
        <%
            if (!string.IsNullOrEmpty(ViewData["CountUser"].ToString()))
            {
             %>
                Numbers of Users: <%= ViewData["CountUser"].ToString()%>
             <%} %>
        </h3>
     
       
     <div align ="center" id="UserListTable" style="padding-top:10px;padding-left:10px">          
        <table  class="tablesorter" align="center">
        <colgroup>
          <col width="10px" />
          <col width="10px" />
          <col width="10px" />
          <col width="10px" />
        </colgroup>
        <thead>
        <tr>
            <th>
                First Name
            </th>
            <th>
                Last Name
            </th>
            <th>
                Email
            </th>
            <th>
                Status
            </th>
            <th>
                Group
            </th>
            <th>
                Title
            </th>
            <th>
                Phone
            </th>
            <th>
                Fax
            </th>
            <th>
                Country Code
            </th>
            <th>
                City
            </th>
            <th>
                State
            </th>          
            <th>
                Report To 
            </th>
            <th>
                Created By
            </th>
            <th>
                Created Date
            </th>              
            <th>
                Last Logged In
            </th> 
            <th>
                Edit Help
            </th>
            <th>
                Login As 
            </th>          
        </tr>
        </thead>
        <tbody>
        <% foreach (var item in Model)
           { %>
        <tr>
            <%--           <td>
                <%= Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%= Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ })%>
            </td>--%>
            <td>
                <%= Html.Encode(item.FirstName) %>
            </td>
            <td>
                <%= Html.Encode(item.LastName) %>
            </td>
            <td>
                <%= Html.Encode(item.Email) %>
            </td>
            <td>
                <%= Html.Encode(item.StatusLabel) %>
            </td>
            <td>
                <%= Html.Encode(item.SecurityGroupName) %>
            </td>
            <td>
                <%= Html.Encode(item.Title) %>
            </td>
            <td>
                <%= Html.Encode(item.Phone) %>
            </td>
            <td>
                <%= Html.Encode(item.Fax) %>
            </td>           
            <td>
                <%= Html.Encode(item.CountryName) %>
            </td>
            <td>
                <%= Html.Encode(item.City) %>
            </td>
            <td>
                <%= Html.Encode(item.Department) %>
            </td>           
            
            <td>
                <%= Html.Encode(item.ReportName) %>
            </td>
            <td>
                <%= Html.Encode(item.CreatedByName) %>
            </td>
            <td>
                <%= Html.Encode(item.CreatedDate) %>
            </td>
            <td>
                <%= Html.Encode(item.LastLogged) %>
            </td>
            <td>
                <%= Html.CheckBox("EditHelp_" + item.Id, item.IsEditHelp, new { id = "EditHelp_" + item.Id, onClick = "updateEditHelp('" + Url.Action("UpdateEditHelp", "MaintenanceAccount") + "','" + item.Id + "')", style = "margin-left: 13px;" })%>
            </td>
            <td>
                <a id='<%= "linkToRedired"+ item.Id %>' href='#'
                            onclick="javascript:redirectoToThisAction('<%=Url.Action("GetRedirection","MaintenanceAccount", new {id = item.Id, clientCompanyId = item.ClientCompany})%>','<%=item.Id %>','<%=item.ClientCompany %>'); return false;" target="_top"
                            >Login</a>
            </td>
        </tr>
        <% } %>
        </tbody>
         </table>         
        </div>

</asp:Content>
