using EShop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IEmailService
    {
        Boolean SendEmailAsync(EmailMessage allMails);
    }
}
