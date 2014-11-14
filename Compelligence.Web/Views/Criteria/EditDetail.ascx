<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<% string formId = ViewData["Scope"].ToString() + "CriteriaEditForm"; %>
<%= Html.ValidationSummary()%>
<script type="text/javascript">
    var hiddenOtherTextBoxs = function() {
        $('.divEditGroup').hide();
        $('.divEditSet').hide();
        $('.divGroup').show();
        $('.divSet').show();
    };
    var showEdit = function(type, identifier) {
        hiddenOtherTextBoxs();
        $('#div' + type + '_' + identifier).hide();
        $('#divEdit' + type + '_' + identifier).show();
        $('#txt' + type + '_' + identifier).focus();
    };
    var saveEdit = function(type, identifier) {
        var txtName = $('#txt' + type + '_' + identifier).val();
        var ubfcompetitors = '<%= Url.Action("UpdateName", "Criteria")%>';
        if (txtName.length > 0) {
            $.post(ubfcompetitors, { id: identifier, type: type, name: txtName });
            $('#lbl' + type + '_' + identifier).text(txtName);
            $('#div' + type + '_' + identifier).show();
            $('#divEdit' + type + '_' + identifier).hide();
        }
    };
    /// THIS FUNCTION WILL UPDATE THE NAME OF CRITERIA GROUP/SET
    /// PARAMETER type -> this can be group/set
    /// PARAMETER identifier ->Id of entity
    var saveName = function(type, identifier) {
        var txtName = $('#txt' + type + '_' + identifier).val();
        var ubfcompetitors = '<%= Url.Action("UpdateName", "Criteria")%>';
        if (txtName.length > 0) {
            $.post(ubfcompetitors, { id: identifier, type: type, name: txtName });
            $('#lbl' + type + '_' + identifier).text(txtName);
        }
    };

    var SaveNewNameAndSaveOrder = function(currentId) {
        var values = currentId.split('_');
        if (values.length > 1) {
            var identifier = values[1];
            var type = '';
            if (values[0].indexOf('Group') != -1) {
                type = 'Group';
            }
            else if (values[0].indexOf('Set') != -1) {
                type = 'Set';
            }
            else { }
            if (type != undefined && type != null && type != '' && identifier != undefined && identifier != null && identifier != '') {
                saveEdit(type, identifier);
            }
        }
        // e.preventDefault();
        var formIdTo = '#<%=formId %>';
        $(formIdTo).submit();
    };
    var SaveNewName = function(currentId) {
        var values = currentId.split('_');
        if (values.length > 1) {
            var identifier = values[1];
            var type = '';
            if (values[0].indexOf('Group') != -1) {
                type = 'Group';
            }
            else if (values[0].indexOf('Set') != -1) {
                type = 'Set';
            }
            else { }
            if (type != undefined && type != null && type != '' && identifier != undefined && identifier != null && identifier != '') {
                saveName(type, identifier);
            }
        }
    };
    $(document).ready(function() {
        $('.txtEdit').keydown(function(e) {
            var currentId = $(this).attr('id');
            if (e.keyCode == 13) {
                SaveNewNameAndSaveOrder(currentId);
            }
        });
        $('.txtEdit').focusout(function(e) {
            var currentId = $(this).attr('id');
            console.log('focusout: ' + currentId);
            SaveNewName(currentId);
        });
    });
</script>
<style type="text/css">
.crttitle
{
    background: url(<%= ResolveUrl("~/content/images/styles/BGYellowGrad1.gif") %>) repeat scroll center top #AAAAAA;    
        border: thin solid black;
    height: 25px;
    margin-left: auto;
    margin-right: auto;
    width: 50%;
}
.crttitle-text
{
    padding: 3px;
     /*position: absolute;*/
}
.crtcontainer
{
    border-style: solid;
    border-width: thin;
    margin-left: auto;
    margin-right: auto;
    width: 50%;
}
.crtset {
    background-color: white;
    border: thin solid darkgray;
    height: 24px;
}
.crtline
{
    margin:0px;
    padding:0px;
}
.crt {
    background-color: white;
    border: thin solid darkgray;
    height: 24px;
}
.crtsortlist {
    list-style: none outside none;
    margin-bottom: 1px;
    margin-left: 1;
    margin-top: 4px;
    padding: 0;
}
.crtsortlist2 {
    list-style: none outside none;
    margin-bottom: 1px;
    margin-left: 1px;
    margin-top: 4px;
    padding: 0;
}
.crtsortlist3 {
    list-style: none outside none;
    margin-bottom: 1px;
    margin-left: 20px;
    margin-top: 4px;
    padding: 0;
}
.crticon
{
    float: left;
    margin-left: 2;
}
</style>

