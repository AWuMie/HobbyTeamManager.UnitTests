using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Sites;
using HobbyTeamManager.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests;

[TestFixture]
public class SiteDetailsModelTests
{
    [Test]
    public async Task OnGetAsync_ParameterIdEqualsNull_ReturnsNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Helpers.TestDbContextOptions());
        var page = new DetailsModel(context);
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
        var page = new DetailsModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(1);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_SiteFoundWithId_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Helpers.TestDbContextOptions());
        Helpers.SeedSite(context);
        var page = new DetailsModel(context);
        var expectedResult = new PageResult();
        int id = 1;

        // act
        var actualResult = await page.OnGetAsync(id);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Site.Id, Is.EqualTo(id));
    }
}
