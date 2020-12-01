<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TBMHMIWebApplication.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta charset="utf-8" />
        <title>TBM Application Dashboard</title>
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="stylesheet" href=".//assets/css/bootstrap.css" />
        <link rel="stylesheet" href=".//assets/css/style.css" />
        <script src=".//Scripts/jquery-1.10.2.min.js"></script>
        <script src=".//Scripts/bootstrap.min.js"></script>
        <script src="Scripts/ManualLib.js"></script>
        <script src="virtualkeyboard/jquery.numpad.js"></script>
        <script src="Scripts/highcharts.js"></script>
        <style>
         
			.nmpd-wrapper {display: none;}
			.nmpd-target {cursor: pointer;}
			.nmpd-grid {position:absolute; left:50px; top:50px; z-index:5000; -khtml-user-select: none; padding:10px; width: 44%;}
			.nmpd-overlay {z-index:4999;}
			input.nmpd-display {text-align: left;font-size:20px;}

            .nmpd-grid .btn
            {
                padding: 22px 20px !important;
            }

            .nmpd-grid .numero
            {
                padding: 17px 46px !important;
                font-size: 22px !important;
            }

            .OEMImagesDiv img {
                max-width: 100%;
                max-height: 100%;
            }

            .OEMImagesDiv
            {
                height: 60px;
                width: 95px;
            }

            .hide
            {
                display:none !important;
                 /*display:none !important;*/
            }

 
		</style>
		<script type="text/javascript">
            // Set NumPad defaults for jQuery mobile. 
            // These defaults will be applied to all NumPads within this document!
            $.fn.numpad.defaults.gridTpl = '<table class="table modal-content"></table>';
            $.fn.numpad.defaults.backgroundTpl = '<div class="modal-backdrop in"></div>';
            $.fn.numpad.defaults.displayTpl = '<input type="text" class="form-control" />';
            $.fn.numpad.defaults.buttonNumberTpl = '<button type="button" class="btn btn-default btn-lg"></button>';
            $.fn.numpad.defaults.buttonFunctionTpl = '<button type="button" class="btn" style="width: 100%;"></button>';
            $.fn.numpad.defaults.onKeypadCreate = function () { $(this).find('.done').addClass('btn-primary'); };

            
            // Instantiate NumPad once the page is ready to be shown
            $(document).ready(function () {
                $('#txtusername').numpad();
                $('#login-password, #txtpassword,#currentPassword, #newPassword, #confirmPassword').numpad({
                    displayTpl: '<input class="form-control" type="password" />',
                    hidePlusMinusButton: true,
                    hideDecimalButton: true
                });

                $("#changeRecipeBtn").click(function ()
                {
                    flag = 1;
                    
                    LoadReceipe();
                    setTimeout(function () {
                        $("#recipeModal").modal({ backdrop: 'static', keyboard: false });
                    },0);
                    
                });

                $("#dpdReceipeName").change(function () {
                    if ($("#dpdReceipeName option:selected").text() == "Other") {
                        $("#newRecipeBox").removeClass("hide");
                    }
                    else {
                        $("#newRecipeBox").addClass("hide");
                    }
                });
                //$('#numpadButton-btn').numpad({
                //    target: $('#numpadButton')
                //});
                //$('#numpad4div').numpad();
                //$('#numpad4column .qtyInput').numpad();

                //$('#numpad4column tr').on('click', function (e) {
                //    $(this).find('.qtyInput').numpad('open');
                //});
            });
		</script>
		<style type="text/css">
			.nmpd-grid {border: none; padding: 20px;}
			.nmpd-grid>tbody>tr>td {border: none;}
			
			/* Some custom styling for Bootstrap */
			.qtyInput {display: block;
			  width: 100%;
			  padding: 6px 12px;
			  color: #555;
			  background-color: white;
			  border: 1px solid #ccc;
			  border-radius: 4px;
			  -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
			  box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
			  -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
			  -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
			  transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
			}
		</style>

          <%--<link href="virtualkeyboard/jquery-ui.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="jquery.min.js" type="text/javascript"></script>--%>
 <%--   <script src="jquery-ui.min.js" type="text/javascript"></script>
    <link href="virtualkeyboard/keyboard.css" rel="stylesheet" type="text/css" />
    <script src="virtualkeyboard/jquery.keyboard.js" type="text/javascript"></script>
    <script src="virtualkeyboard/jquery.keyboard.extension-typing.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            user();
            password();
            password1();
        });

        function user() {
            $('#txtusername').keyboard({

                preventPaste: true,

                autoAccept: true
            })
                .addTyping();


        }
        function password() {
            $('#txtpassword').keyboard({
                preventPaste: true,
                autoAccept: true
            })
                .addTyping();

        }

        function password1() {
            $('#login-password').keyboard({
                preventPaste: true,
                autoAccept: true
            })
                .addTyping();

        }

        
    </script>--%>

  

