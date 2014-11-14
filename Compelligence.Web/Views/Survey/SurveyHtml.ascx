<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<% string formId = ViewData["Scope"].ToString() + "SurveyHtmlForm"; %>
<% decimal quizid = decimal.Parse(ViewData["QuizId"].ToString()); %>

<script src="<%= Url.Content("~/Scripts/CKeditor/ckeditor.js") %>" type="text/javascript"></script>
<style type="text/css">
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
    var loadEditor = function(urlAction) {
        var id = $('<%=formId %>').find("input[name='id']").get(0);
        openPopup(urlAction + '?Id=' + id);
    };
</script>

<script type="text/javascript">
    var hiddenMessage = function() {
    objSurveyBackMessage = document.getElementById('SurveyBackMessage');
        objSurveyBackMessage.style.display = "none";
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>');
    });
</script>

<script type="text/javascript">

    function verifyCkeditor() {

        if (CKEDITOR.instances['WorkspaceSurveyHtmlFormHtmlContent']) {
            CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.destroy();
        }
        CKEDITOR.replace('WorkspaceSurveyHtmlFormHtmlContent');
        CKEDITOR.instances['WorkspaceSurveyHtmlFormHtmlContent'].updateElement();
    }

    function addSection() {
        var SectionPanel = $("#SectionPanel");
        var newSectionPanel = SectionPanel.clone();
        newSectionPanel.prop("style", "display:inline");
        newSectionPanel.addClass("SectionPanel");

        var newSectionName = "SectionPanel_" + ($(".SectionPanel").size() - 2);
        newSectionPanel.prop("id", newSectionName);

        var newSectionButton = newSectionPanel.find("#btnAdd");        //alert(newSectionButton.size());
        newSectionButton.prop("OnClick", "loadPopup('','SurveySection','" + newSectionName + "','coco');");

        var newSectionRemove = newSectionPanel.find("#btnRemove");
        newSectionRemove.prop("OnClick", "removeSection('" + newSectionName + "');");

        //SectionPanel.parent().append(newSectionPanel);
        $("#AddDialog").append(newSectionPanel);

    };

    function removeSection(sectionName) {
        var section = $('#' + sectionName);
        section.remove();
    }

    function addSectionInfo(type, id, name) {
        browsePopup.close();
        alert(id + ' idasdadasd ' + type + ' typeasdsadasdasd ' + name);
        var SearchId = "#" + parentSectionId;
        var Group = parentSectionId.substring(parentSectionId.search("_") + 1);
        var SectionItem = $(SearchId).find("#SectionItem");
        var SectionItemChild = SectionItem.children(":first");
        var newItem = SectionItemChild.clone();
        var newItemInput = newItem.children("#txtItem");
        var newItemArea = newItem.children("#txtItemComment");
        newItemInput.prop("value", name);
        newItemInput.prop("name", "txtItem_" + Group);
        newItemArea.prop("name", "txtItemComment_" + Group);
        //        newItem.attr("class", "newslettervisible");
        SectionItem.append(newItem);
    }

    //For many
    function coco(param) {
        var id = param.id; //
        alert("" + param.id + "-" + param.type);
        var link = param.link;
        var SearchId = "#" + parentSectionId;
        var Group = parentSectionId.substring(parentSectionId.search("_") + 1);
        var SectionItem = $(SearchId).find("#SectionItem");
        var SectionItemChild = SectionItem.children(":first");
        var newItem = SectionItemChild.clone();
        var newItemInput = newItem.children("#txtItem");
        var newItemArea = newItem.children("#txtItemComment");
        var url = '';
        if (param.type == "Project")
            url = '<%=Url.Action("GetName","Project") %>/' + id;
        else
            url = '<%=Url.Action("GetName","Library") %>/' + id;

        $.get(url, {}, function(data) {
            if (data.toString().length > 0) {
                newItemInput.prop("value", data);
                newItemInput.prop("name", "txtItem_" + Group);
                newItemArea.prop("name", "txtItemComment_" + Group);
                //                newItem.attr("class", "newslettervisible");


                SectionItem.append(newItem);
            }
        });

        //Json

    }
    //For items ckeditor
    var verifica = 0;
    function cocoa(param) {

        var id = param.id;
        var link = param.link;

        //alert(id);
        //       // alert(link);

        var SearchId = "#" + parentSectionId;
        var url = '';
        if (param.type == "Project")
            url = '<%=Url.Action("GetName","Project") %>/' + id;
        else
            url = '<%=Url.Action("GetName","Library") %>/' + id;
        //url = '<%=Url.Action("GetName.Value","Library") %>/' + link;

        $.getJSON(url, {}, function(data) {

            //alert(data.Name + data.Link);

            if (data.toString().length > 0) {
                var search = CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.getData();
                var easyini = search.indexOf("<sections>");
                var str = search;
                /* var str = '<ul><li>' + data;
                str = str.replace(/-/g, '</li><li>');*/


                if (easyini != -1) {


                    if (data.Link == undefined) {
                        CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.insertHtml('<ul><li>' + data.Name + ' - No link </li></ul>');
                    }
                    else {
                        CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.insertHtml('<ul><li>' + data.Name.link(data.Link + '"target="_blank') + '</li></ul>');
                    }

                }
                else {
                    var name = prompt("add a Section", "Type your section here");

                    var namelong = name.length;

                    if (name != "" || name != 0 || namelong > 3) {
                        CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.insertHtml('<p><sections>' + name + '</sections>');
                        if (data.Link == undefined) {
                            CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.insertHtml('<ul><li>' + data.Name + ' - No link </li></ul>');
                        }
                        else {
                            CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.insertHtml('<ul><li>' + data.Name.link(data.Link + '"target="_blank') + '</li></ul>');
                        }

                        verifica = 1;
                    } else {
                        alert("must add a valid text");
                    }
                }

            }
        });


    }

    function insertAtCaret(areaId, text) {
        var txtarea = document.getElementById(areaId);
        var scrollPos = txtarea.scrollTop;
        var strPos = 0;
        var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ? "ff" : (document.selection ? "ie" : false));
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

    function addDestinationInfo(type, email) {
        browsePopup.close();
        var textArea = $("#" + parentSectionId);
        if (type == "User")
            insertAtCaret(parentSectionId, email + ", ");
        else
            insertAtCaret(parentSectionId, "(" + email + "), ");

    }




    function copyText() {
        var GetEditor = CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.getData();
        $("#WorkspaceSurveyHtmlFormHtmlContent").val(GetEditor);
        CKEDITOR.instances.WorkspaceSurveyHtmlFormHtmlContent.destroy();

    }
   
