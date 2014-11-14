<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<link href="<%= Url.Content("~/Content/Styles/FrontEndSite.css") %>" rel="stylesheet"
    type="text/css" />
<%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.8.12.custom.css") %>" rel="stylesheet" type="text/css" />--%>
<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>"
    rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet"
    type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/rating.css") %>" rel="stylesheet" type="text/css" />
   
<link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet"
    type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/Sticky/stickytooltip.css") %>" rel="stylesheet"
    type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>"
    rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
<style type="text/css">
    body
    {
        background: none;
    }
    label
    {
        color: Black;
    }
    td
    {
        font-size: 0.75em;
    }
    /*
    
    
    ul.listWithoutBullet
    {
    list-style-type:none;    
    } */.tipbox2
    {
        /* display:none;*/ /*position:absolute;*/
        border: 5px ridge #98BF21;
        background: none repeat scroll 0 0 whitesmoke;
    }
    /*.tipdescription
    {
        border: 1px solid;
        padding: 5px 10px;
    }
    .tiptitle
    {
        border: 1px solid;
        padding: 2px 10px;
        font-weight: bold;
    }*/
    .tipbox
    {
        display:none;
        position:absolute;
        /*border:5px ridge #98BF21;*/
        border: 1px solid lightgray;
        background:none repeat scroll 0 0 whitesmoke;
    }
    .tipdescription
    {
        border:1px solid;
        padding:5px 10px;
    }
    .tiptitle
    {
        border:1px solid;
        padding:2px 10px;
        font-weight:bold;
    }
    ul.listWithoutBullet
    {
    list-style-type:none;    
    }
</style>

<script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>

<%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>" type="text/javascript"></script>--%>

<script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/Sticky/stickytooltip.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery.multiselect.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery.multiselect.filter.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
    type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"
    type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/BackEnd/Utility.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Rating.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Feedback.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Download.js") %>" type="text/javascript"></script>
<script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Functions.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/FunctionsFrontEnd.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/EditAddSW.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        var urlc_deleteS = '<%=Url.Action("deleteStrength","ContentPortal") %>';
        var urlc_deleteW = '<%=Url.Action("deleteWeakness","ContentPortal") %>';
        var uSW = '<%= ViewData["U"] %>';
        var cSW = '<%= ViewData["C"] %>';
    </script>
