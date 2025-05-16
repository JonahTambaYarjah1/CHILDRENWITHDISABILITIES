namespace ChildrenWithDisabilitiesAPI.DTOs
{
    public class RegisterDTO
    {
        public required string Full_Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
