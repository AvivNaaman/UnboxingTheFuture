using AtidRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Configuration
{
    /// <summary>
    /// AppConfig to get from appsettings/whatever
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Timeout until user session is canceled (with no activity, of course)
        /// </summary>
        public double AdminMinutesTimeout { get; set; }
        /// <summary>
        /// Default Person image, base64
        /// </summary>
        public string DefaultPersonImage { get; set; }
        /// <summary>
        /// Default content image, base64
        /// </summary>
        public string DefaultContentImage { get; set; }
        /// <summary>
        /// Allowed images file extension
        /// </summary>
        public List<string> AllowedImageFileExtensions { get; set; }
        /// <summary>
        /// Background image url
        /// </summary>
        public string BackgroundImageURL { get; set; }
        /// <summary>
        /// default admin user properties (for first time setup)
        /// </summary>
        public DefaultAdminUser DefaultAdminUser { get; set; }
        /// <summary>
        /// minimum priorities needed to be chosen by user per timeline
        /// </summary>
        public int MinPrioritiesPerTimeline { get; set; }
    }
    /// <summary>
    /// Default admin user props
    /// </summary>
    public class DefaultAdminUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
