<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="CPanel_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="au theme template" />
    <meta name="author" content="Hau Nguyen" />
    <meta name="keywords" content="au theme template" />
    <!-- Fontfaces CSS-->
    <link href="../css/font-face.css" rel="stylesheet" media="all" />
    <link href="../vendor/font-awesome-4.7/css/font-awesome.min.css" rel="stylesheet" media="all" />
    <link href="../vendor/font-awesome-5/css/fontawesome-all.min.css" rel="stylesheet" media="all" />
    <link href="../vendor/mdi-font/css/material-design-iconic-font.min.css" rel="stylesheet" media="all" />

    <!-- Bootstrap CSS-->
    <link href="../vendor/bootstrap-4.1/bootstrap.min.css" rel="stylesheet" media="all" />

    <!-- Vendor CSS-->
    <link href="../vendor/animsition/animsition.min.css" rel="stylesheet" media="all" />
    <link href="../vendor/bootstrap-progressbar/bootstrap-progressbar-3.3.4.min.css" rel="stylesheet" media="all" />
    <link href="../vendor/wow/animate.css" rel="stylesheet" media="all" />
    <link href="../vendor/css-hamburgers/hamburgers.min.css" rel="stylesheet" media="all" />
    <link href="../vendor/slick/slick.css" rel="stylesheet" media="all" />
    <link href="../vendor/select2/select2.min.css" rel="stylesheet" media="all" />
    <link href="../vendor/perfect-scrollbar/perfect-scrollbar.css" rel="stylesheet" media="all" />

    <!-- Main CSS-->
    <link href="../css/theme.css" rel="stylesheet" media="all" />
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="../js/bootstrap-notify.min.js"></script>
    <!-- Title Page-->
    <title>Login</title>

</head>
<body class="animsition wrapper animated fadeIn">
    <div class="page-wrapper">
        <div class="page-content--bge5">
            <div class="container">
                <div class="login-wrap">
                    <div class="login-content">
                        <div class="login-logo">
                            <a href="#">
                                <img src="images/DabbaGulLogo.png" height="200" width="200" alt="CoolAdmin">
                            </a>
                        </div>
                        <div class="login-form">
                            <form id="form1" runat="server" method="post" defaultbutton="btnsubmit">
                                <div id="dvUserNM" class="form-group">
                                    <label>User Name</label>
                                    <asp:TextBox ID="txtUserName" runat="server" placeholder="Enter user name." MaxLength="15" CssClass="au-input au-input--full" />
                                </div>
                                <div id="dvPwd" class="form-group">
                                    <label>Password</label>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter password." MaxLength="15" CssClass="au-input au-input--full" />
                                </div>
                                <div class="login-checkbox">
                                    <label>
                                        <asp:CheckBox ID="chkRemember" runat="server" Text="Remember Me" />
                                    </label>
                                    <label>
                                        <a href="ForgotPassword.aspx">Forgotten Password?</a>
                                    </label>
                                </div>
                                <asp:Button ID="btnsubmit" runat="server" OnClick="btnsubmit_Click" OnClientClick="return ValidateForm();" CssClass="au-btn au-btn--block au-btn--green m-b-20" Text="Sign In"></asp:Button>

                                <%--<div class="social-login-content">
                                    <div class="social-button">
                                        <button class="au-btn au-btn--block au-btn--blue m-b-20">sign in with facebook</button>
                                        <button class="au-btn au-btn--block au-btn--blue2">sign in with twitter</button>
                                    </div>
                                </div>--%>
                            </form>
                            <%-- <div class="register-link">
                                <p>
                                    Don't you have account?
                                   
                                    <a href="#">Sign Up Here</a>
                                </p>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <!-- Jquery JS-->

    <script type="text/javascript" src="../vendor/jquery-3.2.1.min.js"></script>
    <!-- Bootstrap JS-->
    <script type="text/javascript" src="../vendor/bootstrap-4.1/bootstrap.min.js"></script>  
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>

    <!-- Vendor JS       -->
    <script type="text/javascript" src="../vendor/slick/slick.min.js">    </script>
    <script type="text/javascript" src="../vendor/wow/wow.min.js"></script>
    <script type="text/javascript" src="../vendor/animsition/animsition.min.js"></script>
    <script type="text/javascript" src="../vendor/bootstrap-progressbar/bootstrap-progressbar.min.js">    </script>
    <script type="text/javascript" src="../vendor/counter-up/jquery.waypoints.min.js"></script>
    <script type="text/javascript" src="../vendor/counter-up/jquery.counterup.min.js">    </script>
    <script type="text/javascript" src="../vendor/circle-progress/circle-progress.min.js"></script>
    <script type="text/javascript" src="../vendor/perfect-scrollbar/perfect-scrollbar.js"></script>
    <script type="text/javascript" src="../vendor/chartjs/Chart.bundle.min.js"></script>
    <script type="text/javascript" src="../vendor/select2/select2.min.js">    </script>

    <!-- Main JS-->
    <script type="text/javascript" src="../js/main.js"></script>
    <script type="text/javascript">
        var notify
        function ValidateForm() {

            if (notify != null) {
                notify.close();
            }

            if (($.trim($("#<%=txtUserName.ClientID%>").val()) == "") && ($.trim($("#<%=txtPassword.ClientID%>").val()) == "")) {
                $("#<%=txtUserName.ClientID%>").focus();
                notify = $.notify({ title: "Login Failed : ", message: "Please enter Username and Password.", icon: 'fa fa-close' }, { type: "danger", newest_on_top: true });

                $("#dvUserNM").addClass("has-error");
                $("#dvPwd").addClass("has-error");
                return false;
            }
            else if (($.trim($("#<%=txtUserName.ClientID%>").val()) != "") && ($.trim($("#<%=txtPassword.ClientID%>").val()) == "")) {
                notify = $.notify({ title: "Login Failed : ", message: "Please enter Password.", icon: 'fa fa-close' }, { type: "danger", newest_on_top: true });
                $("#<%=txtPassword.ClientID%>").focus(); ""
                $("#dvUserNM").removeClass("has-error");
                $("#dvPwd").addClass("has-error");
                return false;
            }
            else if (($.trim($("#<%=txtUserName.ClientID%>").val()) == "") && ($.trim($("#<%=txtPassword.ClientID%>").val()) != "")) {
                notify = $.notify({ title: "Login Failed : ", message: "Please enter Username.", icon: 'fa fa-close' }, { type: "danger", newest_on_top: true });
                $("#<%=txtUserName.ClientID%>").focus();
                $("#dvUserNM").addClass("has-error");
                $("#dvPwd").removeClass("has-error");
                return false;
            }
            else {
                $("#dvUserNM").removeClass("has-error");
                $("#dvPwd").removeClass("has-error");
                return true;
            }
}

    </script>

</body>
</html>
