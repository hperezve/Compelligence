<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Newsletter>" %>
<%@ Import Namespace="Compelligence.Common.Utility" %>
<%@ Import Namespace="Compelligence.Util.Type" %>
<script src="<%= Url.Content("~/Scripts/CKeditor/ckeditor.js")%>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/BackEnd/NewsLetter.js")%>" type="text/javascript"></script>
 
<% string formId = ViewData["Scope"].ToString() + "NewsletterEditForm"; %>

<style type="text/css">
    .newsbutton
    {
      margin-left: 720px;
    position: relative;
    top: 63px;
        }
 .newsletterhidden
 {
 	display:none;
 	width:100%;
 }
.newslettervisible
 {
 	/*display:block;*/
 	width:100%;
 } 
</style>
<script type="text/javascript">
   
    $(function() {
    initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>');
    });
</script>


<script type="text/javascript">
    var loadEditor = function(urlAction) {
        var id = $('<%=formId %>').find("input[name='id']").get(0);
        openPopup(urlAction + '?Id=' + id);
    };
</script>
  
<script type="text/javascript">

    function verifyCkeditor() {
        
        if (CKEDITOR.instances['WorkspaceNewsletterEditFormContentText']) {
            CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.destroy();
            } 
             CKEDITOR.replace('WorkspaceNewsletterEditFormContentText');
             CKEDITOR.instances['WorkspaceNewsletterEditFormContentText'].updateElement();
        }
      

    //For items ckeditor
    var verifica = 0;
    function cocoa(param) {
        var id = param.id;
        var link = param.link;        
  
        
        var SearchId = "#" + parentSectionId;
        var url = '';
        var Decrip = param.Description;
        
        if (param.type == "Project") {
            url = '<%=Url.Action("GetName","Project") %>/' + id;
        }
        else {
            if (param.type == "Events")
                url = '<%=Url.Action("GetName","Event") %>/' + id;

            else
                url = '<%=Url.Action("GetName","Library") %>/' + id;

        }
        $.getJSON(url, {}, function(data) {
            if (data.toString().length > 0) {
                var search = CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.getData();
                var str = search;

                if (data.Link == undefined) {
                    CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.insertHtml('<ul><li>' + data.Name + '</li></ul>');
                    var valueck = Decrip.split(":")
                    if (Decrip != '') {
                        $.each(valueck, function(key, value) {
                            var param2 = { id2: value };
                            if (param2.id2 == id) {

                                var desc = data.Description;

                                desc = desc.replace(/(<img.*?>)/g, function(m, key, value) { if (key.search(/(src)/g) < 0) { return ""; } else { return m; } });

                                CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.insertHtml('<ul><li><b>Description: </b> ' + String(desc) + '</li></ul>');

                            }
                        });
                    }
                    if (data.DateStart != undefined) {
                        if (data.DateStart.toString().length > 0) {
                            CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.insertHtml('<ul><li><b>Stard Date: </b> ' + data.DateStart + '</li></ul>');
                        }
                    }
                    if (data.DateEnd != undefined) {
                        if (data.DateEnd.toString().length > 0) {
                            CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.insertHtml('<ul><li><b>End Date: </b> ' + data.DateEnd + '</li></ul>');
                        }
                    }
                }

                else {
                    CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.insertHtml('<ul><li><a href="' + data.Link + '" target="_blank">' + data.Name + '</a></li></ul>');
                    var valueck = Decrip.split(":")
                    //alert(data.Link);
                    if (Decrip != '') {
                        $.each(valueck, function(key, value) {
                            var param2 = { id2: value };
                            if (param2.id2 == id) {
                                var desc = data.Description;

                                desc = desc.replace(/(<img.*?>)/g, function(m, key, value) { if (key.search(/(src)/g) < 0) { return ""; } else { return m; } });

                                CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.insertHtml('<ul><li> <b>Description: </b> ' + String(desc) + '</li></ul>');
                            }
                        });
                    }

                    if (data.DateStart != undefined) {
                        if (data.DateStart.toString().length > 0) {
                            CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.insertHtml('<ul><li><b>Stard Date: </b> ' + data.DateStart + '</li></ul>');
                        }
                    }
                    if (data.DateEnd != undefined) {
                        if (data.DateEnd.toString().length > 0) {
                            CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.insertHtml('<ul><li><b>End Date: </b> ' + data.DateEnd + '</li></ul>');
                        }
                    }
                }
            }
        });


    }

    function insertAtCaret(areaId, text) {        
        //alert(areaId + " insertAtCaret " + text)
        var txtarea = document.getElementById(areaId);
        var scrollPos = txtarea.scrollTop;
        var strPos = 0;
        var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?  "ff" : (document.selection ? "ie" : false));
        if (br == "ie") {
            txtarea.focus();
            var range = document.selection.createRange();
            range.moveStart('character', -txtarea.value.length);
            strPos = range.text.length;
        }
        else if (br == "ff") strPos = txtarea.selectionStart;

        var front = (txtarea.value).substring(0, strPos);
        var back = (txtarea.value).substring(strPos, txtarea.value.length);
        txtarea.value = front + text + back;
        strPos = strPos + text.length;
        if (br == "ie") {
            txtarea.focus();
            var range = document.selection.createRange();
            range.moveStart('character', -txtarea.value.length);
            range.moveStart('character', strPos);
            range.moveEnd('character', 0);
            range.select();
        }
        else if (br == "ff") {
            txtarea.selectionStart = strPos;
            txtarea.selectionEnd = strPos;
            txtarea.focus();
        }
        txtarea.scrollTop = scrollPos;
    }


    function addDestinationInfo2(param) {
        
        var id = param.id;
        urlD = '<%=Url.Action("GetNameestination","Newsletter") %>/' + id;


        $.getJSON(urlD, {}, function(data) {

           // alert("type: " + data.Type + " Name: " + data.Name);
            var textArea = $("#" + parentSectionId);
            if (data.Type == "User") {
                insertAtCaret(parentSectionId, data.Name + ", ");
            }
            else {
                insertAtCaret(parentSectionId, "(" + data.Name + "), ");
            }

        });
        
        }

  
    function copyText() {
        var GetEditor = CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.getData();
        $("#WorkspaceNewsletterEditFormContentText").val(GetEditor);
        CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.destroy();

    }

