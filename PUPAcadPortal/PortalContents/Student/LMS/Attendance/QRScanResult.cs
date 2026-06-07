namespace PUPAcadPortal.PortalContents.Student.LMS.Attendance
{
    public class QRScanResult
    {
        public string RawText { get; set; } = string.Empty;
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? Period { get; set; }
        public string? Session { get; set; }
        public System.DateTime ScanTime { get; set; } = System.DateTime.Now;
        public bool IsValid { get; set; }
    }
}