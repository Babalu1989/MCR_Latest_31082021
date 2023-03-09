<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenerateMCR.aspx.cs" Inherits="GenerateMCR" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="Script/jquery-1.9.1.js" type="text/javascript"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style5
        {
            font-size: 10pt;
        }
        .style10
        {
            font-size: 8pt;
        }
        .style16
        {
            font-size: small;
        }
    </style>

    <script type="text/javascript" language ="javascript">
    <!--
        function printPartOfPage(elementId) {
            var printContent = document.getElementById(elementId);
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        }
        

    // -->
    </script>  
</head>
<body style="font-family: Verdana">
    <form id="form1" runat="server">
    
<div id="Main">
 
    <div id="printACk" runat = "server">
        <table width = "100%" border="1" id="PrintTable" runat="Server" clientidmode="Static">
        <tr>
        <td style="text-align: center;">

        <table width="100%" cellpadding="0" cellspacing="0">
        <tr><td align="center"> 
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Image/BSES.jpg" Height="23px" Width="116px" />
        </td></tr>
        <tr style="height:8px;">
        <td></td>
        </tr>
        <tr><td align="center">
        <span class="style5"><strong>METER PARTICULAR SHEET</strong></span>
        </td></tr>
        </table>                
        </td>
        </tr>                      
            <tr>
                <td>
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Order No:</span> </td>
                <td>  <asp:Label ID="lblOrderNo" runat="server" Text="" CssClass="style10"></asp:Label> </td>
                </tr>  

                <tr>
                <td> 
                    <span class="style16">Contract Account:</span></td>
                <td>  <asp:Label ID="lblCA_Number" runat="server" Text="" CssClass="style10"></asp:Label> </td>
                </tr>  

                <tr>
                <td> 
                    <span class="style16">Activity Type:</span></td>
                <td>  <asp:Label ID="lblActType" runat="server" CssClass="style10"></asp:Label> </td>
                </tr>  

                <tr>
                <td> 
                    <span class="style16">Division:</span></td>
                <td>  <asp:Label ID="lblDivision" runat="server" CssClass="style10"></asp:Label> </td>
                </tr>  

                <tr>
                <td> 
                    <span class="style16">Date of Activity:</span></td>
                <td>  <asp:Label ID="lblDateOfActivity" runat="server" CssClass="style10"></asp:Label> </td>
                </tr>    

                <tr>
                <td> 
                    <span class="style16">Account Class:</span></td>
                <td>  <asp:Label ID="lblAccountClass" runat="server" CssClass="style10"></asp:Label> </td>
                </tr>

                </table>
                </td>                
            </tr>
            
                    
            <tr>
                <td >
                   
                    <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Category:</span> </td>
                <td>  <asp:Label ID="lblCategory" runat="server" CssClass="style10"></asp:Label> </td>
                </tr>  

                  <tr>
                <td><span class="style16">Sanctioned Load:</span> </td>
                <td>  <asp:Label ID="lblSanctionLoad" runat="server" CssClass="style10"></asp:Label> </td>
                </tr>  
                  <tr>
                <td><span class="style16">Consumer Name:</span> </td>
                <td>  <asp:Label ID="lblConsumerName" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 
                <tr>
                <td><span class="style16">Father/Husband Name:</span> </td>
                <td>  <asp:Label ID="lblFatherHusbName" runat="server" CssClass="style10"></asp:Label> </td>
                </tr>  
                    </table>
                </td>
            </tr>

            <tr>
                <td >                 
                  <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Address:</span> </td>
                <td>  <asp:Label ID="lblAddress" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 
                </table>       
                </td>
            </tr>
             <tr>
                <td >                 
                  <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Mobile No:</span> </td>
                <td>  <asp:Label ID="lblMobile" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 
                </table>       
                </td>
            </tr>
            <tr>
                <td style="text-align: center"> 
                <strong><span class="style16">New Meter Details</span></strong>
                </td>
                </tr>
            <tr>
                <td>
                     <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Meter No:</span> </td>
                <td>  <asp:Label ID="lblNewMeterNo" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 
                </table> 
                    </td>
                </tr>
            <tr>
                <td >
                    <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Reading:</span> </td>
                <td>  <asp:Label ID="lblNewReading" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">KWH:</span> </td>
                <td>  <asp:Label ID="lblNewKWH" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">KVAH:</span> </td>
                <td style="margin-left: 40px">  <asp:Label ID="lblNewKVAH" runat="server" 
                        CssClass="style10"></asp:Label> </td>
                </tr> 


                </table> 
                    
                    </td>
                </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Terminal Seal-1:</span> </td>
                <td>  <asp:Label ID="lblNewTermSeal1" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">Terminal Seal-2:</span> </td>
                <td>  <asp:Label ID="lblNewTermSeal2" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                </table>  
                    
                    </td>
                </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Meter Box Seal-1:</span> </td>
                <td>  <asp:Label ID="lblNewMeterBoxSeal1" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">Meter Box Seal-2:</span> </td>
                <td>  <asp:Label ID="lblNewMeterBoxSeal2" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                </table>  
                    </td>
                </tr>
            <tr>
                <td >
                     <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16"><strong>Cable Details</strong></span> </td>
                <td>  &nbsp;</td>
                </tr> 

                <tr>
                <td><span class="style16">Cable Size:</span> </td>
                <td>  <asp:Label ID="lblNewCableSize" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">Cable Length (m):</span> </td>
                <td>  <asp:Label ID="lblNewCableLen" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 


                <tr>
                <td><span class="style16">O/P Cable:</span></td>
                <td>  <asp:Label ID="lblOPCable" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 


                </table> 
                    </td>
                </tr>
             <tr>
                <td style="text-align: center"> 
                <strong><span class="style16">Removed Meter Details (If applicable)</span></strong>
                </td>
                </tr>
                <tr>
                <td>
                      <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Meter No:</span> </td>
                <td>  <asp:Label ID="lblOldMeterNo" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 
                </table> 
                    </td>
                </tr>
            <tr>
                <td > 
                    
                     <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Reading:</span> </td>
                <td>  <asp:Label ID="lblOldReading" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">KWH:</span> </td>
                <td>  <asp:Label ID="lblOldKWH" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">KVAH:</span> </td>
                <td>  <asp:Label ID="lblOldKVAH" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 


                </table> 
                    </td>
                </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Terminal Seal-1:</span> </td>
                <td>  <asp:Label ID="lblOldTermSeal1" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">Terminal Seal-2:</span> </td>
                <td>  <asp:Label ID="lblOldTermSeal2" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                </table>  
                    
                    </td>
                </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16">Meter Box Seal-1:</span> </td>
                <td>  <asp:Label ID="lblOldMeterBoxSeal1" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">Meter Box Seal-2:</span> </td>
                <td>  <asp:Label ID="lblOldMeterBoxSeal2" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                </table>  
                    </td>
                </tr>

            <tr>
                <td >
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td class="style16"><strong>Removed Cable Details</strong></td>
                <td>  &nbsp;</td>
                </tr> 

                <tr>
                <td><span class="style16">Cable Size:</span> </td>
                <td>  <asp:Label ID="lblOldCableSize" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">Cable Length (m):</span> </td>
                <td>  <asp:Label ID="lblOldCableLen" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 


                </table> 
                </td>
                </tr>

               <tr>
                <td >
                 <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16"><strong>Gunny Bag Details</strong></span> </td>
                <td>  &nbsp;</td>
                </tr> 

                <tr>
                <td><span class="style16">Bag No:</span> </td>
                <td>  <asp:Label ID="lblBagNo" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 

                <tr>
                <td><span class="style16">Bag Seal No:</span> </td>
                <td>  <asp:Label ID="lblBagSealNo" runat="server" CssClass="style10"></asp:Label> </td>
                </tr> 
                <tr>
                <td>
              <span class="style16">  Testing Date </span>
                </td>
                <td>
                 <asp:Label ID="lblTestingDate" runat="server" CssClass="style10"></asp:Label>
                </td>
                </tr>
                 <tr>
                <td>
              <span class="style16"> Notice No </span>
                </td>
                <td>
                 <asp:Label ID="lblNoticeNo" runat="server" CssClass="style10"></asp:Label>
                </td>
                </tr>
                </table> 
                </td>
                </tr>

            <tr>
                <td > 
                <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                <td><span class="style16"> Consumer Signature:</span>
                    <asp:Image ID="imgConsumerSign" runat="server" Height="100px" Width="300px" />
                </td>                
                </tr>
                
                </table>

                   </td>
                </tr>
            <tr>
                <td >
                <span class="style16"> BSES Representative ID/Sign:</span>&nbsp;&nbsp;
                 <asp:Label ID="lblEngID" runat="server" CssClass="style10"></asp:Label>
                    </td>
                </tr>
            <tr>
                <td style="text-align: center"> 
                     <span class="style16"> This is a system generated document & does not need any signature.</span>
                    </td>
                </tr>                       
            </table>
    </div>
   
   <div id="Action">
    <table width="100%">
    <tr>
    <td style="text-align: center">
        <input id="Button1" type="button" style="visibility:hidden" value="Print" onclick="javaScript:printPartOfPage('printACk');"/>

            <asp:Button ID="BtnPrint" runat="server" Text="Print" Width="100px" 
            onclick="btnPrint_Click" Visible="false" />
            <asp:ImageButton ID="ImgPrintBtn" runat="server" 
            ImageUrl="~/Image/print.png" onclick="ImgPrintBtn_Click" />
             <asp:ImageButton ID="ImgPdf" runat="server"  Visible="false"
            ImageUrl="~/Image/pdf_icon.gif" onclick="ImgPdf_Click" />
        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px"  Visible="false"
            onclick="btnSave_Click" />
    </td>
    </tr>
    </table>
    </div>

    </div>


    </form>
</body>
</html>
