<%@ Page Title="" Language="C#" MasterPageFile="~/Report/SiteMaster.master" AutoEventWireup="true" CodeFile="frmNewConRequestReport_App.aspx.cs" Inherits="Report_frmNewConRequestReport_App" %>

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
        .style1
        {
            font-size: 10pt;
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
                            MCR Summary Report (App Cases)</h2>
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
                                <td colspan="3">
                                    <asp:DropDownList ID="txtDivision" runat="server" CssClass="textarea" Width="190px">
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
                      <div id="Div1" runat="server" style="width: 100%;" visible = "false">
                            <div id="GHeadH">
                            </div>
                        <div style="overflow: scroll; width: 100%; max-height: 370px">

                            <asp:GridView ID="gvMainData" AllowSorting="True" runat="server"
                                Width="100%" AutoGenerateColumns="False" OnRowCommand="gvMainData_RowCommand" 
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                    GridLines="Both">
                                   <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                    <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left" >
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="TOTAL" HeaderText="Total (A)" ItemStyle-HorizontalAlign="Left" >
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ALLOCATED_IN_APP" HeaderText="Allocated In App" ItemStyle-HorizontalAlign="Left" >
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="METER_INSTALLED_APP" HeaderText="Meter Installed Through App (B)" ItemStyle-HorizontalAlign="Left" >
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Meter Not Installed (App Cases) (C)">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnMeterNotInst" runat="server" Text='<%#Eval("METER_NOT_INSTALLED_APP")%>' CommandName='<%#Eval("DIVISION")%>'
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>                                  
                                </Columns>
                                   <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <HeaderStyle HorizontalAlign="Center" Height="35px" BackColor="#008dde" 
                                       Font-Bold="True" ForeColor="White" />
                                   <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                   <RowStyle ForeColor="Black" BackColor="#EEEEEE" />
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
                        <br />
                        <br />
                        <div id = "DIV2" runat = "server" visible = "false">
                        <strong><font class="style1" size="3">Breakup of Meter Not Installed (App Cases) (<asp:Label ID="lblDivisionDrill1" runat="server" Text=""></asp:Label>
                        </font></strong>)
                        <div style="overflow: scroll; width: 100%; max-height: 370px">

                            <asp:GridView ID="gdvDrill1" AllowSorting="True" runat="server"
                                Width="100%" AutoGenerateColumns="False" OnRowCommand="gdvDrill1_RowCommand" 
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
                                    GridLines="Both">
                                   <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                    <asp:TemplateField HeaderText="PL/PPL/MCD Sealed">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnPL" runat="server" Text='<%#Eval("PL_PPL_MCDSEALED")%>' CommandName = "PL_PPL_MCDSEALED"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <%--<asp:TemplateField HeaderText="ANT">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnANT" runat="server" Text='<%#Eval("ANT")%>' CommandName = "ANT"
                                                ForeColor="Black" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>  --%>  

                                    <asp:TemplateField HeaderText="No responsible person at site/Consumer out of station/wants time">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnConsNotPresent" runat="server" Text='<%#Eval("CONSUMER_OUT_STATION")%>' CommandName = "CONSUMER_OUT_STATION"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <%--<asp:TemplateField HeaderText="Meter not installed due to incorrect information in document">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnWrongInformation" runat="server" Text='<%#Eval("INCORRECT_INFORMATION")%>' CommandName = "INCORRECT_INFORMATION"
                                                ForeColor="Black" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>  --%>  

                                    <%--<asp:TemplateField HeaderText="Cable not provided by consumer(Temp Meter)">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnCableNotProvided" runat="server" Text='<%#Eval("CABLE_NOT_PROVIDED")%>' CommandName = "CABLE_NOT_PROVIDED"
                                                ForeColor="Black" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField> --%>   

                                    <asp:TemplateField HeaderText="ELCB/MCB not installed">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnNoELCB" runat="server" Text='<%#Eval("NO_ELCB")%>' CommandName = "NO_ELCB"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <asp:TemplateField HeaderText="Disputed/Court Case">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnDisputed" runat="server" Text='<%#Eval("Disputed")%>' CommandName = "Disputed"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <%--<asp:TemplateField HeaderText="DB Full">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnDBFull" runat="server" Text='<%#Eval("DB_FULL")%>' CommandName = "DB_FULL"
                                                ForeColor="Black" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField> --%>   

                                    <asp:TemplateField HeaderText="No space for meter installation">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnNoSpace" runat="server" Text='<%#Eval("NO_SPACE")%>' CommandName = "NO_SPACE"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <asp:TemplateField HeaderText="Non-Conforming Area(NX Connection)">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnNonConformingArea" runat="server" Text='<%#Eval("NON_CONFORMING_AREA")%>' CommandName = "NON_CONFORMING_AREA"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <%--<asp:TemplateField HeaderText="CRA">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnCRA" runat="server" Text='<%#Eval("CRA")%>' CommandName = "CRA"
                                                ForeColor="Black" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>   --%> 

                                    <asp:TemplateField HeaderText="RCP Required">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnRCPRequired" runat="server" Text='<%#Eval("RCP_Required")%>' CommandName = "RCP_Required"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <asp:TemplateField HeaderText="Separate line request">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnSeparateLineRequest" runat="server" Text='<%#Eval("SEPARATE_LINE_REQUEST")%>' CommandName = "SEPARATE_LINE_REQUEST"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <asp:TemplateField HeaderText="Under Construction/Vacant Plot">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnUnderConst" runat="server" Text='<%#Eval("UNDER_CONSTRUCTION")%>' CommandName = "UNDER_CONSTRUCTION"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <asp:TemplateField HeaderText="Wiring Incomplete/Panel not installed">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnWiringIncomplete" runat="server" Text='<%#Eval("WIRING_INCOMPLETE")%>' CommandName = "WIRING_INCOMPLETE"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>    

                                    <asp:TemplateField HeaderText="Consumer wants to install meter inside premises">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnInsidePremises" runat="server" Text='<%#Eval("INSIDE_PREMISES")%>' CommandName = "INSIDE_PREMISES"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField>  
                                    
                                    <asp:TemplateField HeaderText="Others">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBtnOthers" runat="server" Text='<%#Eval("OTHER")%>' CommandName = "OTHER"
                                                ForeColor="Blue" CommandArgument='<%#Eval("DIST_CD")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:TemplateField> 
                                                                                                         
                                </Columns>
                                   <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <HeaderStyle HorizontalAlign="Center" Height="35px" BackColor="#008dde" 
                                       Font-Bold="True" ForeColor="White" />
                                   <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                   <RowStyle ForeColor="Black" BackColor="#EEEEEE" />
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
                        <div id = "DIV3" runat = "server" visible = "false"> 
                            <table width = "100%">
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                        <strong><font class="style1" size="3">Case Details (App Cases) (<asp:Label ID="lblDivisionDrill2" runat="server"></asp:Label></font></strong>)            
                                    </td>
                                </tr>
                                <tr>
                                    <td style = "text-align:right;">
                                        <asp:ImageButton ID="btnExcelDrill2" runat="server" 
                                            ImageUrl="~/Image/Excel.png" Height="30px"
                                             Width="80px" Visible="false" onclick="btnExcelDrill2_Click" /> 
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; width: 100%; max-height: 370px">

                            <asp:GridView ID="gdvDrill2" AllowSorting="True" runat="server"
                                Width="100%" AutoGenerateColumns="False" 
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" 
                                BorderWidth="1px">
                                   <AlternatingRowStyle BackColor="#DCDCDC" />
                                   <Columns>
                                       <asp:BoundField DataField="PSTING_DATE" HeaderText="Posting Date" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="ORDERID" HeaderText="Order ID" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="METER_NO" HeaderText="Meter No" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="DIVISION" HeaderText="Division" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="CA_NO" HeaderText="CA No" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="NAME" HeaderText="Name" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="TEL_NO" HeaderText="Mobile No" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="ACCOUNT_CLASS" HeaderText="Account Class" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="REASON" HeaderText="Reason" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="INSTALLER" HeaderText="Installer Name" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="REMARKS" HeaderText="Remarks" />
                                   </Columns>
                                   <Columns>
                                       <asp:BoundField DataField="PUNCHED_DATE" HeaderText="Action Punched Date" />
                                   </Columns>
                                   <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <HeaderStyle HorizontalAlign="Center" Height="35px" BackColor="#008dde" 
                                       Font-Bold="True" ForeColor="White" />
                                   <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                   <RowStyle ForeColor="Black" BackColor="#EEEEEE" />
                                   <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                   <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                   <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                   <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                   <SortedDescendingHeaderStyle BackColor="#000065" />
                            </asp:GridView>

                        </div>                
                                    </td>
                                </tr>
                            </table>                                     
                        
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

