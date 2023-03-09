<%@ Page Title="" Language="C#" MasterPageFile="~/Report/SiteMaster.master" AutoEventWireup="true" CodeFile="MCRSummaryRpt.aspx.cs" 
Inherits="Report_MCRSummaryRpt" %>

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
                            MCR Summary Report</h2>
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
                                    Posting From Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" CssClass="textarea" ReadOnly="true" runat="server"
                                        Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtFromDate" Format="dd mmm yyyy" />
                                </td>
                                <td>
                                    Posting To Date
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
                                <td colspan="4" align="center">
                                    <br />
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
                    <td style="height: 30px" align="right">
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                      <div id="Div2" runat="server" style="width: 100%;">
                            <div id="GHeadH">
                            </div>
                        <div style="overflow: scroll; width: 100%; max-height: 370px">

                            <asp:GridView ID="gvMainData" AllowSorting="True" runat="server"
                                Width="100%" AutoGenerateColumns="False" 
                                BackColor="White" BorderColor="#999999" 
                                BorderStyle="None" BorderWidth="1px" GridLines="Both">
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                     <asp:BoundField DataField="Sno" HeaderText="S.No" 
                                        ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="division" HeaderText="Division" 
                                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Total_Cases" HeaderText="Total Cases" 
                                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Pending_for_Allocation" HeaderText="Pending for Allocation" 
                                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Allotted_Cases" HeaderText="Allotted Cases" 
                                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Completed_Cases" HeaderText="Completed Cases" 
                                        ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Reverted_Cases" HeaderText="Reverted Cases" 
                                        ItemStyle-HorizontalAlign="Left" >
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
                <tr>
                    <td style="height: 15px">
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

