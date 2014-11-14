<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>

<style type="text/css">
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
<script type="text/javascript">
    
    function toggle_visibility1(container, enabled) {
        var e = container.getElementsByTagName('li')[0];
        //if (e.style.display == 'block')
        if (!enabled)
            e.style.display = 'none';
        else {
            e.style.display = 'block';
            e.setAttribute("className", "tooltipspanhover"); //For IE; harmless to other browsers.
            e.setAttribute("class", "tooltipspanhover"); //For IE; harmless to other browsers.
        }
    }
    
    function toggle_visibility(container, enabled) {
        var e = container.getElementsByTagName('span')[0];
        //if (e.style.display == 'block')
        if (!enabled)
            e.style.display = 'none';
        else {
            e.style.display = 'block';
            e.setAttribute("className", "tooltipspanhover"); //For IE; harmless to other browsers.
            e.setAttribute("class", "tooltipspanhover"); //For IE; harmless to other browsers.
        }
    }
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
        var url = '<%= Url.Action("ChangeIndustry", "ContentPortal") %>';
        ChangeAction(url);
    };
    //THIS METHOD WILL SELECT A COMPETITOR IN THE LIST OF COMPETITORS
    var setIndAndCompVal = function(competitorId){
        $('#Competitor').val(competitorId);
        $('#Competitor').multiselect('refresh');
        var url = '<%= Url.Action("ChangeCompetitor", "ContentPortal") %>';
        ChangeAction(url);
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
            var url = '<%= Url.Action("ChangeProduct", "ContentPortal") %>';
            ChangeAction(url);
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
                    var url = '<%= Url.Action("ChangeProduct", "ContentPortal") %>';
                    ChangeAction(url);
                }
            });
        }
    };
    $(function() {

        $("#Industry").multiselect({
            multiple: false,
            header: 'Select an <%=ViewData["IndustryLabel"]%>',
            noneSelectedText: 'Select an <%=ViewData["IndustryLabel"]%>',
            selectedList: 1,
            minWidth: 200,
            classes: "auto fixed "
        }).multiselectfilter();

        $("#Competitor").multiselect({
            multiple: false,
            header: 'Select a <%=ViewData["CompetitorLabel"]%>',
            noneSelectedText: 'Select a <%=ViewData["CompetitorLabel"]%>',
            selectedList: 1,
            minWidth: 200,
            classes: "auto fixed "
        }).multiselectfilter();

        $("#Product").multiselect({
            multiple: false,
            header: 'Select a <%=ViewData["ProductLabel"]%>',
            noneSelectedText: 'Select a <%=ViewData["ProductLabel"]%>',
            selectedList: 1,
            minWidth: 200,
            classes: "auto fixed "
        }).multiselectfilter();

        $('input[name="multiselect_Industry"]').click(function() {
            var competitorId = $('#Competitor').val();
            if (competitorId != undefined && competitorId != null && competitorId != '') {
                $('#Competitor').val('');
                $('#Competitor').multiselect('refresh');
            }
            var var_name = $("input[name=multiselect_Industry]:checked").val();
            $('#Industry').val(var_name);
            var url = '<%= Url.Action("ChangeIndustry", "ContentPortal") %>';
            ChangeAction(url);
        })

        $('input[name="multiselect_Competitor"]').click(function() {
            var verificar = $("input[name=multiselect_Competitor]:checked").val();
            $('#Competitor').val(verificar);
            var url = '<%= Url.Action("ChangeCompetitor", "ContentPortal") %>';
            ChangeAction(url);
        })

        $('input[name="multiselect_Product"]').click(function() {
            var var_name = $("input[name=multiselect_Product]:checked").val();
            $('#Product').val(var_name);
            var url = '<%= Url.Action("ChangeProduct", "ContentPortal") %>';
            ChangeAction(url);
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
    $(document).ready(function() {
        $('.pos-edt-dlg').on({
            'click': function() {
                var industrySelect = $('#Industry');
                var industryId = industrySelect.val();//INDUSTRY ID 
                var competitorSelect = $('#Competitor');
                var competitorId = competitorSelect.val();//COMPETITOR ID
                var productSelect = $('#Product');
                var productId = productSelect.val();// PRODUCT ID
                var actionPositioning = '';
                var eId = '';//ENTITY ID
                var eType = '';//ENTITY TYPE[COMPETITOR/PRODUCT]

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
<script type="text/javascript">
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
</script>

<script type="text/javascript">
    $(document).ready(function() {

        var WidthLeftContent = '<%=ViewData["LefContentWidth"]%>';
        if (WidthLeftContent == "NNN") {
            $("#contentleft").prop("style", "width:100%;");
        }
        setTooltip();
    });

</script>
<div id="contentleftmenu">
<%  using (Html.BeginForm(null, null, FormMethod.Post, new { id = "FrontEndForm", style="width:100%" }))
    {   %>
    <div id="MenuBox" class="contentmenu">
        <div class="comboBoxContent" style="float:left">
            <label for="Industry" class="comboBoxHeader">
                <asp:Literal ID="Industry" runat="server" Text="" /><%=ViewData["IndustryLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%=Html.DropDownList("Industry", (SelectList)ViewData["IndustryCollection"], new { onchange = "ChangeAction('" + Url.Action("ChangeIndustry", "ContentPortal") + "')" })%>
        </div>
        <div class="comboBoxContent" style="float:left">
            <label for="Competitor" class="comboBoxHeader">
                <asp:Literal ID="Competitor" runat="server" Text=""/><%=ViewData["CompetitorLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%= Html.DropDownList("Competitor", (SelectList)ViewData["CompetitorCollection"], new { onchange = "ChangeAction('" + Url.Action("ChangeCompetitor", "ContentPortal") + "')"})%>
        </div>
        <div class="comboBoxContent" style="float:left">
            <label for="Product" class="comboBoxHeader">
                <%=ViewData["ProductLabel"]%><img src="<%= Url.Content("~/Content/Images/Icons/next.png") %>" alt=":" align="top"/>
            </label>
            <%= Html.DropDownList("Product", (SelectList)ViewData["ProductCollection"], new { onchange = "ChangeAction('" + Url.Action("ChangeProduct", "ContentPortal") + "')" })%>
        </div>   
    </div>
<%  }   %>  
</div>
<br/> 
<div>
<% UserProfile user = (UserProfile)ViewData["User"];
   bool DefaultsDisabPublComm = Convert.ToBoolean(ViewData["DefaultsDisabPublComm"].ToString());
   IList<LibraryCatalog> libraryCatalog = (IList<LibraryCatalog>)ViewData["LibraryCatalog"];
   string detailsEntities = string.Empty;
   if (ViewData["DetailsEntities"] != null){detailsEntities = ViewData["DetailsEntities"].ToString();}
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
<br />

</div>
<br />
