function ajaxFunc(url, dataString, dataType, successFunc, beforeFunc, completeFunc, async, errorFunc) {
    var async = (async == undefined) ? true : async;
    var dataType = (dataType == undefined) ? 'html' : dataType;
    var dataString = (dataString == undefined) ? {} : dataString;
    var beforeFunc = (beforeFunc == undefined) ? '' : beforeFunc;
    var errorFunc = (errorFunc == undefined) ? '' : errorFunc;
    var completeFunc = (completeFunc == undefined) ? '' : completeFunc;
    var successFunc = (successFunc == undefined) ? '' : successFunc;

    var loading = '<div style="float: none;left: 50%; position: fixed; top: 50%; z-index: 200;"><img  src="/eDairy/static/images/loader.gif"  style="width: 500px;"></div>';

    $.ajax({
        url: url,
        async: async,
        data: dataString,
        type: 'post',
        dataType: dataType,
        timeout: 360000,
        beforeSend: beforeFunc,
        error: errorFunc,
        success: successFunc,
        complete: completeFunc
    });
}

function validation(inBlock) {
    var blnk = [];
    var eMail = [];
    var name = [];
    var mobile = [];
    var num = [];
    var err = 0;

    $(inBlock + ' .blnk').each(function () {
        blnk.push($(this).attr('id'));
    });

    $(inBlock + ' .eMail').each(function () {
        eMail.push($(this).attr('id'));
    });

    $(inBlock + ' .name').each(function () {
        name.push($(this).attr('id'));
    });

    $(inBlock + ' .mobile').each(function () {
        mobile.push($(this).attr('id'));
    });

    $(inBlock + ' .num').each(function () {
        num.push($(this).attr('id'));
    });

    for (var i = 0; i < blnk.length; i++) {
        if ($.trim($('#' + blnk[i]).val()) == '') {
            $("#err_" + blnk[i]).html("<span class='text-danger'>Cannot be left blank!</span>");
            $("#" + blnk[i]).addClass('required');
            err = 1;
        }
        else {
            $("#err_" + blnk[i]).empty();
            $("#" + blnk[i]).removeClass('required');
        }
    }
    for (var i = 0; i < eMail.length; i++) {
        chkMail = checkEmail(eMail[i]);
        if (chkMail != true) {
            err++;
        }

    }

    for (var i = 0; i < name.length; i++) {
        chkName = checkNameWithSpace(name[i]);
        if (chkName != true) {
            err++;
        }
    }

    for (var i = 0; i < mobile.length; i++) {
        chkMobile = checkMobile(mobile[i]);
        if (chkMobile != true) {
            err++;
        }
    }

    for (var i = 0; i < num.length; i++) {
        chkNum = checkNumber(num[i]);
        if (chkNum != true) {
            err++;
        }
    }

    if (err > 0) {
        return false;
    }
    else {
        return true;
    }
}

function checkEmail(eId) {
    var Email = $.trim($('#' + eId).val()), filter = /^[A-z0-9]+[A-z0-9._-]+@[A-z0-9]+[A-z0-9.-]+\.[A-z]{2,4}$/;

    if (Email != '') {
        if (!filter.test(Email)) {
            $('#err_' + eId).html("<span class='text-danger'>Invalid email!</span>");
            $("#" + eId).addClass('required');
            return false;
        }
        else {
            $('#err_' + eId).html("");
            $("#" + eId).removeClass('required');
            return true;
        }
    }
    else {
        return true;
    }
}
function checkName(eId) {
    var Name = $.trim($('#' + eId).val()), filter = /^[a-zA-Z]+$/;
    if (Name != '') {
        if (!filter.test(Name)) {
            $('#err_' + eId).html("<span class='text-danger'>Only alphabets are allowed!</span>");
            $("#" + eId).addClass('required');
            return false;
        }
        else {
            $('#err_' + eId).html("");
            $("#" + eId).removeClass('required');
            return true;
        }
    }
    else {
        return true;
    }
}
function checkNameWithSpace(eId) {
    var Name = $.trim($('#' + eId).val()), filter = /^[a-zA-Z ]*$/;

    if (Name != '') {
        if (!filter.test(Name)) {
            $('#err_' + eId).html("<span class='text-danger'>Only alphabets are allowed!</span>");
            $("#" + eId).addClass('required');
            return false;
        }
        else {
            $('#err_' + eId).html("");
            $("#" + eId).removeClass('required');
            return true;
        }
    }
    else {
        return true;
    }
}
function checkMobile(eId) {
    var mobile = $.trim($('#' + eId).val()), filter = /^[789]\d{9}$/;

    if (mobile != '') {
        if (!filter.test(mobile)) {
            $('#err_' + eId).html("<span class='text-danger'>Invalid mobile no.!</span>");
            $("#" + eId).addClass('required');
            return false;
        }
        else {
            $('#err_' + eId).html("");
            $("#" + eId).removeClass('required');
            return true;
        }
    }
    else {
        return true;
    }
}

function checkNumber(eId) {
    var num = $.trim($('#' + eId).val()), filter = /^\d+$/;

    if (num != '') {
        if (!filter.test(num)) {
            $('#err_' + eId).html("<span class='text-danger'>Only numbers allow here!</span>");
            $('#' + eId).addClass("required");
            return false;
        }
        else {
            $('#err_' + eId).html("");
            $('#' + eId).removeClass("required");
            return true;
        }
    }
    else {
        return true;
    }
}