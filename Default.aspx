<%@ Page Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Title="Login Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="StyleSheet/Login.css" rel="stylesheet" type="text/css" />
    <div class="divclass" style="background:#eaecef;">
        <div id="login" style="background: #FFF;">
            <h2 style="background: #eaecef;margin:0px;">
                Please Enter Your login Details.</h2>
            <fieldset style="padding-top: 25px">
                <p>
                    <asp:TextBox type="text" ID="txtUserName" runat="server" value="UserID" onblur="if(this.value=='')this.value='UserID'"
                        onfocus="if(this.value=='UserID')this.value='' " /></p>
                <p>
                    <asp:TextBox type="password" ID="txtPassword" runat="server" value="Password" onblur="if(this.value=='')this.value='Password'"
                        onfocus="if(this.value=='Password')this.value='' " /></p>
                <p>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="submit" OnClick="btnLogin_Click" />
                </p>
                <p>
                    <a href="ChangePassword.aspx" style="font-size: 12px; font-weight: bold">Change Password</a></p>
            </fieldset>
        </div>
    </div>
</asp:Content>
