<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%= Html.ValidationSummary()%>
 

<% string formId = (string)ViewData["Scope"] + "WebsiteEditForm";
   string idWebSite = Session["CMSConfigId"].ToString();
    %>
<% using (Ajax.BeginForm("SaveDetail", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "WebsiteDetailFormContent",
               OnBegin = "showLoadingDialog",
               OnComplete = "function() { hideLoadingDialog();}",
               OnSuccess = "function(){}",
               OnFailure = "showFailedResponseDialog"
           }, new { id = formId}
           ))
   { %>
<div class="indexTwo">
    <div class="buttonLink">
        
        <input class="button" type="submit" value="Save" />
        <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" />
        <input class="button" type="button" value="Cancel" onclick="javascript: GoToEntity('<%=idWebSite%>','WEBSITE','Website','./Website.aspx/Edit');" />
    </div>
    <div id="accordion" class="contentFormEdit">
        <% string[] titles = new string[] { "Root", "Industry", "Competitor", "Product" };
           for (int i = 0; i < 4; i++)
           {%>
        <h3>
            <a href="#">
                <%=titles[i] %></a></h3>
        <div>
            <div>
                <table class="WebsiteTable">
                    <thead>
                        <tr>

                            <td class="columnType">
                            
                            
                             <asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:LabelResource, WebsiteEditDetailContentType %>" />
                            <%-- ContentType --%>
                            
                            </td>
                            <td class="columnDisplay">

                            <asp:Literal ID="Literal2"  runat="server" Text="<%$ Resources:LabelResource, WebsiteEditDetailDisplayable %>" />
                            <%-- Displayable --%>
                                

                            </td>
                            <td class="columnAjust">
                            <asp:Literal ID="Literal3"  runat="server" Text="<%$ Resources:LabelResource, WebsiteEditDetailAjust %>" />
                            <%-- Ajust --%>

                            </td>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="sortlist">
                <%List<WebsiteDetail> Details = (List<WebsiteDetail>)ViewData["CMSDetails" + i];
                  foreach (WebsiteDetail Detail in Details)
                  {
                      if (!Detail.ContentType.Name.Equals("More Details"))
                      {
                      %>
                <div class="rowDetail" id="row_<%=Detail.Id %>">
                    <img src="<%= Url.Content("~/Content/Images/Styles/scroller-td.gif") %>" class="icono float-left" alt="move" />
                    <div class="contentTypeDetailWebsite float-left">
                        <%=Html.Hidden("CMSConfigId", Detail.WebsiteId)%>
                        <%=Html.Hidden("Level", Detail.Level)%>
                        <label>
                            <%=Detail.ContentType.Name%></label>
                        <%=Html.Hidden("ContentTypeId", Detail.ContentTypeId)%>
                    </div>
                    <% var list = new[] { new  { Id = "Y", Name = "Yes" }, 
                                             new  { Id = "N", Name = "No" }};
                    %>
                    <div class="float-left">
                        <%=Html.DropDownList("Displayable", new SelectList(list, "Id", "Name", Detail.Displayable), new { style = "width:75px" })%>
                    </div>
                    <div class="float-left">
                        <select name="Ajust" style="width:100px">
                            <option value="S" <%=Detail.Ajust=="S" ? "SELECTED" : "" %>>Two Column</option>
                            <option value="T" <%=Detail.Ajust=="T" ? "SELECTED" : "" %>>Single Column</option>
                        </select>
                    </div>
                </div>
                <%}
                  } %>
            </div>
        </div>
        <%}%>
    </div>
</div>
<% } %>

<script type="text/javascript">
    $(document).ready(function() {

        function ActiveSortable(collectionId, typeCollection, targetviewer) {
            $(collectionId).sortable({
                items: typeCollection,
                cursor: "pointer",
                opacity: 0.7,
                handle: "img.icono",
                update: function() { }
            });
        }

        ActiveSortable("div.sortlist", "> div", "input#sortlist");
        $("#accordion").accordion();

    });
</script>

