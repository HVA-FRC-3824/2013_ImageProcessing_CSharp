namespace ImageProcessing
{
    partial class ImageProcessing
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
         this.bar_hMin = new System.Windows.Forms.TrackBar();
         this.lb_hMin = new System.Windows.Forms.Label();
         this.lb_vMin = new System.Windows.Forms.Label();
         this.bar_vMin = new System.Windows.Forms.TrackBar();
         this.lb_sMin = new System.Windows.Forms.Label();
         this.bar_sMin = new System.Windows.Forms.TrackBar();
         this.bar_vMax = new System.Windows.Forms.TrackBar();
         this.lb_sMax = new System.Windows.Forms.Label();
         this.bar_sMax = new System.Windows.Forms.TrackBar();
         this.lb_vMax = new System.Windows.Forms.Label();
         this.lb_hMax = new System.Windows.Forms.Label();
         this.bar_hMax = new System.Windows.Forms.TrackBar();
         this.im_mask = new System.Windows.Forms.PictureBox();
         this.im_image = new System.Windows.Forms.PictureBox();
         this.lb_hMinValue = new System.Windows.Forms.Label();
         this.lb_sMinValue = new System.Windows.Forms.Label();
         this.lb_vMinValue = new System.Windows.Forms.Label();
         this.lb_hMaxValue = new System.Windows.Forms.Label();
         this.lb_sMaxValue = new System.Windows.Forms.Label();
         this.lb_vMaxValue = new System.Windows.Forms.Label();
         this.tb_message = new System.Windows.Forms.TextBox();
         this.zgc_col = new ZedGraph.ZedGraphControl();
         this.zgc_row = new ZedGraph.ZedGraphControl();
         ((System.ComponentModel.ISupportInitialize)(this.bar_hMin)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_vMin)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_sMin)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_vMax)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_sMax)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_hMax)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.im_mask)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.im_image)).BeginInit();
         this.SuspendLayout();
         // 
         // bar_hMin
         // 
         this.bar_hMin.Location = new System.Drawing.Point(52, 12);
         this.bar_hMin.Maximum = 180;
         this.bar_hMin.Name = "bar_hMin";
         this.bar_hMin.Size = new System.Drawing.Size(220, 45);
         this.bar_hMin.TabIndex = 1;
         this.bar_hMin.Visible = false;
         this.bar_hMin.Scroll += new System.EventHandler(this.bar_Scroll);
         // 
         // lb_hMin
         // 
         this.lb_hMin.AutoSize = true;
         this.lb_hMin.Location = new System.Drawing.Point(12, 12);
         this.lb_hMin.Name = "lb_hMin";
         this.lb_hMin.Size = new System.Drawing.Size(34, 13);
         this.lb_hMin.TabIndex = 2;
         this.lb_hMin.Text = "H min";
         this.lb_hMin.Visible = false;
         // 
         // lb_vMin
         // 
         this.lb_vMin.AutoSize = true;
         this.lb_vMin.Location = new System.Drawing.Point(12, 73);
         this.lb_vMin.Name = "lb_vMin";
         this.lb_vMin.Size = new System.Drawing.Size(33, 13);
         this.lb_vMin.TabIndex = 4;
         this.lb_vMin.Text = "V min";
         this.lb_vMin.Visible = false;
         // 
         // bar_vMin
         // 
         this.bar_vMin.Location = new System.Drawing.Point(52, 73);
         this.bar_vMin.Maximum = 255;
         this.bar_vMin.Name = "bar_vMin";
         this.bar_vMin.Size = new System.Drawing.Size(220, 45);
         this.bar_vMin.TabIndex = 3;
         this.bar_vMin.Visible = false;
         this.bar_vMin.Scroll += new System.EventHandler(this.bar_Scroll);
         // 
         // lb_sMin
         // 
         this.lb_sMin.AutoSize = true;
         this.lb_sMin.Location = new System.Drawing.Point(12, 43);
         this.lb_sMin.Name = "lb_sMin";
         this.lb_sMin.Size = new System.Drawing.Size(33, 13);
         this.lb_sMin.TabIndex = 6;
         this.lb_sMin.Text = "S min";
         this.lb_sMin.Visible = false;
         // 
         // bar_sMin
         // 
         this.bar_sMin.Location = new System.Drawing.Point(52, 43);
         this.bar_sMin.Maximum = 255;
         this.bar_sMin.Name = "bar_sMin";
         this.bar_sMin.Size = new System.Drawing.Size(220, 45);
         this.bar_sMin.TabIndex = 5;
         this.bar_sMin.Visible = false;
         this.bar_sMin.Scroll += new System.EventHandler(this.bar_Scroll);
         // 
         // bar_vMax
         // 
         this.bar_vMax.Location = new System.Drawing.Point(52, 185);
         this.bar_vMax.Maximum = 255;
         this.bar_vMax.Name = "bar_vMax";
         this.bar_vMax.Size = new System.Drawing.Size(220, 45);
         this.bar_vMax.TabIndex = 9;
         this.bar_vMax.Visible = false;
         this.bar_vMax.Scroll += new System.EventHandler(this.bar_Scroll);
         // 
         // lb_sMax
         // 
         this.lb_sMax.AutoSize = true;
         this.lb_sMax.Location = new System.Drawing.Point(12, 155);
         this.lb_sMax.Name = "lb_sMax";
         this.lb_sMax.Size = new System.Drawing.Size(36, 13);
         this.lb_sMax.TabIndex = 12;
         this.lb_sMax.Text = "S max";
         this.lb_sMax.Visible = false;
         // 
         // bar_sMax
         // 
         this.bar_sMax.Location = new System.Drawing.Point(52, 155);
         this.bar_sMax.Maximum = 255;
         this.bar_sMax.Name = "bar_sMax";
         this.bar_sMax.Size = new System.Drawing.Size(220, 45);
         this.bar_sMax.TabIndex = 11;
         this.bar_sMax.Visible = false;
         this.bar_sMax.Scroll += new System.EventHandler(this.bar_Scroll);
         // 
         // lb_vMax
         // 
         this.lb_vMax.AutoSize = true;
         this.lb_vMax.Location = new System.Drawing.Point(12, 185);
         this.lb_vMax.Name = "lb_vMax";
         this.lb_vMax.Size = new System.Drawing.Size(36, 13);
         this.lb_vMax.TabIndex = 10;
         this.lb_vMax.Text = "V max";
         this.lb_vMax.Visible = false;
         // 
         // lb_hMax
         // 
         this.lb_hMax.AutoSize = true;
         this.lb_hMax.Location = new System.Drawing.Point(12, 124);
         this.lb_hMax.Name = "lb_hMax";
         this.lb_hMax.Size = new System.Drawing.Size(37, 13);
         this.lb_hMax.TabIndex = 8;
         this.lb_hMax.Text = "H max";
         this.lb_hMax.Visible = false;
         // 
         // bar_hMax
         // 
         this.bar_hMax.Location = new System.Drawing.Point(52, 124);
         this.bar_hMax.Maximum = 180;
         this.bar_hMax.Name = "bar_hMax";
         this.bar_hMax.Size = new System.Drawing.Size(220, 45);
         this.bar_hMax.TabIndex = 7;
         this.bar_hMax.Visible = false;
         this.bar_hMax.Scroll += new System.EventHandler(this.bar_Scroll);
         // 
         // im_mask
         // 
         this.im_mask.Location = new System.Drawing.Point(370, 259);
         this.im_mask.Name = "im_mask";
         this.im_mask.Size = new System.Drawing.Size(320, 240);
         this.im_mask.TabIndex = 13;
         this.im_mask.TabStop = false;
         // 
         // im_image
         // 
         this.im_image.Location = new System.Drawing.Point(370, 13);
         this.im_image.Name = "im_image";
         this.im_image.Size = new System.Drawing.Size(320, 240);
         this.im_image.TabIndex = 14;
         this.im_image.TabStop = false;
         // 
         // lb_hMinValue
         // 
         this.lb_hMinValue.AutoSize = true;
         this.lb_hMinValue.Location = new System.Drawing.Point(279, 13);
         this.lb_hMinValue.Name = "lb_hMinValue";
         this.lb_hMinValue.Size = new System.Drawing.Size(13, 13);
         this.lb_hMinValue.TabIndex = 15;
         this.lb_hMinValue.Text = "0";
         this.lb_hMinValue.Visible = false;
         // 
         // lb_sMinValue
         // 
         this.lb_sMinValue.AutoSize = true;
         this.lb_sMinValue.Location = new System.Drawing.Point(279, 43);
         this.lb_sMinValue.Name = "lb_sMinValue";
         this.lb_sMinValue.Size = new System.Drawing.Size(13, 13);
         this.lb_sMinValue.TabIndex = 16;
         this.lb_sMinValue.Text = "0";
         this.lb_sMinValue.Visible = false;
         // 
         // lb_vMinValue
         // 
         this.lb_vMinValue.AutoSize = true;
         this.lb_vMinValue.Location = new System.Drawing.Point(279, 73);
         this.lb_vMinValue.Name = "lb_vMinValue";
         this.lb_vMinValue.Size = new System.Drawing.Size(13, 13);
         this.lb_vMinValue.TabIndex = 17;
         this.lb_vMinValue.Text = "0";
         this.lb_vMinValue.Visible = false;
         // 
         // lb_hMaxValue
         // 
         this.lb_hMaxValue.AutoSize = true;
         this.lb_hMaxValue.Location = new System.Drawing.Point(279, 124);
         this.lb_hMaxValue.Name = "lb_hMaxValue";
         this.lb_hMaxValue.Size = new System.Drawing.Size(13, 13);
         this.lb_hMaxValue.TabIndex = 18;
         this.lb_hMaxValue.Text = "0";
         this.lb_hMaxValue.Visible = false;
         // 
         // lb_sMaxValue
         // 
         this.lb_sMaxValue.AutoSize = true;
         this.lb_sMaxValue.Location = new System.Drawing.Point(279, 155);
         this.lb_sMaxValue.Name = "lb_sMaxValue";
         this.lb_sMaxValue.Size = new System.Drawing.Size(13, 13);
         this.lb_sMaxValue.TabIndex = 19;
         this.lb_sMaxValue.Text = "0";
         this.lb_sMaxValue.Visible = false;
         // 
         // lb_vMaxValue
         // 
         this.lb_vMaxValue.AutoSize = true;
         this.lb_vMaxValue.Location = new System.Drawing.Point(279, 185);
         this.lb_vMaxValue.Name = "lb_vMaxValue";
         this.lb_vMaxValue.Size = new System.Drawing.Size(13, 13);
         this.lb_vMaxValue.TabIndex = 20;
         this.lb_vMaxValue.Text = "0";
         this.lb_vMaxValue.Visible = false;
         // 
         // tb_message
         // 
         this.tb_message.Enabled = false;
         this.tb_message.Location = new System.Drawing.Point(15, 222);
         this.tb_message.Multiline = true;
         this.tb_message.Name = "tb_message";
         this.tb_message.ScrollBars = System.Windows.Forms.ScrollBars.Both;
         this.tb_message.Size = new System.Drawing.Size(340, 276);
         this.tb_message.TabIndex = 21;
         // 
         // zgc_col
         // 
         this.zgc_col.Location = new System.Drawing.Point(709, 13);
         this.zgc_col.Name = "zgc_col";
         this.zgc_col.ScrollGrace = 0D;
         this.zgc_col.ScrollMaxX = 0D;
         this.zgc_col.ScrollMaxY = 0D;
         this.zgc_col.ScrollMaxY2 = 0D;
         this.zgc_col.ScrollMinX = 0D;
         this.zgc_col.ScrollMinY = 0D;
         this.zgc_col.ScrollMinY2 = 0D;
         this.zgc_col.Size = new System.Drawing.Size(338, 240);
         this.zgc_col.TabIndex = 66;
         // 
         // zgc_row
         // 
         this.zgc_row.Location = new System.Drawing.Point(709, 259);
         this.zgc_row.Name = "zgc_row";
         this.zgc_row.ScrollGrace = 0D;
         this.zgc_row.ScrollMaxX = 0D;
         this.zgc_row.ScrollMaxY = 0D;
         this.zgc_row.ScrollMaxY2 = 0D;
         this.zgc_row.ScrollMinX = 0D;
         this.zgc_row.ScrollMinY = 0D;
         this.zgc_row.ScrollMinY2 = 0D;
         this.zgc_row.Size = new System.Drawing.Size(338, 239);
         this.zgc_row.TabIndex = 67;
         // 
         // ImageProcessing
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1059, 502);
         this.Controls.Add(this.zgc_row);
         this.Controls.Add(this.zgc_col);
         this.Controls.Add(this.tb_message);
         this.Controls.Add(this.lb_vMaxValue);
         this.Controls.Add(this.lb_sMaxValue);
         this.Controls.Add(this.lb_hMaxValue);
         this.Controls.Add(this.lb_vMinValue);
         this.Controls.Add(this.lb_sMinValue);
         this.Controls.Add(this.lb_hMinValue);
         this.Controls.Add(this.im_image);
         this.Controls.Add(this.im_mask);
         this.Controls.Add(this.bar_vMax);
         this.Controls.Add(this.lb_sMax);
         this.Controls.Add(this.bar_sMax);
         this.Controls.Add(this.lb_vMax);
         this.Controls.Add(this.lb_hMax);
         this.Controls.Add(this.bar_hMax);
         this.Controls.Add(this.bar_vMin);
         this.Controls.Add(this.lb_sMin);
         this.Controls.Add(this.bar_sMin);
         this.Controls.Add(this.lb_vMin);
         this.Controls.Add(this.lb_hMin);
         this.Controls.Add(this.bar_hMin);
         this.Name = "ImageProcessing";
         this.Text = "Image Processing";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageProcessing_FormClosing);
         ((System.ComponentModel.ISupportInitialize)(this.bar_hMin)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_vMin)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_sMin)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_vMax)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_sMax)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.bar_hMax)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.im_mask)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.im_image)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar bar_hMin;
        private System.Windows.Forms.Label lb_hMin;
        private System.Windows.Forms.Label lb_vMin;
        private System.Windows.Forms.TrackBar bar_vMin;
        private System.Windows.Forms.Label lb_sMin;
        private System.Windows.Forms.TrackBar bar_sMin;
        private System.Windows.Forms.TrackBar bar_vMax;
        private System.Windows.Forms.Label lb_sMax;
        private System.Windows.Forms.TrackBar bar_sMax;
        private System.Windows.Forms.Label lb_vMax;
        private System.Windows.Forms.Label lb_hMax;
        private System.Windows.Forms.TrackBar bar_hMax;
        private System.Windows.Forms.PictureBox im_mask;
        private System.Windows.Forms.PictureBox im_image;
        private System.Windows.Forms.Label lb_hMinValue;
        private System.Windows.Forms.Label lb_sMinValue;
        private System.Windows.Forms.Label lb_vMinValue;
        private System.Windows.Forms.Label lb_hMaxValue;
        private System.Windows.Forms.Label lb_sMaxValue;
        private System.Windows.Forms.Label lb_vMaxValue;
        private System.Windows.Forms.TextBox tb_message;
        private ZedGraph.ZedGraphControl zgc_col;
        private ZedGraph.ZedGraphControl zgc_row;
    }
}

