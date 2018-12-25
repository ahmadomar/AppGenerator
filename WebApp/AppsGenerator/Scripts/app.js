$(document).ready(function () {
    $("#btnCheck").click(function () {

        var dt = { db_datasource: $("#db_datasource").val(), db_name: $("#db_name").val(), db_user_id: $("#db_user_id").val(), db_password: $("#db_password").val() };
        var options = {
            type: "POST",
            url: "/Application/CheckConnection",
            data: dt
        };
        $.ajax(options)
           .success(function (data) {
               if (data == "True")
                   bootbox.alert("Test connection succeeded.");
               else
                   bootbox.alert("test connection Error.");
           })
           .error(function (data) {
               bootbox.alert("An error");
           });
    });


    $("#btnCheckUrl").click(function () {

        var dt = { url: $("#url").val()};
        var options = {
            type: "POST",
            url: "/Application/CheckWebsiteUrl",
            data: dt
        };
        $.ajax(options)
       
           .success(function (data) {
               if (data == "True")
                   bootbox.alert("The website name is available.");
               else
                   bootbox.alert("The website name is not available.");
           })
           .error(function (data) {
               bootbox.alert("An error");
           });
    });
    
    $(".btnStartStop").click(function () {

        var appId = $(this).attr('data-app-id');
        var isRunning = $(this).attr('data-app-running');

        var dt = { id: appId };
        var options = {
            type: "POST",
            url: "/Application/StartStopApp",
            data: dt
        };
        $.ajax(options)

           .success(function (data) {
               if (data == "True")
                   $(this).attr('class', 'btn btn-danger btnStartStop').attr('value','Stop');
               else
                   $(this).attr('class', 'btn btn-success btnStartStop').attr('value', 'Start');
           })
           .error(function (data) {
               bootbox.alert("An error");
           });
    });



    $(".btnRestart").click(function () {

        var appId = $(this).attr('data-app-id');

        var dt = { id: appId };
        var options = {
            type: "POST",
            url: "/Application/RestartApp",
            data: dt
        };
        $.ajax(options)

           .success(function (data) {
               bootbox.alert("Application Restarted Successfully.");
           })
           .error(function (data) {
               bootbox.alert("An error");
           });
    });

    $(document).bind("ajaxSend", function () {
            $("").loading();
            
    }).bind("ajaxComplete",function(){
        $("").clearLoading();
    });
});