<script type="text/javascript">
    function ChangeSFAction(url) {
        $('#FrontEndForm').prop("action", url);
        $('#FrontEndForm').submit();
    };
    function toggle_visibility(container, enabled) {
        var e = container.getElementsByTagName('span')[0];
        if (!enabled)
            e.style.display = 'none';
        else {
            e.style.display = 'block';
            e.setAttribute("className", "tooltipspanhover"); //For IE; harmless to other browsers.
            e.setAttribute("class", "tooltipspanhover"); //For IE; harmless to other browsers.
        }
    };

    var loadUrl = function() {
        var url = $('#LinkEntity').text();
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "Url", "scrollbars=1,width=900,height=500")
        }
    };
    var openUrl = function(url) {
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "News", "width=900,height=500")
        }
    };
    //THIS METHOD WILL SELECT A Industry IN THE LIST OF Industries
    var setInd = function(industryId) {
        $('#Industry').val(industryId);
        $('#Industry').multiselect('refresh');
        $('#Competitor').val("");
        $('#Competitor').multiselect('refresh');
        var url = '<%= Url.Action("ContentPortalCHI", "SalesForce") %>';
        ChangeSFAction(url);
    };
    //THIS METHOD WILL SELECT A COMPETITOR IN THE LIST OF COMPETITORS
    var setIndAndCompVal = function(competitorId) {
        $('#Competitor').val(competitorId);
        $('#Competitor').multiselect('refresh');
        var url = '<%= Url.Action("ContentPortalCHC", "SalesForce") %>';
        ChangeSFAction(url);
    };
    //THIS METHOD WILL SELECT A COMPETITOR IN THE LIST COMPETITOR WHEN NO EXIST COMPETITOR SELECTED
    // AND WILL SELECT A PRODUCT IN THE LIST OF PRODUCT, IN THE CASE NO EXIST PRODUCTS THEN 
    // THIS METHOD WILL GET PRODUCTS AND FILL THE PRODUCTS IN PRODUCT LIST
    var setIndCompAndProdVal = function(competitorId, productId) {
        var competitorSelect = $('#Competitor');
        var competitor = competitorSelect.val();
        if (competitor === undefined || competitor === null || competitor === '') {
            $('#Competitor').val(competitorId);
            $('#Competitor').multiselect('refresh');
        }
        var countOptionsProductList = $("#Product option").size();
        var urlGetProduct = '<%= Url.Action("GetProductsByCompetitor", "ContentPortal") %>';
        if (countOptionsProductList > 1) {//THE FIRST OPTION IN THE PRODUCT LIST IS EMPTY
            $("#Product").val(productId);
            $("#Product").multiselect('refresh');
            var url = '<%= Url.Action("ContentPortalCHP", "SalesForce") %>';
            ChangeSFAction(url);
        } else {//IF NO EXIST PRODUCTS IN THE LIST OF PRODUCT WE NEED FILL THIS LISTBOX 
            var industryId = $('#Industry').val();
            var encodeCompanyId = '<%= ViewData["C"] %>';
            $.ajax({
                type: "POST",
                url: urlGetProduct + '/' + competitorId + "?IndustryId=" + industryId + '&C=' + encodeCompanyId,
                dataType: 'json',
                beforeSend: function() {
                    showLoadingDialog();
                },
                success: function(json) {
                    var items = "";
                    $.each(json, function(i, item) {
                        items += "<option value='" + item.Value + "' " + item.Disabled + " >" + item.Text + "</option>";
                    })
                    $("#Product").html(items);
                }, complete: function() {
                    hideLoadingDialog();
                    $("#Product option[value='" + productId + "']").prop("selected", 1).prop("checked", "checked");
                    $("#Product").multiselect('refresh');
                    var url = '<%= Url.Action("ContentPortalCHP", "SalesForce") %>';
                    ChangeSFAction(url);
                }
            });
        }
    };
    $(function() {
        $("#Industry").multiselect({
            multiple: false,
            header: "Select an Industry",
            noneSelectedText: "Select an Industry",
            selectedList: 1,
            minWidth: 200,
            classes: "auto fixed "
        }).multiselectfilter();

        $("#Competitor").multiselect({
            multiple: false,
            header: "Select a Competitor",
            noneSelectedText: "Select a Competitor",
            selectedList: 1,
            minWidth: 200,
            classes: "auto fixed"
        }).multiselectfilter();

        $("#Product").multiselect({
            multiple: false,
            header: "Select a Product",
            noneSelectedText: "Select a Product",
            selectedList: 1,
            minWidth: 200,
            classes: "auto fixed"
        }).multiselectfilter();

        $('input[name="multiselect_Industry"]').click(function() {
            var competitorId = $('#Competitor').val();
            if (competitorId != undefined && competitorId != null && competitorId != '') {
                $('#Competitor').val('');
                $('#Competitor').multiselect('refresh');
            }
            var var_name = $("input[name=multiselect_Industry]:checked").val();
            $('#Industry').val(var_name);
            var url = '<%= Url.Action("ContentPortalCHI", "SalesForce") %>';
            ChangeSFAction(url);
        })


        $('input[name="multiselect_Competitor"]').click(function() {
            var verificar = $("input[name=multiselect_Competitor]:checked").val();
            $('#Competitor').val(verificar);
            var url = '<%= Url.Action("ContentPortalCHC", "SalesForce") %>';
            ChangeSFAction(url);

        })

        $('input[name="multiselect_Product"]').click(function() {
            var var_name = $("input[name=multiselect_Product]:checked").val();
            $('#Product').val(var_name);
            var url = '<%= Url.Action("ContentPortalCHP", "SalesForce") %>';
            ChangeSFAction(url);

        })

        var el = document.getElementById("imgDetail")
        if (el) {
            if (el.height > 80) {
                var newWidth = Math.round(((80 * el.width) / el.height));
                el.style.width = "" + newWidth + "px";
                el.style.height = "80px";
            }
        }
    });
    function resize(which) {
        var elem = document.getElementById(which);
        var height = elem.height;
        var width = elem.width;
        var percentajeRectWidth;
        var percentajeRectHeight;
        if (elem == undefined || elem == null) return false;
        if (elem.width > 250) {
            percentajeRectWidth = (25000 / width);
            elem.style.height = ((height * percentajeRectWidth) / 100).toString() + 'px';
            height = (height * percentajeRectWidth) / 100;
            elem.style.width = 250 + 'px';
            width = 250;
        }
        if (elem.height > 84) {
            percentajeRectHeight = (8400 / height);
            elem.style.width = ((width * percentajeRectHeight) / 100).toString() + 'px';
            elem.style.height = 84 + 'px';
        }
        //alert('run resize()');
    };
    $(document).ready(function() {
        $('.AddTargetBlankToLin a').attr('target', '_blank');
    });
    function ExternalFeedBackWithAttachedDlg(url, title) {
        var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
        ExternalFeedBackWithAttachedVDlg(url, title, urlValidate);
    };
    function ExternalResponseNewDlg(url) {
        var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
        ExternalResponseNewVDlg(url, urlValidate);
    };
    var GetIdByPosition = function(str, element) {
        /// <IdPartText>_<     first id  >_<   second id   >  
        ///  ImgFeedBack_18821786158532650_19071721096826085
        str = str.split('_'); ///
        if (str.length > element)
            return str[element];
        else
            return null;
    };
    function validateVarible(varibale) {
        var result = false;
        if (varibale != undefined && varibale != null && varibale.length > 0) {
            result = true;
        }
        return result;
    };
    function setTooltip() {
        $('.tip').mouseenter(function(e) {
            $("> div", this);
            elem = $(this);

            $("> div", this).css("left", e.pageX + 50);
            $("> div", this).css("top", e.pageY - 30);
            $("> div", this).show(50);
        });
        $('.tip').mouseleave(function(e) {
            $("> div", this).hide(50);
        });
    }
    $(document).ready(function() {
        setTooltip();
        $('.pos-edt-dlg').on({
            'click': function() {
                var industrySelect = $('#Industry');
                var industryId = industrySelect.val(); //INDUSTRY ID 
                var competitorSelect = $('#Competitor');
                var competitorId = competitorSelect.val(); //COMPETITOR ID
                var productSelect = $('#Product');
                var productId = productSelect.val(); // PRODUCT ID
                var actionPositioning = '';
                var eId = ''; //ENTITY ID
                var eType = ''; //ENTITY TYPE[COMPETITOR/PRODUCT]

                //THE ID SHOULD HAVE THE FORMAT: Img<FIELD_Positioning>_<POSITIONING ID>_<ENTITY TYPE>_<POSITIONING RELATION>_<IS GLOBAL> 
                // example: ImgHta_1000000612_COMPT_P_N

                var id = $(this).attr('id');
                var pField = GetIdByPosition(id, 0);
                var positioningId = GetIdByPosition(id, 1); //Id positioning
                var entityType = GetIdByPosition(id, 2); // entity type of positioning
                var pRelation = GetIdByPosition(id, 3);
                var isGlobal = GetIdByPosition(id, 4);
                var typeClasification = '';
                if (validateVarible(pField) && validateVarible(pRelation)) {
                    if (pRelation === '<%= Compelligence.Domain.Entity.Resource.PositioningRelation.CompetitiveMessaging %>') {
                        typeClasification = 'CC'; //CC=ClientCompany
                    } else {
                        if (pField === 'ImgWhp') {
                            typeClasification = 'CC';
                        }
                        else {
                            typeClasification = 'OC'; //OC=OtherCompetitor
                        }
                    }
                }
                if (!validateVarible(isGlobal)) isGlobal = 'N';
                if (validateVarible(entityType)) {
                    if (isGlobal === 'Y') {//If positioning is global need create specific positioning/competitive messaging
                        actionPositioning = 'Create';
                        if (entityType === 'COMPT') {//the current positioning is COMPETITOR
                            if (validateVarible(productId)) {
                                //************ NO ACTION TO HERE-TO FUTURE ***********
                                //CREATE NEW POSITIONING TO THE PRODUCT SELECTED
                                eId = productId;
                                eType = 'PRODT';
                            } else {
                                //CREATE NEW POSITIONING TO THE COMPETITOR SELECTED
                                eId = competitorId;
                                eType = 'COMPT';
                            }
                        } //************ NO ACTION TO HERE-TO FUTURE ***********
                        else { //the current positioning is PRODUCT 
                            //PRODUCT SHOULD BE SELECTED WITH A OPTION
                            if (validateVarible(productId)) {
                                //CREATE NEW POSITIONING TO THE PRODUCT SELECTED
                                eId = productId;
                                eType = 'PRODT';
                            } else {
                                //----------- NO ACTION [ERROR IN SYSTEM] -----------
                            }
                        }
                    } else {
                        actionPositioning = 'Update';
                        if (entityType === 'COMPT') {//the current positioning is COMPETITOR
                            if (validateVarible(productId)) {
                                //************ NO ACTION TO HERE-TO FUTURE ***********
                                //CREATE NEW POSITIONING TO THE PRODUCT SELECTED
                                actionPositioning = 'Create'; //WHEN THE USER SEE THE POSITIONING OF COMPETITOR IN PRODUCT
                                eId = productId;
                                eType = 'PRODT';
                            } else {
                                //CREATE NEW POSITIONING TO THE COMPETITOR SELECTED
                                eId = competitorId;
                                eType = 'COMPT';
                            }
                        } //************ NO ACTION TO HERE-TO FUTURE ***********
                        else { //the current positioning is PRODUCT 
                            //PRODUCT SHOULD BE SELECTED WITH A OPTION
                            if (validateVarible(productId)) {
                                //CREATE NEW POSITIONING TO THE PRODUCT SELECTED
                                eId = productId;
                                eType = 'PRODT';
                            } else {
                                //----------- NO ACTION [ERROR IN SYSTEM] -----------
                            }
                        }
                    }
                }
                var urlActCreate = '<%= Url.Action("CreatePositioning", "Comparinator") %>';
                var urlActUpdate = '<%= Url.Action("UpdatePositioning", "Comparinator") %>';
                var urlActGet = '<%= Url.Action("GetPositioningById", "Comparinator") %>';
                AddGeneralClientPositioningDialog(this, "Edit Statment", urlActCreate, urlActUpdate, urlActGet, industryId, eId, eType, pRelation, '<%= ViewData["C"] %>', '<%= ViewData["U"] %>', positioningId, actionPositioning, typeClasification);
            }
        });
    });
