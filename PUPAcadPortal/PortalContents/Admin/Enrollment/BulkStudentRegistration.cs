using CsvHelper;
using CsvHelper.Configuration;
using PUPAcadPortal.Data;
using PUPAcadPortal.Models;
using PUPAcadPortal.Services;
using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class BulkStudentRegistration : Form
    {
        private List<StudentRegistrationData> _pendingStudents;

        public BulkStudentRegistration()
        {
            InitializeComponent();
            _pendingStudents = new List<StudentRegistrationData>();
            dgvStudents.AutoGenerateColumns = false;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select CSV File",
                Filter = "CSV Files (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<string> errorLog = new List<string>();

                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        ReadingExceptionOccurred = args =>
                        {
                            errorLog.Add($"Row {args.Exception.Context.Parser.Row}: Invalid data found.");
                            return false;
                        }
                    };

                    using (var reader = new StreamReader(openFileDialog.FileName))
                    using (var csv = new CsvReader(reader, config))
                    {
                        _pendingStudents = csv.GetRecords<StudentRegistrationData>().ToList();

                        dgvStudents.DataSource = null;
                        dgvStudents.DataSource = _pendingStudents;

                        if (errorLog.Count > 0)
                        {
                            string errors = string.Join("\n", errorLog.Take(10));
                            if (errorLog.Count > 10) errors += $"\n...and {errorLog.Count - 10} more.";

                            MessageBox.Show(
                                $"Loaded {_pendingStudents.Count} valid records.\n\n" +
                                $"However, {errorLog.Count} rows were skipped due to bad data:\n{errors}",
                                "Import Finished with Warnings",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show(
                                $"Loaded {_pendingStudents.Count} records. Please review before registering.",
                                "Import Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open the CSV file. \n\nError: {ex.Message}", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClearTable_Click(object sender, EventArgs e)
        {
            if (_pendingStudents != null)
            {
                _pendingStudents.Clear();
            }

            dgvStudents.DataSource = null;
            MessageBox.Show("Table cleared.", "Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnRegisterAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to register all students in the table? This action cannot be undone.", "Confirm Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            if (_pendingStudents == null || _pendingStudents.Count == 0)
            {
                MessageBox.Show("No data to register. Please import a CSV first.", "Empty Batch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.FormClosing += CloseApp.Cancel_Closing;
            Application.UseWaitCursor = true;
            btnRegisterAll.Text = $"Registering {_pendingStudents.Count} students...";
            btnRegisterAll.Enabled = false;

            try
            {
                var service = new StudentRegistrationService();

                var result = await service.RegisterBulkStudents(_pendingStudents);

                this.SafeUIUpdate(() =>
                {
                    _pendingStudents.Clear();
                    dgvStudents.DataSource = null;

                    if (result.SkippedRecords.Count > 0)
                    {
                        string msg = $"Registration Complete.\n\n" +
                                     $"Successfully Registered: {result.Processed} students.\n" +
                                     $"Skipped Duplicates: {result.SkippedRecords.Count} students.\n\n" +
                                     $"Would you like to download an Error Report to see exactly which students were skipped?";

                        var dialog = MessageBox.Show(msg, "Duplicates Found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (dialog == DialogResult.Yes)
                        {
                            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File|*.csv", FileName = $"Skipped_Report_{DateTime.Now:yyyyMMdd}.csv" })
                            {
                                if (sfd.ShowDialog() == DialogResult.OK)
                                {
                                    using (var writer = new StreamWriter(sfd.FileName))
                                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                                    {
                                        csv.WriteRecords(result.SkippedRecords);
                                    }
                                    MessageBox.Show("Error report saved successfully! You can correct these rows and upload them later.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Success! The system generated credentials and perfectly registered {result.Processed} students into the database.",
                                "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                });
            }
            catch (Exception ex)
            {
                this.SafeUIUpdate(() =>
                {
                    MessageBox.Show($"A critical error occurred during bulk registration:\n\n{ex.Message}",
                            "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
            finally
            {
                this.SafeUIUpdate(() =>
                {
                    this.FormClosing -= CloseApp.Cancel_Closing;
                    Application.UseWaitCursor = false;
                    btnRegisterAll.Enabled = true;
                    btnRegisterAll.Text = "Register All Students";
                });
            }
        }
    }
}
