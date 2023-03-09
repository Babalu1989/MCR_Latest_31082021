<%@ Page Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    ClientIDMode="Static" CodeFile="MCRPunching.aspx.cs" Inherits="_Default" Title="Order Allocation"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        function RadioCheck(rb) {
            var gv = document.getElementById("<%=gvEmpDetails.ClientID%>");
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
        }    
    </script>
    <script src="Script/jquery-1.9.1.js" type="text/javascript"></script>
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
        .style1
        {
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="divclass">
        <center>
            <table width="95%">
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:RadioButtonList ID="rdbtnList" AutoPostBack="true" Width="600px" runat="server"
                            RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbtnList_SelectedIndexChanged">
                            <asp:ListItem Text="Pending For Allocation" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Allotted Cases" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Completed Cases" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Cancelled Cases" Value="4"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="tr1" runat="server">
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div>
                            <table width="90%" style="border: 1px solid black;">
                                <tr>
                                    <td colspan="4" align="left">
                                        <u><b>Search Criteria</b></u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Meter No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMeterNO" CssClass="textarea" runat="server" Width="251px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Service Order No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtServiceOrdNo" CssClass="textarea" runat="server" Width="251px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Division
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="txtDivision" runat="server" CssClass="textarea" 
                                            Width="252px" onselectedindexchanged="txtDivision_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Vendor Name
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlVendorName" runat="server" CssClass="textarea" Width="252px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Basic Finish Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBasicFinishDate" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="220px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtBasicFinishDate"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFromDate" runat="server" Text="Kitting From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPostingDate" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="220px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPostingDate"
                                            Format="dd mmm yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblToDate" runat="server" Text="Kitting To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPostingToDate" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="220px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtPostingToDate"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        Address
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAddess" runat="server" Style="resize: none" TextMode="MultiLine"
                                            Width="59%" Height="40px" CssClass="textarea"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr2" runat="server" visible="false">
                                    <td>
                                        Installer Name
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlInstallerName" runat="server" CssClass="textarea" Width="252px">
                                            <asp:ListItem Value="0">-Select One-</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr id="trOrderType" runat="server" visible="false">
                                    <td>
                                        Order Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="textarea" Width="252px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        PM Activity
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPMActivity" runat="server" CssClass="textarea" Width="252px">
                                            <asp:ListItem>-ALL-</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <br />
                                        <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnSave_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Refresh" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnCancel_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="submit" Height="30px"
                                            Visible="false" Width="150px" OnClick="btnExit_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 15px; text-align: left;" class="style1">
                                        <strong>Note:- Please Click on Show Button, to View Data.</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 15px">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px">
                        <asp:Label ID="lblRoleCheck" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 30">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <span style="color: Red">Total Cases :
                                        <asp:Label ID="lblTotalCase" runat="server" />
                                        |Selected Case :
                                        <asp:Label ID="lblSelectedCase" Text="0" runat="server" />
                                    </span>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                                        Width="80px" Visible="false" OnClick="btnExcel_Click1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="Div1" runat="server" style="width: 100%;">
                            <div id="GHead">
                            </div>
                            <div style="overflow: scroll; width: 100%; max-height: 270px">
                                <asp:GridView EmptyDataText="No Record Found" ID="gvMainData" AllowSorting="True"
                                    runat="server" Width="100%" AutoGenerateColumns="False" OnSorting="gvMainData_Sorting"
                                    OnSelectedIndexChanged="gvMainData_SelectedIndexChanged" BackColor="White" BorderColor="#999999"
                                    BorderStyle="None" BorderWidth="1px" CellSpacing="2" GridLines="None">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lkSelectAll" runat="server" OnClick="lkSelectAll_Click" ForeColor="White">Select All</asp:LinkButton>
                                                <%--<asp:CheckBox ID="chkb1" runat="server" OnCheckedChanged="sellectAll" AutoPostBack="true" />--%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%--<asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="sellectOne" AutoPostBack="true" />--%>
                                                <asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="sellectOne" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Id" HeaderStyle-Font-Underline="true" SortExpression="ORDERID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderID" runat="server" Text='<%#Eval("ORDERID")%>' Visible="false" />
                                                <asp:Label ID="ORDERID" runat="server" Text='<%#Eval("ORDERID")%>' Visible="true" />
                                            </ItemTemplate>
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DIVISION" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CA_NO" HeaderText="CA No" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CA_NO" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Meter No" HeaderStyle-Font-Underline="true" SortExpression="METER_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMeterNO" runat="server" Text='<%#Eval("METER_NO")%>' Visible="false" />
                                                <asp:Label ID="lblMobNO" runat="server" Text='<%#Eval("TEL_NO")%>' Visible="false" />
                                                <asp:Label ID="METER_NO" runat="server" Text='<%#Eval("METER_NO")%>' Visible="true" />
                                                <asp:Label ID="lbl_AUART" runat="server" Text='<%#Eval("AUART")%>' Visible="false" />
                                                <asp:Label ID="lbl_ILART_ACTIVITY_TYPE" runat="server" Text='<%#Eval("ILART_ACTIVITY_TYPE")%>'
                                                    Visible="false" />
                                                <asp:Label ID="lblDiv" runat="server" Text='<%#Eval("DIVISION")%>' Visible="false" />
                                            </ItemTemplate>
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PLANNER_GROUP" HeaderText="Planner Group" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PLANNER_GROUP" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ACCOUNT_CLASS" HeaderText="Account Class" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ACCOUNT_CLASS" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SANCTIONED_LOAD" HeaderText="Sanction Load" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="SANCTIONED_LOAD" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="POSTINGDATE" HeaderText="Kitting Date" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="POSTINGDATE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FIN_DATE" HeaderText="Basic Finish Date" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FINISH_DATE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="NAME" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ADDRESS" HeaderText="Address" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ADDRESS" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" Width="200px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PUNCHDATE" HeaderText="Meter Execution Date" ItemStyle-HorizontalAlign="Left"
                                            Visible="false" SortExpression="PUNCHDATE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ALLOTEDTO" HeaderText="Allotted To" ItemStyle-HorizontalAlign="Left"
                                            Visible="false" SortExpression="ALLOTEDTO" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cancel_Reason" HeaderText="Cancel Reason" ItemStyle-HorizontalAlign="Left"
                                            Visible="false" SortExpression="Cancel_Reason" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORDER_TYPE" HeaderText="Order Type" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ORDER_TYPE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PM_ACT" HeaderText="PM Activity" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PM_ACT" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OLD_M_READING" HeaderText="Old Meter Reading" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="OLD_M_READING" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OLD_MR_KW" HeaderText="Old Meter KW" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="OLD_MR_KW" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MRKVAH_OLD" HeaderText="Old Meter KVAH" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="MRKVAH_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OLD_MR_KVA" HeaderText="Old Meter KVA" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="OLD_MR_KVA" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="INSTALLEDCABLE_OLD" HeaderText="Inst_Cable_Old" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="INSTALLEDCABLE_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CABLESIZE_OLD" HeaderText="Old Cable Size" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CABLESIZE_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DRUMSIZE_OLD" HeaderText="Old Drum Size" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DRUMSIZE_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CABLEINSTALLTYPE_OLD" HeaderText="Cable_Inst_Type_Old"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="CABLEINSTALLTYPE_OLD" HeaderStyle-Font-Underline="true"
                                            Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RUNNINGLENGTHFROM_OLD" HeaderText="Old Running Length From"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="RUNNINGLENGTHFROM_OLD" HeaderStyle-Font-Underline="true"
                                            Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RUNNINGLENGTHTO_OLD" HeaderText="Old Running Length To"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="RUNNINGLENGTHTO_OLD" HeaderStyle-Font-Underline="true"
                                            Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CABLELENGTH_OLD" HeaderText="Old Cable Length" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CABLELENGTH_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OUTPUTBUSLENGTH_OLD" HeaderText="Old O/P Bus Length" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="OUTPUTBUSLENGTH_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REM_TERMINAL_SEAL" HeaderText="Removed Terminal Seal"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="REM_TERMINAL_SEAL" HeaderStyle-Font-Underline="true"
                                            Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REM_OTHER_SEAL" HeaderText="Removed Other Seal" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="REM_OTHER_SEAL" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REM_BOX_SEAL1" HeaderText="Removed Box Seal1" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="REM_BOX_SEAL1" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REM_BOX_SEAL2" HeaderText="Removed Box Seal2" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="REM_BOX_SEAL2" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REM_BUSBAR_SEAL1" HeaderText="Removed Bus Bar Seal1" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="REM_BUSBAR_SEAL1" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REM_BUSBAR_SEAL2" HeaderText="Removed Bus Bar Seal2" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="REM_BUSBAR_SEAL2" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BOX_OLD" HeaderText="Old Box" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="BOX_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GLANDS_OLD" HeaderText="Old Glands" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="GLANDS_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TCOVER_OLD" HeaderText="Old TCover" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="TCOVER_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BRASSSCREW_OLD" HeaderText="Old Brass Screw" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="BRASSSCREW_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BUSBAR_OLD" HeaderText="Old Bus Bar" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="BUSBAR_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="THIMBLE_OLD" HeaderText="Old Thimble" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="THIMBLE_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SADDLE_OLD" HeaderText="Old Saddle" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="SADDLE_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GUNNYBAG_OLD" HeaderText="Old Gunny Bag" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="GUNNYBAG_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GUNNYBAGSEAL_OLD" HeaderText="Old Gunny Bag Seal" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="GUNNYBAGSEAL_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LABTESTING_DATE_OLD" HeaderText="Old LabTest Date" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="LABTESTING_DATE_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="METERRELOCATE_OLD" HeaderText="Old Meter Relocate" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="METERRELOCATE_OLD" HeaderStyle-Font-Underline="true" Visible="false">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CONTRACT_SD" HeaderText="Contract SD" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CONTRACT_SD" HeaderStyle-Font-Underline="true" Visible="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CONTRACT_ED" HeaderText="Contract ED" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CONTRACT_ED" HeaderStyle-Font-Underline="true" Visible="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DURATION" HeaderText="Duration" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DURATION" HeaderStyle-Font-Underline="true" Visible="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:CommandField ShowSelectButton="true" SelectText="Revert Process" Visible="false" />
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <HeaderStyle HorizontalAlign="Center" Height="35px" BackColor="#008dde" ForeColor="White"
                                        Font-Bold="True" />
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
                <tr id="tr3" runat="server">
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr id="tr6" runat="server">
                    <td align="center">
                        <div style="background-color: #DADADA; height: 30px; vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;Installer wise Allotements Details
                            </div>
                        </div>
                        <br />
                        <div style="width: 100%" id="divInstallerList" runat="server" visible="false">
                            <%--<asp:CheckBox ID="chkb1" runat="server" OnCheckedChanged="sellectAll" AutoPostBack="true" />--%>
                            <table width="95%">
                                <tr>
                                    <td style="width: 50%">
                                        <div style="overflow: scroll; width: 100%; height: 350px">
                                            <asp:GridView ID="gvEmpDetails" runat="server" CssClass="gvwWhite" AutoGenerateColumns="false"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:RadioButton ID="rdbtnSelect" runat="server" CssClass="checkbox" onclick="RadioCheck(this);" />
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
                                                    <asp:TemplateField HeaderText="Pending Cases">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMeterAlloted" runat="server" Text='<%#Eval("MeterAlloted")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="SealAlloted" HeaderText="Pending Seals" ItemStyle-HorizontalAlign="Center" />
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Center" Height="35px" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td style="width: 50%">
                                        <table style="vertical-align: top;">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="LnkGrapgh" runat="server" Font-Bold="True" OnClick="LnkGrapgh_Click"
                                                        OnClientClick="showProgress()">To View Graph, Please Click Here...</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="ltScripts" runat="server"></asp:Literal>
                                                    <div id="chart_div" style="width: 100%; height: 200px;">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr id="tr4" runat="server">
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr id="tr5" runat="server">
                    <td>
                        <table runat="server" id="TabDropData" visible="false" width="100%">
                            <tr>
                                <td colspan="4">
                                    <div style="background-color: #DADADA; height: 30px; vertical-align: middle">
                                        <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                            &nbsp;&nbsp;Action Taken
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <b>Reason </b>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDropReason" runat="server" Width="200px">
                                                    <asp:ListItem>-SELECT-</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDropOrder" runat="server" Text="Order Cancel" CssClass="submit"
                                                    Height="30px" Width="200px" OnClick="btnDropOrder_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td colspan="2" align="right">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Allot To Installer" CssClass="submit"
                                        Height="30px" Width="200px" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="tr8" runat="server" visible="false">
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnSendSMS" runat="server" Text="Resend Happy Code" CssClass="submit"
                                        Height="30px" Width="150px" OnClick="btnSendSMS_Click" />
                                </td>
                                <td align="right">
                                    <asp:DropDownList ID="ddlUpdateInstaller" runat="server" CssClass="textarea" Width="190px">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:Button ID="btnUpdateInstaller" runat="server" Text="Update Installer" CssClass="submit"
                                        Height="30px" Width="140px" OnClick="btnUpdateInstaller_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <asp:LinkButton ID="lnkbtnPopup" runat="server"></asp:LinkButton>
    <ajax:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="lnkbtnPopup"
        CancelControlID="btnClose" BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Height="250px" align="center"
        Style="display: none">
        <div>
            <h4 style="background: #3986DD; color: #ffffff; padding: 5px; font-size: 16px; font-style: italic;
                font-weight: 100; width: 96%; display: block;">
                Revert Process</h4>
            <br />
            <table style="border: 1px solid black; font-weight: bold; height: auto; width: 95%;
                padding: 5px 5px 5px 5px">
                <tr>
                    <td>
                        Order ID
                    </td>
                    <td>
                        <asp:Label ID="lblOrderID" runat="server"></asp:Label>
                    </td>
                    <td>
                        Division
                    </td>
                    <td>
                        <asp:Label ID="lblDivision" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        CA No
                    </td>
                    <td>
                        <asp:Label ID="lblCANo" runat="server"></asp:Label>
                    </td>
                    <td>
                        Meter No
                    </td>
                    <td>
                        <asp:Label ID="lblMeterNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Current Alloted To
                    </td>
                    <td>
                        <asp:Label ID="lblAllotedTo" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Remark
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="Remarks" runat="server" Height="60px" TextMode="MultiLine" Width="230px"
                            Style="resize: none"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Now Allot To
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlEmpName" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <asp:Button ID="btnUpdate" runat="server" Text="Update Allotment" CssClass="submit"
            Height="30px" Width="150px" OnClick="btnUpdate_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="submit" Height="30px"
            Width="150px" />
    </asp:Panel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <div class="modal" style="clear: both; text-align: center; width: 100%;">
                <div class="center" style="text-align: center; margin-top: 170px;">
                    <img alt="" src="Image/pleasewait.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
