using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SimpleTest;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Report_MCRGeneratePDF : System.Web.UI.Page
{
    SimpleBL objBL = new SimpleBL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                BindDivisioin();
                BindVendor(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor                 
                lblRoleCheck.Text = objBL.checkRole(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"])).ToString().Trim();
                if (Convert.ToInt32(lblRoleCheck.Text.Trim()) > 1 || Convert.ToString(Session["ROLE"]) == "PV")
                {
                    BindOrderType(Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));
                }

                btnSave.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnSave, "") + ";this.value='Please wait....';this.disabled = true;");
            }
        }
        else
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
    }

    public void BindOrderType(string _sVendorID, string _sDivision, string _sRole)
    {
        DataTable _dtBindName = objBL.getOrderTypeDetails(_sVendorID, _sDivision, _sRole);

        if (_dtBindName.Rows.Count > 0)
        {
            ddlOrderType.DataSource = _dtBindName;
            ddlOrderType.DataTextField = "ORDER_DESCRIPTION";
            ddlOrderType.DataValueField = "ORDER_TYPE";
            ddlOrderType.DataBind();
            ddlOrderType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
        }
    }
    public void BindVendor(string Vendorid, string Division, string Roleid)//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    {
        DataTable _dtEmpName = objBL.getInstrallorFullDetails(Vendorid, Division, Roleid);
        if (_dtEmpName.Rows.Count > 0)
        {
            ddlVendorName.DataSource = _dtEmpName;
            ddlVendorName.DataTextField = "VENDOR_NAME";
            ddlVendorName.DataValueField = "VENDOR_ID";
            ddlVendorName.DataBind();
            ddlVendorName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
        }
        if (_dtEmpName.Rows.Count == 1)
            ddlVendorName.SelectedIndex = 1;
    }
    private void Show_NotGenearte_PDF_Data()
    {
        try
        {
            string _gFrom = "", _gTo = "", _gCANO = "", _gOrderNO = "";
            string _sOrdType = string.Empty, _sActType = string.Empty, _sDivision = string.Empty, _sVendorid = string.Empty;

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                if (SimpleMethods.validateDate(txtFromDate.Text, txtToDate.Text) == true)
                {
                    _gFrom = txtFromDate.Text;
                    _gTo = txtToDate.Text;
                }
                else
                {
                    txtFromDate.BackColor = System.Drawing.Color.Yellow;
                    txtToDate.BackColor = System.Drawing.Color.Yellow;
                    SimpleMethods.show("From Date should not be greater than To Date.");
                    return;
                }
            }

            //if (txtCANO.Text.Trim() == "" && txtOrderNo.Text.Trim() == "")
            // {
            //   SimpleMethods.show("Please enter CA Number or Order Number and Try Again..");
            // return;
            //}

            if (txtCANO.Text.Trim() != "")
                _gCANO = txtCANO.Text.Trim();

            if (txtOrderNo.Text.Trim() != "")
                _gOrderNO = txtOrderNo.Text.Trim();

            if (Convert.ToInt32(lblRoleCheck.Text.Trim()) > 1 || Convert.ToString(Session["ROLE"]) == "PV")
            {
                if (ddlOrderType.SelectedItem.Text != "-ALL-")
                    _sOrdType = ddlOrderType.SelectedValue;
                else
                    _sOrdType = ddlOrderType.SelectedItem.Text;

                if (ddlPMActivity.SelectedItem.Text != "-ALL-")
                    _sActType = ddlPMActivity.SelectedValue;
                else
                    _sActType = ddlPMActivity.SelectedItem.Text;
            }

            if (txtDivision.SelectedItem.Text != "-ALL-")
                _sDivision = txtDivision.SelectedValue;
            else
                _sDivision = "";

            if (ddlVendorName.SelectedItem.Text != "-ALL-")//Added by Babalu Kumar 04012021 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
                _sVendorid = ddlVendorName.SelectedValue;
            else
                _sVendorid = "";

            DataTable _dtDetails = objBL.getMCR_PDF_Details(_gFrom, _gTo, _gCANO, _gOrderNO, Session["Divison"].ToString(),
                                     _sOrdType, _sActType, _sVendorid, _sDivision);

            if (_dtDetails.Rows.Count > 0)
            {
                btnExcel.Visible = true;
                btnpdf.Visible = true;
                gvMainData.DataSource = _dtDetails;
                gvMainData.DataBind();
                if (ViewState["Main"] != null)
                {
                    ViewState["Main"] = null;
                }
                ViewState["Main"] = _dtDetails;

                //DetailsData_Format();
            }
            else
            {
                btnExcel.Visible = false;
                gvMainData.DataSource = null;
                gvMainData.DataBind();
                ViewState["Main"] = null;

                SimpleMethods.show("No Data Found.");
            }
        }
        catch (Exception ex)
        {
            SimpleMethods.show("Please Try Again.");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Show_NotGenearte_PDF_Data();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MCRGeneratePDF.aspx");
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("../HomePage.aspx");
    }
    protected void gvMainData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Main"];
        SetSortDirection(SortDireaction);
        if (_dtDetails != null)
        {
            //Sort the data.
            _dtDetails.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            gvMainData.DataSource = _dtDetails;
            gvMainData.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvMainData.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvMainData.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            //gvMainData.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

            DetailsData_Format();
        }



    }
    public string SortDireaction
    {
        get
        {
            if (ViewState["SortDireaction"] == null)
                return string.Empty;
            else
                return ViewState["SortDireaction"].ToString();
        }
        set
        {
            ViewState["SortDireaction"] = value;
        }
    }

    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "ASC")
        {
            _sortDirection = "DESC";
            // sortImage.ImageUrl = "view_sort_ascending.png";

        }
        else
        {
            _sortDirection = "ASC";
            // sortImage.ImageUrl = "view_sort_descending.png";
        }
    }

    public string _sortDirection { get; set; }

    private void DetailsData_Format()
    {
        for (int i = 0; i < gvMainData.Rows.Count; i++)
        {
            gvMainData.Rows[i].Cells[4].Text = gvMainData.Rows[i].Cells[4].Text.TrimStart('0');
            gvMainData.Rows[i].Cells[5].Text = gvMainData.Rows[i].Cells[5].Text.TrimStart('0');
            gvMainData.Rows[i].Cells[6].Text = gvMainData.Rows[i].Cells[6].Text.TrimStart('0');
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DataTable _dtDetails = (DataTable)ViewState["Main"];

        HtmlForm form = new HtmlForm();
        Response.Clear();
        Response.Buffer = true;
        if (_dtDetails.Rows.Count > 0)
        {
            GridView grddetails = new GridView();
            grddetails.DataSource = _dtDetails;
            grddetails.DataBind();
            string filename = "MCR_PDF" + DateTime.Now.ToString() + ".xls";

            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            form.Controls.Add(grddetails);
            this.Controls.Add(form);

            form.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }


    protected void ImageMCRPDF_Command(object sender, CommandEventArgs e)
    {

        ImageButton btn = (ImageButton)sender;
        string[] CommandName = btn.CommandName.Split(',');
        string CommandName1 = CommandName[0];
        string CommandName2 = CommandName[1];

        DataTable _dtPDF = new DataTable();
        if (e.CommandArgument.ToString() == "Order")
        {
            _dtPDF = objBL.GenerateOrder_MCRPDF(txtFromDate.Text, txtToDate.Text, CommandName2, CommandName1);
        }
        else
        {
            _dtPDF = objBL.GenerateLooseOrder_MCRPDF(txtFromDate.Text, txtToDate.Text, CommandName2, CommandName1);
        }

        if (_dtPDF.Rows[0][0].ToString() == "-1")
            return;
        else
        {
            if (e.CommandArgument.ToString() == "Order")
                GeneratePDF(_dtPDF);
            else
                GeneratePDFLoose(_dtPDF);

            Response.Write("<script>alert('MCR PDF Has Been Successfully Created !!')</script>");

            Show_NotGenearte_PDF_Data();
        }
    }

    //private void GeneratePDF(DataTable dt)
    //{
    //    string FileName = string.Empty;
    //    string DeviceNo = string.Empty;
    //    Mcr_Pdf_Model model = new Mcr_Pdf_Model();

    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Rows.Count > 0)
    //        {
    //            foreach (DataRow row in dt.Rows)
    //            {
    //                FileName = row["Customer_Acount"].ToString() != "N/A" ? row["Customer_Acount"].ToString() : "";
    //                DeviceNo = row["DEVICENO"].ToString();

    //                model.ORDER_TYPE = row["ORDER_TYPE"].ToString();
    //                model.Division = row["DIVISION"].ToString();

    //                if (model.ORDER_TYPE == "ZDRP" || model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZDIV" || model.ORDER_TYPE == "ZDRM")
    //                {
    //                    model.MTR_READ_AVAIL = row["MTR_READ_AVAIL"].ToString();
    //                    model.Removed_METER_NO = row["Removed_Meter_No"].ToString();
    //                    model.OLD_MTR_MRKWH = row["OLD_MR_KWH"].ToString();
    //                    model.OLD_MTR_MRKVAH = row["OLD_MR_KVAH"].ToString();
    //                    model.Removed_METER_TERMINALSEAL1 = row["REM_TERMINAL_SEAL"].ToString();
    //                    model.Removed_METER_TERMINALSEAL2 = row["REM_OTHER_SEAL"].ToString();
    //                    model.Removed_METER_METERBOXSEAL1 = row["REM_BOX_SEAL1"].ToString();
    //                    model.Removed_METER_METERBOXSEAL2 = row["REM_BOX_SEAL2"].ToString(); ;
    //                    model.Removed_METER_BUSBARSEAL1 = row["REM_BUSBAR_SEAL1"].ToString();
    //                    model.Removed_METER_BUSBARSEAL2 = row["REM_BUSBAR_SEAL2"].ToString();
    //                    if (row["REM_CABLE_SIZE"].ToString() == null || row["REM_CABLE_SIZE"].ToString() == "")
    //                    {
    //                        model.Removed_METER_Cable_Size = "N/A";
    //                    }
    //                    else
    //                    {
    //                        model.Removed_METER_Cable_Size = row["REM_CABLE_SIZE"].ToString();
    //                    }
    //                    if (row["REM_CABLE_LEN"].ToString() == null || row["REM_CABLE_LEN"].ToString() == "")
    //                    {
    //                        model.Removed_METER_Cable_Length = "N/A";
    //                    }
    //                    else
    //                    {
    //                        model.Removed_METER_Cable_Length = row["REM_CABLE_LEN"].ToString();
    //                    }
    //                    model.GUNNY_BAG_NO = row["GUNNYBAG_OLD"].ToString();
    //                    model.GUNNY_BAG_SEAL_NO = row["GUNNYBAGSEAL_OLD"].ToString();
    //                    model.Removed_METER_Testing_Date = row["LABTESTING_DATE_OLD"].ToString() != "" ? Convert.ToDateTime(row["LABTESTING_DATE_OLD"].ToString()).ToString("dd/MM/yyyy") : "N/A";
    //                    model.Notice_No = row["LAB_TSTNG_NTC"].ToString();
    //                }
    //                model.ORDER_NO = row["ORDERID"].ToString();
    //                model.PM_ACTIVITY = row["PM_ACTIVITY"].ToString();
    //                model.MR_KWH = row["MR_KWH"].ToString();
    //                model.MR_KVAH = row["MR_KVAH"].ToString();
    //                model.ACTIVITY_DATE = row["ACTIVITY_DATE"].ToString() != "" ? Convert.ToDateTime(row["ACTIVITY_DATE"].ToString()).ToString("dd/MM/yyyy") : "N/A";
    //                model.accountClass = row["ACCOUNT_CLASS"].ToString();
    //                model.customerName = row["NAME"].ToString();
    //                model.customerAddress = row["ADDRESS"].ToString();
    //                model.customerMobile = row["TEL_NO"].ToString();
    //                model.FatherName = row["FATHER_NAME"].ToString();
    //                model.SANCTIONED_LOAD = row["SANCTIONED_LOAD"].ToString();
    //                model.METER_NO = row["METER_NO"].ToString();
    //                model.FatherName = row["FATHER_NAME"].ToString();
    //                model.Bus_Bar_Size = row["BUSBARSIZE"].ToString();
    //                model.BUS_BAR_NO = row["BUS_BAR_NO"].ToString();
    //                model.ContractAccount = row["Customer_Acount"].ToString();
    //                model.CableLength = row["CABLELENGTH"].ToString();
    //                model.TERMINALSEAL1 = row["TERMINAL_SEAL"].ToString();
    //                model.TERMINALSEAL2 = row["OTHER_SEAL"].ToString();
    //                model.METERBOXSEAL1 = row["METERBOXSEAL1"].ToString();
    //                model.METERBOXSEAL2 = row["METERBOXSEAL2"].ToString();
    //                model.BUSBARSEAL1 = row["BUSBARSEAL1"].ToString();
    //                model.BUSBARSEAL2 = row["BUSBARSEAL2"].ToString();
    //                model.BUS_BAR_CBL_SIZE = row["B_BAR_CABLE_SIZE"].ToString();
    //                model.BUS_Bar_Cable_Length = row["BUS_BAR_CABLE_LENG"].ToString();
    //                model.BUS_Bar_Output_Cable_Length = row["OUTPUTCABLELENGTH"].ToString();
    //                model.CableSize = row["CABLESIZE2"].ToString();
    //                model.ImagePath = row["IMAGE_SIGNATURE"].ToString();
    //                model.TAB_ID = row["TAB_LOGIN_ID"].ToString();
    //                model.TAB_ID_NAME = row["TAB_LN_ID_NAME"].ToString();

    //                if (model != null)
    //                {
    //                    Document document = new Document(PageSize.A4);
    //                    string FullPath = string.Empty;
    //                    string signature = string.Empty;
    //                    //  string path = @"E:\PDF\";
    //                    //string logo = Directory.GetCurrentDirectory() + @"\BSES.jpg";
    //                    string logo = Server.MapPath("~/Report/BSES.jpg");

    //                    if ((model.ImagePath != "X") && (model.ImagePath.Trim() != ""))
    //                        signature = model.ImagePath;

    //                    FullPath = byteArrayToPDF(FileName);

    //                    BaseFont fntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    //                    iTextSharp.text.Font fnt = new iTextSharp.text.Font(fntHead, 16, 1, BaseColor.RED);

    //                    PdfPTable tableLayout = new PdfPTable(4);
    //                    iTextSharp.text.Font font = new iTextSharp.text.Font(fntHead, 10, iTextSharp.text.Font.NORMAL);
    //                    PdfWriter.GetInstance(document, new FileStream(FullPath, FileMode.Create, FileAccess.ReadWrite));
    //                    document.Open();

    //                    iTextSharp.text.Image BsesLogo = iTextSharp.text.Image.GetInstance(logo);

    //                    iTextSharp.text.Font arial = FontFactory.GetFont("Arial", 28, BaseColor.GRAY);
    //                    tableLayout.AddCell(new PdfPCell(BsesLogo)
    //                    {
    //                        Colspan = 4,
    //                        Border = 0,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase("Rajdhani Power Limited", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Colspan = 4,
    //                        Border = 0,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" METER PARTICULAR SHEET ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Colspan = 4,
    //                        Border = 0,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });

    //                    Paragraph lineseprator = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" --------------------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Order No: " + model.ORDER_NO.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 4,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Contract Account: " + model.ContractAccount + "", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 4,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Activity Type: " + model.PM_ACTIVITY.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        //Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    //if (model.looseFlag != "LOOSE")
    //                    // {
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Division: " + model.Division.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    //}
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Date of Activity: " + model.ACTIVITY_DATE.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Account Class: " + model.accountClass.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        //Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });


    //                    if (model.looseFlag != "LOOSE")
    //                    {
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Sanctioned Load: " + model.SANCTIONED_LOAD.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });
    //                    }
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Consumer Name : " + model.customerName.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Address : " + model.customerAddress.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,

    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });



    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                    if (model.ORDER_TYPE != "ZDRM")
    //                    {
    //                        tableLayout.AddCell(new PdfPCell(new Phrase("        New Meter Details \n Meter No." + model.METER_NO.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: KWH:" + model.MR_KWH + " | KVAH: " + model.MR_KVAH + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Size : " + model.Bus_Bar_Size.ToString() + "" + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Number : " + model.BUS_BAR_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Terminal Seal-1 : " + model.TERMINALSEAL1.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Terminal Seal-2 : " + model.TERMINALSEAL2.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Meter Box Seal-1: " + model.METERBOXSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Meter Box Seal-2: " + model.METERBOXSEAL2 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Seal-1: " + model.BUSBARSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Seal-2: " + model.BUSBARSEAL2 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Cable Details:\n\n Cable Size: " + model.CableSize.ToString() + "  Cable Length(m): " + model.CableLength.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Output Cable Length: " + model.BUS_Bar_Output_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Cable \n\n Cable Size: " + model.BUS_BAR_CBL_SIZE.ToString() + "  Cable Length: " + model.BUS_Bar_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });


    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                    }
    //                    if (model.ORDER_TYPE == "ZDRP" || model.ORDER_TYPE == "ZDRM" || model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZDIV" || model.ORDER_TYPE == "ZMSC")
    //                    {
    //                        tableLayout.AddCell(new PdfPCell(new Phrase("        Removed Meter Details (If applicable) \n Meter No." + model.Removed_METER_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        if (model.MTR_READ_AVAIL.ToLower() == "yes")
    //                        {
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: KWH:" + model.OLD_MTR_MRKWH.ToString() + " | KVAH: " + model.OLD_MTR_MRKVAH.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });
    //                        }
    //                        if (model.MTR_READ_AVAIL.ToLower() == "no")
    //                        {
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: N/V  " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });
    //                        }

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Cable Details: \n\n Cable Size: " + model.Removed_METER_Cable_Size.ToString() + "   Cable Length(m): " + model.Removed_METER_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });


    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                        if (model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZMSC" || model.ORDER_TYPE == "ZDIV")
    //                        {

    //                        }
    //                        else
    //                        {
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Gunny Bag Details: \n\n Bag No: " + model.GUNNY_BAG_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bag Seal No: " + model.GUNNY_BAG_SEAL_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });

    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Testing Date: " + model.Removed_METER_Testing_Date.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });

    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Notice No: " + model.Notice_No.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });

    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        }
    //                    }
    //                    else if (model.ORDER_TYPE == "ZDIN")
    //                    {
    //                        //if (model.Removed_METER_Cable_Size.ToString() == null || model.Removed_METER_Cable_Size.ToString() == "")
    //                        //{
    //                        //    model.Removed_METER_Cable_Size = "N/A";
    //                        //}
    //                        //if (model.Removed_METER_Cable_Length.ToString() == null || model.Removed_METER_Cable_Length.ToString() == "")
    //                        //{
    //                        //    model.Removed_METER_Cable_Length = "N/A";
    //                        //}
    //                        //tableLayout.AddCell(new PdfPCell(new Phrase("Removed Cable Details: \n\n Cable Size: " + model.Removed_METER_Cable_Size.ToString() + "   Cable Length(m): " + model.Removed_METER_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        //{
    //                        //    Border = 0,
    //                        //    Colspan = 3,//Babalu Kumar
    //                        //    Rowspan = 2,
    //                        //    Colspan = 6,
    //                        //    HorizontalAlignment = Element.ALIGN_CENTER
    //                        //});
    //                        //tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });


    //                        tableLayout.AddCell(new PdfPCell(new Phrase("Removed Cable Details: \n\n Cable Size: N/A"+"   Cable Length(m): N/A "+  new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            Colspan = 3,//Babalu Kumar
    //                            Rowspan = 2,
    //                            //Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                    }
    //                    //Phrase phrase = new Phrase("Consumer Signature::", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0)));
    //                    //PdfPCell pdfPCell = new PdfPCell(phrase);
    //                    //tableLayout.AddCell(pdfPCell);
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Consumer Signature:: " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        // Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });

    //                    //tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6 });

    //                    if (signature != "")
    //                    {
    //                        iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(signature);
    //                        myImage.ScaleAbsolute(45f, 45f);
    //                        tableLayout.AddCell(new PdfPCell(myImage)
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });
    //                    }
    //                    else
    //                    {
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" N/A", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });
    //                    }

    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" BSES Representative ID/Sign: " + model.TAB_ID.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        // Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" BSES Representative Name: " + model.TAB_ID_NAME.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        // Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });

    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" This is a system generated document & does not need \n any signature. To download another copy, visit link http://125.22.84.50:7871/mams/applogin.aspx, Enter CA no. as your user ID and Meter number as a password. Team BRPL :     ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        // Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                    if (document.Add(tableLayout))
    //                    {
    //                        objBL.Update_MCR_ManualPDF(DeviceNo);
    //                        objBL.Update_MCRManualPDF(FullPath, DeviceNo);
    //                    }
    //                    document.Close();
    //                }
    //            }
    //        }
    //    }
    //}

    //private void GeneratePDFLoose(DataTable dt)
    //{
    //    string FileName = string.Empty;
    //    string DeviceNo = string.Empty;
    //    Mcr_Pdf_Model model = new Mcr_Pdf_Model();

    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Rows.Count > 0)
    //        {
    //            foreach (DataRow row in dt.Rows)
    //            {
    //                FileName = row["Customer_Acount"].ToString() != "N/A" ? row["Customer_Acount"].ToString() : "";
    //                DeviceNo = row["DEVICENO"].ToString();

    //                model.ORDER_TYPE = row["ORDER_TYPE"].ToString();
    //                model.Division = row["DIVISION"].ToString();

    //                if (model.ORDER_TYPE == "ZDRP" || model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZDIV" || model.ORDER_TYPE == "ZDRM")
    //                {
    //                    model.MTR_READ_AVAIL = row["MTR_READ_AVAIL"].ToString();
    //                    model.Removed_METER_NO = row["Removed_Meter_No"].ToString();
    //                    model.OLD_MTR_MRKWH = row["OLD_MR_KWH"].ToString();
    //                    model.OLD_MTR_MRKVAH = row["OLD_MR_KVAH"].ToString();
    //                    model.Removed_METER_TERMINALSEAL1 = row["REM_TERMINAL_SEAL"].ToString();
    //                    model.Removed_METER_TERMINALSEAL2 = row["REM_OTHER_SEAL"].ToString();
    //                    model.Removed_METER_METERBOXSEAL1 = row["REM_BOX_SEAL1"].ToString();
    //                    model.Removed_METER_METERBOXSEAL2 = row["REM_BOX_SEAL2"].ToString(); ;
    //                    model.Removed_METER_BUSBARSEAL1 = row["REM_BUSBAR_SEAL1"].ToString();
    //                    model.Removed_METER_BUSBARSEAL2 = row["REM_BUSBAR_SEAL2"].ToString();
    //                    if (row["REM_CABLE_SIZE"].ToString() == null || row["REM_CABLE_SIZE"].ToString() == "")
    //                    {
    //                        model.Removed_METER_Cable_Size = "N/A";
    //                    }
    //                    else
    //                    {
    //                        model.Removed_METER_Cable_Size = row["REM_CABLE_SIZE"].ToString();
    //                    }
    //                    if (row["REM_CABLE_LEN"].ToString() == null || row["REM_CABLE_LEN"].ToString() == "")
    //                    {
    //                        model.Removed_METER_Cable_Length = "N/A";
    //                    }
    //                    else
    //                    {
    //                        model.Removed_METER_Cable_Length = row["REM_CABLE_LEN"].ToString();
    //                    }
    //                    model.GUNNY_BAG_NO = row["GUNNYBAG_OLD"].ToString();
    //                    model.GUNNY_BAG_SEAL_NO = row["GUNNYBAGSEAL_OLD"].ToString();
    //                    model.Removed_METER_Testing_Date = row["LABTESTING_DATE_OLD"].ToString() != "" ? Convert.ToDateTime(row["LABTESTING_DATE_OLD"].ToString()).ToString("dd/MM/yyyy") : "N/A";
    //                    model.Notice_No = row["LAB_TSTNG_NTC"].ToString();
    //                }
    //                model.ORDER_NO = row["ORDERID"].ToString();
    //                model.PM_ACTIVITY = row["PM_ACTIVITY"].ToString();
    //                model.MR_KWH = row["MR_KWH"].ToString();
    //                model.MR_KVAH = row["MR_KVAH"].ToString();
    //                model.ACTIVITY_DATE = row["ACTIVITY_DATE"].ToString() != "" ? Convert.ToDateTime(row["ACTIVITY_DATE"].ToString()).ToString("dd/MM/yyyy") : "N/A";
    //                model.accountClass = row["ACCOUNT_CLASS"].ToString();
    //                model.customerName = row["NAME"].ToString();
    //                model.customerAddress = row["ADDRESS"].ToString();
    //                model.customerMobile = row["TEL_NO"].ToString();
    //                model.FatherName = row["FATHER_NAME"].ToString();
    //                model.SANCTIONED_LOAD = row["SANCTIONED_LOAD"].ToString();
    //                model.METER_NO = row["METER_NO"].ToString();
    //                model.FatherName = row["FATHER_NAME"].ToString();
    //                model.Bus_Bar_Size = row["BUSBARSIZE"].ToString();
    //                model.BUS_BAR_NO = row["BUS_BAR_NO"].ToString();
    //                model.ContractAccount = row["Customer_Acount"].ToString();
    //                model.CableLength = row["CABLELENGTH"].ToString();
    //                model.TERMINALSEAL1 = row["TERMINAL_SEAL"].ToString();
    //                model.TERMINALSEAL2 = row["OTHER_SEAL"].ToString();
    //                model.METERBOXSEAL1 = row["METERBOXSEAL1"].ToString();
    //                model.METERBOXSEAL2 = row["METERBOXSEAL2"].ToString();
    //                model.BUSBARSEAL1 = row["BUSBARSEAL1"].ToString();
    //                model.BUSBARSEAL2 = row["BUSBARSEAL2"].ToString();
    //                model.BUS_BAR_CBL_SIZE = row["B_BAR_CABLE_SIZE"].ToString();
    //                model.BUS_Bar_Cable_Length = row["BUS_BAR_CABLE_LENG"].ToString();
    //                model.BUS_Bar_Output_Cable_Length = row["OUTPUTCABLELENGTH"].ToString();
    //                model.CableSize = row["CABLESIZE2"].ToString();
    //                model.ImagePath = row["IMAGE_SIGNATURE"].ToString();
    //                model.TAB_ID = row["TAB_LOGIN_ID"].ToString();
    //                model.TAB_ID_NAME = row["TAB_LN_ID_NAME"].ToString();

    //                if (model != null)
    //                {
    //                    Document document = new Document(PageSize.A4);
    //                    string FullPath = string.Empty;
    //                    string signature = string.Empty;
    //                    //  string path = @"E:\PDF\";
    //                    //string logo = Directory.GetCurrentDirectory() + @"\BSES.jpg";
    //                    string logo = Server.MapPath("~/Report/BSES.jpg");

    //                    if ((model.ImagePath != "X") && (model.ImagePath.Trim() != ""))
    //                        signature = model.ImagePath;

    //                    FullPath = byteArrayToPDF(FileName);

    //                    BaseFont fntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    //                    iTextSharp.text.Font fnt = new iTextSharp.text.Font(fntHead, 16, 1, BaseColor.RED);

    //                    PdfPTable tableLayout = new PdfPTable(4);
    //                    iTextSharp.text.Font font = new iTextSharp.text.Font(fntHead, 10, iTextSharp.text.Font.NORMAL);
    //                    PdfWriter.GetInstance(document, new FileStream(FullPath, FileMode.Create, FileAccess.ReadWrite));
    //                    document.Open();

    //                    iTextSharp.text.Image BsesLogo = iTextSharp.text.Image.GetInstance(logo);

    //                    iTextSharp.text.Font arial = FontFactory.GetFont("Arial", 28, BaseColor.GRAY);
    //                    tableLayout.AddCell(new PdfPCell(BsesLogo)
    //                    {
    //                        Colspan = 4,
    //                        Border = 0,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase("Rajdhani Power Limited", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Colspan = 4,
    //                        Border = 0,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" METER PARTICULAR SHEET ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Colspan = 4,
    //                        Border = 0,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });

    //                    Paragraph lineseprator = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" --------------------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Order No: " + model.ORDER_NO.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 4,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Contract Account: " + model.ContractAccount + "", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 4,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Activity Type: " + model.PM_ACTIVITY.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        //Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    //if (model.looseFlag != "LOOSE")
    //                    // {
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Division: " + model.Division.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    //}
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Date of Activity: " + model.ACTIVITY_DATE.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Account Class: " + model.accountClass.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        //Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });


    //                    if (model.looseFlag != "LOOSE")
    //                    {
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Sanctioned Load: " + model.SANCTIONED_LOAD.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });
    //                    }
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Consumer Name : " + model.customerName.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Address : " + model.customerAddress.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,

    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });



    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                    if (model.ORDER_TYPE != "ZDRM")
    //                    {
    //                        tableLayout.AddCell(new PdfPCell(new Phrase("        New Meter Details \n Meter No." + model.METER_NO.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: KWH:" + model.MR_KWH + " | KVAH: " + model.MR_KVAH + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Size : " + model.Bus_Bar_Size.ToString() + "" + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Number : " + model.BUS_BAR_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Terminal Seal-1 : " + model.TERMINALSEAL1.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Terminal Seal-2 : " + model.TERMINALSEAL2.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Meter Box Seal-1: " + model.METERBOXSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Meter Box Seal-2: " + model.METERBOXSEAL2 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Seal-1: " + model.BUSBARSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Seal-2: " + model.BUSBARSEAL2 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Cable Details:\n\n Cable Size: " + model.CableSize.ToString() + "  Cable Length(m): " + model.CableLength.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Output Cable Length: " + model.BUS_Bar_Output_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Cable \n\n Cable Size: " + model.BUS_BAR_CBL_SIZE.ToString() + "  Cable Length: " + model.BUS_Bar_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });


    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                    }
    //                    if (model.ORDER_TYPE == "ZDRP" || model.ORDER_TYPE == "ZDRM" || model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZDIV" || model.ORDER_TYPE == "ZMSC")
    //                    {
    //                        tableLayout.AddCell(new PdfPCell(new Phrase("        Removed Meter Details (If applicable) \n Meter No." + model.Removed_METER_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });

    //                        if (model.MTR_READ_AVAIL.ToLower() == "yes")
    //                        {
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: KWH:" + model.OLD_MTR_MRKWH.ToString() + " | KVAH: " + model.OLD_MTR_MRKVAH.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });
    //                        }
    //                        if (model.MTR_READ_AVAIL.ToLower() == "no")
    //                        {
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: N/V  " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });
    //                        }

    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" Cable Details: \n\n Cable Size: " + model.Removed_METER_Cable_Size.ToString() + "   Cable Length(m): " + model.Removed_METER_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });


    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                        if (model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZMSC" || model.ORDER_TYPE == "ZDIV")
    //                        {

    //                        }
    //                        else
    //                        {
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Gunny Bag Details: \n\n Bag No: " + model.GUNNY_BAG_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });
    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bag Seal No: " + model.GUNNY_BAG_SEAL_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });

    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Testing Date: " + model.Removed_METER_Testing_Date.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });

    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" Notice No: " + model.Notice_No.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                            {
    //                                Border = 0,
    //                                // Colspan = 3,
    //                                Rowspan = 2,
    //                                Colspan = 6,
    //                                HorizontalAlignment = Element.ALIGN_CENTER
    //                            });

    //                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                        }
    //                    }
    //                    else if (model.ORDER_TYPE == "ZDIN")
    //                    {
    //                        if (model.Removed_METER_Cable_Size == null || model.Removed_METER_Cable_Size =="")//babalu kumar
    //                            model.Removed_METER_Cable_Size = "N/A";
    //                        if (model.Removed_METER_Cable_Length == null || model.Removed_METER_Cable_Length=="")//babalu kumar
    //                            model.Removed_METER_Cable_Length = "N/A";

    //                        tableLayout.AddCell(new PdfPCell(new Phrase("Removed Cable Details: \n\n Cable Size: " + model.Removed_METER_Cable_Size.ToString() + "   Cable Length(m): " + model.Removed_METER_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                    }
    //                    //Phrase phrase = new Phrase("Consumer Signature::", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0)));
    //                    //PdfPCell pdfPCell = new PdfPCell(phrase);
    //                    //tableLayout.AddCell(pdfPCell);
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" Consumer Signature:: " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        // Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });

    //                    //tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6 });

    //                    if (signature != "")
    //                    {
    //                        iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(signature);
    //                        myImage.ScaleAbsolute(45f, 45f);
    //                        tableLayout.AddCell(new PdfPCell(myImage)
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });
    //                    }
    //                    else
    //                    {
    //                        tableLayout.AddCell(new PdfPCell(new Phrase(" N/A", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                        {
    //                            Border = 0,
    //                            // Colspan = 3,
    //                            Rowspan = 2,
    //                            Colspan = 6,
    //                            HorizontalAlignment = Element.ALIGN_CENTER
    //                        });
    //                    }

    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" BSES Representative ID/Sign: " + model.TAB_ID.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        // Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" BSES Representative Name: " + model.TAB_ID_NAME.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        // Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });

    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" This is a system generated document & does not need \n any signature. To download another copy, visit link http://125.22.84.50:7871/mams/applogin.aspx, Enter CA no. as your user ID and Meter number as a password. Team BRPL :     ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
    //                    {
    //                        Border = 0,
    //                        // Colspan = 3,
    //                        Rowspan = 2,
    //                        Colspan = 6,
    //                        HorizontalAlignment = Element.ALIGN_CENTER
    //                    });
    //                    tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

    //                    if (document.Add(tableLayout))
    //                    {
    //                        objBL.Update_MCR_ManualPDFLoose(row["LM_CUSTOMERCA"].ToString());
    //                        objBL.Update_MCRManualPDFLoose(FullPath, row["LM_CUSTOMERCA"].ToString());
    //                    }
    //                    document.Close();
    //                }
    //            }
    //        }
    //    }
    //}
    private void GeneratePDF(DataTable dt)
    {
        string FileName = string.Empty;
        string DeviceNo = string.Empty;
        Mcr_Pdf_Model model = new Mcr_Pdf_Model();

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    FileName = row["Customer_Acount"].ToString() != "N/A" ? row["Customer_Acount"].ToString() : "";
                    DeviceNo = row["DEVICENO"].ToString();

                    model.ORDER_TYPE = row["ORDER_TYPE"].ToString();
                    model.Division = row["DIVISION"].ToString();

                    if (model.ORDER_TYPE == "ZDRP" || model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZDIV" || model.ORDER_TYPE == "ZDRM")
                    {
                        model.MTR_READ_AVAIL = row["MTR_READ_AVAIL"].ToString();
                        model.Removed_METER_NO = row["Removed_Meter_No"].ToString();
                        model.OLD_MTR_MRKWH = row["OLD_MR_KWH"].ToString();
                        model.OLD_MTR_MRKVAH = row["OLD_MR_KVAH"].ToString();
                        model.Removed_METER_TERMINALSEAL1 = row["REM_TERMINAL_SEAL"].ToString();
                        model.Removed_METER_TERMINALSEAL2 = row["REM_OTHER_SEAL"].ToString();
                        model.Removed_METER_METERBOXSEAL1 = row["REM_BOX_SEAL1"].ToString();
                        model.Removed_METER_METERBOXSEAL2 = row["REM_BOX_SEAL2"].ToString(); ;
                        model.Removed_METER_BUSBARSEAL1 = row["REM_BUSBAR_SEAL1"].ToString();
                        model.Removed_METER_BUSBARSEAL2 = row["REM_BUSBAR_SEAL2"].ToString();
                        if (row["REM_CABLE_SIZE"].ToString() == null || row["REM_CABLE_SIZE"].ToString() == "")
                        {
                            model.Removed_METER_Cable_Size = "N/A";
                        }
                        else
                        {
                            model.Removed_METER_Cable_Size = row["REM_CABLE_SIZE"].ToString();
                        }
                        if (row["REM_CABLE_LEN"].ToString() == null || row["REM_CABLE_LEN"].ToString() == "")
                        {
                            model.Removed_METER_Cable_Length = "N/A";
                        }
                        else
                        {
                            model.Removed_METER_Cable_Length = row["REM_CABLE_LEN"].ToString();
                        }
                        model.GUNNY_BAG_NO = row["GUNNYBAG_OLD"].ToString();
                        model.GUNNY_BAG_SEAL_NO = row["GUNNYBAGSEAL_OLD"].ToString();
                        model.Removed_METER_Testing_Date = row["LABTESTING_DATE_OLD"].ToString() != "" ? Convert.ToDateTime(row["LABTESTING_DATE_OLD"].ToString()).ToString("dd/MM/yyyy") : "N/A";
                        model.Notice_No = row["LAB_TSTNG_NTC"].ToString();
                    }
                    model.ORDER_NO = row["ORDERID"].ToString();
                    model.PM_ACTIVITY = row["PM_ACTIVITY"].ToString();
                    model.MR_KWH = row["MR_KWH"].ToString();
                    model.MR_KVAH = row["MR_KVAH"].ToString();
                    model.ACTIVITY_DATE = row["ACTIVITY_DATE"].ToString() != "" ? Convert.ToDateTime(row["ACTIVITY_DATE"].ToString()).ToString("dd/MM/yyyy") : "N/A";
                    model.accountClass = row["ACCOUNT_CLASS"].ToString();
                    model.customerName = row["NAME"].ToString();
                    model.customerAddress = row["ADDRESS"].ToString();
                    model.customerMobile = row["TEL_NO"].ToString();
                    model.FatherName = row["FATHER_NAME"].ToString();
                    model.SANCTIONED_LOAD = row["SANCTIONED_LOAD"].ToString();
                    model.METER_NO = row["METER_NO"].ToString();
                    model.FatherName = row["FATHER_NAME"].ToString();
                    model.Bus_Bar_Size = row["BUSBARSIZE"].ToString();
                    model.BUS_BAR_NO = row["BUS_BAR_NO"].ToString();
                    model.ContractAccount = row["Customer_Acount"].ToString();
                    model.CableLength = row["CABLELENGTH"].ToString();
                    model.TERMINALSEAL1 = row["TERMINAL_SEAL"].ToString();
                    model.TERMINALSEAL2 = row["OTHER_SEAL"].ToString();
                    model.METERBOXSEAL1 = row["METERBOXSEAL1"].ToString();
                    model.METERBOXSEAL2 = row["METERBOXSEAL2"].ToString();
                    model.BUSBARSEAL1 = row["BUSBARSEAL1"].ToString();
                    model.BUSBARSEAL2 = row["BUSBARSEAL2"].ToString();
                    model.BUS_BAR_CBL_SIZE = row["B_BAR_CABLE_SIZE"].ToString();
                    model.BUS_Bar_Cable_Length = row["BUS_BAR_CABLE_LENG"].ToString();
                    model.BUS_Bar_Output_Cable_Length = row["OUTPUTCABLELENGTH"].ToString();
                    model.CableSize = row["CABLESIZE2"].ToString();
                    model.ImagePath = row["IMAGE_SIGNATURE"].ToString();
                    model.TAB_ID = row["TAB_LOGIN_ID"].ToString();
                    model.TAB_ID_NAME = row["TAB_LN_ID_NAME"].ToString();

                    if (model != null)
                    {
                        Document document = new Document(PageSize.A4);
                        string FullPath = string.Empty;
                        string signature = string.Empty;
                        //  string path = @"E:\PDF\";
                        //string logo = Directory.GetCurrentDirectory() + @"\BSES.jpg";
                        string logo = Server.MapPath("~/Report/BSES.jpg");

                        if ((model.ImagePath != "X") && (model.ImagePath.Trim() != ""))
                            signature = model.ImagePath;

                        FullPath = byteArrayToPDF(FileName);

                        BaseFont fntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        iTextSharp.text.Font fnt = new iTextSharp.text.Font(fntHead, 16, 1, BaseColor.RED);

                        PdfPTable tableLayout = new PdfPTable(4);
                        iTextSharp.text.Font font = new iTextSharp.text.Font(fntHead, 10, iTextSharp.text.Font.NORMAL);
                        PdfWriter.GetInstance(document, new FileStream(FullPath, FileMode.Create, FileAccess.ReadWrite));
                        document.Open();

                        iTextSharp.text.Image BsesLogo = iTextSharp.text.Image.GetInstance(logo);

                        iTextSharp.text.Font arial = FontFactory.GetFont("Arial", 28, BaseColor.GRAY);
                        tableLayout.AddCell(new PdfPCell(BsesLogo)
                        {
                            Colspan = 4,
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase("Rajdhani Power Limited", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Colspan = 4,
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" METER PARTICULAR SHEET ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Colspan = 4,
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });

                        Paragraph lineseprator = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                        tableLayout.AddCell(new PdfPCell(new Phrase(" --------------------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        tableLayout.AddCell(new PdfPCell(new Phrase(" Order No: " + model.ORDER_NO.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 4,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Contract Account: " + model.ContractAccount + "", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 4,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Activity Type: " + model.PM_ACTIVITY.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            //Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        //if (model.looseFlag != "LOOSE")
                        // {
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Division: " + model.Division.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        //}
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Date of Activity: " + model.ACTIVITY_DATE.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Account Class: " + model.accountClass.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            //Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });


                        if (model.looseFlag != "LOOSE")
                        {
                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Sanctioned Load: " + model.SANCTIONED_LOAD.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Consumer Name : " + model.customerName.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        tableLayout.AddCell(new PdfPCell(new Phrase(" Address : " + model.customerAddress.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,

                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });



                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        if (model.ORDER_TYPE != "ZDRM")
                        {
                            tableLayout.AddCell(new PdfPCell(new Phrase("        New Meter Details \n Meter No." + model.METER_NO.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: KWH:" + model.MR_KWH + " | KVAH: " + model.MR_KVAH + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Size : " + model.Bus_Bar_Size.ToString() + "" + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Number : " + model.BUS_BAR_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Terminal Seal-1 : " + model.TERMINALSEAL1.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Terminal Seal-2 : " + model.TERMINALSEAL2.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Meter Box Seal-1: " + model.METERBOXSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Meter Box Seal-2: " + model.METERBOXSEAL2 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Seal-1: " + model.BUSBARSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Seal-2: " + model.BUSBARSEAL2 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Cable Details:\n\n Cable Size: " + model.CableSize.ToString() + "  Cable Length(m): " + model.CableLength.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Output Cable Length: " + model.BUS_Bar_Output_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Meter-Busbar Cable \n\n Cable Size: " + model.BUS_BAR_CBL_SIZE.ToString() + "  Cable Length: " + model.BUS_Bar_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                        }
                        if (model.ORDER_TYPE == "ZDRP" || model.ORDER_TYPE == "ZDRM" || model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZDIV" || model.ORDER_TYPE == "ZMSC")
                        {
                            tableLayout.AddCell(new PdfPCell(new Phrase("        Removed Meter Details (If applicable) \n Meter No." + model.Removed_METER_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            if (model.MTR_READ_AVAIL.ToLower() == "yes")
                            {
                                tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                                tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: KWH:" + model.OLD_MTR_MRKWH.ToString() + " | KVAH: " + model.OLD_MTR_MRKVAH.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });
                            }
                            if (model.MTR_READ_AVAIL.ToLower() == "no")
                            {
                                tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: N/V  " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });
                            }

                            //Added By Babalu Kumar on 11072021

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Removed Seal 1 : " + model.TERMINALSEAL1.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Removed Seal 2 : " + model.TERMINALSEAL2.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Removed Seal 3: " + model.METERBOXSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Pole-Meter Busbar Cable: \n\n Cable Size: " + model.Removed_METER_Cable_Size.ToString() + "   Cable Length(m): " + model.Removed_METER_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            if (model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZMSC" || model.ORDER_TYPE == "ZDIV")
                            {

                            }
                            else
                            {
                                tableLayout.AddCell(new PdfPCell(new Phrase(" Gunny Bag Details: \n\n Bag No: " + model.GUNNY_BAG_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });
                                tableLayout.AddCell(new PdfPCell(new Phrase(" Bag Seal No: " + model.GUNNY_BAG_SEAL_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(" Testing Date: " + model.Removed_METER_Testing_Date.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(" Notice No: " + model.Notice_No.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            }
                        }
                        else if (model.ORDER_TYPE == "ZDIN")
                        {
                            //if (model.Removed_METER_Cable_Size.ToString() == null || model.Removed_METER_Cable_Size.ToString() == "")
                            //{
                            //    model.Removed_METER_Cable_Size = "N/A";
                            //}
                            //if (model.Removed_METER_Cable_Length.ToString() == null || model.Removed_METER_Cable_Length.ToString() == "")
                            //{
                            //    model.Removed_METER_Cable_Length = "N/A";
                            //}
                            //tableLayout.AddCell(new PdfPCell(new Phrase("Removed Cable Details: \n\n Cable Size: " + model.Removed_METER_Cable_Size.ToString() + "   Cable Length(m): " + model.Removed_METER_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            //{
                            //    Border = 0,
                            //    Colspan = 3,//Babalu Kumar
                            //    Rowspan = 2,
                            //    Colspan = 6,
                            //    HorizontalAlignment = Element.ALIGN_CENTER
                            //});
                            //tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });


                            tableLayout.AddCell(new PdfPCell(new Phrase("Removed Cable Details: \n\n Cable Size: N/A" + "   Cable Length(m): N/A " + new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                Colspan = 3,//Babalu Kumar
                                Rowspan = 2,
                                //Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Removed Seal Details:: \n\n Removed Seal1: " + model.TERMINALSEAL1.ToString() + "   Removed Seal2: " + model.TERMINALSEAL2.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                //Colspan = 3,//Babalu Kumar
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        }
                        //Phrase phrase = new Phrase("Consumer Signature::", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0)));
                        //PdfPCell pdfPCell = new PdfPCell(phrase);
                        //tableLayout.AddCell(pdfPCell);
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Consumer Signature:: " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            // Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });

                        //tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6 });

                        if (signature != "")
                        {
                            iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(signature);
                            myImage.ScaleAbsolute(45f, 45f);
                            tableLayout.AddCell(new PdfPCell(myImage)
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }
                        else
                        {
                            tableLayout.AddCell(new PdfPCell(new Phrase(" N/A", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }

                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" BSES Representative ID/Sign: " + model.TAB_ID.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            // Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" BSES Representative Name: " + model.TAB_ID_NAME.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            // Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" This is a system generated document & does not need \n any signature. To download another copy, visit link http://125.22.84.50:7871/mams/applogin.aspx, Enter CA no. as your user ID and Meter number as a password. Team BRPL :     ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            // Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        if (document.Add(tableLayout))
                        {
                            objBL.Update_MCR_ManualPDF(DeviceNo);
                            objBL.Update_MCRManualPDF(FullPath, DeviceNo);
                        }
                        document.Close();
                    }
                }
            }
        }
    }

    private void GeneratePDFLoose(DataTable dt)
    {
        string FileName = string.Empty;
        string DeviceNo = string.Empty;
        Mcr_Pdf_Model model = new Mcr_Pdf_Model();

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    FileName = row["Customer_Acount"].ToString() != "N/A" ? row["Customer_Acount"].ToString() : "";
                    DeviceNo = row["DEVICENO"].ToString();

                    model.ORDER_TYPE = row["ORDER_TYPE"].ToString();
                    model.Division = row["DIVISION"].ToString();

                    if (model.ORDER_TYPE == "ZDRP" || model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZDIV" || model.ORDER_TYPE == "ZDRM")
                    {
                        model.MTR_READ_AVAIL = row["MTR_READ_AVAIL"].ToString();
                        model.Removed_METER_NO = row["Removed_Meter_No"].ToString();
                        model.OLD_MTR_MRKWH = row["OLD_MR_KWH"].ToString();
                        model.OLD_MTR_MRKVAH = row["OLD_MR_KVAH"].ToString();
                        model.Removed_METER_TERMINALSEAL1 = row["REM_TERMINAL_SEAL"].ToString();
                        model.Removed_METER_TERMINALSEAL2 = row["REM_OTHER_SEAL"].ToString();
                        model.Removed_METER_METERBOXSEAL1 = row["REM_BOX_SEAL1"].ToString();
                        model.Removed_METER_METERBOXSEAL2 = row["REM_BOX_SEAL2"].ToString(); ;
                        model.Removed_METER_BUSBARSEAL1 = row["REM_BUSBAR_SEAL1"].ToString();
                        model.Removed_METER_BUSBARSEAL2 = row["REM_BUSBAR_SEAL2"].ToString();
                        if (row["REM_CABLE_SIZE"].ToString() == null || row["REM_CABLE_SIZE"].ToString() == "")
                        {
                            model.Removed_METER_Cable_Size = "N/A";
                        }
                        else
                        {
                            model.Removed_METER_Cable_Size = row["REM_CABLE_SIZE"].ToString();
                        }
                        if (row["REM_CABLE_LEN"].ToString() == null || row["REM_CABLE_LEN"].ToString() == "")
                        {
                            model.Removed_METER_Cable_Length = "N/A";
                        }
                        else
                        {
                            model.Removed_METER_Cable_Length = row["REM_CABLE_LEN"].ToString();
                        }
                        model.GUNNY_BAG_NO = row["GUNNYBAG_OLD"].ToString();
                        model.GUNNY_BAG_SEAL_NO = row["GUNNYBAGSEAL_OLD"].ToString();
                        model.Removed_METER_Testing_Date = row["LABTESTING_DATE_OLD"].ToString() != "" ? Convert.ToDateTime(row["LABTESTING_DATE_OLD"].ToString()).ToString("dd/MM/yyyy") : "N/A";
                        model.Notice_No = row["LAB_TSTNG_NTC"].ToString();
                    }
                    model.ORDER_NO = row["ORDERID"].ToString();
                    model.PM_ACTIVITY = row["PM_ACTIVITY"].ToString();
                    model.MR_KWH = row["MR_KWH"].ToString();
                    model.MR_KVAH = row["MR_KVAH"].ToString();
                    model.ACTIVITY_DATE = row["ACTIVITY_DATE"].ToString() != "" ? Convert.ToDateTime(row["ACTIVITY_DATE"].ToString()).ToString("dd/MM/yyyy") : "N/A";
                    model.accountClass = row["ACCOUNT_CLASS"].ToString();
                    model.customerName = row["NAME"].ToString();
                    model.customerAddress = row["ADDRESS"].ToString();
                    model.customerMobile = row["TEL_NO"].ToString();
                    model.FatherName = row["FATHER_NAME"].ToString();
                    model.SANCTIONED_LOAD = row["SANCTIONED_LOAD"].ToString();
                    model.METER_NO = row["METER_NO"].ToString();
                    model.FatherName = row["FATHER_NAME"].ToString();
                    model.Bus_Bar_Size = row["BUSBARSIZE"].ToString();
                    model.BUS_BAR_NO = row["BUS_BAR_NO"].ToString();
                    model.ContractAccount = row["Customer_Acount"].ToString();
                    model.CableLength = row["CABLELENGTH"].ToString();
                    model.TERMINALSEAL1 = row["TERMINAL_SEAL"].ToString();
                    model.TERMINALSEAL2 = row["OTHER_SEAL"].ToString();
                    model.METERBOXSEAL1 = row["METERBOXSEAL1"].ToString();
                    model.METERBOXSEAL2 = row["METERBOXSEAL2"].ToString();
                    model.BUSBARSEAL1 = row["BUSBARSEAL1"].ToString();
                    model.BUSBARSEAL2 = row["BUSBARSEAL2"].ToString();
                    model.BUS_BAR_CBL_SIZE = row["B_BAR_CABLE_SIZE"].ToString();
                    model.BUS_Bar_Cable_Length = row["BUS_BAR_CABLE_LENG"].ToString();
                    model.BUS_Bar_Output_Cable_Length = row["OUTPUTCABLELENGTH"].ToString();
                    model.CableSize = row["CABLESIZE2"].ToString();
                    model.ImagePath = row["IMAGE_SIGNATURE"].ToString();
                    model.TAB_ID = row["TAB_LOGIN_ID"].ToString();
                    model.TAB_ID_NAME = row["TAB_LN_ID_NAME"].ToString();

                    if (model != null)
                    {
                        Document document = new Document(PageSize.A4);
                        string FullPath = string.Empty;
                        string signature = string.Empty;
                        //  string path = @"E:\PDF\";
                        //string logo = Directory.GetCurrentDirectory() + @"\BSES.jpg";
                        string logo = Server.MapPath("~/Report/BSES.jpg");

                        if ((model.ImagePath != "X") && (model.ImagePath.Trim() != ""))
                            signature = model.ImagePath;

                        FullPath = byteArrayToPDF(FileName);

                        BaseFont fntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        iTextSharp.text.Font fnt = new iTextSharp.text.Font(fntHead, 16, 1, BaseColor.RED);

                        PdfPTable tableLayout = new PdfPTable(4);
                        iTextSharp.text.Font font = new iTextSharp.text.Font(fntHead, 10, iTextSharp.text.Font.NORMAL);
                        PdfWriter.GetInstance(document, new FileStream(FullPath, FileMode.Create, FileAccess.ReadWrite));
                        document.Open();

                        iTextSharp.text.Image BsesLogo = iTextSharp.text.Image.GetInstance(logo);

                        iTextSharp.text.Font arial = FontFactory.GetFont("Arial", 28, BaseColor.GRAY);
                        tableLayout.AddCell(new PdfPCell(BsesLogo)
                        {
                            Colspan = 4,
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase("Rajdhani Power Limited", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Colspan = 4,
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" METER PARTICULAR SHEET ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Colspan = 4,
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });

                        Paragraph lineseprator = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                        tableLayout.AddCell(new PdfPCell(new Phrase(" --------------------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        tableLayout.AddCell(new PdfPCell(new Phrase(" Order No: " + model.ORDER_NO.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 4,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Contract Account: " + model.ContractAccount + "", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 4,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Activity Type: " + model.PM_ACTIVITY.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            //Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        //if (model.looseFlag != "LOOSE")
                        // {
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Division: " + model.Division.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        //}
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Date of Activity: " + model.ACTIVITY_DATE.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Account Class: " + model.accountClass.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            //Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });


                        if (model.looseFlag != "LOOSE")
                        {
                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Sanctioned Load: " + model.SANCTIONED_LOAD.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Consumer Name : " + model.customerName.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        tableLayout.AddCell(new PdfPCell(new Phrase(" Address : " + model.customerAddress.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,

                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });



                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        if (model.ORDER_TYPE != "ZDRM")
                        {
                            tableLayout.AddCell(new PdfPCell(new Phrase("        New Meter Details \n Meter No." + model.METER_NO.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: KWH:" + model.MR_KWH + " | KVAH: " + model.MR_KVAH + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Size : " + model.Bus_Bar_Size.ToString() + "" + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Number : " + model.BUS_BAR_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Terminal Seal-1 : " + model.TERMINALSEAL1.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Terminal Seal-2 : " + model.TERMINALSEAL2.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Meter Box Seal-1: " + model.METERBOXSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Meter Box Seal-2: " + model.METERBOXSEAL2 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Seal-1: " + model.BUSBARSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Bus-Bar Seal-2: " + model.BUSBARSEAL2 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Pole-Meter/Busbar Cable:\n\n Cable Size: " + model.CableSize.ToString() + "  Cable Length(m): " + model.CableLength.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Output Cable Length: " + model.BUS_Bar_Output_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Meter-Busbar Cable \n\n Cable Size: " + model.BUS_BAR_CBL_SIZE.ToString() + "  Cable Length: " + model.BUS_Bar_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                        }
                        if (model.ORDER_TYPE == "ZDRP" || model.ORDER_TYPE == "ZDRM" || model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZDIV" || model.ORDER_TYPE == "ZMSC")
                        {
                            tableLayout.AddCell(new PdfPCell(new Phrase("        Removed Meter Details (If applicable) \n Meter No." + model.Removed_METER_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            if (model.MTR_READ_AVAIL.ToLower() == "yes")
                            {
                                tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                                tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: KWH:" + model.OLD_MTR_MRKWH.ToString() + " | KVAH: " + model.OLD_MTR_MRKVAH.ToString() + " " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });
                            }
                            if (model.MTR_READ_AVAIL.ToLower() == "no")
                            {
                                tableLayout.AddCell(new PdfPCell(new Phrase(" Reading: N/V  " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });
                            }

                            //Added By Babalu Kumar on 11072021

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Removed Seal 1 : " + model.TERMINALSEAL1.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Removed Seal 2 : " + model.TERMINALSEAL2.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Removed Seal 3: " + model.METERBOXSEAL1 + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" Pole-Meter/Busbar Cable: \n\n Cable Size: " + model.Removed_METER_Cable_Size.ToString() + "   Cable Length(m): " + model.Removed_METER_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            if (model.ORDER_TYPE == "ZMSO" || model.ORDER_TYPE == "ZMSC" || model.ORDER_TYPE == "ZDIV")
                            {

                            }
                            else
                            {
                                tableLayout.AddCell(new PdfPCell(new Phrase(" Gunny Bag Details: \n\n Bag No: " + model.GUNNY_BAG_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });
                                tableLayout.AddCell(new PdfPCell(new Phrase(" Bag Seal No: " + model.GUNNY_BAG_SEAL_NO.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(" Testing Date: " + model.Removed_METER_Testing_Date.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(" Notice No: " + model.Notice_No.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    Border = 0,
                                    // Colspan = 3,
                                    Rowspan = 2,
                                    Colspan = 6,
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                            }
                        }
                        else if (model.ORDER_TYPE == "ZDIN")
                        {
                            if (model.Removed_METER_Cable_Size == null || model.Removed_METER_Cable_Size == "")//babalu kumar
                                model.Removed_METER_Cable_Size = "N/A";
                            if (model.Removed_METER_Cable_Length == null || model.Removed_METER_Cable_Length == "")//babalu kumar
                                model.Removed_METER_Cable_Length = "N/A";

                            tableLayout.AddCell(new PdfPCell(new Phrase("Removed Cable Details: \n\n Cable Size: " + model.Removed_METER_Cable_Size.ToString() + "   Cable Length(m): " + model.Removed_METER_Cable_Length.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                            tableLayout.AddCell(new PdfPCell(new Phrase(" Removed Seal Details:: \n\n Removed Seal1: " + model.TERMINALSEAL1.ToString() + "   Removed Seal2: " + model.TERMINALSEAL2.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                //Colspan = 3,//Babalu Kumar
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                            tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                        }
                        //Phrase phrase = new Phrase("Consumer Signature::", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0)));
                        //PdfPCell pdfPCell = new PdfPCell(phrase);
                        //tableLayout.AddCell(pdfPCell);
                        tableLayout.AddCell(new PdfPCell(new Phrase(" Consumer Signature:: " + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            // Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });

                        //tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6 });

                        if (signature != "")
                        {
                            iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(signature);
                            myImage.ScaleAbsolute(45f, 45f);
                            tableLayout.AddCell(new PdfPCell(myImage)
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }
                        else
                        {
                            tableLayout.AddCell(new PdfPCell(new Phrase(" N/A", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Border = 0,
                                // Colspan = 3,
                                Rowspan = 2,
                                Colspan = 6,
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }

                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" BSES Representative ID/Sign: " + model.TAB_ID.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            // Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" BSES Representative Name: " + model.TAB_ID_NAME.ToString() + "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            // Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" This is a system generated document & does not need \n any signature. To download another copy, visit link http://125.22.84.50:7871/mams/applogin.aspx, Enter CA no. as your user ID and Meter number as a password. Team BRPL :     ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Border = 0,
                            // Colspan = 3,
                            Rowspan = 2,
                            Colspan = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tableLayout.AddCell(new PdfPCell(new Phrase(" ----------------------------- ", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 7, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Border = 0, Colspan = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                        if (document.Add(tableLayout))
                        {
                            objBL.Update_MCR_ManualPDFLoose(row["LM_CUSTOMERCA"].ToString());
                            objBL.Update_MCRManualPDFLoose(FullPath, row["LM_CUSTOMERCA"].ToString());
                        }
                        document.Close();
                    }
                }
            }
        }
    }
    public static string byteArrayToPDF(string filename)
    {
        Utilities.Network.NetworkDrive nd = new Utilities.Network.NetworkDrive();
        string pth = string.Empty, sl = string.Empty;

        try
        {
            pth = @"\\10.125.64.236\UploadedImages";
            nd.MapNetworkDrive(@"\\10.125.64.236\UploadedImages", "Z:", "uploadimages", "Bses@123");
            sl = pth;

            string _sDir = sl + "\\MCR\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();

            DirectoryInfo _DirInfo = new DirectoryInfo(_sDir);
            if (_DirInfo.Exists == false)
                _DirInfo.Create();

            string _sPath = _DirInfo.FullName + "\\" + filename + ".pdf";
            return _sPath;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    class Mcr_Pdf_Model
    {
        public string Division { get; set; }
        public string ORDER_NO { get; set; }
        public string ORDER_TYPE { get; set; }
        public string PM_ACTIVITY { get; set; }
        public string DEVICE_NO { get; set; }
        public string TERMINALSEAL1 { get; set; }
        public string TERMINALSEAL2 { get; set; }
        public string METERBOXSEAL1 { get; set; }
        public string METERBOXSEAL2 { get; set; }
        public string BUSBARSEAL1 { get; set; }
        public string BUSBARSEAL2 { get; set; }

        public string Category { get; set; }
        public string ContractAccount { get; set; }
        public string ConsumerName { get; set; }
        public string CableSize { get; set; }
        public string CableLength { get; set; }
        public string BUS_Bar_Cable_Size { get; set; }
        public string BUS_Bar_Cable_Length { get; set; }
        public string BUS_Bar_Output_Cable_Length { get; set; }
        public string METER_NO { get; set; }
        public string Removed_METER_NO { get; set; }
        public string Removed_METER_Reading { get; set; }
        public string Removed_METER_TERMINALSEAL1 { get; set; }
        public string Removed_METER_TERMINALSEAL2 { get; set; }
        public string Removed_METER_METERBOXSEAL1 { get; set; }
        public string Removed_METER_METERBOXSEAL2 { get; set; }
        public string Removed_METER_BUSBARSEAL1 { get; set; }
        public string Removed_METER_BUSBARSEAL2 { get; set; }
        public string Removed_METER_Cable_Size { get; set; }
        public string Removed_METER_Cable_Length { get; set; }
        public string Removed_METER_BagNo { get; set; }
        public string Removed_METER_Bag_Seal_No { get; set; }
        public string Removed_METER_Testing_Date { get; set; }
        public string Notice_No { get; set; }
        public string GUNNY_BAG_NO { get; set; }
        public string GUNNY_BAG_SEAL_NO { get; set; }
        public string OLD_MTR_MRKWH { get; set; }
        public string OLD_MTR_MRKVAH { get; set; }

        public string Address { get; set; }
        public string Reading { get; set; }
        public string Bus_Bar_Size { get; set; }
        public string Bus_Bar_Number { get; set; }
        public string MR_KWH { get; set; }
        public string MR_KVAH { get; set; }
        public string BUS_BAR_SIZE { get; set; }
        public string BUS_BAR_NO { get; set; }
        public string BUS_BAR_CBL_SIZE { get; set; }
        public string CABLELENGTH { get; set; }
        public string OUTPUTCABLELENGTH { get; set; }
        public string ACTIVITY_DATE { get; set; }
        public string OLD_MTR_MRKW { get; set; }
        public string OLD_MTR_MRKVA { get; set; }
        public string TAB_ID { get; set; }
        public string TAB_ID_NAME { get; set; }

        public string OUTPUT_CABLE_LEN_USED { get; set; }
        public string customerName { get; set; }
        public string customerAddress { get; set; }
        public string customerMobile { get; set; }
        public string accountClass { get; set; }
        public string looseFlag { get; set; }
        public string mcrPDF { get; set; }
        public string FatherName { get; set; }
        public string SANCTIONED_LOAD { get; set; }


        public string CableSize2 { get; set; }
        public string ImagePath { get; set; }

        public string MTR_READ_AVAIL { get; set; }
        public string LABTESTING_DATE_OLD { get; set; }
        public string LAB_TSTNG_NTC { get; set; }
    }


    protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrderType.SelectedItem.Text != "-ALL-")
        {
            DataTable _dtBindName = objBL.getPM_Activity_OrderWise(ddlOrderType.SelectedValue, Convert.ToString(Session["VENDOR_ID"]), Convert.ToString(Session["Divison"]), Convert.ToString(Session["ROLE"]));    //16032018
            if (_dtBindName.Rows.Count > 0)
            {
                ddlPMActivity.DataSource = _dtBindName;
                ddlPMActivity.DataTextField = "PM_DESCRIPTION";
                ddlPMActivity.DataValueField = "PM_ACTIVTY";
                ddlPMActivity.DataBind();
                ddlPMActivity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
            }
        }
        else
        {
            ddlPMActivity.Items.Clear();
            ddlPMActivity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
        }
    }

    public void BindDivisioin()
    {
        DataTable _dtBindName = objBL.getDivisionDetails(Convert.ToString(Session["Divison"]));
        if (_dtBindName.Rows.Count > 0)
        {
            txtDivision.DataSource = _dtBindName;
            txtDivision.DataTextField = "DIVISION_NAME";
            txtDivision.DataValueField = "DIST_CD";
            txtDivision.DataBind();
            txtDivision.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-ALL-", "0"));
        }

        if (_dtBindName.Rows.Count == 1)
            txtDivision.SelectedIndex = 1;
    }

    protected void sellectOne(object sender, EventArgs e)
    {
        int Count = 0;
        LinkButton lkbtnHeader = (LinkButton)gvMainData.HeaderRow.FindControl("lkSelectAll");
        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
            if (ChkBoxRows.Checked == true)
            {
                Count = Count + 1;
                lkbtnHeader.Text = "DeSelect All";
            }
            else
            {
                lkbtnHeader.Text = "Select All";
            }
            if (Count == 0)
            {

            }
            if (Count > 10)
            {
                ChkBoxRows.Checked = false;
                SimpleMethods.show("You can not select more than 10 records");
                return;
            }
        }
    }

    protected void lkSelectAll_Click(object sender, EventArgs e)
    {
        int Count = 0;
        LinkButton lkbtnHeader = (LinkButton)gvMainData.HeaderRow.FindControl("lkSelectAll");
        foreach (GridViewRow row in gvMainData.Rows)
        {
            CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
            if (lkbtnHeader.Text == "Select All")
            {
                Count = Count + 1;
                ChkBoxRows.Checked = true;
            }
            else
            {
                Count = Count - 1;
                ChkBoxRows.Checked = false;
            }

            if (Count > 10)
            {
                ChkBoxRows.Checked = false;
                SimpleMethods.show("You can not select more than 10 records");
                // return;
            }
        }
        if (lkbtnHeader.Text == "Select All")
            lkbtnHeader.Text = "DeSelect All";
        else if (lkbtnHeader.Text == "DeSelect All")
            lkbtnHeader.Text = "Select All";
    }
    protected void btnpdf_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            foreach (GridViewRow row in gvMainData.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox objcheck = (CheckBox)row.FindControl("chkRow") as CheckBox;
                    if (objcheck.Checked == true)
                    {
                        Label strOrder = (Label)row.FindControl("lblOrder");
                        Label strca = (Label)row.FindControl("lblCA");
                        Label strType = (Label)row.FindControl("lblType");

                        DataTable _dtPDF = new DataTable();
                        if (strType.Text.ToString() == "Order")
                        {
                            _dtPDF = objBL.GenerateOrder_MCRPDF(txtFromDate.Text, txtToDate.Text, strca.Text.ToString(), strOrder.Text.ToString());
                        }
                        else
                        {
                            _dtPDF = objBL.GenerateLooseOrder_MCRPDF(txtFromDate.Text, txtToDate.Text, strca.Text.ToString(), strOrder.Text.ToString());
                        }

                        if (_dtPDF.Rows[0][0].ToString() == "-1")
                            return;
                        else
                        {
                            if (strType.Text.ToString() == "Order")
                                GeneratePDF(_dtPDF);
                            else
                                GeneratePDFLoose(_dtPDF);

                           // Response.Write("<script>alert('MCR PDF Has Been Successfully Created !!')</script>");

                            Show_NotGenearte_PDF_Data();
                        }
                      //  Response.Write("<script>alert('MCR PDF Has Been Successfully Created !!')</script>");
                    }
                }
            }
            Response.Write("<script>alert('MCR PDF Has Been Successfully Created !!')</script>");
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.Message.ToString() + "')</script>");
        }
        finally
        {

        }
    }
    protected void txtDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindVendor("", txtDivision.SelectedValue, Convert.ToString(Session["ROLE"]));//Added By Babalu Kumar 30122020 Req. No. REQ04122020121216 And PCN No. 2021010104 for Add Multiple Vendor 
    }
}