</script>

<div id="contentleftmenu">
    <%  using (Html.BeginForm(null, null, FormMethod.Post, new { id = "FrontEndForm", style = "width:100%" }))
        {   %>
    <%=Html.Hidden("U",(string)ViewData["U"]) %>
    <%=Html.Hidden("C",(string)ViewData["C"]) %>
    <div id="MenuBox" class="contentmenu">
        <div class="comboBoxContent" style="float: left">
            <label for="Industry" class="comboBoxHeader">
                <asp:Literal ID="Industry" runat="server" Text="Industry " /><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>"
                    alt=":" align="top" />
            </label>
            <%=Html.DropDownList("Industry", (SelectList)ViewData["IndustryCollection"], new { onchange = "ChangeSFAction('" + Url.Action("ContentPortalCHI", "SalesForce") + "')" })%>
        </div>
        <div class="comboBoxContent" style="float: left">
            <label for="Competitor" class="comboBoxHeader">
                <asp:Literal ID="Competitor" runat="server" Text="Competitor " /><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>"
                    alt=":" align="top" />
            </label>
            <%= Html.DropDownList("Competitor", (SelectList)ViewData["CompetitorCollection"], new { onchange = "ChangeSFAction('" + Url.Action("ContentPortalCHC", "SalesForce") + "')" })%>
        </div>
        <div class="comboBoxContent" style="float: left">
            <label for="Product" class="comboBoxHeader">
                <asp:Literal ID="Product" runat="server" Text="Product " /><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>"
                    alt=":" align="top" />
            </label>
            <%= Html.DropDownList("Product", (SelectList)ViewData["ProductCollection"], new { onchange = "ChangeSFAction('" + Url.Action("ContentPortalCHP", "SalesForce") + "')" })%>
        </div>
    </div>
    <%} %>
    <br />
    <div style="float: left; width: 100%">
        <div>
            <%  UserProfile user = (UserProfile)ViewData["User"];    
                bool DefaultsDisabPublComm = Convert.ToBoolean(ViewData["DefaultsDisabPublComm"].ToString());
                IList<LibraryCatalog> libraryCatalog = (IList<LibraryCatalog>)ViewData["LibraryCatalog"];
                string detailsEntities = string.Empty;
                if (ViewData["DetailsEntities"] != null) { detailsEntities = ViewData["DetailsEntities"].ToString(); }
                string entityDetail = string.Empty;
                if (ViewData["EntityDetail"] != null) { entityDetail = ViewData["EntityDetail"].ToString(); }
                string nameDetail = string.Empty;
                if (ViewData["NameDetail"] != null) { nameDetail = ViewData["NameDetail"].ToString(); }
                string imageDetail = string.Empty;
                if (ViewData["ImageDetail"] != null) { imageDetail = ViewData["ImageDetail"].ToString(); }
                string urlDetail = string.Empty;
                if (ViewData["UrlDetail"] != null) { urlDetail = ViewData["UrlDetail"].ToString(); }
                string urlDetailText = string.Empty;
                if (ViewData["UrlDetailText"] != null) { urlDetailText = ViewData["UrlDetailText"].ToString(); }
                string descriptionDetail = string.Empty;
                if (ViewData["DescriptionDetail"] != null) { descriptionDetail = ViewData["DescriptionDetail"].ToString(); }
                IList<StrengthWeakness> sByIndtr = null;
                if (ViewData["StrengthforIndustry"] != null) sByIndtr = (IList<StrengthWeakness>)ViewData["StrengthforIndustry"];
                IList<StrengthWeakness> wByIndtr = null;
                if (ViewData["WeaknessforIndustry"] != null) wByIndtr = (IList<StrengthWeakness>)ViewData["WeaknessforIndustry"];
                string swCompetitorId = string.Empty;
                if (ViewData["SWCompetitorId"] != null) { swCompetitorId = ViewData["SWCompetitorId"].ToString(); }
                string swIndustryId = string.Empty;
                if (ViewData["SWIndustryId"] != null) { swIndustryId = ViewData["SWIndustryId"].ToString(); }
                bool competitorHasComment = true; //by default
                if (ViewData["CompetitorHasComment"] != null) { competitorHasComment = (bool)ViewData["CompetitorHasComment"]; }
                Positioning pstng = null;
                if (ViewData["Positionig"] != null) { pstng = (Positioning)ViewData["Positionig"]; }
                Positioning cmpmg = null;
                if (ViewData["Competitive Messaging"] != null) { cmpmg = (Positioning)ViewData["Competitive Messaging"]; } 
                %>
                <div>
                <% for (int i = 0; i < libraryCatalog.Count; i++)
                { %>
                    <%= Html.Porlet(libraryCatalog[i],i,DefaultsDisabPublComm, detailsEntities, entityDetail, nameDetail, imageDetail, urlDetail, urlDetailText, descriptionDetail, sByIndtr, wByIndtr, swCompetitorId, swIndustryId, competitorHasComment, pstng, cmpmg, user)%>    
                <%} %>
                </div>
        </div>
    </div>
