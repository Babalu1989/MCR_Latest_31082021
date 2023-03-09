<%@ Page Title="" Language="C#" MasterPageFile="SiteMaster.master" AutoEventWireup="true"
    CodeFile="MCR_LosseUpload.aspx.cs" Inherits="Report_MCR_LosseUpload" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/elevateweb/elevatezoom/master/jquery.elevateZoom-3.0.8.min.js"></script>
    <%-- <script type="text/javascript">
        $(function () {
            $("[id*=GridView2] img").elevateZoom({
                cursor: 'pointer',
                imageCrossfade: true,
                loadingIcon: 'loading.gif'
            });
        });
    </script>--%>
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
            <table style="text-align: center" width="100%">
                <tr>
                    <td style="height: 15px">
                        <h2>
                            Losse Meter Photo & MCR Uploaded Report</h2>
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
                            <tr id="Tr10" runat="server" visible="false">
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
                                    Activity From Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtActFrmDT" CssClass="textarea" ReadOnly="true" runat="server"
                                        Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtActFrmDT" Format="dd mmm yyyy" />
                                </td>
                                <td>
                                    Activity To Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtActToDT" CssClass="textarea" ReadOnly="true" runat="server" Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar4" runat="server" Control="txtActToDT" Format="dd mmm yyyy" />
                                </td>
                            </tr>
                            <tr id="trOrderType" runat="server" visible="false">
                                <td>
                                    Meter No
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMeterNo" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                </td>
                                <td>
                                    Order Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="textarea" Width="190px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Division
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDivision" runat="server" CssClass="textarea" Width="190px"
                                        OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true">
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
                                <td colspan="4" align="center">
                                    <br />
                                    <asp:UpdatePanel runat="server" ID="updatepanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="submit" Height="30px"
                                                Width="150px" OnClick="btnSave_Click" OnClientClick="showProgress()" />
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
                    <td style="height: 25px" align="right">
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="25px"
                            Width="80px" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div id="GridView1_div" runat="server" style="width: 100%;">
                            <div id="GHead">
                            </div>
                            <div style="overflow: scroll; width: 99.9%; max-height: 400px">
                                <asp:GridView ID="gvMainData" AllowSorting="True" runat="server" Width="100%" AutoGenerateColumns="False"
                                    OnSorting="gvMainData_Sorting" BackColor="White" BorderColor="#999999" BorderStyle="None"
                                    BorderWidth="1px" CellSpacing="2" GridLines="None">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DIVISION">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="VENDOR_CODE">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VENDOR" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="VENDOR">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORDERID" HeaderText="Order No" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ORDERID">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="METER" HeaderText="Meter No" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="METER">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PhotoUpload" HeaderText="Photo Uploaded" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PhotoUpload">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MCR_IMAGE" HeaderText="MCR Uploaded" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="MCR_IMAGE">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Both" HeaderText="Both MCR & Photo Uploaded" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="Both">
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORDER_TYPE" HeaderText="Ord Type" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ORDER_TYPE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PM_ACTIVITY" HeaderText="PM Act" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PM_ACTIVITY" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="View">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgComplaintID" CommandArgument='<%#Eval("DEVICENO")%>' CommandName='<%#Eval("DEVICENO")%>'
                                                    ImageUrl="~/Image/Draft.gif" Width="25px" Height="25px" OnCommand="imgEmpID_Command"
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <HeaderStyle HorizontalAlign="Center" Height="35px" Font-Underline="true" BackColor="#008dde"
                                        Font-Bold="True" ForeColor="White" />
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
                    <td style="height: 5px">
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <asp:LinkButton ID="lnkbtn" runat="server"></asp:LinkButton>
    <ajax:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="lnkbtn"
        CancelControlID="btnClose" BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
        <div>
            <h4 style="background: #3986DD; color: #ffffff; padding: 5px; font-size: 16px; font-style: italic;
                font-weight: 100; width: 90%; display: block;">
                Download Images</h4>
            <center>
                <div style="overflow: scroll; width: 100%; max-height: 250px">
                    <table style="border: 1px solid black; font-weight: bold; width: 90%">
                        <tr id="tr1" runat="server">
                            <td style="vertical-align: middle">
                                Meter Photograph
                            </td>
                            <td>
                                <%-- <asp:Image ID="Image1" runat="server"  BackColor="Black"  Width="100%" />--%>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageButton1_Click" />
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td>
                                Complete Meter Photograph with Background
                            </td>
                            <td>
                                <%--<asp:Image ID="Image2" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageButton2_Click" />
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td>
                                Pole End Photograph
                            </td>
                            <td>
                                <%--<asp:Image ID="Image3" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageButton3_Click" />
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td>
                                Other
                            </td>
                            <td>
                                <%--<asp:Image ID="Image4" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageButton4_Click" />
                            </td>
                        </tr>
                        <tr id="tr5" runat="server">
                            <td>
                                MCR
                            </td>
                            <td>
                                <%-- <asp:Image ID="imgMCR" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageMCR" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageMCR_Click" />
                            </td>
                        </tr>
                        <tr id="tr6" runat="server">
                            <td>
                                Meter Test Report
                            </td>
                            <td>
                                <%--<asp:Image ID="imgMeterTest" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageMeterTest" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageMeterTest_Click" />
                            </td>
                        </tr>
                        <tr id="tr7" runat="server">
                            <td>
                                Meter Lab Test Report
                            </td>
                            <td>
                                <%--<asp:Image ID="imgLabTest" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageLabTest" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageLabTest_Click" />
                            </td>
                        </tr>
                        <tr id="tr8" runat="server">
                            <td>
                                Consumer Signature
                            </td>
                            <td>
                                <%-- <asp:Image ID="imgSign" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageSign" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageSign_Click" />
                            </td>
                        </tr>
                        <tr id="tr9" runat="server">
                            <td>
                                Cancel Image
                            </td>
                            <td>
                                <%--<asp:Image ID="imgCancel" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageCancel" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageCancel_Click" />
                            </td>
                        </tr>
                        <tr id="tr11" runat="server">
                            <td>
                                Old Meter Image1
                            </td>
                            <td>
                                <%--<asp:Image ID="imgCancel" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageOldMeter1" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageOldMeter1_Click" />
                            </td>
                        </tr>
                        <tr id="tr12" runat="server">
                            <td>
                                Old Meter Image2
                            </td>
                            <td>
                                <%--<asp:Image ID="imgCancel" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageOldMeter2" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageOldMeter2_Click" />
                            </td>
                        </tr>
                        <tr id="tr13" runat="server">
                            <td>
                                Auto MCR PDF
                            </td>
                            <td>
                                <%--<asp:Image ID="imgCancel" runat="server" BackColor="Black" Width="100%" />--%>
                                <asp:ImageButton ID="ImageAutoMCR" runat="server" ImageUrl="~/Image/up.gif" OnClick="ImageAutoMCR_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="submit" Height="20px"
                    Width="150px" />
            </center>
            <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel1">
                <ProgressTemplate>
                    <div class="modal" style="text-align: center;">
                        <div class="center" style="text-align: center; padding-top: 100px;">
                            <img alt="" src="../Image/pleasewait.gif" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </asp:Panel>
</asp:Content>
