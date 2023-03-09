<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="frmLooseMeterPunching.aspx.cs" Inherits="frmLooseMeterPunching" EnableViewState = "true" MaintainScrollPositionOnPostback = "true" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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

     <script type="text/javascript">
         function CheckAll(oCheckbox) {
             var MainData = document.getElementById("<%=gvMainData.ClientID %>");
             for (i = 1; i < MainData.rows.length; i++) {
                 MainData.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
             }
         }
     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                            RepeatDirection="Horizontal" 
                            OnSelectedIndexChanged="rdbtnList_SelectedIndexChanged" 
                            style="border: 1px solid black;">   <%--09032018--%>
                            <asp:ListItem Text="Loose Meter Pending For Allocation" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Loose Meter Allotted Cases" Value="2"></asp:ListItem>
                            <%--<asp:ListItem Text="Completed Cases" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Cancelled Cases" Value="4"></asp:ListItem>--%>
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
                                        Division</td>
                                    <td style = "text-align:left;">
                                        <asp:DropDownList ID="ddlDivision" runat="server" CssClass="textarea" 
                                             Width="190px" AutoPostBack="True" 
                                            onselectedindexchanged="ddlDivision_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Scheme No.</td>
                                    <td style = "text-align:left;">
                                        <asp:DropDownList ID="ddlScehme" runat="server" CssClass="textarea" 
                                            Width="190px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        
                                        Meter Series From
                                    </td>
                                    <td style = "text-align:left;">
                                        <asp:TextBox ID="txtMeterNoFrom" CssClass="textarea"  runat="server"
                                            Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Meter Series To</td>
                                    <td style = "text-align:left;">
                                        <asp:TextBox ID="txtMeterNoTo" CssClass="textarea"  runat="server"
                                            Width="190px"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        Posting Date From</td>
                                    <td style = "text-align:left;">
                                         <asp:TextBox ID="txtPostingDateFrom" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPostingDateFrom"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        Posting Date To
                                    </td>
                                    <td style = "text-align:left;">
                                        <asp:TextBox ID="txtPostingDateTo" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtPostingDateTo"
                                            Format="dd mmm yyyy" />
                                    </td>
                                </tr>
                                  
                           
                                  
                                <tr>
                                    <td>
                                        Material Doc No.</td>
                                    <td style = "text-align:left;">
                                        <asp:TextBox ID="txtMaterialDocNo" CssClass="textarea"  runat="server"
                                            Width="190px"></asp:TextBox>
                                    </td>
                                       <td>
                                        Vendor Name
                                    </td>
                                    <td style = "text-align:left;">
                                        <asp:DropDownList ID="ddlVendorName" runat="server" CssClass="textarea" 
                                            Width="190px" onselectedindexchanged="ddlVendorName_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                  
                           
                                  
                                  <tr id="tr2" runat="server">
                                
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>                             

                                <tr>
                                    <td colspan="4" align="center">                                        
                                        <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnSave_Click" OnClientClick="showProgress()" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Refresh" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnCancel_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="submit" Height="30px"
                                            Visible="false" Width="150px" OnClick="btnExit_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 15px; text-align:left;" class="style1" >

                                        <strong>Note:- Please Click on Show Button, to View Data.</strong></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 15px">
                                        &nbsp;</td>
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
                            <div style="overflow: scroll; width: 99.9%; max-height: 270px">

                            <asp:GridView EmptyDataText="No Record Found" ID="gvMainData" AllowSorting="True" 
                                runat="server" Width="100%" AutoGenerateColumns="False" OnSorting="gvMainData_Sorting"
                                OnSelectedIndexChanged="gvMainData_SelectedIndexChanged" BackColor="White" 
                                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellSpacing = "2"
                                    GridLines="None">
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkb1" runat="server" OnCheckedChanged="sellectAll" 
                                                AutoPostBack="true" />
                                              <%--  <input id="Checkbox2" type="checkbox" onclick="CheckAll(this)" runat="server" />--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                      <%--  <asp:CheckBox ID="ItemCheckBox" runat="server" />--%>
                                            <asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="sellectOne" AutoPostBack="true" />                                           
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>                                    
                                    <asp:BoundField DataField="MBLNR_MATERIAL_DOC_NO" HeaderText="Mat. Doc No." ItemStyle-HorizontalAlign="Left"
                                        SortExpression="MBLNR_MATERIAL_DOC_NO" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                     <asp:BoundField DataField="MATNR_MATERIAL_NO" HeaderText="Material No." ItemStyle-HorizontalAlign="Left"
                                        SortExpression="MENGE_QUANTITY" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                     <asp:BoundField DataField="MAKTX_MATERIAL_DESC" HeaderText="Material Desc." ItemStyle-HorizontalAlign="Left"
                                        SortExpression="MAKTX_MATERIAL_DESC" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SERNR_SERIAL_NO" HeaderText="Meter No." ItemStyle-HorizontalAlign="Left"
                                        SortExpression="SERNR_SERIAL_NO" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                     <asp:BoundField DataField="SCHEME_NUMBER" HeaderText="Scheme No." ItemStyle-HorizontalAlign="Left"
                                        SortExpression="SCHEME_NUMBER" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                     <asp:BoundField DataField="BUDAT_POSTING_DT" HeaderText="Posting Date" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="BUDAT_POSTING_DATE" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                     <asp:BoundField DataField="CPUDT_DATE" HeaderText="CPUDT Date" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="CPUDT_DAY" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>


                                    <asp:BoundField DataField="XBLNR_REFERENCE_DOC" HeaderText="Ref. Doc." ItemStyle-HorizontalAlign="Left"
                                        SortExpression="XBLNR_REFERENCE_DOC" HeaderStyle-Font-Underline="true" >                                    
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="WERKS_PLANT" HeaderText="Plant" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="WERKS_PLANT" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    
                                   
                                     <asp:BoundField DataField="LIFNR_ACCOUNT_NO" HeaderText="Vendor" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="LIFNR_ACCOUNT_NO" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>                                    
                                   
                                    <asp:BoundField DataField="BUKRS_COMP_CODE" HeaderText="Company" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="BUKRS_COMP_CODE" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="ALLOTED_TO" HeaderText="Assign To" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="ALLOTED_TO" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ALLOTED_DATE" HeaderText="Allotted Date" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="ALLOTED_DATE" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                  
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <HeaderStyle HorizontalAlign="Center" Height="35px" BackColor="#008dde" 
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
                <tr id="tr3" runat="server">
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr id="tr6" runat="server">
                    <td align="center">
                        <div style="background-color: #DADADA; height: 30px; vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;Vendor wise Allotements Details
                            </div>
                        </div>
                        <br />
                        <div style="width: 100%">
                            <table width="95%">
                                <tr>
                                    <td style="width: 70%">
                                        <div id = "divVendorList" runat = "server" style="overflow: scroll; width: 100%; height: 350px" visible = "false">  <%--09032018--%>
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
                                                   <%-- <asp:BoundField DataField="SealAlloted" HeaderText="Pending Seals" ItemStyle-HorizontalAlign="Center" />--%>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Center" Height="35px" />
                                            </asp:GridView>

                                        </div>
                                    </td>
                                    <td style="width: 50%">                                    
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
                        
                        <table runat="server" id="TabDropData" visible="false" width=100%>
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
               <div class="modal" style="clear:both; text-align:center; width:100%;">
              <div class="center" style="text-align:center;margin-top:170px;">
                <img alt="" src="Image/pleasewait.gif" />
                </div>
                </div>
                </ProgressTemplate>
                </asp:UpdateProgress>
</asp:Content>

