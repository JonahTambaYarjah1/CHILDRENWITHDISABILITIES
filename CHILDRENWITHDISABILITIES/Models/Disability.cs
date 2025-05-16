using System.ComponentModel.DataAnnotations;

namespace ChildrenWithDisabilitiesAPI.Models
{
    public class Disability
    {
        public int Id { get; set; }

        [Required] public required string FirstName { get; set; }
        [Required] public required string LastName { get; set; }
        [Required] public required string Gender { get; set; }
        [Required] public int Age { get; set; }
        [Required] public required string Adds { get; set; }
        [Required] public required string District { get; set; }
        [Required] public required string DisabilityType { get; set; }
        [Required] public required string Contact { get; set; }
        public required string PhotoPath { get; set; }
    }
}
