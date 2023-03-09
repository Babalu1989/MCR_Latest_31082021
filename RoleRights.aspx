<%@ Page Language="C#" MasterPageFile="SiteMaster.master" AutoEventWireup="true"
    ClientIDMode="Static" CodeFile="RoleRights.aspx.cs" Inherits="_Default" Title="Add User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                        Company
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCompany" runat="server" Width="190px" Height="23px">
                                            <asp:ListItem Selected="True" Text="--Select One--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="BYPL" Value="BYPL"></asp:ListItem>
                                            <asp:ListItem Text="BRPL" Value="BRPL"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Role Name
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRoleID" runat="server" Width="190px" Height="23px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlRoleID_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">
                                        <br />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="submit" Height="30px"
                                            Width="150px" onclick="btnSave_Click" />
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
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                         <div id="GridView1_div"  runat="server" style="width:100%;">
                    <div id="GHead"></div>

                     <div style="width:100%; height: 485px; overflow:auto;" > 
                            <asp:GridView ID="gvMainData" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                Width="100%" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No.">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <asp:Label ID="lblFormCode" runat="server" Visible="false" Text='<%#Eval("PAGE_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Page_title" HeaderText="Page Title" />
                                    <asp:TemplateField HeaderText="Allow">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdbtnAllow" runat="server" GroupName="ab" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Not Allowed">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdbtnNone" runat="server" GroupName="ab" />
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
