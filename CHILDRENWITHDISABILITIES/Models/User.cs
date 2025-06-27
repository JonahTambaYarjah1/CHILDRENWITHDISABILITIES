using System.ComponentModel.DataAnnotations;

namespace ChildrenWithDisabilitiesAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Full_Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public string? EmailVerificationToken { get; set; }

        // ✅ Must exist in the model!
        public int? Reset_Token { get; set; }
        public DateTime? Reset_Token_Expiry { get; set; }
    }
}
