using System.Drawing;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{
    public partial class QrCodeAttendanceControl
    {
        /// <summary>
        /// Returns a clone of the current QR bitmap, or null if the code has
        /// expired or has not been generated yet.
        /// </summary>
        public Bitmap? GetQrBitmap()
        {
            if (_isExpired || _qrBitmap == null) return null;
            return (Bitmap)_qrBitmap.Clone();
        }

        /// <summary>
        /// Returns the current signed token, or an empty string if expired.
        /// </summary>
        public string GetCurrentToken() => _isExpired ? string.Empty : _currentToken;
    }
}