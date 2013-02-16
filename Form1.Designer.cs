namespace ImageProcessing
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
            this.btn_start = new System.Windows.Forms.Button();
            this.bar_hMin = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bar_vMin = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.bar_sMin = new System.Windows.Forms.TrackBar();
            this.bar_vMax = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.bar_sMax = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bar_hMax = new System.Windows.Forms.TrackBar();
            this.im_mask = new System.Windows.Forms.PictureBox();
            this.im_image = new System.Windows.Forms.PictureBox();
            this.lb_hMin = new System.Windows.Forms.Label();
            this.lb_sMin = new System.Windows.Forms.Label();
            this.lb_vMin = new System.Windows.Forms.Label();
            this.lb_hMax = new System.Windows.Forms.Label();
            this.lb_sMax = new System.Windows.Forms.Label();
            this.lb_vMax = new System.Windows.Forms.Label();
            this.bar_servo = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bar_hMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_vMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_sMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_vMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_sMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_hMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.im_mask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.im_image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_servo)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(144, 236);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(128, 23);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "Start Image Processing";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // bar_hMin
            // 
            this.bar_hMin.Location = new System.Drawing.Point(52, 12);
            this.bar_hMin.Maximum = 180;
            this.bar_hMin.Name = "bar_hMin";
            this.bar_hMin.Size = new System.Drawing.Size(220, 45);
            this.bar_hMin.TabIndex = 1;
            this.bar_hMin.ValueChanged += new System.EventHandler(this.bar_hMin_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "H min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "V min";
            // 
            // bar_vMin
            // 
            this.bar_vMin.Location = new System.Drawing.Point(52, 73);
            this.bar_vMin.Maximum = 255;
            this.bar_vMin.Name = "bar_vMin";
            this.bar_vMin.Size = new System.Drawing.Size(220, 45);
            this.bar_vMin.TabIndex = 3;
            this.bar_vMin.ValueChanged += new System.EventHandler(this.bar_vMin_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "S min";
            // 
            // bar_sMin
            // 
            this.bar_sMin.Location = new System.Drawing.Point(52, 43);
            this.bar_sMin.Maximum = 255;
            this.bar_sMin.Name = "bar_sMin";
            this.bar_sMin.Size = new System.Drawing.Size(220, 45);
            this.bar_sMin.TabIndex = 5;
            this.bar_sMin.ValueChanged += new System.EventHandler(this.bar_sMin_ValueChanged);
            // 
            // bar_vMax
            // 
            this.bar_vMax.Location = new System.Drawing.Point(52, 185);
            this.bar_vMax.Maximum = 255;
            this.bar_vMax.Name = "bar_vMax";
            this.bar_vMax.Size = new System.Drawing.Size(220, 45);
            this.bar_vMax.TabIndex = 9;
            this.bar_vMax.ValueChanged += new System.EventHandler(this.bar_vMax_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "S max";
            // 
            // bar_sMax
            // 
            this.bar_sMax.Location = new System.Drawing.Point(52, 155);
            this.bar_sMax.Maximum = 255;
            this.bar_sMax.Name = "bar_sMax";
            this.bar_sMax.Size = new System.Drawing.Size(220, 45);
            this.bar_sMax.TabIndex = 11;
            this.bar_sMax.ValueChanged += new System.EventHandler(this.bar_sMax_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "V max";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "H max";
            // 
            // bar_hMax
            // 
            this.bar_hMax.Location = new System.Drawing.Point(52, 124);
            this.bar_hMax.Maximum = 180;
            this.bar_hMax.Name = "bar_hMax";
            this.bar_hMax.Size = new System.Drawing.Size(220, 45);
            this.bar_hMax.TabIndex = 7;
            this.bar_hMax.ValueChanged += new System.EventHandler(this.bar_hMax_ValueChanged);
            // 
            // im_mask
            // 
            this.im_mask.Location = new System.Drawing.Point(591, 258);
            this.im_mask.Name = "im_mask";
            this.im_mask.Size = new System.Drawing.Size(320, 240);
            this.im_mask.TabIndex = 13;
            this.im_mask.TabStop = false;
            // 
            // im_image
            // 
            this.im_image.Location = new System.Drawing.Point(591, 12);
            this.im_image.Name = "im_image";
            this.im_image.Size = new System.Drawing.Size(320, 240);
            this.im_image.TabIndex = 14;
            this.im_image.TabStop = false;
            // 
            // lb_hMin
            // 
            this.lb_hMin.AutoSize = true;
            this.lb_hMin.Location = new System.Drawing.Point(279, 13);
            this.lb_hMin.Name = "lb_hMin";
            this.lb_hMin.Size = new System.Drawing.Size(13, 13);
            this.lb_hMin.TabIndex = 15;
            this.lb_hMin.Text = "0";
            // 
            // lb_sMin
            // 
            this.lb_sMin.AutoSize = true;
            this.lb_sMin.Location = new System.Drawing.Point(279, 43);
            this.lb_sMin.Name = "lb_sMin";
            this.lb_sMin.Size = new System.Drawing.Size(13, 13);
            this.lb_sMin.TabIndex = 16;
            this.lb_sMin.Text = "0";
            // 
            // lb_vMin
            // 
            this.lb_vMin.AutoSize = true;
            this.lb_vMin.Location = new System.Drawing.Point(279, 73);
            this.lb_vMin.Name = "lb_vMin";
            this.lb_vMin.Size = new System.Drawing.Size(13, 13);
            this.lb_vMin.TabIndex = 17;
            this.lb_vMin.Text = "0";
            // 
            // lb_hMax
            // 
            this.lb_hMax.AutoSize = true;
            this.lb_hMax.Location = new System.Drawing.Point(279, 124);
            this.lb_hMax.Name = "lb_hMax";
            this.lb_hMax.Size = new System.Drawing.Size(13, 13);
            this.lb_hMax.TabIndex = 18;
            this.lb_hMax.Text = "0";
            // 
            // lb_sMax
            // 
            this.lb_sMax.AutoSize = true;
            this.lb_sMax.Location = new System.Drawing.Point(279, 155);
            this.lb_sMax.Name = "lb_sMax";
            this.lb_sMax.Size = new System.Drawing.Size(13, 13);
            this.lb_sMax.TabIndex = 19;
            this.lb_sMax.Text = "0";
            // 
            // lb_vMax
            // 
            this.lb_vMax.AutoSize = true;
            this.lb_vMax.Location = new System.Drawing.Point(279, 185);
            this.lb_vMax.Name = "lb_vMax";
            this.lb_vMax.Size = new System.Drawing.Size(13, 13);
            this.lb_vMax.TabIndex = 20;
            this.lb_vMax.Text = "0";
            // 
            // bar_servo
            // 
            this.bar_servo.Location = new System.Drawing.Point(52, 317);
            this.bar_servo.Maximum = 100;
            this.bar_servo.Name = "bar_servo";
            this.bar_servo.Size = new System.Drawing.Size(220, 45);
            this.bar_servo.TabIndex = 21;
            this.bar_servo.ValueChanged += new System.EventHandler(this.bar_servo_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 317);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Servo";
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(15, 235);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 23;
            this.btn_stop.Text = "Stop Image Processing";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 502);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bar_servo);
            this.Controls.Add(this.lb_vMax);
            this.Controls.Add(this.lb_sMax);
            this.Controls.Add(this.lb_hMax);
            this.Controls.Add(this.lb_vMin);
            this.Controls.Add(this.lb_sMin);
            this.Controls.Add(this.lb_hMin);
            this.Controls.Add(this.im_image);
            this.Controls.Add(this.im_mask);
            this.Controls.Add(this.bar_vMax);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bar_sMax);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bar_hMax);
            this.Controls.Add(this.bar_vMin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bar_sMin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bar_hMin);
            this.Controls.Add(this.btn_start);
            this.Name = "Form1";
            this.Text = "Image Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.bar_hMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_vMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_sMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_vMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_sMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_hMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.im_mask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.im_image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar_servo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.TrackBar bar_hMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar bar_vMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar bar_sMin;
        private System.Windows.Forms.TrackBar bar_vMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar bar_sMax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar bar_hMax;
        private System.Windows.Forms.PictureBox im_mask;
        private System.Windows.Forms.PictureBox im_image;
        private System.Windows.Forms.Label lb_hMin;
        private System.Windows.Forms.Label lb_sMin;
        private System.Windows.Forms.Label lb_vMin;
        private System.Windows.Forms.Label lb_hMax;
        private System.Windows.Forms.Label lb_sMax;
        private System.Windows.Forms.Label lb_vMax;
        private System.Windows.Forms.TrackBar bar_servo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_stop;
    }
}

