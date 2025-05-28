namespace EDOM
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox grpExportOptions;
        private System.Windows.Forms.RadioButton rbLecturerCourse;
        private System.Windows.Forms.RadioButton rbClass;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.ComboBox cmbLecturerCourse;
        private System.Windows.Forms.ComboBox cmbClass;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvData = new DataGridView();
            flowLayoutPanel = new FlowLayoutPanel();
            btnLoad = new Button();
            btnExport = new Button();
            grpExportOptions = new GroupBox();
            rbLecturerCourse = new RadioButton();
            rbClass = new RadioButton();
            rbAll = new RadioButton();
            cmbLecturerCourse = new ComboBox();
            cmbClass = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();
            flowLayoutPanel.SuspendLayout();
            grpExportOptions.SuspendLayout();
            SuspendLayout();
            // 
            // dgvData
            // 
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvData.ColumnHeadersHeight = 34;
            dgvData.Dock = DockStyle.Fill;
            dgvData.Location = new Point(0, 125);
            dgvData.Name = "dgvData";
            dgvData.RowHeadersWidth = 62;
            dgvData.Size = new Size(1024, 325);
            dgvData.TabIndex = 0;
            // 
            // flowLayoutPanel
            // 
            flowLayoutPanel.Controls.Add(btnLoad);
            flowLayoutPanel.Controls.Add(btnExport);
            flowLayoutPanel.Dock = DockStyle.Top;
            flowLayoutPanel.Location = new Point(0, 0);
            flowLayoutPanel.Name = "flowLayoutPanel";
            flowLayoutPanel.Size = new Size(1024, 125);
            flowLayoutPanel.TabIndex = 2;
            flowLayoutPanel.Paint += flowLayoutPanel_Paint;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(3, 3);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(100, 36);
            btnLoad.TabIndex = 0;
            btnLoad.Text = "Load CSVs";
            btnLoad.Click += BtnLoad_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(109, 3);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(100, 36);
            btnExport.TabIndex = 1;
            btnExport.Text = "Export PDF";
            btnExport.Click += BtnExport_Click;
            // 
            // grpExportOptions
            // 
            grpExportOptions.Controls.Add(rbLecturerCourse);
            grpExportOptions.Controls.Add(rbClass);
            grpExportOptions.Controls.Add(rbAll);
            grpExportOptions.Controls.Add(cmbLecturerCourse);
            grpExportOptions.Controls.Add(cmbClass);
            grpExportOptions.Location = new Point(0, 40);
            grpExportOptions.Name = "grpExportOptions";
            grpExportOptions.Size = new Size(1024, 85);
            grpExportOptions.TabIndex = 1;
            grpExportOptions.TabStop = false;
            grpExportOptions.Text = "Export Options";
            grpExportOptions.Enter += grpExportOptions_Enter;
            // 
            // rbLecturerCourse
            // 
            rbLecturerCourse.Location = new Point(6, 23);
            rbLecturerCourse.Name = "rbLecturerCourse";
            rbLecturerCourse.Size = new Size(135, 56);
            rbLecturerCourse.TabIndex = 0;
            rbLecturerCourse.Text = "Lecturer";
            rbLecturerCourse.CheckedChanged += RbExportOption_CheckedChanged;
            // 
            // rbClass
            // 
            rbClass.Location = new Point(162, 39);
            rbClass.Name = "rbClass";
            rbClass.Size = new Size(100, 24);
            rbClass.TabIndex = 1;
            rbClass.Text = "Kelas";
            rbClass.CheckedChanged += RbExportOption_CheckedChanged;
            // 
            // rbAll
            // 
            rbAll.Checked = true;
            rbAll.Location = new Point(268, 39);
            rbAll.Name = "rbAll";
            rbAll.Size = new Size(100, 24);
            rbAll.TabIndex = 2;
            rbAll.TabStop = true;
            rbAll.Text = "All";
            rbAll.CheckedChanged += RbExportOption_CheckedChanged;
            // 
            // cmbLecturerCourse
            // 
            cmbLecturerCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLecturerCourse.Enabled = false;
            cmbLecturerCourse.Location = new Point(374, 36);
            cmbLecturerCourse.Name = "cmbLecturerCourse";
            cmbLecturerCourse.Size = new Size(512, 33);
            cmbLecturerCourse.TabIndex = 3;
            cmbLecturerCourse.SelectedIndexChanged += cmbLecturerCourse_SelectedIndexChanged;
            // 
            // cmbClass
            // 
            cmbClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClass.Enabled = false;
            cmbClass.Location = new Point(902, 36);
            cmbClass.Name = "cmbClass";
            cmbClass.Size = new Size(100, 33);
            cmbClass.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 450);
            Controls.Add(dgvData);
            Controls.Add(grpExportOptions);
            Controls.Add(flowLayoutPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Lecturer Evaluation System";
            ((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
            flowLayoutPanel.ResumeLayout(false);
            grpExportOptions.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}