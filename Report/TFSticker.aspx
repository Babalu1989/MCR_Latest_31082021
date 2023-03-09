<%@ Page Language="C#" MasterPageFile="SiteMaster.master" AutoEventWireup="true"
    ClientIDMode="Static" CodeFile="TFSticker.aspx.cs" Inherits="_Default" Title="TF Sticker Report" %>

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
     <style type="text/css">
        .auto-style1 {
            font-size: small;
        }
    </style>
 
    <style type="text/css">
        .auto-style2 {
            font-size: small;
        }
        .auto-style3
        {
            font-size: small;
            color: #FFFFCC;
        }
    </style>

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
                            TF Sticker Report</h2>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="90%" style="border: 1px solid black;">
                            <tr>
                                <td colspan="4" align="left">
                                    <u><b>Search Criteria</b></u>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Kitting From Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" CssClass="textarea" ReadOnly="true" runat="server"
                                        Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtFromDate" Format="dd mmm yyyy" />
                                </td>
                                <td>
                                    Kitting To Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" CssClass="textarea" ReadOnly="true" runat="server" Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtToDate" Format="dd mmm yyyy" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    CA NO
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCANO" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                </td>
                                <td>
                                    Meter No
                                </td>
                                <td>
                                <asp:TextBox ID="txtMeterNo" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id = "trOrderType" runat = "server" visible = "false">
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
                                        Width="150px" OnClick="btnSave_Click"  OnClientClick="showProgress()"/>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Refresh" CssClass="submit" Height="30px"
                                        Width="150px" OnClick="btnCancel_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="submit"
                                        Height="30px" Width="150px" onclick="btnExit_Click" />
                                         </ContentTemplate>
                                      <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSave" />                          
                                            </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="height: 15px">
                                    <asp:Label ID="lblRoleCheck" runat="server" Visible="False"></asp:Label>
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
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">

                        <div id="GridView1_div"  runat="server" style="width:100%;"   >
                    <div id="GHead"></div>
                        <div style="overflow: scroll; width: 99.9%; max-height: 370px">

                            <asp:GridView ID="gvMainData" AllowSorting="True" runat="server"
                                Width="100%" AutoGenerateColumns="False" OnSorting="gvMainData_Sorting" 
                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                CellSpacing = "2" GridLines="None">
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                 <asp:TemplateField HeaderText="S.No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DIVISION" HeaderText="Division" 
                                        ItemStyle-HorizontalAlign="Left"  SortExpression="DIVISION">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                     <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Code" 
                                        ItemStyle-HorizontalAlign="Left"  SortExpression="VENDOR_CODE">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VENDOR" HeaderText="Vendor Name" 
                                        ItemStyle-HorizontalAlign="Left"  SortExpression="VENDOR">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CA_NO" HeaderText="CA No" 
                                        ItemStyle-HorizontalAlign="Left"  SortExpression="CA_NO">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="METER_NO" HeaderText="Meter No" 
                                        ItemStyle-HorizontalAlign="Left"  SortExpression="METER_NO">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Sticker_Found" HeaderText="TF Sticker Found at site" 
                                        ItemStyle-HorizontalAlign="Center" SortExpression="STICKER" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Sticker_NO" HeaderText="Sticker No" 
                                        ItemStyle-HorizontalAlign="Left" SortExpression="OTHERSTICKER">

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                     <asp:BoundField DataField="ORDER_TYPE" HeaderText="Order Type" ItemStyle-HorizontalAlign="Left"
                                         SortExpression="ORDER_TYPE" HeaderStyle-Font-Underline="true" 
                                        HeaderStyle-Width="15%" ItemStyle-Width="15%" >
<HeaderStyle Font-Underline="True" Width="15%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                    </asp:BoundField>
                                        <asp:BoundField DataField="PM_ACT" HeaderText="PM Activity" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="PM_ACT" HeaderStyle-Font-Underline="true" 
                                        HeaderStyle-Width="15%" ItemStyle-Width="15%">

