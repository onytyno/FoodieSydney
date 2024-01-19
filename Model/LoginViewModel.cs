using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Models;

namespace FoodieSydney.Model
{
    public class LoginViewModel : PostRedirectModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; } = null;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(256)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
