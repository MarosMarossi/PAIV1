using Microsoft.AspNetCore.Identity;

namespace Honeytor.Models
{
    public class ApplicationRole : IdentityRole
    {
        public int PrivilegeLevel { get; set; }
    }
}
