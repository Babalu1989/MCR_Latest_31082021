<%@ Page Title="" Language="C#" MasterPageFile="SiteMaster.master" AutoEventWireup="true" CodeFile="MIS_Summary_Report.aspx.cs" Inherits="MMG_New_Summary" EnableEventValidation="false"
    ClientIDMode="Static" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script src="Script/jquery-1.9.1.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/elevateweb/elevatezoom/master/jquery.elevateZoom-3.0.8.min.js"></script>
    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdMcrSummary.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdMcrSummary.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdMcrSummary.ClientID%>').offset().top);

        });
        function showProgress() {
            var updateProgress = $get("<%= UpdateProgress.ClientID %>");
            updateProgress.style.display = "block";
            var startDate = $('#<%=txtStartDate.ClientID%>').val();
            var endDate = $('#<%=txtEndDate.ClientID%>').val();

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

        function tableToExcel() {
            debugger;
            var uri = 'data:application/vnd.ms-excel;base64,', template = '<html xmlns:o="urn:schemas-microsoft-com:office:office"xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head></head><body><table>{table}</table></body></html>', base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }, format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
            //return function (table, name) {
            // if (!table.nodeType)
            table = document.getElementById('grdMcrSummary')
            var table_str = table.outerHTML;
            table_str = table_str.replace(/&nbsp;/g, '').replace(/\<br\s*[\/]?>/gi, '');
            console.log(table_str);
            var ctx = { worksheet: name || 'Worksheet', table: table_str }
            window.location.href = uri + base64(format(template, ctx))
            //}

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
                        <h2>MIS Summary Report</h2>
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
                                        <asp:ListItem Text="Select One" Value="0" Enabled="true"></asp:ListItem>
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
                <%--<tr>
                    <td style="color: red">
                        <span runat="server" id="hdnTotal" visible="false">Total: </span>
                        <asp:Label ID="hdnTotalCount" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td style="text-align: right">
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr runat="server" id="totals" visible="false">
                                <td style="width: 300px;color:red; font-weight:600; padding-left: 200px">
                                    <span runat="server" id="Span1">Total Order Bases Cancelled Cases: </span>
                                    <asp:Label ID="lbl_ttl_OrdrCancelled" runat="server" Visible="true"></asp:Label></td>
                                <td style="width: 300px;color:red; font-weight:600;padding-left: 60px">
                                    <span runat="server" id="Span2">Total Order Bases Completed Cases: </span>
                                    <asp:Label ID="lbl_ttl_OrdrComplete" runat="server" Visible="true"></asp:Label></td>
                                <td style="width: 300px;color:red; font-weight:600;padding-left: 60px">
                                    <span runat="server" id="Span3">Total Loose Completed Cases: </span>
                                    <asp:Label ID="lbl_ttl_LooseComplete" runat="server" Visible="true"></asp:Label></td>
                            </tr>
                        </table>
                    </td>

                </tr>
                <tr>
                    <td align="center">
                        <div id="GridView1_div" runat="server" style="width: 100%;">
                            <div id="GHead">
                            </div>
                            <div style="width: 100%; height: 400px; overflow: auto;">
                                <asp:GridView ID="grdMcrSummary" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                    Width="100%" AutoGenerateColumns="false" OnSorting="grdMcrSummary_Sorting" OnRowDataBound="grdMcrSummary_RowDataBound" ShowFooter="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DIVISION" HeaderText="DIVISION" SortExpression="DIVISION" HeaderStyle-Font-Underline="true"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="VENDOR_CODE" HeaderText="VENDOR" SortExpression="VENDOR_CODE" HeaderStyle-Font-Underline="true"
                                            ItemStyle-HorizontalAlign="Left" />

                                        <asp:BoundField DataField="e_orderbasedCancellCases" HeaderText="NUMBER OF ORDER BASED CANCELLED CASES" SortExpression="e_orderbasedCancellCases" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="c_orderbasedCompleted" HeaderText=" NUMBER OF ORDER BASED COMPLETED CASES" SortExpression="c_orderbasedCompleted" HeaderStyle-Font-Underline="true"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="c_looseCompleted" HeaderText="NUMBER OF LOOSE COMPLETED CASES" SortExpression="c_looseCompleted" HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
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


        <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel1">
            <ProgressTemplate>
                <div class="modal" style="clear: both; text-align: center; width: 100%;">
                    <div class="center" style="text-align: center; margin-top: 170px;">
                        <img alt="" src="../Image/pleasewait.gif" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        
    </div>
</asp:Content>

