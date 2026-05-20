using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PHAddress
{
    public partial class PHAddressFields : UserControl
    {
        // 1. Declare layout elements internally
        private FlowLayoutPanel flowLayoutPanel1;
        private ComboBox cmbRegions;
        private ComboBox cmbProvinces;
        private ComboBox cmbCities;
        private ComboBox cmbBarangays;
        private TextBox txtPostal;

        // 2. Public Read-Only Properties to expose selected names/codes to your Main Form
        public string SelectedAddressLine1 => ((TextBox)this.Controls.Find("txtAddress1", true)[0]).Text.Trim();
        public string SelectedAddressLine2 => ((TextBox)this.Controls.Find("txtAddress2", true)[0]).Text.Trim();
        public string SelectedPostalCode => ((TextBox)this.Controls.Find("txtPostal", true)[0]).Text.Trim();
        public string SelectedRegionCode => getSelectedCode(cmbRegions);
        public string SelectedRegionName => cmbRegions.SelectedIndex != -1 ? cmbRegions.Text : "";

        public string SelectedProvinceCode => getSelectedCode(cmbProvinces);
        public string SelectedProvinceName => cmbProvinces.SelectedIndex != -1 ? cmbProvinces.Text : "";

        public string SelectedCityCode => getSelectedCode(cmbCities);
        public string SelectedCityName => cmbCities.SelectedIndex != -1 ? cmbCities.Text : "";

        public string SelectedBarangayCode => getSelectedCode(cmbBarangays);
        public string SelectedBarangayName => cmbBarangays.SelectedIndex != -1 ? cmbBarangays.Text : "";

        public PHAddressFields()
        {
            InitializeComponentLayout();
            SetRegistrationFontsTo12pt();
        }

        // Programmatic UI creation removes the need for an accompanying .Designer.cs file
        private void InitializeComponentLayout()
        {
            this.Load += PHAddressFields_Load;
            // Address Panel
            Panel addressPanel = new Panel();
            addressPanel.Location = new Point(0, 0);
            addressPanel.Size = new Size(850, 250);
            addressPanel.Name = "addressPanel";

            // Address Line 1 (Required)
            System.Windows.Forms.Label lblAddress1 = new System.Windows.Forms.Label();
            lblAddress1.Text = "Address Line 1:*";
            lblAddress1.Location = new Point(0, 0);
            lblAddress1.Size = new Size(120, 25);
            lblAddress1.ForeColor = Color.Maroon;

            TextBox txtAddress1 = new TextBox();
            txtAddress1.Name = "txtAddress1";
            txtAddress1.Location = new Point(130, 0);
            txtAddress1.Size = new Size(500, 23);
            txtAddress1.PlaceholderText = "House/Unit No., Street Name, Subdivision";

            // Address Line 2 (Optional)
            System.Windows.Forms.Label lblAddress2 = new System.Windows.Forms.Label();
            lblAddress2.Text = "Address Line 2:";
            lblAddress2.Location = new Point(0, 35);
            lblAddress2.Size = new Size(120, 25);

            TextBox txtAddress2 = new TextBox();
            txtAddress2.Name = "txtAddress2";
            txtAddress2.Location = new Point(130, 35);
            txtAddress2.Size = new Size(500, 23);
            txtAddress2.PlaceholderText = "Building, Barangay (Optional)";

            // "Same as Address 1" Checkbox
            CheckBox chkSameAddress = new CheckBox();
            chkSameAddress.Text = "Same as Address Line 1";
            chkSameAddress.Location = new Point(130, 65);
            chkSameAddress.Size = new Size(200, 25);
            chkSameAddress.CheckedChanged += (s, e) =>
            {
                if (chkSameAddress.Checked)
                {
                    txtAddress2.Text = txtAddress1.Text;
                    txtAddress2.Enabled = false;
                    txtAddress2.BackColor = Color.LightGray;
                }
                else
                {
                    txtAddress2.Text = "";
                    txtAddress2.Enabled = true;
                    txtAddress2.BackColor = Color.White;
                }
            };

            // ===== FLEXIBLE LOCATION FIELDS =====

            // Region (Dropdown)
            System.Windows.Forms.Label lblRegion = new System.Windows.Forms.Label();
            lblRegion.Text = "Region:*";
            lblRegion.Location = new Point(0, 100);
            lblRegion.Size = new Size(120, 25);
            lblRegion.ForeColor = Color.Maroon;

            cmbRegions = new ComboBox();
            cmbRegions.Name = "cmbRegion";
            cmbRegions.Location = new Point(130, 100);
            cmbRegions.Size = new Size(250, 23);
            cmbRegions.DataSource = AddToAddressCMB.Regions;
            cmbRegions.DisplayMember = "Name";
            cmbRegions.ValueMember = "Code";

            // Province (Dropdown)
            System.Windows.Forms.Label lblProvince = new System.Windows.Forms.Label();
            lblProvince.Text = "Province:*";
            lblProvince.Location = new Point(0, 135);
            lblProvince.Size = new Size(120, 25);
            lblProvince.ForeColor = Color.Maroon;

            cmbProvinces = new ComboBox();
            cmbProvinces.Name = "cmbProvince";
            cmbProvinces.Location = new Point(130, 135);
            cmbProvinces.Size = new Size(250, 23);

            // City/Municipality (Dropdown)
            System.Windows.Forms.Label lblCity = new System.Windows.Forms.Label();
            lblCity.Text = "City/Municipality:*";
            lblCity.Location = new Point(0, 170);
            lblCity.Size = new Size(120, 25);
            lblCity.ForeColor = Color.Maroon;

            cmbCities = new ComboBox();
            cmbCities.Name = "cmbCity";
            cmbCities.Location = new Point(130, 170);
            cmbCities.Size = new Size(250, 23);

            // Barangay (Textbox - can type or select from common ones)
            System.Windows.Forms.Label lblBarangay = new System.Windows.Forms.Label();
            lblBarangay.Text = "Barangay:*";
            lblBarangay.Location = new Point(0, 205);
            lblBarangay.Size = new Size(120, 25);
            lblBarangay.ForeColor = Color.Maroon;

            cmbBarangays = new ComboBox();
            cmbBarangays.Name = "cmbBarangays";
            cmbBarangays.Location = new Point(130, 205);
            cmbBarangays.Size = new Size(250, 23);

            // Changed to combo box
            //TextBox txtBarangay = new TextBox();
            //txtBarangay.Name = "txtBarangay";
            //txtBarangay.Location = new Point(130, 205);
            //txtBarangay.Size = new Size(250, 23);
            //txtBarangay.PlaceholderText = "Enter barangay/district";

            // Postal Code
            System.Windows.Forms.Label lblPostal = new System.Windows.Forms.Label();
            lblPostal.Text = "Postal Code:";
            lblPostal.Location = new Point(400, 170);
            lblPostal.Size = new Size(80, 25);

            txtPostal = new TextBox();
            txtPostal.Name = "txtPostal";
            txtPostal.Location = new Point(480, 170);
            txtPostal.Size = new Size(120, 23);
            txtPostal.PlaceholderText = "Postal code";

            addressPanel.Controls.Add(lblAddress1);
            addressPanel.Controls.Add(txtAddress1);
            addressPanel.Controls.Add(lblAddress2);
            addressPanel.Controls.Add(txtAddress2);
            addressPanel.Controls.Add(chkSameAddress);
            addressPanel.Controls.Add(lblRegion);
            addressPanel.Controls.Add(cmbRegions);
            addressPanel.Controls.Add(lblProvince);
            addressPanel.Controls.Add(cmbProvinces);
            addressPanel.Controls.Add(lblCity);
            addressPanel.Controls.Add(cmbCities);
            addressPanel.Controls.Add(lblBarangay);
            addressPanel.Controls.Add(cmbBarangays);
            addressPanel.Controls.Add(lblPostal);
            addressPanel.Controls.Add(txtPostal);
            this.Controls.Add(addressPanel);
        }

        private void PHAddressFields_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            Action<ComboBox> configureAutoComplete = (cb) =>
            {
                cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
            };

            configureAutoComplete(cmbRegions);
            configureAutoComplete(cmbProvinces);
            configureAutoComplete(cmbCities);
            configureAutoComplete(cmbBarangays);

            Action<ComboBox, Action> wireAutoSnapOnLeave = (cb, updateCascadeAction) =>
            {
                cb.Leave += (s, e) =>
                {
                    if (cb.SelectedIndex == -1 && !string.IsNullOrEmpty(cb.Text))
                    {
                        var closestMatch = cb.Items.Cast<LocationItem>()
                            .FirstOrDefault(item => item.Name.StartsWith(cb.Text, StringComparison.OrdinalIgnoreCase));

                        if (closestMatch != null)
                        {
                            cb.SelectedIndex = cb.Items.IndexOf(closestMatch);
                            updateCascadeAction();
                        }
                    }
                };
            };

            Action updateBarangays = () =>
            {
                string selectedCityCode = getSelectedCode(cmbCities);
                if (!string.IsNullOrEmpty(selectedCityCode))
                {
                    var filteredBarangays = AddToAddressCMB.Barangays.Where(b => b.ParentCode == selectedCityCode).ToList();
                    var displayList = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select Barangay ---" } };
                    displayList.AddRange(filteredBarangays);

                    cmbBarangays.BeginUpdate();
                    cmbBarangays.DataSource = displayList;
                    cmbBarangays.DisplayMember = "Name";
                    cmbBarangays.ValueMember = "Code";
                    cmbBarangays.EndUpdate();
                    cmbBarangays.Enabled = filteredBarangays.Count > 0;
                }
                else
                {
                    cmbBarangays.DataSource = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select Barangay ---" } };
                    cmbBarangays.DisplayMember = "Name";
                    cmbBarangays.ValueMember = "Code";
                    cmbBarangays.Enabled = false;
                }
            };

            Action updateCities = () =>
            {
                string selectedProvinceCode = getSelectedCode(cmbProvinces);
                if (!string.IsNullOrEmpty(selectedProvinceCode))
                {
                    var filteredCities = AddToAddressCMB.Cities.Where(c => c.ParentCode == selectedProvinceCode).ToList();
                    var displayList = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select City/Municipality ---" } };
                    displayList.AddRange(filteredCities);

                    cmbCities.BeginUpdate();
                    cmbCities.DataSource = displayList;
                    cmbCities.DisplayMember = "Name";
                    cmbCities.ValueMember = "Code";
                    cmbCities.EndUpdate();
                    cmbCities.Enabled = filteredCities.Count > 0;
                    updateBarangays();
                }
                else
                {
                    cmbCities.DataSource = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select City/Municipality ---" } };
                    cmbCities.DisplayMember = "Name";
                    cmbCities.ValueMember = "Code";
                    cmbCities.Enabled = false;
                    updateBarangays();
                }
            };

            Action updateProvinces = () =>
            {
                string selectedRegionCode = getSelectedCode(cmbRegions);
                if (!string.IsNullOrEmpty(selectedRegionCode))
                {
                    var filteredProvinces = AddToAddressCMB.Provinces.Where(p => p.ParentCode == selectedRegionCode).ToList();
                    var displayList = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select Province ---" } };
                    displayList.AddRange(filteredProvinces);

                    cmbProvinces.BeginUpdate();
                    cmbProvinces.DataSource = displayList;
                    cmbProvinces.DisplayMember = "Name";
                    cmbProvinces.ValueMember = "Code";
                    cmbProvinces.EndUpdate();
                    cmbProvinces.Enabled = filteredProvinces.Count > 0;
                    updateCities();
                }
                else
                {
                    cmbProvinces.DataSource = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select Province ---" } };
                    cmbProvinces.DisplayMember = "Name";
                    cmbProvinces.ValueMember = "Code";
                    cmbProvinces.Enabled = false;
                    updateCities();
                }
            };

            EventHandler regionChangeTrigger = (s, e) => updateProvinces();
            cmbRegions.SelectedIndexChanged += regionChangeTrigger;
            cmbRegions.TextUpdate += regionChangeTrigger;

            EventHandler provinceChangeTrigger = (s, e) => updateCities();
            cmbProvinces.SelectedIndexChanged += provinceChangeTrigger;
            cmbProvinces.TextUpdate += provinceChangeTrigger;

            EventHandler cityChangeTrigger = (s, e) => updateBarangays();
            cmbCities.SelectedIndexChanged += cityChangeTrigger;
            cmbCities.TextUpdate += cityChangeTrigger;

            wireAutoSnapOnLeave(cmbRegions, updateProvinces);
            wireAutoSnapOnLeave(cmbProvinces, updateCities);
            wireAutoSnapOnLeave(cmbCities, updateBarangays);

            var regionsWithPlaceholder = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select Region ---" } };
            regionsWithPlaceholder.AddRange(AddToAddressCMB.Regions);

            cmbRegions.DataSource = regionsWithPlaceholder;
            cmbRegions.DisplayMember = "Name";
            cmbRegions.ValueMember = "Code";

            updateProvinces();
        }

        private string getSelectedCode(ComboBox cb)
        {
            if (cb.SelectedIndex != -1 && cb.SelectedValue is string val) return val;
            var matchedItem = cb.Items.Cast<LocationItem>().FirstOrDefault(item => string.Equals(item.Name, cb.Text, StringComparison.OrdinalIgnoreCase));
            return matchedItem?.Code ?? "";
        }

        private void SetRegistrationFontsTo12pt()
        {
            // Font for labels (Bold)
            Font labelFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            // Font for textboxes and inputs (Regular)
            Font inputFont = new Font("Segoe UI", 12F, FontStyle.Regular);

            // Update all labels in the registration container
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label lbl)
                {
                    lbl.Font = labelFont;
                }
                else if (ctrl is TextBox txt)
                {
                    txt.Font = inputFont;
                }
                else if (ctrl is ComboBox cmb)
                {
                    cmb.Font = inputFont;
                }
                else if (ctrl is MaskedTextBox mtb)
                {
                    mtb.Font = inputFont;
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    dtp.Font = inputFont;
                }
                else if (ctrl is Panel panel && panel.HasChildren)
                {
                    // Recursively update child controls
                    foreach (Control child in panel.Controls)
                    {
                        if (child is Label lblChild) lblChild.Font = labelFont;
                        else if (child is TextBox txtChild) txtChild.Font = inputFont;
                        else if (child is ComboBox cmbChild) cmbChild.Font = inputFont;
                        else if (child is MaskedTextBox mtbChild) mtbChild.Font = inputFont;
                        else if (child is DateTimePicker dtpChild) dtpChild.Font = inputFont;
                    }
                }
            }
        }
        public void ClearAddressFields()
        {
            var txtAddress1 = this.Controls.Find("txtAddress1", true).FirstOrDefault() as TextBox;
            var txtAddress2 = this.Controls.Find("txtAddress2", true).FirstOrDefault() as TextBox;
            var chkSameAddress = this.Controls.Find("chkSameAddress", true).FirstOrDefault() as CheckBox;

            if (txtAddress1 != null) txtAddress1.Clear();
            if (txtAddress2 != null) txtAddress2.Clear();
            if (chkSameAddress != null) chkSameAddress.Checked = false;

            cmbRegions.Text = "";
            cmbProvinces.Text = "";
            cmbCities.Text = "";
            cmbBarangays.Text = "";
            txtPostal.Clear();

            if (cmbRegions.Items.Count > 0)
            {
                cmbRegions.SelectedIndex = 0; // Triggers "--- Select Region ---"
            }
            else
            {
                cmbRegions.SelectedIndex = -1;
            }

            cmbProvinces.DataSource = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select Province ---" } };
            cmbProvinces.Enabled = false;

            cmbCities.DataSource = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select City/Municipality ---" } };
            cmbCities.Enabled = false;

            cmbBarangays.DataSource = new List<LocationItem> { new LocationItem { Code = "", Name = "--- Select Barangay ---" } };
            cmbBarangays.Enabled = false;
        }
    }
}
