using FoodieSydney.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Web.Website.Models;

namespace FoodieSydney.Controller
{
    public class AccountController : SurfaceController
    {
        private readonly IMemberManager _memberManager;
        private readonly IMemberService _memberService;
        private readonly IMemberSignInManager _memberSignInManager;

        public AccountController(IUmbracoContextAccessor umbracoContextAccessor, 
                                 IUmbracoDatabaseFactory databaseFactory,
                                 ServiceContext services, 
                                 AppCaches appCaches, 
                                 IProfilingLogger profilingLogger, 
                                 IPublishedUrlProvider publishedUrlProvider, 
                                 IMemberManager memberManager, 
                                 IMemberService memberService, 
                                 IMemberSignInManager memberSignInManager)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)

        {
            _memberManager = memberManager;
            _memberService = memberService;
            _memberSignInManager = memberSignInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _memberSignInManager.PasswordSignInAsync(model.Email, model.Password, false, true);


            if (result.Succeeded)
            {
                //redirect to certain page
                RedirectToUmbracoPage(Guid.Empty);
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("loginViewModel", "Member is locked out");
            }
            else
            {
                ModelState.AddModelError(nameof(LoginViewModel), "Invalid username or password");
            }
          
            return CurrentUmbracoPage();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // if register failes, this will be triggered. 
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var memberService = this.Services.MemberService;

            // check email exists
            if (memberService.GetByEmail(model.Email) != null)
            {
                ModelState.AddModelError("", "Email already been used");
                return CurrentUmbracoPage();
            }

            //create user with Identity
            var member = memberService.CreateMemberWithIdentity(model.Email, model.Email, "Member");

            //save user into system
            memberService.Save(member);

            //get memberIdentityUser and add password
            var memberIdentityUser = new MemberIdentityUser(member.Id);
            await _memberManager.AddPasswordAsync(memberIdentityUser, model.Password);

            //should redirect to the page that shows register success
            return RedirectToUmbracoPage(model.RegisterSuccessPageID);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel viewModel)
        {
            // if register failes, this will be triggered. 
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            

            // check email exists
            if (_memberService.GetByEmail(viewModel.Email) != null)
            {
                ModelState.AddModelError("", "Email already been used");
                return CurrentUmbracoPage();
            }

            var identityUser = MemberIdentityUser.CreateNew(viewModel.Email, viewModel.Email, "Member", true, "Testing User");
            IdentityResult result = await _memberManager.CreateAsync(identityUser, viewModel.Password);

            RegisterModel model;

            if(result.Succeeded)
            {
                var member = _memberService.GetByKey(identityUser.Key);

                _memberService.Save(member);

            }

            return RedirectToUmbracoPage(viewModel.RegisterSuccessPageID);
        }

    }
}
