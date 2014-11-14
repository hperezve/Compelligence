function showOptions(nameMenu, nameSubMenu, ppos) {
    var pos = $('#' + nameMenu).position();
    if ((arguments.length == 3) && (arguments[2] != null))
        pos = ppos;
    if (nameSubMenu == 'optionsWinLoss' && $("#" + nameSubMenu).children(0).length <= 0) {
        return;
    }

    $("#" + nameSubMenu).show();
    if (nameSubMenu == "optionsWinLoss") {
        $("#" + nameSubMenu).css("left", pos.left + 201 + "px");
        $("#" + nameSubMenu).css("top", (pos.top + 157) + "px");
    }
    else {
        $("#" + nameSubMenu).css("left", pos.left + "px");
        $("#" + nameSubMenu).css("top", (pos.top + 37) + "px");
    }

    console.log("l:" + pos.left + "t:" + pos.top);

}

function hideOptions(nameSubMenu) {
    $("#" + nameSubMenu).css({ display: "none" });
}
