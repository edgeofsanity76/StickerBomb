namespace StickerBomb
{
    partial class frmStickerBombGenerator
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
            pictureBox1 = new PictureBox();
            btnGenerate = new Button();
            btnCancel = new Button();
            label1 = new Label();
            btnAddImageFiles = new Button();
            label2 = new Label();
            numX = new NumericUpDown();
            label3 = new Label();
            numY = new NumericUpDown();
            label4 = new Label();
            numMaxStickersPerCell = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxStickersPerCell).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 41);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(961, 707);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(106, 12);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(75, 23);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(0, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 7;
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 6;
            // 
            // btnAddImageFiles
            // 
            btnAddImageFiles.Location = new Point(12, 12);
            btnAddImageFiles.Name = "btnAddImageFiles";
            btnAddImageFiles.Size = new Size(88, 23);
            btnAddImageFiles.TabIndex = 8;
            btnAddImageFiles.Text = "Add Images";
            btnAddImageFiles.UseVisualStyleBackColor = true;
            btnAddImageFiles.Click += btnAddImageFiles_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(187, 16);
            label2.Name = "label2";
            label2.Size = new Size(109, 15);
            label2.TabIndex = 9;
            label2.Text = "Canvass Resolution";
            // 
            // numX
            // 
            numX.Location = new Point(302, 14);
            numX.Maximum = new decimal(new int[] { 3840, 0, 0, 0 });
            numX.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
            numX.Name = "numX";
            numX.Size = new Size(120, 23);
            numX.TabIndex = 10;
            numX.Value = new decimal(new int[] { 1024, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(428, 16);
            label3.Name = "label3";
            label3.Size = new Size(14, 15);
            label3.TabIndex = 11;
            label3.Text = "X";
            // 
            // numY
            // 
            numY.Location = new Point(448, 14);
            numY.Maximum = new decimal(new int[] { 2560, 0, 0, 0 });
            numY.Minimum = new decimal(new int[] { 768, 0, 0, 0 });
            numY.Name = "numY";
            numY.Size = new Size(120, 23);
            numY.TabIndex = 12;
            numY.Value = new decimal(new int[] { 768, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(586, 16);
            label4.Name = "label4";
            label4.Size = new Size(115, 15);
            label4.TabIndex = 13;
            label4.Text = "Max Stickers Per Cell";
            // 
            // numMaxStickersPerCell
            // 
            numMaxStickersPerCell.Location = new Point(707, 14);
            numMaxStickersPerCell.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numMaxStickersPerCell.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxStickersPerCell.Name = "numMaxStickersPerCell";
            numMaxStickersPerCell.Size = new Size(120, 23);
            numMaxStickersPerCell.TabIndex = 14;
            numMaxStickersPerCell.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // frmStickerBombGenerator
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(985, 760);
            Controls.Add(numMaxStickersPerCell);
            Controls.Add(label4);
            Controls.Add(numY);
            Controls.Add(label3);
            Controls.Add(numX);
            Controls.Add(label2);
            Controls.Add(btnAddImageFiles);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnGenerate);
            Controls.Add(pictureBox1);
            Name = "frmStickerBombGenerator";
            Text = "Sticker Bomb Generator";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numX).EndInit();
            ((System.ComponentModel.ISupportInitialize)numY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxStickersPerCell).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button btnGenerate;
        private Button btnCancel;
        private Label label1;
        private Button btnAddImageFiles;
        private Label label2;
        private NumericUpDown numX;
        private Label label3;
        private NumericUpDown numY;
        private Label label4;
        private NumericUpDown numMaxStickersPerCell;
    }
}
