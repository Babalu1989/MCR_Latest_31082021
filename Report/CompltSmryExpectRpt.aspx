<%@ Page Title="Completed Meter Installation Expectional Report" Language="C#" MasterPageFile="SiteMaster.master" AutoEventWireup="true" CodeFile="CompltSmryExpectRpt.aspx.cs" 
 Inherits="Report_CompltSmryExpectRpt" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=gvMainData.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=gvMainData.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=gvMainData.ClientID%>').offset().top);

        });
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="divclass">
        <center>
            <table style="text-align: center" width="90%">
                <tr>
                    <td style="height: 15px">
                        <h2>
                            Completed Meter Installation Exceptional Report</h2>
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
                                    <u><b>Search Criteria</b></u>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Division
                                </td>
                                <td>
                                    <asp:DropDownList ID="txtDivision" runat="server" CssClass="textarea" AutoPostBack="true"
                                        Width="190px" OnSelectedIndexChanged="txtDivision_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Vendor Name
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlVendorName" runat="server" CssClass="textarea" Width="190px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Activity Date From
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" CssClass="textarea" ReadOnly="true" runat="server"
                                        Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtFromDate" Format="dd mmm yyyy" />
                                </td>
                                <td>
                                    &nbsp;Activity Date To
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" CssClass="textarea" ReadOnly="true" runat="server" Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtToDate" Format="dd mmm yyyy" />
                                </td>
                            </tr>
                             <tr>
                                    <td>
                                        Order Type</td>
                                    <td>
                                         <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="textarea" 
                                             Width="190px" AutoPostBack="True" 
                                             onselectedindexchanged="ddlOrderType_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                    <td>
                                       PM Activity</td>
                                    <td>
                                       <asp:DropDownList ID="ddlPMActivity" runat="server" CssClass="textarea" Width="190px">
                                       <asp:ListItem>-ALL-</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                               
                            <tr>
                                <td colspan="4" align="center">
                                    <br />
                                     <asp:UpdatePanel runat="server" ID="updatepanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                    <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="submit" Height="30px"
                                        Width="150px" OnClick="btnSave_Click" /> <%-- OnClientClick="showProgress()"--%>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Refresh" CssClass="submit" Height="30px"
                                        Width="150px" OnClick="btnCancel_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="submit" Height="30px"
                                        Width="150px" OnClick="btnExit_Click" />
                                         </ContentTemplate>
                                      <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSave" />                          
                                            </Triggers>
                                    </asp:UpdatePanel>
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
                <tr>
                    <td style="height: 30px" align="right">
                        <asp:Label ID="lblTotCunt" runat="server"  Visible="false" Font-Bold="true"></asp:Label>
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
            </table>
            <table style="text-align: center" width="100%">
                <tr>
                    <td align="center">
                          <div id="GridView1_div" runat="server" style="width: 100%;">
                            <div id="GHead">
                            </div>
                            <div style="width: 99.9%; height: 360px; overflow: auto;">
                            <asp:GridView ID="gvMainData" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                Width="100%" AutoGenerateColumns="false">
                                 <Columns>
                                    <asp:BoundField DataField="COMPANY_CODE" HeaderText="Company" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SUB_DIVISION" HeaderText="Sub Div" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="CA_NO" HeaderText="CA No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ORDER_NO" HeaderText="Order No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ORDER_TYPE" HeaderText="Order Type" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ACCOUNT_CLASS" HeaderText="Account Class" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BASIC_START_DATE" HeaderText="Basic Start Date" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BASIE_FINISH_DATE" HeaderText="Basic Finish Date" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="FATHER_NAME" HeaderText="Father Name" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ADDRESS" HeaderText="Address" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MOBILE_NO" HeaderText="Mobile No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BP_NO" HeaderText="BP No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="POLE_NO" HeaderText="Pole No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="CABLE_SIZE" HeaderText="Cable Size" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="CABLE_LENGTH" HeaderText="Cable Length" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="PLANNER_GROUP" HeaderText="Planner Group" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ACTIVITY_REASON" HeaderText="Activity Reason" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ACTIVITY_DATE" HeaderText="Activity Date" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="METER_NO" HeaderText="Meter No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Old_Meter" HeaderText="Old Meter No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="OTHERSTICKER" HeaderText="Sticker No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ELCB_INSTALLED" HeaderText="ELCB Installed" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BUS_BAR_INSTALLED" HeaderText="Bus Bar Installed" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BUS_BAR_TYPE" HeaderText="Bus Bar Type" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BUS_BAR_NO" HeaderText="Bus Bar No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BUS_BAR_CABLE_SIZE" HeaderText="Bus Bar Cable Size" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BUS_BAR_CABLE_LENG" HeaderText="Bus Bar Cable Length" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BUS_BAR_DRUM_NO" HeaderText="Bus Bar Drum No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="CABLE_INSTALLED" HeaderText="Cable Installed" ItemStyle-HorizontalAlign="Left" />                                    
                                    <asp:BoundField DataField="CABLE_DRUM_NO" HeaderText="Cable Drum No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="RUNNING_LENGTH_L1" HeaderText="Running Length L1" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="RUNNING_LENGTH_L2" HeaderText="Running Length L2" ItemStyle-HorizontalAlign="Left" />                                    
                                    <asp:BoundField DataField="OUTPUT_CABLE_LENGTH" HeaderText="Output Cable Length" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="CABLE_OH_UG" HeaderText="Cable OH/UG" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="TERMINAL_SEAL_1" HeaderText="Terminal Seal 1" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="TERMINAL_SEAL_2" HeaderText="Terminal Seal 2" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BOX_SEAL_1" HeaderText="Box seal 1" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BOX_SEAL_2" HeaderText="Box Seal 2" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BUS_BAR_SEAL_1" HeaderText="Bus Bar Seal 2" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BUS_BAR_SEAL_2" HeaderText="Bus Bar Seal 2" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="METER_INSTALLED_LOCATION" HeaderText="Meter Installation Location" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="POLE_NO_TAB" HeaderText="Pole No (Tab)" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="GIS_LONGITUDE" HeaderText="Longitude" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="GIS_LATITUDE" HeaderText="Latitude" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MR_KWH" HeaderText="Meter KWH" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MR_KW" HeaderText="Meter KW" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MR_KVAH" HeaderText="Meter KVAH" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="MR_KVA" HeaderText="Meter KVA" ItemStyle-HorizontalAlign="Left" />
                                   <%-- <asp:BoundField DataField="IMAGE1" HeaderText="Image 1" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="IMAGE2" HeaderText="Image 2" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="IMAGE3" HeaderText="Image 3" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="IMEAGE_MCR" HeaderText="MCR Image" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="IMAGE_METERTESTREPORT" HeaderText="Meter Image Testing Report" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="IMAGE_LABTESTINGREPORT" HeaderText="Meter Image Lab Testing Report" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="IMAGE_SIGNATURE" HeaderText="Consumer Signature" ItemStyle-HorizontalAlign="Left" />  --%>                                  
                                    <asp:BoundField DataField="ANCHOR_POLE_END_QTY" HeaderText="Pole End Qty" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ANCHOR_CONSUMER_END_QTY" HeaderText="Cons End Qty" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="L_ANGLE" HeaderText="L_ANGLE" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="I_ANGLE" HeaderText="I_ANGLE" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="PIPE_POLE_END_QTY" HeaderText="PIPE_POLE_END_QTY" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="POLE_END" HeaderText="POLE_END" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ACCES_GLAND_QTY" HeaderText="Acces Gld Qty" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ACCES_EARTH_NUT_QTY" HeaderText="Acces Earth Qty" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ACCES_THIMBLE_QTY" HeaderText="Acces Thim Qty" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ACCES_SADDLE_CLAMP_QTY" HeaderText="Acces Saddle Qty" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="PUNCH_BY" HeaderText="Punch By" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="PUNCH_DATE" HeaderText="Punch Date" ItemStyle-HorizontalAlign="Left" />

                                    <asp:BoundField DataField="ACTIVITY_TYPE" HeaderText="Activity Type" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="PM_ACTIVITY" HeaderText="PM Activity" ItemStyle-HorizontalAlign="Left" />                                                                                                                                                                                                                                                                                                                                                                                                                                               
                                    <asp:BoundField DataField="ASSIGNED_VENDOR_DT" HeaderText="Assigned Date" ItemStyle-HorizontalAlign="Left" />                                                                                                                                                                                                                                                                                                                                                                                                          
                                    <asp:BoundField DataField="CABLENOTINSTALLREASON" HeaderText="Reason for Not Remove Cable" ItemStyle-HorizontalAlign="Left" />                                                                        
                                    <asp:BoundField DataField="LM_CUSTOMERPOLE" HeaderText="Cust. Pole" ItemStyle-HorizontalAlign="Left" />                                                                        
                                    <asp:BoundField DataField="SMARTMETERBOOL" HeaderText="Smart Meter Bool" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SMARTMETERSIMNO" HeaderText="Smart Meter No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SMARTMETERSIMCODE" HeaderText="Smart Meter Code" ItemStyle-HorizontalAlign="Left" />

                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" Height="35px" />
                            </asp:GridView>
                        </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
            </table>
        </center>
        <div style="clear:both;"></div>
         <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel1">
                <ProgressTemplate>                
             <div class="modal" style="clear:both; text-align:center; width:100%;">
              <div class="center" style="text-align:center;margin-top:170px;">
                <img alt="" src="../Image/pleasewait.gif" />
                </div>
                </div>
                </ProgressTemplate>
                </asp:UpdateProgress>

    </div>
</asp:Content>