<% using (Ajax.BeginForm("SaveDetail", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CriteriaDetailFormContent",
               OnBegin = "showLoadingDialog",
               OnComplete = "function() { hideLoadingDialog();}",
               OnSuccess = "function(){initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Criteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           }, new { id = formId }
           ))
{ %>
<div class="indexTwo">
    <div class="buttonLink">
        <input class="button" type="submit" value="Save" />
        <input class="button" type="button" value="Cancel" onclick="javascript: reloadGrid('#<%=ViewData["Scope"] %>CriteriaIndustryDetailListTable');cancelEntity('EnvironmentIndustry', 'Criteria', 'CriteriaDetail', true);" />
    </div>
    <div style="overflow-x:hidden; height:320px; width:99%;">
        <div class="crttitle">
            <div style="float: left;margin-top: 1%;"><b class='crttitle-text'>Drag and Drop Groups or Sets</b></div>
			 <b style="float:right">
			 <input name="order" type="radio"  checked onclick="updateSort(1)"/> Groups
			 <input name="order" type="radio"  onclick="updateSort(2)"/> Sets
			 <input name="order" type="radio"  onclick="updateSort(3)"/> Criterias
			 </b>             
        </div>
        <div class="crtcontainer">
        <div class="crtsortlist">
         
<%      //List<CriteriaGroupContentDTO> Groups = (List<CriteriaGroupContentDTO>)ViewData["OrderedCriteriaGroups"];
            CatalogOrderDTO catalogToOrder = (CatalogOrderDTO) ViewData["OrderedCatalogCriteria"];
            
            decimal GroupId,SetId,CriteriaId;
            string GroupName,SetName,CriteriaName;%>
            <%=Html.Hidden("IndustriesId", catalogToOrder.IndustryId) %>
            <%=Html.Hidden("IsDetail",(string)ViewData["IsDetail"])%>
            <%=Html.Hidden("Scope", (string)ViewData["Scope"]) %>
        <% if (catalogToOrder.LstCriteriaGroup != null)
        {
            for (int i = 0; i < catalogToOrder.LstCriteriaGroup.Count; i++ )
            {
                GroupId = catalogToOrder.LstCriteriaGroup[i].CriteriaGroupId;
                GroupName = catalogToOrder.LstCriteriaGroup[i].CriteriaGroupName;  
%> 
            <div id="group_<%=GroupId%>">
                <img src="<%= Url.Content("~/Content/Images/Icons/sort-solid-black.png") %>" class="crticon" alt="move" />
                <div style="width:500px">
                    <%=Html.Hidden("CriteriasGroupId", GroupId)%>
                    <div id="divGroup_<%= GroupId%>"  class="divGroup">
                        <label  id="lblGroup_<%= GroupId%>" class="lgroup" style="font-weight:bold">
                            <%=GroupName%>
                        </label>
                        <img src="<%= Url.Content("~/Content/Images/Icons/properties.png") %>" onclick="showEdit('Group','<%= GroupId%>')"/>
                    </div>
                    <div id="divEditGroup_<%= GroupId%>" style="display:none" class="divEditGroup">
                        <input type="text" id="txtGroup_<%= GroupId%>" name="txtGroup_<%= GroupId%>" class="txtEdit" value="<%=GroupName%>" />
                        <img src="<%= Url.Content("~/Content/Images/Icons/properties.png") %>" onclick="saveEdit('Group','<%= GroupId%>')"/>
                    </div>
                </div>
                
<%              if ( catalogToOrder.LstCriteriaGroup[i].LstCriteriaSet != null ) { %>
                <div class="crtsortlist2" style="margin-right:10px">
<%
                    for (int j=0; j<catalogToOrder.LstCriteriaGroup[i].LstCriteriaSet.Count;j++)
                    {
                        SetId = catalogToOrder.LstCriteriaGroup[i].LstCriteriaSet[j].CriteriaSetId;
                        SetName = catalogToOrder.LstCriteriaGroup[i].LstCriteriaSet[j].CriteriaSetName;
%>
                <div id="set_<%=SetId%>">
                    <img src="<%= Url.Content("~/Content/Images/Icons/sort-solid-gray.png") %>" class="crticon" 
                        alt="move" style='Z-INDEX: 1001; height: 15px; margin-left: 1px' />
                    <div style="width:490px">
                        <%=Html.Hidden("CriteriaSetId", SetId)%>
					    <%=Html.Hidden("CriteriaSetIdByGroup", GroupId.ToString() + '_' + SetId.ToString())%>
                        <div id="divSet_<%= SetId%>"  class="divSet">
                            <label id="lblSet_<%= SetId%>" class="lset" style="font-weight:bolder">
                                <%=SetName%>
                            </label>
                            <img src="<%= Url.Content("~/Content/Images/Icons/properties.png") %>" onclick="showEdit('Set','<%= SetId%>')"/>
                        </div>
                        <div id="divEditSet_<%= SetId%>" style="display:none" class="divEditSet">
                            <input type="text" id="txtSet_<%= SetId%>" name="txtSet_<%= SetId%>" value="<%=SetName%>" class="txtEdit" />
                            <img src="<%= Url.Content("~/Content/Images/Icons/properties.png") %>" onclick="saveEdit('Set','<%= SetId%>')"/>
                        </div>
                    </div>
                 
<%                      if ( catalogToOrder.LstCriteriaGroup[i].LstCriteriaSet[j] != null ) { %>
                    <div class="crtsortlist3">
                  <%
                            for (int k=0; k<catalogToOrder.LstCriteriaGroup[i].LstCriteriaSet[j].LstCriteria.Count;k++)
                            {
                                CriteriaId = catalogToOrder.LstCriteriaGroup[i].LstCriteriaSet[j].LstCriteria[k].CriteriaId;
                                CriteriaName = catalogToOrder.LstCriteriaGroup[i].LstCriteriaSet[j].LstCriteria[k].CriteriaName;
                  %>
                        <div class="crt" id="criteria_<%=CriteriaId%>">
                            <img src="<%= Url.Content("~/Content/Images/Icons/sort-solid-gray.png") %>" class="crticon" 
                                alt="move" style='Z-INDEX: 1001; width: 16px;' />
                            <div style="width:305px">
                                <%=Html.Hidden("CriteriaId", CriteriaId)%>
								<%=Html.Hidden("CriteriaIdBySet", SetId.ToString() + '_' + CriteriaId.ToString())%>
                                <label class="lcri">
                                    <%=CriteriaName%>
                                </label>
                            </div>
                        </div>
                        
                  <%        } %>
                    </div>
<%                      } %>
                </div>
                        
<%                  } %>
                </div>
<%              } %>
            </div>
<%          }
         }
%>
        </div>
        </div>
    </div>
</div>
<% } %>

