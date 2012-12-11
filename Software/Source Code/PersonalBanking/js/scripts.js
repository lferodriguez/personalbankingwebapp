function showDeleteOption(checkbox,div) {

    if ($("#" + checkbox).attr('checked') == true) {
        $("#" + div).show();
    } else {
        $("#" + div).hide();
    }
    

}