<HeaderStyle Font-Underline="True" Width="15%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                                    </asp:BoundField>

                                           <asp:BoundField DataField="OLD_M_READING" HeaderText="Old_M_Read" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="OLD_M_READING" HeaderStyle-Font-Underline="true" 
                                        Visible="false">                                         
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="OLD_MR_KW" HeaderText="Old_Mr_KW" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="OLD_MR_KW" HeaderStyle-Font-Underline="true" 
                                        Visible="false" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="MRKVAH_OLD" HeaderText="MRKVAH_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="MRKVAH_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="OLD_MR_KVA" HeaderText="Old_Mr_KVA" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="OLD_MR_KVA" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="INSTALLEDCABLE_OLD" 
                                        HeaderText="Inst_Cable_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="INSTALLEDCABLE_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="CABLESIZE_OLD" HeaderText="Cable_Size_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="CABLESIZE_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="DRUMSIZE_OLD" HeaderText="DrumSize_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="DRUMSIZE_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="CABLEINSTALLTYPE_OLD" 
                                        HeaderText="Cable_Inst_Type_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="CABLEINSTALLTYPE_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="RUNNINGLENGTHFROM_OLD" 
                                        HeaderText="Run_Len_Frm_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="RUNNINGLENGTHFROM_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="RUNNINGLENGTHTO_OLD" 
                                        HeaderText="Run_Len_To_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="RUNNINGLENGTHTO_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="CABLELENGTH_OLD" HeaderText="Cable_len_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="CABLELENGTH_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="OUTPUTBUSLENGTH_OLD" 
                                        HeaderText="O/P_Bus_len_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="OUTPUTBUSLENGTH_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="REM_TERMINAL_SEAL" 
                                        HeaderText="Rem_Terminal_Seal" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="REM_TERMINAL_SEAL" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="REM_OTHER_SEAL" HeaderText="Rem_Other_Seal" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="REM_OTHER_SEAL" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="REM_BOX_SEAL1" HeaderText="Rem_Box_Seal1" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="REM_BOX_SEAL1" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="REM_BOX_SEAL2" HeaderText="Rem_Box_Seal2" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="REM_BOX_SEAL2" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="REM_BUSBAR_SEAL1" 
                                        HeaderText="Rem_Bus_Bar_Seal1" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="REM_BUSBAR_SEAL1" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="REM_BUSBAR_SEAL2" 
                                        HeaderText="Rem_Bus_Bar_Seal2" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="REM_BUSBAR_SEAL2" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                           <asp:BoundField DataField="BOX_OLD" HeaderText="Box_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="BOX_OLD" HeaderStyle-Font-Underline="true" Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                          <asp:BoundField DataField="GLANDS_OLD" HeaderText="Glands_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="GLANDS_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                          <asp:BoundField DataField="TCOVER_OLD" HeaderText="Tcover_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="TCOVER_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                          <asp:BoundField DataField="BRASSSCREW_OLD" HeaderText="Brass_Screw_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="BRASSSCREW_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                          <asp:BoundField DataField="BUSBAR_OLD" HeaderText="Bus_Bar_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="BUSBAR_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                          <asp:BoundField DataField="THIMBLE_OLD" HeaderText="Thimble_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="THIMBLE_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                          <asp:BoundField DataField="SADDLE_OLD" HeaderText="Saddle_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="SADDLE_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                              <asp:BoundField DataField="GUNNYBAG_OLD" HeaderText="Gunny_Bag_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="GUNNYBAG_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                              <asp:BoundField DataField="GUNNYBAGSEAL_OLD" 
                                        HeaderText="Gunny_Bag_Seal_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="GUNNYBAGSEAL_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                              <asp:BoundField DataField="LABTESTING_DATE_OLD" 
                                        HeaderText="LabTest_DT_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="LABTESTING_DATE_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                         <asp:BoundField DataField="METERRELOCATE_OLD" 
                                        HeaderText="Mtr_Relocate_Old" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="METERRELOCATE_OLD" HeaderStyle-Font-Underline="true" 
                                        Visible="false">

<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <HeaderStyle HorizontalAlign="Center" Height="35px" Font-Underline="true" 
                                    BackColor="#008dde" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#000065" />
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
