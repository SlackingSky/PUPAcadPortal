namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    partial class AccountsContentAdmin
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountsContentAdmin));
            pnlAccountingRecordsContent = new Panel();
            pnlARResultContainer = new Panel();
            dgvAccountingRecords = new DataGridView();
            btnARSearch = new Button();
            txtARSearchBar = new TextBox();
            pnlARUnpaidAmount = new Panel();
            pictureBox13 = new PictureBox();
            label60 = new Label();
            lblUnpaid = new Label();
            pnlARPaidAmount = new Panel();
            pictureBox12 = new PictureBox();
            label62 = new Label();
            lblPaid = new Label();
            pnlARTotalAmount = new Panel();
            pictureBox11 = new PictureBox();
            label64 = new Label();
            lblTotalAmount = new Label();
            label66 = new Label();
            label67 = new Label();
            pictureBox10 = new PictureBox();
            pnlAccountingRecordsContent.SuspendLayout();
            pnlARResultContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccountingRecords).BeginInit();
            pnlARUnpaidAmount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).BeginInit();
            pnlARPaidAmount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).BeginInit();
            pnlARTotalAmount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            SuspendLayout();
            // 
            // pnlAccountingRecordsContent
            // 
            pnlAccountingRecordsContent.AutoScroll = true;
            pnlAccountingRecordsContent.BackColor = SystemColors.ControlLight;
            pnlAccountingRecordsContent.Controls.Add(pnlARResultContainer);
            pnlAccountingRecordsContent.Controls.Add(pnlARUnpaidAmount);
            pnlAccountingRecordsContent.Controls.Add(pnlARPaidAmount);
            pnlAccountingRecordsContent.Controls.Add(pnlARTotalAmount);
            pnlAccountingRecordsContent.Controls.Add(label66);
            pnlAccountingRecordsContent.Controls.Add(label67);
            pnlAccountingRecordsContent.Controls.Add(pictureBox10);
            pnlAccountingRecordsContent.Dock = DockStyle.Fill;
            pnlAccountingRecordsContent.Location = new Point(0, 0);
            pnlAccountingRecordsContent.Name = "pnlAccountingRecordsContent";
            pnlAccountingRecordsContent.Size = new Size(1254, 719);
            pnlAccountingRecordsContent.TabIndex = 11;
            // 
            // pnlARResultContainer
            // 
            pnlARResultContainer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlARResultContainer.BackColor = Color.White;
            pnlARResultContainer.Controls.Add(dgvAccountingRecords);
            pnlARResultContainer.Controls.Add(btnARSearch);
            pnlARResultContainer.Controls.Add(txtARSearchBar);
            pnlARResultContainer.Location = new Point(32, 250);
            pnlARResultContainer.Margin = new Padding(3, 2, 3, 2);
            pnlARResultContainer.Name = "pnlARResultContainer";
            pnlARResultContainer.Size = new Size(1177, 444);
            pnlARResultContainer.TabIndex = 21;
            // 
            // dgvAccountingRecords
            // 
            dgvAccountingRecords.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAccountingRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccountingRecords.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvAccountingRecords.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvAccountingRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvAccountingRecords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvAccountingRecords.DefaultCellStyle = dataGridViewCellStyle2;
            dgvAccountingRecords.Location = new Point(14, 60);
            dgvAccountingRecords.Name = "dgvAccountingRecords";
            dgvAccountingRecords.ReadOnly = true;
            dgvAccountingRecords.RowHeadersVisible = false;
            dgvAccountingRecords.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvAccountingRecords.ScrollBars = ScrollBars.Vertical;
            dgvAccountingRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccountingRecords.Size = new Size(1146, 376);
            dgvAccountingRecords.TabIndex = 2;
            // 
            // btnARSearch
            // 
            btnARSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnARSearch.BackColor = Color.Maroon;
            btnARSearch.FlatStyle = FlatStyle.Flat;
            btnARSearch.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnARSearch.ForeColor = Color.White;
            btnARSearch.Location = new Point(1018, 17);
            btnARSearch.Margin = new Padding(3, 2, 3, 2);
            btnARSearch.Name = "btnARSearch";
            btnARSearch.Size = new Size(144, 32);
            btnARSearch.TabIndex = 1;
            btnARSearch.Text = "Search";
            btnARSearch.UseVisualStyleBackColor = false;
            // 
            // txtARSearchBar
            // 
            txtARSearchBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtARSearchBar.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtARSearchBar.Location = new Point(12, 18);
            txtARSearchBar.Margin = new Padding(3, 2, 3, 2);
            txtARSearchBar.Name = "txtARSearchBar";
            txtARSearchBar.PlaceholderText = "Search by student name, ID, or transaction type...";
            txtARSearchBar.Size = new Size(998, 29);
            txtARSearchBar.TabIndex = 0;
            // 
            // pnlARUnpaidAmount
            // 
            pnlARUnpaidAmount.BackColor = Color.White;
            pnlARUnpaidAmount.BorderStyle = BorderStyle.Fixed3D;
            pnlARUnpaidAmount.Controls.Add(pictureBox13);
            pnlARUnpaidAmount.Controls.Add(label60);
            pnlARUnpaidAmount.Controls.Add(lblUnpaid);
            pnlARUnpaidAmount.Location = new Point(860, 119);
            pnlARUnpaidAmount.Margin = new Padding(3, 2, 3, 2);
            pnlARUnpaidAmount.Name = "pnlARUnpaidAmount";
            pnlARUnpaidAmount.Size = new Size(368, 95);
            pnlARUnpaidAmount.TabIndex = 20;
            // 
            // pictureBox13
            // 
            pictureBox13.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox13.BackColor = Color.Maroon;
            pictureBox13.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox13.BorderStyle = BorderStyle.FixedSingle;
            pictureBox13.Image = Properties.Resources.calendar_10_48;
            pictureBox13.Location = new Point(264, 12);
            pictureBox13.Name = "pictureBox13";
            pictureBox13.Size = new Size(80, 66);
            pictureBox13.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox13.TabIndex = 23;
            pictureBox13.TabStop = false;
            // 
            // label60
            // 
            label60.AutoSize = true;
            label60.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label60.ForeColor = Color.DimGray;
            label60.Location = new Point(3, 8);
            label60.Name = "label60";
            label60.Size = new Size(132, 21);
            label60.TabIndex = 16;
            label60.Text = "Unpaid Amount";
            // 
            // lblUnpaid
            // 
            lblUnpaid.AutoSize = true;
            lblUnpaid.BackColor = Color.Transparent;
            lblUnpaid.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUnpaid.ForeColor = Color.Red;
            lblUnpaid.Location = new Point(3, 22);
            lblUnpaid.Name = "lblUnpaid";
            lblUnpaid.Size = new Size(100, 65);
            lblUnpaid.TabIndex = 17;
            lblUnpaid.Text = " ₱0";
            // 
            // pnlARPaidAmount
            // 
            pnlARPaidAmount.BackColor = Color.White;
            pnlARPaidAmount.BorderStyle = BorderStyle.Fixed3D;
            pnlARPaidAmount.Controls.Add(pictureBox12);
            pnlARPaidAmount.Controls.Add(label62);
            pnlARPaidAmount.Controls.Add(lblPaid);
            pnlARPaidAmount.Location = new Point(443, 119);
            pnlARPaidAmount.Margin = new Padding(3, 2, 3, 2);
            pnlARPaidAmount.Name = "pnlARPaidAmount";
            pnlARPaidAmount.Size = new Size(368, 97);
            pnlARPaidAmount.TabIndex = 19;
            // 
            // pictureBox12
            // 
            pictureBox12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox12.BackColor = Color.Maroon;
            pictureBox12.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox12.BorderStyle = BorderStyle.FixedSingle;
            pictureBox12.Image = Properties.Resources.card_in_use_48;
            pictureBox12.Location = new Point(264, 12);
            pictureBox12.Name = "pictureBox12";
            pictureBox12.Size = new Size(80, 66);
            pictureBox12.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox12.TabIndex = 22;
            pictureBox12.TabStop = false;
            // 
            // label62
            // 
            label62.AutoSize = true;
            label62.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label62.ForeColor = Color.DimGray;
            label62.Location = new Point(3, 8);
            label62.Name = "label62";
            label62.Size = new Size(110, 21);
            label62.TabIndex = 16;
            label62.Text = "Paid Amount";
            // 
            // lblPaid
            // 
            lblPaid.AutoSize = true;
            lblPaid.BackColor = Color.Transparent;
            lblPaid.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaid.ForeColor = Color.LimeGreen;
            lblPaid.Location = new Point(3, 22);
            lblPaid.Name = "lblPaid";
            lblPaid.Size = new Size(100, 65);
            lblPaid.TabIndex = 17;
            lblPaid.Text = " ₱0";
            // 
            // pnlARTotalAmount
            // 
            pnlARTotalAmount.BackColor = Color.White;
            pnlARTotalAmount.BorderStyle = BorderStyle.Fixed3D;
            pnlARTotalAmount.Controls.Add(pictureBox11);
            pnlARTotalAmount.Controls.Add(label64);
            pnlARTotalAmount.Controls.Add(lblTotalAmount);
            pnlARTotalAmount.Location = new Point(32, 122);
            pnlARTotalAmount.Margin = new Padding(3, 2, 3, 2);
            pnlARTotalAmount.Name = "pnlARTotalAmount";
            pnlARTotalAmount.Size = new Size(368, 95);
            pnlARTotalAmount.TabIndex = 18;
            // 
            // pictureBox11
            // 
            pictureBox11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox11.BackColor = Color.Maroon;
            pictureBox11.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox11.BorderStyle = BorderStyle.FixedSingle;
            pictureBox11.Image = Properties.Resources.card_inserting_48__1_;
            pictureBox11.Location = new Point(264, 12);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(80, 66);
            pictureBox11.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox11.TabIndex = 21;
            pictureBox11.TabStop = false;
            // 
            // label64
            // 
            label64.AutoSize = true;
            label64.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label64.ForeColor = Color.DimGray;
            label64.Location = new Point(3, 8);
            label64.Name = "label64";
            label64.Size = new Size(114, 21);
            label64.TabIndex = 15;
            label64.Text = "Total Amount";
            // 
            // lblTotalAmount
            // 
            lblTotalAmount.AutoSize = true;
            lblTotalAmount.BackColor = Color.Transparent;
            lblTotalAmount.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalAmount.ForeColor = Color.Black;
            lblTotalAmount.Location = new Point(3, 22);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(100, 65);
            lblTotalAmount.TabIndex = 15;
            lblTotalAmount.Text = " ₱0";
            // 
            // label66
            // 
            label66.AutoSize = true;
            label66.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label66.ForeColor = Color.Black;
            label66.Location = new Point(94, 32);
            label66.Name = "label66";
            label66.Size = new Size(291, 40);
            label66.TabIndex = 17;
            label66.Text = "Accounting Records";
            // 
            // label67
            // 
            label67.AutoSize = true;
            label67.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            label67.ForeColor = Color.DimGray;
            label67.Location = new Point(102, 73);
            label67.Name = "label67";
            label67.Size = new Size(279, 19);
            label67.TabIndex = 16;
            label67.Text = "View and manage student financial records";
            // 
            // pictureBox10
            // 
            pictureBox10.BackColor = Color.Maroon;
            pictureBox10.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox10.BorderStyle = BorderStyle.FixedSingle;
            pictureBox10.Image = (Image)resources.GetObject("pictureBox10.Image");
            pictureBox10.Location = new Point(32, 31);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new Size(59, 66);
            pictureBox10.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox10.TabIndex = 15;
            pictureBox10.TabStop = false;
            // 
            // AccountsContentAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlAccountingRecordsContent);
            Name = "AccountsContentAdmin";
            Size = new Size(1254, 719);
            Resize += AdminAccountsContent_Resize;
            pnlAccountingRecordsContent.ResumeLayout(false);
            pnlAccountingRecordsContent.PerformLayout();
            pnlARResultContainer.ResumeLayout(false);
            pnlARResultContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccountingRecords).EndInit();
            pnlARUnpaidAmount.ResumeLayout(false);
            pnlARUnpaidAmount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).EndInit();
            pnlARPaidAmount.ResumeLayout(false);
            pnlARPaidAmount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).EndInit();
            pnlARTotalAmount.ResumeLayout(false);
            pnlARTotalAmount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlAccountingRecordsContent;
        private Panel pnlARResultContainer;
        private Button btnARSearch;
        private TextBox txtARSearchBar;
        private Panel pnlARUnpaidAmount;
        private PictureBox pictureBox13;
        private Label label60;
        private Label lblUnpaid;
        private Panel pnlARPaidAmount;
        private PictureBox pictureBox12;
        private Label label62;
        private Label lblPaid;
        private Panel pnlARTotalAmount;
        private PictureBox pictureBox11;
        private Label label64;
        private Label lblTotalAmount;
        private Label label66;
        private Label label67;
        private PictureBox pictureBox10;
        private DataGridView dgvAccountingRecords;
    }
}