<script type="text/javascript">
    $(document).ready(function() {

        function ActiveSortable(collectionId, typeCollection, zIndex) {
            $(collectionId).sortable({
                items: typeCollection,
                cursor: "pointer",
                //opacity: 0.7,
                handle: "img.crticon",
                update: function() { },
                zIndex: zIndex
            });
        }

        ActiveSortable("div.crtsortlist", "> div", 1000);
        ActiveSortable("div.crtsortlist2", "> div", 1000);
        ActiveSortable("div.crtsortlist3", "> div", 1000);

        updateSort(1);
    });

    function updateSort(source) {

        if (source.toString() == "1") //g
        {
            $("div.crtsortlist3").sortable("option", "disabled", true);
            $("div.crtsortlist3").css('opacity', '0.9');
            $("div.crtsortlist2").sortable("option", "disabled", true);
            $("div.crtsortlist2").css('opacity', '0.9');
            $("div.crtsortlist").sortable("option", "disabled", false);
            $("div.crtsortlist").css('opacity', '');
            $(".lcri").css("color", "#535050");
            $(".lset").css("color", "#535050");
            $(".lgroup").css("color", "black");
            $("div.crtsortlist3").hide();
            $("div.crtsortlist2").hide();
        }
        else if (source == 2) //s
        {
            $("div.crtsortlist3").sortable("option", "disabled", true);
            $("div.crtsortlist3").css('opacity', '0.9');
            $("div.crtsortlist").sortable("option", "disabled", true);
            $("div.crtsortlist").css('opacity', '0.9');
            $("div.crtsortlist2").sortable("option", "disabled", false);
            $("div.crtsortlist2").css('opacity', '');
            $(".lcri").css("color", "#535050");
            $(".lset").css("color", "black");
            $(".lgroup").css("color", "#535050");
            $("div.crtsortlist3").hide();
            $("div.crtsortlist2").show();
        }
        else if (source == 3)  //c
        {
            $("div.crtsortlist").sortable("option", "disabled", true);
            $("div.crtsortlist").css('opacity', '0.9');
            $("div.crtsortlist2").sortable("option", "disabled", true);
            $("div.crtsortlist2").css('opacity', '0.9');
            $("div.crtsortlist3").sortable("option", "disabled", false);
            $("#div.crtsortlist3").css('opacity', '');
            $(".lcri").css("color", "black");
            $(".lset").css("color", "#535050");
            $(".lgroup").css("color", "#535050");
            $("div.crtsortlist3").show();
            $("div.crtsortlist2").show();
        }
    }    
</script>

