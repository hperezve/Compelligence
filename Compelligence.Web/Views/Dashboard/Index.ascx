<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<System.Web.UI.DataVisualization.Charting.Chart>>" %>
<%@ Register assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>


<script type="text/javascript">
    $(function() {
        DashboardSubtabs = new Ext.TabPanel({
            renderTo: 'DashboardContent',
            autoWidth: true,
            frame: true,
            defaults: { autoHeight: true },
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                    document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > Dashboard";
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>DashboardEditFormContent', title: 'DashAdmin', id: '<%= ViewData["Scope"] %>DashboardEditFormContent'}//tabs
                ]
        });
    });
</script>
<style type ="text/css">

/* TABLE
 * ========================================================================= */
table {
  /* border: 1px solid #888; */
  border-collapse: collapse;
  line-height: 1;
  /*margin: 1em auto;*/
  width: 90%;
}

table span {
  background-position: center left;
  background-repeat: no-repeat;
  padding: .1em 0 .1em 1.2em;
}

.ui-widget input, .ui-widget select, .ui-widget textarea, .ui-widget button  {
    cursor: pointer;
    font-family: Arial;
    font-size: 12px;
    margin-bottom: 5px;
    margin-left: 5px;
    margin-top: 4px;
    margin-right: 3px;
}

.ui-multiselect-checkboxes
    {
        font-size:1.3em;
    }
    .ui-multiselect-checkboxes label input
    {
        left: 1px;
    position: relative;
    top: 5px;
    }
</style>

<% string formId = "DashboardForm";  %>

<script type="text/javascript">

    var loaderContent = function(urlAction, target) {
        var valueSelectUser = $('#SelectUser').val();

        showLoadingDialogForSection('#<%= DashboardListContent.ClientID %>');

        $('#DashboardDataContent').load(urlAction, { SelectedUser: valueSelectUser }, function() { hideLoadingDialogForSection('#<%= DashboardListContent.ClientID %>'); });
    };

    function updatePanels(data) {        
        var panels = data.toString().split(',');
        if (data == 'HiddenAllPanels') {
            $('#contentBodyDashboard > div').hide();            
        } else {
        if (panels != null && panels != undefined && panels != '') {
            $('#SelectPanels').val('');
            for (var a = 0; a < panels.length; a++) {
                    $("#SelectPanels option[value='" + panels[a] + "']").prop('selected', 'selected');
                    $("#SelectPanels").multiselect("refresh");
                    $("#" + panels[a]).show();
                }
            } else {
                $("#SelectPanels").multiselect("checkAll");
                $('#contentBodyDashboard > div').show();                             
            }
        }        
    }
    
    $("#SelectPanels").multiselect({
        multiple: true,
        selectedList: 1,
        clas_ajust: "adjust-textc",
        //minWidth: 200,
        uncheckAll: function() {
            saveShowPanels("HiddenAllPanels");
            updatePanels('HiddenAllPanels');
        },
        checkAll: function() {
            saveShowPanels($("#SelectPanels").val());
            updatePanels($("#SelectPanels").val());
        },
        click: function(event, ui) {
            if (ui.checked) {
                $("#" + ui.value).show();
                $("#SelectPanels").find('option[value="' + ui.value + '"]').prop('selected', true);
            } else {
                $("#" + ui.value).hide();
                $("#SelectPanels").find('option[value="' + ui.value + '"]').prop('selected', false);
            }            
            saveShowPanels($("#SelectPanels").val());
        },       

        classes: "auto fixed"
    });

    function saveShowPanels(panels) {
        if (panels == '' || panels == null) {
            panels = 'HiddenAllPanels';
        }        
        $.ajax({
            url: '<%= Url.Action("DashboardPanels", "Dashboard") %>',
            type: 'POST',
            data: { Id: panels.toString(), UserId: $("#SelectUser").val() },
            success: function(Data) {

            }
        });
    }

    
//    $("#SelectPanels").multiselect("checkAll");
</script>

<script type="text/javascript">    
    $(function() {
        initializeForm('#<%= formId %>');        
    });
</script>
<asp:Panel ID="DashboardListContent" runat="server">
   
    <div id="contentHeaderDashboard" style="border-bottom: 1px solid black; margin: 5px">
        <div class="tblOne" style="padding: 5px">
         <% if ((string)ViewData["ReportTo"] == "true")
            { %>
           <div style="float: left;width:50%;">
            <label for="SelectUser">
                <asp:Literal ID="DashboardSelectUser" runat="server" Text="<%$ Resources:LabelResource, DashboardSelectUser %>" />:</label>
            <%= Html.DropDownList("SelectUser", (SelectList)ViewData["AssignedToList"], new { id = "SelectUser", onChange = "loaderContent('" + Url.Action("DashboardContent", "Dashboard") + "','DashboardDataContent');" })%>
           </div>
           <% } %>
           <div style="width:100%;">
            <div style="float: left; margin-top: 3px;margin-right: 2px;">
                <label>Select Panels to View: </label>
            </div>
            <%= Html.DropDownList("SelectPanels", (SelectList)ViewData["DashboardList"], new { Multiple = "true" })%>
            </div>
        </div>        
    </div>
    
    <div id="DashboardDataContent">
        <% Html.RenderPartial("DashboardContent"); %>
    </div>
</asp:Panel>
