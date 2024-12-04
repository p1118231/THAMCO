using System.ComponentModel.DataAnnotations;

namespace THAMCOMVC.Models;

public class User{
    public int Id{get;set;}

    [Required]
    [Display (Name = "First Name")]
    public required String FirstName{get; set;}

    [Required]
    [Display (Name = "Last Name")]
    public required String LastName{get; set;}
    
    [Required]
    [EmailAddress]
    public required String Email{get; set;}

    [Required]
    [StringLength(500, ErrorMessage = "Address must be up to 500 characters long.")]
    [Display (Name = "Payment Address")]

    public required String PaymentAddress{get; set;}
    
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
    ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    [DataType(DataType.Password)]
    public required String Password{get; set;}

    [Phone]
    [Required]
    [RegularExpression(@"^\+?[1-9]\d{11}$", ErrorMessage = "The phone number must be in international format (e.g., +1234567890).")]

    [Display (Name = "Phone Number")]
    public required string PhoneNumber { get; set; }


    public string? Auth0UserId { get; set; }
}