<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RnDPage.aspx.cs" Inherits="RnDPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .buttonClass
        {
            padding: 2px 20px;
            text-decoration: none;
            border: solid 1px black;
            background-color: #ababab;
        }
        .buttonClass:hover
        {
            border: solid 1px Black;
            background-color: #ffffff;
        }
        .style1
        {
            color: #FF9933;
        }
        </style>
</head>
<body>
     <form id="form1" runat="server">
    <div>
    <asp:LinkButton ID="LnkButtion" CssClass="buttonClass" runat="server" Text="Button" />

 &nbsp;dd&nbsp;

 <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
<p>Password 
    <asp:TextBox ID="txtpass" TextMode="Password" runat="server"></asp:TextBox> &nbsp;
    <span class="style1">sadjasdn&nbsp;&nbsp; </span>
    <asp:ImageButton ID="ImageButton1" runat="server" 
        onclick="ImageButton1_Click" />
        </p>
    <asp:TextBox ID="txtQueries" TextMode="MultiLine"
    Width="80%" Height="200px" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">LinkButton</asp:LinkButton>
    <asp:Label ID="lblResult" runat="server" Font-Bold="true" Text=""></asp:Label>


 <%--   <div id="divDocumentListGrid" visible="false" runat="server">
        <table border="0" cellpadding="0" cellspacing="3" style="width: 80%; margin-left: 10%;
            border: 1px solid #3986DD;">
            <tbody>
                <tr>
                    <td style="background-color: #3986DD; text-align: center; padding-top: 5px; padding-bottom: 5px">
                        <strong><span style="font-size: 12pt; color: White;">Document List</span></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdDocumentList" runat="server" ShowHeader="true" Style="overflow: auto;"
                            CellPadding="5" ForeColor="#333333" Width="100%" HeaderStyle-CssClass="FixedHeader2"
                            ShowFooter="false" AutoGenerateColumns="false" >
                            <RowStyle Font-Names="Verdana" Font-Size="8pt" BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>                               
                                <asp:BoundField DataField="sn" HeaderText="S.No." HeaderStyle-Width="2%" ItemStyle-Width="2%"
                                    Visible="true">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DOC_NAME" HeaderText="Document Name" Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DOC_TYPE" HeaderText="Document Type" Visible="true">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ENTRY_DT" HeaderText="Uploaded Date" Visible="true">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ENTRY_BY" HeaderText="Uploaded By" Visible="true">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Download/Show">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkUploadedFileName" runat="server" Text='<%#Eval("DOC_NAME")%>'
                                            CommandArgument='<%#Eval("DOC_PHYSICAL_PATH")%>' CommandName='<%#Eval("DOC_NAME")%>'
                                            OnCommand="lnkUploadedFileName_Command" ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                           
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#847D7D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />                           
                            <HeaderStyle BackColor="#3970ca" BorderColor="#3970ca" Font-Bold="false" Font-Names=" Arial, Helvetica, Verdana, sans-serif"
                                Font-Size="9pt" ForeColor="White" CssClass="header3" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />                          
                        </asp:GridView>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>--%>


        <table cellspacing="2" cellpadding="2" width="60%" border="1" style="border-color:Blue;">
            <tr style="background-color:Blue;">
                <td align="center">
                <font color="#FFFFFF;"> <strong> Important Note </strong></font>
                    </td>
            </tr>
            <tr>
                <td>
                <br />
                  Testing is OK on Q-92/100. Pls arrange to transport toP-92/100                  
                  </td>
                  
            </tr>
        </table>


    </div>
    </form>
</body>
</html>
