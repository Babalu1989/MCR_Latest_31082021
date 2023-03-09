<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="frmGISComparisionMIS.aspx.cs" Inherits="Report_frmGISComparisionMIS" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="divclass">
        <center>
            <table style="text-align: center" width="90%">
                <tr>
                    <td style="height: 15px">
                        <h2>
                            GIS Comparison Report</h2>
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
                                    Division
                                </td>
                                <td>
                                    <asp:DropDownList ID="txtDivision" runat="server" CssClass="textarea" AutoPostBack="true"
                                        Width="190px" onselectedindexchanged="txtDivision_SelectedIndexChanged">
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
                                <td>
                                    Activity Date From
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" CssClass="textarea" ReadOnly="true" runat="server"
                                        Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtFromDate" Format="dd mmm yyyy" />
                                </td>
                                <td>
                                    &nbsp;Activity Date To
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" CssClass="textarea" ReadOnly="true" runat="server" Width="190px"></asp:TextBox>
                                    &nbsp;<rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtToDate" Format="dd mmm yyyy" />
                                </td>
                            </tr>
                              <tr>
                                <td>
                                    BP Number
                                </td>
                                <td>
                                   <asp:TextBox ID="txtbpnumber" CssClass="textarea"  runat="server"
                                        Width="190px"></asp:TextBox>
                                </td>
                                <td></td>
                                 <td></td>
                              </tr> 
                            <tr>
                                <td colspan="4" align="center">
                                    <br />
                                     <asp:UpdatePanel runat="server" ID="updatepanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                    <asp:Button ID="btnSave" runat="server" Text="Show" CssClass="submit" Height="30px"
                                        Width="150px" onclick="btnSave_Click"/> <%-- OnClientClick="showProgress()"--%>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Refresh" CssClass="submit" Height="30px"
                                        Width="150px" onclick="btnCancel_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="submit" Height="30px"
                                        Width="150px" />
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
                        <asp:Label ID="lblTotCunt" runat="server"  Visible="false" Font-Bold="true"></asp:Label>
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" onclick="btnExcel_Click"  />
                    </td>
                </tr>
            </table>
            <table style="text-align: center" width="100%">
                <tr>
                    <td align="center">
                          <div id="GridView1_div" runat="server" style="width: 100%;">
                            <div id="GHead">
                            </div>
                            <div style="width: 99.9%; height: 360px; overflow: auto;">
                            <asp:GridView ID="gvMainData" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                Width="100%" AutoGenerateColumns="false">
                                 <Columns>
                                    <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="BP_NO" HeaderText="BP No." ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="CA" HeaderText="CA No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="DEVICENO" HeaderText="Device No" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="LATITUDE" HeaderText="Business Latitude" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="LONGTITUDE" HeaderText="Business Longitude" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="GIS_LAT" HeaderText="MMG Latitude" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="GIS_LONG" HeaderText="MMG Longitude" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="totalLat" HeaderText="Latitude Difference" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="totallang" HeaderText="Longitude Difference" ItemStyle-HorizontalAlign="Left" />                                   
                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" Height="35px" />
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
        <div style="clear:both;"></div>
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

