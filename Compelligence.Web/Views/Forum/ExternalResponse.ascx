<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import namespace="Compelligence.Domain.Entity"  %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<link href="<%= Url.Content("~/Content/Styles/BackEndPopupComment.css") %>" rel="stylesheet"  type="text/css" /> 

<style type="text/css">
     body 
     {
     	background-color:Transparent;
     	margin-left:0px;
     	width :100%;
     }
     .ertext
     { border-color: darkgray;
    border-style: solid;
    border-width: thin;
    height: 157px;
    width: 100%;
         }
.commentbuttons
{
    text-align:center;
}     
    .inputFile {
	BORDER-BOTTOM: 	#bbb 1px solid;
	BORDER-LEFT: #bbb 1px solid; 
	HEIGHT: 22px; 
	BORDER-TOP: #bbb 1px solid; 
	BORDER-RIGHT: #bbb 1px solid
}
a
{
	text-decoration :none;
}
.lineDialog {
   display: table-row;
   clear: both;
   float: left;
   clear: both;
   width: 98%;
}
.fieldDialog
{
    float: left;
    padding:  1px 5px 5px 0;
        vertical-align:middle;
}
.textDialogUpload
{
    height:20px;
    width:280px;
    readonly:readonly;

    }
    .fileUpload { display:none;
                  visibility: hidden;
                   }
    .responseTextReply
    {
        color: Gray;
        font-size: 12px; 
        margin-bottom: 5px;  
        margin-left: 5px; 
    } 
    .disableButtom
    {
        background: none repeat scroll 0 0 transparent !important;
        cursor: pointer !important;
    }
    .validation-summary-errors {
    background: url("../Content/Styles/Images/Icons/icon-error.gif") no-repeat scroll 5px 6px #EEEEEE;
    border: 1px solid #AAAAAA;
    color: #FF0000;
    /*font-weight: bold;*/
   /* margin: 10px;*/
   /* padding: 10px 10px 10px 60px;*/
}
.validation-error-items
{
    /*border-color:#FF0000;    */
    border :1px solid #FF0000;
} 
.validation-error-text
{
    /*border-color:#FF0000;    */
    color :#FF0000;
} 
</style>
<STYLE type="text/css" media="all">
    .inputFile {
	BORDER-BOTTOM: 
	#bbb 1px solid; BORDER-LEFT: #bbb 1px solid; 
	HEIGHT: 22px; BORDER-TOP: #bbb 1px solid; 
	BORDER-RIGHT: #bbb 1px solid
	
	
}
a
{
	text-decoration :none;
}
.fieldDialog {
    float: left;
    padding: 1px 5px 5px 0;
    vertical-align: middle;
}
.lineDialog {
    clear: both;
    display: table-row;
    float: left;
    /*width: 98%;*/
    width: 99%;
}
.textDialogUpload {
    /*height: 20px;*/
    /*width: 280px;*/
    width: 250px;
}

.divButtonDialog {

  /* IMPORTANT STUFF */
  overflow: hidden;
  position: relative;   

  /* SOME STYLING */
  width:  65px;
  /*width: auto;*/
  /*height: 28px;*/
  height: 20px;
  /*border: 1px solid  #AAAAAA;*/
 /* font-weight: normal;*/
  /*background: red;*/
  /*background-image: url("../Images/Styles/BGYellowGrad1.gif") repeat scroll left top rgba(0, 0, 0, 0);*/
  background:repeat-x scroll 50% 50% #E6E6E6;
  /* background:url("images/ui-bg_glass_75_e6e6e6_1x400.png") repeat-x scroll 50% 50% #E6E6E6;*/
 /* color: #666666;*/
  cursor: pointer;
  /*font-weight: bold;*/
   padding-left: 5px;
    padding-right: 5px;
    
    border: 1px solid #D3D3D3;
    color: #555555;
    font-weight: normal;
    text-align: center;
}

