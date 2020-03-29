using AtidRegister.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AtidRegister.Services
{
    public interface ISMSService
    {
        /// <summary>
        /// Sends a single SMS message <see langword="async"/>
        /// </summary>
        /// <param name="phoneNumber">The full phone number, formatted by E.164 standard.</param>
        /// <param name="message">The message body contnet</param>
        public Task SingleMessage(string phoneNumber, string message);
        public Task BulkMessages(List<BulkSmsMessage> smsMessages);
    }
    public class SMSService : ISMSService, IDisposable
    {
        private readonly TwilioSettings _config;
        private readonly ILogger<SMSService> _logger;

        public SMSService(IOptions<AppConfig> config, ILogger<SMSService> logger)
        {
            _config = config.Value.Twilio;
            _logger = logger;
            // Initialize Twilio Client
            TwilioClient.Init(_config.AccountSID, _config.AuthToken);
        }

        public async Task BulkMessages(List<BulkSmsMessage> smsMessages)
        {
            _logger.LogInformation("========Starting Bulk SMS send. Totally {0} messages.========", smsMessages.Count);
            // for each message
            for (int i = 0; i < smsMessages.Count; i++)
            {
                // send as single
                var currMsg = smsMessages[i];
                await SingleMessage(currMsg.PhoneNumber, currMsg.Message);
                await Task.Delay(250); // and wait 1/4 second
            }
            _logger.LogInformation("============================End.=============================");
        }

        public async Task SingleMessage(string phoneNumber, string message)
        {
            _logger.LogInformation("Sending SMS to {0} with content {1}", phoneNumber, message);
            var result = await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                body: message, messagingServiceSid: _config.MessagingServiceSID
            );
            if (!result.ErrorCode.HasValue)
                _logger.LogInformation("Success.");
            else
                _logger.LogError("ERROR! {0}/{1}", result.ErrorCode, result.ErrorMessage);
        }

        public void Dispose()
        {
            // Dispose Twilio Client
            TwilioClient.Invalidate();
        }
    }
    public class BulkSmsMessage
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
