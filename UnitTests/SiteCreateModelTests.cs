using HobbyTeamManager.Data;
using HobbyTeamManager.Pages;
using HobbyTeamManager.Pages.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests;

[TestFixture]
internal class SiteCreateModelTests
{
    [Test]
    public void OnGet_WhenCalled_InitializesDropdownLists()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Utilities.TestDbContextOptions());

        var page = new CreateModel(context);
        var expectedResult = new PageResult();

        // act
        var actualResult = page.OnGet();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ConfirmationModeOptions, Is.Not.Null);
        Assert.That(page.MenuPositionOptions, Is.Not.Null);
    }

    public async Task OnPostAsync_ModelStateInvalid_ReturnsToPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Utilities.TestDbContextOptions());

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new DummySession();
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var pageContext = new PageContext(actionContext) { ViewData = viewData };
        
        var page = new CreateModel(context)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };
        page.ModelState.AddModelError("Message.Text", "Some error text.");

        // SeedSiteInDb(context);
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    public T PageFromDummyContext<T>(HobbyTeamManagerContext context)
        where T : BasePageModel
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new DummySession();
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var pageContext = new PageContext(actionContext) { ViewData = viewData };

        var page = new BasePageModel(context)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };
        return (T)page;
    }
}
