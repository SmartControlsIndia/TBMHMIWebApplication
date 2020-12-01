<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TBMHMIWebApplication.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta charset="utf-8" />
        <title>TBM Application Login</title>
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="stylesheet" href=".//assets/css/bootstrap.css" />
        <link rel="stylesheet" href=".//assets/css/style.css" />
        <script src=".//Scripts/jquery-1.10.2.min.js"></script>
        <script src=".//Scripts/bootstrap.min.js"></script>

    <script src="virtualkeyboard/jquery.numpad.js"></script>


    <%--<link href="virtualkeyboard/jquery-ui.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="jquery.min.js" type="text/javascript"></script>--%>
    <%--<script src="jquery-ui.min.js" type="text/javascript"></script>--%>
  <%--  <link href="virtualkeyboard/keyboard.css" rel="stylesheet" type="text/css" />
    <script src="virtualkeyboard/jquery.keyboard.js" type="text/javascript"></script>
    <script src="virtualkeyboard/jquery.keyboard.extension-typing.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            user();
            password();
           
        });

        function user()  {
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
    </script>--%>


    	<style>
         
			.nmpd-wrapper {display: none;}
			.nmpd-target {cursor: pointer;}
			.nmpd-grid {position:absolute; left:50px; top:50px; z-index:5000; -khtml-user-select: none; padding:10px; width: 44%;}
			.nmpd-overlay {z-index:4999;}
			input.nmpd-display {text-align: left;font-size:20px;}

            

            .nmpd-grid .btn{
                padding: 22px 20px !important;
            }

            .nmpd-grid .numero{
                padding: 17px 46px !important;
                font-size: 22px !important;
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
                $('#txtpassword').numpad({
                    displayTpl: '<input class="form-control" type="password" />',
                    hidePlusMinusButton: true,
                    hideDecimalButton: true
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


    <script>
        //$(document).ready(function () {
            
        //    if ($("#lblinformation").text() != '0' && $("#lblinformation").text() != '' )
        //    {
               
        //        $("#lblinformation").text("First You Logout From HMI " + $("#lblinformation").text());
        //        $("#msgModal").modal("show");
        //        setTimeout(function () { $("#msgModal").modal("hide"); }, 5000);
        //    }
            
        //});
      </script> 


</head>
 <body class="bgcolorBlue">
      <form id="form1" runat="server">
        <div class="row header">

            <div class="col-sm-12">

                <div class="logo col-sm-2">
                    <img alt="Smart Logo" src=".//assets/images/tbm_app/New Logo.png" />
                </div>

                <div class="col-sm-8"></div>

                <div class="jklogo col-sm-2">
                    <img alt="JK Logo" src=".//assets/images/tbm_app/jkLogo.png" />
                </div>

            </div>

        </div>

        <div class="row" id="container">
      
														 
													 
            <div style="margin-top:8%;" class="wrapper-container col-sm-offset-1 col-sm-10 col-sm-offset-1">

                <div class="cart col-sm-3">
                    <div class="picDiv">
                           <asp:Image ID="imgop1" runat="server" />
                       <%-- <img alt="Operator Pic" src=".//assets/images/tbm_app/default-user-img.jpg" />--%>
                    </div>
                    <div class="text-center"><h4> <asp:Label ID="lblop1" runat="server" Text="Operator 1"></asp:Label></h4></div>
                    <div class="text-center">
                        <asp:Button ID="btnloginop1" CssClass="btn btn-info margin15px" Visible="false" runat="server"   
                               Text="Logout" OnClick="btnloginop1_Click" />
                        
                        <button type="button" class="btn btn-info margin15px" data-toggle="modal" runat="server" id="oplogin1"
                            data-target="#loginModal" data-backdrop="static" data-keyboard="false">
                            Login to your Account</button>
                    </div>

                </div>

                <div class="cart col-sm-3">
                    <div class="picDiv">
                           <asp:Image ID="imgop2" runat="server" />
                       <%-- <img alt="Operator Pic" src=".//assets/images/tbm_app/default-user-img.jpg" />--%>
                    </div>
                    <div class="text-center"><h4><asp:Label ID="lblop2" runat="server" Text="Operator 2"></asp:Label> </h4></div>
                    <div class="text-center">
                        <asp:Button ID="btnloginop2" CssClass="btn btn-info margin15px" Visible="false" runat="server"   
                               Text="Logout" OnClick="btnloginop2_Click" />
                        <button type="button" class="btn btn-info margin15px" data-toggle="modal" data-target="#loginModal" data-backdrop="static" runat="server" id="oplogin2" 
                            data-keyboard="false">
                            Login to your Account</button>
                    </div>

                </div>

                <div class="cart col-sm-3">
                    <div class="picDiv">
                        <asp:Image ID="imgop3" runat="server" />
                        <%--<img alt="Operator Pic" src=".//assets/images/tbm_app/default-user-img.jpg" />--%>
                    </div>
                    <div class="text-center"><h4><asp:Label ID="lblop3" runat="server" Text="Operator 3"></asp:Label></h4></div>
                    <div class="text-center">
                        <asp:Button ID="btnloginop3" CssClass="btn btn-info margin15px" Visible="false" runat="server"   
                               Text="Logout" OnClick="btnloginop3_Click" />
                        <button type="button" class="btn btn-info margin15px" data-toggle="modal" runat="server" id="oplogin3"
                            data-target="#loginModal" data-backdrop="static" data-keyboard="false">
                            Login to your Account</button>
                    </div>

                </div>

            </div>

        </div>

        <div class="row" style="margin-top:11%;">
            <div class="footer">
                <h4 class="margin10px">&copy Smart Controls India Ltd.</h4>
            </div>
        </div>

        <!-- Login Modal -->
        <div id="loginModal" class="modal fade" role="dialog" runat="server">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><span class="glyphicon glyphicon-lock"></span> Login to your Account</h4>
                    </div>
                    <div class="modal-body">
                      <%--  <div id="Login-Form" role="form">--%>

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
                            
                         
                        <%--</div>--%>
                    </div>
                    <div class="modal-footer">
                         
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
                        <%--<h4 class="modal-title">Response</h4>--%>
                    </div>
                    <div class="modal-body">
                        <p class="text-warning">  <b>    <asp:Label ID="lblinformation" runat="server" Text=""></asp:Label></b></p>
                    </div>
                </div>

            </div>
        </div>
          </form>
    </body>
 
</html>