<script type = "text/javascript">
    "use strict";

    var modal = 0;
    var ReceipeName = '';
    var flag = 0;
    var flag1 = 0;
    var flagDowntime = 0;
    var flagDowntimestartfrom = 0;

    //UpdateOktag();
    LoadLastTyre();

    setTimeout(function () {
        var timmer = setInterval(UpdateOktag, 1000);
    },1000)

    
    function LoadReceipe() {

        $("#dpdReceipeName").empty();

        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/LoadReceipe",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                Result = Result.d;
                $.each(Result, function (key, value) {
                    $("#dpdReceipeName").append($("<option></option>").val
                        (value.id).html(value.Name));
                });
                $("#dpdReceipeName").append($("<option></option>").val
                    ('0').append('Other'));
            },
            error: function (Result) {
                console.log('err:' + Result);
            }
        });
    }

    function LoadReasion() {

        $("#dpdResion").empty();

        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/LoadReasion",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                Result = Result.d;
                $.each(Result, function (key, value) {
                    $("#dpdResion").append($("<option></option>").val
                        (value.id).html(value.Name));
                });
            },
            error: function (Result) {
                console.log('err:' + Result);
            }
        });
    }

    function AddNewReceipe() {


        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/AddReceipe",
            data: "{ReceipeName: '" + $("#TxtReceipeName").val() + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                var data = JSON.parse(Result.d);
                if (data == 'OK') {
                   
                   // $("#lblmsg").text($("#TxtReceipeName").val() + ' : Receipe Add Sucessfully!');
                    $("#TxtReceipeName").val('');
                    LoadReceipe();
                }
            },
            error: function (Result) {
                console.log('err:' + Result);
            }
        });
    }

    function UpdateReceipeTag() {


        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/UpdateTag",
            data: "{ReceipeName: '" + $("#dpdReceipeName option:selected").text() + "',Wcid: '" + $('select[id=dpdMachineName]').val() + "',WcName: '" + $("#dpdMachineName option:selected").text() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {

                var data = JSON.parse(Result.d);
                if (data == 'Update') {
                    $("#recipeMsgDiv").html("<span class='text-success'>Save Successfully.</span>");
                }
                else {
                  //  alert(data)
                    if (data == 'Logout')
                    {
                        //location.reload();
                        location.href = document.URL;
                    }

                    $("#recipeMsgDiv").html("<span class='text-danger'>Not Saved.</span>");
                }

            },
            error: function (Result) {
                console.log('err:' + Result);
            }
        });
    }
      
    function closeDownTime()
    {
       // $("#downtimeModal").modal("hide");
        SaveDownTimeOK();
        flagDowntime = 0;
        flag1 = 0;
        flag = 0;
        $("#downtimeModal").modal("hide");
    }

    function UpdateOktag() {
        flagDowntime = flagDowntime + 1;
        //console.log(typeof flagDowntime, typeof flagDowntimestartfrom);
        //console.log(flagDowntime, flagDowntimestartfrom);
           
            if (flagDowntime > flagDowntimestartfrom)
            {
               

                flag = 1;
                if (flag1 == 0)
                {
                    flag1 = 1;
                      
                    SaveDownTime();
                }
                var seconds = flagDowntime - flagDowntimestartfrom;
                var days = Math.floor(seconds / (3600 * 24));
                seconds -= days * 3600 * 24;
                var hrs = Math.floor(seconds / 3600);
                seconds -= hrs * 3600;
                var mnts = Math.floor(seconds / 60);
                seconds -= mnts * 60;

               
                
                $("#lbldowninsec").html('<span class="">&nbsp; ' + days + " days, " + hrs + " Hrs, " + mnts + " Minutes, " + seconds + " Seconds" + ' </span>');
                $("#downtimeModal").modal({ backdrop: 'static', keyboard: false });

                
            }

        

            if (flag == 0) {
                flag = 1;
            
                $.ajax({
                    type: "POST",
                    url: "TbmActionPerform.asmx/GetTime",
                    data: "{ HmiId:'" + $("#lblwcname").text() + "'  , Barcode: '" + $("#txtbarcode").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (Result) {
                        
                        var data = JSON.parse(Result.d);
                        var operator1 = '<%:Session["OP1"]%>';
                        var operator2 = '<%:Session["OP2"]%>';
                        var operator3 = '<%:Session["OP3"]%>';

                        var ErrorMessage = data[0]['ErrorMessageTBM'];
                        var AutoLogout = data[0]['AutoLogoutTBM'];
                        if (AutoLogout == 'AU1000') {
                            //location.reload();
                            location.href = document.URL;
                        }
                        
                        
                        if (ErrorMessage == '001') {

                            var CyclestartStopTBM = data[0]['CyclestartStopTBM'];
                            var ReceipeNameTBM = data[0]['ReceipeNameTBM'];
                            var ReceipeImageTBM = data[0]['ReceipeImageTBM'];
                            var okTBM = data[0]['okTBM'];
                            var nokTBM = data[0]['nokTBM'];
                            var InterlockTBM = data[0]['InterlockTBM'];
                            var messageTBM = data[0]['messageTBM'];
                            var Dission = data[0]['Dission'];
                            var ScanningAllow = data[0]['ScanningAllow'];
                            var Datetimenow = data[0]['Datetimenow'];
                            var TBMcyclestart = data[0]['TBMcyclestart'];
                            var sapcode = data[0]['sapcode'];
                            

                            

                            $("#lblTime").html('<span class="">&nbsp; ' + Datetimenow+' </span>');
                           
                            if (InterlockTBM == '1' || InterlockTBM == 'True') {
                                document.getElementById('ImgInterlock').src = "Images/ok.jpg";
                            }
                            else
                            {
                                document.getElementById('ImgInterlock').src = "Images/nok.png";
                            }
                            if (CyclestartStopTBM == '1' || CyclestartStopTBM == 'True') {
                                document.getElementById('imgCyclestart').src = "Images/ok.jpg";
                            }
                            else
                            {
                                document.getElementById('imgCyclestart').src = "Images/nok.png";
                            }

                            var txtreceipename = $("#lblraceipe").text();

                            if (ReceipeNameTBM != txtreceipename)
                            {
                                if (txtreceipename == '') {

                                    ReceipeName = txtreceipename;
                                    $("#lblraceipe").text(ReceipeNameTBM);
                                    $("#lblsapcode").text(sapcode);
                                    
                                   // $("#imgreceipe").attr('src', 'Logo/logo.png');
                                }
                                if (ReceipeImageTBM != '') {
                                    $("#lblraceipe").text(ReceipeNameTBM);
                                    $("#lblsapcode").text(sapcode);
                                   // $("#imgreceipe").attr('src', ReceipeImageTBM);

                                }
                            }
                            if (CyclestartStopTBM == "1" || CyclestartStopTBM == "True") {

                                var txtbarcodeval = $("#txtbarcode").val();
                                if (txtbarcodeval == '') {

                                    $("#lblerrormessage").html('<span class="text-success"><span class="glyphicon glyphicon-time"></span>&nbsp; Waiting For Cycle Stop </span>');
                                }
                                else {
                                    $("#lblerrormessage").html('<span class="text-success"><span class="glyphicon glyphicon-time"></span>&nbsp; Waiting For OK/NOK Input </span>');
                                }

                                $("#txtbarcode").prop("disabled", true);
                                buttonaddCss();
                              //  console.log('CyclestartStopTBM');

                            }
                            else {
                                if ((ScanningAllow == "0" || ScanningAllow == "False")) {
                                   
                                    
                                        $("#lblerrormessage").html('<span class="text-success"><span class="glyphicon glyphicon-time"></span>&nbsp; Waiting For Barcode Scanning </span>');

                                    
                                    $("#txtbarcode").prop("disabled", false);
                                    buttonRemoveCss();
                                    $("#btnnok,#btnok").addClass("btn-default");
                                    $("#txtbarcode").focus();
                                    //console.log('ScanningAllow');


                                }
                                else {
                                    // $("#lblerrormessage").html('<span class="text-success"><span class="glyphicon glyphicon-ok"></span>&nbsp; Wait for cycle Stop </span>');
                                    if (CyclestartStopTBM == "1" || CyclestartStopTBM == "True")
                                    {
                                        buttonaddCss();
                                       // console.log('CyclestartStopTBM1');
                                    }
                                    else {
                                         
                                            if (TBMcyclestart == '1')
                                        {
                                                buttonaddCss();
                                          //  console.log('TBMcyclestart1');
                                        }
                                        else if (TBMcyclestart == '2')
                                        {
                                                buttonaddCss();
                                           // console.log('TBMcyclestart2');
                                        }
                                        else
                                            {
                                                if (InterlockTBM == "1" || InterlockTBM == "True") {
                                                    $("#lblerrormessage").html('<span class="text-success"><span class="glyphicon glyphicon-time"></span>&nbsp; Waiting For Cycle Start </span>');
                                                }
                                                buttonRemoveCss();
                                           // console.log('TBMcyclestart3');
                                        }

                                    }

                                }
                            }

                            

                        
                            if (messageTBM == 'Save Successfully!') {
                                flagDowntime = 0;
                                getproductioncount();
                               
                                $("#lblmsg").html('<span class="text-success"><span class="glyphicon glyphicon-ok"></span>&nbsp;'+ $("#txtbarcode").val() + ' Save Successfully!</span>');
                               // textboxEnable();

                                
                            }
                            else if (messageTBM == 'Barcode AllReady Exists.') {
                                $("#lblmsg").html('<span class="text-danger"><span class="glyphicon glyphicon-remove"></span>&nbsp;' + $("#txtbarcode").val() + ' Barcode Already Exists!</span>');
                              
                                //textboxEnable();

                            }
                        }
                        document.getElementById('imgCommunication').src = "Images/green_bl.gif";
                         flag = 0;
                        //if (ErrorMessage == '401') {
                        //    flag = 0;
                        //}


                    },
                    error: function (xhr, status, error) {
                        document.getElementById('imgCommunication').src = "Images/red_bl.gif";
                       
                       
                        //var err = eval("(" + xhr.responseText + ")");
                        flag = 0;
                       // alert(xhr.responseText);
                       
                    }

                });
            }
        }
        
        function buttonRemoveCss()
        {
            $("#btnok,#btnnok").prop("disabled", true);
            $("#btnok").remove("btn-success");
            $("#btnnok").remove("btn-danger"); }

        function buttonaddCss()
        {
            $("#btnok,#btnnok").prop("disabled", false);
            $("#btnok").addClass("btn-success");
            $("#btnnok").addClass("btn-danger"); }
        function drawChart(result) {


            var data = JSON.parse(result.d);
            var x;
            var AToalcount = 0;
            var BToalcount = 0; var CToalcount = 0; var AOK = 0; var BOK = 0; var COK = 0; var ANOK = 0; var BNOK = 0; var CNOK = 0;
            for (x in data) {
                if (data[x].Shift == "A") {

                    AToalcount = AToalcount + parseInt(data[x].count);

                    if (data[x].status == "OK")
                        AOK = parseInt(data[x].count);
                    else
                        ANOK = parseInt(data[x].count);

                }
                if (data[x].Shift == "B") {
                    BToalcount = BToalcount + parseInt(data[x].count);
                    if (data[x].status == "OK")
                        BOK = parseInt(data[x].count);
                    else
                        BNOK = parseInt(data[x].count);

                }
                if (data[x].Shift == "C") {
                    CToalcount = CToalcount + parseInt(data[x].count);
                    if (data[x].status == "OK")
                        COK = parseInt(data[x].count);
                    else
                        CNOK = parseInt(data[x].count);

                }
               

            }

            Highcharts.setOptions({
                colors: ['#5bc0de', '#d9534f']
            });

            //console.log(dataArray);
            Highcharts.chart('containerA', {

               


                chart: {

                    type: 'pie'
                    
                },

                title: {
                   // text: 'SHIFT A'
                     verticalAlign: 'middle',
                    floating: true,
                    text: '<div style="font-size:40px;">' +AToalcount+'</div>'
                },


                plotOptions: {
                    pie: {
                      //  showInLegend: true,
                        innerSize: '60%',
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            color: 'white',
                            distance: -20,
                            enabled: true,
                            format: '{point.y:1f} ',
                            fontWeight: '',
                            crop: false,
                            overflow: 'none'
                        },
                       
                       
                        showInLegend: false

                    }
                },
                
                series: [{
                    name: 'Count',
                    colorByPoint: true,
                    data: [{
                        name: 'OK',
                        y: AOK
                    }, {
                        name: 'NOK',
                        y: ANOK,
                        sliced: false,
                        selected: true
                    }]
                }]


               

            });

            Highcharts.chart('containerB', {

                


                chart: {

                    type: 'pie'

                },

                title: {
                    // text: 'SHIFT A'
                    verticalAlign: 'middle',
                    floating: true,
                    text: '<div style="font-size:40px;">' + BToalcount+'</div>'
                },

                yAxis: {

                    min: 1,
                    title: {
                        text: 'Tyre In Numbers'
                    }
                },

                xAxis: {

                    categories: []
                },
                credits: {
                    enabled: false
                },

                plotOptions: {
                    pie: {
                      //  showInLegend: true,
                        innerSize: '60%',
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            color: 'white',
                            distance: -20,
                            enabled: true,
                            format: '{point.y:1f} ',
                           
                            crop: false,
                            overflow: 'none'

                        },
                    }
                },



                series: [{
                    name: 'Count',
                    colorByPoint: true,
                    data: [{
                        name: 'OK',
                        y: BOK
                    }, {
                        name: 'NOK',
                        y: BNOK,
                        sliced: false,
                        selected: true
                    }]
                }]

            });

            Highcharts.chart('containerC', {

               



                chart: {

                    type: 'pie',
                     
                },

                title: {
                    // text: 'SHIFT A'
                    verticalAlign: 'middle',
                    floating: true,
                    text: '<div style="font-size:40px;">' +CToalcount+'</div>'
                },

                yAxis: {

                    min: 1,
                    title: {
                        text: 'Tyre In Numbers'
                    }
                },

                xAxis: {

                    categories: ['Shift C']
                },
                credits: {
                    enabled: false
                },

                plotOptions: {
                    pie: {
                        //showInLegend: true,
                        innerSize: '60%',
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            color: 'white',
                            distance: -20,
                            enabled: true,
                            format: '{point.y:1f} ',
                            
                            crop: false,
                            overflow: 'none'

                        },
                    }
                },

               
                series: [{
                    name: 'Count',
                    colorByPoint: true,
                    data: [{
                        name: 'OK',
                        y: COK
                    }, {
                        name: 'NOK',
                        y: CNOK,
                        sliced: false,
                        selected: true
                    }]
                }]

            });
            
            $('.highcharts-text-outline').removeAttr("stroke");

        }

        function getproductioncount() {

            $.ajax({
                type: "POST",
                url: "TbmActionPerform.asmx/GetShiftWiseCount",
                data: "{ wcname:'" + $("#lblwcname").text() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (Result) {
                    drawChart(Result);
                    //console.log(Result);

                },
                error: function (Result) {
                   // console.log('err:' + Result);
                }
            });
        }
        function textboxDisable() {
            
            $("#txtbarcode").prop("disabled", true);

            buttonRemoveCss();

            $("#btnnok,#btnok").addClass("btn-default");
        }
        function textboxEnable() {
            $("#txtbarcode").val("");
            $("#txtbarcode").prop("disabled", false);

            buttonRemoveCss();

            $("#btnnok,#btnok").addClass("btn-default");
            $("#txtbarcode").focus();
        }
    function okfunction() {
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/okfunction",
            data: "{ HmiId: '" + $("#lblwcname").text() + "', Barcode: '" + $("#txtbarcode").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                var data = JSON.parse(Result.d);
                if (data == 'Nok') {

                    //$("#lblmsg").text('<span class="text-danger">Barcode Already Exists</span>');
                    //$("#msgModal").modal("show");
                    //setTimeout(function () { $("#msgModal").modal("hide"); }, 5000);
                    $("#lblmsg").html('<span class="text-danger"><span class="glyphicon glyphicon-remove"></span>&nbsp;'+ $("#txtbarcode").val() + ' Barcode Already Exists</span>');
                    textboxEnable();
                    //$("#lblerrormessage").text('Barcode Already Exists');
                }
                else if (data == 'OK')
                {
                    getproductioncount();
                    $("#lblmsg").html('<span class="text-success"><span class="glyphicon glyphicon-ok"></span>&nbsp;'+$("#txtbarcode").val() + ' Saved Successfully!</span>');
                    //$("#lblmsg").text('Save Successfully!');
                    textboxEnable();
                   // $("#msgModal").modal("show");
                   // setTimeout(function () { $("#msgModal").modal("hide"); }, 5000);
                   // $("#lblerrormessage").text('Save Successfully!');
                }
                else if (data == 'Logout') {
                    //location.reload();
                    location.href = document.URL;
                }

                //console.log(data);
            }
        });

    }
    function nokfunction() {
            $.ajax({
                type: "POST",
                url: "TbmActionPerform.asmx/nokfunction",
                data: "{ HmiId: '" + $("#lblwcname").text() + "', Barcode: '" + $("#txtbarcode").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (Result) {
                    var data = JSON.parse(Result.d);
                    if (data == 'Nok') {

                      
                       // $("#msgModal").modal("show");
                       // setTimeout(function () { $("#msgModal").modal("hide"); }, 5000);
                        $("#lblmsg").html('<span class="text-danger"><span class="glyphicon glyphicon-remove"></span>&nbsp;'+$("#txtbarcode").val() + ' Barcode Already Exists!</span>');
                        textboxEnable();
                         
                    }
                    else if (data == 'OK')
                    {
                        getproductioncount();
                        $("#lblmsg").html('<span class="text-success "><span class="glyphicon glyphicon-ok"></span>&nbsp;'+$("#txtbarcode").val() + ' Saved Successfully!</span>');
                     //   $("#lblmsg").text('Saved Successfully!');
                        textboxEnable();
                     //   $("#msgModal").modal("show");
                       // setTimeout(function () { $("#msgModal").modal("hide"); }, 5000);
                       
                    }
                    else if (data == 'Logout')
                    {
                        //location.reload();
                        location.href = document.URL;
                    }
                    //console.log(data);
                }
            });
        }
    function Logout() {
            $.ajax({
                type: "POST",
                url: "TbmActionPerform.asmx/Logout",
                data: "{ UserId: '" + $("#current-operator").text() + "',Password: '" + $("#login-password").val() + "',TBMName:'" + $("#lblwcname").text()+"'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (Result) {

                    var data = JSON.parse(Result.d);
                //    console.log(document.URL);
                    location.href = document.URL;
                    //location.reload();
                    //console.log(data);
                }
            });
    }
    function InterLockEnable() {
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/InterLockEnable",
            data: "{ HmiId: '" + $("#lblwcname").text() + "', Barcode: '" + $("#txtbarcode").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {

                var data = JSON.parse(Result.d);
              //  console.log(data);
                if (data == 'Nok') {
                  //  console.log(data);
                    
                    $("#lblmsg").html('<span class="text-danger"><span class="glyphicon glyphicon-remove"></span>&nbsp;' +$("#txtbarcode").val() + ' Barcode Already Exists');
                    $("#txtbarcode").val("");
                    $("#txtbarcode").prop("disabled", false);
                    $("#txtbarcode").focus();
                    $("#btnok,#btnnok").prop("disabled", true);
                    $("#btnnok,#btnok").addClass("btn-default");

                    $("#btnok").removeClass("btn-success");
                    $("#btnnok").removeClass("btn-danger");
                    $("#btnnok,#btnok").addClass("btn-default");
                   
                }
                else if (data == 'ok')
                {
                    $("#lblmsg").html('');

                    //$("#btnok,#btnnok").prop("disabled", false);
                    //$("#btnok").addClass("btn-info");
                    //$("#btnnok").addClass("btn-danger");
                   // console.log(data);
                }
               
            }
        });
    }

    function ChangePassword()
    {
        var curpass = $("#currentPassword").val();
        var newpass = $("#newPassword").val();
        var conpass = $("#confirmPassword").val();
        if (curpass == '' || conpass == '' || newpass == '')
        {
            $("#changepasswordconformation").text('Fill Blank Fields!');
        }
        else if (conpass == newpass) {
            CallChangePassword();
        }
        else
        {
            $("#changepasswordconformation").text('New Password and current password not matched!');
        }
      
    }
    

    function getHMIDisplayInfo() {
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/getHMIDisplayInfo",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {

                var data = JSON.parse(Result.d);
                $("#lblinformationflow").text(data);
                

            }
        });
    }

    function CallChangePassword() {
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/ChangePassword",
            data: "{ UserId: '" + $("#current-operator").text() + "',Password: '" + $("#currentPassword").val() + "',NewPassword: '" + $("#newPassword").val() + "',TBMName:'" + $("#lblwcname").text() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {

                var data = JSON.parse(Result.d);
                $("#changepasswordconformation").text(data);
                $("#currentPassword").val('');
                 $("#newPassword").val('');
                 $("#confirmPassword").val('');
                 
                // $("#logoutModal").modal("hide");
                 
                 //collapsed
                
            }
        });
    }
    
    function Login() {
            $.ajax({
                type: "POST",
                url: "TbmActionPerform.asmx/Login",
                data: "{ UserId: '" + $("#txtusername").val() + "',Password: '" + $("#txtpassword").val() + "',TBMName:'" + $("#lblwcname").text() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (Result) {

                    var data = JSON.parse(Result.d);
                    location.reload();
                    //console.log(data);
                }
            });
        }
    function setModalFlag()
    {
        $("#recipeMsgDiv").empty();
        $("#newRecipeBox").addClass("hide");
        modal = 0;
        flag = 0;
        $("#changepasswordconformation").text('');
        }

