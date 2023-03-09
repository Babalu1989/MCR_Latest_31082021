<%@ Page Language="C#" MasterPageFile="SiteMaster.master" AutoEventWireup="true"
    ClientIDMode="Static" CodeFile="SealReconciliation.aspx.cs" Inherits="_Default"
    Title="Seal Reconciliation Report" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

 <script type="text/javascript" language="javascript">
     $(document).ready(function () {
         var gridHeader = $('#<%=gvMainData.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
         $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
         $('#<%=gvMainData.ClientID%> tr th').each(function (i) {
             // Here Set Width of each th from gridview to new table(clone table) th 
             $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
         });
         $("#GHeadH").append(gridHeader);
         $('#GHeadH').css('position', 'absolute');
         $('#GHeadH').css('top', $('#<%=gvMainData.ClientID%>').offset().top);

     });
    </script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=gvDetails.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=gvDetails.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=gvDetails.ClientID%>').offset().top);

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
            <table style="text-align: center" width="90%">
                <tr>
                    <td style="height: 15px">
                        <h2>
                            Seal Reconciliation Report</h2>
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
                            <tr>
                                <td>
                                    From Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" CssClass="textarea" ReadOnly="true" runat="server"
                                        Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtFromDate" Format="dd mmm yyyy" />
                                </td>
                                <td>
                                    &nbsp;To Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" CssClass="textarea" ReadOnly="true" runat="server" Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtToDate" Format="dd mmm yyyy" />
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
                                 <td>Seal Type</td>
                                <td><asp:RadioButtonList ID="RbdSealType" runat="server"  Font-Bold="true"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="P">Plastic Seal</asp:ListItem>
                            <asp:ListItem Value="G">Gunny Seal</asp:ListItem>
                        </asp:RadioButtonList></td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    
                                     <asp:UpdatePanel runat="server" ID="updatepanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                    <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="submit" Height="30px"
                                        Width="150px" OnClick="btnSave_Click" OnClientClick="showProgress()"/>
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
                    <td style="height: 30px" align="right">
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                      <div id="Div1" runat="server" style="width: 100%;">
                            <div id="GHeadH">
                            </div>
                        <div style="overflow: scroll; width: 100%; max-height: 300px">
                            <asp:GridView ID="gvMainData" AllowSorting="True" runat="server"
                                Width="100%" AutoGenerateColumns="False" 
                                OnRowCommand="gvMainData_RowCommand" BackColor="White" BorderColor="#999999" 
                                BorderStyle="None" BorderWidth="1px" GridLines="Both">
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                    <asp:BoundField DataField="DIVISION" HeaderText="Division" 
                                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Code" 
                                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Vender_name" HeaderText="Vendor Name" 
                                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Seal Issued To Vendor">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnCASESCOUNT" CommandName="CASESCOUNT" runat="server" Text='<%#Eval("Seal_Issued_Vendor")%>'
                                                ForeColor="Black" CommandArgument='<%#Eval("DIVISION")+ "," + Eval("VENDOR_CODE")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Seal Consumed">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnSEALCONSUMED" CommandName="SEALCONSUMED" runat="server"
                                                Text='<%#Eval("SEALCONSUMED")%>' ForeColor="Black" CommandArgument='<%#Eval("DIVISION")+ "," + Eval("VENDOR_CODE")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Seal with Installer">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnSEALPENDING" CommandName="SEALPENDING" runat="server" Text='<%#Eval("Seal_with_Installer")%>'
                                                ForeColor="Black" CommandArgument='<%#Eval("DIVISION")+ "," + Eval("VENDOR_CODE")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Seal Pending">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnSEALNOTASSIGN" CommandName="SEALNOTASSIGN" runat="server"
                                                Text='<%#Eval("Seal_Pendding")%>' ForeColor="Black" CommandArgument='<%#Eval("DIVISION")+ "," + Eval("VENDOR_CODE")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
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
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td style="height: 30px">

                     <table width="100%" cellpadding="0px" cellspacing="0px">
                    <tr>
                    <td align="left">
                        <asp:Label ID="lblReportHead" runat="server" Font-Bold="True" 
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td align="right">
                     <asp:ImageButton ID="imgBtnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" OnClick="imgBtnExcel_Click" />
                    </td>
                    </tr>
                    </table>

                       
                    </td>
                </tr>
                <tr>
                    <td>
                       
                    <div id="GridView1_div"  runat="server" style="width:100%;">
                    <div id="GHead"></div>

                     <div style="width:99.9%; height: 225px; overflow:auto;" > 
                       
                            <asp:GridView ID="gvDetails" AllowSorting="True" runat="server"
                                AutoGenerateColumns="False" OnSorting="gvDetails_Sorting" 
                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                CellSpacing = "2" GridLines="None">
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="2%">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>

<HeaderStyle Width="2%"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   <asp:BoundField DataField="COMP_CODE" HeaderText="Company" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="4%" SortExpression="COMP_CODE" >
<ItemStyle HorizontalAlign="Left" Width="4%"></ItemStyle>
                                    </asp:BoundField>
                                          <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="DIVISION" >                                   
                                  
<HeaderStyle Width="6.5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                  
                                    <asp:BoundField DataField="SEAL" HeaderText="Seal" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="SEAL" >
<HeaderStyle Width="6.5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MATERIAL_CODE" HeaderText="Material Code" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="MATERIAL_CODE" >
                                   
<HeaderStyle Width="6.5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                   
                                    <asp:BoundField DataField="ALLOTED_TO" HeaderText="Allot To" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="ALLOTED_TO" >
<HeaderStyle Width="6.5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                         <asp:BoundField DataField="POSTING_DATE" HeaderText="Posting Date" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%"  SortExpression="POSTING_DATE" >
<HeaderStyle Width="6.5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ALLOTED_DATE" HeaderText="Alloted Date" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="ALLOTED_DATE" >
                                   
<HeaderStyle Width="6.5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                   
                                    <asp:BoundField DataField="PUNCH_DATE" HeaderText="Punch Date" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="PUNCH_DATE" >
<HeaderStyle Width="6.5%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <HeaderStyle HorizontalAlign="Center" Height="35px" Font-Underline="true" 
                                    BackColor="#008dde" Font-Bold="True" ForeColor="White" />
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

         <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="updatepanel1">
                <ProgressTemplate>
              <div class="modal" style="clear:both; text-align:center; width:100%;">
              <div class="center" style="text-align:center;margin-top:170px;">
                <img alt="" src="../Image/pleasewait.gif" />
                </div>
                </div>
                </ProgressTemplate>
                </asp:UpdateProgress>

    </div>
</asp:Content>
