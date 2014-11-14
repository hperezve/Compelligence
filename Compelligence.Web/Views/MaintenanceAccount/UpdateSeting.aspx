<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titleTextTwo">
        Update Settings</div>
    <div id="contentRegister">
        <div class="marginTop10">
            <fieldset>
                <% using (Html.BeginForm("UpdateSetting", "MaintenanceAccount", FormMethod.Post, new { id = "idformchangecredential" }))
                   { %>
                <div class="line">
                    <div class="field">
                        <label for="BingKey">
                            <asp:Literal ID="BingKey" runat="server" Text="Bing Key" />:</label>
                        <%= Html.TextBox("BingKey", ViewData["BingKey"].ToString(), new { @style = "width: 460px;" })%>
                    </div>
                </div>
                <div class="float-left marginLR5px">
                    <input class="button" type="submit" value="Save" /></div>
                <div class="float-left">
                    <input class="button" type="button" value="Reset" /></div>
                <%} %>
            </fieldset>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