function Reload()
        {
           location.reload();
    }

    function loadoem() {

        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/OemImagePathFunction",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {

                var data = JSON.parse(Result.d);
                //console.log(data);

                var dataLen = data.length;
                var i = 0;
                var html = '';

                if (dataLen > 0)
                {
                    for (i = 0; i < dataLen; i++)
                    {
                        html += '<div class="col-sm-1 OEMImagesDiv"><a data-oem-path="OEMLogo/' + data[i].logopath +'" class="oemLink" href="javascript:;"><img src="OEMLogo/' + data[i].logopath+'"/></a></div>';
                    }

                    $("#OEMDiv").html(html);
                }
            }
        });

    }



    function LoadSimulationWcName() {
        $("#dpdMachineName").empty();
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/LoadselectselectSimulationWcName",
            data: "{ }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                Result = Result.d;
                $.each(Result, function (key, value) {
                    $("#dpdMachineName").append($("<option></option>").val
                        (value.id).html(value.Name.toUpperCase()));
                });
                
                $('#dpdMachineName option:contains(' + $("#lblwcname").text().toUpperCase() + ')').prop('selected', true);

                var checkWC = $("#lblwcname").text().toUpperCase();
                
                $('#dpdMachineName option').each(function ()
                {
                    if (this.text.toUpperCase() == checkWC)
                    {
                        $("#changeRecipeBtn").removeClass("hide");
                    }
                });
                
            },
            error: function (Result) {
                console.log('err:' + Result);
            }
        });

    }

    function LoadLastTyre() {
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/LoadLastTyre",
            data: "{  wcname:'" + $("#lblwcname").text() + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                 
                var data = JSON.parse(Result.d);
                flagDowntime =  parseInt(data);
            }
        });
    }


    function SaveDownTime() {
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/SaveDownTime",
            data: "{ wcname: '" + $("#lblwcname").text() + "',resionid: '" + $('select[id=dpdResion]').val()   + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                var data = JSON.parse(Result.d);
                if (data == 'OK') {

                   
                }
                
            }
        });

    }

    function SaveDownTimeOK() {
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/SaveDownTimeOK",
            data: "{ wcname: '" + $("#lblwcname").text() + "',resionid: '" + $('select[id=dpdResion]').val() + "',save:'ok' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {
                var data = JSON.parse(Result.d);
                if (data == 'OK') {


                }

            }
        });

    }
    function getHMIDownTimeStart() {
        flagDowntimestartfrom = 0;
        $.ajax({
            type: "POST",
            url: "TbmActionPerform.asmx/getHMIDownTimeStart",
            data: "{ Hmiid: '" + $("#lblwcname").text() + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (Result) {

                var data = JSON.parse(Result.d);
                flagDowntimestartfrom = parseInt(data);
                


            }
        });
    }
    </script>

 

        <script>
            $(document).ready(function () {
                $("#txtbarcode").focus();
                LoadReasion();
                getproductioncount();
                LoadSimulationWcName();
                getHMIDisplayInfo();
                getHMIDownTimeStart();
                $(document).click(function (event) {
                    if (!($(event.target).hasClass('operatorNameBtn')) ) {
                        $("#txtbarcode").focus();
                    }
                });

                $(document).on('click', '.oemLink', function ()
                {
                    var path = $(this).attr("data-oem-path");
                    $("#imgreceipe").attr("src", path);
                    setTimeout(function () {
                        $("#openOEMmodal").modal("hide");
                    },250);
                    
                });

                $('#txtbarcode').keypress(function (e) {
                    if (e.which == 13) {
                       
                       // var isDisabled = $('textbox').prop('disabled');
                        //if (!$("#txtbarcode").prop("disabled")) {
                           
                            var txtbarcodeval = $("#txtbarcode").val();
                            if (txtbarcodeval.length == 10) {
                                $("#btnok,#btnnok").prop("disabled", false);
                                $("#btnok").addClass("btn-success");
                                $("#btnnok").addClass("btn-danger");
                                $("#txtbarcode").prop("disabled", true);
                                InterLockEnable();

                            }
                            else {
                                $("#btnok,#btnnok").prop("disabled", true);
                                $("#btnok").removeClass("btn-success");
                                $("#btnnok").removeClass("btn-danger");
                                $("#btnnok,#btnok").addClass("btn-default");
                                $("#txtbarcode").val("");

                            }
                        //}
                    }
                });


                var operator1 = '<%:Session["UserId1"]%>';
                var operator2 = '<%:Session["UserId2"]%>';
                var operator3 = '<%:Session["UserId3"]%>';
                
                if (operator1 != undefined && operator1 != '') {
                    $("#operator1Btn").attr('data-target', '#logoutModal');
                }
                else {
                    $("#operator1Btn").attr('data-target', '#loginModal');
                }

                if (operator2 != undefined && operator2 != '') {
                    $("#operator2Btn").attr('data-target', '#logoutModal');
                }
                else {
                    $("#operator2Btn").attr('data-target', '#loginModal');
                }

                if (operator3 != undefined && operator3 != '') {
                    $("#operator3Btn").attr('data-target', '#logoutModal');
                }
                else {
                    $("#operator3Btn").attr('data-target', '#loginModal');
                }

                $(".operatorNameBtn").click(function () {
                    var name = $(this).find(".operatorName").text();
                    modal = 1;
                    $("#current-operator").text(name);
                   
                    
                });

            });


        </script>
          
    </head>
    <body class="bgcolorBlue">
         <form id="form1" runat="server" onkeypress="return event.keyCode != 13">
        <div class="row header">
             <div id="myDiv"></div>
            <div class="col-sm-12">
               

                <div class="logo col-sm-2">
                    <img alt="Smart Logo" src=".//assets/images/tbm_app/New Logo.png" />
                </div>

                <div class="col-sm-8">
                   
                    <ul class="nav navbar-nav">
                        <li class="dropdown">
                            <a id="operator1Btn"  href="javascript:;" data-toggle="modal" data-target="#logoutModal" data-backdrop="static" data-keyboard="false" class="dropdown-toggle operatorNameBtn" data-toggle="dropdown">
                                <div class="logout-panel">
                                      <asp:Image ID="imgop1" runat="server" />
                                  
                                    <span> <asp:Label ID="lblop1" class="operatorName" style="display:none" runat="server" Text="Operator 1"></asp:Label></span>
                                     <asp:Label ID="opname1"  runat="server" Text=""></asp:Label>
                                </div>
                            </a>
                        </li>
                        <li class="dropdown">
                            <a id="operator2Btn"  href="javascript:;" data-toggle="modal" data-target="#logoutModal" data-backdrop="static" data-keyboard="false" class="dropdown-toggle operatorNameBtn" data-toggle="dropdown">
                                <div class="logout-panel">
                                      <asp:Image ID="imgop2" runat="server" />
                                    <%--<img src=".//assets/images/tbm_app/default-user-img.jpg" alt="Profile Pic" />--%>
                                    
                                    <span><asp:Label ID="lblop2" class="operatorName"  style="display:none" runat="server" Text="Operator 2"></asp:Label></span>
                                     <asp:Label ID="opname2"  runat="server" Text=""></asp:Label>
                                </div>
                            </a>
                        </li>
                        <li class="dropdown">
                            <a id="operator3Btn"  href="javascript:;" data-toggle="modal" data-target="#logoutModal" data-backdrop="static" data-keyboard="false" class="dropdown-toggle operatorNameBtn" data-toggle="dropdown">
                                <div class="logout-panel">
                                     <asp:Image ID="imgop3" runat="server" />
                                    
                                    <span><asp:Label ID="lblop3" class="operatorName"  style="display:none" runat="server" Text="Operator 3"></asp:Label></span>
                                       <asp:Label ID="opname3"  runat="server" Text=""></asp:Label>
                                </div>
                            </a>
                        </li>
                         
                    </ul>
                   
                </div>
                    
                                
                                
                <div class="jklogo col-sm-2">
                    <img alt="JK Logo" src=".//assets/images/tbm_app/jkLogo.png" />
                </div>

            </div>

        </div>
         
           <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
    
     
        <div class="row">

            <div class="common-box" style="margin-left:2.5%;width:95%;margin-right:2.5%;margin-top:2.5%;height:620px;">
              
                <div class="col-sm-12">

                    <div class="col-sm-12 text-center" style="background-color:#428bca;color:#000;">
                        <label style="margin-bottom:0;" class="big-font">
                                <asp:Label ID="lblwcname" runat="server" Text=""></asp:Label>
                        </label>
                        <div class="pull-right" style="display:inline-block;">
                            <h5><b>  <asp:Label ID="lblTime"  style="font-size:17px;" runat="server" Text=""></asp:Label></b> </h5>
                        </div>
                        
                    </div>

                    <div class="col-sm-9" style="border:1px solid silver;height: 215px;">
                        
                        <div class="col-sm-10">

                            <div class="col-sm-1">
                                <a class="btn btn-primary hide operatorNameBtn" style="margin-top: 12.5px;font-size:18px;" id="changeRecipeBtn" href="javascript:;">
                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;Edit
                                </a>
                            </div>
                            <div class="col-sm-11" style="padding-left: 0;">
                               <label class="big-font-large" style="font-size:25px !important;margin-top: 7px;margin-left:9%;"> 
                                  Recipe  :   <asp:Label ID="lblraceipe" runat="server" Text=""></asp:Label>
                                  
                                </label> 
                            </div>
                             <div class="col-sm-11" style="padding-left: 0;">
                               <label class="big-font-large" style="font-size:25px !important;margin-top: 7px;margin-left:9%;"> 
                                  
                                     &nbsp;  &nbsp; &nbsp; &nbsp;  Recipe Code :    <asp:Label ID="lblsapcode" runat="server" Text=""></asp:Label>
                                </label> 
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-sm-12" style="margin-top:4%;">
                            <div class="form-horizontal" id="tbmForm" >
                                <div class="form-group" style="">

                                    <div class="col-sm-12">
                                        <div class="col-sm-6 big-font" style="padding:0;">
                                            <asp:TextBox ID="txtbarcode"  style="width: 280px;height:56px;margin-top:2px;" runat="server"></asp:TextBox>
                                
                                        </div>
                                        <div class="col-sm-6">
                                            <input id="btnok"  class="btn btn-success marginRight15px big-btn" type="button" value="OK" onclick="okfunction();" />
                                            <input id="btnnok"  class="btn btn-danger marginRight15px big-btn" type="button" value="NOK" onclick="nokfunction();" />
                               
                                        </div>
                                    </div>
                                    
                                    <div class="clearfix"></div>

                                    <div class="form-group">
                                        <div class="alert alert-block alert-warning" style="margin-top:2%;margin-bottom:2.5px;padding:10px;width:120%;">
                                
						                        <i class="ace-icon fa fa-check green"></i>

						                        <asp:Label ID="lblmsg" style="font-size:20px;" runat="server" Text=""></asp:Label> 
                                        </div>
                                    </div>
  
                                </div>
                            </div>
                        </div>


                        </div>

                        <div class="col-sm-2" style="padding:0;">
                            <a class="operatorNameBtn" onclick="loadoem();" href="javascript:;" data-toggle="modal" data-target="#openOEMmodal" data-backdrop="static" data-keyboard="false" >
                                <div class="col-sm-12" id="carImageBox" style="padding-left: 0;">
                                    <asp:Image ID="imgreceipe" src="Logo/logo.png" class="img-thumbnail" style="margin-top: 10px;" runat="server" />
                                </div>
                            </a>
                        </div>
                        
                        <div class="clearfix"></div>

                   </div>

                        <%--<div class="col-sm-6">
                           
                        <a class="operatorNameBtn btn btn-primary btn-lg" onclick="loadoem();" href="javascript:;" data-toggle="modal" data-target="#openOEMmodal" data-backdrop="static" data-keyboard="false" style="width: 155px;margin-bottom:5px;">Change OEM</a>
                        </div>--%>

                    <div class="col-sm-3" style="border:1px solid silver;height: 215px;">
                        <div class="col-sm-12" style="margin-top: 15px;">
                             <ul style="font-size:22px;" class="list-unstyled">
                                 <li>
                                    <asp:Image ID="imgCyclestart" ImageUrl="~/Images/nok.png" Width="30px" class="img-thumbnail"  runat="server" />    Cycle Start  
                                 </li>
                                 <li>
                                    <asp:Image ID="ImgInterlock" ImageUrl="~/Images/nok.png" Width="30px" class="img-thumbnail"  runat="server" />   Interlock Bit  
                                 </li>
                                 <li>
                                    <asp:Image ID="imgCommunication" ImageUrl="~/Images/red_bl.gif" Width="30px" class="img-thumbnail"  runat="server" />     Communication 
                                 </li>
                             </ul>
                  
                        </div>
                    </div>

                    <div class="clearfix"></div>

                </div>

                 <%--<div class="col-sm-3" id="carImageBox">
                     <a class="operatorNameBtn btn btn-primary btn-lg" onclick="loadoem();" href="javascript:;" data-toggle="modal" data-target="#openOEMmodal" data-backdrop="static" data-keyboard="false" style="width: 125px;margin-left: 47%;margin-bottom:5px;">Change OEM</a>
                        
                    <asp:Image ID="imgreceipe" src="Logo/logo.png" class="img-thumbnail"  runat="server" />
                   <%-- <img src=".//assets/images/tbm_app//maruti.jpg" class="img-thumbnail" alt="Image Preview">
                </div>--%>

                <div class="clearfix"></div>
                
                <div class="col-sm-12">
                    <div class="col-sm-4" id="chartBox">
                        <h4 style="margin-bottom:0;" class="text-center">A Shift</h4>
                         <div class="col-sm-12" id="containerA" style="width: 100%; height: 220px; margin: 0 auto;">
                        <%--<img src=".//assets/images/tbm_app//Charts.png" class="img-thumbnail" alt="Chart Preview">--%>
                        </div>

                    </div>
                    <div class="col-sm-4" id="chartBox2">
                        <h4 style="margin-bottom:0;" class="text-center">B Shift</h4>
                        <div class="col-sm-12" id="containerB" style="width: 100%; height: 220px; margin: 0 auto;">
                        <%--<img src=".//assets/images/tbm_app//Charts.png" class="img-thumbnail" alt="Chart Preview">--%>
                        </div>

                    </div>
                    <div class="col-sm-4" id="chartBox3">
                        <h4 style="margin-bottom:0;" class="text-center">C Shift</h4>
                        <div class="col-sm-12" id="containerC" style="width: 100%; height: 220px; margin: 0 auto">
                        <%--<img src=".//assets/images/tbm_app//Charts.png" class="img-thumbnail" alt="Chart Preview">--%>
                        </div>

                    </div>

                
               </div>
         <div class="col-sm-12">
             <div class="alert alert-block alert-warning" style="margin-bottom:1px;padding:2.5px;">
								 
				<i class="ace-icon fa fa-check green"></i>

				<asp:Label ID="lblerrormessage" style="font-size:22px;" runat="server" Text=""></asp:Label> 
			</div>
                             
        </div>

        <div class="col-sm-12">
             <div class="alert alert-block alert-info" style="margin-bottom:1px;padding:2.5px;">
                <marquee style="font-size:22px;color:#000;" direction="right"><asp:Label ID="lblinformationflow" runat="server" Text=""></asp:Label></marquee>
			</div>                
        </div>

        <!-- Message Modal -->
        <div id="downtimeModal" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-sm-4">
                            <h4 style="margin-top: 7px;"class="modal-title">Down Time</h4>
                        </div>
                        <div class="col-sm-8 pull-right">
                             <h5><b>  <asp:Label ID="lbldowninsec"  style="font-size:17px;" runat="server" Text=""></asp:Label></b> </h5>
                        </div>
                        
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-sm-2" style="margin-top:7px;">Reason</label>
                            <div class="col-sm-10">
                              <asp:DropDownList style="width:200px;" ID="dpdResion"  CssClass="dropdown form-control operatorNameBtn"   runat="server">
                                </asp:DropDownList>
                                
                            </div>  
                        </div>
                        <div class="form-group"><br /><br />
                            <button  style="margin-top:15px;margin-left: 10px;" type="button" onclick="closeDownTime();" class="btn btn-block btn-primary">OK</button>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!-- Message Modal -->
        <div id="msgModal" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Response</h4>
                    </div>
                    <div class="modal-body">
                        <p>  <%--<asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>--%></p>
                    </div>
                </div>

            </div>
        </div>

                <!-- Message Modal -->
        <div id="msgModalWait" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content" style="margin-top: 24%;height: 132px;">
                    
                    <div class="modal-body">
                        <p style="font-size: 37px;text-align: center;margin-top: 20px;">  Waiting For Next Cycle </p>
                    </div>
                </div>

            </div>
        </div>

        <!-- Login Modal -->
        <div id="loginModal" class="modal fade" role="dialog"  >
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" onclick="setModalFlag();" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><span class="glyphicon glyphicon-lock"></span> Login to your Account</h4>
                    </div>
                    <div class="modal-body">
                        <div id="Login-Form" >

                            <div class="form-group">
                                <div class="input-group">
                                    <div class="input-group-addon"><span class="glyphicon glyphicon-envelope"></span></div>
                                   <asp:TextBox ID="txtusername" placeholder="Enter Username" CssClass="form-control input-lg" runat="server"></asp:TextBox>
                                     
                                </div>
                            </div>


                            <div class="form-group">
                                <div class="input-group">
                                    <div class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></div>
                                    <asp:TextBox ID="txtpassword" TextMode="Password" CssClass="form-control input-lg" placeholder="Enter Password" runat="server"></asp:TextBox>
                                  
                                </div>
                            </div>
                        <asp:Button ID="btnlogin" CssClass="btn btn-info btn-block btn-lg" runat="server" Text="LOGIN" OnClick="btnlogin_Click" />
                            <%--<button ID="btnlogin" onclick="Login();" class="btn btn-info btn-block btn-lg" >LOGIN</button>--%>
                            
                         
                        </div>
                    </div>
                    <div class="modal-footer">
                        <%-- <button ID="btnReload" onclick="reload();" class="btn btn-info btn-block btn-lg" >Reload</button>--%>
                    </div>
                </div>

            </div>
        </div>

        <!-- Logout Modal -->
        <div id="logoutModal" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" onclick="setModalFlag();" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><span class="glyphicon glyphicon-lock"></span> Logout to your Account</h4>
                    </div>
                    <div class="modal-body">
                        <div id="Login-Form" >
                            <div class="form-group">
                                
                                <div class="panel-group">
                                    <div class="panel panel-default">
                                      <div class="panel-heading" style="background-color:#b76060;">
                                        <h4 class="panel-title">
                                          <a style="padding-right: 385px;padding-top: 10px;padding-bottom: 10px;text-decoration:none;" data-toggle="collapse" href="#collapse1">Change password</a>
                                        </h4>
                                      </div>
                                      <div id="collapse1" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <div class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></div>
                                                    <input name="password" id="currentPassword" type="password" class="form-control input-lg" placeholder="Enter Current Password" required="">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <div class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></div>
                                                    <input name="password" id="newPassword" type="password" class="form-control input-lg" placeholder="Enter New Password" required="">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <div class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></div>
                                                    <input name="password" id="confirmPassword" type="password" class="form-control input-lg" placeholder="Enter Confirm Password" required="">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="input-group">
                                                    
                                                   <i class="ace-icon fa fa-check green"></i>

				<span id="changepasswordconformation" style="color:red"></span>
                                                </div>
                                            </div>
                                            <button type="button" onclick="ChangePassword();" class="btn btn-info btn-block btn-lg">Save Change</button>

                                        </div>
                                      </div>
                                    </div>
                                </div>
    
                                <div class="input-group">
                                      <label id="current-operator">Operator Name</label>
                                      <asp:Label ID="lblopid" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group">
                                    <div class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></div>
                                    <input name="password" id="login-password" type="password" class="form-control input-lg" placeholder="Enter Password" required="">
                                </div>
                            </div>
                            <button type="button" onclick="Logout();" class="btn btn-info btn-block btn-lg">LOGOUT</button>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div id="openOEMmodal" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" onclick="setModalFlag();" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Select OEM Pic</h4>
                    </div>
                    <div class="modal-body" style="height: 240px;overflow-y: auto;">
                        <div id="Login-Form" >
                            <div class="form-group" id="OEMDiv">
                            </div>
            
                        </div>
                    </div>
                </div>

            </div>
        </div>

                <div id="recipeModal" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" onclick="setModalFlag();" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Select Recipe</h4>
                    </div>
                    <div class="modal-body" style="height: 260px;">
                        <div class="col-sm-12" >
                           
                            <div class="">
                               <div class="form-group">
                                    <label class="col-sm-4 big-font2">Machine</label>
                                      <div class="col-sm-6 big-font2">
                                       <asp:DropDownList ID="dpdMachineName" Enabled="false"  CssClass="dropdown"   runat="server">
                                </asp:DropDownList></div>
                                </div>

                                <div class="clearfix"></div>

                                <div class="form-group">
                                    <label class="col-sm-4 big-font2">Receipe</label>
                                     <div class="col-sm-8 big-font2">
                                        <asp:DropDownList CssClass="dropdown operatorNameBtn " ID="dpdReceipeName" runat="server">
                                </asp:DropDownList>

                                  </div>
                                </div>

                                <div class="clearfix"></div>

                                <div class="hide" id="newRecipeBox">
                                    <label class="col-sm-4 big-font2">New Receipe</label>
                                      <div class="col-sm-8 big-font2">
                                        <asp:TextBox ID="TxtReceipeName" class="operatorNameBtn" style="width:260px" runat="server"></asp:TextBox>
                                         <input id="btnnewReceipe" class="btn btn-app btn-primary radius-4 btn-lg" onclick="AddNewReceipe();" type="button" value="Add" />
                                      </div>
                                </div>

                                <div class="clearfix"></div>
                       
                                <div class="form-group">
                                    <input id="btnsave" style="margin-left: 10px;margin-top: 5px;" class="btn btn-lg btn-app btn-success radius-4" onclick="UpdateReceipeTag();" type="button" value="Save Changes" />
                                </div>
                                <div class="form-group text-center">
                                    <div id="recipeMsgDiv"></div>
                                </div>
                            </div>
      
                        </div>
                    </div>
                </div>

            </div>

        </div>

                

            </div>

            
            </div>


             </form>
    </body>
</html>
