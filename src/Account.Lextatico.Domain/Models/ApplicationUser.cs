using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Account.Lextatico.Domain.Models
{
    public class ApplicationUser : BaseIdentityUser
    {
        public string Name { get; private set; }
    }
}
