using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Website.Models;

namespace FoodieSydney.Model
{
    public class RegisterViewModel : RegisterModel
    {
        public Guid RegisterSuccessPageID { get; set; }

        public RegisterViewModel() { }

        public RegisterViewModel(Guid PageID)
        {
            RegisterSuccessPageID = PageID;
        }
    }
}