</script>
  
<script type="text/javascript">

    var updatetemplate = function() {

        var idtemplate = document.getElementById('WorkspaceNewsletterEditFormTemplateId').value;

        if (idtemplate != '') {
            var urltemplate = '<%=Url.Action("PreviewContent","Newsletter")%>' + '/' + idtemplate;

            $.getJSON(urltemplate, {}, function(data) {
                if (data.toString().length > 0) {

                    var text2 = data.contenttemplate;

                    do {
                        text2 = text2.replace('/', ' ');
                    }
                    while (text2.indexOf('/') >= 0);

                    CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.destroy();
                    var elemento = document.getElementById("WorkspaceNewsletterEditFormContentText");
                    elemento.value = '';
                    elemento.value = data.contenttemplate;
                    CKEDITOR.replace('WorkspaceNewsletterEditFormContentText');
                    CKEDITOR.instances['WorkspaceNewsletterEditFormContentText'].updateElement();
//                    $('#WorkspaceNewsletterEditFormTemplateId').attr("disabled", "disabled");

                }
            })

        }
        else {

            CKEDITOR.instances.WorkspaceNewsletterEditFormContentText.destroy();
            var elemento = document.getElementById("WorkspaceNewsletterEditFormContentText");
            elemento.value = '';
            CKEDITOR.replace('WorkspaceNewsletterEditFormContentText');
            CKEDITOR.instances['WorkspaceNewsletterEditFormContentText'].updateElement();                    
        }
    };
    var showCheckingAddressDialog = function() {
        $.blockUI({ message: $('#CheckingAddressDialog'),
            css: { width: '20%', margin: 'auto' }
        });
    };
    var hideCheckingAddressDialog = function() {
        $.unblockUI();
    };
    var checkDestinationEmail = function() {
        var newsletterId = $('#Id').val();
        var newsletterdesti = $('#<%= formId %>DestinationMail').val();
        //showCheckingAddressDialog();
        if (newsletterdesti != '' && newsletterdesti != null && newsletterdesti != undefined) {
            var urlAction = '<%= Url.Action("CheckDestinationAddress", "Newsletter") %>';
            var parameters = { Destination: newsletterdesti };
            var xmlhttp;
            var results = null;
            $.get(
            urlAction,
            parameters,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        var textToShow = 'Warning: The following destitnations appear to be invalid. The user might not receive the newsletter:' + results;
                        $('#CheckingAddressNewsletter').empty();
                        $('#CheckingAddressNewsletter').html(textToShow);
                        $("#CheckingAddressNewsletter").dialog({
                            bgiframe: true,
                            autoOpen: false,
                            modal: true,
                            title: 'Warning Newsletter',
                            buttons: { Ok: function() {
                                $(this).dialog('close');
                                //hideCheckingAddressDialog();
                            } 
                            }
                        });

                        $('#CheckingAddressNewsletter').dialog("open");
                        //alert('Warning: The following destitnations appear to be invalid. The user might not receive the newsletter:' + results);
                    }
                }
            });            
        }
    };
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "NewsletterEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Newsletter', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");verifyCkeditor();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        
        <% if (ViewData["UserSecurityAccess"].ToString().Equals(UserSecurityAccess.Edit))
           { %>
        <div class="buttonLink">
            <input class="button" type="submit" value="Save" id="getTextArea" onclick="javascript: copyText() "/>
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" />
            <input class="button" type="button" value="Cancel" onclick="javascript: cancelEntity('<%= ViewData["Scope"] %>', 'Newsletter', '<%= ViewData["BrowseId"] %>', <%= ViewData["IsDetail"].ToString().ToLower() %>);" />
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
        <%= Html.Hidden("LibraryId",null,new{ id=formId+"LibraryId"}) %>
        <%= Html.Hidden("LibrariesId", null, new { id = formId + "LibrariesId" })%>
        <%= Html.Hidden("EntityType", null, new { id = formId + "EntityType" })%>
        <div class="contentFormEdit ">
         
            <div style="overflow:auto;border-right:none;" class="content">       
        
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Title" class="required">
                        <asp:Literal ID="NewsletterTitle" runat="server" Text="<%$ Resources:LabelResource, NewsletterTitle %>" />:</label>
                    <%= Html.TextBox("Title", null, new { id = formId + "Title" })%>
                    <%= Html.ValidationMessage("Title", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">                   
                                       
                    <asp:Literal ID="NewsletterAssignedTo" runat="server" Text="<%$ Resources:LabelResource, NewsletterAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                
                <div class="field">
                    <label for="<%= formId %>TemplateId">
                        <asp:Literal ID="NewsletterTemplateId" runat="server" Text="<%$ Resources:LabelResource, NewsletterTemplate %>" />:</label>
                        
                          <%if (Model != null && Model.TemplateId > 0) 
                     { %>
                   <%= Html.DropDownList("TemplateId", (SelectList)ViewData["TemplateIdList"], new { id = formId + "TemplateId" , Disabled  ="disabled" } )%>                   
                   <%}
                     else
                     { %>
                   <%= Html.DropDownList("TemplateId", (SelectList)ViewData["TemplateIdList"], string.Empty, new { id = formId + "TemplateId" , onchange = "javascript: updatetemplate();"  } )%>                   
                   <%} %>                                                                
                    
                    <%= Html.ValidationMessage("TemplateId", "*")%>
                </div>
                                
            </div>
       
            <div class="line">
                <div class="field">
                   <label for="<%= formId %>ContentText">
                    <asp:Literal ID="NewsletterContent" runat="server" Text="<%$ Resources:LabelResource, NewsLetterContent %>" />:</label>
                    <p>
                      <input value="Add Items" type="button" class="newsbutton" onclick="javascript: loadDLG();" />
                     <%=Html.TextArea("ContentText", new { id = formId + "ContentText", Class = "ckeditor"  })%>
                    <script type="text/javascript">
                         verifyCkeditor();
                     </script>
                   </p>
                </div>
            </div>
    
            
              <br />
              
              <div class="line">  
                <div class="field">
                  <label for="<%= formId %>DestinationMail" class="required">
                    <asp:Literal ID="DestinationMail" runat="server" Text="<%$ Resources:LabelResource, NewsLetterDestinationMail %>" />:</label>
                    <p>
                     <%=Html.TextArea("DestinationMail", new { id = formId + "DestinationMail" })%>
                   </p>
                </div>                
                <div class="field">
                  <input type="button" id="btnMail" value=" + " title="Add Use/Team" onclick="javascript: loadPopup('','NewsLetterDestination','<%=formId + "DestinationMail" %>','addDestinationInfo2');"  /> 
                </div>
              </div>
              
            </div>
              
        </div>
        
  </fieldset>
</div>
<% } %>
   <div id="<%=formId %>NewsLetterAddItemDlg" ></div>
<div id="CheckingAddressDialog" class="displayNone">
            <p>
                <img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                    class="left" /><span class="loadingDialog">Checking Address ...</span>
            </p>
</div>
  <script type="text/javascript">
      function loadDLG() {
          NewsLetterAddItemDlg('<%=Url.Action("GetBrowseNewsLetterPopup","Browse") %>','<%=formId %>');
      };
      $(function() {
          var url = '<%=Url.Content("~/Content/Styles/rte.css") %>';
          //  $(".rte-zone").rte({ content_css_url: '<%=Url.Content("~/Content/Styles/rte.css") %>' });
      });

</script>

