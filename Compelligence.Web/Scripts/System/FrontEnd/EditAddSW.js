////////////
//Rename Strength
function editStrength(id, name, description, isglobal, urlc_rename, SWIndustryId) {
    var descip = $('#strengths_' + id).text();
    var nameItem = $('#strengths_' + id).attr('namesw');
    var global = $('#strengths_' + id).attr('globalsw');
    var u = $('#U').val();
    var commentObject = $("#StrengthWeaknessBox");

    commentObject.dialog({ autoOpen: false,
        title: "Edit Strength",
        width: 360,
        modal: true,
        buttons: {
            "Delete": function() {
                $.get(urlc_deleteS, { Id: id },
                function() {
                    $('#strengths_' + id).remove();

                    var count = $("#Strengths li").length;

                    console.log(count);
                    if (count == 0) {//<li id='messageS'>
                        $("#Strengths").append("<li id='messageS'><b>No strengths configured for this competitor and industry</b></li>");
                        $("#Strengths").addClass("listWithoutBullet");
                    }
                });

                $(this).dialog("destroy");
            },
            "Ok": function() {
                var name = commentObject.children("#name").val();
                var description = commentObject.children("#description").val();
                var isglobal = commentObject.children("#check").is(':checked');
                console.log(isglobal);
                //start validation

                //Strength name length validation
                if (name == " ") {
                    commentObject.children("#alert").html("<b style='color:red'>Please enter a valid name!</b><br><br>");
                    commentObject.children("#name").focus();
                    setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                    return false;
                }
                //Strength description length validation
                if (description == " ") {
                    commentObject.children("#alert").html("<b style='color:red'>Please enter a valid description!</b><br><br>");
                    commentObject.children("#description").focus();
                    setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                    return false;
                }

                //ebd-validation

                $.get(urlc_rename, { Id: id,
                    N: name,
                    D: description,
                    G: isglobal,
                    industryid: SWIndustryId,
                    U:u
                },
            function(data) {
                if (data != "") {
                    $("#strengths_" + id).html(description); //refrescar el div que contenga los  Strength
                    $("#strengths_" + id).attr('namesw', name);
                    $("#strengths_" + id).attr('globalsw', isglobal);

                }
            });
                $(this).dialog("destroy");
            }
            //$(this).dialog("destroy");
        }
    });


var setChecked = '';
if (global === "true" || global === "True" || global === "TRUE" || global === "y" || global === "Y") {
    var valorTF = "true";
    setChecked = 'checked';
}
else {
    var valorTF = "false";
} 
    var contentlines = "<label id='alert'></label>";
    contentlines += "Enter update strength name: <input id='name' type='text' value='" + nameItem + "' style='width:324px;margin-left:1px;'>";
    contentlines += "Enter update strength description: <textarea id='description' value='" + descip + "' style='width:324px;margin-left:1px;'>" + descip + "</textarea>";
    contentlines += "Applies to all industries: <input id='check' type='checkbox' style='height:10px;' name=''  class='chkItemConfiguration chkComparinatorExport' " + setChecked + " />"; 
    
    commentObject.html(contentlines);
    commentObject.dialog("open");

    commentObject.children("#name").focus();
    
};

////////////////////
//Rename Weakness

