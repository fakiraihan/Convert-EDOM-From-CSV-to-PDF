using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections.Generic;
using System.Linq;

namespace EDOM
{
    public class PdfGenerator
    {
        private static readonly (string Category, int[] Indices)[] Categories = new[]
        {
            ("Kompetensi Pedagogik", Enumerable.Range(0, 13).ToArray()),
            ("Kompetensi Profesional", Enumerable.Range(13, 5).ToArray()),
            ("Kepribadian Dosen", Enumerable.Range(18, 5).ToArray()),
            ("Metode Pembelajaran", Enumerable.Range(23, 5).ToArray()),
            ("Media Pembelajaran", Enumerable.Range(28, 5).ToArray())
        };

        public static void GeneratePdf(List<LecturerReport> reports, string outputPath)
        {
            using (var writer = new PdfWriter(outputPath))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                document.SetMargins(40, 40, 40, 40); // Slightly larger margins for a spacious look
                PageSize pageSize = PageSize.A4;

                // Define colors
                var headerBlue = new DeviceRgb(74, 144, 226); // Modern blue #4A90E2
                var gradientBlueStart = new DeviceRgb(135, 206, 250); // LightSkyBlue #87CEFA
                var gradientBlueEnd = new DeviceRgb(255, 255, 255); // White
                var borderGray = new DeviceRgb(211, 211, 211); // LightGray #D3D3D3
                var textGray = new DeviceRgb(80, 80, 80); // Darker gray for text #505050
                var white = ColorConstants.WHITE;

                // Load custom fonts (Roboto) or fallback to Helvetica
                PdfFont regularFont;
                PdfFont boldFont;
                PdfFont italicFont;
                try
                {
                    regularFont = PdfFontFactory.CreateFont("Roboto-Regular.ttf", PdfEncodings.IDENTITY_H);
                    boldFont = PdfFontFactory.CreateFont("Roboto-Bold.ttf", PdfEncodings.IDENTITY_H);
                    italicFont = PdfFontFactory.CreateFont("Roboto-Italic.ttf", PdfEncodings.IDENTITY_H);
                }
                catch
                {
                    regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    italicFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
                }

                int pageNumber = 0;
                foreach (var report in reports)
                {
                    pageNumber++;
                    PdfPage page = pdf.AddNewPage();
                    PdfCanvas canvas = new PdfCanvas(page);

                    // Gradient header background
                    canvas.SaveState();
                    canvas.SetFillColor(gradientBlueStart)
                          .Rectangle(0, pageSize.GetTop() - 100, pageSize.GetWidth(), 100)
                          .Fill();
                    canvas.SetFillColor(gradientBlueEnd)
                          .Rectangle(0, pageSize.GetTop() - 100, pageSize.GetWidth() * 0.7f, 100)
                          .Fill();
                    canvas.RestoreState();

                    // Add logo
                    try
                    {
                        var logo = new iText.Layout.Element.Image(ImageDataFactory.Create("logo.png"))
                            .SetWidth(UnitValue.CreatePointValue(80))
                            .SetHeight(UnitValue.CreatePointValue(80))
                            .SetFixedPosition(40, 742); // Adjusted for new margin
                        document.Add(logo);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to load logo: {ex.Message}");
                    }

                    // Header
                    document.Add(new Paragraph("Evaluasi Dosen oleh Mahasiswa")
                        .SetFont(boldFont)
                        .SetFontSize(18)
                        .SetFontColor(textGray)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(10));
                    document.Add(new Paragraph("Politeknik Siber dan Sandi Negara | Jurusan Kriptografi")
                        .SetFont(regularFont)
                        .SetFontSize(12)
                        .SetFontColor(textGray)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(5));
                    document.Add(new Paragraph("Semester Gasal T.A. 2024/2025")
                        .SetFont(regularFont)
                        .SetFontSize(12)
                        .SetFontColor(textGray)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(5));

                    // Add decorative header line
                    canvas.SetStrokeColor(headerBlue)
                          .SetLineWidth(2.5f)
                          .MoveTo(40, 692) // Adjusted for new margin
                          .LineTo(pageSize.GetWidth() - 40, 692)
                          .Stroke();

                    // Content below the line
                    document.Add(new Paragraph($"Nama Dosen: {report.NamaDosen}")
                        .SetFont(boldFont)
                        .SetFontSize(12)
                        .SetFontColor(textGray)
                        .SetMarginTop(25));
                    document.Add(new Paragraph($"Mata Kuliah: {report.MataKuliah} ({report.KodeKelas})")
                        .SetFont(boldFont)
                        .SetFontSize(12)
                        .SetFontColor(textGray)
                        .SetMarginTop(5));

                    // Table for scores
                    foreach (var (category, indices) in Categories)
                    {
                        document.Add(new Paragraph(category)
                            .SetFont(boldFont)
                            .SetFontSize(14)
                            .SetFontColor(headerBlue)
                            .SetMarginTop(20));

                        var table = new Table(UnitValue.CreatePercentArray(new float[] { 70, 15, 15 }))
                            .UseAllAvailableWidth()
                            .SetMarginTop(5)
                            .SetBorder(new SolidBorder(borderGray, 0.5f))
                            .SetBorderRadius(new BorderRadius(5)); // Subtle rounded corners

                        // Header
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph("Aspek/Pertanyaan")
                                .SetFont(boldFont)
                                .SetFontColor(white))
                            .SetBackgroundColor(headerBlue)
                            .SetBorder(new SolidBorder(borderGray, 0.5f))
                            .SetPadding(8));
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph("Nilai")
                                .SetFont(boldFont)
                                .SetFontColor(white))
                            .SetBackgroundColor(headerBlue)
                            .SetBorder(new SolidBorder(borderGray, 0.5f))
                            .SetPadding(8));
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph("Rata-Rata Kelas")
                                .SetFont(boldFont)
                                .SetFontColor(white))
                            .SetBackgroundColor(headerBlue)
                            .SetBorder(new SolidBorder(borderGray, 0.5f))
                            .SetPadding(8));

                        var questions = report.QuestionScores.ToList();
                        foreach (var index in indices)
                        {
                            if (index < questions.Count)
                            {
                                var (question, (score, classAvg)) = questions[index];
                                table.AddCell(new Cell()
                                    .Add(new Paragraph(question)
                                        .SetFont(regularFont)
                                        .SetFontColor(textGray))
                                    .SetBorder(new SolidBorder(borderGray, 0.5f))
                                    .SetPadding(6));
                                table.AddCell(new Cell()
                                    .Add(new Paragraph(score.ToString("0.00"))
                                        .SetFont(regularFont)
                                        .SetFontColor(textGray))
                                    .SetBorder(new SolidBorder(borderGray, 0.5f))
                                    .SetPadding(6));
                                table.AddCell(new Cell()
                                    .Add(new Paragraph(classAvg.ToString("0.00"))
                                        .SetFont(regularFont)
                                        .SetFontColor(textGray))
                                    .SetBorder(new SolidBorder(borderGray, 0.5f))
                                    .SetPadding(6));
                            }
                        }

                        document.Add(table);
                        document.Add(new Paragraph("\n"));
                    }

                    // Suggestions
                    if (report.Suggestions.Any())
                    {
                        document.Add(new Paragraph("Saran/Masukan:")
                            .SetFont(boldFont)
                            .SetFontSize(12)
                            .SetFontColor(headerBlue)
                            .SetMarginTop(15));
                        foreach (var saran in report.Suggestions)
                        {
                            document.Add(new Paragraph($"- {saran}")
                                .SetFont(regularFont)
                                .SetFontSize(12)
                                .SetFontColor(textGray)
                                .SetMarginTop(2));
                        }
                    }

                    // Keterangan
                    document.Add(new Paragraph("Keterangan: Penilaian dalam skala 4.")
                        .SetFont(italicFont)
                        .SetFontSize(10)
                        .SetFontColor(textGray)
                        .SetMarginTop(15));

                    // Footer with page number - Use the current page
                    PdfCanvas footerCanvas = new PdfCanvas(pdf.GetPage(pdf.GetNumberOfPages()));
                    if (footerCanvas != null)
                    {
                        footerCanvas.SetFillColor(headerBlue);
                        footerCanvas.Rectangle(40, 20, pageSize.GetWidth() - 80, 1).Fill();
                        document.Add(new Paragraph($"Halaman {pageNumber} dari {reports.Count}")
                            .SetFont(italicFont)
                            .SetFontSize(9)
                            .SetFontColor(textGray)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFixedPosition(40, 10, pageSize.GetWidth() - 80));
                    }

                    if (report != reports.Last())
                    {
                        document.Add(new AreaBreak());
                    }
                }
            }
        }
    }
}