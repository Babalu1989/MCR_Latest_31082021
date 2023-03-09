<%@ Page Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="HomePage.aspx.cs" Inherits="_Default" Title="Home Page" ClientIDMode="Static" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

<script type="text/javascript">
    function showProgress() {
        var updateProgress = $get("<%= UpdateProgress.ClientID %>");
        updateProgress.style.display = "block";
    }
</script>
<style type="text/css">

.modal
{
    position: fixed;
    z-index: 999;
    height: 100%;
    width: 100%;
    top: 0;
    background-color: Black;
    filter: alpha(opacity=60);
    opacity: 0.6;
    -moz-opacity: 0.8;
}
.center
{
    z-index: 1000;
    margin: 300px auto;
    padding: 10px;
    width: 130px;
    background-color: White;
    border-radius: 10px;
    filter: alpha(opacity=100);
    opacity: 1;
    -moz-opacity: 1;
}
.center img
{
    height: 128px;
    width: 128px;
}
</style>

    <%--<script type="text/javascript" src="https://www.google.com/jsapi"></script>--%>
    <script src="Script/jsapi.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server"> </asp:ScriptManager>
    <link href="StyleSheet/Login.css" rel="stylesheet" type="text/css" />
    <div class="divclass" style="height: auto">
        <table width="100%" style="height: 300px; vertical-align: middle">
            <tr>
                <td align="center" style="width: 50%">
                    <span style="font-size: 22px; font-style: italic; font-weight: bold; color: Red">Welcome
                        To Meter Allocation & Monitoring System</span>                   
                </td>
                <td align="center" style="width: 50%">
                    <br />
                    <table style="border: 1px solid black; font-weight: bold; width: 60%">
                        <tr>
                            <td colspan="2">
                                <h4 style="background: #3986DD; color: #ffffff; padding: 5px; font-size: 16px; font-style: italic;
                                    font-weight: 100; display: block;">
                                    User information</h4>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Name
                            </td>
                            <td>
                                <asp:Label ID="lblName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                User ID
                            </td>
                            <td>
                                <asp:Label ID="lblEmpCode" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               Role
                            </td>
                            <td>
                                <asp:Label ID="lblEmployeeType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="TRDiv_Row" runat="server">
                            <td>
                               Division
                            </td>
                            <td>
                                <asp:Label ID="lblDivision" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr id="tr1" runat="server" visible="false">
                            <td>
                                Company
                            </td>
                            <td>
                                <asp:Label ID="lblCompany" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                 <asp:UpdatePanel runat="server" ID="updatepanel1" UpdateMode="Conditional">
                <ContentTemplate>
                &nbsp;
                    <asp:LinkButton ID="LnkGrapgh" runat="server" Font-Bold="True" 
                        onclick="LnkGrapgh_Click"  OnClientClick="showProgress()">To View Graph, Please Click Here...</asp:LinkButton>
                        </ContentTemplate>
          <Triggers>
                <asp:PostBackTrigger ControlID="LnkGrapgh" />                          
                </Triggers>
        </asp:UpdatePanel>
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <asp:Literal ID="ltScripts" runat="server"></asp:Literal>
                    <div id="chart_div" style="width: 100%; height: 200px;">
                    </div>
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="center" style="width: 50%">
                                <asp:Literal ID="ltScripts_meter" runat="server"></asp:Literal>
                                <div id="chart_Meter" style="width: 100%; height: 200px;">
                                </div>
                            </td>
                            <td align="center" style="width: 50%">
                                <asp:Literal ID="ltScripts_Seal" runat="server"></asp:Literal>
                                <div id="chart_Seal" style="width: 100%; height: 200px;">
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

           <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel1">
                    <ProgressTemplate>                
             <div class="modal" style="clear:both; text-align:center; width:100%;">
              <div class="center" style="text-align:center;margin-top:170px;">
                <img alt="" src="Image/pleasewait.gif" />
                </div>
                </div>
                </ProgressTemplate>
                </asp:UpdateProgress>

    </div>
</asp:Content>
