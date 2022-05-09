using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Seasons;
using HobbyTeamManager.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests.Season;

[TestFixture]
internal class SeasonDetailsModelTests
{
    [Test]
    public async Task OnGetAsync_IdNull_ReturnsNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DetailsModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(null);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_NoSeasonWithId_ReturnsNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DetailsModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(1);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_SeasonFoundWithId_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        Helpers.SeedNumSites(context, 1);
        Helpers.SeedNumSeasons(context, 1);
        var page = new DetailsModel(context);
        var expectedResult = new PageResult();
        int id = 1;

        // act
        var actualResult = await page.OnGetAsync(id);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.That(page.Season.Id, Is.EqualTo(id));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}