function editWeakness(id, name, description, isglobal, urlc_rename, SWIndustryId) {
    var descip = $('#weakness_' + id).text();
    var nameItem = $('#weakness_' + id).attr('namesw');
    var global = $('#weakness_' + id).attr('globalsw');
    var u = $('#U').val();
    var commentObject = $("#StrengthWeaknessBox");

    commentObject.dialog({ autoOpen: false,
        title: "Edit weakness",
        width: 360,
        modal: true,
        buttons: {
            "Delete": function() {
                $.get(urlc_deleteW, { Id: id },
                function() {
                    $('#weakness_' + id).remove();

                    var count = $("#Weaknesses li").length;

                    console.log(count);
                    if (count == 0) {
                        $("#Weaknesses").append("<li id='messageW'><b>No weaknesses configured for this competitor and industry</b></li>");
                        $("#Weaknesses").addClass("listWithoutBullet");
                    }
                });

                $(this).dialog("destroy");
            },
            "Ok": function() {
                var name = commentObject.children("#name").val();
                var description = commentObject.children("#description").val();
                var isglobal = commentObject.children("#check").is(':checked');
                //start validation
                //Weakness name length validation
                if (name == " ") {
                    commentObject.children("#alert").html("<b style='color:red'>Please enter a valid name!</b><br><br>");
                    commentObject.children("#name").focus();
                    setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                    return false;
                }
                //Weakness description length validation
                if (description == " ") {
                    commentObject.children("#alert").html("<b style='color:red'>Please enter a valid description!</b><br><br>");
                    commentObject.children("#description").focus();
                    setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                    return false;
                }

                //ebd-validation

                $.get(urlc_rename, { Id: id,
                    N: name,
                    D: description,
                    G: isglobal,
                    industryid: SWIndustryId,
                    U:u
                },
            function(data) {
                if (data != "") {
                    $("#weakness_" + id).html(description); //refrescar el div que contenga los  Strength
                    $("#weakness_" + id).attr('namesw', name);
                    $("#weakness_" + id).attr('globalsw', isglobal);
                }
            });
                $(this).dialog("destroy");
            }
        }

    });

var setChecked = '';
if (global === "true" || global === "True" || global === "TRUE" || global ==="y" || global ==="Y") {
    var valorTF = "true";
    setChecked = 'checked';
}
else 
{
    var valorTF = "false";
} 
        //var valcheck = "false"; }
    var contentlines = "<label id='alert'></label>";
    contentlines += "Enter update weakness name: <input id='name' type='text' value='" + nameItem + " ' style='width:324px;margin-left:1px;'>";
    contentlines += "Enter update weakness description: <textarea id='description' value='" + descip + "' style='width:324px;margin-left:1px;'>" + descip + "</textarea>";
    contentlines += "Applies to all industries: <input id='check' type='checkbox' value='" + valorTF + "' style='height:10px;' name='' id='' class='chkItemConfiguration chkComparinatorExport' " + setChecked + " />";
    
    commentObject.html(contentlines);
    commentObject.dialog("open");

    commentObject.children("#name").focus();

};

////////////////
//Ad new Strength

