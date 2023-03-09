<%@ Page Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="ChangePassword.aspx.cs" Inherits="_Default" Title="Change Password" %>

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
                    <table width="100%" style="font-weight: bold">
                        <tr>
                            <td>
                                User Name
                            </td>
                            <td>
                                <asp:TextBox type="text" ID="txtUserName" value="User Name" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Old Password
                            </td>
                            <td>
                                <asp:TextBox type="password" ID="txtPassword" value="Old Password" runat="server"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                New Password
                            </td>
                            <td>
                                <asp:TextBox type="password" ID="txtNPassword" value="New Password" runat="server" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="2">
                                <br />
                                <asp:Button ID="btnLogin" Width="180px" runat="server" Text="Update Password" CssClass="submit"
                                    OnClick="btnLogin_Click1" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnCancel" Width="180px" runat="server" Text="Cancel" CssClass="submit"
                                    OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </p>
            </fieldset>
        </div>
    </div>
</asp:Content>
