<%@ Page Title="Reallocation Cancel Order" Language="C#" MasterPageFile="~/SiteMaster.master"
    AutoEventWireup="true" CodeFile="ReallocationCancelOrder.aspx.cs" Inherits="ReallocationCancelOrder" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

        function ex() {
            var x = confirm("Have you Cancel the case/cases in SAP?")
            if (x)
                confirm("Are you sure, you want to Cancel this Order?")
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="divclass">
        <center>
            <table width="98%">
                <tr id="tr1" runat="server">
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div>
                            <table width="95%" style="border: 1px solid black;">
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
                                        <asp:TextBox ID="txtMeterNO" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Service Order No.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtServiceOrdNo" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Division
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="txtDivision" runat="server" CssClass="textarea" 
                                            Width="190px" onselectedindexchanged="txtDivision_SelectedIndexChanged" AutoPostBack="true">
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
                                        Basic Finish Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBasicFinishDate" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtBasicFinishDate"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        Kitting From Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPostingDate" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPostingDate"
                                            Format="dd mmm yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Kitting To Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPostingToDate" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtPostingToDate"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        Order Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="textarea" Width="190px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        PM Activity
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPMActivity" runat="server" CssClass="textarea" Width="190px">
                                            <asp:ListItem>-ALL-</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <br />
                                        <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnShow_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Refresh" CssClass="submit" Height="30px"
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
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 30">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <span style="color: Red">Total Case :
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
                        <div style="background-color: #DADADA; height: 30px; vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;Cancelled Cases Details
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="Div1" runat="server" style="width: 100%;">
                            <div id="GHead">
                            </div>
                            <div style="overflow: scroll; width: 99.9%; max-height: 310px">
                                <asp:GridView EmptyDataText="No Record Found" ID="gvMainData" AllowSorting="true"
                                    CssClass="gvwWhite" runat="server" Width="100%" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkb1" runat="server" OnCheckedChanged="sellectAll" AutoPostBack="true" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="sellectOne" AutoPostBack="true" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Id" HeaderStyle-Font-Underline="true" SortExpression="ORDERID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderID" runat="server" Text='<%#Eval("ORDERID")%>' Visible="false" />
                                                <asp:Label ID="ORDERID" runat="server" Text='<%#Eval("ORDERID")%>' Visible="true" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DIVISION" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="CA_NO" HeaderText="CA No" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CA_NO" HeaderStyle-Font-Underline="true" />
                                        <asp:TemplateField HeaderText="Meter No" HeaderStyle-Font-Underline="true" SortExpression="METER_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMeterNO" runat="server" Text='<%#Eval("METER_NO")%>' Visible="false" />
                                                <asp:Label ID="METER_NO" runat="server" Text='<%#Eval("METER_NO")%>' Visible="true" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PLANNER_GROUP" HeaderText="Planner Group" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PLANNER_GROUP" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="ACCOUNT_CLASS" HeaderText="Account Class" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ACCOUNT_CLASS" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="SANCTIONED_LOAD" HeaderText="Sanction Load" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="SANCTIONED_LOAD" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="POSTINGDATE" HeaderText="Kitting Date" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="POSTINGDATE" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="FIN_DATE" HeaderText="Basic Finish Date" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FINISH_DATE" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="NAME" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="ADDRESS" HeaderText="Address" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="25%" SortExpression="ADDRESS" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="PUNCHDATE" HeaderText="Meter Execution Date" ItemStyle-HorizontalAlign="Left"
                                            Visible="false" SortExpression="PUNCHDATE" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="ALLOTEDTO" HeaderText="Allotted To" ItemStyle-HorizontalAlign="Left"
                                            Visible="false" SortExpression="ALLOTEDTO" HeaderStyle-Font-Underline="true" />
                                        <asp:BoundField DataField="Cancel_Reason" HeaderText="Cancel Reason" ItemStyle-HorizontalAlign="Left"
                                            Visible="false" SortExpression="Cancel_Reason" HeaderStyle-Font-Underline="true" />
                                        <asp:CommandField ShowSelectButton="true" SelectText="Revert Process" Visible="false" />
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center" Height="35px" />
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr id="tr3" runat="server">
                    <td style="height: 15px">
                        <table id="TABUpdate" width="100%" runat="server" visible="false">
                            <tr>
                                <td colspan="2">
                                    <div style="background-color: #DADADA; height: 30px; vertical-align: middle">
                                        <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                            &nbsp;&nbsp;Update Order Details
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr style="height: 50px;">
                                <td>
                                    <b>Remarks</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Height="40px" Width="800px" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 35px;">
                                    <asp:Button ID="btnDelete" runat="server" Text="Cancel In SAP" CssClass="submit"
                                        Height="30px" Width="200px" OnClick="btnDelete_Click" />
                                    &nbsp;
                                    <asp:Button ID="BtnReallocate" runat="server" Text="Reallocate Order" CssClass="submit"
                                        Height="30px" Width="200px" OnClick="BtnReallocate_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>
