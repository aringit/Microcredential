<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ClientUI.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <!------ Include the above in your HEAD tag ---------->
    <link href="css/main.css" rel="stylesheet" />
    <form id="form1" runat="server">
        <div class="sidenav">
            <div class="login-main-text">
                <h2>Welcome to Client Portal</h2>
                <%-- <br> Login Page
            <p>Login or register from here to access.</p>--%>
            </div>
        </div>
        <div class="main">
            <div class="col-md-6 col-sm-12">
                <div class="login-form">


                    <div class="form-group">
                        <label>User Name</label>                       
                        <asp:TextBox Id="txtUsername"  runat="server" class="form-control" placeholder="User Name"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Password</label>
                        <asp:TextBox Id="txtPassword" runat="server" class="form-control" placeholder="Password"></asp:TextBox>
                    </div>
                     <div class="form-group">
                        <asp:Label ID="lblLoginMessage" runat="server"></asp:Label>
                       
                    </div>
                    <asp:Button runat="server" class="btn btn-black" ID="btnLogin" Text="Login" OnClick="btnLogin_Click" />
                    <%--<button type="submit" class="btn btn-black">Login</button>--%>
                    <%--<button type="submit" class="btn btn-secondary">Register</button>--%>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
