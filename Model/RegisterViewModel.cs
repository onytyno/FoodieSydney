using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace FoodieSydney.Model
{
    public class RegisterViewModel 
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        public Guid RegisterSuccessPageID { get; set; }

        public RegisterViewModel() { }

        public RegisterViewModel(Guid PageID)
        {
            RegisterSuccessPageID = PageID;
        }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
