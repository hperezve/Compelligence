<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Compelligence.Domain.Entity.Views.ClientCompanyAllView>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/tinytable.style.css") %>" rel="stylesheet" type="text/css" />
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.meio.mask.min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"  type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script> 
    
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

    <link href="<%= Url.Content("~/Content/Styles/StyleSorterTabla.css") %>" rel="stylesheet" type="text/css" />

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/jquery.tablesorter.js") %>"  type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("table").tablesorter({ debug: true });
            
            $("th").mouseover(function(){
                $(this).css("color", "#FFFFFF");
	            $(this).css("background-color", "#464647");
            }).mouseout(function(){
                $(this).css("color", "#000000");
	            $(this).css("background-color", "#EEEFEF");
            });
            
            $('input[name=PaymentNextDate]').datepicker({
                changeMonth: true,
                changeYear: true
            }).setMask({ mask: '19/39/9999' });
        });

        function updateNextPaymentDate(companyid) {
            $.post('<%= Url.Action("UpdateNextPaymentDate", "MaintenanceAccount") %>/' + companyid, { npd: $('#' + companyid + 'PaymentNextDate').val() });
        }

        function updateCompanyStatus(companyid) {
            $.post('<%= Url.Action("UpdateCompanyStatus", "MaintenanceAccount") %>/' + companyid, { sts: $('#' + companyid + 'Status').val() });
        }

        function updateCompanies(itemBox, companyId) {
            var hidenvalues = $('#HdnIds').val();
            if (itemBox.checked) {
                if (hidenvalues.indexOf(companyId) == -1) {
                    var tempo = '';
                    if (hidenvalues != '') {
                        tempo += ':';
                    }
                    tempo += companyId;
                    hidenvalues += tempo;
                    $('#HdnIds').val(hidenvalues); 
                }
            }
            else {
                if (hidenvalues.indexOf(companyId) != -1) {
                    if (hidenvalues.indexOf(':' + companyId) != -1) {
                        hidenvalues = hidenvalues.replace(':'+ companyId, '');
                    }
                    else if (hidenvalues.indexOf(companyId+':') != -1) {
                        hidenvalues = hidenvalues.replace(companyId + ':', '');
                    }
                    else {
                        hidenvalues = hidenvalues.replace(companyId, '');
                    }
                    $('#HdnIds').val(hidenvalues); 
                }
            }
        }
        
        function permanentlyDelete() {
            var hidenvalues = $('#HdnIds').val();
            var urlAction = '<%= Url.Action("PermanentlyDeleteCompanies", "MaintenanceAccount") %>';
            var urlAction2 = '<%= Url.Action("List", "MaintenanceAccount") %>';
            ConfirmToDeleteDDlg(urlAction, urlAction2,'Delete Companies', hidenvalues);
        }

        function UnDelete() {
            var hidenvalues = $('#HdnIds').val();
            var urlAction = '<%= Url.Action("ReturnToTopList", "MaintenanceAccount") %>';
            var urlAction2 = '<%= Url.Action("List", "MaintenanceAccount") %>';
            $.ajax({
                url: urlAction,
                type: 'POST',
                data: { Id: hidenvalues },
                traditional: true,
                success: function(Data) {
                    if (Data != "") {
                        location.href = urlAction2;
                    }
                }
            });
        }


        function executeActionsToDetele() {
            var hidenvalues = $('#HdnIds').val();
            return hidenvalues;
        }
        
        var OnClickSelectUserResetPassword = function(par) {
            var $link = $('#' + par);
            var $dialog = $('<div></div>').load($link.prop('href')).dialog({
                autoOpen: false,
                modal: true,
                title: $link.prop('title'),
                width: 400,
                buttons: {
                    Ok: function() {
                        $_form = $('#ClientCompanyForm');
                        $_form.submit();
                        $(this).dialog('close');
                    },
                    Cancel: function() {
                        $(this).dialog('close');
                    }
                }
            });

            $dialog.dialog('open');
            return false;
        };

    </script>

    <div class="marginTop10">
        <fieldset id="ListTable">
            <div class="line">
                <div class="field">
                    <a href="<%= Url.Action("ViewFormCredential","MaintenanceAccount") %>">Change user/password
                    </a>
                </div>
                <div class="field">
                    <%= Html.ActionLink("Generate Report", "GenerateReportClient", "Report")%></div>
                <div class="field">
                    <%= Html.ActionLink("White Papers", "Index", "WhitePaper")%>
                </div>
                <div class="field">
                    <%= Html.ActionLink("Configure Email", "ConfigurationEmail", "MaintenanceAccount")%>
                </div>
                <div class="field">
                    <%= Html.ActionLink("Logout", "Logout", "MaintenanceAccount")%></div>
                
            </div>
            <h2>
                List (
                <%=ViewData["ClientCompanyCount"] %>
                Companies )
            </h2>
            <table class="tablesorter">
                <thead>
                    <tr>
                        <th>
                            Name
                        </th>
                        <th>
                            Email
                        </th>
                        <th>
                            Dns
                        </th>
                        <th>
                            Created Date
                        </th>
                        <th>
                            Next Bill Due Date
                        </th>
                        <th>
                            Details
                        </th>
                        <th>
                            Users
                        </th>
                        <th>
                            Status Company
                        </th>
                        <th>
                            Delete Company
                        </th>
                        <th>
                            Reset Password
                        </th>
                         <th>
                           Files
                        </th>
                    </tr>
                </thead>
                <%  
                    int index = 0;
                    foreach (var item in Model)
                    { %>
                <tr>
                    <td>
                        <%= Html.Encode(item.Name) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Email) %>
                    </td>
                    <td>
                        <%= Html.Encode(item.Dns) %>
                    </td>
                    <td>
                        <%= Html.Encode(DateTimeUtility.ConvertToString(item.CreatedDate, "MM/dd/yyyy"))%>
                    </td>
                    <td>
                        <div style="width: 160px;">
                            <%= Html.TextBox("PaymentNextDate", DateTimeUtility.ConvertToString(item.PaymentNextDate, "MM/dd/yyyy"), new { id = item.Id + "PaymentNextDate", Style = "width:100px;" })%>
                            <input type="button" id="BtnPaymentNextDate" value="Update" style="width: 50px;"
                                onclick="javascript:updateNextPaymentDate('<%= item.Id %>');" />
                        </div>
                    </td>
                    <td>
                        <%= Html.ActionLink("Detail", "EditCompany", "MaintenanceAccount", new { id = item.Id }, null)%>
                    </td>
                    <td>
                        <%= Html.ActionLink(Html.Encode(item.NumUsers), "GetUsersOfCompany", "MaintenanceAccount", new { id = item.Id }, null)%>
                    </td>
                    <td>
                        <%= Html.DropDownList("CompanyStatusList", ((IList<SelectList>)ViewData["ListStatusList"])[index], new { id = item.Id + "Status", Style = "width:100px;", onchange = "javascript: updateCompanyStatus('" + item.Id + "');" })%>
                    </td>
                    <td>
                        <%--<a href="#" onclick="ConfirmDlg('<%= Url.Action("DeleteAccountFromList", "MaintenanceAccount") + "/" + item.Id%>');">
                            Delete</a>--%>
                            <a href="#" onclick="ConfirmDlg('<%= Url.Action("UpdateToDeleteFromList", "MaintenanceAccount") + "/" + item.Id%>');">
                            Delete</a>
                    </td>
                    <td>
                        <a id='<%= "link"+ item.Id %>' href='<%=Url.Action("ViewFormResetPassword","MaintenanceAccount", new {id = item.Id })%>'
                            onclick="javascript:OnClickSelectUserResetPassword('<%= "link"+ item.Id %>'); return false;"
                            title="Reset User Password">Reset password</a>
                    </td>
                    <td>
                        <%= Html.ActionLink("files", "Index", "ClientCompanyFiles", new { id = item.Id}, null)%>
                    </td>
                    
                </tr>
                <% index++;
                    } %>
            </table>
        </fieldset>
    </div>
    <div>
            <% decimal companiesToDeleteCount = Decimal.Parse( ViewData["CompaniesToDeleteCount"].ToString());
               if (companiesToDeleteCount > 0)
               {%> 
            <h2>
                To Be Deleted (<%= companiesToDeleteCount %>).
            </h2>
            <table class="tablesorter">
                <thead>
                    <tr>
                        <th>
                            
                        </th>
                        <th>
                            Name
                        </th>
                        <th>
                            Email
                        </th>
                        <th>
                            Dns
                        </th>
                        <th>
                            Created Date
                        </th>
                        <th>
                            Details
                        </th>
                        <th>
                            Users
                        </th>
                        <th>
                           Deleted Date
                        </th>
                    </tr>
                </thead>
                <%  
                int indexBotton = 0;
                IList<Compelligence.Domain.Entity.Views.ClientCompanyAllView> companyToDeleteList = (IList<Compelligence.Domain.Entity.Views.ClientCompanyAllView>)ViewData["CompaniesToDeletelist"];
                foreach (Compelligence.Domain.Entity.Views.ClientCompanyAllView item in companyToDeleteList)
                { %>
                <tr>
                    <td>
                        <%= Html.CheckBox("delete", new { id = "delete" + indexBotton, onclick = "updateCompanies(this, '"+item.Id+"')" })%>
                    </td>
                    <td>
                        <%= Html.Encode(item.Name)%>
                    </td>
                    <td>
                        <%= Html.Encode(item.Email)%>
                    </td>
                    <td>
                        <%= Html.Encode(item.Dns)%>
                    </td>
                    <td>
                        <%= Html.Encode(DateTimeUtility.ConvertToString(item.CreatedDate, "MM/dd/yyyy"))%>
                    </td>
                    <td>
                        <%= Html.ActionLink("Detail", "EditCompany", "MaintenanceAccount", new { id = item.Id }, null)%>
                    </td>
                    <td>
                        <%= Html.ActionLink(Html.Encode(item.NumUsers), "GetUsersOfCompany", "MaintenanceAccount", new { id = item.Id }, null)%>
                    </td>
                    <td>
                        <%= Html.Encode(DateTimeUtility.ConvertToString(item.MarkedDeleteDate, "MM/dd/yyyy"))%>
                    </td>
                    
                </tr>
                <% indexBotton++;
                } %>
            </table>
            <input type="hidden" id="HdnIds" value="" />
            <input class="button" type="button" value="Permanently delete checked companies" onclick="permanentlyDelete();" />
            <input class="button" type="button" value="Undelete" onclick="UnDelete();" />
            <% } %>
    </div>
  
    
    <div id="FormMessages" align="center">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
