using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class DashboardContentAdmin : UserControl
    {
        public DashboardContentAdmin()
        {
            InitializeComponent();
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(pnlDashboardRegisterStudent, "Click to register a new student");
            toolTip.SetToolTip(pnlDashboardRegisterProfessor, "Click to register a new professor");
            toolTip.SetToolTip(pnlDashboardViewAllUsers, "Click to view all users");
        }
        private void FixDashboardLayoutCompletely()
        {
            if (!pnlDashboardContent.Visible) return;

            pnlDashboardContent.SuspendLayout();

            try
            {
                int containerWidth = pnlDashboardContent.ClientSize.Width;
                int margin = 40;
                int cardSpacing = 25;

                int availableWidth = containerWidth - (margin * 2);
                int cardWidth = (availableWidth - (cardSpacing * 3)) / 4;

                // === 1. POSITION LABELS ===
                lblAdminDashboard.Location = new Point(margin, 25);
                lblAdminDashboardDesc.Location = new Point(margin, 67);

                // === 2. POSITION STAT CARDS WITH RIGHT-ALIGNED ICONS ===
                int cardsY = 110;
                int cardHeight = 95;

                // Array of stat cards and their picture boxes
                var statCards = new[] {
            new { Card = pnlDashboardTotalStudents, PictureBox = pictureBox15, Label = label72, Value = lblTotalStudents },
            new { Card = pnlDashboardTotalProfs, PictureBox = pictureBox14, Label = label70, Value = lblTotalProfessors },
            new { Card = pnlDashboardTotalCoursess, PictureBox = pictureBox16, Label = label68, Value = lblTotalCourses },
            new { Card = pnlDashboardActiveUsers, PictureBox = pictureBox17, Label = label58, Value = lblActiveUsers }
        };

                for (int i = 0; i < statCards.Length; i++)
                {
                    int cardX = margin + (cardWidth + cardSpacing) * i;
                    var card = statCards[i].Card;

                    // Position the card
                    card.Location = new Point(cardX, cardsY);
                    card.Width = cardWidth;
                    card.Height = cardHeight;

                    // Position PictureBox on the RIGHT side of the card
                    var pb = statCards[i].PictureBox;
                    if (pb != null)
                    {
                        int pbSize = 66;
                        pb.Location = new Point(card.Width - pbSize - 10, (card.Height - pbSize) / 2);
                        pb.Size = new Size(pbSize, pbSize);
                        pb.Visible = true;
                    }

                    // Position labels on the LEFT side
                    var titleLabel = statCards[i].Label;
                    var valueLabel = statCards[i].Value;

                    if (titleLabel != null)
                    {
                        titleLabel.Location = new Point(12, 8);
                        titleLabel.AutoSize = false;
                        titleLabel.Width = card.Width - 90;
                    }

                    if (valueLabel != null)
                    {
                        valueLabel.Location = new Point(12, 22);  // Moved upward (was 30)
                        valueLabel.AutoSize = false;
                        valueLabel.Width = card.Width - 90;
                    }
                }

                // === 3. POSITION QUICK ACTIONS PANEL ===
                int quickActionsY = cardsY + cardHeight + 30;
                pnlDashboardContainerQuickActions.Location = new Point(margin, quickActionsY);
                pnlDashboardContainerQuickActions.Width = availableWidth;

                // === 4. DISTRIBUTE QUICK ACTIONS BUTTONS EQUALLY ===
                var quickActionButtons = new[] {
            new { Panel = pnlDashboardRegisterStudent, Button = btnDashboardRegisterStudent,
                  Title = label79, Desc = label80 },
            new { Panel = pnlDashboardRegisterProfessor, Button = btnDashboardREgisterProfessor,
                  Title = label76, Desc = label82 },
            new { Panel = pnlDashboardViewAllUsers, Button = btnDashboardViewAllUsers,
                  Title = label77, Desc = label78 }
        };

                int quickActionsPanelWidth = availableWidth;
                int quickActionsMargin = 20;  // Margin inside Quick Actions panel
                int quickActionsSpacing = 25; // Space between buttons

                int quickActionsInnerWidth = quickActionsPanelWidth - (quickActionsMargin * 2);
                int buttonWidth = (quickActionsInnerWidth - (quickActionsSpacing * 2)) / 3;
                int buttonHeight = 80;

                for (int i = 0; i < quickActionButtons.Length; i++)
                {
                    int buttonX = quickActionsMargin + (buttonWidth + quickActionsSpacing) * i;
                    int buttonY = 63;  // Y position within the Quick Actions panel

                    var panel = quickActionButtons[i].Panel;
                    var btn = quickActionButtons[i].Button;
                    var titleLabel = quickActionButtons[i].Title;
                    var descLabel = quickActionButtons[i].Desc;

                    if (panel != null)
                    {
                        panel.Location = new Point(buttonX, buttonY);
                        panel.Width = buttonWidth;
                        panel.Height = buttonHeight;
                    }

                    if (btn != null)
                    {
                        btn.Location = new Point(16, 12);
                        btn.Size = new Size(56, 54);
                    }

                    if (titleLabel != null)
                    {
                        titleLabel.Location = new Point(86, 38);  // Moved to bottom (was 13)
                        titleLabel.AutoSize = true;
                    }

                    if (descLabel != null)
                    {
                        descLabel.Location = new Point(86, 13);  // Moved to top (was 38)
                        descLabel.AutoSize = true;
                    }
                }

                // === 5. POSITION RECENT ACTIVITY PANEL ===
                int recentActivityY = quickActionsY + pnlDashboardContainerQuickActions.Height + 30;
                pnlDashboardContainerRecentAct.Location = new Point(margin, recentActivityY);
                pnlDashboardContainerRecentAct.Width = availableWidth;

                // === 6. SETUP SCROLLING ===
                int totalContentHeight = recentActivityY + pnlDashboardContainerRecentAct.Height + margin;
                pnlDashboardContent.AutoScroll = true;
                pnlDashboardContent.AutoScrollMinSize = new Size(0, totalContentHeight);
            }
            finally
            {
                pnlDashboardContent.ResumeLayout();
            }
        }

        private void btnDashboardRegisterStudent_Click(object sender, EventArgs e)
        {

        }

        private void pnlDashboardContent_Resize(object sender, EventArgs e)
        {
            FixDashboardLayoutCompletely();
        }

        private void MakePanelClickable(Panel panel, Action onClickAction)
        {
            panel.Cursor = Cursors.Hand;
            panel.Click += (s, e) => onClickAction();

            // Also make all child controls clickable to trigger the same action
            foreach (Control ctrl in panel.Controls)
            {
                ctrl.Cursor = Cursors.Hand;
                ctrl.Click += (s, e) => onClickAction();
            }
        }

        //private void SetupClickableQuickActions()
        //{
        //    // Register Student Card -> Navigate to Register Student
        //    MakePanelClickable(pnlDashboardRegisterStudent, () =>
        //    {
        //        btnRegisterStudent.PerformClick();
        //    });

        //    // Register Professor Card -> Navigate to Register Professor
        //    MakePanelClickable(pnlDashboardRegisterProfessor, () =>
        //    {
        //        btnRegisterProfessor.PerformClick();
        //    });

        //    // View All Users Card -> Navigate to View All Users (Students tab by default)
        //    MakePanelClickable(pnlDashboardViewAllUsers, () =>
        //    {
        //        viewingStudents = true;
        //        btnViewAllUsers.PerformClick();
        //    });

        //    AddCardHoverEffect(pnlDashboardRegisterStudent);
        //    AddCardHoverEffect(pnlDashboardRegisterProfessor);
        //    AddCardHoverEffect(pnlDashboardViewAllUsers);
        //}

        private void AddCardHoverEffect(Panel card)
        {
            Color originalColor = card.BackColor;
            Color hoverColor = Color.FromArgb(245, 245, 245); // Slightly darker

            card.MouseEnter += (s, e) => card.BackColor = hoverColor;
            card.MouseLeave += (s, e) => card.BackColor = originalColor;

            foreach (Control ctrl in card.Controls)
            {
                ctrl.MouseEnter += (s, e) => card.BackColor = hoverColor;
                ctrl.MouseLeave += (s, e) => card.BackColor = originalColor;
            }
        }

        private void AdminDashboardContent_Load(object sender, EventArgs e)
        {

        }
    }
}