function addStrength(SWCompetitorId, SWIndustryId, urlc_add, urlc_upd) { //Idstrengthlist == entity id

    var commentObject = $("#StrengthWeaknessBox");
    var c = $('#C').val();
    var u = $('#U').val();
    commentObject.dialog({ autoOpen: false,
        title: "Add new strength",
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            var name = commentObject.children("#name").val();
            var description = commentObject.children("#description").val();
            var isglobal = commentObject.children("#check").is(':checked');
            //alert(isglobal);
            //start validation

            //Strength name length validation
            if (name == " ") {
                commentObject.children("#alert").html("<b style='color:red'>Please enter a valid name!</b><br><br>");
                commentObject.children("#name").focus();
                setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                return false;
            }
            //Strength description length validation
            if (description == " ") {
                commentObject.children("#alert").html("<b style='color:red'>Please enter a valid description!</b><br><br>");
                commentObject.children("#description").focus();
                setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                return false;
            }

            //name exist validation

            $.get(urlc_add, {
                entityId: SWCompetitorId,
                industryid: SWIndustryId,
                N: $("#name").val(),
                D: $("#description").val(),
                G: $("#check").is(':checked'),
                C: c,
                U: u
            },
            function(data) {
                if (data != "") {
                    $("#messageS").remove();
                    $("#Strengths").removeClass("listWithoutBullet");
                    $("#Strengths").append('<div id="strengths"><li id="strengths_' + data + '"  title="Click to re-name saved Strength." onclick="editStrength(\'' + data + '\',\'' + $("#name").val() + '\',\'' + $("#description").val() + '\',\'' + $("#check").val() + '\',\'' + urlc_upd + '\');" namesw="' + $("#name").val() + '" globalsw="' + $("#check").is(':checked') + '" >' + $("#description").val() + '</li></div>');
                }
            });
            $(this).dialog("destroy");
        }
        }

    });
    var globalString = '';
    var strGlobal = " ";
    if (strGlobal) {
        var valcheck = " ";
        globalString = 'checked';
    }
    else {

        var valcheck = "Y";
    } 
    
    var contentlines = "<label id='alert'></label>";
    contentlines += "Strength name: <input id='name' type='text' value=' ' style='width:324px;margin-left:1px;'>";
    contentlines += "Strength description: <textarea id='description' value=' ' style='width:324px;margin-left:1px;'></textarea>";
    contentlines += "Applies to all industries: <input id='check' type='checkbox'  style='height:10px;' globalString name='check' >"; 
    
    commentObject.html(contentlines);
    commentObject.dialog("open");

    commentObject.children("#name").focus();
    //commentObject.children("#description").focus();

};
///////////////
//Ad new Weakness
function addWeakness(SWCompetitorId, SWIndustryId, urlc_add, urlc_upd) {

    var commentObject = $("#StrengthWeaknessBox");
    var c = $('#C').val();
    var u = $('#U').val();
    commentObject.dialog({ autoOpen: false,
        title: "Add new weakness",
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            var name = commentObject.children("#name").val();
            var description = commentObject.children("#description").val();
            var isglobal = commentObject.children("#check").is(':checked');
            //start validation

            //Weakness name length validation
            if (name == "") {
                commentObject.children("#alert").html("<b style='color:red'>Please enter a valid name!</b><br><br>");
                commentObject.children("#name").focus();
                setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                return false;
            }
            //Weakness description length validation
            if (description == "") {
                commentObject.children("#alert").html("<b style='color:red'>Please enter a valid description!</b><br><br>");
                commentObject.children("#description").focus();
                setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                return false;
            }

            //ebd-validation

            $.get(urlc_add, {
                entityId: SWCompetitorId,
                industryid: SWIndustryId,
                N: $("#name").val(),
                D: $("#description").val(),
                G: $("#check").is(':checked'),
                C: c,
                U: u
            },
             function(data) {
                 if (data != "") {
                     $("#messageW").remove();
                     $("#Weaknesses").removeClass("listWithoutBullet");
                     $("#Weaknesses").append('<div id="weakness"><li id="weakness_' + data + '"  title="Click to re-name saved weakness." onclick="editWeakness(\'' + data + '\',\'' + $("#name").val() + '\',\'' + $("#description").val() + '\',\'' + $("#check").val() + '\',\'' + urlc_upd + '\');" namesw="' + $("#name").val() + '" globalsw="' + $("#check").is(':checked') + '">' + $("#description").val() + '</li></div>');                     
                 }
             });
            $(this).dialog("destroy");
        }
        }

    });
    var globalString = '';
    var strGlobal = " ";
    if (strGlobal) {
        var valcheck = " ";
        globalString = 'checked';
    }
    else {
        
        var valcheck = "Y";
    }    
    
    var contentlines = "<label id='alert'></label>";
    contentlines += "Weakness name: <input id='name' type='text' value='' style='width:324px;margin-left:1px;'>";
    contentlines += "Weakness description: <textarea id='description' value=' ' style='width:324px;margin-left:1px;'></textarea>";
    contentlines += "Applies to all industries: <input id='check' type='checkbox' style='height:10px;' name='check' class='chkItemConfiguration chkComparinatorExport'>";   
	
	commentObject.html(contentlines);
    commentObject.dialog("open");
    commentObject.children("#name").focus();
    //commentObject.children("#description").focus();
};