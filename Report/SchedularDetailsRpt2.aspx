<%@ Page Title="" Language="C#" MasterPageFile="~/Report/SiteMaster.master" AutoEventWireup="true" CodeFile="SchedularDetailsRpt2.aspx.cs" Inherits="Report_SchedularDetailsRpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table style="text-align: center" width="100%">
                <tr>
                    <td style="height: 15px">
                        <h2>
                            Schedular Details Report</h2>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="100%" style="border: 1px solid black;">
                            <tr>
                                <td colspan="4" align="left">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    Alloted Cases from SAP
                                </td>
                                <td>
                                    <asp:Label ID="lblMaxDateRpt1" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                   Alloted Seal from SAP
                                </td>
                                <td>
                                    <asp:Label ID="lblMaxDateRpt2" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Allocated Case to Installer
                                </td>
                                <td>
                                   <asp:Label ID="lblMaxDateRpt3" runat="server" Font-Bold="True"></asp:Label>
                                   </td>
                                   <td>Punched Cases through TAB</td>
                                   <td> <asp:Label ID="lblMaxDateRpt4" runat="server" Font-Bold="True"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    Cases from TAB to SAP
                                </td>
                                <td>
                                   <asp:Label ID="lblMaxDateRpt5" runat="server" Font-Bold="True"></asp:Label>
                                   </td>
                                   <td></td>
                                   <td> </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <br />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="refresh" CssClass="submit" Height="30px"
                                        Width="150px" OnClick="btnCancel_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="submit" Height="30px"
                                        Width="150px" OnClick="btnExit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="height: 15px">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>                              
                </table>
</asp:Content>

