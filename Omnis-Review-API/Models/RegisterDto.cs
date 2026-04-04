namespace OmnisReview.Models;

public class RegisterDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime Birth_Date { get; set; }
    public string Password { get; set; } = string.Empty;
}