.divButtonDialog:hover {
 background:repeat-x scroll 50% 50% #DADADA;
 /*background:url("images/ui-bg_glass_75_dadada_1x400.png") repeat-x scroll 50% 50% #DADADA;*/
 border: 1px solid #999999;
 color:#212121;
}
input.fileDialogUpload {
  height: 30px;
  cursor: pointer;
  position: absolute;
  top: 0px;
  right: 0px;
  /*font-size: 100px;*/
  z-index: 2;

  opacity: 0.0; /* Standard: FF gt 1.5, Opera, Safari */
  filter: alpha(opacity=0); /* IE lt 8 */
  -ms-filter: "alpha(opacity=0)"; /* IE 8 */
  -khtml-opacity: 0.0; /* Safari 1.x */
  -moz-opacity: 0.0; /* FF lt 1.5, Netscape */
}
.labelButtonDialog
{
   position: relative;
   cursor: pointer;
   text-align: center;
   overflow: visible;
   /*background-color: red;*/
   overflow: hidden;
   font-size:12px;
   font-weight: normal;
   text-align: center; 
  vertical-align: middle;
  font-family:Arial;
  /*padding: 0.4em 1em;*/
  margin-top: 0px; top: 2px;
}
    
</STYLE>
<!--

-->
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>  
<script src="<%= Url.Content("~/Scripts/jquery.filestyle.js") %>" type="text/javascript" charset="utf-8"></script>


<script type="text/javascript">

    $(function() {
        $("input[type=file]").filestyle({
            image: "url(<%= ResolveUrl("~/Images/Styles/panelrigth_title_8.jpg") %>)",
            imageheight: 22,
            imagewidth: 65,
            width: 280
        });
        inspectTextArea();
    });
    $(document).ready(function() {
        $('#btnAddNewInputFile').click(function() {
            AddNewVersionInputFile($(this));
        });
    });
    var AddComentsclassImage = function() {
        $('#ImgComents' + '<%= ViewData["EntityId"]%>', window.parent.document).removeClass("ImageCommentsN");
        $('#ImgComents' + '<%= ViewData["EntityId"]%>', window.parent.document).addClass("ImageCommentsY");
    };
    function inspectTextArea() {
        var textArea = $("#commentExternalId").val();
        if (textArea === '') {
            $('#saveMessage').attr('disabled', 'disabled');
            $('#saveMessage').addClass('disableButtom');
        }
        else {
            $('#saveMessage').removeAttr('disabled');
            $('#saveMessage').removeClass('disableButtom');
        }
    };    
</script>

<div class="discussionTitleReply">
<div><%=ViewData["ResponseText"] %>  </div>
<% using (Html.BeginForm("ExternalResponseSave", "Forum", FormMethod.Post, 
     new { id = "CommentsResponseForm", ENCTYPE = "multipart/form-data" }))
  { %>
    <div class="error" style="color: red;"></div>
    <%=Html.Hidden("U",(string)ViewData["U"]) %>
    <%=Html.Hidden("C",(string)ViewData["C"]) %>

    <input type="hidden" id="EntityId" name="EntityId" value='<%=ViewData["EntityId"] %>' />
    <input type="hidden" id="ForumResponseId" name="ForumResponseId" value='<%=ViewData["ForumResponseId"] %>' />
    <input type="hidden" id="DomainObjectType" name="DomainObjectType" value='<%=ViewData["DomainObjectType"] %>' />
    <input type="hidden" id="IndustryId" name="IndustryId" value='<%=ViewData["IndustryId"] %>' />
    <input type="hidden" id="CriteriaId" name="CriteriaId" value='<%=ViewData["CriteriaId"] %>' />
    <input type="hidden" id="ProductId" name="ProductId" value='<%=ViewData["ProductId"] %>' />
    
    <table style=" width:100%;">
  <tbody>
    <tr>
      <td colspan="4">
   
            <label for="Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold;">Comment/Response:</label>
            <%=Html.TextArea("Response", new { id = "commentExternalId", Class = "ertext", onkeyup = "inspectTextArea()" })%>
            <%= Html.ValidationMessage("Response", "*")%>
        
   
    </td>
    </tr>
    <tr>
     
      <td align ="right" style="vertical-align:bottom">
      <div id="uploadfileuploadbox" style="font-size:11px;padding-top: 3px;"></div>
           <div id="btnAddNewInputFile" class="ui-dialog-buttonpane divButtonDialog" style="width: 175px;float:left">
            <span><label class="labelButtonDialog">Attach a file (max. 2 MB)</label></span>
           </div>
           <div id="PopupImageUploader" title="Upload File">
            <div id="uploaderFile"></div>
           </div>
      <input id="saveMessage" class="button" type="submit" value="Save" onclick="AddComentsclassImage();" style="display:none"/>
      </td>
      
    </tr>
  </tbody>
</table>



</div>
<%} %>

