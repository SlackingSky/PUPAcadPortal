using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Course
{
    public partial class StudentLMSHost : UserControl
    {
        private Control _currentView;

        public StudentLMSHost()
        {
            InitializeComponent();
            ShowDashboard();
        }

        public void ShowDashboard()
        {
            var dash = new StudentActivityDashboard();
            dash.Dock = DockStyle.Fill;

            dash.OnOpenCourse += course =>
            {
                var list = new StudentActivityList(course);
                list.Dock = DockStyle.Fill;

                list.OnBack += ShowDashboard;

                list.OnOpenActivity += activity =>
                {
                    if (activity.Type == "Quiz")
                        ShowQuiz(activity, course, list);
                    else
                        ShowSubmit(activity, course, list);
                };

                SwapView(list);
            };

            SwapView(dash);
        }

        public void ShowCourseDashboard() => ShowDashboard();

        private void ShowSubmit(
            StudentActivityItem activity,
            StudentCourse course,
            StudentActivityList list)
        {
            var submit = new StudentActivitySubmit(activity, course);
            submit.Dock = DockStyle.Fill;

            submit.OnBack += () =>
            {
                var updatedList = new StudentActivityList(course);
                updatedList.Dock = DockStyle.Fill;

                updatedList.OnBack += ShowDashboard;

                updatedList.OnOpenActivity += act =>
                {
                    if (act.Type == "Quiz")
                        ShowQuiz(act, course, updatedList);
                    else
                        ShowSubmit(act, course, updatedList);
                };

                SwapView(updatedList);
            };

            SwapView(submit);
        }

        private void ShowQuiz(
            StudentActivityItem activity,
            StudentCourse course,
            StudentActivityList list)
        {
            var quizPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.Maroon
            };

            var btnBack = new buttonRounded
            {
                Text = "< Back",
                Location = new Point(10, 11),
                Size = new Size(85, 34),
                BackColor = Color.FromArgb(109, 0, 0),
                ForeColor = Color.White,
                BorderRadius = 10,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            var lblTitle = new Label
            {
                Text = activity.Title,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(108, 10),
                Size = new Size(700, 26)
            };

            var lblMeta = new Label
            {
                Text = $"{activity.Points} pts  ·  Due {activity.Deadline:MMM dd, yyyy}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(255, 220, 150),
                Location = new Point(108, 34),
                AutoSize = true
            };

            var btnSubmit = new buttonRounded
            {
                Text = "Submit Quiz",
                Size = new Size(120, 34),
                BackColor = Color.FromArgb(60, 0, 0),
                ForeColor = Color.White,
                BorderRadius = 10,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            pnlHeader.Controls.Add(btnBack);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblMeta);
            pnlHeader.Controls.Add(btnSubmit);

            pnlHeader.SizeChanged += (s, e) =>
            {
                btnSubmit.Location = new Point(
                    pnlHeader.Width - btnSubmit.Width - 14,
                    11);
            };

            var pnlInstrBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White,
                Padding = new Padding(14, 10, 14, 10)
            };

            pnlInstrBar.Controls.Add(new Label
            {
                Text = activity.Instructions,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Black
            });

            var pnlQuestionNav = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10),
                AutoScroll = true,
                WrapContents = false
            };

            var pnlScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(238, 238, 238)
            };

            var flpQuestions = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                AutoScroll = false,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(24, 20, 24, 20),
                BackColor = Color.Transparent
            };

            flpQuestions.Width = pnlScroll.Width;

            pnlScroll.Controls.Add(flpQuestions);

            string[][] sampleQuestions =
            {
                new[]
                {
                    "What is the primary purpose of a simulation in system design?",
                    "To write code faster",
                    "To model system behaviour without building the real system",
                    "To deploy directly to production",
                    "To remove bugs automatically"
                },

                new[]
                {
                    "Which methodology combines multiple development approaches for better outcomes?",
                    "Waterfall only",
                    "Agile only",
                    "Integrated methodology",
                    "Ad-hoc development"
                },

                new[]
                {
                    "In an LMS, what does 'activity submission' refer to?",
                    "Logging in to the portal",
                    "Browsing course materials",
                    "Sending completed work for instructor review",
                    "Creating a new user account"
                }
            };

            var cards = new List<Panel>();

            for (int i = 0; i < sampleQuestions.Length; i++)
            {
                int questionNumber = i + 1;

                var navBtn = new Button
                {
                    Text = questionNumber.ToString(),
                    Width = 40,
                    Height = 40,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.LightGray,
                    ForeColor = Color.Black,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };

                navBtn.FlatAppearance.BorderSize = 0;

                pnlQuestionNav.Controls.Add(navBtn);

                var card = BuildQuizCard(
                    questionNumber,
                    sampleQuestions[i],
                    () =>
                    {
                        navBtn.BackColor = Color.Maroon;
                        navBtn.ForeColor = Color.White;
                    });

                cards.Add(card);

                flpQuestions.Controls.Add(card);

                navBtn.Click += (s, e) =>
                {
                    pnlScroll.AutoScrollPosition =
                        new Point(0, card.Top - 20);
                };
            }

            void ResizeCards()
            {
                int width =
                    pnlScroll.ClientSize.Width
                    - flpQuestions.Padding.Horizontal
                    - SystemInformation.VerticalScrollBarWidth
                    - 30;

                int totalHeight = flpQuestions.Padding.Top;

                foreach (var card in cards)
                {
                    card.Width = width;

                    totalHeight +=
                        card.Height + card.Margin.Bottom;
                }

                totalHeight += flpQuestions.Padding.Bottom;

                flpQuestions.Height = totalHeight;
            }

            pnlScroll.Resize += (s, e) =>
            {
                flpQuestions.Width =
                    pnlScroll.ClientSize.Width
                    - SystemInformation.VerticalScrollBarWidth;

                ResizeCards();
            };

            ResizeCards();

            Action goBack = () =>
            {
                var updatedList = new StudentActivityList(course);
                updatedList.Dock = DockStyle.Fill;

                updatedList.OnBack += ShowDashboard;

                updatedList.OnOpenActivity += act =>
                {
                    if (act.Type == "Quiz")
                        ShowQuiz(act, course, updatedList);
                    else
                        ShowSubmit(act, course, updatedList);
                };

                SwapView(updatedList);
            };

            btnBack.Click += (s, e) => goBack();

            btnSubmit.Click += (s, e) =>
            {
                activity.SubmissionStatus = "Submitted";

                MessageBox.Show(
                    "Quiz submitted successfully!",
                    "Submitted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                goBack();
            };

            quizPanel.Controls.Add(pnlScroll);
            quizPanel.Controls.Add(pnlQuestionNav);
            quizPanel.Controls.Add(pnlInstrBar);
            quizPanel.Controls.Add(pnlHeader);

            SwapView(quizPanel);
        }

        private Panel BuildQuizCard(
            int number,
            string[] lines,
            Action onAnswered)
        {
            const int CHOICE_H = 46;
            const int BADGE_W = 52;
            const int TOP_PAD = 16;
            const int Q_NUM_H = 24;
            const int Q_GAP = 8;
            const int Q_TEXT_H = 56;

            const int CHOICES_Y =
                TOP_PAD + Q_NUM_H + Q_GAP + Q_TEXT_H + 12;

            int totalH =
                CHOICES_Y + 4 * CHOICE_H + 16;

            var outer = new Panel
            {
                Width = 900,
                Height = totalH + 6,
                BackColor = Color.Maroon,
                Margin = new Padding(0, 0, 0, 18)
            };

            var inner = new Panel
            {
                BackColor = Color.White,
                Location = new Point(0, 6),
                Anchor = AnchorStyles.Top
                       | AnchorStyles.Left
                       | AnchorStyles.Right
                       | AnchorStyles.Bottom
            };

            outer.SizeChanged += (s, e) =>
            {
                inner.Size = new Size(
                    outer.Width,
                    outer.Height - 6);
            };

            inner.Size = new Size(
                outer.Width,
                outer.Height - 6);

            var lblQNum = new Label
            {
                Text = $"Question {number}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(14, TOP_PAD),
                Size = new Size(200, Q_NUM_H),
                ForeColor = Color.Black
            };

            var pnlQBar = new Panel
            {
                BackColor = Color.Maroon,
                Location = new Point(
                    14,
                    TOP_PAD + Q_NUM_H + Q_GAP),
                Height = Q_TEXT_H
            };

            inner.SizeChanged += (s, e) =>
            {
                pnlQBar.Width = inner.Width - 28;
            };

            var txtQ = new TextBox
            {
                Text = lines[0],
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Maroon,
                Multiline = true,
                ReadOnly = true,
                Location = new Point(10, 8)
            };

            pnlQBar.SizeChanged += (s, e) =>
            {
                txtQ.Size = new Size(
                    pnlQBar.Width - 20,
                    pnlQBar.Height - 12);
            };

            pnlQBar.Controls.Add(txtQ);

            string[] letters = { "A", "B", "C", "D" };

            RadioButton selectedRadio = null;
            bool answered = false;

            for (int i = 0; i < 4; i++)
            {
                int y = CHOICES_Y + (i * CHOICE_H);

                var pnlBadge = new Panel
                {
                    BackColor = Color.Maroon,
                    Location = new Point(14, y),
                    Size = new Size(BADGE_W, CHOICE_H - 4)
                };

                var pnlBadgeInner = new Panel
                {
                    BackColor = Color.White,
                    Location = new Point(4, 4),
                    Size = new Size(BADGE_W - 8, CHOICE_H - 12)
                };

                pnlBadgeInner.Controls.Add(new Label
                {
                    Text = letters[i],
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.Maroon,
                    TextAlign = ContentAlignment.MiddleCenter
                });

                pnlBadge.Controls.Add(pnlBadgeInner);

                var pnlChoice = new Panel
                {
                    BackColor = Color.Maroon,
                    Location = new Point(14 + BADGE_W + 4, y),
                    Height = CHOICE_H - 4
                };

                inner.SizeChanged += (s, e) =>
                {
                    pnlChoice.Width =
                        inner.Width - 14 - BADGE_W - 18;
                };

                var rb = new RadioButton
                {
                    Location = new Point(8, 10),
                    Size = new Size(20, 20),
                    BackColor = Color.Maroon
                };

                string choiceText =
                    i + 1 < lines.Length
                    ? lines[i + 1]
                    : "";

                var lblChoice = new Label
                {
                    Text = choiceText,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.White,
                    Location = new Point(34, 0),
                    Height = CHOICE_H - 4,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                pnlChoice.SizeChanged += (s, e) =>
                {
                    lblChoice.Width =
                        pnlChoice.Width - 42;
                };

                rb.CheckedChanged += (s, e) =>
                {
                    if (rb.Checked)
                    {
                        if (selectedRadio != null &&
                            selectedRadio != rb)
                        {
                            selectedRadio.Checked = false;
                        }

                        selectedRadio = rb;

                        pnlChoice.BackColor =
                            Color.FromArgb(140, 0, 0);

                        lblChoice.ForeColor =
                            Color.FromArgb(255, 220, 150);

                        if (!answered)
                        {
                            answered = true;
                            onAnswered?.Invoke();
                        }
                    }
                    else
                    {
                        pnlChoice.BackColor = Color.Maroon;
                        lblChoice.ForeColor = Color.White;
                    }
                };

                pnlChoice.Click += (s, e) =>
                {
                    rb.Checked = true;
                };

                lblChoice.Click += (s, e) =>
                {
                    rb.Checked = true;
                };

                pnlChoice.Controls.Add(rb);
                pnlChoice.Controls.Add(lblChoice);

                inner.Controls.Add(pnlBadge);
                inner.Controls.Add(pnlChoice);
            }

            inner.Controls.Add(lblQNum);
            inner.Controls.Add(pnlQBar);

            outer.Controls.Add(inner);

            return outer;
        }

        private void SwapView(Control next)
        {
            SuspendLayout();

            if (_currentView != null)
            {
                Controls.Remove(_currentView);
                _currentView.Dispose();
            }

            _currentView = next;

            Controls.Add(next);

            next.BringToFront();

            ResumeLayout(true);
        }
    }
}