using AtidRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Configuration
{
    public class AppConfig
    {
        public double AdminMinutesTimeout { get; set; }
        public string DefaultPersonImage { get; set; }
        public string DefaultContentImage { get; set; }
        public List<string> AllowedImageFileExtensions { get; set; }
        public string BackgroundImageURL { get; set; }
        public DefaultAdminUser DefaultAdminUser { get; set; }
        public int MinPrioritiesPerTimeline { get; set; }
        public TwilioSettings Twilio { get; set; }
    }

    public class TwilioSettings
    {
        public string AccountSID { get; set; }
        public string AuthToken { get; set; }
        public string MessagingServiceSID { get; set; }
    }

    public class DefaultAdminUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