</script>


<div class="contentSurveyItems">
  <%OperationStatus astatus = ((OperationStatus)ViewData["OperationStatus"]);%>
  <% using (Ajax.BeginForm("SaveHtml", "Survey", new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "SurveyResultContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnFailure = "showFailedResponseDialog",
               OnSuccess = "function(content) { if ( content.get_data()=='Success') MessageDlg('Message','Success..!');isSuccessful();}" + ");verifyCkeditor();}",
               
           }, new { id = formId }))
   { %>

<div id="surveyIndexTwo" class="indexTwo">         
<fieldset>
        <legend>Edit Html Content</legend>

    <%= Html.Hidden("Scope")%>
    <%= Html.Hidden("BrowseId")%>
    <%= Html.Hidden("IsDetail")%>
    <%= Html.Hidden("OperationStatus")%>
    <%= Html.Hidden("Container")%>
    <%= Html.Hidden("HeaderType")%>
    <%= Html.Hidden("DetailFilter")%>
    <input type="hidden" id="QuizId" name="QuizId" value='<%=ViewData["QuizId"] %>' />
    
    


    <div id="SurveyBackMessage" style="display: block">
    <% string message = (string)ViewData["Message"]; %>

    
    <% if (!string.IsNullOrEmpty(message))
       { %>
            <% if (message.IndexOf("Thank") != -1)
               {%>
                       <script type="text/javascript">
                           showSendResponseMessageDialog();
                       </script>
            <%}
               else
               { %>
                    <p><span class="marginTextSurvey" style="color: red">
                        <%= Html.Encode(message) %></span></p>
            <% } %>
    <% } %>
    </div>

     <div class="line">
        <div class="field">
            <label for="<%= formId %>MetaData">
                Html content:</label>
            <%= Html.TextArea("HtmlContent",(string)ViewData["HtmlContent"], new { id = formId + "HtmlContent", Class = "ckeditor"  })%>
            <script type="text/javascript">
                verifyCkeditor();
            </script>
            
        </div>
    </div>

    <div class="buttonLink">
        <input class="button" id="Submit1" type="submit" value="Send" />
        <input class="button" type="button" value="Reset" onclick="hiddenMessage();javascript: resetFormFields('#<%= formId %>');" />
    </div>
   </fieldset>
</div>

 <%} %>
 
 </div>