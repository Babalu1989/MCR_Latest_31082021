<%@ Page Title="Seal Allocation Transfer" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" 
CodeFile="SealAllotmentTransfer.aspx.cs" Inherits="SealAllotmentTransfer" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        function RadioCheck(rb) {
            var gv = document.getElementById("<%=gvSeriesWiseAllocation.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }

            var updateProgress = $get("<%= UpdateProgress.ClientID %>");
            updateProgress.style.display = "block";
        }    
    </script>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="divclass">
        <center>
            <table id="table_Series_Wise_Allocation" runat="server" width="95%">
                <tr>
                    <td colspan="2" style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%" align="center">

                    <table width="100%">
                    <tr>
                    <td>Division</td>
                    <td> 
                    
                    <asp:DropDownList ID="ddlDivision" runat="server" Width="130px" 
                                        AutoPostBack="True" onchange="showProgress()"
                            onselectedindexchanged="ddlDivision_SelectedIndexChanged" >
                                    <asp:ListItem>-ALL-</asp:ListItem>
                                    </asp:DropDownList>
                                    
                    </td>
                    <td>Vendor</td>
                    <td>  
                    <table>
                    <tr>
                    <td>
                    <asp:DropDownList ID="ddlVendor" runat="server" Width="210px">
                                    </asp:DropDownList></td>
                    <td>
                   
                         <asp:Button ID="btnViewData" runat="server" Text="View Data" CssClass="submit" 
                                        Height="30px" Visible="false" Width="85px" OnClientClick="showProgress()" OnClick="btnViewData_Click" />

                    </td>
                    </tr>
                    </table>
                    
                                    
                                    </td>
                    </tr>
                    <tr>
                    <td colspan="4">
                        <div style="background-color:lightgray; overflow: scroll; width: 100%; height: 310px">

                         <table width="100%" border="1" cellpadding="0" cellspacing="0" style="border-style:solid"  runat="server" id="BlankHeader">
                         <tr style="height:40px">
                         <td><b>&nbsp;Select</b></td>
                         <td><b>&nbsp;Installer Name</b></td>
                         <td><b>&nbsp;Installer ID</b></td>
                         <td><b>&nbsp;Previous seals available with Installer</b></td>                    
                         </tr>
                         </table>


                            <asp:GridView ID="gvSeriesWiseAllocation" runat="server" CssClass="gvwWhite" AutoGenerateColumns="false"
                                Width="99%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdbtnseriesSelect" AutoPostBack="true" OnCheckedChanged="rdbdiscontinued_CheckedChanged"
                                                runat="server" CssClass="checkbox"  onclick="RadioCheck(this);" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Installer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEMPNAME" runat="server" Text='<%#Eval("EMPNAME")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Installer ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInstallerID" runat="server" Text='<%#Eval("EMP_ID")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Previous seals available with Installer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPSeal" runat="server" Text='<%#Eval("SEALALLOTED")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle Height="40px" />
                            </asp:GridView>
                        </div>
                    </td>
                    </tr>
                    </table>
                    
                    </td>
                    <td align="center">
                        <asp:Chart ID="Chart_Meter" runat="server" Height="270px" Width="480px">
                            <Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                        <div>
                            <asp:Label ID="lblMapTitle" runat="server" Text="Seal Reconciliation" 
                                Font-Bold="True"></asp:Label>
                            
                            </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 15px">
                    </td>
                </tr>
                <tr id="tr1" runat="server" visible="false">
                    <td colspan="2">
                        <div style="background-color: #DADADA; height: 30px; vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;Installer wise Allotements Transfer Details
                            </div>
                        </div>
                    </td>
                </tr>
               
                <tr  id="tr3" runat="server" visible="false">
                    <td align="center" colspan="2">
                        <table width="100%" style="border: 1px solid black;">
                            <tr>
                                <td colspan="4" style="height: 5px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">                                    
                                    <div style="background-color: #DADADA; height: 25px; vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;<strong>Transfer From</strong>
                            </div>
                        </div>
                                    </td>
                              
                            </tr>
                            <tr>
                                <td style="height:20px;">
                                    <b>Installer
                                    Name</b>
                                </td>
                                <td>
                                    <asp:Label ID="txtName" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <b>Installer ID</b></td>
                                <td>
                                    <asp:Label ID="txtEmployeeID" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                 <td style="height:20px;">
                                    <b>Seal No.</b>
                                </td>
                                <td>
                                   <asp:Button ID="btnChecked" runat="server" Text="Checked ALL" CssClass="submit"
                                        Height="30px" Width="120px" onclick="btnChecked_Click"  />
                                 </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                 <td colspan="4">
                                    <div style="background-color:lightgary; overflow: scroll; width: 100%; height: 100px">                                        
                                        <asp:CheckBoxList ID="chboxListSealNO" runat="server" Width="100%" 
                                            RepeatColumns="10" RepeatDirection="Horizontal" Font-Bold="True" 
                                            BackColor="#FFFF99">
                                        </asp:CheckBoxList>
                                    </div>
                                 </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    
                                    <div style="background-color: #DADADA; height: 25px; vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;<strong>Transfer To</strong>
                            </div>
                        </div>
                                    </td>
                               
                            </tr>
                            <tr>
                                 <td style="height:20px;">
                                    Installer Name</td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlInstaller" runat="server" Width="200px" 
                                        AutoPostBack="True" onselectedindexchanged="ddlInstaller_SelectedIndexChanged">
                                    <asp:ListItem>-SELECT-</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                 <td>
                                    Installer ID</td>
                                <td colspan="2">
                                    <asp:Label ID="lblInstallerID" runat="server"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                           
                            <tr>
                                <td colspan="4" align="center">
                                    
                                    <asp:Button ID="btnSubmitSealAllocation" runat="server" Text="Seal Transfer" CssClass="submit"
                                        Height="30px" Visible="false" Width="180px" OnClick="btnSubmitSealAllocation_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnCancelSealAllocation" runat="server" Text="Cancel" CssClass="submit"
                                        Height="30px" Visible="false" Width="180px" OnClick="btnCancelSealAllocation_Click" />
                                </td>
                            </tr>
                        </table>
                     
                    </td>
                </tr>
            </table>
        </center>

         <asp:UpdateProgress ID="UpdateProgress" runat="server">
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

