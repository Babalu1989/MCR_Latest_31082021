<%@ Page Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="SealAllotment.aspx.cs" Inherits="_Default" Title="Seal Allocation" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        function RadioCheck(rb) {
            var gv = document.getElementById("<%=gvSeriesWiseAllocation.ClientID%>");
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="divclass">
        <center>
            <table id="table_Series_Wise_Allocation" runat="server" width="95%">
                <tr align="center">
                    <td colspan="2" style="height: 15px">
                        <asp:RadioButtonList ID="RbdSealType" runat="server" AutoPostBack="True" Font-Bold="true"
                            onselectedindexchanged="RbdSealType_SelectedIndexChanged" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="P">Plastic Seal</asp:ListItem>
                            <asp:ListItem Value="G">Gunny Seal</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%" align="center">
                        <div style="overflow: scroll; width: 100%; height: 350px">
                            <asp:GridView ID="gvSeriesWiseAllocation" runat="server" CssClass="gvwWhite" AutoGenerateColumns="false"
                                Width="90%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdbtnseriesSelect" AutoPostBack="true" OnCheckedChanged="rdbdiscontinued_CheckedChanged"
                                                runat="server" CssClass="checkbox" onclick="RadioCheck(this);" />
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
                                    <asp:TemplateField HeaderText="Previous seals available with Installer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPSeal" runat="server" Text='<%#Eval("SEALALLOTED")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Previous gunny seals available with Installer">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPSealG" runat="server" Text='<%#Eval("SEALALLOTED")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle Height="40px" />
                            </asp:GridView>
                        </div>
                    </td>
                    <td align="center">
                        <asp:Chart ID="Chart_Meter" runat="server" Height="270px" Width="480px">
                            <Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>

                        <%--<asp:Chart ID="Chart_Gunny" runat="server" Height="270px" Width="480px">
                            <Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>--%>

                        <div>
                            <asp:Label ID="lblChart" runat="server" Text="Seal Reconciliation Chart" Font-Bold="true"></asp:Label>
                            </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 15px">
                    </td>
                </tr>
                <tr id="tr1" runat="server" visible="false">
                    <td colspan="2">
                        <div style="background-color: #DADADA; height: 30px; vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;Installer wise Allotements Details
                            </div>
                        </div>
                    </td>
                </tr>
                <tr id="tr2" runat="server" visible="false">
                    <td colspan="2" style="height: 15px">
                        <asp:HiddenField ID="hdfValue" runat="server" />
                        <asp:HiddenField ID="hdfDivision" runat="server" />
                    </td>
                </tr>
                <tr  id="tr3" runat="server" visible="false">
                    <td align="center" colspan="2">
                        <table width="80%" style="border: 1px solid black;">
                            <tr>
                                <td colspan="4" style="height: 15px">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Name
                                </td>
                                <td>
                                    <asp:Label ID="txtName" runat="server"></asp:Label>
                                </td>
                                <td>
                                    User ID
                                </td>
                                <td>
                                    <asp:Label ID="txtEmployeeID" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Allot Seal From
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSealFrom" runat="server" CssClass="textarea" AutoPostBack="true"
                                        OnTextChanged="txtSealFrom_TextChanged"></asp:TextBox>
                                </td>
                                <td>
                                    Allot Seal To
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSealTo" runat="server" CssClass="textarea" AutoPostBack="true"
                                        OnTextChanged="txtSealTo_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trSealDetail" runat="server" visible="false" style="height: 80PX">
                                <td align="center" style="font-weight: bold; font-size: 16px" colspan="3">
                                    <br />
                                    <asp:Label ID="lblNOAllotSeal" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <br />
                                    <div style="overflow: scroll; width: 60%; height: 80px">
                                        Seal No
                                        <asp:CheckBoxList ID="chboxListSealNO" runat="server" AutoPostBack="true" Width="60%"
                                            OnSelectedIndexChanged="chboxListSealNO_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <br />
                                    <asp:Button ID="btnSubmitSealAllocation" runat="server" Text="Submit" CssClass="submit"
                                        Height="30px" Visible="false" Width="180px" OnClick="btnSubmitSealAllocation_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnCancelSealAllocation" runat="server" Text="Cancel" CssClass="submit"
                                        Height="30px" Visible="false" Width="180px" OnClick="btnCancelSealAllocation_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>
