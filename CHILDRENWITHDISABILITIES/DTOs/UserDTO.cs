namespace ChildrenWithDisabilitiesAPI.DTOs
{
    public class RegisterDTO
    {
        public required string Full_Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        // ✅ Add this to accept the confirmation code
        public required string EmailVerificationToken { get; set; }
    }

    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
