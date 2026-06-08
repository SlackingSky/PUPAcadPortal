using PUPAcadPortal.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PUPAcadPortal.Services;
using PUPAcadPortal.ReusableUserControls;

namespace PUPAcadPortal.PortalContents.Admin.Enrollment
{
    public partial class DashboardContentAdmin : UserControl
    {
        private DashboardService _dashboardService = new();

        public DashboardContentAdmin()
        {
            InitializeComponent();
            DashboardService adminDashboardService = new();
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

        private async void AdminDashboardContent_Load(object sender, EventArgs e)
        {
            pnlDashboardRegisterProfessor.BindClick(); 
            pnlDashboardRegisterStudent.BindClick(); 
            pnlDashboardViewAllUsers.BindClick();

            await LoadAdminDashboardAsync();
            FixDashboardLayoutCompletely();
        }

        private async Task LoadAdminDashboardAsync()
        {
            using (var context = new Models.AppDbContext())
            {
                // Count all students in user table
                int totalStudents = await context.Users
                    .Where(u => u.Students.Any())
                    .CountAsync();

                // Count all professors in user table
                int totalProfessors = await context.Users
                    .Where(u => u.Professors.Any())
                    .CountAsync();

                // Count all courses in the system
                int totalCourses = await context.Subjects
                    .CountAsync();

                // Count all active users in the system
                int activeUsers = await context.Users
                    .Where(u => u.IsActive == true)
                    .CountAsync();

                var recentActivities = await _dashboardService.GetActivityLogsAsync(Data.UserSession.Role ?? "");

                // --- UI CARD UPDATES ---
                // Map the computed numerical numbers cleanly into your structural layout tracking label elements
                lblTotalStudents.Text = totalStudents.ToString();
                lblTotalProfessors.Text = totalProfessors.ToString();
                lblTotalCourses.Text = totalCourses.ToString();
                lblActiveUsers.Text = activeUsers.ToString();

                foreach (var activity in recentActivities)
                {
                    var actPnl = new AdminRecentActivityReusable
                    {
                        ActivityTitle = activity.Action,
                        ActivityTimeAgo = activity.Timestamp.ToString(),
                        CreatedBy = $"{activity.User.LastName} {activity.User.FirstName}"
                    };
                    //actPnl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    fpnlRecentAct.Controls.Add(actPnl);
                }
            }
        }


    }
}