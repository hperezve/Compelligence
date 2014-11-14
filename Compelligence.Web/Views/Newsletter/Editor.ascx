<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Newsletter>" %>
<%@ Import Namespace="Compelligence.Common.Utility" %>
<%@ Import Namespace="Compelligence.Util.Type" %>

<% string formId = ViewData["Scope"].ToString() + "NewsletterEditorFormContent"; %>

<style type="text/css">
 .newsletterhidden
 {
 	display:none;
 	width:100%;
 }
 .newslettervisible
 {
 	display:block;
 	width:100%;
 } 
</style>
<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>');
        });

        var copyTextAreas = function() {

            var txtEditor = CKEDITOR.instances.WorkspaceNewsletterEditorFormContentOpeningText.getData();
            $('#WorkspaceNewsletterEditorFormContentOpeningText').prop("value", String(txtEditor));
            //$("textarea#WorkspaceNewsletterEditorFormContentOpeningText").val(txtEditor);
        }
</script>


<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm("SaveEdition", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "NewsletterEditorFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Newsletter', '" + ViewData["BrowseId"] + "', " + "'false');}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        
        <% if (ViewData["UserSecurityAccess"].ToString().Equals(UserSecurityAccess.Edit))
           { %>
        <div class="buttonLink">
            <input class="button" type="submit" value="Save" onclick="copyTextAreas();" />
            <%--<input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" />
            <input class="button" type="button" value="Cancel" onclick="javascript: cancelEntity('<%= ViewData["Scope"] %>', 'Newsletter', '<%= ViewData["BrowseId"] %>','false');" />--%>
        </div>
        <% } %>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("IdNews", default(decimal))%>        
        <%= Html.Hidden("Title")%>
        <%= Html.Hidden("AssignedTo")%>
        <%= Html.Hidden("TemplateId")%>
        <%= Html.Hidden("DestinationMail")%>
        
        <div class="contentFormEdit resizeMe">
        
            <div style="overflow:auto;border-right:none;" class="content">       
        
            <div class="line">
                <div class="field">
                   <label for="<%= formId %>OpeningText">
                    <asp:Literal ID="OpeningText" runat="server" Text="<%$ Resources:LabelResource, NewsletterOpeningText %>" />:</label>
                    <p>
                     <%=Html.TextArea("OpeningText", new { id = formId + "OpeningText", Class = "ckeditor"  })%>
                     <script type="text/javascript">
                         if (!CKEDITOR.instances.WorkspaceNewsletterEditorFormContentOpeningText)
                             CKEDITOR.replace('WorkspaceNewsletterEditorFormContentOpeningText');
                         else {
                             //CKEDITOR.instances.WorkspaceNewsletterEditorFormContentOpeningText.destroy();ç
                             CKEDITOR.remove(CKEDITOR.instances['WorkspaceNewsletterEditorFormContentOpeningText']);
                             CKEDITOR.replace('WorkspaceNewsletterEditorFormContentOpeningText');
                         }   
                     </script>
                   </p>
                </div>
            </div>
         
            
  <div id="SectionPanelGroup" style="width:100%;float:left">
     <div id="SectionPanel" style="width:100%;display:none">
         <div id="SectionTitle" style="width:85%;float:left">
            <label>Section :</label>
            <input type="text" id="txtTitle" name="txtTitle" style=" border: none;width:580px"/>
            <label>Description :</label><textarea id="txtDescription" name="txtDescription" class="nsDescription"></textarea>
         </div>
         <div  style="width:15%;float:left">
            <input type="button" id="btnAdd"    onclick="javascript: void(0)" value="+" title="Add Project/Library" /> <br/>
            <input type="button" id="btnRemove" onclick="javascript: void(0)" value="-" title="Remove this Section" /> 
         </div>
         <div id="SectionItem">
            
            <div class="newsletterhidden" style="margin-left:100px;float:left">
              <label>Item :</label><input type="text" id="txtItem" name="txtItem" style="width:390px"/> 
              <textarea id="txtItemComment" name="txtItemComment" rows=10 cols=20></textarea>
            </div>
            <br />
         </div>
    </div>

     
     <% MultiList<Pair<string, string>> sections = (MultiList<Pair<string, string>>)ViewData["Sections"];
        if (sections == null) sections = new MultiList<Pair<string, string>>();
        int i = 0;
       foreach(NodeMultiList<Pair<string,string>> section in sections.Items)
       {
           i++;
           %> 
         <div id="SectionPanel_<%=i%>" style="width:100%; visibility:hidden" class="SectionPanel">
             <div id="SectionTitle" style="width:85%;float:left">
                <label>Section :</label>
                <input type="text" id="Text1" name="txtTitle" style="border: none;width:580px" value='<%=section.Value.First %>'/>
                <label>Description :</label><textarea id="txtDescription" name="txtDescription" class="nsDescription"><%=section.Value.Second%></textarea>                
             </div>
             <div  style="width:15%;float:left">
                <input type="button" id="btnAdd" onclick="javascript: loadSelectedFunction='coco';loadPopup('','NewsLetterSection','SectionPanel_<%=i%>');" value="+" title="Add Project/Library" /> <br />
                <input type="button" id="btnRemove" onclick="javascript: removeSection('SectionPanel_<%=i%>')" value="-" title="Remove this Section" /> 
             </div>
             <div id="SectionItem">
                   <div class="newsletterhidden" style="margin-left:100px;float:left">
                      <span>Item :</span><input type="text" id="txtItem" name="txtItem" style="width:390px"/> 
                      <textarea id="txtItemComment" name="txtItemComment" rows=10 cols=20></textarea>
                    </div>             
                <% foreach (NodeMultiList<Pair<string,string>> item in section.Items)
                  {%>
                   <div style="margin-left:100px;float:left">
                      <label>Item :</label><input type="text" id='txtItem_<%=i%>' name="txtItem_<%=i%>" value='<%=item.Value.First %>'style="width:390px" /> 
                      <textarea id="txtItemComment_<%=i%>" name="txtItemComment_<%=i%>" rows=10 cols=20 style="text-align:left"><%=item.Value.Second%></textarea>
                   </div>
                <%}%>
                <br />
             </div>
        </div>
            
     <%}%>

  </div>       
  
              <br />
              <div class="line">
                <div class="field">
                  <label for="<%= formId %>ClosingText">
                    <asp:Literal ID="CloseText" runat="server" Text="<%$ Resources:LabelResource, NewsletterClosingText %>" />:</label>
                    <p>
                     <%=Html.TextArea("ClosingText", new { id = formId + "ClosingText", Class = "ckeditor"})%>
                     <script type="text/javascript">
                         if (!CKEDITOR.instances.WorkspaceNewsletterEditorFormContentClosingText)
                             CKEDITOR.replace('WorkspaceNewsletterEditorFormContentClosingText');
                         else {
                             //  CKEDITOR.instances.WorkspaceNewsletterEditorFormContentClosingText.destroy();
                             CKEDITOR.remove(CKEDITOR.instances['WorkspaceNewsletterEditorFormContentClosingText']);
                             CKEDITOR.replace('WorkspaceNewsletterEditorFormContentClosingText');
                         }
                     </script>
                   </p>
                </div>
              </div>

              
            </div>
              
        </div>
  </fieldset>
</div>
<% } %>
   




