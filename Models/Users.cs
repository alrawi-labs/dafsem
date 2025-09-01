using Microsoft.AspNetCore.Identity;

namespace dafsem.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
