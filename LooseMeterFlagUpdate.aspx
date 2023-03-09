<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="LooseMeterFlagUpdate.aspx.cs" Inherits="LooseMeterFlagUpdate" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page_dimmer" id="pagedimmer" visible="false" runat="server"> </div>  
    <div class="divclass">
        <center>
            <table width="95%">
                 <tr>
                    <td align="center">
                      
                        
                      
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                      
                        
                      
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px; text-align:center">
                      <h2>
                                <i>Loose Meter Kitting</i></h2>
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
                                        Activity Date From</td>
                                    <td style = "text-align:left;">
                                         <asp:TextBox ID="txtPostingDateFrom" CssClass="textarea" ReadOnly="true" runat="server"
                                            Width="190px"></asp:TextBox>
                                        &nbsp;<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtPostingDateFrom"
                                            Format="dd mmm yyyy" />
                                    </td>
                                    <td>
                                        Activity Date To
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
                                            Width="190px"></asp:TextBox></td>
                                  <td>
                                        Vendor Name
                                    </td>
                                    <td style = "text-align:left;">
                                        <asp:DropDownList ID="ddlVendorName" runat="server" CssClass="textarea" Width="190px" onselectedindexchanged="ddlVendorName_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>                                                     
                                <tr>
                                    <td colspan="4" align="center">                                        
                                        <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnSave_Click" OnClientClick="showProgress()"/>
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
                                                
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="sellectOne" AutoPostBack="true" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>                                    
                                  
                                  <asp:BoundField DataField="NEW_METER" HeaderText="New Meter" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="NEW_METER" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                     <asp:BoundField DataField="ORDERID" HeaderText="Order No" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="ORDERID" HeaderStyle-Font-Underline="true" ItemStyle-Width="8%" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>                                  

                                    <asp:BoundField DataField="NAME1_NAME" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="VEN_CODE" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>  
                                    <asp:BoundField DataField="DIV_NAME" HeaderText="Division" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="DIV_NAME" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>  

                                    <asp:BoundField DataField="MATNR_MATERIAL_NO" HeaderText="Meter Code" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="MATNR_MATERIAL_NO" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                     <asp:BoundField DataField="SCHEME_NUMBER" HeaderText="Scheme" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="SCHEME_NUMBER" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VEN_CODE" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="VEN_CODE" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>  
                                     <asp:BoundField DataField="ACTIVITY_DATE" HeaderText="Activity Date" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="ACTIVITY_DATE" HeaderStyle-Font-Underline="true" ItemStyle-Width="7%" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ORDER_TYPE" HeaderText="Ord. Type" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="ORDER_TYPE" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                     <asp:BoundField DataField="Usr_Resp" HeaderText="User Resp" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="Usr_Resp" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                     <asp:BoundField DataField="Usr_Resp_ID" HeaderText="User Resp ID" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="Usr_Resp_ID" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                     <asp:BoundField DataField="LM_CUSTOMERCA" HeaderText="CA No." ItemStyle-HorizontalAlign="Left"
                                        SortExpression="LM_CUSTOMERCA" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                     <asp:BoundField DataField="ACC_CLASS" HeaderText="Acc Class" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="ACC_CLASS" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>                                                                                                                                                                                      
                                    <asp:BoundField DataField="OLD_METER" HeaderText="Old Meter" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="OLD_METER" HeaderStyle-Font-Underline="true" >                                    
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>                                    
                                     <asp:BoundField DataField="PM_ACTIVITY" HeaderText="PM Activity" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="PM_ACTIVITY" HeaderStyle-Font-Underline="true" ItemStyle-Width="10%" >
<HeaderStyle Font-Underline="True"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>                                     


                                  <%--  <asp:BoundField DataField="Cust_Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="Cust_Name" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PUNCH_NAME" HeaderText="Punch" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="PUNCH_NAME" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                                                                                                                                                                                                                                                                                                                                     
                                    <asp:BoundField DataField="PUNCH_BY" HeaderText="Punch By" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="PUNCH_BY" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>                                    
                                                                                                                                                                                 
                                    <asp:BoundField DataField="BUKRS_COMP_CODE" HeaderText="Company" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="BUKRS_COMP_CODE" HeaderStyle-Font-Underline="true" >
