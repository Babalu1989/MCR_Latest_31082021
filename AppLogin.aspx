<%@ Page Title="" Language="C#" MasterPageFile="~/MCR.master" AutoEventWireup="true" CodeFile="AppLogin.aspx.cs" Inherits="AppLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<link href="StyleSheet/Login.css" rel="stylesheet" type="text/css" />
    <div class="divclass" style="background:#eaecef;">
        <div id="login" style="background: #FFF;">
            <h2 style="background: #eaecef;margin:0px;">
                Please Enter Your login Details.</h2>
            <fieldset style="padding-top: 25px">
                <p>
                    <asp:TextBox type="text" ID="txtUserName" runat="server" value="CA Number" onblur="if(this.value=='')this.value='CA Number'"
                        onfocus="if(this.value=='CA Number')this.value='' " /></p>
                 <p>
                    <asp:TextBox type="text" ID="txtOrdNo" runat="server" value="Order Number" onblur="if(this.value=='')this.value='Order Number'"
                        onfocus="if(this.value=='Order Number')this.value='' " /></p>
                <p>
                    <asp:TextBox type="text" ID="txtPassword" runat="server" value="Meter Number" onblur="if(this.value=='')this.value='Meter Number'"
                        onfocus="if(this.value=='Meter Number')this.value='' "/></p>
                <p>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="submit" OnClick="btnLogin_Click" />
                    <asp:LinkButton ID="LnkPDFDownload" runat="server" Visible="false" 
                    onclick="LnkPDFDownload_Click" Font-Bold="True" Font-Size="Small" 
                        ForeColor="#006600">Download MCR</asp:LinkButton>
                </p>

                                
                    
            </fieldset>
        </div>
    </div>
</asp:Content>

