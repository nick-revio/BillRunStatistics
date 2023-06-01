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
            SuspendLayout();
            // 
            // Button1
            // 
            Button1.Location = new Point(38, 280);
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
            CancelButton.Location = new Point(227, 280);
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
            checkedListBox1.Location = new Point(491, 114);
            checkedListBox1.Margin = new Padding(2);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(211, 202);
            checkedListBox1.TabIndex = 5;
            // 
            // button2
            // 
            button2.Location = new Point(38, 211);
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
            button3.Location = new Point(38, 245);
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
            label2.Location = new Point(38, 102);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(61, 15);
            label2.TabIndex = 9;
            label2.Text = "Start Date:";
            // 
            // StartDateTimePicker
            // 
            StartDateTimePicker.Location = new Point(108, 102);
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
            CreateCombinedFileCheckBox.Location = new Point(227, 245);
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
            CheckBoxAll.Location = new Point(491, 91);
            CheckBoxAll.Name = "CheckBoxAll";
            CheckBoxAll.Size = new Size(133, 19);
            CheckBoxAll.TabIndex = 12;
            CheckBoxAll.Text = "Check / Uncheck All";
            CheckBoxAll.UseVisualStyleBackColor = true;
            CheckBoxAll.CheckedChanged += CheckBoxAll_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(710, 327);
            Controls.Add(CheckBoxAll);
            Controls.Add(CreateCombinedFileCheckBox);
            Controls.Add(StartDateTimePicker);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(checkedListBox1);
            Controls.Add(MessagesListBox);
            Controls.Add(CancelButton);
            Controls.Add(Button1);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
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
    }
}