<HeaderStyle Font-Underline="True"></HeaderStyle>
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>--%>
                                                                                   
                                  <asp:TemplateField HeaderText="View" HeaderStyle-Width="3%" ItemStyle-Width="3%" >
                     <ItemTemplate>                                                  
                           <asp:ImageButton ID="imgAction" CommandArgument='<%# Eval("MBLNR_MATERIAL_DOC_NO") %>' CommandName='<%# Eval("NEW_METER") %>'
                            runat="server" Visible="true" 
                       ImageUrl="~/Image/Draft.gif" OnCommand="imgAction_Command" />                                                            
                     </ItemTemplate>
                      </asp:TemplateField>
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
                                        <asp:Button ID="btnSubmit" runat="server" Text="Kitting Confirm" CssClass="submit"
                                            Height="30px" Width="200px" OnClick="btnSubmit_Click" />
                                    </td>
                                </tr>
                            </table>
                        
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

                     <div id="divAttachment" runat="server" visible="false" style="width: 60%;
        height: 300px; left: 22%; top: 220px; height: auto; position: fixed; z-index: 999;
        display: block; cursor: auto;">
        <div style="background: #f5f5f5; min-height: 16px;">           
            <div style="cursor: pointer; float: right; margin-left: 10px; margin-top: -4px;">             
                <asp:ImageButton ID="btnDocClose"  runat="server" ImageUrl="~/Image/Close.png"
                  Style="width: 16px; height: 16px; float: right; margin: 10px 3px 2px 2px;"  onclick="btnDocClose_Click" />
                    </div>
            <div style="cursor: pointer; visibility: hidden; float: right; margin-left: 10px;
                margin-top: -4px;">
                <span style="width: 16px; height: 16px; display: block; float: right; margin: 10px 3px 2px 2px;
                    background: url(images/pin-w.png)center center no-repeat;"></span>
            </div>            
            <h4 style="border:2px solid #3986DD; background: #3986DD; color: #ffffff; padding: 5px; font-size: 16px; font-weight: 500;
                text-align: center; display: block;">
               Loose Meter Case Details</h4>
            <div style="border:4px solid #3986DD; display: block; color: #696969; padding: 10px;">
                <table style="height: 100px; width: 100%;">
                    <tbody>                                               
                                                                      
                        <tr>
                            <td colspan="4" >
                                 <table width="100%">
                                 <tr>
                                 <td><b>Division:</b></td>
                                 <td>
                                     <asp:Label ID="lblDivision" runat="server" ></asp:Label></td>
                                 <td><b>Device No:</b></td>
                                 <td><asp:Label ID="lblDeviceNo" runat="server" ></asp:Label></td>
                                 <td><b>Order ID:</b></td>
                                 <td><asp:Label ID="lblOrderID" runat="server" ></asp:Label></td>
                                 </tr>
                                  <tr>
                                 <td><b>Install Bus Bar:</b></td>
                                 <td>
                                     <asp:Label ID="lblINSTALLEDBUSBAR" runat="server" ></asp:Label></td>
                                 <td><b>Installation:</b></td>
                                 <td><asp:Label ID="lblINSTALLATION" runat="server" ></asp:Label></td>
                                 <td><b>Order Type:</b></td>
                                 <td><asp:Label ID="lblORDER_TYPE" runat="server" ></asp:Label></td>
                                 </tr>
                                <tr>
                                 <td><b>ELCB Installed:</b></td>
                                 <td>
                                     <asp:Label ID="lblELCB_INSTALLED" runat="server" ></asp:Label></td>
                                 <td><b>Activity Date:</b></td>
                                 <td><asp:Label ID="lblACTIVITY_DATE" runat="server" ></asp:Label></td>
                                 <td><b>Bus Bar No:</b></td>
                                 <td><asp:Label ID="lblBUS_BAR_NO" runat="server" ></asp:Label></td>
                                 </tr>
                                  <tr>
                                 <td><b>Customer Name:</b></td>
                                 <td>
                                     <asp:Label ID="lblCustName" runat="server" ></asp:Label></td>
                                 <td><b>Punched Name:</b></td>
                                 <td><asp:Label ID="lblPunchName" runat="server" ></asp:Label></td>
                                 <td><b>Punched By:</b></td>
                                 <td><asp:Label ID="lblPunchedBy" runat="server" ></asp:Label></td>
                                 </tr>
                                  <tr>
                                 <td><b>Install Bus Bar:</b></td>
                                 <td>
                                     <asp:Label ID="Label7" runat="server" ></asp:Label></td>
                                 <td><b>Installation:</b></td>
                                 <td><asp:Label ID="Label8" runat="server" ></asp:Label></td>
                                 <td><b>Order Type:</b></td>
                                 <td><asp:Label ID="Label9" runat="server" ></asp:Label></td>
                                 </tr>
                                
                                 </table>
                             </td>
                        </tr>                                                                                              
                       
                        <tr>
                            <td colspan="4" style="text-align: center;">                              
                            <asp:Button ID="btnDocCancel" CssClass="btnAdd"
                                    runat="server" Text="Close" onclick="btnDocCancel_Click" Width="100px"></asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

</asp:Content>