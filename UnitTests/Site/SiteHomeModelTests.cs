using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;
using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Sites;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HobbyTeamManager.UnitTests.Utilities;

namespace HobbyTeamManager.UnitTests.UnitTests;

[TestFixture]
internal class SiteHomeModelTests
{
    [Test]
    public async Task OnGetAsync_ParameterIdEqualsNull_ReturnsNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Helpers.TestDbContextOptions());
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
        using var context = new HobbyTeamManagerContext(Utilities.Helpers.TestDbContextOptions());
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
        using var context = new HobbyTeamManagerContext(Utilities.Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<HomeModel>(context);
        SeedSiteInDb(context);
        var expectedResult = new PageResult();

        // act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var actualResult = await page.OnGetAsync(1);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    private void SeedSiteInDb(HobbyTeamManagerContext context)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        context.Sites.Add(new Models.Site());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();
    }
}
