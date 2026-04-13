using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    public partial class AddNotesForm : Form
    {
        public string NoteText { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        public AddNotesForm(DateTime selectedDate, string existingNote = "")
        {
            InitializeComponent();
            txtDate.Text = selectedDate.ToString("MMMM dd, yyyy");
            txtNote.Text = existingNote;
        }

        private void AddNotesForm_Load(object sender, EventArgs e) { }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NoteText = txtNote.Text;
            IsDeleted = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Delete this note?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                IsDeleted = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        private void txtNote_TextChanged(object sender, EventArgs e) { }
    }
}