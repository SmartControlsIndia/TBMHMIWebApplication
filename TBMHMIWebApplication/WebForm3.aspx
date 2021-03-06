﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="TBMHMIWebApplication.WebForm3" %>

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
</head>
 <body class="bgcolorBlue">

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
                        <img alt="Operator Pic" src=".//assets/images/tbm_app/default-user-img.jpg" />
                    </div>
                    <div class="text-center"><h4>Operator 1</h4></div>
                    <div class="text-center">
                        <button type="button" class="btn btn-info margin15px" data-toggle="modal" data-target="#loginModal" data-backdrop="static" data-keyboard="false">Login to your Account</button>
                    </div>

                </div>

                <div class="cart col-sm-3">
                    <div class="picDiv">
                        <img alt="Operator Pic" src=".//assets/images/tbm_app/default-user-img.jpg" />
                    </div>
                    <div class="text-center"><h4>Operator 2</h4></div>
                    <div class="text-center">
                        <button type="button" class="btn btn-info margin15px" data-toggle="modal" data-target="#loginModal" data-backdrop="static" data-keyboard="false">Login to your Account</button>
                    </div>

                </div>

                <div class="cart col-sm-3">
                    <div class="picDiv">
                        <img alt="Operator Pic" src=".//assets/images/tbm_app/default-user-img.jpg" />
                    </div>
                    <div class="text-center"><h4>Operator 3</h4></div>
                    <div class="text-center">
                        <button type="button" class="btn btn-info margin15px" data-toggle="modal" data-target="#loginModal" data-backdrop="static" data-keyboard="false">Login to your Account</button>
                    </div>

                </div>

            </div>

        </div>

        <div class="row">
            <div class="footer">
                <h4 class="margin10px">&copy Smart Controls India Ltd.</h4>
            </div>
        </div>

        <!-- Login Modal -->
        <div id="loginModal" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title"><span class="glyphicon glyphicon-lock"></span> Login to your Account</h4>
                    </div>
                    <div class="modal-body">
                        <form method="post" id="Login-Form" role="form">

                            <div class="form-group">
                                <div class="input-group">
                                    <div class="input-group-addon"><span class="glyphicon glyphicon-envelope"></span></div>
                                    <input name="username" id="username" type="text" class="form-control input-lg" placeholder="Enter Username" required="">
                                </div>
                            </div>


                            <div class="form-group">
                                <div class="input-group">
                                    <div class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></div>
                                    <input name="password" id="login-password" type="password" class="form-control input-lg" placeholder="Enter Password" required="" data-parsley-length="[6, 10]" data-parsley-trigger="keyup">
                                </div>
                            </div>

                            <div class="checkbox">
                                <label><input type="checkbox" value="" checked="">Remember me</label>
                            </div>
                            <button type="button" class="btn btn-info btn-block btn-lg">LOGIN</button>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <a href="javascript:void(0)">Forgot Password?</a>
                    </div>
                </div>

            </div>
        </div>

        <script>
        </script>

    </body>
</html>
