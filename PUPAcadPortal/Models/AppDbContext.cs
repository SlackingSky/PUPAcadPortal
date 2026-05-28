using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PUPAcadPortal.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicPeriod> AcademicPeriods { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }

    public virtual DbSet<CalendarEvent> CalendarEvents { get; set; }

    public virtual DbSet<ClassSession> ClassSessions { get; set; }

    public virtual DbSet<Curriculum> Curricula { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<EnrollmentSubject> EnrollmentSubjects { get; set; }

    public virtual DbSet<FinalCourseGrade> FinalCourseGrades { get; set; }

    public virtual DbSet<GradingCategory> GradingCategories { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<PersonalNote> PersonalNotes { get; set; }

    public virtual DbSet<Professor> Professors { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomSchedule> RoomSchedules { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAccount> StudentAccounts { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<SubjectOffering> SubjectOfferings { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicPeriod>(entity =>
        {
            entity.HasKey(e => e.AcademicPeriodId).HasName("PRIMARY");

            entity.ToTable("AcademicPeriod");

            entity.Property(e => e.AcademicPeriodId)
                .HasMaxLength(50)
                .HasColumnName("AcademicPeriodID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.SchoolYear).HasMaxLength(20);
            entity.Property(e => e.Semester).HasMaxLength(20);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PRIMARY");

            entity.HasIndex(e => e.CategoryId, "FK_Activities_Category");

            entity.HasIndex(e => e.ModuleId, "FK_Activities_Module");

            entity.HasIndex(e => e.SubjectOfferingId, "FK_Activities_Offering");

            entity.Property(e => e.ActivityId)
                .HasMaxLength(50)
                .HasColumnName("ActivityID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.ModuleId)
                .HasMaxLength(50)
                .HasColumnName("ModuleID");
            entity.Property(e => e.SubjectOfferingId)
                .HasMaxLength(50)
                .HasColumnName("SubjectOfferingID");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Activities)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Activities_Category");

            entity.HasOne(d => d.Module).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Activities_Module");

            entity.HasOne(d => d.SubjectOffering).WithMany(p => p.Activities)
                .HasForeignKey(d => d.SubjectOfferingId)
                .HasConstraintName("FK_Activities_Offering");
        });

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("ActivityLog");

            entity.HasIndex(e => e.UserId, "FK_ActivityLog_User");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(255);
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ActivityLog_User");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PRIMARY");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.UserId, "FK_Admin_User");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.RoleTitle).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Admins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Admin_User");
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.AnnouncementId).HasName("PRIMARY");

            entity.HasIndex(e => e.CreatedByUserId, "FK_Announcement_User");

            entity.Property(e => e.AnnouncementId).HasColumnName("AnnouncementID");
            entity.Property(e => e.AttachedFile).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.PostedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Announcement_User");

            entity.HasMany(d => d.TargetRoles).WithMany(p => p.Announcements)
                .UsingEntity<Dictionary<string, object>>(
                    "AnnouncementAudience",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("TargetRoleId")
                        .HasConstraintName("FK_Audience_Role"),
                    l => l.HasOne<Announcement>().WithMany()
                        .HasForeignKey("AnnouncementId")
                        .HasConstraintName("FK_Audience_Announcement"),
                    j =>
                    {
                        j.HasKey("AnnouncementId", "TargetRoleId").HasName("PRIMARY");
                        j.ToTable("AnnouncementAudience");
                        j.HasIndex(new[] { "TargetRoleId" }, "FK_Audience_Role");
                        j.IndexerProperty<int>("AnnouncementId").HasColumnName("AnnouncementID");
                        j.IndexerProperty<int>("TargetRoleId").HasColumnName("TargetRoleID");
                    });
        });

        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PRIMARY");

            entity.ToTable("AttendanceRecord");

            entity.HasIndex(e => e.SessionId, "FK_Attendance_Session");

            entity.HasIndex(e => e.StudentId, "FK_Attendance_Student");

            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.Remarks).HasMaxLength(255);
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Session).WithMany(p => p.AttendanceRecords)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK_Attendance_Session");

            entity.HasOne(d => d.Student).WithMany(p => p.AttendanceRecords)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Attendance_Student");
        });

        modelBuilder.Entity<CalendarEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PRIMARY");

            entity.HasIndex(e => e.RoomId, "FK_CalendarEvents_Room");

            entity.HasIndex(e => e.SubjectOfferingId, "FK_Calendar_Subject");

            entity.HasIndex(e => e.UserId, "FK_Calendar_User");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.EventType).HasMaxLength(100);
            entity.Property(e => e.IsPrivate)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.SubjectOfferingId)
                .HasMaxLength(50)
                .HasColumnName("SubjectOfferingID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Room).WithMany(p => p.CalendarEvents)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_CalendarEvents_Room");

            entity.HasOne(d => d.SubjectOffering).WithMany(p => p.CalendarEvents)
                .HasForeignKey(d => d.SubjectOfferingId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Calendar_Subject");

            entity.HasOne(d => d.User).WithMany(p => p.CalendarEvents)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Calendar_User");
        });

        modelBuilder.Entity<ClassSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PRIMARY");

            entity.ToTable("ClassSession");

            entity.HasIndex(e => e.SubjectOfferingId, "FK_Session_Offering");

            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.EndTime).HasColumnType("time");
            entity.Property(e => e.SessionDate).HasColumnType("date");
            entity.Property(e => e.StartTime).HasColumnType("time");
            entity.Property(e => e.SubjectOfferingId)
                .HasMaxLength(50)
                .HasColumnName("SubjectOfferingID");
            entity.Property(e => e.Topic).HasMaxLength(255);

            entity.HasOne(d => d.SubjectOffering).WithMany(p => p.ClassSessions)
                .HasForeignKey(d => d.SubjectOfferingId)
                .HasConstraintName("FK_Session_Offering");
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.HasKey(e => e.CurriculumId).HasName("PRIMARY");

            entity.ToTable("Curriculum");

            entity.HasIndex(e => e.SubjectId, "FK_Curriculum_Subject");

            entity.Property(e => e.CurriculumId).HasColumnName("CurriculumID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Program).HasMaxLength(100);
            entity.Property(e => e.SubjectId)
                .HasMaxLength(50)
                .HasColumnName("SubjectID");

            entity.HasOne(d => d.Subject).WithMany(p => p.Curricula)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_Curriculum_Subject");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PRIMARY");

            entity.ToTable("Enrollment");

            entity.HasIndex(e => e.AcademicPeriodId, "FK_Enrollment_AcademicPeriod");

            entity.HasIndex(e => e.StudentId, "FK_Enrollment_Student");

            entity.Property(e => e.EnrollmentId)
                .HasMaxLength(50)
                .HasColumnName("EnrollmentID");
            entity.Property(e => e.AcademicPeriodId)
                .HasMaxLength(50)
                .HasColumnName("AcademicPeriodID");
            entity.Property(e => e.EnrollmentDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.AcademicPeriod).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.AcademicPeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_AcademicPeriod");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Student");
        });

        modelBuilder.Entity<EnrollmentSubject>(entity =>
        {
            entity.HasKey(e => e.EnrollmentSubjId).HasName("PRIMARY");

            entity.ToTable("EnrollmentSubject");

            entity.HasIndex(e => e.EnrollmentId, "FK_EnrollmentSubj_Enrollment");

            entity.HasIndex(e => e.SubjectOfferingId, "FK_EnrollmentSubj_Offering");

            entity.Property(e => e.EnrollmentSubjId)
                .HasMaxLength(50)
                .HasColumnName("EnrollmentSubjID");
            entity.Property(e => e.EnrollmentId)
                .HasMaxLength(50)
                .HasColumnName("EnrollmentID");
            entity.Property(e => e.Section).HasMaxLength(20);
            entity.Property(e => e.SubjectOfferingId)
                .HasMaxLength(50)
                .HasColumnName("SubjectOfferingID");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.EnrollmentSubjects)
                .HasForeignKey(d => d.EnrollmentId)
                .HasConstraintName("FK_EnrollmentSubj_Enrollment");

            entity.HasOne(d => d.SubjectOffering).WithMany(p => p.EnrollmentSubjects)
                .HasForeignKey(d => d.SubjectOfferingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnrollmentSubj_Offering");
        });

        modelBuilder.Entity<FinalCourseGrade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PRIMARY");

            entity.HasIndex(e => e.EnrollmentSubjId, "FK_FinalGrade_EnrollmentSubj");

            entity.HasIndex(e => e.EncodedByProfId, "FK_FinalGrade_Prof");

            entity.Property(e => e.GradeId).HasColumnName("GradeID");
            entity.Property(e => e.EncodedByProfId).HasColumnName("EncodedByProfID");
            entity.Property(e => e.EnrollmentSubjId)
                .HasMaxLength(50)
                .HasColumnName("EnrollmentSubjID");
            entity.Property(e => e.FinalCalculatedGrade).HasPrecision(5);
            entity.Property(e => e.FinalRating).HasPrecision(3);
            entity.Property(e => e.Remarks).HasMaxLength(50);

            entity.HasOne(d => d.EncodedByProf).WithMany(p => p.FinalCourseGrades)
                .HasForeignKey(d => d.EncodedByProfId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FinalGrade_Prof");

            entity.HasOne(d => d.EnrollmentSubj).WithMany(p => p.FinalCourseGrades)
                .HasForeignKey(d => d.EnrollmentSubjId)
                .HasConstraintName("FK_FinalGrade_EnrollmentSubj");
        });

        modelBuilder.Entity<GradingCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("GradingCategory");

            entity.HasIndex(e => e.SubjectOfferingId, "FK_Category_Offering");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.SubjectOfferingId)
                .HasMaxLength(50)
                .HasColumnName("SubjectOfferingID");
            entity.Property(e => e.WeightPercentage).HasPrecision(5);

            entity.HasOne(d => d.SubjectOffering).WithMany(p => p.GradingCategories)
                .HasForeignKey(d => d.SubjectOfferingId)
                .HasConstraintName("FK_Category_Offering");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PRIMARY");

            entity.HasIndex(e => e.SubjectOfferingId, "FK_Modules_Offering");

            entity.Property(e => e.ModuleId)
                .HasMaxLength(50)
                .HasColumnName("ModuleID");
            entity.Property(e => e.FileUrl).HasMaxLength(500);
            entity.Property(e => e.ModuleDescription).HasColumnType("text");
            entity.Property(e => e.SubjectOfferingId)
                .HasMaxLength(50)
                .HasColumnName("SubjectOfferingID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.SubjectOffering).WithMany(p => p.Modules)
                .HasForeignKey(d => d.SubjectOfferingId)
                .HasConstraintName("FK_Modules_Offering");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");

            entity.ToTable("PaymentHistory");

            entity.HasIndex(e => e.AccountId, "FK_Payment_Account");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Amount).HasPrecision(10);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.DueDate).HasColumnType("date");
            entity.Property(e => e.PaidDate).HasColumnType("date");
            entity.Property(e => e.ReferenceId)
                .HasMaxLength(50)
                .HasColumnName("ReferenceID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pending'");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.PaymentHistories)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Payment_Account");
        });

        modelBuilder.Entity<PersonalNote>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PRIMARY");

            entity.HasIndex(e => e.UserId, "FK_Notes_User");

            entity.Property(e => e.NoteId).HasColumnName("NoteID");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.PersonalNotes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Notes_User");
        });

        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(e => e.ProfessorId).HasName("PRIMARY");

            entity.ToTable("Professor");

            entity.HasIndex(e => e.EmployeeId, "EmployeeID").IsUnique();

            entity.HasIndex(e => e.UserId, "FK_Professor_User");

            entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EmploymentStatus)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Active'");
            entity.Property(e => e.EmploymentType)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Full-Time'");
            entity.Property(e => e.HighestDegree)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Not Specified'");
            entity.Property(e => e.Rank).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Professors)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Professor_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PRIMARY");

            entity.ToTable("Room");

            entity.HasIndex(e => e.RoomName, "RoomName").IsUnique();

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.Building)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Main Building'");
            entity.Property(e => e.Capacity).HasDefaultValueSql("'40'");
            entity.Property(e => e.RoomName).HasMaxLength(50);
            entity.Property(e => e.RoomType)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Lecture Room'");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Available'");
        });

        modelBuilder.Entity<RoomSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PRIMARY");

            entity.ToTable("RoomSchedule");

            entity.HasIndex(e => e.RoomId, "FK_RoomSchedule_Room");

            entity.HasIndex(e => e.SubjectOfferingId, "FK_Schedule_Offering");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.DayOfWeek).HasMaxLength(20);
            entity.Property(e => e.EndTime).HasColumnType("time");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.SessionType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Lecture'");
            entity.Property(e => e.StartTime).HasColumnType("time");
            entity.Property(e => e.SubjectOfferingId)
                .HasMaxLength(50)
                .HasColumnName("SubjectOfferingID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomSchedules)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_RoomSchedule_Room");

            entity.HasOne(d => d.SubjectOffering).WithMany(p => p.RoomSchedules)
                .HasForeignKey(d => d.SubjectOfferingId)
                .HasConstraintName("FK_Schedule_Offering");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.ToTable("Student");

            entity.HasIndex(e => e.UserId, "FK_Student_User");

            entity.HasIndex(e => e.StudentNumber, "StudentNumber").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Program).HasMaxLength(100);
            entity.Property(e => e.StudentNumber).HasMaxLength(50);
            entity.Property(e => e.StudentType).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Student_User");
        });

        modelBuilder.Entity<StudentAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.ToTable("StudentAccount");

            entity.HasIndex(e => e.AcademicPeriodId, "FK_Account_AcademicPeriod");

            entity.HasIndex(e => e.StudentId, "FK_Account_Student");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AcademicPeriodId)
                .HasMaxLength(50)
                .HasColumnName("AcademicPeriodID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.TotalAssessment).HasPrecision(10);
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AcademicPeriod).WithMany(p => p.StudentAccounts)
                .HasForeignKey(d => d.AcademicPeriodId)
                .HasConstraintName("FK_Account_AcademicPeriod");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentAccounts)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Account_Student");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PRIMARY");

            entity.ToTable("Subject");

            entity.HasIndex(e => e.SubjectCode, "SubjectCode").IsUnique();

            entity.Property(e => e.SubjectId)
                .HasMaxLength(50)
                .HasColumnName("SubjectID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.LabUnits).HasColumnName("Lab_Units");
            entity.Property(e => e.LecUnits).HasColumnName("Lec_Units");
            entity.Property(e => e.SubjectCode).HasMaxLength(20);
            entity.Property(e => e.SubjectName).HasMaxLength(150);
        });

        modelBuilder.Entity<SubjectOffering>(entity =>
        {
            entity.HasKey(e => e.SubjectOfferingId).HasName("PRIMARY");

            entity.ToTable("SubjectOffering");

            entity.HasIndex(e => e.AcademicPeriodId, "FK_Offering_AcademicPeriod");

            entity.HasIndex(e => e.ProfessorId, "FK_Offering_Professor");

            entity.HasIndex(e => e.SubjectId, "FK_Offering_Subject");

            entity.Property(e => e.SubjectOfferingId)
                .HasMaxLength(50)
                .HasColumnName("SubjectOfferingID");
            entity.Property(e => e.AcademicPeriodId)
                .HasMaxLength(50)
                .HasColumnName("AcademicPeriodID");
            entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");
            entity.Property(e => e.Section)
                .HasMaxLength(50)
                .HasDefaultValueSql("'TBA'");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.SubjectId)
                .HasMaxLength(50)
                .HasColumnName("SubjectID");

            entity.HasOne(d => d.AcademicPeriod).WithMany(p => p.SubjectOfferings)
                .HasForeignKey(d => d.AcademicPeriodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offering_AcademicPeriod");

            entity.HasOne(d => d.Professor).WithMany(p => p.SubjectOfferings)
                .HasForeignKey(d => d.ProfessorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offering_Professor");

            entity.HasOne(d => d.Subject).WithMany(p => p.SubjectOfferings)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offering_Subject");
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PRIMARY");

            entity.HasIndex(e => e.ActivityId, "FK_Submissions_Activity");

            entity.HasIndex(e => e.StudentId, "FK_Submissions_Student");

            entity.Property(e => e.SubmissionId)
                .HasMaxLength(50)
                .HasColumnName("SubmissionID");
            entity.Property(e => e.ActivityId)
                .HasMaxLength(50)
                .HasColumnName("ActivityID");
            entity.Property(e => e.Grade).HasPrecision(5);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.SubmissionDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.SubmittedFile).HasMaxLength(500);

            entity.HasOne(d => d.Activity).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.ActivityId)
                .HasConstraintName("FK_Submissions_Activity");

            entity.HasOne(d => d.Student).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_Submissions_Student");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.RoleId, "FK_User_Role");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AddressLine1).HasMaxLength(255);
            entity.Property(e => e.AddressLine2).HasMaxLength(255);
            entity.Property(e => e.Barangay)
                .HasMaxLength(100)
                .HasDefaultValueSql("'N/A'");
            entity.Property(e => e.Birthdate).HasColumnType("date");
            entity.Property(e => e.CityMunicipality).HasMaxLength(100);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Province).HasMaxLength(100);
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasDefaultValueSql("'N/A'");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Suffix).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
