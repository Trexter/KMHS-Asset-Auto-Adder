namespace Asset_Auto_Adder
{
    partial class Main
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
            this.label1 = new System.Windows.Forms.Label();
            this.addAssets = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.DetailsBox = new System.Windows.Forms.RichTextBox();
            this.CSVGrid = new System.Windows.Forms.DataGridView();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.filePath = new System.Windows.Forms.Label();
            this.chooseFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.assetTypetextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.CSVGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Full Path To CSV File";
            // 
            // addAssets
            // 
            this.addAssets.Location = new System.Drawing.Point(563, 4);
            this.addAssets.Name = "addAssets";
            this.addAssets.Size = new System.Drawing.Size(75, 23);
            this.addAssets.TabIndex = 3;
            this.addAssets.Text = "Add Assets";
            this.addAssets.UseVisualStyleBackColor = true;
            this.addAssets.Click += new System.EventHandler(this.addAssets_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 653);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(623, 23);
            this.progressBar.TabIndex = 4;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // DetailsBox
            // 
            this.DetailsBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.DetailsBox.ForeColor = System.Drawing.SystemColors.MenuBar;
            this.DetailsBox.Location = new System.Drawing.Point(6, 350);
            this.DetailsBox.Name = "DetailsBox";
            this.DetailsBox.Size = new System.Drawing.Size(632, 297);
            this.DetailsBox.TabIndex = 5;
            this.DetailsBox.Text = "";
            // 
            // CSVGrid
            // 
            this.CSVGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CSVGrid.Location = new System.Drawing.Point(6, 86);
            this.CSVGrid.Name = "CSVGrid";
            this.CSVGrid.Size = new System.Drawing.Size(632, 255);
            this.CSVGrid.TabIndex = 6;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // filePath
            // 
            this.filePath.AutoSize = true;
            this.filePath.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.filePath.Location = new System.Drawing.Point(12, 70);
            this.filePath.Name = "filePath";
            this.filePath.Size = new System.Drawing.Size(88, 13);
            this.filePath.TabIndex = 7;
            this.filePath.Text = "Path To CSV File";
            // 
            // chooseFile
            // 
            this.chooseFile.Location = new System.Drawing.Point(482, 4);
            this.chooseFile.Name = "chooseFile";
            this.chooseFile.Size = new System.Drawing.Size(75, 23);
            this.chooseFile.TabIndex = 8;
            this.chooseFile.Text = "Choose File";
            this.chooseFile.UseVisualStyleBackColor = true;
            this.chooseFile.Click += new System.EventHandler(this.chooseFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "KMHS Auto Asset Adder";
            // 
            // assetTypetextBox
            // 
            this.assetTypetextBox.Location = new System.Drawing.Point(347, 6);
            this.assetTypetextBox.Name = "assetTypetextBox";
            this.assetTypetextBox.Size = new System.Drawing.Size(129, 20);
            this.assetTypetextBox.TabIndex = 11;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(645, 688);
            this.Controls.Add(this.assetTypetextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chooseFile);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.CSVGrid);
            this.Controls.Add(this.DetailsBox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.addAssets);
            this.Controls.Add(this.label1);
            this.Name = "Main";
            this.Text = "Asset Adder";
            ((System.ComponentModel.ISupportInitialize)(this.CSVGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addAssets;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.RichTextBox DetailsBox;
        private System.Windows.Forms.DataGridView CSVGrid;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label filePath;
        private System.Windows.Forms.Button chooseFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox assetTypetextBox;
    }
}

