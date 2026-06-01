using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PUPAcadPortal.PortalContents.Student.LMS.Grades
{
    public partial class GradesPanel : UserControl
    {

        private struct GradeEntry
        {
            public int No;
            public string SubjectCode;
            public string SubjectTitle;
            public int Units;
            public double Grade;
            public double Equivalent;
            public string Remarks;
            // Grade breakdown components
            public double Activities;
            public double Quizzes;
            public double LongQuizzes;
            public double Attendance;
            public double MajorAssessments;
            // Scholastic status & instructor feedback
            public string ScholasticStatus;   // "Regular" | "Irregular"  (PUPSIS enrollment type per subject)
            public string InstructorFeedback;
        }

        //  STATIC SAMPLE DATA  – AY 2025-2026

        private readonly List<GradeEntry> _midterm = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 009",   SubjectTitle="Object Oriented Programming",                       Units=3,Grade=92,Equivalent=1.25,Remarks="PASSED",Activities=93,Quizzes=91,LongQuizzes=90,Attendance=95,MajorAssessments=92,ScholasticStatus="Regular",InstructorFeedback="Excellent performance. Shows strong grasp of OOP concepts."},
            new GradeEntry{No=2,SubjectCode="COMP 010",   SubjectTitle="Information Management",                            Units=3,Grade=88,Equivalent=1.50,Remarks="PASSED",Activities=87,Quizzes=88,LongQuizzes=89,Attendance=90,MajorAssessments=87,ScholasticStatus="Regular",InstructorFeedback="Good analytical skills in database design."},
            new GradeEntry{No=4,SubjectCode="COMP 013",   SubjectTitle="Human Computer Interaction",                       Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED",Activities=91,Quizzes=89,LongQuizzes=90,Attendance=92,MajorAssessments=91,ScholasticStatus="Regular",InstructorFeedback="Creative UI designs. Keep up the excellent work."},
            new GradeEntry{No=5,SubjectCode="COMP 014",   SubjectTitle="Quantitative Methods with Modeling and Simulation", Units=3,Grade=78,Equivalent=2.25,Remarks="PASSED",Activities=76,Quizzes=78,LongQuizzes=77,Attendance=82,MajorAssessments=79,ScholasticStatus="Regular",InstructorFeedback="Needs improvement on simulation modelling."},
            new GradeEntry{No=6,SubjectCode="ELEC IT-FE2",SubjectTitle="BSIT Free Elective 2",                              Units=3,Grade=83,Equivalent=1.75,Remarks="PASSED",Activities=82,Quizzes=83,LongQuizzes=84,Attendance=85,MajorAssessments=82,ScholasticStatus="Regular",InstructorFeedback="Consistent performance throughout the term."},
            new GradeEntry{No=7,SubjectCode="INTE 202",   SubjectTitle="Interactive Programming and Technologies 1",       Units=3,Grade=95,Equivalent=1.00,Remarks="PASSED",Activities=96,Quizzes=95,LongQuizzes=94,Attendance=97,MajorAssessments=95,ScholasticStatus="Regular",InstructorFeedback="Outstanding! Top performer in the class."},
            new GradeEntry{No=8,SubjectCode="PATHFIT 4",  SubjectTitle="Physical Activity Towards Health and Fitness 4",   Units=2,Grade=88,Equivalent=1.50,Remarks="PASSED",Activities=87,Quizzes=89,LongQuizzes=88,Attendance=90,MajorAssessments=88,ScholasticStatus="Regular",InstructorFeedback="Active participation in all fitness activities."},
        };

        private readonly List<GradeEntry> _final = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 009",   SubjectTitle="Object Oriented Programming",                       Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED",Activities=90,Quizzes=91,LongQuizzes=92,Attendance=94,MajorAssessments=91,ScholasticStatus="Regular",InstructorFeedback="Maintained excellent standing throughout the semester."},
            new GradeEntry{No=2,SubjectCode="COMP 010",   SubjectTitle="Information Management",                            Units=3,Grade=86,Equivalent=1.75,Remarks="PASSED",Activities=85,Quizzes=87,LongQuizzes=85,Attendance=88,MajorAssessments=86,ScholasticStatus="Regular",InstructorFeedback="Good work on the final project."},
            new GradeEntry{No=3,SubjectCode="COMP 012",   SubjectTitle="Network Administration",                            Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED",Activities=83,Quizzes=84,LongQuizzes=82,Attendance=87,MajorAssessments=85,ScholasticStatus="Regular",InstructorFeedback="Improved lab scores in the finals period."},
            new GradeEntry{No=4,SubjectCode="COMP 013",   SubjectTitle="Human Computer Interaction",                       Units=3,Grade=89,Equivalent=1.50,Remarks="PASSED",Activities=89,Quizzes=88,LongQuizzes=90,Attendance=91,MajorAssessments=89,ScholasticStatus="Regular",InstructorFeedback="Final prototype was impressive and well-documented."},
            new GradeEntry{No=5,SubjectCode="COMP 014",   SubjectTitle="Quantitative Methods with Modeling and Simulation", Units=3,Grade=76,Equivalent=2.25,Remarks="PASSED",Activities=75,Quizzes=76,LongQuizzes=76,Attendance=80,MajorAssessments=77,ScholasticStatus="Regular",InstructorFeedback="Passed but should review probability concepts."},
            new GradeEntry{No=6,SubjectCode="ELEC IT-FE2",SubjectTitle="BSIT Free Elective 2",                              Units=3,Grade=81,Equivalent=2.00,Remarks="PASSED",Activities=80,Quizzes=82,LongQuizzes=80,Attendance=83,MajorAssessments=81,ScholasticStatus="Regular",InstructorFeedback="Decent output on elective project."},
            new GradeEntry{No=7,SubjectCode="INTE 202",   SubjectTitle="Interactive Programming and Technologies 1",       Units=3,Grade=93,Equivalent=1.00,Remarks="PASSED",Activities=94,Quizzes=93,LongQuizzes=92,Attendance=96,MajorAssessments=93,ScholasticStatus="Regular",InstructorFeedback="Consistently top-tier work. Excellent attitude."},
            new GradeEntry{No=8,SubjectCode="PATHFIT 4",  SubjectTitle="Physical Activity Towards Health and Fitness 4",   Units=2,Grade=85,Equivalent=1.75,Remarks="PASSED",Activities=84,Quizzes=86,LongQuizzes=85,Attendance=88,MajorAssessments=85,ScholasticStatus="Regular",InstructorFeedback="Good fitness assessment performance."},
        };

        private readonly List<GradeEntry> _midterm2 = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 015",   SubjectTitle="Software Engineering",                               Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED",Activities=91,Quizzes=89,LongQuizzes=90,Attendance=93,MajorAssessments=90,ScholasticStatus="Regular",InstructorFeedback="Well-structured SDLC documentation."},
            new GradeEntry{No=2,SubjectCode="COMP 016",   SubjectTitle="Systems Analysis and Design",                       Units=3,Grade=87,Equivalent=1.50,Remarks="PASSED",Activities=86,Quizzes=87,LongQuizzes=88,Attendance=90,MajorAssessments=87,ScholasticStatus="Regular",InstructorFeedback="Clear system diagrams and models."},
            new GradeEntry{No=3,SubjectCode="COMP 017",   SubjectTitle="Web Systems and Technologies",                      Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED",Activities=83,Quizzes=84,LongQuizzes=85,Attendance=86,MajorAssessments=84,ScholasticStatus="Regular",InstructorFeedback="Web project shows good frontend skills."},
            new GradeEntry{No=4,SubjectCode="COMP 018",   SubjectTitle="Technopreneurship",                                 Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED",Activities=92,Quizzes=90,LongQuizzes=91,Attendance=94,MajorAssessments=91,ScholasticStatus="Regular",InstructorFeedback="Innovative business plan. Excellent presentation."},
            new GradeEntry{No=5,SubjectCode="COMP 019",   SubjectTitle="Mobile Application Development",                    Units=3,Grade=79,Equivalent=2.25,Remarks="PASSED",Activities=78,Quizzes=79,LongQuizzes=78,Attendance=82,MajorAssessments=80,ScholasticStatus="Regular",InstructorFeedback="Mobile app needs more polishing."},
            new GradeEntry{No=6,SubjectCode="ELEC IT-FE3",SubjectTitle="BSIT Free Elective 3",                              Units=3,Grade=82,Equivalent=2.00,Remarks="PASSED",Activities=81,Quizzes=82,LongQuizzes=83,Attendance=84,MajorAssessments=82,ScholasticStatus="Regular",InstructorFeedback="Satisfactory elective performance."},
            new GradeEntry{No=7,SubjectCode="INTE 203",   SubjectTitle="Interactive Programming and Technologies 2",        Units=3,Grade=94,Equivalent=1.00,Remarks="PASSED",Activities=95,Quizzes=94,LongQuizzes=93,Attendance=96,MajorAssessments=94,ScholasticStatus="Regular",InstructorFeedback="Best in class for interactive projects."},
            new GradeEntry{No=8,SubjectCode="PATHFIT 5",  SubjectTitle="Physical Activity Towards Health and Fitness 5",    Units=2,Grade=86,Equivalent=1.75,Remarks="PASSED",Activities=85,Quizzes=87,LongQuizzes=86,Attendance=89,MajorAssessments=86,ScholasticStatus="Regular",InstructorFeedback="Good improvement in fitness metrics."},
        };

        private readonly List<GradeEntry> _final2 = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 015",   SubjectTitle="Software Engineering",                               Units=3,Grade=88,Equivalent=1.50,Remarks="PASSED",Activities=87,Quizzes=88,LongQuizzes=89,Attendance=91,MajorAssessments=88,ScholasticStatus="Regular",InstructorFeedback="Capstone-worthy design documentation."},
            new GradeEntry{No=2,SubjectCode="COMP 016",   SubjectTitle="Systems Analysis and Design",                       Units=3,Grade=85,Equivalent=1.75,Remarks="PASSED",Activities=84,Quizzes=86,LongQuizzes=84,Attendance=87,MajorAssessments=85,ScholasticStatus="Regular",InstructorFeedback="DFDs and ERDs are well-drawn."},
            new GradeEntry{No=3,SubjectCode="COMP 017",   SubjectTitle="Web Systems and Technologies",                      Units=3,Grade=83,Equivalent=1.75,Remarks="PASSED",Activities=82,Quizzes=83,LongQuizzes=84,Attendance=85,MajorAssessments=83,ScholasticStatus="Regular",InstructorFeedback="Final web app is functional and clean."},
            new GradeEntry{No=4,SubjectCode="COMP 018",   SubjectTitle="Technopreneurship",                                 Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED",Activities=91,Quizzes=89,LongQuizzes=90,Attendance=93,MajorAssessments=90,ScholasticStatus="Regular",InstructorFeedback="Pitch deck was well-received by panel."},
            new GradeEntry{No=5,SubjectCode="COMP 019",   SubjectTitle="Mobile Application Development",                    Units=3,Grade=77,Equivalent=2.25,Remarks="PASSED",Activities=76,Quizzes=77,LongQuizzes=77,Attendance=80,MajorAssessments=78,ScholasticStatus="Regular",InstructorFeedback="App works but UI needs refinement."},
            new GradeEntry{No=6,SubjectCode="ELEC IT-FE3",SubjectTitle="BSIT Free Elective 3",                              Units=3,Grade=80,Equivalent=2.00,Remarks="PASSED",Activities=79,Quizzes=80,LongQuizzes=81,Attendance=82,MajorAssessments=80,ScholasticStatus="Regular",InstructorFeedback="Adequate completion of elective requirements."},
            new GradeEntry{No=7,SubjectCode="INTE 203",   SubjectTitle="Interactive Programming and Technologies 2",        Units=3,Grade=92,Equivalent=1.00,Remarks="PASSED",Activities=93,Quizzes=92,LongQuizzes=91,Attendance=95,MajorAssessments=92,ScholasticStatus="Regular",InstructorFeedback="Outstanding final interactive portfolio."},
            new GradeEntry{No=8,SubjectCode="PATHFIT 5",  SubjectTitle="Physical Activity Towards Health and Fitness 5",    Units=2,Grade=84,Equivalent=1.75,Remarks="PASSED",Activities=83,Quizzes=85,LongQuizzes=84,Attendance=87,MajorAssessments=84,ScholasticStatus="Regular",InstructorFeedback="Consistent PE performance."},
        };

        // AY 2024-2025  1st Semester
        private readonly List<GradeEntry> _ay2425_sem1_mid = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 005",   SubjectTitle="Data Structures and Algorithms",                     Units=3,Grade=89,Equivalent=1.50,Remarks="PASSED",Activities=88,Quizzes=90,LongQuizzes=88,Attendance=91,MajorAssessments=89,ScholasticStatus="Regular",InstructorFeedback="Solid algorithmic thinking."},
            new GradeEntry{No=2,SubjectCode="COMP 006",   SubjectTitle="Operating Systems",                                  Units=3,Grade=85,Equivalent=1.75,Remarks="PASSED",Activities=84,Quizzes=85,LongQuizzes=84,Attendance=88,MajorAssessments=86,ScholasticStatus="Regular",InstructorFeedback="Good grasp of process management."},
            new GradeEntry{No=3,SubjectCode="COMP 007",   SubjectTitle="Computer Organization and Architecture",             Units=3,Grade=82,Equivalent=2.00,Remarks="PASSED",Activities=81,Quizzes=82,LongQuizzes=81,Attendance=85,MajorAssessments=83,ScholasticStatus="Regular",InstructorFeedback="Needs more review on CPU pipelines."},
            new GradeEntry{No=4,SubjectCode="COMP 008",   SubjectTitle="Discrete Mathematics",                               Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED",Activities=92,Quizzes=90,LongQuizzes=91,Attendance=93,MajorAssessments=91,ScholasticStatus="Regular",InstructorFeedback="Excellent proof-writing skills."},
            new GradeEntry{No=5,SubjectCode="GEED 006",   SubjectTitle="Ethics",                                             Units=3,Grade=87,Equivalent=1.50,Remarks="PASSED",Activities=86,Quizzes=88,LongQuizzes=87,Attendance=89,MajorAssessments=87,ScholasticStatus="Regular",InstructorFeedback="Thoughtful case analysis outputs."},
            new GradeEntry{No=6,SubjectCode="GEED 007",   SubjectTitle="Science, Technology and Society",                   Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED",Activities=83,Quizzes=84,LongQuizzes=84,Attendance=86,MajorAssessments=84,ScholasticStatus="Regular",InstructorFeedback="Good participation in discussions."},
            new GradeEntry{No=7,SubjectCode="ELEC IT-FE1",SubjectTitle="BSIT Free Elective 1",                               Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED",Activities=89,Quizzes=90,LongQuizzes=91,Attendance=92,MajorAssessments=90,ScholasticStatus="Regular",InstructorFeedback="Excellent elective project output."},
            new GradeEntry{No=8,SubjectCode="PATHFIT 3",  SubjectTitle="Physical Activity Towards Health and Fitness 3",     Units=2,Grade=86,Equivalent=1.75,Remarks="PASSED",Activities=85,Quizzes=87,LongQuizzes=86,Attendance=88,MajorAssessments=86,ScholasticStatus="Regular",InstructorFeedback="Good sport skills demonstration."},
        };

        private readonly List<GradeEntry> _ay2425_sem1_final = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 005",   SubjectTitle="Data Structures and Algorithms",                     Units=3,Grade=87,Equivalent=1.50,Remarks="PASSED",Activities=86,Quizzes=88,LongQuizzes=86,Attendance=89,MajorAssessments=87,ScholasticStatus="Regular",InstructorFeedback="Final project was well-optimised."},
            new GradeEntry{No=2,SubjectCode="COMP 006",   SubjectTitle="Operating Systems",                                  Units=3,Grade=83,Equivalent=1.75,Remarks="PASSED",Activities=82,Quizzes=84,LongQuizzes=82,Attendance=86,MajorAssessments=84,ScholasticStatus="Regular",InstructorFeedback="Improved memory management concepts."},
            new GradeEntry{No=3,SubjectCode="COMP 007",   SubjectTitle="Computer Organization and Architecture",             Units=3,Grade=80,Equivalent=2.00,Remarks="PASSED",Activities=79,Quizzes=80,LongQuizzes=80,Attendance=83,MajorAssessments=81,ScholasticStatus="Regular",InstructorFeedback="Passed. Review assembly language basics."},
            new GradeEntry{No=4,SubjectCode="COMP 008",   SubjectTitle="Discrete Mathematics",                               Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED",Activities=91,Quizzes=89,LongQuizzes=90,Attendance=92,MajorAssessments=90,ScholasticStatus="Regular",InstructorFeedback="Consistent high performance in math."},
            new GradeEntry{No=5,SubjectCode="GEED 006",   SubjectTitle="Ethics",                                             Units=3,Grade=85,Equivalent=1.75,Remarks="PASSED",Activities=84,Quizzes=86,LongQuizzes=85,Attendance=87,MajorAssessments=85,ScholasticStatus="Regular",InstructorFeedback="Well-argued final essay."},
            new GradeEntry{No=6,SubjectCode="GEED 007",   SubjectTitle="Science, Technology and Society",                   Units=3,Grade=82,Equivalent=2.00,Remarks="PASSED",Activities=81,Quizzes=83,LongQuizzes=82,Attendance=84,MajorAssessments=82,ScholasticStatus="Regular",InstructorFeedback="Adequate final research paper."},
            new GradeEntry{No=7,SubjectCode="ELEC IT-FE1",SubjectTitle="BSIT Free Elective 1",                               Units=3,Grade=88,Equivalent=1.50,Remarks="PASSED",Activities=87,Quizzes=89,LongQuizzes=88,Attendance=90,MajorAssessments=88,ScholasticStatus="Regular",InstructorFeedback="Great final elective presentation."},
            new GradeEntry{No=8,SubjectCode="PATHFIT 3",  SubjectTitle="Physical Activity Towards Health and Fitness 3",     Units=2,Grade=84,Equivalent=1.75,Remarks="PASSED",Activities=83,Quizzes=85,LongQuizzes=84,Attendance=86,MajorAssessments=84,ScholasticStatus="Regular",InstructorFeedback="Good end-of-term fitness test results."},
        };

        // AY 2024-2025  2nd Semester
        private readonly List<GradeEntry> _ay2425_sem2_mid = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 009",   SubjectTitle="Object Oriented Programming",                       Units=3,Grade=88,Equivalent=1.50,Remarks="PASSED",Activities=87,Quizzes=88,LongQuizzes=89,Attendance=91,MajorAssessments=88,ScholasticStatus="Regular",InstructorFeedback="Solid OOP implementation."},
            new GradeEntry{No=2,SubjectCode="COMP 010",   SubjectTitle="Information Management",                            Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED",Activities=83,Quizzes=85,LongQuizzes=83,Attendance=86,MajorAssessments=84,ScholasticStatus="Regular",InstructorFeedback="Database normalization is well understood."},
            new GradeEntry{No=3,SubjectCode="COMP 011",   SubjectTitle="Social Issues and Professional Practice",           Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED",Activities=92,Quizzes=90,LongQuizzes=91,Attendance=93,MajorAssessments=91,ScholasticStatus="Regular",InstructorFeedback="Excellent reflection papers."},
            new GradeEntry{No=4,SubjectCode="GEED 008",   SubjectTitle="Art Appreciation",                                  Units=3,Grade=93,Equivalent=1.00,Remarks="PASSED",Activities=94,Quizzes=92,LongQuizzes=93,Attendance=95,MajorAssessments=93,ScholasticStatus="Regular",InstructorFeedback="Outstanding artistic critique work."},
            new GradeEntry{No=5,SubjectCode="GEED 009",   SubjectTitle="The Contemporary World",                            Units=3,Grade=86,Equivalent=1.75,Remarks="PASSED",Activities=85,Quizzes=87,LongQuizzes=86,Attendance=88,MajorAssessments=86,ScholasticStatus="Regular",InstructorFeedback="Well-researched global issues report."},
            new GradeEntry{No=6,SubjectCode="MATH 003",   SubjectTitle="Numerical Methods",                                 Units=3,Grade=79,Equivalent=2.25,Remarks="PASSED",Activities=78,Quizzes=79,LongQuizzes=78,Attendance=82,MajorAssessments=80,ScholasticStatus="Regular",InstructorFeedback="Needs more practice with iterative methods."},
            new GradeEntry{No=7,SubjectCode="INTE 201",   SubjectTitle="Integrative Programming and Technologies",          Units=3,Grade=92,Equivalent=1.00,Remarks="PASSED",Activities=93,Quizzes=92,LongQuizzes=91,Attendance=94,MajorAssessments=92,ScholasticStatus="Regular",InstructorFeedback="Integrative project is outstanding."},
            new GradeEntry{No=8,SubjectCode="PATHFIT 4",  SubjectTitle="Physical Activity Towards Health and Fitness 4",   Units=2,Grade=87,Equivalent=1.50,Remarks="PASSED",Activities=86,Quizzes=88,LongQuizzes=87,Attendance=89,MajorAssessments=87,ScholasticStatus="Regular",InstructorFeedback="Excellent endurance test scores."},
        };

        private readonly List<GradeEntry> _ay2425_sem2_final = new List<GradeEntry>
        {
            new GradeEntry{No=1,SubjectCode="COMP 009",   SubjectTitle="Object Oriented Programming",                       Units=3,Grade=86,Equivalent=1.75,Remarks="PASSED",Activities=85,Quizzes=87,LongQuizzes=85,Attendance=88,MajorAssessments=86,ScholasticStatus="Regular",InstructorFeedback="Final OOP project is well-structured."},
            new GradeEntry{No=2,SubjectCode="COMP 010",   SubjectTitle="Information Management",                            Units=3,Grade=82,Equivalent=2.00,Remarks="PASSED",Activities=81,Quizzes=83,LongQuizzes=82,Attendance=84,MajorAssessments=82,ScholasticStatus="Regular",InstructorFeedback="Good query optimisation in final output."},
            new GradeEntry{No=3,SubjectCode="COMP 011",   SubjectTitle="Social Issues and Professional Practice",           Units=3,Grade=89,Equivalent=1.50,Remarks="PASSED",Activities=90,Quizzes=88,LongQuizzes=89,Attendance=91,MajorAssessments=89,ScholasticStatus="Regular",InstructorFeedback="Thoughtful professional ethics research."},
            new GradeEntry{No=4,SubjectCode="GEED 008",   SubjectTitle="Art Appreciation",                                  Units=3,Grade=91,Equivalent=1.25,Remarks="PASSED",Activities=92,Quizzes=90,LongQuizzes=91,Attendance=93,MajorAssessments=91,ScholasticStatus="Regular",InstructorFeedback="Final art analysis was creative and insightful."},
            new GradeEntry{No=5,SubjectCode="GEED 009",   SubjectTitle="The Contemporary World",                            Units=3,Grade=84,Equivalent=1.75,Remarks="PASSED",Activities=83,Quizzes=85,LongQuizzes=84,Attendance=86,MajorAssessments=84,ScholasticStatus="Regular",InstructorFeedback="Good final essay on globalisation."},
            new GradeEntry{No=6,SubjectCode="MATH 003",   SubjectTitle="Numerical Methods",                                 Units=3,Grade=76,Equivalent=2.25,Remarks="PASSED",Activities=75,Quizzes=76,LongQuizzes=76,Attendance=80,MajorAssessments=77,ScholasticStatus="Regular",InstructorFeedback="Passed. Focus on Newton-Raphson convergence."},
            new GradeEntry{No=7,SubjectCode="INTE 201",   SubjectTitle="Integrative Programming and Technologies",          Units=3,Grade=90,Equivalent=1.25,Remarks="PASSED",Activities=91,Quizzes=90,LongQuizzes=89,Attendance=93,MajorAssessments=90,ScholasticStatus="Regular",InstructorFeedback="Very good final integrative system."},
            new GradeEntry{No=8,SubjectCode="PATHFIT 4",  SubjectTitle="Physical Activity Towards Health and Fitness 4",   Units=2,Grade=85,Equivalent=1.75,Remarks="PASSED",Activities=84,Quizzes=86,LongQuizzes=85,Attendance=87,MajorAssessments=85,ScholasticStatus="Regular",InstructorFeedback="Consistent athletic performance."},
        };

        // Grade Scale Reference
        private readonly string[,] _scale =
        {
            {"1.00","97–100","Excellent"},   {"1.25","94–96","Excellent"},    {"1.50","91–93","Very Good"},
            {"1.75","88–90","Very Good"},    {"2.00","85–87","Good"},         {"2.25","82–84","Good"},
            {"2.50","79–81","Satisfactory"}, {"2.75","76–78","Satisfactory"}, {"3.00","75","Passing"},
            {"4.00","68–74","Conditional"},  {"5.00","Below 68","Failed"},    {"Inc.","–","Incomplete"},
            {"W","–","Withdrawal"},          {"P","–","Passed (Non-credit)"}, {"","",""}
        };

        //  RUNTIME STATE

        private bool _isMidterm = true;
        private GradeEntry? _detailSubject = null;      // currently expanded row
        private readonly List<string> _notes = new List<string>();

        private DataTable _dtMid, _dtFinal, _dtMid2, _dtFinal2;
        private DataTable _dt2425s1Mid, _dt2425s1Final, _dt2425s2Mid, _dt2425s2Final;

        //  CONSTRUCTOR

        public GradesPanel()
        {
            InitializeComponent();

            cmbSemester.SelectedIndex = 0;
            cmbAcYear.SelectedIndex = 1;   // default: 2025-2026

            BuildDataTables();
            BindGrids();
            PopulateGradeScale();

            // Wire events AFTER initial binding
            cmbSemester.SelectedIndexChanged += CmbFilterChanged;
            cmbAcYear.SelectedIndexChanged += CmbFilterChanged;

            dgvMid.CellFormatting += DgGrades_CellFormatting;
            dgvFinal.CellFormatting += DgGrades_CellFormatting;
            dgvMid.DataBindingComplete += DgGrades_DataBindingComplete;
            dgvFinal.DataBindingComplete += DgGrades_DataBindingComplete;
            dgvMid.CellClick += DgGrades_CellClick;
            dgvFinal.CellClick += DgGrades_CellClick;

            RefreshAll();
        }

        //  DATA / BINDING

        private void BuildDataTables()
        {
            _dtMid = CreateDT(_midterm);
            _dtFinal = CreateDT(_final);
            _dtMid2 = CreateDT(_midterm2);
            _dtFinal2 = CreateDT(_final2);
            _dt2425s1Mid = CreateDT(_ay2425_sem1_mid);
            _dt2425s1Final = CreateDT(_ay2425_sem1_final);
            _dt2425s2Mid = CreateDT(_ay2425_sem2_mid);
            _dt2425s2Final = CreateDT(_ay2425_sem2_final);
        }

        private static DataTable CreateDT(List<GradeEntry> src)
        {
            var dt = new DataTable();
            dt.Columns.Add("#", typeof(int));
            dt.Columns.Add("Subject Code", typeof(string));
            dt.Columns.Add("Subject Title", typeof(string));
            dt.Columns.Add("Units", typeof(int));
            dt.Columns.Add("Current Grade", typeof(string));
            dt.Columns.Add("Final Grade", typeof(string));
            dt.Columns.Add("Equivalent", typeof(string));
            dt.Columns.Add("Scholastic Status", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));

            foreach (var e in src)
                dt.Rows.Add(
                    e.No, e.SubjectCode, e.SubjectTitle, e.Units,
                    e.Grade.ToString("F0"),
                    e.Grade.ToString("F0"),    // final grade same column for now
                    e.Equivalent.ToString("F2"),
                    e.ScholasticStatus,
                    e.Remarks);

            return dt;
        }

        private void BindGrids()
        {
            dgvMid.DataSource = _dtMid.DefaultView;
            dgvFinal.DataSource = _dtFinal.DefaultView;
        }

        private void PopulateGradeScale()
        {
            dgvScale.Rows.Clear();
            int total = _scale.GetLength(0);
            int rows = (int)Math.Ceiling(total / 3.0);
            for (int r = 0; r < rows; r++)
            {
                var vals = new object[9];
                for (int col = 0; col < 3; col++)
                {
                    int idx = r * 3 + col;
                    if (idx < total)
                    {
                        vals[col * 3 + 0] = _scale[idx, 0];
                        vals[col * 3 + 1] = _scale[idx, 1];
                        vals[col * 3 + 2] = _scale[idx, 2];
                    }
                }
                dgvScale.Rows.Add(vals);
            }

            // size dgvScale to show every row without a scrollbar ──
            // Height = column header + all data rows
            int dgvH = dgvScale.ColumnHeadersHeight
                       + rows * dgvScale.RowTemplate.Height
                       + 2;   // +2 for border pixels
            dgvScale.ScrollBars = ScrollBars.None;
            dgvScale.Height = dgvH;

            //  Resize pnlScale so the grid is fully visible 
            // pnlScale padding is 9px top + 9px bottom; lblScale sits at y=9 with
            // height ~19px; dgvScale is positioned at y=37 (9 padding + 19 label + 9 gap).
            int pnlH = 37 + dgvH + 9;   // top offset + grid height + bottom padding
            pnlScale.Height = pnlH;
            pnlScale.MinimumSize = new Size(0, pnlH);
        }

        //  REFRESH / FILTER

        private void RefreshAll()
        {
            FilterTable();
            UpdateSummaryCards();
            HideDetailPanel();
            pnlTrendChart?.Invalidate();
            pnlPieChart?.Invalidate();
        }

        private void FilterTable()
        {
            string term = txtSearch?.Text.Trim().ToLower() ?? "";
            var (tMid, tFinal) = GetActiveTables();
            var src = _isMidterm ? tMid : tFinal;

            var dv = _isMidterm
                ? dgvMid?.DataSource as DataView
                : dgvFinal?.DataSource as DataView;
            if (dv == null) return;

            dv.RowFilter = string.IsNullOrEmpty(term)
                ? ""
                : $"([Subject Code] LIKE '%{term}%' OR [Subject Title] LIKE '%{term}%')";

            if (lblPageInfo != null)
                lblPageInfo.Text = $"Showing 1 to {dv.Count} of {src.Rows.Count} subjects";
        }

        private void UpdateSummaryCards()
        {
            var (activeMid, activeFinal) = GetActiveData();
            var data = _isMidterm ? activeMid : activeFinal;
            if (data == null || data.Count == 0) return;

            int totalU = data.Sum(e => e.Units);
            int earned = data.Where(e => e.Remarks == "PASSED").Sum(e => e.Units);
            int passed = data.Count(e => e.Remarks == "PASSED");
            int failed = data.Count(e => e.Remarks == "FAILED");
            int ip = data.Count(e => e.Remarks == "INC" || e.Remarks == "IN PROGRESS");

            double tw = 0; int tp = 0;
            foreach (var e in data.Where(e => e.Remarks == "PASSED"))
            { tw += e.Equivalent * e.Units; tp += e.Units; }
            double gwa = tp > 0 ? tw / tp : 0;

            lblGWA.Text = gwa > 0 ? gwa.ToString("F2") : "—";
            lblTotalUnits.Text = totalU.ToString();
            lblUnitsEarned.Text = earned.ToString();
            lblPassed.Text = passed.ToString();
            lblFailed.Text = failed.ToString();
            lblInProgress.Text = ip.ToString();

            // Scholastic Status card: show "Regular" or "Irregular" from PUPSIS data
            // (majority vote – if any subject is Irregular, student is Irregular)
            string scholasticStatus = data.Any(x => x.ScholasticStatus == "Irregular")
                ? "Irregular"
                : "Regular";
            lblScholasticStatus.Text = scholasticStatus;
            lblScholasticStatus.BackColor = ScholasticColor(scholasticStatus);

            // GWA colour
            if (gwa > 0 && gwa <= 1.75) lblGWA.ForeColor = Color.FromArgb(22, 163, 74);
            else if (gwa > 0 && gwa <= 2.50) lblGWA.ForeColor = Color.FromArgb(217, 119, 6);
            else if (gwa > 0) lblGWA.ForeColor = Color.FromArgb(220, 38, 38);
        }

        private static string GetSubjectEnrollmentType(GradeEntry entry) =>
            entry.ScholasticStatus;   // already "Regular" | "Irregular" from data

        private static string DeriveAcademicStanding(double gwa, int failedCount)
        {
            if (failedCount >= 3) return "Academic Dismissal";
            if (failedCount > 0) return "Probationary";
            if (gwa > 0 && gwa <= 1.45) return "University Scholar";
            if (gwa > 0 && gwa <= 1.75) return "College Scholar";
            if (gwa > 0 && gwa <= 2.00) return "Dean's List";
            if (gwa > 0 && gwa <= 3.00) return "Good Standing";
            return "Conditional";
        }

        private static Color AcademicStandingColor(string standing)
        {
            return standing switch
            {
                "University Scholar" => Color.FromArgb(30, 64, 175),
                "College Scholar" => Color.FromArgb(5, 150, 105),
                "Dean's List" => Color.FromArgb(16, 124, 65),
                "Good Standing" => Color.FromArgb(107, 114, 128),
                "Probationary" => Color.FromArgb(180, 83, 9),
                "Academic Dismissal" => Color.FromArgb(185, 28, 28),
                _ => Color.FromArgb(107, 114, 128),
            };
        }

        /// <summary>
        /// Returns a display colour for the per-subject Regular/Irregular badge.
        /// </summary>
        private static Color ScholasticColor(string status) =>
            status == "Irregular"
                ? Color.FromArgb(180, 83, 9)     // amber for irregular
                : Color.FromArgb(21, 128, 61);   // green for regular


        private (List<GradeEntry> mid, List<GradeEntry> final) GetActiveData()
        {
            bool is2nd = cmbSemester?.SelectedIndex == 1;
            bool is2425 = cmbAcYear?.SelectedIndex == 0;
            if (is2425) return is2nd ? (_ay2425_sem2_mid, _ay2425_sem2_final)
                                     : (_ay2425_sem1_mid, _ay2425_sem1_final);
            return is2nd ? (_midterm2, _final2) : (_midterm, _final);
        }

        private (DataTable mid, DataTable final) GetActiveTables()
        {
            bool is2nd = cmbSemester?.SelectedIndex == 1;
            bool is2425 = cmbAcYear?.SelectedIndex == 0;
            if (is2425) return is2nd ? (_dt2425s2Mid, _dt2425s2Final)
                                     : (_dt2425s1Mid, _dt2425s1Final);
            return is2nd ? (_dtMid2, _dtFinal2) : (_dtMid, _dtFinal);
        }

        private GradeEntry? FindEntry(string subjectCode)
        {
            var (mid, final) = GetActiveData();
            var list = _isMidterm ? mid : final;
            foreach (var e in list)
                if (e.SubjectCode == subjectCode) return e;
            return null;
        }


        private void ShowDetailPanel(GradeEntry entry)
        {
            _detailSubject = entry;

            lblDetailTitle.Text = $"{entry.SubjectCode} – {entry.SubjectTitle}";
            lblDetailGrade.Text = $"{entry.Grade:F0}";
            lblDetailEquivalent.Text = $"{entry.Equivalent:F2}";
            lblDetailRemarks.Text = entry.Remarks;
            lblDetailScholastic.Text = entry.ScholasticStatus;   // "Regular" | "Irregular"
            lblDetailFeedback.Text = entry.InstructorFeedback;

            lblDetailRemarks.ForeColor = entry.Remarks == "PASSED"
                ? Color.FromArgb(22, 163, 74) : Color.FromArgb(220, 38, 38);
            lblDetailScholastic.ForeColor = ScholasticColor(entry.ScholasticStatus);

            // Breakdown bars
            UpdateBreakdownBar(pbarActivities, lblActVal, entry.Activities, "Activities");
            UpdateBreakdownBar(pbarQuizzes, lblQzVal, entry.Quizzes, "Quizzes");
            UpdateBreakdownBar(pbarLongQuizzes, lblLQzVal, entry.LongQuizzes, "Long Quizzes");
            UpdateBreakdownBar(pbarAttendance, lblAttVal, entry.Attendance, "Attendance");
            UpdateBreakdownBar(pbarMajorAssmt, lblMajVal, entry.MajorAssessments, "Major Assessments");

            pnlDetail.Visible = true;
            pnlDetail.BringToFront();
        }

        private static void UpdateBreakdownBar(ProgressBar bar, Label lbl, double value, string _)
        {
            bar.Value = Math.Min(100, Math.Max(0, (int)value));
            lbl.Text = $"{value:F0}%";
        }

        private void HideDetailPanel()
        {
            pnlDetail.Visible = false;
            _detailSubject = null;
        }

        //  CHART PAINTING

        private void PnlTrendChart_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            var pnl = (Panel)sender;
            int pw = pnl.Width, ph = pnl.Height;
            if (pw < 20 || ph < 20) return;

            bool perYear = cmbTrend?.SelectedIndex == 1;
            double[] midVals, finalVals;
            string[] labels;

            if (!perYear)
            {
                midVals = new[] { 1.53, 1.75, 1.88, 2.00 };
                finalVals = new[] { 1.60, 1.70, 1.80, 1.95 };
                labels = new[] { "2nd'23", "1st'24", "2nd'24", "1st'25" };
            }
            else
            {
                midVals = new[] { 1.64, 1.72, 1.81 };
                finalVals = new[] { 1.68, 1.75, 1.85 };
                labels = new[] { "2023", "2024", "2025" };
            }

            double[] vals = _isMidterm ? midVals : finalVals;
            int count = vals.Length;
            double minV = vals.Min();
            double maxV = vals.Max();
            double range = (maxV - minV) < 0.01 ? 0.5 : (maxV - minV);

            using var fVal = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            using var fLbl = new Font("Segoe UI", 7.5f);
            using var barBr = new SolidBrush(Color.FromArgb(128, 0, 0));
            using var textBr = new SolidBrush(Color.FromArgb(110, 110, 110));
            using var valBr = new SolidBrush(Color.FromArgb(40, 40, 40));

            // Measure the tallest value label so padT is always enough
            float valLblH = g.MeasureString("0.00", fVal).Height;
            float lblH = g.MeasureString("Wg", fLbl).Height;

            int padL = 4;
            int padR = 4;
            int padT = (int)valLblH + 4;   // guaranteed room above tallest bar
            int padB = (int)lblH + 6;   // guaranteed room below bars

            // Use float arithmetic throughout to avoid integer-division remainder clipping
            float chartW = pw - padL - padR;
            float chartH = ph - padT - padB;
            float slotW = chartW / count;                      // exact float slot width
            float barW = Math.Max(6f, slotW * 0.55f);        // 55 % of slot

            for (int i = 0; i < count; i++)
            {
                double norm = (vals[i] - minV) / range;
                float bH = (float)(norm * chartH * 0.70 + chartH * 0.22);
                bH = Math.Max(4f, Math.Min(bH, chartH));

                // Slot starts at exact float position – no rounding accumulation
                float slotX = padL + i * slotW;
                float bx = slotX + (slotW - barW) / 2f;
                float by = padT + chartH - bH;

                g.FillRectangle(barBr, bx, by, barW, bH);

                // Value label: centred above bar, clipped to top edge
                string vStr = vals[i].ToString("F2");
                var vSz = g.MeasureString(vStr, fVal);
                float vx = bx + (barW - vSz.Width) / 2f;
                float vy = Math.Max(0f, by - vSz.Height - 2f);
                g.DrawString(vStr, fVal, valBr, vx, vy);

                // X-axis label: centred under slot, clipped to bottom edge
                var lSz = g.MeasureString(labels[i], fLbl);
                float lx = slotX + (slotW - lSz.Width) / 2f;
                float ly = padT + chartH + 4f;
                g.DrawString(labels[i], fLbl, textBr, lx, ly);
            }
        }

        private void PnlPieChart_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            var pnl = (Panel)sender;
            int pw = pnl.Width, ph = pnl.Height;
            if (pw < 20 || ph < 20) return;

            var (activeMid, activeFinal) = GetActiveData();
            var data = _isMidterm ? activeMid : activeFinal;

            // Build non-empty buckets only
            var allBuckets = new (string Key, int Count)[]
            {
                ("1.00–1.25", data.Count(x => x.Equivalent <= 1.25)),
                ("1.50–1.75", data.Count(x => x.Equivalent  > 1.25 && x.Equivalent <= 1.75)),
                ("2.00–2.25", data.Count(x => x.Equivalent  > 1.75 && x.Equivalent <= 2.25)),
                ("2.50–3.00", data.Count(x => x.Equivalent  > 2.25)),
            };
            var buckets = allBuckets.Where(b => b.Count > 0).ToArray();
            int total = buckets.Sum(b => b.Count);
            if (total == 0) return;

            Color[] palette = {
                Color.FromArgb(128,   0,   0),   // maroon
                Color.FromArgb(180,  83,   9),   // amber
                Color.FromArgb( 21, 128,  61),   // green
                Color.FromArgb( 29,  78, 216),   // blue
            };

            using var fLbl = new Font("Segoe UI", 7.5f);
            using var textBr = new SolidBrush(Color.FromArgb(60, 60, 60));

            // Measure a reference string once to get the real rendered text height
            var szRef = g.MeasureString("Ag", fLbl);
            float txtH = szRef.Height;           // true rendered height

            // Pie: square, vertically centred, left margin 8px
            int padL = 8;
            int pieSize = Math.Min(ph - 8, Math.Min(pw / 3, 80));
            int pieX = padL;
            int pieY = (ph - pieSize) / 2;

            // Legend: starts right of pie, each row = txtH + 4px gap
            int legX = pieX + pieSize + 14;
            int swatchW = 10;
            int swatchH = 10;
            float rowH = txtH + 4f;                // row height tracks actual text height
            float totalLegH = buckets.Length * rowH;
            float legStartY = (ph - totalLegH) / 2f;  // vertically centred in panel

            // Draw pie slices
            float startAngle = -90f;
            for (int i = 0; i < buckets.Length; i++)
            {
                Color c = palette[i % palette.Length];
                float sweep = (buckets[i].Count / (float)total) * 360f;
                using (var br = new SolidBrush(c))
                    g.FillPie(br, pieX, pieY, pieSize, pieSize, startAngle, sweep);
                // white divider line
                using (var sep = new Pen(Color.White, 1.2f))
                    g.DrawPie(sep, pieX, pieY, pieSize, pieSize, startAngle, sweep);
                startAngle += sweep;
            }

            // Draw legend rows – each row is exactly rowH tall, swatch and text
            // are both vertically centred inside that row
            for (int i = 0; i < buckets.Length; i++)
            {
                Color c = palette[i % palette.Length];
                float rowY = legStartY + i * rowH;

                // Swatch: centred vertically in the row
                float swY = rowY + (rowH - swatchH) / 2f;
                g.FillRectangle(new SolidBrush(c), legX, swY, swatchW, swatchH);

                // Text: baseline aligned with swatch centre
                float txY = rowY + (rowH - txtH) / 2f;
                g.DrawString(
                    $"{buckets[i].Key}: {buckets[i].Count}",
                    fLbl, textBr,
                    legX + swatchW + 5f, txY);
            }
        }

        //  EVENT HANDLERS

        private void CmbFilterChanged(object sender, EventArgs e)
        {
            var (tMid, tFinal) = GetActiveTables();
            dgvMid.DataSource = tMid.DefaultView;
            dgvFinal.DataSource = tFinal.DefaultView;
            if (txtSearch != null) txtSearch.Text = "";
            RefreshAll();
        }

        private void TabGrades_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tc = (TabControl)sender;
            var tab = tc.TabPages[e.Index];
            bool sel = e.Index == tc.SelectedIndex;

            e.Graphics.FillRectangle(
                sel ? new SolidBrush(Color.White)
                    : new SolidBrush(Color.FromArgb(245, 245, 245)), e.Bounds);

            if (sel)
            {
                using var thick = new Pen(Color.Maroon, 2);
                e.Graphics.DrawLine(thick, e.Bounds.Left, e.Bounds.Top + 1, e.Bounds.Right - 1, e.Bounds.Top + 1);
            }
            using var br = new SolidBrush(sel ? Color.FromArgb(128, 0, 0) : Color.FromArgb(100, 100, 100));
            using var f = new Font("Segoe UI", 9f);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.DrawString(tab.Text, f, br, e.Bounds, sf);
        }

        private void TabGrades_SelectedIndexChanged(object sender, EventArgs e)
        {
            _isMidterm = tabGrades.SelectedIndex == 0;
            FilterTable();
            UpdateSummaryCards();
            HideDetailPanel();
            pnlPieChart?.Invalidate();
            pnlTrendChart?.Invalidate();
        }

        private void DgGrades_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = (DataGridView)sender;

            // Remarks colouring
            int remIdx = grid.Columns["Remarks"]?.Index ?? -1;
            if (e.ColumnIndex == remIdx && e.Value != null)
            {
                switch (e.Value.ToString())
                {
                    case "PASSED":
                        e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                        e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                        break;
                    case "FAILED":
                        e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                        e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                        break;
                    case "INC":
                        e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                        break;
                }
            }

            // Scholastic Status colouring (Regular = green, Irregular = amber)
            int ssIdx = grid.Columns["Scholastic Status"]?.Index ?? -1;
            if (e.ColumnIndex == ssIdx && e.Value != null)
            {
                e.CellStyle.ForeColor = ScholasticColor(e.Value.ToString());
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }

            // Alternate row background
            e.CellStyle.BackColor = e.RowIndex % 2 == 0
                ? Color.White
                : Color.FromArgb(250, 250, 252);
        }

        private void DgGrades_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dg = (DataGridView)sender;
            if (dg.Columns.Count == 0) return;

            // Explicit fill weights so every column is visible and proportioned.
            // Total = 100 units.  Match proportions seen in the screenshot:
            //   #  SubjectCode  SubjectTitle  Units  CurrentGrade  FinalGrade  Equivalent  ScholasticStatus  Remarks
            var weights = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "#",                3  },
                { "Subject Code",     10 },
                { "Subject Title",    22 },
                { "Units",            5  },
                { "Current Grade",    12 },
                { "Final Grade",      12 },
                { "Equivalent",       9  },
                { "Scholastic Status",14 },
                { "Remarks",          13 },
            };

            foreach (DataGridViewColumn col in dg.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (weights.TryGetValue(col.HeaderText, out int w))
                    col.FillWeight = w;
                else
                    col.FillWeight = 10;   // safe default for any unexpected column
            }

            // ── Fix 2: shrink tabGrades to fit rows exactly, removing blank space ──
            // Calculate the exact pixel height required for all visible rows.
            int rowCount = dg.Rows.Count;
            int rowH = dg.RowTemplate.Height;              // 28 px per row
            int headerH = dg.ColumnHeadersHeight;             // 32 px
            int tabStrip = tabGrades.ItemSize.Height + 4;      // tab strip + border (~30 px)
            int tpPad = 6;                                   // TabPage padding top+bottom
            int minRows = 4;                                   // always show at least 4 rows

            int contentH = headerH + Math.Max(minRows, rowCount) * rowH + tpPad + 2;
            int newTabH = tabStrip + contentH;

            if (tabGrades.Height != newTabH)
            {
                // Remove Bottom anchor so height change is not fought by the layout engine
                tabGrades.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                tabGrades.Height = newTabH;

                // Reposition lblPageInfo just below the tab control
                int pageInfoY = tabGrades.Bottom + 4;
                lblPageInfo.Location = new Point(lblPageInfo.Left, pageInfoY);

                // Shrink pnlLeft to wrap its content tightly
                int pnlLeftH = pageInfoY + lblPageInfo.Height + 8;
                pnlLeft.Height = pnlLeftH;

                // Shrink tlpMain row to the taller of left panel or right panel
                int rightH = tlpRight.PreferredSize.Height > 0
                    ? tlpRight.PreferredSize.Height
                    : pnlLeftH;
                tlpMain.Height = Math.Max(pnlLeftH, rightH);
            }
        }

        private void DgGrades_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dg = (DataGridView)sender;
            var codeCell = dg.Rows[e.RowIndex].Cells["Subject Code"];
            if (codeCell?.Value == null) return;

            string code = codeCell.Value.ToString();
            var entry = FindEntry(code);
            if (entry.HasValue)
                ShowDetailPanel(entry.Value);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e) => FilterTable();
        private void CmbTrend_SelectedIndexChanged(object sender, EventArgs e) => pnlTrendChart?.Invalidate();

        private void BtnCloseDetail_Click(object sender, EventArgs e) => HideDetailPanel();

        private void BtnGenerateCOG_Click(object sender, EventArgs e)
        {
            var (mid1, final1) = GetActiveData();
            bool is2425 = cmbAcYear?.SelectedIndex == 0;
            string ayLabel = is2425 ? "2024 – 2025" : "2025 – 2026";

            using var sfd = new SaveFileDialog
            {
                Filter = "PDF Documents (*.pdf)|*.pdf",
                FileName = $"COG_{ayLabel.Replace(" ", "").Replace("–", "_")}.pdf",
                Title = "Save Certificate of Grades"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                string logoPath = System.IO.Path.Combine(
                    Application.StartupPath, "Resources", "pup_logo.png");

                // Map to CogGenerator.GradeEntry
                static List<CogGenerator.GradeEntry> Map(List<GradeEntry> src) =>
                    src.Select(e => new CogGenerator.GradeEntry
                    {
                        SubjectCode = e.SubjectCode,
                        SubjectTitle = e.SubjectTitle,
                        Units = e.Units,
                        Equivalent = e.Equivalent,
                        Remarks = e.Remarks,
                        EnrollmentType = e.ScholasticStatus   // "Regular" | "Irregular"
                    }).ToList();

                bool is2nd = cmbSemester?.SelectedIndex == 1;
                List<GradeEntry> sem1Mid, sem1Final, sem2Mid, sem2Final;

                if (is2425)
                {
                    sem1Mid = _ay2425_sem1_mid; sem1Final = _ay2425_sem1_final;
                    sem2Mid = _ay2425_sem2_mid; sem2Final = _ay2425_sem2_final;
                }
                else
                {
                    sem1Mid = _midterm; sem1Final = _final;
                    sem2Mid = _midterm2; sem2Final = _final2;
                }

                // Use final grades for COG
                CogGenerator.Generate(sfd.FileName, logoPath, ayLabel, Map(sem1Final), Map(sem2Final));
                ShowToast("COG generated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating COG:\n{ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddNote_Click(object sender, EventArgs e)
        {
            using var dlg = new NoteDialog();
            if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.NoteText))
            {
                _notes.Add(dlg.NoteText.Trim());
                RefreshNotes();
            }
        }

        private void RefreshNotes()
        {
            flpNotes.Controls.Clear();
            if (_notes.Count == 0) { flpNotes.Controls.Add(lblNoNotes); return; }

            for (int i = 0; i < _notes.Count; i++)
            {
                int captured = i;
                var row = new Panel { Width = flpNotes.Width - 4, Height = 28, BackColor = Color.White };
                var lbl = new Label
                {
                    Text = _notes[i],
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(60, 60, 60),
                    AutoSize = true,
                    Location = new Point(4, 6),
                    Width = row.Width - 28,
                    AutoEllipsis = true
                };
                var btn = new Button
                {
                    Text = "×",
                    Location = new Point(row.Width - 24, 4),
                    Size = new Size(20, 20),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s2, e2) => { _notes.RemoveAt(captured); RefreshNotes(); };
                row.Controls.Add(lbl);
                row.Controls.Add(btn);
                flpNotes.Controls.Add(row);
                flpNotes.Controls.Add(new Panel
                { Width = flpNotes.Width - 4, Height = 1, BackColor = Color.FromArgb(230, 230, 230) });
            }
        }

        private void ShowToast(string msg)
        {
            var toast = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                BackColor = Color.FromArgb(22, 163, 74),
                Size = new Size(260, 40),
                ShowInTaskbar = false,
                TopMost = true,
                Opacity = 0
            };
            toast.Controls.Add(new Label
            {
                Text = msg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
            var screen = Screen.PrimaryScreen.WorkingArea;
            toast.Location = new Point(screen.Right - 280, screen.Bottom - 58);
            toast.Show();

            var fade = new System.Windows.Forms.Timer { Interval = 30 };
            int step = 0;
            fade.Tick += (s, ev) =>
            {
                step++;
                toast.Opacity = step < 10 ? step * 0.1 :
                                step < 40 ? 1 :
                                step < 50 ? 1 - (step - 40) * 0.1 : 0;
                if (step >= 50) { fade.Stop(); toast.Close(); }
            };
            fade.Start();
        }
    }

    internal class NoteDialog : Form
    {
        public string NoteText { get; private set; } = "";

        public NoteDialog()
        {
            Text = "Add Note"; Size = new Size(360, 150);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false; MinimizeBox = false;
            BackColor = Color.White; Font = new Font("Segoe UI", 9f);

            var lbl = new Label { Text = "Note:", Location = new Point(12, 16), AutoSize = true };
            var txt = new TextBox { Location = new Point(12, 36), Width = 318, Height = 26, BorderStyle = BorderStyle.FixedSingle };
            var btnOk = new Button { Text = "Save", DialogResult = DialogResult.OK, Location = new Point(172, 74), Size = new Size(80, 28), BackColor = Color.FromArgb(128, 0, 0), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCnl = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(260, 74), Size = new Size(70, 28), FlatStyle = FlatStyle.Flat };
            btnOk.Click += (s, e) => NoteText = txt.Text;
            Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCnl });
            AcceptButton = btnOk; CancelButton = btnCnl;
        }
    }
}