</div>
<div id="FormComments">
</div>
<div id="FormDiscussions">
</div>
<div id="FormConfirm" title="Confirm deletion?" class="displayNone">
    <p>
        <span class="ui-icon ui-icon-alert confirmDialog"></span>These comment will be deleted.
        Are you sure?</p>
</div>
<div id="FormFeedBack">
</div>
<div id="FormQuiz">
</div>
<div id="FormMessage">
</div>
<div id="FileNotFound">
    <br />
</div>
<div id="ExternalResponse" title="Comment Form">
</div>
<div id="ExternalResponseNew">
</div>
<div id="DiscussionsResponse">
</div>
<div id="DiscussionsResponseNew">
</div>
<div id="PrivateCommentNewDlg">
</div>
<div id="PositioningBox"></div>
<div style="display: none;">
    <iframe id="DownloadFileSection" src="javascript: void(0);" frameborder="0" marginheight="0"
        marginwidth="0"></iframe>
</div>
<div id="DownloadFileNotFound" title="Error" style="display: none;">
    <p>
        <span class="ui-icon ui-icon-alert"></span><strong>File not found...!</strong>
    </p>
</div>
<div id="SuccessSubmitted" title="Message" style="display: none;">
    <p>
        <strong>It was successfully submitted!</strong>
    </p>
