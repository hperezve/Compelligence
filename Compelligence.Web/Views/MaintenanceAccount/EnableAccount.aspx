<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Enable Company</title>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% string formId = "ClientCompanyForm"; %>
    <div style="text-align: right">
        <%= Html.ActionLink("Return to Home", "Index", "Home")%>
    </div>
    <div class="titleTextOne">
        Enable Company in Compelligence Systems</div>
    <div class="titleTextTwo">
        your Competitive Intelligence Systems!!</div>
    <div id="contentRegister">
        <%= Html.ValidationSummary()%>
        <% using (Html.BeginForm("EnableAccount", "MaintenanceAccount", FormMethod.Post, new { id = formId, align = "left" }))
           { %>
        <div class="marginTop10">
            <fieldset>
                <legend>Register
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, Company %>" /></legend>
                <%= Html.Hidden("Id", default(decimal))%>
                <div class="line">
                    <div class="field">
                        <label for="Name">
                            <asp:Literal ID="ClientCompanyName" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyName %>" />:</label>
                        <%= Html.DropDownList("Name", (SelectList)ViewData["ClientCompanyList"], string.Empty)%>
                        <%= Html.ValidationMessage("ClientCompanyName", "*")%>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="buttonLink">
            <input class="button" type="submit" value="Enable" />
            <input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("Index", "MaintenanceAccount")%>'" />
        </div>
    </div>
    <% } %>
</asp:Content>
