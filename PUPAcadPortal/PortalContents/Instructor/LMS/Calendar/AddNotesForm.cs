using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PUPAcadPortal
{
    public partial class AddNotesForm : Form
    {
        public string NoteText { get; private set; } = "";
        public bool IsDeleted { get; private set; } = false;

        public AddNotesForm(DateTime selectedDate, string existingNote = "")
        {
            InitializeComponent();
            lblDateVal.Text = selectedDate.ToString("MMMM dd, yyyy");
            txtNote.Text = existingNote;
        }

        private void AddNotesForm_Load(object sender, EventArgs e) { }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NoteText = txtNote.Text.Trim();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this note?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IsDeleted = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        private void txtNote_TextChanged(object sender, EventArgs e) { }
    }
}