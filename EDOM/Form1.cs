using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace EDOM
{
    public partial class Form1 : Form
    {
        private List<SurveyResponse> surveyData = new List<SurveyResponse>();
        private List<LecturerReport> aggregatedData = new List<LecturerReport>();
        private string[] questionHeaders = Array.Empty<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "CSV Files (*.csv)|*.csv"
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    surveyData.Clear();
                    aggregatedData.Clear();

                    foreach (var file in ofd.FileNames)
                    {
                        try
                        {
                            var responses = CsvService.ReadCsv(file, out var headers);
                            surveyData.AddRange(responses);
                            if (!questionHeaders.Any())
                            {
                                questionHeaders = headers;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (surveyData.Any())
                    {
                        aggregatedData = CsvService.AggregateData(surveyData, questionHeaders);
                        UpdateDataGridView();
                        UpdateComboBoxes();
                        MessageBox.Show($"{surveyData.Count} responses loaded.", "Success");
                    }
                    else
                    {
                        MessageBox.Show("No valid data loaded.", "Warning");
                    }
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (!aggregatedData.Any())
            {
                MessageBox.Show("Please load CSV data first.", "Warning");
                return;
            }

            List<LecturerReport> reportsToExport;

            if (rbLecturerCourse.Checked)
            {
                if (cmbLecturerCourse.SelectedItem == null)
                {
                    MessageBox.Show("Please select a lecturer and course.", "Warning");
                    return;
                }
                var selected = (cmbLecturerCourse.SelectedItem as dynamic);
                string lecturer = selected.Lecturer;
                string course = selected.Course;
                reportsToExport = aggregatedData
                    .Where(r => r.NamaDosen == lecturer && r.MataKuliah == course)
                    .ToList();
            }
            else if (rbClass.Checked)
            {
                if (cmbClass.SelectedItem == null)
                {
                    MessageBox.Show("Please select a class.", "Warning");
                    return;
                }
                string selectedClass = cmbClass.SelectedItem.ToString();
                reportsToExport = aggregatedData
                    .Where(r => r.KodeKelas == selectedClass)
                    .ToList();
            }
            else // rbAll.Checked
            {
                reportsToExport = aggregatedData.ToList();
            }

            if (!reportsToExport.Any())
            {
                MessageBox.Show("No data matches the selected criteria.", "Warning");
                return;
            }

            using (var sfd = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = "LecturerEvaluation.pdf"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        PdfGenerator.GeneratePdf(reportsToExport, sfd.FileName);
                        MessageBox.Show("PDF successfully created!", "Success");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to create PDF: {ex.Message}", "Error");
                    }
                }
            }
        }

        private void UpdateDataGridView()
        {
            var dt = new DataTable();
            dt.Columns.Add("Lecturer");
            dt.Columns.Add("Course");
            dt.Columns.Add("Class");
            foreach (var header in questionHeaders)
            {
                dt.Columns.Add(CsvService.CleanQuestionHeader(header));
            }

            foreach (var report in aggregatedData)
            {
                var row = dt.NewRow();
                row["Lecturer"] = report.NamaDosen;
                row["Course"] = report.MataKuliah;
                row["Class"] = report.KodeKelas;
                foreach (var kvp in report.QuestionScores)
                {
                    row[kvp.Key] = kvp.Value.Score.ToString("0.00");
                }
                dt.Rows.Add(row);
            }

            dgvData.DataSource = dt;
        }

        private void UpdateComboBoxes()
        {
            // Populate lecturer/course ComboBox
            var lecturerCourses = aggregatedData
                .Select(r => new { Lecturer = r.NamaDosen, Course = r.MataKuliah })
                .Distinct()
                .OrderBy(x => x.Lecturer)
                .ThenBy(x => x.Course)
                .ToList();
            cmbLecturerCourse.DataSource = lecturerCourses;
            cmbLecturerCourse.DisplayMember = "Lecturer";
            cmbLecturerCourse.ValueMember = "Course";
            cmbLecturerCourse.Format += (s, e) =>
            {
                var item = e.ListItem as dynamic;
                e.Value = $"{item.Lecturer} ({item.Course})";
            };
            cmbLecturerCourse.Enabled = rbLecturerCourse.Checked;

            // Populate class ComboBox
            var classes = aggregatedData
                .Select(r => r.KodeKelas)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
            cmbClass.DataSource = classes;
            cmbClass.Enabled = rbClass.Checked;
        }

        private void RbExportOption_CheckedChanged(object sender, EventArgs e)
        {
            cmbLecturerCourse.Enabled = rbLecturerCourse.Checked;
            cmbClass.Enabled = rbClass.Checked;
        }

        private void cmbLecturerCourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grpExportOptions_Enter(object sender, EventArgs e)
        {

        }
    }
}