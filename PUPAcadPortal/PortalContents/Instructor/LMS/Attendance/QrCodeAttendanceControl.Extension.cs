using System.Drawing;

namespace PUPAcadPortal.PortalContents.Instructor.LMS
{

    public partial class QrCodeAttendanceControl
    {
        /// <summary>
        /// Returns a copy of the current QR bitmap, or null if the code has
        /// expired or has not been generated yet.
        /// </summary>
        public Bitmap? GetQrBitmap()
        {
            if (_isExpired || _qrBitmap == null) return null;
            // Return a clone so the caller can save/dispose it independently.
            return (Bitmap)_qrBitmap.Clone();
        }

        public string GetCurrentToken() => _isExpired ? string.Empty : _currentToken;
    }
}