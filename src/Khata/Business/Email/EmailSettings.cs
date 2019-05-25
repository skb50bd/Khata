using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class EmailSettings
    {
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string MailGunDomain { get; set; }
        public string MailGunApiKey { get; set; }
    }
}
