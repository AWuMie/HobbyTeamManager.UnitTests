using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;
using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Sites;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;

namespace HobbyTeamManager.UnitTests.UnitTests;

[TestFixture]
public class SiteHomeModelTests
{
    [Test]
    public async Task OnGetAsync_ParameterIdEqualsNull_ReturnsNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Utilities.TestDbContextOptions());

        var page = new HomeModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(null);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_NoSiteWithId_ReturnsNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Utilities.TestDbContextOptions());

        var page = new HomeModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(1);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_SiteFoundInDbWithId_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Utilities.TestDbContextOptions());

        var httpContext = new DefaultHttpContext
        {
            Session = new DummySession()
        };
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext,new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var pageContext = new PageContext(actionContext) { ViewData = viewData };
        var page = new HomeModel(context)
        {
            PageContext = pageContext,
            TempData = tempData,
            Url = new UrlHelper(actionContext)
        };

        SeedSiteInDb(context);
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnGetAsync(1);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    private void SeedSiteInDb(HobbyTeamManagerContext context)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _ = context.Sites.Add(new Models.Site());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();
    }
}
