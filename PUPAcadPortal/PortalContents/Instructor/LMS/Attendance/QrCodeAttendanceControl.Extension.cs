using System.Drawing;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public partial class QrCodeAttendanceControl
    {
        public Bitmap? GetQrBitmap()
        {
            if (_isExpired || _qrBitmap == null) return null;
            return (Bitmap)_qrBitmap.Clone();
        }

        public string GetCurrentToken() => _isExpired ? string.Empty : _currentToken;
    }
}