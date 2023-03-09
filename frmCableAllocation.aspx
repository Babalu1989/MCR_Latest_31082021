<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="frmCableAllocation.aspx.cs" Inherits="frmCableAllocation" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:RadioButtonList ID="rdbtnList" AutoPostBack="true" Width="500px" runat="server"
                            RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbtnList_SelectedIndexChanged">
                            <asp:ListItem Text="Cable Length Pending For Allocation" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Cable Length Allotted Cases" Value="2"></asp:ListItem>
                            <%--<asp:ListItem Text="Return Cable Length" Value="3"></asp:ListItem>--%>
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
                                        Division
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="textarea" Width="190px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Vendor Name
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlVendor" runat="server" CssClass="textarea" Width="190px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date of Issue From
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtPostingDateFrom" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPostingDateFrom"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        Date of Issues To
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtPostingDateTo" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtPostingDateTo"
                                            Format="dd mmm yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Updated Date From
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtupdatefrom" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar12" runat="server" Control="txtupdatefrom"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        Updated Date To
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtupdateto" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar13" runat="server" Control="txtupdateto" Format="dd mmm yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Material Doc No.
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtMaterialDocNo" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Material No.
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtMeterNo" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
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
                                        <asp:Label ID="lblSelectedCase" Text="0" runat="server" Visible="false" />
                                    </span>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                                        Width="80px" Visible="false" OnClick="btnExcel_Click" />
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
                                <asp:GridView EmptyDataText="No Record Found" ID="gvMainData" AllowSorting="True"
                                    runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="None" BorderWidth="1px" CellSpacing="2" GridLines="None">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="sellectOne" AutoPostBack="true" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Material Doc No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmaterialdoc" runat="server" Text='<%#Eval("MBLNR_NUMBER_MATERIAL_DOCUMENT")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Material No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmaterialno" runat="server" Text='<%#Eval("MATNR_MATERIAL_NUMBER")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Division">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDivcode" runat="server" Text='<%#Eval("DIV_CODE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorId" runat="server" Text='<%#Eval("VENDOR_CODE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendornamenew" runat="server" Text='<%#Eval("VENDOR_NAME")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Movement Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmoventtype" runat="server" Text='<%#Eval("BWART_MOVEMENT_TYPE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="Valuation Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmaterialno" runat="server" Text='<%#Eval("BWTAR_D_VALUATION_TYPE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Cable Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbatch" runat="server" Text='<%#Eval("CHARG_D_BATCH_NUMBER")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmaterialdesc" runat="server" Text='<%#Eval("MENGE_D_QUANTITY")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                        <%-- <asp:TemplateField HeaderText="Material Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmadesc" runat="server" Text='<%#Eval("MAKTX_MATERIAL_DESCRIPTION")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                        <%-- <asp:TemplateField HeaderText="Amount Local Currency">
                                            <ItemTemplate>
                                                <asp:Label ID="lblvendorid" runat="server" Text='<%#Eval("DMBTR_AMOUNT_LOCAL_CURRENCY")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="MRS No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMRSNo" runat="server" Text='<%#Eval("RSNUM_NUMBER_RESERVATION")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="USNAM_USER_NAME" HeaderText="UserId" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="USNAM_USER_NAME" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ZMRS_NAME_MRS_NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ZMRS_NAME_MRS_NAME" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="CPUDT_DAY_ON_WHICH_ACCOUNTING" HeaderText="Date of Issue"
                                            ItemStyle-HorizontalAlign="Left" DataFormatString="{0:dd/MM/yyyy}" SortExpression="CPUDT_DAY_ON_WHICH_ACCOUNTING"
                                            HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UPDATED_DATE" HeaderText="Updated Date" ItemStyle-HorizontalAlign="Left"
                                            DataFormatString="{0:dd/MM/yyyy}" SortExpression="UPDATED_DATE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Drum No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldrumno" runat="server" Text='<%#Eval("DRUM_NO")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cable Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcablesize" runat="server" Text='<%#Eval("CABLE_SIZE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Make">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmake" runat="server" Text='<%#Eval("MAKE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No From">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsrfrom" runat="server" Text='<%#Eval("SERIAL_NO_FROM")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serial No To">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSrto" runat="server" Text='<%#Eval("SERIAL_NO_TO")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Cable Quantity Available">
                                            <ItemTemplate>
                                                <asp:Label ID="txtcable" runat="server" Text='<%#Eval("MENGE_D_QUANTITY")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="Balance Cable Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="txtunusedcable" runat="server" Text='<%#Eval("RETURN_UNUSED_CABLE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Allocated Cable Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="txtallocatecable" runat="server" Text='<%#Eval("ALLOCATED_CABLE_LENGTH")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Allocated To">
                                            <ItemTemplate>
                                                <asp:Label ID="txtallocatedto" runat="server" Text='<%#Eval("ALLOCATED_TO")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
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
                <tr id="tr3" runat="server">
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr id="tr6" runat="server">
                    <td align="left">
                        <div id="installertab" runat="server" style="background-color: #DADADA; height: 30px;
                            vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;Installer wise Allotements Details
                            </div>
                        </div>
                        <br />
                        <div style="width: 70%" id="divVendorList" runat="server">
                            <%--<asp:CheckBox ID="chkb1" runat="server" OnCheckedChanged="sellectAll" AutoPostBack="true" />--%>
                            <table width="70%">
                                <tr>
                                    <td style="width: 70%">
                                        <div style="overflow: scroll; width: 100%; height: 350px">
                                            <asp:GridView ID="gvSeriesWiseAllocation" runat="server" CssClass="gvwWhite" AutoGenerateColumns="false"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:RadioButton ID="rdbtnseriesSelect" AutoPostBack="true" OnCheckedChanged="rdbdiscontinued_CheckedChanged"
                                                                runat="server" CssClass="checkbox" onclick="RadioCheck(this);" />
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
                                                    <asp:TemplateField HeaderText="Pending Cable length">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPSeal" runat="server" Text='<%#Eval("SEALALLOTED")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Previous gunny seals available with Installer" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPSealG" runat="server" Text='<%#Eval("SEALALLOTED")%>' />
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
                        </div>
                    </td>
                </tr>
                <tr id="tr7" runat="server" visible="false">
                    <td align="left" colspan="2">
                        <table width="49%" style="border: 1px solid black;">
                            <tr>
                                <td colspan="4" style="height: 15px">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Name
                                </td>
                                <td>
                                    <asp:Label ID="txtName" runat="server"></asp:Label>
                                </td>
                                <td>
                                    User ID
                                </td>
                                <td>
                                    <asp:Label ID="txtEmployeeID" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Enter Cable Length
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCablelength" runat="server" CssClass="textarea"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trSealDetail" runat="server" visible="false" style="height: 80PX">
                                <td align="center" style="font-weight: bold; font-size: 16px" colspan="3">
                                    <br />
                                    <asp:Label ID="lblNOAllotSeal" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <br />
                                    <div style="overflow: scroll; width: 60%; height: 80px">
                                        Seal No
                                        <asp:CheckBoxList ID="chboxListSealNO" runat="server" Width="60%">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
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
                    <td align="right">
                        <asp:DropDownList ID="ddlUpdateInstaller" runat="server" CssClass="textarea" Width="190px">
                        </asp:DropDownList>
                        &nbsp; &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnUpdateInstaller" runat="server" Text="Update Installer" CssClass="submit"
                            Height="30px" Width="200px" OnClick="btnUpdateInstaller_Click" />
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
                <asp:ImageButton ID="btnDocClose" runat="server" ImageUrl="~/Image/Close.png" Style="height: 16px;
                    float: right; margin: 10px 3px 2px 2px;" OnClick="btnDocClose_Click" />
            </div>
            <div style="cursor: pointer; visibility: hidden; float: right; margin-left: 10px;
                margin-top: -4px;">
                <span style="width: 16px; height: 16px; display: block; float: right; margin: 10px 3px 2px 2px;
                    background: url(images/pin-w.png)center center no-repeat;"></span>
            </div>
            <h4 style="border: 2px solid #3986DD; background: #3986DD; color: #ffffff; padding: 5px;
                font-size: 16px; font-weight: 500; text-align: center; display: block;">
                Returned Cable Details
            </h4>
            <div style="border: 4px solid #3986DD; display: block; color: #696969; padding: 10px;">
                <table style="height: 100px; width: 100%;">
                    <tbody>
                        <tr>
                            <td colspan="4">
                                <asp:GridView EmptyDataText="No Record Found" ID="GridView1" AllowSorting="True"
                                    runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="None" BorderWidth="1px" CellSpacing="2" GridLines="None">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow1" runat="server" OnCheckedChanged="sellectrow" AutoPostBack="true" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Material Doc">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmateraildoc" runat="server" Text='<%#Eval("MATERIAL_DOC_NO")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Material No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmaterialno" runat="server" Text='<%#Eval("MATERIAL_NO")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField> <asp:TemplateField HeaderText="Cable Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCablesize" runat="server" Text='<%#Eval("CABLE_SIZE")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Installer">
                                            <ItemTemplate>
                                                <asp:Label ID="lblallocate" runat="server" Text='<%#Eval("ALLOCATED_TO")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance Cable">
                                            <ItemTemplate>
                                                <asp:Label ID="lblreturncable" runat="server" Text='<%#Eval("CABLE_LENGTH")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
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
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnaccept" CssClass="btnAdd" runat="server" Text="Accept" Width="100px"
                                    Visible="false" OnClick="btnaccept_Click"></asp:Button>
<%--                                <asp:Button ID="btnreject" CssClass="btnAdd" runat="server" Text="Reject" Width="100px"
                                    Visible="false"></asp:Button>--%>
                                <asp:Button ID="btnclose" CssClass="btnAdd" runat="server" Text="Close" Width="100px" OnClick="btnclose_Click">
                                </asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
