using System;

namespace AtidRegister.Models
{
    /// <summary>
    /// a ViewModel for application customer exception eror handling.
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
