using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain
{
    public class MailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpServerPort { get; set; }
        public string EmailDisplayName { get; set; }
        public string SendersName { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public Boolean EnableSsl { get; set; }
    }

}
