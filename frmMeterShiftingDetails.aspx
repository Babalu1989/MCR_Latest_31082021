<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="frmMeterShiftingDetails.aspx.cs" Inherits="frmMeterShiftingDetails" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
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
    <script type="text/javascript">
        function CheckAll(oCheckbox) {
            var MainData = document.getElementById("<%=gvMainData.ClientID %>");
            for (i = 1; i < MainData.rows.length; i++) {
                MainData.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="divclass">
        <center>
            <table width="95%">
                <tr>
                    <td style="height: 15px; text-align: center">
                        <h2>
                            <i>Meter Shifting Details</i></h2>
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
<%--                                    <td>
                                        Division
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="textarea" Width="190px"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>--%>
                                       <td>
                                        Activity Type
                                    </td>
                                    <td style="text-align: left;">
                                         <asp:DropDownList ID="ddlactivitytype" runat="server" CssClass="textarea" Width="190px">
                                         <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                         <asp:ListItem Text="Same" Value="1"></asp:ListItem>
                                         <asp:ListItem Text="Differ" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Order Id
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtorderid" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Activity Date From
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtPostingDateFrom" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPostingDateFrom"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        Activity Date To
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtPostingDateTo" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtPostingDateTo"
                                            Format="dd mmm yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                 <%--   <td>
                                        Activity Type
                                    </td>
                                    <td style="text-align: left;">
                                         <asp:DropDownList ID="ddlactivitytype" runat="server" CssClass="textarea" Width="190px">
                                         <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                         <asp:ListItem Text="Same" Value="1"></asp:ListItem>
                                         <asp:ListItem Text="Differ" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>--%>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="tr2" runat="server" visible="false">
                                    <td>
                                        Vendor Name
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlVendorName" runat="server" CssClass="textarea" Width="190px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="submit" Height="30px"
                                            Width="150px" OnClientClick="showProgress()" OnClick="btnSave_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Refresh" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnCancel_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="submit" Height="30px"
                                            Visible="false" Width="150px" />
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
                                        Width="80px" Visible="false" onclick="btnExcel_Click" />
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
                            <div style="overflow: scroll; width: 99.9%; max-height: 270px">
                                <asp:GridView EmptyDataText="No Record Found" ID="gvMainData" 
                                    runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="None" BorderWidth="1px" CellSpacing="2" GridLines="None">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:BoundField DataField="ORDERID" HeaderText="Order Id" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ORDERID" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DIVISION" HeaderText="Devision" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DIVISION" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CA" HeaderText="CA Number" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CA" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DEVICENO" HeaderText="Meter Number" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DEVICENO" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORDER_TYPE" HeaderText="Order Type" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ORDER_TYPE" HeaderStyle-Font-Underline="true">
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
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PM_DESC" HeaderText="PM Activity(Old)" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PM_DESC" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OLD_DESC" HeaderText="PM Activity(New)" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="OLD_DESC" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="METERSHIFTING_REMARKS" HeaderText="Meter Shifting Remarks"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="METERSHIFTING_REMARKS" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ENTRY_DATE" HeaderText="Activity Date" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ENTRY_DATE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                          <asp:BoundField DataField="AMOUNT" HeaderText="Previous Amount" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="AMOUNT" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lknbutton" runat="server" Text="Select" ForeColor="Blue" OnClick="lknbutton_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <HeaderStyle HorizontalAlign="Center" Height="35px" BackColor="#008dde" Font-Bold="True"
                                        ForeColor="White" />
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
            </table>
        </center>
    </div>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <div class="modal" style="clear: both; text-align: center; width: 100%;">
                <div class="center" style="text-align: center; margin-top: 170px;">
                    <img alt="" src="Image/pleasewait.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div id="divAttachment" runat="server" visible="false" style="width: 60%; height: 300px;
        left: 22%; top: 220px; height: auto; position: fixed; z-index: 999; display: block;
        cursor: auto;">
        <div style="background: #f5f5f5; min-height: 16px;">
            <div style="cursor: pointer; float: right; margin-left: 10px; margin-top: -4px;">
                <asp:ImageButton ID="btnDocClose" runat="server" ImageUrl="~/Image/Close.png" Style="width: 16px;
                    height: 16px; float: right; margin: 10px 3px 2px 2px;" OnClick="btnDocClose_Click" />
            </div>
            <div style="cursor: pointer; visibility: hidden; float: right; margin-left: 10px;
                margin-top: -4px;">
                <span style="width: 16px; height: 16px; display: block; float: right; margin: 10px 3px 2px 2px;
                    background: url(images/pin-w.png)center center no-repeat;"></span>
            </div>
            <h4 style="border: 2px solid #3986DD; background: #3986DD; color: #ffffff; padding: 5px;
                font-size: 16px; font-weight: 500; text-align: center; display: block;">
                Meter Shifting Details</h4>
            <div style="border: 4px solid #3986DD; display: block; color: #696969; padding: 10px;">
                <table style="height: 100px; width: 100%;">
                    <tbody>
                        <tr>
                            <td colspan="4">
                                <table width="100%" height="180px">
                                    <tr>
                                        <td>
                                            <b>Order Id:</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblorderid" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <b>Division:</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldivision" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>PM_Activity(Old):</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblactivityold" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <b>PM_Activity(New):</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblactivitynew" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                       <tr>
                                        <td>
                                            <b>Previous Amount:</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblprevamt" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <b>Current Amount:</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblcurramt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                            <b>Difference Amount:</b>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblamtdiff" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <b>Action Type:<span style="color: Red;">*</span></b>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddlstatus" runat="server" Width="200px">
                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Credit" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Debit" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Field Engineer Remarks:</b>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtrmk" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>MMG Coordinator Remarks:<span style="color: Red;">*</span></b>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtmmgrmk" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnsubmit" CssClass="btnAdd" runat="server" Text="Submit" 
                                    Width="100px" onclick="btnsubmit_Click">
                                </asp:Button>
                                <asp:Button ID="btnclose" CssClass="btnAdd" runat="server" Text="Close" Width="100px"
                                    OnClick="btnclose_Click"></asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
