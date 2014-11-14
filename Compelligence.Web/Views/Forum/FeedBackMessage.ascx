<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script> 
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>  
<script src="<%= Url.Content("~/Scripts/jquery.filestyle.js") %>" type="text/javascript" charset="utf-8"></script>
<link href="<%= Url.Content("~/Content/Styles/BackEndPopupComment.css") %>" rel="stylesheet"  type="text/css" /> 
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
    padding: 1px 0px 5px 0;
    vertical-align: middle;
    width:64%;
}
.InputDialog {
    float: left;
    padding: 1px 0px 5px 0;
    vertical-align: middle;
    margin-left: 1%;
    width: 17%;
}
.lineDialog {
    clear: both;
    display: table-row;
    float: left;
    /*width: 98%;*/
    width: 100%;
}
.textDialogUpload {
    /*height: 20px;*/
    /*width: 280px;*/
    width: 100%;
}

.divButtonDialog {

  /* IMPORTANT STUFF */
  overflow: hidden;
  position: relative;   

  /* SOME STYLING */
  width:  auto;
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
</STYLE>
<script type="text/javascript">
    $(document).ready(function() {
        $('#btnAddNewInputFile').click(function() {
            AddNewVersionInputFile($(this));
        });
    });

    var AddNewVersionInputFile = function() {
        var id = 'uploadfileuploadbox';
        var el = document.getElementById(id);
        var number = 1;
        var divLines = el.childNodes;

        if (divLines != null && divLines != undefined && divLines.length > 0) {
            var lastItem = divLines.length - 1;
            var idOfLastDiv = divLines[lastItem];
            if (idOfLastDiv != null) {
                var idOfDiv = idOfLastDiv.id;
                var toRemove = 'divlinefile';
                var indexToCheck = idOfDiv.indexOf(toRemove);
                if (indexToCheck == 0) {
                    var lastNumberStr = idOfDiv.replace(toRemove, '');
                    if (lastNumberStr != null && lastNumberStr != undefined && lastNumberStr != '') {
                        var lastNumber = parseInt(lastNumberStr);
                        number = number + lastNumber;
                    }
                }
            }
        }
        // DIV LINE
        var divLine = document.createElement("div");
        divLine.setAttribute('class', 'lineDialog');
        divLine.setAttribute('className', 'lineDialog');
        divLine.id = 'divlinefile' + number;
        // DIV FIELD (TEXTBOX)
        var divField1 = document.createElement("div");
        divField1.setAttribute('class', 'fieldDialog');
        divField1.setAttribute('className', 'fieldDialog');
        // DIV FIELD (BROWSE BUTTON)
        var divField2 = document.createElement("div");
        divField2.setAttribute('class', 'InputDialog');
        divField2.setAttribute('className', 'InputDialog');
        // DIV FIELD (REMOVE BUTTON)
        var divField3 = document.createElement("div");
        divField3.setAttribute('class', 'InputDialog');
        divField3.setAttribute('className', 'InputDialog');

        // TO FIRST DIV FIELD
        var newText = document.createElement('input');
        newText.type = 'text';
        newText.id = 'txtuploadfile' + number;
        newText.name = 'txtuploadfile';
        //newText.setAttribute('style', 'width:240px;');
        newText.setAttribute('class', 'textDialogUpload');
        newText.setAttribute('className', 'textDialogUpload');
        divField1.appendChild(newText);
        // TO SECOND DIV FIELD
        var divContentField = document.createElement("div");
        divContentField.setAttribute('class', 'divButtonDialog ');
        divContentField.setAttribute('className', 'divButtonDialog');
        divContentField.id = 'mybutton' + number;
        divContentField.onclick = function() { $('#uploadfile' + number).trigger('click'); };


        var newInputFile = document.createElement('input');
        newInputFile.setAttribute('class', 'fileDialogUpload ');
        newInputFile.setAttribute('className', 'fileDialogUpload');
        newInputFile.type = 'file';
        newInputFile.id = 'uploadfile' + number;
        newInputFile.name = 'upload';
        newInputFile.onchange = function() { $("#txtuploadfile" + number).prop("value", newInputFile.value); };

        var newLabel = document.createElement("label");
        newLabel.setAttribute('class', 'labelButtonDialog ');
        newLabel.setAttribute('className', 'labelButtonDialog ');

        var newSpan = document.createElement("span");
        newSpan.innerHTML = "Browse";
        //newSpan.setAttribute('style', 'margin-top: 1px;');
        newLabel.appendChild(newSpan);

        divContentField.appendChild(newLabel);
        divContentField.appendChild(newInputFile);
        divField2.appendChild(divContentField);

        // TO THIRD DIV FIELD
        var removeLabel = document.createElement("label");
        removeLabel.setAttribute('class', 'labelButtonDialog ');
        removeLabel.setAttribute('className', 'labelButtonDialog ');

        var removeSpan = document.createElement("span");
        //removeSpan.setAttribute('style', 'margin-top: 1px;');
        removeLabel.innerHTML = "Remove";

        newLabel.appendChild(removeSpan);

        var divButtonReset = document.createElement("div");
        divButtonReset.setAttribute('class', 'divButtonDialog ');
        divButtonReset.setAttribute('className', 'divButtonDialog');
        //divButtonReset.setAttribute('style', 'padding-right: 0px;');
        divButtonReset.id = 'mybuttonreset' + number;
        divButtonReset.onclick = function() { RemoveFile('divlinefile' + number); };

        divButtonReset.appendChild(removeLabel);
        divField3.appendChild(divButtonReset);

        divLine.appendChild(divField1);
        divLine.appendChild(divField2);
        divLine.appendChild(divField3);

        el.appendChild(divLine);
    };
</script>
<div>
 <% using (Html.BeginForm("FeedBackResponse", "Forum", FormMethod.Post, new { id = "FeeBackResponseForm", ENCTYPE = "multipart/form-data" }))
           {%>
           <div class="error" style="color: red;"></div>
           <%=Html.Hidden("U",(string)ViewData["U"]) %>
           <%=Html.Hidden("C",(string)ViewData["C"]) %>
           <%=Html.Hidden("EntityType", (string)ViewData["EntityType"])%>
           <%=Html.Hidden("IndustryId", (string)ViewData["IndustryId"]) %>
           <%=Html.Hidden("SubmittedVia", (string)ViewData["SubmittedVia"])%>

           <input type='hidden' name='hdnEntityId' id='hdnEntityId'  value='<%=ViewData["EntityId"] %>' />
           <label for="Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold;"> Comment</label>
           <textarea name="txtComment" id="txtComment" WRAP="SOFT" class="textareafeedbackdialog text ui-widget-content ui-corner-all" style="width:100%" ROWS="4"></textarea>
           <div id="uploadfileuploadbox" style="font-size:11px;padding-top: 3px;"></div>
           <div id="btnAddNewInputFile" class="ui-dialog-buttonpane divButtonDialog" style="width: 175px;">
            <span><label class="labelButtonDialog">Attach a file (max. 2 MB)</label></span>
           </div>
           <div id="PopupImageUploader" title="Upload File">
            <div id="uploaderFile"></div>
           </div>
           <input type="submit" class="button" id="btnSubmit" value="Submit" style="display:none;" />
        <%   } %>
</div>