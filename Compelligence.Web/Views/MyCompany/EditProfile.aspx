<%@ Page Title="Compelligence - My Company" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.ClientCompany>" %>

   
<asp:Content ID="indexHead" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/jquery.treeview.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/jquery.searchFilter.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.jqgrid.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.multiselect.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/ext-all.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/rte.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ui-widget {
            font-size: 0.8em;
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"       type="text/javascript"></script>
     <script  src="<%= Url.Content("~/Scripts/ajaxupload.js") %>"  type="text/javascript"></script>
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        var check_extension = function(filename, submitId, labelId) {
            var re = /\..+$/;
            var ext = filename.match(re);
            var submitEl = document.getElementById(submitId);
            var labelEl = document.getElementById(labelId);
            if (hash[ext]) {
                submitEl.disabled = false;
                labelEl.innerHTML = 'Ensure that all industries and products are already configured before uploading criteria.';
                labelEl.style.color = "blue";
                return true;
            } else {
                labelEl.innerHTML = 'Invalid filename, please select another file';
                labelEl.style.color = "red";
                submitEl.disabled = true;

                return false;
            }

        };
        $(function() {
            loadAjaxProfile("AjaxUpload", "MyCompany");
        });
        var executeLink = function(urlAction) {
            showLoadingDialog();
            $.get(urlAction, {}, function(data) {
                if (data != "")
                    alert(data);
                else
                    alert('Salesforce.com updated..!.');
                hideLoadingDialog();
            });
        };
        var loadContent = function(urlAction, target) {
            $("#ImageCompany").val("");
        };

        var uploadFileComponent;
        var autoSubmitValue = true;
        var loadAjaxProfile = function(urlAction, target) {
            var typeButton = '<%=Session["Imageurl"] %>';
            if (typeButton == "") {
                var uploadFileLink = '#FileCheckIns';
            }
            else {
                var uploadFileLink = '#FileCheckIn';
            }
            uploadFileComponent = new 
            AjaxUpload(uploadFileLink, {
                action: urlAction,
                onChange: function(file, extension) {
                    document.getElementById("confirmBox").style.visibility = "hidden";
                },
                onSubmit: function(file, ext) {
                    if (!(ext && /^(jpg|gif)$/.test(ext))) {
                        $('#AlertReturnMessageDialog').html('"The provided file is not valid. Only .gif and .jpg fornats are allowed."');
                        $('#AlertReturnMessageDialog').dialog('open');
                        return false;
                    }
                },
                onComplete: function(file, response) {
                    $("#ImageCompany").val(response);
                }
            });
        };
        var PreBrowse = function() {
            //document.getElementById("ShowConfirmDialog").style.visibility = "visible";
            document.getElementById("confirmBox").style.visibility = "visible";
        };
        var answer = function() {
            //document.getElementById("ShowConfirmDialog").style.visibility = "hidden";
            document.getElementById("confirmBox").style.visibility = "hidden";
        }


        function carga() {
            posicion = 0;

            // IE
            if (navigator.userAgent.indexOf("MSIE") >= 0) navegador = 0;
            // Otros
            else navegador = 1;
        }

        function evitaEventos(event) {
            // Funcion que evita que se ejecuten eventos adicionales
            if (navegador == 0) {
                window.event.cancelBubble = true;
                window.event.returnValue = false;
            }
            if (navegador == 1) event.preventDefault();
        }

        function comienzoMovimiento(event, id) {

            elMovimiento = document.getElementById(id);
            // Obtengo la posicion del cursor
            if (navegador == 0) {
                cursorComienzoX = window.event.clientX + document.documentElement.scrollLeft + document.body.scrollLeft;
                cursorComienzoY = window.event.clientY + document.documentElement.scrollTop + document.body.scrollTop;

                document.attachEvent("onmousemove", enMovimiento);
                document.attachEvent("onmouseup", finMovimiento);
            }
            if (navegador == 1) {
                cursorComienzoX = event.clientX + window.scrollX;
                cursorComienzoY = event.clientY + window.scrollY;

                document.addEventListener("mousemove", enMovimiento, true);
                document.addEventListener("mouseup", finMovimiento, true);
            }

            elComienzoX = parseInt(elMovimiento.style.left);
            elComienzoY = parseInt(elMovimiento.style.top);
            // Actualizo el posicion del elemento
            elMovimiento.style.zIndex = ++posicion;

            evitaEventos(event);
        }

        function enMovimiento(event) {
            var xActual, yActual;
            if (navegador == 0) {
                xActual = window.event.clientX + document.documentElement.scrollLeft + document.body.scrollLeft;
                yActual = window.event.clientY + document.documentElement.scrollTop + document.body.scrollTop;
            }
            if (navegador == 1) {
                xActual = event.clientX + window.scrollX;
                yActual = event.clientY + window.scrollY;
            }

            elMovimiento.style.left = (elComienzoX + xActual - cursorComienzoX) + "px";
            elMovimiento.style.top = (elComienzoY + yActual - cursorComienzoY) + "px";

            evitaEventos(event);
        }

        function finMovimiento(event) {



            if (navegador == 0) {
                document.detachEvent("onmousemove", enMovimiento);
                document.detachEvent("onmouseup", finMovimiento);
                alert(event);
            }
            if (navegador == 1) {
                document.removeEventListener("mousemove", enMovimiento, true);
                document.removeEventListener("mouseup", finMovimiento, true);
            }
        }

        window.onload = carga;
        
        
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <% Html.RenderPartial("BackEndFormMessages"); %>
    <%--<div id="ShowConfirmDialog" class="ui-widget-overlay" style="visibility: hidden;width: 1423px; height: 1288px; z-index: 1001;"></div>--%>
    <div id="confirmBox" onmousedown="comienzoMovimiento(event, this.id);" onmouseover="this.style.cursor='move'"style="visibility: hidden;position: absolute; overflow: hidden; z-index: 1006; outline: 0px none; height: auto; width: 300px; top: 574.5px; left: 499px;" class="ui-dialog ui-widget ui-widget-content ui-corner-all  ui-draggable " tabindex="-1" role="dialog" aria-labelledby="ui-dialog-title-UploadFileConfirmDialog">
    <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix" unselectable="on" style="-moz-user-select: none;">
    <span class="ui-dialog-title" id="ui-dialog-title-UploadFileConfirmDialog" unselectable="on" style="-moz-user-select: none;">Confirm upload</span><button class="ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close" role="button" aria-disabled="false" title="close" onclick="answer(false)"><span class="ui-button-icon-primary ui-icon ui-icon-dialog ui-icon-closethick"></span><span class="ui-button-text" onclick="answer(false)">close</span></button></div>
        <div class="ui-dialog-content ui-widget-content" style="height: auto; min-height: 67px; width: auto;">
            </span>Replace existing logo?
        </div>
            <div class="ui-dialog-buttonpane ui-widget-content ui-helper-clearfix">
            <button type="button" id="FileCheckIn" class="ui-state-default ui-corner-all" onclick="javascript:loadAjaxProfile('<%= Url.Action("AjaxUpload", "MyCompany") %>','');">Yes</button>
            <button type="button" class="ui-state-default ui-corner-all" onclick="answer(false)">No</button>
        </div>
    </div>
    <div id="AlertReturnMessageDialog" title="Return Message">
        </div>
        <div id="AlertLoadMessage" title="Return Message">
        </div>
    <div class="EditProfile">
        <fieldset>
            <legend>Edit My Company</legend>
            <%= Html.ValidationSummary()%>
            <% using (Html.BeginForm("EditProfile", "MyCompany", FormMethod.Post, new { id = "ClientCompanyEditProfileForm", align = "left" }))
               { %>
            <%= Html.Hidden("ClientCompanyId")%>
            <div class="line">
                <div class="field">
                    <label for="Name" class="required">
                        <asp:Literal ID="ClientCompanyName" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyName %>" />:</label>
                    <%= Html.TextBox("Name")%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="Address" class="required">
                        <asp:Literal ID="ClientCompanyAddress" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyAddress %>" />:</label>
                    <%= Html.TextBox("Address")%>
                    <%= Html.ValidationMessage("Address", "*")%>
                </div>
            </div>
            <div class="line">
                 <div class="field">
                        <label for="CountryCode" class="required">
                            <asp:Literal ID="ClientCompanyCountryCode" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyCountryId %>" />:</label>
                        <%= Html.DropDownList("CountryCode", (SelectList)ViewData["CountryCodeList"], string.Empty)%>
                        <%= Html.ValidationMessage("CountryCode", "*")%>
                    </div>
                    <div class="field">
                        <label for="City" class="required">
                            <asp:Literal ID="ClientCompanyCity" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyCity %>" />:</label>
                        <%= Html.TextBox("City")%>
                        <%= Html.ValidationMessage("City", "*")%>
                    </div>
            </div>
            <div class="line">
                <div class="field">
                        <label for="State" class="required">
                            <asp:Literal ID="ClientCompanyState" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyState %>" />:</label>
                        <%= Html.TextBox("State")%>
                        <%= Html.ValidationMessage("State", "*")%>
                    </div>
                    <div class="field">
                        <label for="ZipCode" class="required">
                            <asp:Literal ID="ClientCompanyZipCode" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyZipCode %>" />:</label>
                        <%= Html.TextBox("ZipCode")%>
                        <%= Html.ValidationMessage("ZipCode", "*")%>
                    </div>
            </div>
            <div class="line">
                <div class="field">
                        <label for="Phone" class="required">
                            <asp:Literal ID="ClientCompanyPhone" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyPhone %>" />:</label>
                        <%= Html.TextBox("Phone")%>
                        <%= Html.ValidationMessage("Phone", "*")%>
                    </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="Description">
                        <asp:Literal ID="ClientCompanyDescription" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyDescription %>" />:</label>
                    <%= Html.TextArea("Description")%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="Email">
                        <asp:Literal ID="Email" runat="server" Text="<%$ Resources:LabelResource, CompanyEmail%>" />:</label>
                    <%=(string)ViewData["Email"]%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="SalesForceToken">
                        <asp:Literal ID="ClientCompanySalesForceToken" runat="server" Text="<%$ Resources:LabelResource, ClientCompanySalesForceToken%>" />:</label>
                    <%= Html.TextBox("SalesForceToken")%>
                    <%= Html.ValidationMessage("SalesForceToken", "*")%>
                </div>
                <div class="field">
                    <label for="SalesForceUser">
                        <asp:Literal ID="ClientCompanySalesForceUser" runat="server" Text="<%$ Resources:LabelResource, ClientCompanySalesForceUser%>" />:</label>
                    <%= Html.TextBox("SalesForceUser")%>
                    <%= Html.ValidationMessage("SalesForceUser", "*")%>
                </div>
                
            </div>
            <div class="line">              
              <div class="field"> 
               <label >Upload Logo:</label>
                    <%= Html.TextBox("Imageurl",null,new { id = "ImageCompany" })%>
                    <%= Html.ValidationMessage("Imageurl", "*")%> 
              </div>                   
                        <% var StaticImageUrl = Model != null ? Html.Encode(Model.Imageurl) : string.Empty;%>
              <div class="field">
              <label style="visibility: hidden;">Upload Logo:</label>                
                    <div style="float: left;">                      
                         <% string ImageUrl = (string)Session["Imageurl"];
                           if (string.IsNullOrEmpty(ImageUrl))
                           {%>   
                           <input class="button" id="FileCheckIns" value="Browse" type="button" onclick="javascript:loadAjaxProfile('<%= Url.Action("AjaxUpload", "MyCompany") %>','');"/>
                                <%}
                           else
                           { %>
                           <input class="button" id="Button2" value="Browse" type="button" onclick="PreBrowse()"/>
                                <%} %>
                         <input class="button" type="submit" value="Submit" />                        
                    </div>
              </div>                        
             </div>
             <div class="field">              
                <label>Recommended size is 280 x 70 pixels</label>  
                <input class="button" id="Button1" type="button" value="Remove Logo" onclick="javascript:loadContent('<%= Url.Action("EditProfile", "MyCompany") %>','FormResults');"/>    
              </div>
            <div class="line">
            <% if (!(string.IsNullOrEmpty(Model.SalesForceUser) || string.IsNullOrEmpty(Model.SalesForceToken)))
               {%>
                <div class="field" style="padding-bottom: 10px">
                    <a href="<%=Url.Action("EditPassword", "MyCompany") %>">Edit Password </a>
                </div>
                <%} %>
            </div>
            <hr width="100%" color="darkgray" size="2" align="left" style="float: left;">
            <div class="float-left" style="margin-top: 20px; margin-left: 12px; padding-left: 10px;
                padding-bottom: 10px;">
                <input class="button" type="submit" value="Save" />
                <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#ClientCompanyEditProfileForm');" />
                <input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("Index","BackEnd")%>'" />
            </div>
            <% } %>
        </fieldset>
    </div>
    <div id="LoadingDialog" class="displayNone">
        <p>
            <img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                class="left" /><span class="loadingDialog">Loading ...</span>
        </p>
    </div>
</asp:Content>