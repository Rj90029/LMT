using LMT.Models.Model;
using LMT.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMT.Controllers
{
    public class ReportController : SecureController
    {
        // GET: Report
        public ActionResult GetReports()
        {
            DetailedReport model = new DetailedReport();
            ReportServices rs = new ReportServices();
            model.UserList = rs.GetNames();
            model.UserList.Insert(0, "ALL");
            model.TypeList = rs.GetTypeList();
            model.UserName = "ALL";
            model.StatusSelected = "ALL";
            var list = new List<SelectListItem>{
                new SelectListItem {Text = "ALL", Value = "ALL" },
                new SelectListItem {Text = "PENDING", Value = "PENDING" },
                new SelectListItem {Text = "REJECTED", Value = "REJECTED" },
                new SelectListItem {Text = "APPROVED", Value = "APPROVED" }
            };
            model.StatusSelected = list[0].ToString();
            ViewBag.Status = list;
            ViewBag.UserList = new SelectList(model.UserList);
            ViewBag.TypeList = new SelectList(model.TypeList);
            return View();
        }
        public FileResult EmployeeMaster()
        {
            ReportServices rs = new ReportServices();
            List<EmployeeMaster> emp = rs.MasterReport();
            string filepath = Server.MapPath("~/Report/ExportExcelFile.xlsx");
            FileInfo files = new FileInfo(filepath);
            // EPPlus dll is used for ExcelPackage
            ExcelPackage excel = new ExcelPackage(files);
            var sheet = excel.Workbook.Worksheets.FirstOrDefault(x => x.Name == "EmployeeMaster");
            if (sheet == null)
            {
                sheet = excel.Workbook.Worksheets.Add("EmployeeMaster");
            }
            sheet.Cells.Clear();
            using (ExcelRange rnge = sheet.Cells[1, 1, 4, 5])
            {
                rnge.Value = "Employee Master Records";
                rnge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rnge.Merge = true;
                rnge.Style.Font.Size = 16;
                rnge.Style.Font.Bold = true;
                rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                rnge.Style.Border.BorderAround(ExcelBorderStyle.Thick, Color.Green);
                rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rnge.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            }
            using (ExcelRange rnge = sheet.Cells[5, 1, 5, 5])
            {
                rnge.Style.Font.Bold = true;
                rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                rnge.Style.Border.BorderAround(ExcelBorderStyle.Thick);
                rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rnge.Style.Fill.BackgroundColor.SetColor(Color.Green);
            }
            sheet.Cells[5, 1].Value = "Name";
            sheet.Cells[5, 2].Value = "Employee ID";
            sheet.Cells[5, 3].Value = "ATTUID";
            sheet.Cells[5, 4].Value = "Email ID";
            sheet.Cells[5, 5].Value = "Contact No.";
            for (int i = 0; i < emp.Count; i++)
            {
                sheet.Cells[i + 6, 1].Value = emp[i].Name;
                sheet.Cells[i + 6, 2].Value = emp[i].EmpID;
                sheet.Cells[i + 6, 3].Value = emp[i].ATTUID;
                sheet.Cells[i + 6, 4].Value = emp[i].Email;
                sheet.Cells[i + 6, 5].Value = emp[i].Phone;
            }
            excel.Save();
            return File(filepath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "ExportExcelFile.xlsx");
        }

        [HttpPost]
        public ActionResult GetDetailedReport(DetailedReport model)
        {
            ReportServices rs = new ReportServices();
            List<DetailedReport> dr = rs.GetDetailedReport(model);
            string filepath = Server.MapPath("~/Report/DetailedReport.xlsx");
            FileInfo files = new FileInfo(filepath);
            // EPPlus dll is used for ExcelPackage
            ExcelPackage excel = new ExcelPackage(files);
            var sheet = excel.Workbook.Worksheets.FirstOrDefault(x => x.Name == "DetailedReport");
            if (sheet == null)
            {
                sheet = excel.Workbook.Worksheets.Add("DetailedReport");
            }
            sheet.Cells.Clear();
            using (ExcelRange rnge = sheet.Cells[1, 1, 3, 4])
            {
                rnge.Value = "Request Type :" + model.RequestType;
                rnge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                rnge.Merge = true;
                rnge.Style.Font.Size = 16;
                rnge.Style.Font.Bold = true;
                rnge.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                rnge.Style.Border.Top.Color.SetColor(Color.Green);

                rnge.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                rnge.Style.Border.Left.Color.SetColor(Color.Green);

                rnge.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                rnge.Style.Border.Right.Color.SetColor(Color.Green);

                rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rnge.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            }
            using (ExcelRange rnge = sheet.Cells[4, 1, 4, 4])
            {
                rnge.Value = "From Date :" + model.FromDate + "  To Date:" + model.ToDate;

                rnge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rnge.Merge = true;
                rnge.Style.Font.Size = 10;
                rnge.Style.Font.Bold = false;
                rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                rnge.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                rnge.Style.Border.Left.Color.SetColor(Color.Green);

                rnge.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                rnge.Style.Border.Right.Color.SetColor(Color.Green);

                rnge.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                rnge.Style.Border.Bottom.Color.SetColor(Color.Green);

                rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rnge.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            }
            using (ExcelRange rnge = sheet.Cells[5, 1, 5, 4])
            {
                rnge.Style.Font.Bold = true;
                rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                rnge.Style.Border.BorderAround(ExcelBorderStyle.Thick);
                rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rnge.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
            }
            sheet.Cells[5, 1].Value = "Employee ID";
            sheet.Cells[5, 2].Value = "Employee Name";
            sheet.Cells[5, 3].Value = "Request Date";
            sheet.Cells[5, 4].Value = "Request Status";
            if (dr.Count == 0)
            {
                sheet.Cells[6, 2].Value = "No records found!";
                excel.Save();
                return File(filepath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "DetailedReport.xlsx");
            }
           
            if (model.UserName != "ALL" && model.StatusSelected != "ALL")
            {
                using (ExcelRange rnge = sheet.Cells[6, 1, 6 + dr.Count - 1, 1])
                {
                    rnge.Value = dr[0].EmpID;

                    rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rnge.Merge = true;
                    rnge.Style.Font.Size = 10;
                    rnge.Style.Font.Bold = false;
                    rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    rnge.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rnge.Style.Border.Right.Color.SetColor(Color.Black);

                    rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rnge.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                using (ExcelRange rnge = sheet.Cells[6, 2, 6 + dr.Count - 1, 2])
                {
                    rnge.Value = model.UserName;

                    rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rnge.Merge = true;
                    rnge.Style.Font.Size = 10;
                    rnge.Style.Font.Bold = false;
                    rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    rnge.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rnge.Style.Border.Right.Color.SetColor(Color.Black);

                    rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rnge.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                using (ExcelRange rnge = sheet.Cells[6, 4, 6 + dr.Count - 1, 4])
                {
                    rnge.Value = model.StatusSelected;

                    rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rnge.Merge = true;
                    rnge.Style.Font.Size = 10;
                    rnge.Style.Font.Bold = false;
                    rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    rnge.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rnge.Style.Border.Right.Color.SetColor(Color.Black);

                    rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rnge.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                for (int i = 0; i < dr.Count; i++)
                {
                    sheet.Cells[i + 6, 3].Value = dr[i].RequestDate;
                }
            }
            else if (model.UserName == "ALL" && model.StatusSelected != "ALL")
            {
                using (ExcelRange rnge = sheet.Cells[6, 4, 6 + dr.Count - 1, 4])
                {
                    rnge.Value = model.StatusSelected;

                    rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rnge.Merge = true;
                    rnge.Style.Font.Size = 10;
                    rnge.Style.Font.Bold = false;
                    rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    rnge.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rnge.Style.Border.Right.Color.SetColor(Color.Black);

                    rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rnge.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                for (int i = 0; i < dr.Count; i++)
                {
                    sheet.Cells[i + 6, 1].Value = dr[i].EmpID;
                    sheet.Cells[i + 6, 2].Value = dr[i].UserName;
                    sheet.Cells[i + 6, 3].Value = dr[i].RequestDate;
                }
            }
            else if (model.UserName != "ALL" && model.StatusSelected == "ALL")
            {
                using (ExcelRange rnge = sheet.Cells[6, 1, 6 + dr.Count - 1, 1])
                {
                    rnge.Value = dr[0].EmpID;

                    rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rnge.Merge = true;
                    rnge.Style.Font.Size = 10;
                    rnge.Style.Font.Bold = false;
                    rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    rnge.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rnge.Style.Border.Right.Color.SetColor(Color.Black);

                    rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rnge.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                using (ExcelRange rnge = sheet.Cells[6, 2, 6 + dr.Count - 1, 2])
                {
                    rnge.Value = model.UserName;

                    rnge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rnge.Merge = true;
                    rnge.Style.Font.Size = 10;
                    rnge.Style.Font.Bold = false;
                    rnge.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    rnge.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rnge.Style.Border.Right.Color.SetColor(Color.Black);

                    rnge.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rnge.Style.Fill.BackgroundColor.SetColor(Color.White);
                }
                for (int i = 0; i < dr.Count; i++)
                {
                    sheet.Cells[i + 6, 3].Value = dr[i].RequestDate;
                    sheet.Cells[i + 6, 4].Value = dr[i].StatusSelected;
                }
            }
            else
            {
                for (int i = 0; i < dr.Count; i++)
                {

                    sheet.Cells[i + 6, 1].Value = dr[i].EmpID;
                    sheet.Cells[i + 6, 2].Value = dr[i].UserName;
                    sheet.Cells[i + 6, 3].Value = dr[i].RequestDate;
                    sheet.Cells[i + 6, 4].Value = dr[i].StatusSelected;
                }
            }
            excel.Save();
            return File(filepath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "DetailedReport.xlsx");
        }
        [HttpPost]
        public JsonResult ShowGrid(DetailedReport model)
        {
            ReportServices rs = new ReportServices();
            var report = rs.GetDetailedReport(model).Select(x=>new {
                EmpID = x.EmpID,
                Name = x.UserName,
                RequestDate = x.RequestDate,
                Status = x.StatusSelected
            }).ToList();
            return Json(report,JsonRequestBehavior.AllowGet);
        }
    }
}