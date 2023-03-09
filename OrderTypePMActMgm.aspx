<%@ Page Title="Order Type" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="OrderTypePMActMgm.aspx.cs" Inherits="OrderTypePMActMgm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="divclass">
        <center>
            <table width="95%">
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                     <tr id="tr2" runat="server">
                    <td align="center">
                        <div>
                            <table width="90%" style="border: 1px solid black;">
                                <tr>
                                    <td colspan="4" style="height: 15px">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Order Type</td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="textarea" 
                                             Width="250px" AutoPostBack="True" 
                                             onselectedindexchanged="ddlOrderType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                       PM Activity</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPMActivity" runat="server" CssClass="textarea" Width="250px">
                                       <asp:ListItem>-ALL-</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <br />
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="submit" Height="30px"
                                            Width="150px" onclick="btnSubmit_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Exit" CssClass="submit" Height="30px"
                                            Width="150px" onclick="btnCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 15px">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>          
                <tr>
                    <td align="center">
                         <div id="GridView1_div"  runat="server" style="width:100%;">
                    <div id="GHead"></div>

                     <div style="width:100%; height: 600px; overflow:auto;" > 
                            <asp:GridView ID="gvMainData" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                Width="100%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <asp:Label ID="lblActive" runat="server" Visible="false" Text='<%#Eval("ACTIVE_FLAG")%>'></asp:Label>
                                            <asp:Label ID="lblSAP_FLAG" runat="server" Visible="false" Text='<%#Eval("SAP_SENT_FLAG")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ORDER_TYPE" HeaderText="Order Type" />
                                    <asp:BoundField DataField="ORDER_DESC" HeaderText="Order Desc" />
                                    <asp:BoundField DataField="PM_ACTIVTY" HeaderText="PM Activity" />
                                    <asp:BoundField DataField="PM_DESC" HeaderText="PM Activity Desc" />

                                    <asp:TemplateField HeaderText="Tab-Allow">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdbtnAllow" runat="server" GroupName="ab" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tab-Not Allowed">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdbtnNone" runat="server" GroupName="ab" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SAP-Allow">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdbtnSAPAllow" runat="server" GroupName="cd" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SAP-Not Allowed">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdbtnSAPNone" runat="server" GroupName="cd" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Update">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgUpdate" CommandArgument='<%#Eval("ORDER_TYPE")%>' CommandName='<%#Eval("PM_ACTIVTY")%>'
                                                ImageUrl="~/Image/arrow.png" OnCommand="imgUpdate_Command"
                                                runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                        </div>
                        </div>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>

