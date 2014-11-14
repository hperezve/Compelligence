<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%var scope = ViewData["Scope"]; %>

<script type="text/javascript">
    $(function(){
        if ($('#<%= ViewData["Scope"] %>FileCheckIn').length) {
            initializeUploadField('<%= Url.Action("CheckIn", "File") %>', '<%= ViewData["Scope"] %>', 'FileDetail', '#<%= ViewData["Scope"] %>FileCheckIn', '#<%= ViewData["Scope"] %>FileUploadResult', null, '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>', false);
        }
    });
    
    var downloadTemplate = function(field) {
        var template = $(field).val();
        
        if ((template != null) && (template != '')) {
            downloadTemplateFile('<%= Url.Action("CheckOutTemplate", "File") %>', '<%= ViewData["Scope"] %>', template);
        } else {
            alert('Select a valid Template');
            return false;
        }
    };

</script>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'FileDetail');
    }).trigger('resize');
</script>

<script type="text/javascript">

    var AddDialogLink = function(ff) {

       
        $("#ProjectFileLinkDialog").dialog({
            bgiframe: true,
            autoOpen: false,
            modal: true,
            buttons: {
                Ok: function() {
                    DataID();
                    $("#ProjectFileLink").val("");
                    $(".AddLink").attr("checked", true);                    
                    $(this).dialog('close');

                }
            }
           
        });
        $("#ProjectFileLinkDialog").dialog('open');
    };   

    var DataID = function() {
        var typeLink = $('#TypeLink:checked').val();
        var val = $("#ProjectFileLink").val();
        var url = '<%=Url.Action("SaveFileLink","File") %>';
        var FilterId = '<%=ViewData["EntityIdfilter"]%>';
        showLoadingDialogForSection('<%=ViewData["Container"]%>');
        $.getJSON(url, { valuelink: val, EntityId: FilterId, TypeLink: typeLink }, function(data) {
            if (data.toString().length > 0) {
                hideLoadingDialogForSection('<%=ViewData["Container"]%>');                
                
                if (data.message == "Fail") {
                    $("#ProjectFileLink").val("");
                    $('#AlertReturnMessageDialog').html('Invalid link location specified.');
                }
                else if (data.message == "ok") {
                $('#AlertReturnMessageDialog').html('The file was successfully uploaded.');
                    reloadGrid('#WorkspaceProjectFileDetailListTable');
                }
                else if (data.message == "StandardFileError") {
                $('#AlertReturnMessageDialog').html('Warning! Non-standard file types (.pdf, .doc, .xls, etc) might not be fully saved as a file.');
                reloadGrid('#WorkspaceProjectFileDetailListTable');
                }
                else {
                    $("#ProjectFileLink").val("");                    
                    $('#AlertReturnMessageDialog').html('There was an error uploading the file. Please try again.');
                }
                $('#AlertReturnMessageDialog').dialog('open');
            }
        });

    }
    

</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>FileDetailDataListContent">
        <% if (ViewData["UserSecurityAccess"].ToString().Equals(UserSecurityAccess.Edit))
           { %>
        <% if (!ViewData["HeaderType"].ToString().Equals(DomainObjectType.Template))
           {
        %>
        <br />
        <div id="<%= ViewData["Scope"] %>FileDetailDataFormContent">
            Template:
            <%= Html.DropDownList("TemplateFile", (SelectList)ViewData["TemplateList"], string.Empty)%>
            <input class="button" type="button" value="Download" onclick="javascript: downloadTemplate('#TemplateFile');" />
        </div>
        <br />
        <% } %>
         <div class="divCheckIn">
        <label id="<%= ViewData["Scope"] %>FileUploadResult"></label>
        </div>
        <asp:Panel ID="FileDetailToolbarContent" runat="server" CssClass="buttonLink">
       
            <input class="button" id="<%= ViewData["Scope"] %>FileCheckIn" type="button" value="Check In" />
            <input class="button" id="<%= ViewData["Scope"] %>FileCheckOut" type="button" value="Check Out"
                                onclick="javascript: downloadFileById('<%= Url.Action("CheckOutById", "File") %>', '<%= ViewData["Scope"] %>', 'FileDetail', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>', 'File');" />
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "File") + "', '" + ViewData["Scope"] + "', 'File', 'FileDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { value = "Add From Library", onClick = "javascript: addEntity('" + Url.Action("CreateDetailOfLibrary", "File") + "', '" + ViewData["Scope"] + "', 'File', 'FileDetail', 'LibraryFileDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>

            <input class="button" id="<%= ViewData["Scope"] %>FileAddLink" type="button" value="Add from Link" onclick= "AddDialogLink('<%=ViewData["EntityIdfilter"]%>')" />                                                                                                                             
            <% string scopeValue = ViewData["Scope"].ToString();
               if (!string.IsNullOrEmpty(scopeValue))
               {
                   if (scopeValue.IndexOf("Template") != -1)
                   {
            %>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { value = "NEW HTML Template", onClick = "javascript: newEntity('" + Url.Action("CreateTemplate", "File") + "', '" + ViewData["Scope"] + "', 'File','" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { value = "EDIT HTML Template", onClick = "javascript: optionDetailEntity('" + Url.Action("CreateTemplate", "File") + "','" + Url.Action("EditTemplate", "File") + "','" + ViewData["Scope"] + "', 'File','" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <% }
               } %>
            </asp:Panel>
        <% } %>
        <asp:Panel ID="FileDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("FileDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="FileDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>FileEditFormContent"></div>
    </asp:Panel>
</div>
