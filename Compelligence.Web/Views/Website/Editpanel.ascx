<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%= Html.ValidationSummary()%>
<% string formId = (string)ViewData["Scope"] + "WebsiteEditFormPanel";
   string idWebSitePanel = Session["CMSConfigId"].ToString();
%>

<% using (Ajax.BeginForm("SavePanel", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "WebsitePanelFormContent",
               OnBegin = "showLoadingDialog",
               OnComplete = "function() { hideLoadingDialog();}",
               OnSuccess = "function(){}",
               OnFailure = "showFailedResponseDialog"
           }, new { id = formId }
           ))
   { %>
<div class="indexTwo">
    <div class="buttonLink">
        <input class="button" type="submit" value="Save" />
        <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" />
        <input class="button" type="button" value="Cancel" onclick="javascript: GoToEntity('<%=idWebSitePanel%>','WEBSITE','Website','./Website.aspx/Edit');" />
    </div>
    <br />
    <br />
    <div class='div_rounding' style="width:400px;float:left;background-color:White;margin-left:50px" >
        <div>
            <table style="width:390px">
               <colgroup>
                  <col width='300px'/>
                  <col width='90px'/>
                </colgroup>            
                <thead>
                    <tr>
                        <td class="columnType">
                        <label><asp:Literal ID="WebsiteEditPanelComponents" runat="server" Text="<%$ Resources:LabelResource, WebsiteEditPanelComponents %>" /></label>
                        </td>
                        <td class="columnDisplay">
                         <label><asp:Literal ID="WebsiteEditPanelDisplayable" runat="server" Text="<%$ Resources:LabelResource, WebsiteEditPanelDisplayable %>" /></label>
                        </td>
                    </tr>
                </thead>
            </table>
        </div>
        <div class="sortlist">
            <%List<WebsitePanel> Details = (List<WebsitePanel>)ViewData["WebsitePanels"];
              foreach (WebsitePanel Detail in Details)
              {
                  bool disableCtrl = (Detail.ComponentType.Equals("MENU") || Detail.ComponentType.Equals("BODY"));
                  if (!disableCtrl)
                  {
                  %>
            <div class="rowDetail" id="row_<%=Detail.Id %>">
                <img src="<%= Url.Content("~/Content/Images/Styles/scroller-td.gif") %>" class="icono float-left" alt="move" />
                <div class="float-left" style="width:250px">
                    <%=Html.Hidden("WebsiteId", Detail.WebsiteId)%>
                    <%=Html.Hidden("Level", Detail.Level)%>
                    <%=Html.Hidden("ComponentType", Detail.ComponentType)%>
                    <%=Html.Hidden("ComponentName", Detail.ComponentName)%>
                    <label>
                        <%=Detail.ComponentType%> - <%=Detail.ComponentName%></label>
                        <%=Html.Hidden("WebsitePanelId", Detail.Id)%>
                </div>
                <% var list = new[] { new  { Id = "Y", Name = "Yes" }, 
                                         new  { Id = "N", Name = "No" }};
                %>
                <div class="float-left">
                    <%=Html.DropDownList("Displayable", new SelectList(list, "Id", "Name", Detail.Displayable), new { style = "width:75px" })%>
                </div>
            </div>
            <%}
              }%>
        </div>
   </div>
   <div style="width:400px;height:300px;float:right;margin-right:50px">
      <div class='div_rounding' style="width:93%;height:30px;text-align:center">MENU</div>
      <div class='div_rounding'style="width:70%;height:200px;float:left;text-align:center"><span style="position:relative;top:50%">BODY</span></div>
      <div class='div_rounding'style="width:20%;height:200px;float:left;text-align:center"><span style="position:relative;top:50%">PANELS</span></div>
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


    });
</script>


