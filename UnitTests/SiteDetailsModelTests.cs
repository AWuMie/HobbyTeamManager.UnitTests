using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Sites;
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
        using var context = new HobbyTeamManagerContext(Utilities.Utilities.TestDbContextOptions());

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
        using var context = new HobbyTeamManagerContext(Utilities.Utilities.TestDbContextOptions());

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
        using var context = new HobbyTeamManagerContext(Utilities.Utilities.TestDbContextOptions());

        SeedSite(context);
        var page = new DetailsModel(context);
        var expectedResult = new PageResult();
        int id = 1;

        // act
        var actualResult = await page.OnGetAsync(id);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Site.Id, Is.EqualTo(id));
    }

    private void SeedSite(HobbyTeamManagerContext context)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _ = context.Sites.Add(new Models.Site());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();
    }
}