</div>
<div id="StrengthWeaknessBox"></div>
<div id="mystickytooltip" class="stickytooltip" style="max-width:35%;max-height:190px;">
        <div style="padding: 5px">
            <%if ((string)ViewData["IndustryImageUrl"] == null || (string)ViewData["IndustryImageUrl"] == "")
              { %>
            <div id="sticky1" class="atip">
                <img src="/Content/Images/Icons/none.png" />
            </div>
            <%}
              else
              {
                  string IndustryImageUrl = (string)ViewData["IndustryImageUrl"];
                  if (IndustryImageUrl.Substring(0, 2).Equals("./"))
                      IndustryImageUrl = "." + IndustryImageUrl;%>
            <div id="sticky1" class="atip">
                <img src="<%=IndustryImageUrl%>" style="max-width:100%;max-height:168px"/>
            </div>
            <%} %>
            <%if ((string)ViewData["CompetitorImageUrl"] == null || (string)ViewData["CompetitorImageUrl"] == "")
              { %>
            <div id="sticky2" class="atip">
                <img src="/Content/Images/Icons/none.png" />
            </div>
            <%}
              else
              {
                  string CompetitorImageUrl = (string)ViewData["CompetitorImageUrl"];
                  if (CompetitorImageUrl.Substring(0, 2).Equals("./"))
                      CompetitorImageUrl = "." + CompetitorImageUrl;%>
            <div id="sticky2" class="atip">
                <img src="<%=CompetitorImageUrl%>" style="max-width:100%;max-height:168px"/>
            </div>
            <%} %>
            <%if ((string)ViewData["ProductImageUrl"] == null || (string)ViewData["ProductImageUrl"] == "")
              { %>
            <div id="sticky3" class="atip">
                <img src="/Content/Images/Icons/none.png" />
            </div>
            <%}
              else
              {
                  string ProductImageUrl = (string)ViewData["ProductImageUrl"];
                  if (ProductImageUrl.Substring(0, 2).Equals("./"))
                      ProductImageUrl = "." + ProductImageUrl;%>
            <div id="sticky3" class="atip">
                <img src="<%=ProductImageUrl%>" style="max-width:100%;max-height:168px" />
            </div>
            <%} %>
    </div>
    <div class="stickystatus">
    </div>
</div>
<script type="text/javascript">
    $('#DownloadFileNotFound').dialog({
        bgiframe: true,
        autoOpen: false,
        modal: false,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });
    $('#SuccessSubmitted').dialog({
        bgiframe: true,
        autoOpen: false,
        modal: false,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });
</script>