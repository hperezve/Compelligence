<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.DataTransfer.Entity.ClientCompanyDTO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Company Detail</title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
   <%-- <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet"
        type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.meio.mask.min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $('#SubscriptionDueDate').datepicker({
                changeMonth: true,
                changeYear: true
            });
            $('#PaymentLastDate').datepicker({
                changeMonth: true,
                changeYear: true
            });
            $('#PaymentNextDate').datepicker({
                changeMonth: true,
                changeYear: true
            });
            $('#SubscriptionDueDate').setMask({ mask: '19/39/9999' });
            $('#PaymentLastDate').setMask({ mask: '19/39/9999' });
            $('#PaymentNextDate').setMask({ mask: '19/39/9999' });
            $('#SubscriptionRate').setMask({ mask: '99.999,999,999,999', type: 'reverse', defaultValue: '000' });
            $('#PaymentLastAmount').setMask({ mask: '99.999,999,999,999', type: 'reverse' });
            $('#PaymentNextAmount').setMask({ mask: '99.999,999,999,999', type: 'reverse' });
            $('#SubscriptionMaxUsers').setMask({ mask: '999999' });
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
        <%=Html.ActionLink("Return to List All Client Companies", "List", "MaintenanceAccount")%>
    </div>
    <div id="contentRegister">
        <%= Html.ValidationSummary()%>
        <% using (Html.BeginForm("EditCompany", "MaintenanceAccount", FormMethod.Post, new { id = "CompanyEditForm", align = "left" }))
           { %>
        <%= Html.Hidden("CompanyId")%>
        <div>
            <label>
                <%= HttpUtility.HtmlEncode(LabelResource.FormRequiredFieldsMessage)%></label>
        </div>
        <div class="marginTop10">
            <fieldset>
                <h2>
                    Client Company</h2>
                <div class="line">
                    <div class="field">
                        <label for="CompanyName" class="required">
                            <asp:Literal ID="CompanyName" runat="server" Text="<%$ Resources:LabelResource, CompanyName %>" />:</label>
                        <%= Html.TextBox("CompanyName")%>
                        <%= Html.ValidationMessage("CompanyName", "*")%>
                    </div>
                    <div class="field">
                        <label for="CompanyDns" class="required">
                            <asp:Literal ID="CompanyDns" runat="server" Text="<%$ Resources:LabelResource, CompanyDns %>" />:</label>http://
                        <%= Html.TextBox("CompanyDns", null, new { @style = "width: 100px;text-transform: lowercase;", @onchange = "javascript: trim(this);" })%>
                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:LabelResource, DomainUrl %>" />
                        <%= Html.ValidationMessage("CompanyDns", "*")%>
                    </div>
                    <div class="field">
                        <label for="CompanyEmail" class="required">
                            <asp:Literal ID="CompanyEmail" runat="server" Text="<%$ Resources:LabelResource, CompanyEmail %>" />:</label>
                        <%= Html.TextBox("CompanyEmail", null, new { @style = "text-transform: lowercase;", @onchange = "javascript: trim(this);" })%>
                        <%= Html.ValidationMessage("CompanyEmail", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="CompanyStatus" class="required">
                            <asp:Literal ID="CompanyStatus" runat="server" Text="<%$ Resources:LabelResource, CompanyStatus %>" />:</label>
                        <%= Html.DropDownList("CompanyStatus", (SelectList)ViewData["CompanyStatusList"])%>
                        <%= Html.ValidationMessage("CompanyStatus", "*")%>
                    </div>
                    <div class="field">
                        <label for="CompanyDescription" class="required">
                            <asp:Literal ID="CompanyDescription" runat="server" Text="<%$ Resources:LabelResource, CompanyDescription %>" />:</label>
                        <%= Html.TextArea("CompanyDescription")%>
                        <%= Html.ValidationMessage("CompanyDescription", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="CompanyCountryCode" class="required">
                            <asp:Literal ID="CompanyCountryCode" runat="server" Text="<%$ Resources:LabelResource, CompanyCountryId %>" />:</label>
                        <%= Html.DropDownList("CompanyCountryCode", (SelectList)ViewData["CountryCodeList"], string.Empty)%>
                        <%= Html.ValidationMessage("CompanyCountryCode", "*")%>
                    </div>
                    <div class="field">
                        <label for="CompanyAddress" class="required">
                            <asp:Literal ID="CompanyAddress" runat="server" Text="<%$ Resources:LabelResource, CompanyAddress %>" />:</label>
                        <%= Html.TextBox("CompanyAddress")%>
                        <%= Html.ValidationMessage("CompanyAddress", "*")%>
                    </div>
                    <div class="field">
                        <label for="CompanyCity" class="required">
                            <asp:Literal ID="CompanyCity" runat="server" Text="<%$ Resources:LabelResource, CompanyCity %>" />:</label>
                        <%= Html.TextBox("CompanyCity")%>
                        <%= Html.ValidationMessage("CompanyCity", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="CompanyState" class="required">
                            <asp:Literal ID="CompanyState" runat="server" Text="<%$ Resources:LabelResource, CompanyState %>" />:</label>
                        <%= Html.TextBox("CompanyState")%>
                        <%= Html.ValidationMessage("CompanyState", "*")%>
                    </div>
                    <div class="field">
                        <label for="CompanyZipCode" class="required">
                            <asp:Literal ID="CompanyZipCode" runat="server" Text="<%$ Resources:LabelResource, CompanyZipCode %>" />:</label>
                        <%= Html.TextBox("CompanyZipCode")%>
                        <%= Html.ValidationMessage("CompanyZipCode", "*")%>
                    </div>
                    <div class="field">
                        <label for="CompanyPhone" class="required">
                            <asp:Literal ID="CompanyPhone" runat="server" Text="<%$ Resources:LabelResource, CompanyPhone %>" />:</label>
                        <%= Html.TextBox("CompanyPhone")%>
                        <%= Html.ValidationMessage("CompanyPhone", "*")%>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="marginTop10">
            <fieldset>
                <h2>
                    Company Subscription</h2>
                <div class="line">
                    <div class="field">
                        <label for="SubscriptionRate">
                            <asp:Literal ID="SubscriptionRate" runat="server" Text="<%$ Resources:LabelResource, CompanySubscriptionRate %>" />:</label>
                        <%= Html.TextBox("SubscriptionRate")%>
                        <%= Html.ValidationMessage("SubscriptionRate", "*")%>
                    </div>
                    <div class="field">
                        <label for="SubscriptionCurrencyType">
                            <asp:Literal ID="SubscriptionCurrencyType" runat="server" Text="<%$ Resources:LabelResource, CompanySubscriptionCurrency%>" />:</label>
                        <%= Html.DropDownList("SubscriptionCurrencyType", (SelectList)ViewData["CurrencyList"])%>
                        <%= Html.ValidationMessage("SubscriptionCurrencyType", "*")%>
                    </div>
                    <div class="field">
                        <label for="SubscriptionDueDate">
                            <asp:Literal ID="SubscriptionDueDate" runat="server" Text="<%$ Resources:LabelResource, CompanySubscriptionDueDate %>" />:</label>
                        <%= Html.TextBox("SubscriptionDueDate")%>
                        <%= Html.ValidationMessage("SubscriptionDueDate", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="SubscriptionMaxUsers" class="required">
                            <asp:Literal ID="SubscriptionMaxUsers" runat="server" Text="<%$ Resources:LabelResource, CompanySubscriptionMaxUsers %>" />:</label>
                        <%= Html.TextBox("SubscriptionMaxUsers")%>
                        <%= Html.ValidationMessage("SubscriptionMaxUsers", "*")%>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="marginTop10">
            <fieldset>
                <h2>
                    Company Payments</h2>
                <fieldset>
                    <legend>Next Payment</legend>
                    <div class="line">
                        <div class="field">
                            <label for="PaymentNextDate">
                                <asp:Literal ID="PaymentNextDate" runat="server" Text="<%$ Resources:LabelResource, CompanyPaymentNextDate %>" />:</label>
                            <%= Html.TextBox("PaymentNextDate")%>
                            <%= Html.ValidationMessage("PaymentNextDate", "*")%>
                        </div>
                        <div class="field">
                            <label for="PaymentNextCurrencyType">
                                <asp:Literal ID="PaymentNextCurrencyType" runat="server" Text="<%$ Resources:LabelResource, CompanyPaymentNextCurrencyType %>" />:</label>
                            <%= Html.DropDownList("PaymentNextCurrencyType", (SelectList)ViewData["CurrencyList"])%>
                            <%= Html.ValidationMessage("PaymentNextCurrencyType", "*")%>
                        </div>
                        <div class="field">
                            <label for="PaymentNextAmount">
                                <asp:Literal ID="PaymentNextAmount" runat="server" Text="<%$ Resources:LabelResource, CompanyPaymentNextAmount %>" />:</label>
                            <%= Html.TextBox("PaymentNextAmount")%>
                            <%= Html.ValidationMessage("PaymentNextAmount", "*")%>
                        </div>
                    </div>
                    <div class="line">
                        <div class="field">
                            <label for="PaymentNextStatus">
                                <asp:Literal ID="PaymentNextStatus" runat="server" Text="<%$ Resources:LabelResource, CompanyPaymentNextStatus %>" />:</label>
                            <%= Html.DropDownList("PaymentNextStatus", (SelectList)ViewData["PaymentStatusList"])%>
                            <%= Html.ValidationMessage("PaymentNextStatus", "*")%>
                        </div>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>Last Payment</legend>
                    <div class="line">
                        <div class="field">
                            <label for="PaymentLastDate">
                                <asp:Literal ID="PaymentLastDate" runat="server" Text="<%$ Resources:LabelResource, CompanyPaymentLastDate %>" />:</label>
                            <%= Html.TextBox("PaymentLastDate")%>
                            <%= Html.ValidationMessage("PaymentLastDate", "*")%>
                        </div>
                        <div class="field">
                            <label for="PaymentLastCurrencyType">
                                <asp:Literal ID="PaymentLastCurrencyType" runat="server" Text="<%$ Resources:LabelResource, CompanyPaymentLastCurrencyType %>" />:</label>
                            <%= Html.DropDownList("PaymentLastCurrencyType", (SelectList)ViewData["CurrencyList"])%>
                            <%= Html.ValidationMessage("PaymentLastCurrencyType", "*")%>
                        </div>
                        <div class="field">
                            <label for="PaymentLastAmount">
                                <asp:Literal ID="PaymentLastAmount" runat="server" Text="<%$ Resources:LabelResource, CompanyPaymentLastAmount %>" />:</label>
                            <%= Html.TextBox("PaymentLastAmount")%>
                            <%= Html.ValidationMessage("PaymentLastAmount", "*")%>
                        </div>
                    </div>
                </fieldset>
            </fieldset>
        </div>
        <div class="marginTop10">
            <fieldset>
                <h2>
                    Salesforce.com</h2>
                <div class="line">
                    <div class="field">
                        <label>
                            <asp:Literal ID="CompanySalesForceToken" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForceToken%>" />:</label>
                        <%= HttpUtility.HtmlEncode(Model.SalesForceToken)%>
                    </div>
                    <div class="field">
                        <label>
                            <asp:Literal ID="CompanySalesForceUser" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForceUser%>" />:</label>
                        <%= HttpUtility.HtmlEncode(Model.SalesForceUser)%>
                    </div>
                    <div class="field">
                        <a href="<%=Url.Action("EditCompanySalesforce", "MaintenanceAccount") + "/" + Model.CompanyId %>">
                            Edit Salesforce data </a>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="marginTop10">
            <fieldset>
                <h2>
                    User Contact</h2>
                <div class="line">
                    <div class="field">
                        <label for="UserFirstName" class="required">
                            <asp:Literal ID="UserFirstName" runat="server" Text="<%$ Resources:LabelResource, UserFirstName %>" />:</label>
                        <%= Html.TextBox("UserFirstName")%>
                        <%= Html.ValidationMessage("UserFirstName", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserLastName" class="required">
                            <asp:Literal ID="UserLastName" runat="server" Text="<%$ Resources:LabelResource, UserLastName %>" />:</label>
                        <%= Html.TextBox("UserLastName")%>
                        <%= Html.ValidationMessage("UserLastName", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserEmail">
                            <asp:Literal ID="UserEmail" runat="server" Text="<%$ Resources:LabelResource, UserEmail %>" />:</label>
                        <%= HttpUtility.HtmlEncode(Model.UserEmail)%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="UserCountryCode" class="required">
                            <asp:Literal ID="UserCountryCode" runat="server" Text="<%$ Resources:LabelResource, UserCountryId %>" />:</label>
                        <%= Html.DropDownList("UserCountryCode", (SelectList)ViewData["CountryCodeList"], string.Empty)%>
                        <%= Html.ValidationMessage("UserCountryCode", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserAddress" class="required">
                            <asp:Literal ID="UserAddress" runat="server" Text="<%$ Resources:LabelResource, UserAddress %>" />:</label>
                        <%= Html.TextBox("UserAddress")%>
                        <%= Html.ValidationMessage("UserAddress", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserCity" class="required">
                            <asp:Literal ID="UserCity" runat="server" Text="<%$ Resources:LabelResource, UserCity %>" />:</label>
                        <%= Html.TextBox("UserCity")%>
                        <%= Html.ValidationMessage("UserCity", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="UserState" class="required">
                            <asp:Literal ID="UserDepartment" runat="server" Text="<%$ Resources:LabelResource, UserDepartment %>" />:</label>
                        <%= Html.TextBox("UserState")%>
                        <%= Html.ValidationMessage("UserState", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserZipCode" class="required">
                            <asp:Literal ID="UserZipCode" runat="server" Text="<%$ Resources:LabelResource, UserZipCode %>" />:</label>
                        <%= Html.TextBox("UserZipCode")%>
                        <%= Html.ValidationMessage("UserZipCode", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserPhone" class="required">
                            <asp:Literal ID="UserPhone" runat="server" Text="<%$ Resources:LabelResource, UserPhone %>" />:</label>
                        <%= Html.TextBox("UserPhone")%>
                        <%= Html.ValidationMessage("UserPhone", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="UserFax">
                            <asp:Literal ID="UserFax" runat="server" Text="<%$ Resources:LabelResource, UserFax %>" />:</label>
                        <%= Html.TextBox("UserFax")%>
                        <%= Html.ValidationMessage("UserFax", "*")%>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="marginTop10">
            <fieldset>
                <h2>
                    Settings</h2>
                <div class="line">
                    <div class="field">
                        <label for="SendAnyMail"><asp:Literal ID="DontSendAnyMail" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyDontSendAnyMailSet %>" />:</label>
                        <%= Html.CheckBox("SendAnyMail", Convert.ToBoolean(ViewData["DontSendAnyMailSet"]))%>
                    </div>
                    <div class="field">
                        <label for="ShowDashboardTab"><asp:Literal ID="ShowDashboardTab" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyShowDashboardTab %>" />:</label>
                        <%= Html.CheckBox("ShowDashboardTab", Convert.ToBoolean(ViewData["ShowDashboardTab"]))%>
                    </div>
                    <div class="field">
                        <label for="ShowCalendarTab"><asp:Literal ID="ShowCalendarTab" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyShowCalendarTab %>" />:</label>
                        <%= Html.CheckBox("ShowCalendarTab", Convert.ToBoolean(ViewData["ShowCalendarTab"]))%>
                    </div>
                    <div class="field">
                        <label for="ShowCalendarTab"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyUseSystemEmail %>" />:</label>
                        <%= Html.CheckBox("UseSystemEmail", Convert.ToBoolean(ViewData["UseSystemEmail"]))%>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="float-left marginLR5px">
            <input class="button" type="submit" value="Save" /></div>
        <div class="float-left">
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#ClientCompanyUserEditForm');" /></div>
        <% } %>
    </div>
</asp:Content>
