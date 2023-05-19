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
            clientTextBox = new TextBox();
            label1 = new Label();
            Button1 = new Button();
            SuspendLayout();
            // 
            // clientTextBox
            // 
            clientTextBox.Location = new Point(231, 91);
            clientTextBox.Name = "clientTextBox";
            clientTextBox.Size = new Size(288, 31);
            clientTextBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(120, 91);
            label1.Name = "label1";
            label1.Size = new Size(105, 25);
            label1.TabIndex = 1;
            label1.Text = "Enter Client:";
            // 
            // Button1
            // 
            Button1.Location = new Point(143, 303);
            Button1.Name = "Button1";
            Button1.Size = new Size(112, 34);
            Button1.TabIndex = 2;
            Button1.Text = "Do Stuff";
            Button1.UseVisualStyleBackColor = true;
            Button1.Click += Button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Button1);
            Controls.Add(label1);
            Controls.Add(clientTextBox);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox clientTextBox;
        private Label label1;
        private Button Button1;
    }
}