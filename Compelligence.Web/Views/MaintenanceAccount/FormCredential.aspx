<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="titleTextTwo">
        Change User and Password admin</div>
    <div id="contentRegister">
        <div class="marginTop10">
            <fieldset>
            <a href="<%= Url.Action("List","MaintenanceAccount") %>">Return</a>
            <%= Html.ValidationSummary()%>
            
            <% using (Html.BeginForm("ChangeCredential", "MaintenanceAccount", FormMethod.Post, new { id="idformchangecredential"}))
               { %>
                   <table>
                       <tr>
                        <td>Old login:</td>
                        <td><input name="olduser" type="text" id="idolduser"/><%= Html.ValidationMessage("olduser", "*")%></td>
                       </tr>
                       <tr>
                        <td>Old password:</td>
                        <td><input name="oldKennwort" type="password" id="idoldpassword" autocomplete="off" /><%= Html.ValidationMessage("oldpassword", "*")%></td></tr>
                       <tr>
                        <td>New login</td>
                        <td><input name="user" type="text" id="iduser"/><%= Html.ValidationMessage("user", "*")%></td></tr>
                       <tr>
                        <td>New password:</td>
                        <td><input name="Kennwort" type="password" id="idpassword" autocomplete="off" /><%= Html.ValidationMessage("password", "*")%></td></tr>
                       <tr>
                        <td colspan="2"><input type="submit" value="Change" /></td>
                        </tr>
                   </table>
                <%} %>
            </fieldset>
        </div>
    </div>
    
    
    
    
    
    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
