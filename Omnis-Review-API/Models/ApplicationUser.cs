using Microsoft.AspNetCore.Identity;

namespace OmnisReview.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public DateTime Birth_Date { get; set; }
}
