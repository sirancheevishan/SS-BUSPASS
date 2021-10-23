function GetPassdata() {
    $.blockUI({
        message: '<img alt="Please Wait..." src="' + loaderimgurl + '" style="background-color:#fff;width: 4%;border-radius:8px; margin-top:12%;" id="FareRuleLodingImage" />',
        css: { padding: '5px' }
    });
    $.ajax({
        type: "POST",
        url: GetPassDetails,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            debugger
            $.unblockUI();
            if (data.status == "01") {
                formRenewalTable(JSON.parse(data.Result));
            }
            else {
                $("#dvRenewalDetails").html('<img src="~/Images/no-record.jpg" />');
                swal({
                    text: data.Message,
                    icon: "error",
                    button: "Close",
                    dangerMode: true,
                });
            }
        },
        error: function (ex) {
            $.unblockUI();
        }
    });
};


function formRenewalTable(data) {
    data = data.Temp;
    var RenewalBuilder = '<table id="RenewalTable">';
    RenewalBuilder += '<tr>';
    RenewalBuilder += '<th>Pass Number</th>';
    RenewalBuilder += '<th>Pass Code</th>';
    RenewalBuilder += '<th>Pass Name</th>';
    RenewalBuilder += '<th>Amount</th>';
    RenewalBuilder += '<th>Expiry Date</th>';
    RenewalBuilder += '<th>Status</th>';
    RenewalBuilder += '<th>Action</th>';
    RenewalBuilder += '</tr>';
    $.each(data, function (i, val) {
        RenewalBuilder += '<tr>';
        RenewalBuilder += '<td>' + val.PI_PASS_NUMBER + '</td>';
        RenewalBuilder += '<td>' + val.PI_PASS_CODE + '</td>';
        RenewalBuilder += '<td>' + val.PI_PASS_NAME + '</td>';
        RenewalBuilder += '<td>' + val.PI_AMOUNT + '</td>';
        RenewalBuilder += '<td>' + val.PI_EFFECTIVE_TO + '</td>';
        var date = val.PI_EFFECTIVE_TO.split('/');
        var statusSpan = new Date(date[2], date[1], date[0]) > new Date() ? '<span class="spnActive">Active</span>' : '<span class="spnCanel">Expired</span>';
        RenewalBuilder += '<td>' + statusSpan + '</td>';
        RenewalBuilder += '<td><span class="spnProceed" data-passcode="' + val.PI_PASS_CODE + '" data-passno="' + val.PI_PASS_NUMBER + '" onclick="getSelectedPassDetails(this)">Proceed</span></td>';
        RenewalBuilder += '</tr>';
    });
    RenewalBuilder += "</table>";
    if (RenewalBuilder != "") {
        $("#dvRenewalDetails").html(RenewalBuilder);
    }
    $('#RenewalModal').modal({ backdrop: 'static', keyboard: false })
}

function getSelectedPassDetails(e) {
    var PassID = $(e).data("passcode");
    var PassNo = $(e).data("passno");
    var param = {
        strPassNumber: PassNo
    }
    $.ajax({
        type: "POST",
        url: GetSlctdPassDetails,
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            debugger
            if (data.status == "01") {
                window.location.href = BookingURL + '?type=' + PassID;
                
            }
            else {
                swal({
                    text: data.Message,
                    icon: "error",
                    button: "Close",
                    dangerMode: true,
                });
            }
        },
        error: function (ex) {

        }
    });
}
