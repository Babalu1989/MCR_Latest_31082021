<%@ Page Title="Seal Manual Installed" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ManualSealInstalled.aspx.cs" Inherits="ManualSealInstalled" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Ajax.Net" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" src="https://www.google.com/jsapi"></script>
   
    <style type="text/css">
        .style1
        {
            width: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajax:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div>
        <center>
            <table  runat="server" width="95%">
                <tr align="center">
                    <td style="height: 15px;" >
                        <h2>
                            Manual Seals Installed and Consumed</h2>
                    </td>
                </tr>
               
                <tr>
                    <td colspan="2" style="height: 15px">
                        <table align="center" cellspacing="1" class="style1">
                            <tr>
                                <td>
                                    Upload File</td>
                                <td>
                                    <input id="FileUpload1" runat="server" name="filMyFile" size="49" type="file" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height:45px;">
                                     <asp:Button ID="btnUpload" runat="server" Text="Upload File" CssClass="submit"
                                        Height="30px" Width="180px" onclick="btnUpload_Click"  />&nbsp;&nbsp;
                                         <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="submit"
                                        Height="30px"  Width="180px" onclick="btnCancel_Click"  />&nbsp;&nbsp;
                                    
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="tr1" runat="server" >
                    <td colspan="2">
                        <div style="background-color:#DADADA; height: 30px; vertical-align: middle">
                            <div style="float: left; color: Black; padding: 5px 5px 0 0">
                                &nbsp;&nbsp;<b>Last Uploaded Manual Seals Installed Files</b>
                            </div>
                        </div>
                    </td>
                </tr>
               
                <tr id="BlankHeader" runat="server">
                    <td colspan="2">
                  
                     <div style="background-color:lightgray; height: 30px; vertical-align: middle">
                            <table width="100%" border="1" cellpadding="0" cellspacing="0" style="border: 1px solid black;">
                     <tr style="height:28px;">
                     <td>SNo.</td>
                     <td>File Name</td>
                     <td>Upload By</td>
                     <td>Date/Time</td>
                     <td>Date & Time</td>
                     <td>View</td>
                     </tr>
                     </table>
                        </div>
                          </td>
                    </tr>
                    <tr>
                    <td colspan="2">
                     <div style="overflow: scroll; width: 100%; height: 400px">
                    
                            <asp:GridView ID="gvUploadFile" runat="server" CssClass="gvwWhite" AutoGenerateColumns="false"
                                Width="100%">
                                <Columns>                                                                       
                                    <asp:BoundField DataField="SNO" HeaderText="Sno">
                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FILE_NAME" HeaderText="File Name" ></asp:BoundField>
                                    <asp:BoundField DataField="FILE_UPLOAD_BY" HeaderText="Upload By" ></asp:BoundField>
                                    <asp:BoundField DataField="FILE_UPLOAD_DATE" HeaderText="Date/Time"></asp:BoundField>
                                   
                                     <asp:TemplateField HeaderText="Download/Show">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkUploadedFileName" runat="server" Text='View'
                                                                    CommandArgument='<%#Eval("FILE_PATH_ADD")%>' CommandName='<%#Eval("FILE_NAME")%>'
                                                                    OnCommand="lnkUploadedFileName_Command" ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                </Columns>
                                <HeaderStyle Height="40px" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>


