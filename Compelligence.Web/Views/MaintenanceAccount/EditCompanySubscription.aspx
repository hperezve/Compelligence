<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.DataTransfer.Entity.CompanySubscriptionDTO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Edit Company Subscription</title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet"
        type="text/css" />--%>
<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.meio.mask.min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function(){
            $('#SubscriptionDueDate').datepicker({
                changeMonth: true,
                changeYear: true
            });
             $('#SubscriptionDueDate').setMask({ mask: '19/39/9999' });
             $('#SubscriptionRate').setMask({ mask : '99.999,999,999,999', type : 'reverse', defaultValue: '000' });
             $('#SubscriptionMaxUsers').setMask({ mask : '999999'});
        });
    </script>

    <style type="text/css">
        .ui-widget
        {
            font-size: 0.8em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: right;">
        <a href="<%=Url.Action("EditCompany", "MaintenanceAccount") + "/" + Model.CompanyId %>">
            Return to Company </a>
    </div>
    <%= Html.ValidationSummary()%>
    <% using (Html.BeginForm("EditCompanySubscription", "MaintenanceAccount", FormMethod.Post, new { id = "CompanySubscriptionEditForm" }))
       { %>
    <%= Html.Hidden("CompanyId")%>
    <%= Html.Hidden("SubscriptionId")%>
    <div>
        <label>
            <%= HttpUtility.HtmlEncode(LabelResource.FormRequiredFieldsMessage)%></label>
    </div>
    <div class="marginTop10">
        <fieldset>
            <h2>
                Subscription for Company
                <%= HttpUtility.HtmlEncode(Model.CompanyName) %></h2>
            <div class="line">
                <div class="field">
                    <label for="SubscriptionRate" class="required">
                        <asp:Literal ID="SubscriptionRate" runat="server" Text="<%$ Resources:LabelResource, CompanySubscriptionRate %>" />:</label>
                    <%= Html.TextBox("SubscriptionRate")%>
                    <%= Html.ValidationMessage("SubscriptionRate", "*")%>
                </div>
                <div class="field">
                    <label for="SubscriptionCurrency" class="required">
                        <asp:Literal ID="SubscriptionCurrency" runat="server" Text="<%$ Resources:LabelResource, CompanySubscriptionCurrency%>" />:</label>
                    <%= Html.DropDownList("SubscriptionCurrency", (SelectList)ViewData["CurrencyList"], string.Empty)%>
                    <%= Html.ValidationMessage("SubscriptionCurrency", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="SubscriptionDueDate" class="required">
                        <asp:Literal ID="SubscriptionDueDate" runat="server" Text="<%$ Resources:LabelResource, CompanySubscriptionDueDate %>" />:</label>
                    <%= Html.TextBox("SubscriptionDueDate")%>
                    <%= Html.ValidationMessage("SubscriptionDueDate", "*")%>
                </div>
                <div class="field">
                    <label for="SubscriptionMaxUsers" class="required">
                        <asp:Literal ID="SubscriptionMaxUsers" runat="server" Text="<%$ Resources:LabelResource, CompanySubscriptionMaxUsers %>" />:</label>
                    <%= Html.TextBox("SubscriptionMaxUsers")%>
                    <%= Html.ValidationMessage("SubscriptionMaxUsers", "*")%>
                </div>
            </div>
        </fieldset>
    </div>
    <div class="float-left marginLR5px">
        <input class="button" type="submit" value="Save" /></div>
    <div class="float-left">
        <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#CompanySubscriptionEditForm');" /></div>
    <% } %>
</asp:Content>
