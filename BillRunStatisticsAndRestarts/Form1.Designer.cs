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
            SuspendLayout();
            // 
            // Button1
            // 
            Button1.Location = new Point(54, 466);
            Button1.Name = "Button1";
            Button1.Size = new Size(248, 34);
            Button1.TabIndex = 2;
            Button1.Text = "Get Metrics";
            Button1.UseVisualStyleBackColor = true;
            Button1.Click += Button1_Click;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(324, 466);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(146, 34);
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
            MessagesListBox.ItemHeight = 25;
            MessagesListBox.Location = new Point(12, 12);
            MessagesListBox.Name = "MessagesListBox";
            MessagesListBox.ScrollAlwaysVisible = true;
            MessagesListBox.Size = new Size(990, 129);
            MessagesListBox.TabIndex = 4;
            // 
            // checkedListBox1
            // 
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(702, 160);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(300, 340);
            checkedListBox1.TabIndex = 5;
            // 
            // button2
            // 
            button2.Location = new Point(54, 352);
            button2.Name = "button2";
            button2.Size = new Size(248, 34);
            button2.TabIndex = 6;
            button2.Text = "Test Global01 Connection";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(54, 408);
            button3.Name = "button3";
            button3.Size = new Size(248, 34);
            button3.TabIndex = 7;
            button3.Text = "Test Client Connections";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(54, 170);
            label2.Name = "label2";
            label2.Size = new Size(94, 25);
            label2.TabIndex = 9;
            label2.Text = "Start Date:";
            // 
            // StartDateTimePicker
            // 
            StartDateTimePicker.Location = new Point(154, 170);
            StartDateTimePicker.Name = "StartDateTimePicker";
            StartDateTimePicker.Size = new Size(300, 31);
            StartDateTimePicker.TabIndex = 10;
            StartDateTimePicker.Value = new DateTime(2023, 2, 1, 0, 0, 0, 0);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1014, 524);
            Controls.Add(StartDateTimePicker);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(checkedListBox1);
            Controls.Add(MessagesListBox);
            Controls.Add(CancelButton);
            Controls.Add(Button1);
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
    }
}