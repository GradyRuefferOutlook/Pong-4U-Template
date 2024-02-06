namespace Pong
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gameUpdateLoop = new System.Windows.Forms.Timer(this.components);
            this.startLabel = new System.Windows.Forms.Label();
            this.wTime = new System.Windows.Forms.Timer(this.components);
            this.sTime = new System.Windows.Forms.Timer(this.components);
            this.upTime = new System.Windows.Forms.Timer(this.components);
            this.downTime = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // gameUpdateLoop
            // 
            this.gameUpdateLoop.Interval = 16;
            this.gameUpdateLoop.Tick += new System.EventHandler(this.gameUpdateLoop_Tick);
            // 
            // startLabel
            // 
            this.startLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startLabel.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.Color.White;
            this.startLabel.Location = new System.Drawing.Point(188, 160);
            this.startLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(439, 197);
            this.startLabel.TabIndex = 0;
            this.startLabel.Text = "Press Space To Start or Esc to Exit";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // wTime
            // 
            this.wTime.Interval = 16;
            this.wTime.Tick += new System.EventHandler(this.wTime_Tick);
            // 
            // sTime
            // 
            this.sTime.Interval = 16;
            this.sTime.Tick += new System.EventHandler(this.sTime_Tick);
            // 
            // upTime
            // 
            this.upTime.Interval = 16;
            // 
            // downTime
            // 
            this.downTime.Interval = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(821, 554);
            this.Controls.Add(this.startLabel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pong";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer gameUpdateLoop;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Timer wTime;
        private System.Windows.Forms.Timer sTime;
        private System.Windows.Forms.Timer upTime;
        private System.Windows.Forms.Timer downTime;
    }
}

