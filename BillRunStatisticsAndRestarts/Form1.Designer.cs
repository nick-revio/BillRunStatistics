namespace BillRunStatisticsAndRestarts
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Button1 = new Button();
            CancelButton = new Button();
            MessagesListBox = new ListBox();
            checkedListBox1 = new CheckedListBox();
            button2 = new Button();
            button3 = new Button();
            label2 = new Label();
            StartDateTimePicker = new DateTimePicker();
            CreateCombinedFileCheckBox = new CheckBox();
            CheckBoxAll = new CheckBox();
            AddAllClientsSheetCheckBox = new CheckBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            label4 = new Label();
            label3 = new Label();
            checkBox1 = new CheckBox();
            dateTimePicker1 = new DateTimePicker();
            BillRunMetricsButton = new Button();
            SaveDirectoryTextBox = new TextBox();
            label1 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // Button1
            // 
            Button1.Location = new Point(23, 141);
            Button1.Margin = new Padding(2);
            Button1.Name = "Button1";
            Button1.Size = new Size(174, 20);
            Button1.TabIndex = 2;
            Button1.Text = "Get Bill Run Metrics";
            Button1.UseVisualStyleBackColor = true;
            Button1.Click += Button1_Click;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(202, 141);
            CancelButton.Margin = new Padding(2);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(102, 20);
            CancelButton.TabIndex = 3;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // MessagesListBox
            // 
            MessagesListBox.AllowDrop = true;
            MessagesListBox.CausesValidation = false;
            MessagesListBox.FormattingEnabled = true;
            MessagesListBox.ItemHeight = 15;
            MessagesListBox.Location = new Point(8, 7);
            MessagesListBox.Margin = new Padding(2);
            MessagesListBox.Name = "MessagesListBox";
            MessagesListBox.ScrollAlwaysVisible = true;
            MessagesListBox.Size = new Size(694, 79);
            MessagesListBox.TabIndex = 4;
            // 
            // checkedListBox1
            // 
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(398, 29);
            checkedListBox1.Margin = new Padding(2);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(211, 184);
            checkedListBox1.TabIndex = 5;
            // 
            // button2
            // 
            button2.Location = new Point(23, 74);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(174, 20);
            button2.TabIndex = 6;
            button2.Text = "Test Global01 Connection";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(23, 108);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(174, 20);
            button3.TabIndex = 7;
            button3.Text = "Test Client Connections";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 17);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(61, 15);
            label2.TabIndex = 9;
            label2.Text = "Start Date:";
            // 
            // StartDateTimePicker
            // 
            StartDateTimePicker.Location = new Point(98, 17);
            StartDateTimePicker.Margin = new Padding(2);
            StartDateTimePicker.Name = "StartDateTimePicker";
            StartDateTimePicker.Size = new Size(211, 23);
            StartDateTimePicker.TabIndex = 10;
            StartDateTimePicker.Value = new DateTime(2023, 2, 1, 0, 0, 0, 0);
            // 
            // CreateCombinedFileCheckBox
            // 
            CreateCombinedFileCheckBox.AutoSize = true;
            CreateCombinedFileCheckBox.Checked = true;
            CreateCombinedFileCheckBox.CheckState = CheckState.Checked;
            CreateCombinedFileCheckBox.Location = new Point(202, 75);
            CreateCombinedFileCheckBox.Name = "CreateCombinedFileCheckBox";
            CreateCombinedFileCheckBox.Size = new Size(140, 19);
            CreateCombinedFileCheckBox.TabIndex = 11;
            CreateCombinedFileCheckBox.Text = "Create Combined File";
            CreateCombinedFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // CheckBoxAll
            // 
            CheckBoxAll.AutoSize = true;
            CheckBoxAll.Checked = true;
            CheckBoxAll.CheckState = CheckState.Checked;
            CheckBoxAll.Location = new Point(398, 6);
            CheckBoxAll.Name = "CheckBoxAll";
            CheckBoxAll.Size = new Size(133, 19);
            CheckBoxAll.TabIndex = 12;
            CheckBoxAll.Text = "Check / Uncheck All";
            CheckBoxAll.UseVisualStyleBackColor = true;
            CheckBoxAll.CheckedChanged += CheckBoxAll_CheckedChanged;
            // 
            // AddAllClientsSheetCheckBox
            // 
            AddAllClientsSheetCheckBox.AutoSize = true;
            AddAllClientsSheetCheckBox.Location = new Point(202, 108);
            AddAllClientsSheetCheckBox.Margin = new Padding(2);
            AddAllClientsSheetCheckBox.Name = "AddAllClientsSheetCheckBox";
            AddAllClientsSheetCheckBox.Size = new Size(185, 19);
            AddAllClientsSheetCheckBox.TabIndex = 13;
            AddAllClientsSheetCheckBox.Text = "Combine All Clients in 1 Sheet";
            AddAllClientsSheetCheckBox.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(8, 91);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(690, 263);
            tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(StartDateTimePicker);
            tabPage1.Controls.Add(CheckBoxAll);
            tabPage1.Controls.Add(checkedListBox1);
            tabPage1.Controls.Add(AddAllClientsSheetCheckBox);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(CreateCombinedFileCheckBox);
            tabPage1.Controls.Add(Button1);
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(CancelButton);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(682, 235);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Containers";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(checkBox1);
            tabPage2.Controls.Add(dateTimePicker1);
            tabPage2.Controls.Add(BillRunMetricsButton);
            tabPage2.Controls.Add(SaveDirectoryTextBox);
            tabPage2.Controls.Add(label1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(682, 235);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "All Production";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 183);
            label4.Name = "label4";
            label4.Size = new Size(169, 15);
            label4.TabIndex = 7;
            label4.Text = "Passing in Null as the End Date";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 148);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 6;
            label3.Text = "Start Date";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(25, 210);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(99, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Include MRCs";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(89, 148);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(200, 23);
            dateTimePicker1.TabIndex = 3;
            dateTimePicker1.Value = new DateTime(2023, 1, 1, 0, 0, 0, 0);
            // 
            // BillRunMetricsButton
            // 
            BillRunMetricsButton.Location = new Point(46, 113);
            BillRunMetricsButton.Name = "BillRunMetricsButton";
            BillRunMetricsButton.Size = new Size(154, 23);
            BillRunMetricsButton.TabIndex = 2;
            BillRunMetricsButton.Text = "og.bill_run_metrics";
            BillRunMetricsButton.UseVisualStyleBackColor = true;
            BillRunMetricsButton.Click += BillRunMetricsButton_Click;
            // 
            // SaveDirectoryTextBox
            // 
            SaveDirectoryTextBox.Location = new Point(147, 45);
            SaveDirectoryTextBox.Name = "SaveDirectoryTextBox";
            SaveDirectoryTextBox.Size = new Size(216, 23);
            SaveDirectoryTextBox.TabIndex = 1;
            SaveDirectoryTextBox.Text = "C:\\Temp\\BillRunMetrics";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(46, 48);
            label1.Name = "label1";
            label1.Size = new Size(82, 15);
            label1.TabIndex = 0;
            label1.Text = "Save Directory";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(712, 366);
            Controls.Add(tabControl1);
            Controls.Add(MessagesListBox);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Form1";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button Button1;
        private Button CancelButton;
        private ListBox MessagesListBox;
        private CheckedListBox checkedListBox1;
        private Button button2;
        private Button button3;
        private Label label2;
        private DateTimePicker StartDateTimePicker;
        private CheckBox CreateCombinedFileCheckBox;
        private CheckBox CheckBoxAll;
        private CheckBox AddAllClientsSheetCheckBox;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label1;
        private Label label4;
        private Label label3;
        private CheckBox checkBox1;
        private DateTimePicker dateTimePicker1;
        private Button BillRunMetricsButton;
        private TextBox SaveDirectoryTextBox;
    }
}