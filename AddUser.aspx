<%@ Page Language="C#" MasterPageFile="SiteMaster.master" AutoEventWireup="true"
    ClientIDMode="Static" CodeFile="AddUser.aspx.cs" Inherits="_Default" Title="Add User" %>

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
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css"
        rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js"
        type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstFruits]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divclass">
        <center>
            <table width="100%">
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr id="tr2" runat="server">
                    <td align="center">
                        <div>
                            <table width="98%" style="border: 1px solid black; height:200px;">
                                <tr>
                                    <td colspan="4" style="height: 15px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:small;">
                                        Name<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="textarea" Width="190px" MaxLength="100">
                                        </asp:TextBox>
                                    </td>
                                    <td style="font-size:small;">
                                        User ID<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:TextBox ID="txtUserID" CssClass="textarea" runat="server" Width="190px" MaxLength="20"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:small;">
                                        IMEI No<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:TextBox ID="txtIMEINo" runat="server" CssClass="textarea" Width="190px" MaxLength="18">
                                        </asp:TextBox>
                                          <span>IMEI No 2</span><asp:TextBox ID="txtIMEINo1" runat="server" CssClass="textarea" Width="190px" MaxLength="18">
                                        </asp:TextBox>
                                    </td>
                                    <td style="font-size:small;">
                                        Mobile No.<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:TextBox ID="txtMobNo" runat="server" MaxLength="10" CssClass="textarea" Width="190px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:small;">
                                        Designation<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:TextBox ID="txtDesgnation" runat="server" MaxLength="50" CssClass="textarea"
                                            Width="190px">
                                        </asp:TextBox>
                                    </td>
                                    <td style="font-size:small;">
                                        Division<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:DropDownList ID="ddlDivision" CssClass="textarea" runat="server" 
                                            Width="190px" onselectedindexchanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="tr1" runat="server" visible="false">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="textarea" Width="190px" value="12345678"
                                            Visible="false">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        Company<span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompany" CssClass="textarea" runat="server" ReadOnly="true" Width="190px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-size:small;">
                                        Status<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:DropDownList ID="ddlActivation" CssClass="textarea" runat="server" Width="190px">
                                            <asp:ListItem Text="-Select One-" Selected="True" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="font-size:small;">
                                        Role<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="textarea" Width="190px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                               
                                <tr id="tabv" runat="server" visible="false">
                                     <td style="font-size:small;">
                                        Vendor<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                     <asp:TextBox ID="txtvendor" CssClass="textarea" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr id="trid" runat="server" visible="false" >
                                    <td style="font-size:small;">
                                        Vendor<span style="color: Red;">*</span>
                                    </td>
                                    <td style="font-size:small;">
                                        <asp:DropDownList ID="ddlVendorname" CssClass="textarea" runat="server" Width="190px"
                                         ></asp:DropDownList>
                                    </td>                        
                                </tr>
                                <tr>
                                    <td colspan="4" align="center" style="font-size:small;">
                                        <br />
                                        <asp:Button ID="btnSave" runat="server" Text="Confirm" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnSave_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Refresh" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnCancel_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="submit" Height="30px"
                                            Width="150px" OnClick="btnExit_Click" />
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
                            <div style="width: 100%; height: 400px; overflow: auto;">
                                <asp:GridView ID="gvMainData" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                    Width="100%" AutoGenerateColumns="false" OnSorting="gvMainData_Sorting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EMP_NAME" HeaderText="Name" SortExpression="EMP_NAME"
                                            HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="EMP_ID" HeaderText="User ID" SortExpression="EMP_ID" HeaderStyle-Font-Underline="true"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="IMEI_NO" HeaderText="IMEI No" SortExpression="IMEI_NO"
                                            HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MOBILE_NO" HeaderText="Mobile No" SortExpression="MOBILE_NO"
                                            HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DIVISION" HeaderText="Division" SortExpression="DIVISION"
                                            HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DESIGNATION" HeaderText="Designation" SortExpression="DESIGNATION"
                                            HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="EMP_TYPE" HeaderText="Role" SortExpression="EMP_TYPE"
                                            HeaderStyle-Font-Underline="true" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-Font-Underline="true"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Password Reset" ItemStyle-Width="10%" HeaderStyle-Font-Underline="false"
                                            HeaderStyle-Font-Size="12px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgResetPass" CommandArgument='<%#Eval("EMP_ID")%>' CommandName='<%#Eval("EMP_ID")%>'
                                                    ToolTip="Reset Password" ImageUrl="~/image/reset.png" OnCommand="imgResetPass_Command"
                                                    runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="8%" HeaderStyle-Font-Underline="false"
                                            HeaderStyle-Font-Size="12px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnSelet" runat="server" CommandArgument='<%#Eval("EMP_ID")%>'
                                                    CommandName='<%#Eval("EMP_TYPE")%>' ImageUrl="~/Image/Select.png" Width="30px"
                                                    Height="30px" ToolTip="Update" OnCommand="imgEmpID_Command" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle HorizontalAlign="Center" Height="35px" />
                                </asp:GridView>
                            </div>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>
