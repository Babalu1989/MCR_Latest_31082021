<%@ Page Title="" Language="C#" MasterPageFile="~/Report/SiteMaster.master" AutoEventWireup="true" CodeFile="SchedularDetailsRpt.aspx.cs" 
Inherits="Report_SchedularDetailsRpt" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="divclass">
        <center>
            <table style="text-align: center" width="90%">
                <tr>
                    <td>
                        <h2>
                            Schedular Details Report</h2>
                    </td>
                </tr>                                              
                <tr>
                    <td align="right">
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/Image/Excel.png" Height="30px"
                            Width="80px" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div style="overflow: scroll; width: 100%; max-height: 300px">
                            <asp:GridView ID="gvMainData" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                Width="100%" AutoGenerateColumns="false" OnRowCommand="gvMainData_RowCommand">
                                <Columns>                                
                                 <asp:BoundField DataField="SechName" HeaderText="SNo" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SechName" HeaderText="Schedular Name" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="DateTime" HeaderText="Last Run Date/Time" ItemStyle-HorizontalAlign="Left" />                                   
                                   
                                    <asp:TemplateField HeaderText="Total Count">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSchCount" CommandName="SCHCOUNT" runat="server"
                                                Text='<%#Eval("COUNT")%>' ForeColor="Black" CommandArgument='<%#Eval("SechName")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" Height="35px" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td>

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
                       
                            <asp:GridView ID="gvSealDetails" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                AutoGenerateColumns="false" OnSorting="gvSealDetails_Sorting" Visible="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="2%">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   <asp:BoundField DataField="COMP_CODE" HeaderText="Company" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="4%" SortExpression="COMP_CODE" />
                                          <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="DIVISION" />                                                                     
                                    <asp:BoundField DataField="POSTING_DATE" HeaderText="Posting Date" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="POSTING_DATE" />
                                    <asp:BoundField DataField="PLANNER_GROUP" HeaderText="Planner Group" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="PLANNER_GROUP" />                                   
                                    <asp:BoundField DataField="SEAL" HeaderText="Seal" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="SEAL" />
                                         <asp:BoundField DataField="MATERIAL_CODE" HeaderText="Meterial Code" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%"  SortExpression="MATERIAL_CODE" />
                                    <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="VENDOR_CODE" />                                   
                                    <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="VENDOR_NAME" />
                                           <asp:BoundField DataField="CONSUM_SEAL_FLAG" HeaderText="Consume Flag" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="CONSUM_SEAL_FLAG" />
                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" Height="35px" Font-Underline="true" />
                            </asp:GridView>                             

                             <asp:GridView ID="gvInputData" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                AutoGenerateColumns="false" OnSorting="gvInputData_Sorting" Visible="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="2%">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   <asp:BoundField DataField="COMP_CODE" HeaderText="Company" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Width="4%" SortExpression="COMP_CODE" />
                                          <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="DIVISION" />                                   
                                  
                                    <asp:BoundField DataField="PSTING_DATE" HeaderText="Posting Date" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="PSTING_DATE" />

                                    <asp:BoundField DataField="ORDERID" HeaderText="Order No" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="ORDERID" />
                                   
                                    <asp:BoundField DataField="METER_NO" HeaderText="Meter No" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="METER_NO" />

                                         <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%"  SortExpression="VENDOR_CODE" />

                                    <asp:BoundField DataField="CA_NO" HeaderText="CA No." ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="CA_NO" />
                                   
                                    <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="NAME" />
                                         <asp:BoundField DataField="PLANNER_GROUP" HeaderText="Group" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="6.5%" SortExpression="PLANNER_GROUP" />

                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" Height="35px" Font-Underline="true" />
                            </asp:GridView>


                        </div>
                        </div>
                    </td>
                </tr>

                <tr>
                <td>
                
                <table width="50%" cellpadding="0" cellspacing="0">
                <tr>
                <td>
                                   <table width="100%" cellpadding="0" cellspacing="0">
                                   <tr>
                                   <td>Sync Required Date</td>
                                   <td>
                                    <asp:TextBox ID="txtFrmDate" CssClass="textarea" ReadOnly="true" runat="server" Width="100px"></asp:TextBox>
                                    <rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtFrmDate" Format="dd mmm yyyy" />
                                </td>                              
                                <td>Circle
                                </td>
                                <td>
                                <asp:DropDownList ID="ddlCircle" runat="server" CssClass="textarea" Width="110px" >
                                <asp:ListItem Value="S">SOUTH</asp:ListItem>
                                <asp:ListItem Value="W">WEST</asp:ListItem>
                                    </asp:DropDownList></td>
                                   </tr>
                                   </table>
                                    
                        </td>
                                
                <td>
                    <asp:Button ID="BtnSyncData" Width="125px" runat="server" Text="Sync MCR Data" 
                        onclick="BtnSyncData_Click" />
                </td>
                </tr>
                </table>
                </td>
                </tr>
            </table>
        </center>
    </div>

</asp:Content>

