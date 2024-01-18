using FoodieSydney.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace FoodieSydney.Controller
{
    public class AccountController : SurfaceController
    {
        private readonly IMemberManager _memberManager;
        public AccountController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
                                ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, IMemberManager memberManager)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)

        {
            _memberManager = memberManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            //// This doesn't count login failures towards account lockout
            //// To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}

            //need to implement further
            return RedirectToUmbracoPage(Guid.Empty);
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

            var test = memberService.GetByEmail(model.Email);


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

            //add Password of user
            var memberIdentityUser = new MemberIdentityUser(member.Id);

            await _memberManager.AddPasswordAsync(memberIdentityUser, model.Password);


            //memberService.Create

            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //var result = await UserManager.CreateAsync(user, model.Password);
            //if (result.Succeeded)
            //{
            //    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            //    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //    // Send an email with this link
            //    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            //    return RedirectToAction("Index", "Home");
            //}
            //AddErrors(result);

            //should redirect to the page that shows register success
            return RedirectToUmbracoPage(Guid.Empty);
        }

    }
}
