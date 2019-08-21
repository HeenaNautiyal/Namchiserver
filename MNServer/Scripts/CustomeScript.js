//Date of Death
$(function () {
    var dtToday = new Date();
    var month = ("0" + dtToday.getMonth() + 1).slice(-2);
    var day = ("0" + dtToday.getDate()).slice(-2);
    var year = dtToday.getFullYear();

    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    var maxDate = year + '-' + month + '-' + day;
    $('#Date_of_Birth').attr('max', maxDate);
});

//Death Birth
$(function () {
    //debugger
    var dtToday = new Date();

    var month = ("0" + dtToday.getMonth() + 1).slice(-2);
    var day = ("0" + dtToday.getDate()).slice(-2);
    var year = dtToday.getFullYear();

    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    var maxDate = year + '-' + month + '-' + day;
    $('#Date_of_Death').attr('max', maxDate);
});

$(function () {
    $("#btnShowModal").click(function () {

        $("#loginModal").show("slow");
        $("#btnShowModal").hide();
        //$.ajax({
        //    type: "GET",
        //    url: "/Dashboard/PopUp",
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "html",
        //    success: function (response) {
        //        if (response != null) {
                     
        //        }
        //        else {
        //            // alert("Something went wrong");
        //        }
        //    },
        //    failure: function (response) {
        //        alert(response.responseText);
        //    },
        //    error: function (response) {
        //        alert(response.responseText);
        //    }
        //});












    });
})

$(function () {
    $("#btnReset").click(function () {

        $("#btnShowModal").show("slow");
        $("#loginModal").hide();
      
    });
})

//CheckBox and their selection toggel
$(document).ready(function () {
    $('input[type="checkbox"]').change(function () {
        //debugger
        var value = $(this).attr('value');
        var Checked = $(this).prop('checked');
        var someVarName = 0;

        if (value == "Verify" && Checked == true) {
            $('input[type="checkbox"]').not(this).prop('checked', false);
            document.getElementById("btnPost").disabled = false;
            document.getElementById("btnReject").disabled = true;
            document.getElementById("btnClarify").disabled = true;
            someVarName = value;
            localStorage.setItem("someVarKey", someVarName);
        }
        else {
            var someVarName = localStorage.getItem("someVarKey");
            if (someVarName == "Verify" && value != "Verify") {
                $('input[type="checkbox"]').not(this).prop('checked', false);
                document.getElementById("btnPost").disabled = true;
                document.getElementById("btnReject").disabled = false;
                document.getElementById("btnClarify").disabled = false;
                someVarName = value;
                localStorage.setItem("someVarKey", someVarName);
            }
            else {
                document.getElementById("btnPost").disabled = true;
                document.getElementById("btnReject").disabled = false;
                document.getElementById("btnClarify").disabled = false;
                someVarName = value;
                localStorage.setItem("someVarKey", someVarName);
            }
        }
    
    });
 });
       
window.onload = function () {
    document.getElementById("loginModal").style.display = 'none';
}

//For Approve Action
$(function () {
    $("#btnPost").click(function () {
        // debugger
        var selected = "Approve";
        var Commentsection = $("#Comments").val();
        var Role = new Object();
        Role.Action = selected;
        Role.Comments = Commentsection

        if (Role != null) {
            $.ajax({
                type: "POST",
                url: "/Dashboard/AjaxPostCall",
                data: JSON.stringify(Role),
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                success: function (response) {
                    if (response != null) {
                        alert("Your Application has been approve");
                        window.location = "/Dashboard/Dashboard";
                    }
                    else {
                        // alert("Something went wrong");
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        }

    });
})

//For Rejection Action
$(function () {
    $("#btnReject").click(function () {
        var Reject = [];
      
        var Commentsection = $("#Comments").val();
        $.each($('input[type = "checkbox"]:checked'), function () {
             Reject.push($(this).val());
        });
        if (Reject.length <= 0) {
            alert("Please select at least one option")
        }
        else if (Commentsection == null) { alert(" Please enter your comments in the space provided below:") }

        else {
        var RejectRecommend = new Object();
            RejectRecommend.Action = Reject.toString();
            RejectRecommend.Comments = Commentsection;
            RejectRecommend.ActionSection = "RecommendRejection";
       
        if (RejectRecommend != null) {
                $.ajax({
                    type: "POST",
                    url: "/Dashboard/AjaxRecomendRejectCall",
                    data: JSON.stringify(RejectRecommend),
                    contentType: "application/json; charset=utf-8",
                    dataType: "html",
                    success: function (response) {
                        if (response != null) {
                            alert("Your Application has been processed");
                            window.location = "/Dashboard/Dashboard";
                        }
                        else {
                            // alert("Something went wrong");
                        }
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    },
                    error: function (response) {
                        alert(response.responseText);
                    }
                });
            } 
        }
    });
})

//For Clarification Action
$(function () {
    $("#btnClarify").click(function () {

        var Reject = [];

        var Commentsection = $("#Comments").val();
        $.each($('input[type = "checkbox"]:checked'), function () {
            Reject.push($(this).val());
        });
        if (Reject.length <= 0) { alert("Please select at least one option") }

        else if (Commentsection == null) { alert(" Please enter your comments in the space provided below:") }

        else {
            var RejectRecommend = new Object();
            RejectRecommend.Action = Reject.toString();
            RejectRecommend.Comments = Commentsection;
            RejectRecommend.ActionSection = "Clarification";

            if (RejectRecommend != null) {
                $.ajax({
                    type: "POST",
                    url: "/Dashboard/AjaxRecomendRejectCall",
                    data: JSON.stringify(RejectRecommend),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response != null) {
                            alert("Your Application has been processed");
                            window.location = "/Dashboard/Dashboard";
                        }
                        else { }
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    },
                    error: function (response) {
                        alert(response.responseText);
                    }
                });
            }
        }
    });
})


$(function () {
    $("#btnShowModal").click(function () {

        $("#loginModal").show("slow");
        //$.ajax({
        //    type: "GET",
        //    url: "/Dashboard/PopUp",
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "html",
        //    success: function (response) {
        //        if (response != null) {

        //        }
        //        else {
        //            // alert("Something went wrong");
        //        }
        //    },
        //    failure: function (response) {
        //        alert(response.responseText);
        //    },
        //    error: function (response) {
        //        alert(response.responseText);
        //    }
        //});












    });
})

  ////For Display Image
  //      $(function () {
  //          $('#previewDocumentDialog').dialog({
  //              autoOpen: false,
  //              resizable: false,
  //              title: 'Preview',
  //              modal: true,
  //              width: 400,
  //              height: 500,
  //              open: function (event, ui) {
  //                  //Load the PreviewWordDocument action which will return a partial view called _PreviewWordDocument
  //                  $(this).load("@Url.Action("PreviewWordDocument")"); //add parameters if necessary
  //              },
  //              buttons: {
  //                  "Close": function () {
  //                      $(this).dialog("close");
  //                  }
  //              }
  //          });

  //          // an event click for you preview link to open the dialog
  //          $('#previewLink').click(function () {
  //              $('#previewDocumentDialog').dialog('open');
  //          });
  //      });
