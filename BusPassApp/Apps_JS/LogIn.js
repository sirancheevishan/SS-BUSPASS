
function Showloginpop() {
    $('#LoginModal').modal({ backdrop: 'static', keyboard: false })
    return false;
}
$(document).ready(function () {
    $('input[type=text]').attr('autocomplete', 'off');
})

$(".clsLogin").keypress(function () {
    $("#spnErrMsg").html("");
    $("#dverrmsg").hide();
});

function adminLogin() {
    
    $(".dvclslgnerrormsg").html("").hide();
    $(".clsLogin").keypress(function () {
        $("#spnErrMsg").html("");
        $("#dverrmsg").hide();
    });
    $("#dverrmsg").hide(); //For Errmsg
    $("#spnErrMsg").html(""); //For Errmsg
    var reg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if ($("#txtlgnmailid").val() == "") {
        $("#dverrmsg").show();
        $("#spnErrMsg").html("Please enter mail id");
        return false;
    }
    if ($("#txtlgnpassword").val() == "") {
        $("#dverrmsg").show();
        $("#spnErrMsg").html("Please enter password");
        return false;
    }

    if (reg.test($("#txtlgnmailid").val()) == false) {
        $("#dverrmsg").show();
        $("#spnErrMsg").html("Enter valid Mail ID");
        return false;
    }
    var param = {
        strAdminMail: $("#txtlgnmailid").val(),
        strPassword: $("#txtlgnpassword").val(),
        LoginFlag: "F"

    }


    $("#dvspin").show();
    
    $.ajax({
        
        type: "POST",
        url: strAdminLoginpath,
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        
        success: function (data) {
            
            $("#dvspin").hide();
            if (data.Status == "00") {
             
              
                
                window.location.replace("https://localhost:44398/?Ad_MailId=" + param.strAdminMail +  "&Ad_Password=" + param.strPassword + "");
            }
            else{
                var msg = data.Message != null && data.Message != "" ? data.Message : "unable to login. Please try again later";
                $("#dverrmsg").show();
                $("#spnErrMsg").html(data.Message);
                $(".dvclslgnerrormsg").html(data.Message).show();
                $("#txtlgnmailid").focus();
                return false;
               }

        },
        error: function (ex) {
            $("#dvspin").hide();
            $("#dverrmsg").show();
            $("#spnErrMsg").html("Unable to connect remote server.");
            $(".dvclslgnerrormsg").html("Unable to connect remote server.").show();
        }
    });
}

function login() {

    if ($("#txtlgnmailid").val().indexOf("@bps.com") != -1) {
        adminLogin();
        return false;           
    }
    
    $(".dvclslgnerrormsg").html("").hide();
    $(".clsLogin").keypress(function () {
        $("#spnErrMsg").html("");
        $("#dverrmsg").hide();
    });
    $("#dverrmsg").hide(); //For Errmsg
    $("#spnErrMsg").html(""); //For Errmsg
    var reg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if ($("#txtlgnmailid").val() == "") {
        $("#dverrmsg").show();
        $("#spnErrMsg").html("Please enter mail id");
        return false;
    }
    if ($("#txtlgnpassword").val() == "") {
        $("#dverrmsg").show();
        $("#spnErrMsg").html("Please enter password");
        return false;
    }

    if (reg.test($("#txtlgnmailid").val()) == false) {
        $("#dverrmsg").show();
        $("#spnErrMsg").html("Enter valid Mail ID");
        return false;
    }
    var param = {
        strUserMail: $("#txtlgnmailid").val(),
        strPassword: $("#txtlgnpassword").val(),
        struserDetails: "",
        LoginFlag: "M",
        strMobileNo: "",
    }
    
    $("#loginSpin").show();

    $.ajax({
        type: "POST",
        url: strLoginpath,
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#loginSpin").hide();
            if (data.Status == "00") {
                if (data.Result == "Y") {
                    window.location.href = RedirectToDashboard;
                }
                else {
                    window.location.href = RedirectToHomePath;
                }

            }
            else if (data.Status == "02") {
                $(".clsverifypopup").attr("data-otpdetails", data.OTPDet);
                $('#LoginModal').modal('hide');
                $('#VerifyModal').modal('show');
            }
            else {
                var msg = data.Message != null && data.Message != "" ? data.Message : "unable to login. Please try again later";
                $("#dverrmsg").show();
                $("#spnErrMsg").html(data.Message);
                $(".dvclslgnerrormsg").html(data.Message).show();
                $("#txtlgnmailid").focus();
                return false;
                //showerralert(data.Message, "", '');
            }

        },
        error: function (ex) {
            $("#loginSpin").hide();
            $("#dverrmsg").show();
            $("#spnErrMsg").html("Unable to connect remote server.");
            $(".dvclslgnerrormsg").html("Unable to connect remote server.").show();
        }
    });
}
function verifyAccount(that) {
    var details = $(that).data("otpdetails");
    var OTPDetails = details.split("~");

    var param = {
        strName: OTPDetails[0],
        strRegisteredNo: OTPDetails[1],
        strMbl: OTPDetails[2],
        strMail: OTPDetails[3]
    }

    $("#clicktoVerifySpin").show();
    $.ajax({
        type: "POST",
        url: strOTPURL,
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#clicktoVerifySpin").hide();
            if (data.result == true && data.Status == "00") {
                $("#VerifyModal").modal("hide");
                $("#spnEmailNo").html(OTPDetails[2]);
                $("#spnMblNo").html(OTPDetails[3]);
                $('#OTPVerficationPopup').modal('show');
            }
            else {
                $("#VerifyModal").modal("hide");
                swal({
                    text: data.Message,
                    icon: "error",
                    button: "Close",
                    dangerMode: true,
                });

            }
        },
        error: function (ex) {
            $("#clicktoVerifySpin").hide();
            $("#VerifyModal").modal("hide");
            swal({
                text: "Unable to connect remote server.",
                icon: "error",
                button: "Close",
                dangerMode: true,
            });
        }
    });

}

