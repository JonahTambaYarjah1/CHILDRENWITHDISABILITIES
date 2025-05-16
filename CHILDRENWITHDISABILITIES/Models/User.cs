using System.ComponentModel.DataAnnotations;

namespace ChildrenWithDisabilitiesAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required] public required string Full_Name { get; set; }

        [Required, EmailAddress] public required string Email { get; set; }

        [Required] public required string Password { get; set; }

        public int? Reset_Token { get; set; }
        public DateTime? Reset_Token_Expiry { get; set; }
    }
}
