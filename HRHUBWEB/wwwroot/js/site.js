function numberToWords(amount) {
    var singleDigits = [
        "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
        "seventeen", "eighteen", "nineteen"
    ];

    var tensDigits = [
        "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
    ];

    var scales = ["", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion"];

    if (amount === 0) {
        return "zero";
    }

    var words = [];

    function convertThreeDigit(number) {
        var word = "";
        var hundreds = Math.floor(number / 100);
        var tens = Math.floor((number % 100) / 10);
        var units = number % 10;

        if (hundreds !== 0) {
            word += singleDigits[hundreds] + " hundred ";
        }

        if (tens >= 2) {
            word += tensDigits[tens] + " ";
            word += singleDigits[units];
        } else if (tens === 1) {
            word += singleDigits[tens * 10 + units];
        } else if (units !== 0) {
            word += singleDigits[units];
        }

        return word.trim();
    }

    var scaleIndex = 0;

    while (amount > 0) {
        var threeDigit = amount % 1000;
        if (threeDigit !== 0) {
            var scaleWord = scales[scaleIndex];
            var digitWord = convertThreeDigit(threeDigit);
            var formattedWord = digitWord + " " + scaleWord;
            words.unshift(formattedWord.trim());
        }
        amount = Math.floor(amount / 1000);
        scaleIndex++;
    }

    return words.join(" ");
}


function makeAjaxRequest(url, method, data, async,  successCallback, errorCallback) {
    $.ajax({
        url: url,
        method: method,
        data: data,
        dataType: 'json',
        async: async,
        contentType: 'application/json',
        success: function (response) {
            if (successCallback && typeof successCallback === 'function') {
                successCallback(response);
            }
        },
        error: function (xhr, status, error) {
            if (errorCallback && typeof errorCallback === 'function') {
                errorCallback(xhr.status);
            }
        }
    });
}

// Usage example:
//var apiUrl = 'https://example.com/api';
//var requestData = { key: 'value' };

//makeAjaxRequest(apiUrl, 'POST', JSON.stringify(requestData), false, function (response) {
//    console.log('Success:', response);
//}, function (errorStatus) {
//    console.log('Error:', errorStatus);
//});


$('.number').keypress(function (event) {
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

//________ Datepicker
$(".fc-datepicker").datepicker({
    dateFormat: "dd-M-yy",
    monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"]
});

//________ Countdonwtimer
$("#clocktimer").countdowntimer({
    currentTime: true,
    size: "md",
    borderColor: "transparent",
    backgroundColor: "transparent",
    fontColor: "#313e6a",
    // timeZone : "+1"
});

//________ Countdonwtimer
$("#clocktimer2").countdowntimer({
    currentTime: true,
    size: "md",
    borderColor: "transparent",
    backgroundColor: "transparent",
    fontColor: "#313e6a",
    // timeZone : "+1"
});

//________ Datepicker
$('.fc-datepicker').datepicker('setDate', 'today');






function changePassword() {


    if ($("#NewPassword").val() != $("#ConfirmPassword").val()) {
        notif({
            msg: "<i class='fa fa-check fs-20 me-2'></i><b>Warning: </b>Password confirm not matach .",
            type: "warning"
        });
        return false
    }


    if ($("#NewPassword").val() == $("#ConfirmPassword").val()) {

        const apiUrl = "/User/passwordchange"; // Replace with your MVC controller action URL
        const requestData = {

            Password: $("#ConfirmPassword").val(),
            OldPasword: $("#CurrentPassword").val()
        };

        //	var Data = { id: $("#DesignationId").val(), title: $('#Title').val() == '' ? '""' : $('#Title').val() }
        //console.log(Data)
        $.ajax({
            async: false,
            url: "/User/passwordchange",
            type: "POST",
            data: requestData,
            success: function (data) {
                console.log(data)
                if (data != null) {

                    if (data.success) {

                        notif({
                            msg: "<i class='fa fa-check fs-20 me-2'></i><b>Success: </b> Your password Updated Succesfully.",
                            type: "success"
                        });
                    }
                    else {

                        notif({
                            msg: "<i class='fa fa-check fs-20 me-2'></i> <b>Warning: </b> " + data.message,
                            type: "warning"
                        });
                    }
                }
                else {
                    notif({
                        msg: "<i class='fa fa-check fs-20 me-2'></i><b>Error:</b> Password updated fail. ",
                        type: "error"
                    });

                }
            },
            error: function () {
                status = false;
                //   toastr.error("Please Fill Required Field");
            }
        })

    }
}