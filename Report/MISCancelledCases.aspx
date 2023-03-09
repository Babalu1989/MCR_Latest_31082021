<%@ Page Title="MIS Cancelled Cases" Language="C#" MasterPageFile="SiteMaster.master" AutoEventWireup="true" CodeFile="MISCancelledCases.aspx.cs" Inherits="Report_MISCancelledCases" EnableEventValidation="false" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css" />--%>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/elevateweb/elevatezoom/master/jquery.elevateZoom-3.0.8.min.js"></script>
    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>


    

    <script type="text/javascript" language="javascript">  
        debugger;
        $(document).ready(function () {
            debugger;
            var gridHeader = $('#<%=grdCanecelledCases.ClientID%>').clone(true);
            $(gridHeader).find("tr:gt(0)").remove();
            $('#<%=grdCanecelledCases.ClientID%> tr th').each(function (i) {

                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdCanecelledCases.ClientID%>').offset().top);

        });


        function showProgress() {
           <%-- var updateProgress = $get("<%= UpdateProgress.ClientID %>");
            updateProgress.style.display = "block";--%>
            var startDate = $('#<%=txtStartDate.ClientID%>').val();
            var endDate = $('#<%=txtEndDate.ClientID%>').val();
            var division = $('#<%=ddlDivision.ClientID%>').val();
            var orderType = $('#<%=ddlOrderType.ClientID%>').val();

            if (startDate != "" && endDate == "") {
                alert('Please enter end date');
                updateProgress.style.display = "none";
                return false;
            }
            if (startDate == "" && endDate != "") {
                alert('Please enter start date');
                updateProgress.style.display = "none";
                return false;
            }

        }

        function GetCancelledCases(division, orderType, startDate, endDate) {
            debugger;
            $.ajax({
                url: "miscancelledcases.aspx/GetCancelledCases",
                type: "POST",
                data: { Division: division, OrderType: orderType, StartDate: startDate, EndDate: endDate },
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    debugger;
                },
                error: function (error) {
                    debugger;
                    alert('something went wrong')
                }
            });
        }
    </script>


    <style type="text/css">
        .modal {
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

        .center {
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

            .center img {
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

            <table width="95%">
                <tr>
                    <td style="height: 15px; text-align: center">
                        <h2>MIS Cancelled Cases</h2>
                    </td>
                </tr>
                <tr id="tr2" runat="server">
                    <td align="center">
                        <div>
                            <table width="90%" style="border: 1px solid black;">
                                <tr>
                                    <td colspan="4" style="height: 15px"></td>
                                </tr>

                                <tr>
                                    <td>Division
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDivision" CssClass="textarea" runat="server" 
                                            Width="190px" onselectedindexchanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true">
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
                                    <td>Start Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStartDate" runat="server" MaxLength="50" CssClass="textarea"
                                            Width="190px">
                                        </asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtStartDate" Format="dd mmm yyyy" To-Today="true" />
                                    </td>
                                    <td>End Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndDate" runat="server" MaxLength="50" CssClass="textarea"
                                            Width="190px">
                                        </asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtEndDate" Format="dd mmm yyyy" To-Today="true" />
                                    </td>
                                </tr>
                                <tr>
                                 <td>Order Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrderType" CssClass="textarea" runat="server" Width="190px">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <br />
                                        <asp:UpdatePanel runat="server" ID="updatepanel1" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnSave" runat="server" Text="SHOW" CssClass="submit" Height="30px"
                                                    Width="150px" OnClick="btnSave_Click" OnClientClick="return showProgress();" />
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
                                    <td colspan="4" style="height: 15px"></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px"></td>
                </tr>
                <tr>
                    <td style="text-align: left; color: red">
                        <span id="hdnTotalLBL" runat="server" visible="false">Total:</span>
                        <asp:Label ID="hdnTotalCount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-left: 20px">
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div id="GridView1_div" runat="server" style="width: 100%;">
                            <div id="GHead">
                            </div>
                            <div style="width: 100%; height: 400px; overflow: auto;">
                                <asp:GridView ID="grdCanecelledCases" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                    Width="100%" AutoGenerateColumns="false" OnSorting="grdCanecelledCases_Sorting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DIVISION" HeaderText="DIVISION" SortExpression="DIVISION" HeaderStyle-Font-Underline="true"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="VENDOR_CODE" HeaderText="VENDOR_CODE" SortExpression="VENDOR_CODE" HeaderStyle-Font-Underline="true"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="CA_NO" HeaderText="CA_NO" SortExpression="CA_NO" HeaderStyle-Font-Underline="true"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="OrderID" HeaderText="Order ID" SortExpression="OrderID" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ORDER_TYPE" HeaderText="ORDER_TYPE" SortExpression="ORDER_TYPE" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="BASIC_START_DATE" HeaderText="BASIC_START_DATE" SortExpression="BASIC_START_DATE" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="BASIE_FINISH_DATE" HeaderText="BASIE_FINISH_DATE" SortExpression="BASIE_FINISH_DATE" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="NAME" HeaderText="NAME" SortExpression="NAME" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="FATHER_NAME" HeaderText="FATHER_NAME" SortExpression="FATHER_NAME" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ADDRESS" HeaderText="ADDRESS" SortExpression="ADDRESS" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MOBILE_NO" HeaderText="MOBILE_NO" SortExpression="MOBILE_NO" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="CANCEL_REASON" HeaderText="CANCEL_REASON" SortExpression="CANCEL_REASON" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle HorizontalAlign="Center" Height="35px" />
                                </asp:GridView>

                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </center>

        <%--<asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel1">
            <ProgressTemplate>
                <div class="modal" style="clear: both; text-align: center; width: 100%;">
                    <div class="center" style="text-align: center; margin-top: 170px;">
                        <img alt="" src="../Image/pleasewait.gif" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
    </div>
    <%--<div>
        <table id="Customer-Info" class="table">
            <thead>
                <tr>
                    <th>S.NO.</th>
                    <th>DIVISION</th>
                    <th>VENDOR_CODE	</th>
                    <th>CA_NO </th>
                    <th>Order ID</th>
                    <th>ORDER_TYPE</th>
                    <th>BASIC_START_DATE</th>
                    <th>BASIE_FINISH_DATE</th>
                    <th>NAME</th>
                    <th>FATHER_NAME</th>
                    <th>ADDRESS</th>
                    <th>MOBILE_NO	</th>
                    <th>Cancel Reason</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td></td>
                </tr>
            </tbody>
        </table>
    </div>--%>
     
</asp:Content>