function closeLoginPopup(id) {
    $("#dvloginpopup").hide();
    $('#' + id).modal('hide');
}


function VerifyOTP() {
    $("#dvOTPErr").hide();
    $("#spnErrMSG").html("");

    var SMSOTP = "";
    var MailOTP = "";
    var OTPCheck = false;
    $(".clssmsotp").each(function () {
        if ($(this).val() != "")
            SMSOTP += $(this).val();
        else
            OTPCheck = true;
    });
    if (OTPCheck == true) {
        $("#dvOTPErr").show();
        $("#spnErrMSG").html("Please enter OTP");
        return false;
    }
    $(".clsMailotp").each(function () {
        if ($(this).val() != "")
            MailOTP += $(this).val();
        else
            OTPCheck = true;
    });
    if (OTPCheck == true) {
        $("#dvOTPErr").show();
        $("#spnErrMSG").html("Please enter OTP");
        return false;
    }

    var OTPParam = {
        strSMSOTP: SMSOTP,
        strMailOTP: MailOTP,
        strMobileNo: "",
        strMailID: "",
    };
    $("#VerifySpin").show();
    $.ajax({
        url: Verify_RegistrationURL,
        type: "POST",
        data: JSON.stringify(OTPParam),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#VerifySpin").hide();
            if (data.status == "00") {
                location.replace(RedirectURL);
            }
            else {
                $("#dvOTPErr").show();
                $("#spnErrMSG").html(data.Message);
            }
        },
        error: function (err) {
            $("#VerifySpin").hide();
            $("#dvOTPErr").show();
            $("#spnErrMSG").html("Unable to connect remote server");
        }
    });
}



function getCodeBoxElement(index, key) {
    if (key == "S")
        return document.getElementById('SMScodeBox' + index);
    else if (key == 'M')
        return document.getElementById('MailcodeBox' + index);

}
function onKeyUpEvent(index, event, key) {
    const eventCode = event.which || event.keyCode;
    if (getCodeBoxElement(index, key).value.length === 1) {
        if (index !== 5) {
            getCodeBoxElement((index + 1), key).focus();
        } else {
            getCodeBoxElement(index, key).blur();
            // Submit code
            console.log('submit code ');
        }
    }
    if (eventCode === 8 && index !== 1) {
        getCodeBoxElement((index - 1), key).focus();
    }
}
function onFocusEvent(index, key) {
    for (item = 1; item < index; item++) {
        const currentElement = getCodeBoxElement(item, key);
        if (!currentElement.value) {
            currentElement.focus();
            break;
        }
    }
}

function redirectToHome() {
    window.location.hre = HomeURL;
}




function textToPass(id, that) {
    if ($("#" + id).prop("type") == "text") {
        $("#" + id).attr("type", "password");
        $(that).removeClass("fa fa-eye-slash");
        $(that).addClass("fa fa-fw fa-eye");
    }
    else if ($("#" + id).prop("type") == "password") {
        $(that).removeClass("fa fa-fw fa-eye");
        $(that).addClass("fa fa-eye-slash");
        $("#" + id).attr("type", "text");
    }
}


function ShowPasspop(passcode) {
    $(".clsEligibleCriteria").hide();
    $("." + passcode).show();
    $('#PasPopUp').modal('show');

}


$(window).scroll(function () {

    var scrollTop = $(window).scrollTop();
    if ($(window).scrollTop() > 60) {
        $(".clsscroll").addClass("clsmintop")
    }
    else {
        $(".clsscroll").removeClass("clsmintop")
    }
});

$(".redirectohome").click(function () {
    window.location.href = HomeURL;
})



function getPassNameAndID(id, name) {
    if (id != "") {
        switch (id) {
            case "PBSS001":
                return "Student Pass (School)"
                break;
            case "PBSC001":
                return "Student Pass (College)"
                break;
            case "PBSZ001":
                return "Senior Citizens pass"
                break;
            case "PBHB001":
                return "Handicapped and Blind pass";
                break;

            case "PBJP001":
                return "Journalists / Press Reporter pass";
                break;
            case "PBTP001":
                return "Travel as you Please Tickets";
                break;
            case "PBMP001":
                return "Monthly Pass";
                break;
            case "PBFF001":
                return "Freedom fighters pass";
                break;

        }
    }
    if (name != "") {
        switch (id) {
            case "Student Pass (School)":
                return "PBSS001"
                break;
            case "Student Pass (College)":
                return "PBSC001"
                break;
            case "Senior Citizens pass":
                return "PBSZ001"
                break;
            case "Handicapped and Blind pass":
                return "PBHB001";
                break;

            case "Journalists / Press Reporter pass":
                return "PBJP001";
                break;
            case "Travel as you Please Tickets":
                return "PBTP001";
                break;
            case "Monthly Pass":
                return "PBMP001";
                break;
            case "Freedom fighters pass":
                return "PBFF001";
                break;

        }
    }

}



