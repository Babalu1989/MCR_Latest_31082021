<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="frmCableLengthMST.aspx.cs" Inherits="frmCableLengthMST" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>--%>
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
  <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <div class="divclass">
        <center>
            <table width="95%">
                <tr>
                    <td style="height: 15px; text-align: center">
                        <h2>
                            <i>Cable Length Details</i></h2>
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
                                            AutoPostBack="True" 
                                            onselectedindexchanged="ddlDivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                 <td>
                                        Vendor Name
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlVendorName" runat="server" CssClass="textarea" Width="190px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date of Issue From
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtPostingDateFrom" CssClass="textarea" ReadOnly="false" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<%--<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPostingDateFrom"
                                            Format="dd mmm yyyy" />--%>
                                             <asp:ImageButton ID="imgPopup2" ImageUrl="~/Image/calendar_icon.png" ImageAlign="Bottom"
                                            runat="server" Width="25px" Height="22px" />
                                        <cc1:CalendarExtender ID="CalendarExtender3" PopupButtonID="imgPopup2" runat="server" TargetControlID="txtPostingDateFrom"
                                            Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td>
                                         Date of Issue To
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtPostingDateTo" CssClass="textarea" ReadOnly="false" runat="server"
                                            Width="190px"></asp:TextBox>
                                        <%--&nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtPostingDateTo"
                                            Format="dd mmm yyyy" />--%>
                                             <asp:ImageButton ID="imgPopup3" ImageUrl="~/Image/calendar_icon.png" ImageAlign="Bottom"
                                            runat="server" Width="25px" Height="22px" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" PopupButtonID="imgPopup3" runat="server" TargetControlID="txtPostingDateTo"
                                            Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Updated Date From
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtupdatefrom" CssClass="textarea" ReadOnly="false" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<%--<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPostingDateFrom"
                                            Format="dd mmm yyyy" />--%>
                                             <asp:ImageButton ID="ImageButton1" ImageUrl="~/Image/calendar_icon.png" ImageAlign="Bottom"
                                            runat="server" Width="25px" Height="22px" />
                                        <cc1:CalendarExtender ID="CalendarExtender4" PopupButtonID="ImageButton1" runat="server" TargetControlID="txtupdatefrom"
                                            Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td>
                                        Updated Date To
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtupdateto" CssClass="textarea" ReadOnly="false" runat="server"
                                            Width="190px"></asp:TextBox>
                                        <%--&nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtPostingDateTo"
                                            Format="dd mmm yyyy" />--%>
                                             <asp:ImageButton ID="ImageButton2" ImageUrl="~/Image/calendar_icon.png" ImageAlign="Bottom"
                                            runat="server" Width="25px" Height="22px" />
                                        <cc1:CalendarExtender ID="CalendarExtender5" PopupButtonID="ImageButton2" runat="server" TargetControlID="txtupdateto"
                                            Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Material No.
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtmaterial" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Material Doc No.
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtMaterialDocNo" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
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
                                    <td colspan="4" style="height: 15px; text-align: left;" class=
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
                        <span>
                            Total Selected Case :
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
                            <div style="overflow: scroll; width: 100%; max-height: 270px">
                                   <asp:GridView EmptyDataText="No Record Found" ID="gvMainData" AllowSorting="True"
                                    runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                                    BorderStyle="None" BorderWidth="1px" CellSpacing="2" GridLines="None">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:BoundField DataField="MBLNR_NUMBER_MATERIAL_DOCUMENT" HeaderText="Material Doc No."
                                            ItemStyle-HorizontalAlign="Left" SortExpression="MBLNR_NUMBER_MATERIAL_DOCUMENT"
                                            HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                          <asp:BoundField DataField="MATNR_MATERIAL_NUMBER" HeaderText="Meterial No." ItemStyle-HorizontalAlign="Left"
                                            SortExpression="MATNR_MATERIAL_NUMBER" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DIV_CODE" HeaderText="Division"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="DIVISION" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Id" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="VENDOR_CODE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="BWART_MOVEMENT_TYPE" HeaderText="Movement Type"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="Movement Type" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                    
                                    <%--    <asp:BoundField DataField="BWTAR_D_VALUATION_TYPE" HeaderText="Valuation Type"
                                            ItemStyle-HorizontalAlign="Left" SortExpression="BWTAR_D_VALUATION_TYPE"
                                            HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>--%>
                                      <asp:BoundField DataField="CHARG_D_BATCH_NUMBER" HeaderText="Cable Type" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CHARG_D_BATCH_NUMBER" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="MAKTX_MATERIAL_DESCRIPTION" HeaderText="Material Description" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="MAKTX_MATERIAL_DESCRIPTION" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MENGE_D_QUANTITY" HeaderText="Quantity" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="MENGE_D_QUANTITY" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                       
                                        <asp:BoundField DataField="DMBTR_AMOUNT_LOCAL_CURRENCY" HeaderText="Amount Local Currency" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DMBTR_AMOUNT_LOCAL_CURRENCY" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RSNUM_NUMBER_RESERVATION" HeaderText="MRS No." ItemStyle-HorizontalAlign="Left"
                                            SortExpression="RSNUM_NUMBER_RESERVATION" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="USNAM_USER_NAME" HeaderText="UserId" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="USNAM_USER_NAME" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="ZMRS_NAME_MRS_NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ZMRS_NAME_MRS_NAME" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                          <asp:BoundField DataField="CPUDT_DAY_ON_WHICH_ACCOUNTING" HeaderText="Date of Issue" ItemStyle-HorizontalAlign="Left" DataFormatString = "{0:dd/MM/yyyy}"
                                            SortExpression="CPUDT_DAY_ON_WHICH_ACCOUNTING" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DRUM_NO" HeaderText="Drum No." ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DRUM_NO" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CABLE_SIZE" HeaderText="Cable Size" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CABLE_SIZE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="MAKE" HeaderText="Make" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="MAKE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UPDATED_DATE" HeaderText="Updated Date" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="UPDATED_DATE" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        
                                      <%--   <asp:BoundField DataField="CABLE_TYPE" HeaderText="Cable Type" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CABLE_TYPE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>--%>
                                     <%--    <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="QUANTITY" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="MAKE" HeaderText="Make" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="MAKE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="VENDOR_CODE" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="VENDOR_NAME" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="SERIAL_NO_FROM" HeaderText="Serial No From" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="SERIAL_NO_FROM" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="SERIAL_NO_TO" HeaderText="Serial No TO" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="SERIAL_NO_TO" HeaderStyle-Font-Underline="true">
                                            <HeaderStyle Font-Underline="True"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>--%>
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
                <asp:ImageButton ID="btnDocClose" runat="server" ImageUrl="~/Image/Close.png" 
                    Style="height: 16px; float: right; margin: 10px 3px 2px 2px;" 
                    onclick="btnDocClose_Click" />
            </div>
            <div style="cursor: pointer; visibility: hidden; float: right; margin-left: 10px;
                margin-top: -4px;">
                <span style="width: 16px; height: 16px; display: block; float: right; margin: 10px 3px 2px 2px;
                    background: url(images/pin-w.png)center center no-repeat;"></span>
            </div>
                <h4 style="border: 2px solid #3986DD; background: #3986DD; color: #ffffff; padding: 5px;
                font-size: 16px; font-weight: 500; text-align: center; display: block;">Details
                </h4>
            <div style="border: 4px solid #3986DD; display: block; color: #696969; padding: 10px;">
                <table style="height: 100px; width: 100%;">
                    <tbody>
                        <tr>
                            <td colspan="4">
                                <table width="100%" height="200px">
                                    <tr>
                                        <td>
                                            <b>Division:<span style="color:Red;"></span></b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddldiv" runat="server" Width="92%" Height="27px" 
                                                onselectedindexchanged="ddldiv_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <b>Vendor Name:</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlvendor" runat="server" Width="92%" Height="27px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                     <b>Date of Issues:<span style="color:Red;"></span></b>
                                    </td>
                                    <td>
                                              <asp:TextBox ID="txtdateofissues" runat="server" Height="25px"  Width="80%" Enabled="false"></asp:TextBox>
                                        <asp:ImageButton ID="imgPopup1" ImageUrl="~/Image/calendar_icon.png" ImageAlign="Bottom"
                                            runat="server" Width="25px" Height="22px" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtdateofissues"
                                            Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td> <b>MRS No.:<span style="color:Red;"></span></b></td>
                                    <td>
                                    <asp:TextBox ID="txtmsrno" runat="server" Width="90%" Height="25px" Enabled="false"></asp:TextBox></td>
                                       
                                    </tr>
                                    <tr>
                                     <td>
                                            <b>Make:<span style="color:Red;"></span></b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmake" runat="server" Width="90%" Height="25px"></asp:TextBox>
                                        </td>
                                     <td>
                                            <b>Drum Number:<span style="color:Red;"></span></b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDrumno" runat="server" Width="90%" Height="25px"></asp:TextBox>
                                        </td>
                                      
                                    </tr>
                                    <tr>
                                      <td>
                                            <b>Cable Size:<span style="color:Red;"></span></b>
                                        </td>
                                        <td>
                                           <asp:DropDownList ID="ddlcablesize" runat="server" Width="90%" Height="25px">
                                           <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="2X10" Value="2X10"></asp:ListItem>
                                             <asp:ListItem Text="2X25" Value="2X25"></asp:ListItem>
                                              <asp:ListItem Text="4X25" Value="4X25"></asp:ListItem>
                                               <asp:ListItem Text="4X50" Value="4X50"></asp:ListItem>
                                                <asp:ListItem Text="4X150" Value="4X150"></asp:ListItem>
                                           </asp:DropDownList>
                                        </td>
                                        <td>
                                            <b>Quantity:<span style="color:Red;"></span></b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQuantity" runat="server" Width="90%" Height="25px" Enabled="false"></asp:TextBox>
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                     
                                    <td>
                                            <b>Serial Number From:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtserialnofrom" runat="server" Width="90%" Height="25px"></asp:TextBox>
                                        </td>
                                       <td>
                                            <b>Serial Number To:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtserialnoto" runat="server" Width="90%" Height="25px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                     <td>
                                            <b>Cable Type:</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcabletype" runat="server" Width="92%" Height="25px">
                                             <asp:ListItem Text="Select" Value="Select" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="NEW" Value="NEW"></asp:ListItem>
                                            <asp:ListItem Text="OLD" Value="OLD"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnsubmit" CssClass="btnAdd" runat="server" Text="Update" 
                                    Width="100px" onclick="btnsubmit_Click">
                                </asp:Button>
                                <asp:Button ID="btnclose" CssClass="btnAdd" runat="server" Text="Close" 
                                    Width="100px" onclick="btnclose_Click">
                                </asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
