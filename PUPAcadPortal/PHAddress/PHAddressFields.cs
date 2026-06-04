using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq; // Required for your LINQ queries (.Where, .FirstOrDefault)
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PHAddress
{
    public partial class PHAddressFields : UserControl
    {
        // Removed the unused FlowLayoutPanel.
        private ComboBox cmbRegions;
        private ComboBox cmbProvinces;
        private ComboBox cmbCities;
        private ComboBox cmbBarangays;
        private TextBox txtPostal;
        CheckBox chkSameAddress = new CheckBox { Text = "Same as Address Line 1", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 0, 3, 15) };
        public TextBox AddressLine1 => (TextBox)this.Controls.Find("txtAddress1", true)[0];
        public TextBox AddressLine2 => (TextBox)this.Controls.Find("txtAddress2", true)[0];
        public CheckBox SameAsAddress1 => chkSameAddress;
        public ComboBox RegionComboBox => cmbRegions;
        public ComboBox ProvinceComboBox => cmbProvinces;
        public ComboBox CityComboBox => cmbCities;
        public ComboBox BarangayComboBox => cmbBarangays;
        public TextBox PostalTextBox => txtPostal;

        // 2. Public Read-Only Properties
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
            this.AutoSize = true;
            InitializeComponentLayout();
            SetRegistrationFontsTo12pt();
        }

        private void InitializeComponentLayout()
        {
            this.Load += PHAddressFields_Load;

            // ===== DYNAMIC LAYOUT: TableLayoutPanel =====
            // This grid automatically adjusts heights when fonts change, preventing overlaps.
            TableLayoutPanel addressPanel = new TableLayoutPanel();
            addressPanel.Name = "addressPanel";
            addressPanel.Dock = DockStyle.Top;
            addressPanel.AutoSize = true;
            addressPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addressPanel.ColumnCount = 4;

            // Grid Columns: [Primary Labels] | [Main Inputs] | [Postal Label] | [Postal Input]
            addressPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            addressPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            addressPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            addressPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            addressPanel.Padding = new Padding(0, 0, 10, 10);

            // Using Object Initializers for cleaner code. AnchorStyles.Left aligns them vertically without hardcoding coordinates.
            Padding marginSpacing = new Padding(3, 8, 3, 8); // Adds vertical breathing room between rows

            // Address Line 1
            Label lblAddress1 = new Label { Text = "Address Line 1:*", AutoSize = true, ForeColor = Color.Maroon, Anchor = AnchorStyles.Left, Margin = marginSpacing };
            TextBox txtAddress1 = new TextBox { Name = "txtAddress1", Width = 500, PlaceholderText = "House/Unit No., Street Name, Subdivision", Anchor = AnchorStyles.Left, Margin = marginSpacing };

            // Address Line 2
            Label lblAddress2 = new Label { Text = "Address Line 2:", AutoSize = true, Anchor = AnchorStyles.Left, Margin = marginSpacing };
            TextBox txtAddress2 = new TextBox { Name = "txtAddress2", Width = 500, PlaceholderText = "Building, Barangay (Optional)", Anchor = AnchorStyles.Left, Margin = marginSpacing, Tag = "optional"};

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

            // Region
            Label lblRegion = new Label { Text = "Region:*", AutoSize = true, ForeColor = Color.Maroon, Anchor = AnchorStyles.Left, Margin = marginSpacing };
            cmbRegions = new ComboBox { Name = "cmbRegion", Width = 250, Anchor = AnchorStyles.Left, Margin = marginSpacing };
            cmbRegions.DataSource = AddToAddressCMB.Regions;
            cmbRegions.DisplayMember = "Name";
            cmbRegions.ValueMember = "Code";

            // Province
            Label lblProvince = new Label { Text = "Province:*", AutoSize = true, ForeColor = Color.Maroon, Anchor = AnchorStyles.Left, Margin = marginSpacing };
            cmbProvinces = new ComboBox { Name = "cmbProvince", Width = 250, Anchor = AnchorStyles.Left, Margin = marginSpacing };

            // City
            Label lblCity = new Label { Text = "City/Municipality:*", AutoSize = true, ForeColor = Color.Maroon, Anchor = AnchorStyles.Left, Margin = marginSpacing };
            cmbCities = new ComboBox { Name = "cmbCity", Width = 250, Anchor = AnchorStyles.Left, Margin = marginSpacing };

            // Postal Code (Shares a row with City)
            Label lblPostal = new Label { Text = "Postal Code:", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(20, 8, 3, 8) };
            txtPostal = new TextBox { Name = "txtPostal", Width = 120, PlaceholderText = "Postal code", Anchor = AnchorStyles.Left, Margin = marginSpacing };

            // Barangay
            Label lblBarangay = new Label { Text = "Barangay:*", AutoSize = true, ForeColor = Color.Maroon, Anchor = AnchorStyles.Left, Margin = marginSpacing };
            cmbBarangays = new ComboBox { Name = "cmbBarangays", Width = 250, Anchor = AnchorStyles.Left, Margin = marginSpacing };

            // ===== ADD CONTROLS TO GRID (Control, Column Index, Row Index) =====

            // Row 0
            addressPanel.Controls.Add(lblAddress1, 0, 0);
            addressPanel.Controls.Add(txtAddress1, 1, 0);
            addressPanel.SetColumnSpan(txtAddress1, 3); // Spans across remaining columns

            // Row 1
            addressPanel.Controls.Add(lblAddress2, 0, 1);
            addressPanel.Controls.Add(txtAddress2, 1, 1);
            addressPanel.SetColumnSpan(txtAddress2, 3);

            // Row 2
            addressPanel.Controls.Add(chkSameAddress, 1, 2);
            addressPanel.SetColumnSpan(chkSameAddress, 3);

            // Row 3
            addressPanel.Controls.Add(lblRegion, 0, 3);
            addressPanel.Controls.Add(cmbRegions, 1, 3);
            addressPanel.SetColumnSpan(cmbRegions, 3);

            // Row 4
            addressPanel.Controls.Add(lblProvince, 0, 4);
            addressPanel.Controls.Add(cmbProvinces, 1, 4);
            addressPanel.SetColumnSpan(cmbProvinces, 3);

            // Row 5 (City & Postal share this row side-by-side)
            addressPanel.Controls.Add(lblCity, 0, 5);
            addressPanel.Controls.Add(cmbCities, 1, 5);
            addressPanel.Controls.Add(lblPostal, 2, 5);
            addressPanel.Controls.Add(txtPostal, 3, 5);

            // Row 6
            addressPanel.Controls.Add(lblBarangay, 0, 6);
            addressPanel.Controls.Add(cmbBarangays, 1, 6);
            addressPanel.SetColumnSpan(cmbBarangays, 3);

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
                cb.Leave += (s, ev) =>
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

            EventHandler regionChangeTrigger = (s, ev) => updateProvinces();
            cmbRegions.SelectedIndexChanged += regionChangeTrigger;
            cmbRegions.TextUpdate += regionChangeTrigger;

            EventHandler provinceChangeTrigger = (s, ev) => updateCities();
            cmbProvinces.SelectedIndexChanged += provinceChangeTrigger;
            cmbProvinces.TextUpdate += provinceChangeTrigger;

            EventHandler cityChangeTrigger = (s, ev) => updateBarangays();
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
            Font labelFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 12F, FontStyle.Regular);

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
                // Because TableLayoutPanel inherits from Panel, this condition still works beautifully
                else if (ctrl is Panel panel && panel.HasChildren)
                {
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
            // The existing .Find(..., true) logic will still easily locate your controls inside the TableLayoutPanel
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
                cmbRegions.SelectedIndex = 0;
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