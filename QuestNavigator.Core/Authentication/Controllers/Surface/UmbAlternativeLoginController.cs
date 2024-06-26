using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using QuestNavigator.Core.Authentication.Models;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;

namespace QuestNavigator.Core.Authentication.Controllers.Surface;

public class UmbAlternativeLoginController : SurfaceController
{
    private readonly MemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly ICoreScopeProvider _coreScopeProvider;
	private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

	[ActivatorUtilitiesConstructor]
    public UmbAlternativeLoginController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        MemberManager memberManager,
        IMemberService memberService,
        ICoreScopeProvider coreScopeProvider,
        IPublishedSnapshotAccessor publishedSnapshotAccessor)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _memberManager = memberManager;
        _memberService = memberService;
        _coreScopeProvider = coreScopeProvider;
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateUmbracoFormRouteString]
    public async Task<IActionResult> HandleLogin([Bind(new string[] { }, Prefix = "loginModel")] PasswordLessLoginModel model)
    {
        if (!base.ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        MergeRouteValuesToModel(model);

        var memberIdentityUser = await _memberManager.FindByNameAsync(model.Email);
        if (memberIdentityUser == null)
        {
            IdentityResult result = await RegisterMemberAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("loginModel", "Error on member registration");
                return CurrentUmbracoPage();
            }
            else
            {
                memberIdentityUser = await _memberManager.FindByNameAsync(model.Email);
                if (memberIdentityUser == null)
                {
                    ModelState.AddModelError("loginModel", "Error on member registration");
                    return CurrentUmbracoPage();
                }
            }
        }
        
        if (memberIdentityUser.IsLockedOut)
        {
            ModelState.AddModelError("loginModel", "Member is locked out");
            return CurrentUmbracoPage();
        }
        else if (!memberIdentityUser.IsApproved)
        {
            ModelState.AddModelError("loginModel", "Member is not allowed");
            return CurrentUmbracoPage();
        }


        var verificationUrl = await GetEmailVerificationUrlAsync(memberIdentityUser);

        TempData["verificationUrl"] = verificationUrl;

        var member = _memberService.GetByKey(memberIdentityUser.Key);

        var customMemberProperty = QuestNavigator.Umbraco.Models.Member.GetModelPropertyType(_publishedSnapshotAccessor, x => x.CustomProperty)!.Alias;

        member.SetValue(customMemberProperty, verificationUrl);

        _memberService.Save(member);

        return CurrentUmbracoPage();
    }

    private async Task<string> GetEmailVerificationUrlAsync(MemberIdentityUser memberIdentityUser)
    {
        var baseUrl = "https://localhost:44337";

        var token = await _memberManager.GenerateUserTokenAsync(memberIdentityUser, "Default", "passwordless");

        return $"{baseUrl}?token={WebUtility.UrlEncode(token)}&email={memberIdentityUser.Email}";
    }

    //
    // Summary:
    //     We pass in values via encrypted route values so they cannot be tampered with
    //     and merge them into the model for use
    //
    // Parameters:
    //   model:
    private void MergeRouteValuesToModel(PasswordLessLoginModel model)
    {
        if (RouteData.Values.TryGetValue("RedirectUrl", out object value) && value != null)
        {
            model.RedirectUrl = value.ToString();
        }
    }

    private async Task<IdentityResult> RegisterMemberAsync(PasswordLessLoginModel model)
    {
        using ICoreScope scope = _coreScopeProvider.CreateCoreScope(autoComplete: true);

        var identityUser =
            MemberIdentityUser.CreateNew(model.Email, model.Email, Constants.Conventions.MemberTypes.DefaultAlias, true);

        IdentityResult identityResult = await _memberManager.CreateAsync(identityUser);

        if (identityResult.Succeeded)
        {
            IMember? member = _memberService.GetByKey(identityUser.Key);
            if (member == null)
            {
                throw new InvalidOperationException($"Could not find a member with key: {member?.Key}.");
            }

            //Before we save the member we make sure to assign the group, for this the "Group" must exist in the backoffice.
            string memberGroup = "QuestMembers";

            AssignMemberGroup(model.Email, memberGroup);

            _memberService.Save(member);
        }

        return identityResult;
    }

    //Here we created a helper Method to assign a MemberGroup to a member.
    private void AssignMemberGroup(string email, string group)
    {
        try
        {
            _memberService.AssignRole(email, group);
        }
        catch (Exception ex)
        {
            //handle the exception
        }

    }
}
