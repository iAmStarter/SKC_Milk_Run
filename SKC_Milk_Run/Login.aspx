<%@ Page Title="Login K-Epress" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SKC_Milk_Run.Login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>K-Epress | Log in</title>

    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
    <!-- icheck bootstrap -->
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.min.css">
    <style type="text/css">
        .auto-SKC_Logo {
            width: 360px;
            height: 180px;
        }
    </style>
</head>
<body class="hold-transition login-page">
    <div class="login-box" style="background-color:#ffffff !important">
        <div class="login-logo" >
            <img class="auto-SKC_Logo" src="Images/logo/K_Express.png" />
        </div>
        <!-- /.login-logo -->
        <div class="card">
            <div class="card-body login-card-body">
                <p style="font-size:24px;color:orangered;text-align:center">K-EXPRESS (TEST)</p>
                <p class="login-box-msg">Sign in to start your session</p>

                <form method="post"  runat="server">
                    <div class="input-group mb-3">
                       
                        <asp:TextBox type="text" ID="TextUsername" CssClass="form-control" placeholder="User Name" runat="server" BackColor="#ffffff" ForeColor="#999999"></asp:TextBox>
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-user"></span>
                            </div>
                        </div>
                    </div>
                    <div class="input-group mb-3">
                      
                        <asp:TextBox type="password" ID="TextPassword" CssClass="form-control" placeholder="Password" runat="server" BackColor="#ffffff" ForeColor="#999999"></asp:TextBox>
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-lock"></span>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                       
                        <!-- /.col -->
                        <div class="col-4">                            
                            <asp:Button type="submit" CssClass="btn btn-block" BackColor="#999999" ForeColor="#ffffff" Text="Log In" OnClick="BtnLogin_Click" runat="server" />
                        </div>
                        <!-- /.col -->
                    </div>
                     <div class="row mt-2">
                       
                        <!-- /.col -->
                        <div class="col">    
                            <a class="btn btn-primary btn-block" href="AzureLogin.aspx">Log In Azure</a>
                        </div>
                        <!-- /.col -->
                    </div>
                    <div class="row">
                        <div class="col">
                            <asp:Label ID="Labelmsg" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                </form>
            </div>
            <!-- /.login-card-body -->
        </div>
    </div>
    <!-- /.login-box -->

    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
</body>
</html>


