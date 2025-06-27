namespace ChildrenWithDisabilitiesAPI.DTOs
{
    public class ResetRequestDTO
    {
        public required string Email { get; set; }
    }

    public class ConfirmResetDTO
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
