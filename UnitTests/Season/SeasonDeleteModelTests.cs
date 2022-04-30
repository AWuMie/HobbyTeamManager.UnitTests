using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Seasons;
using HobbyTeamManager.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests;

[TestFixture]
public class SeasonDeleteModelTests
{
    [Test]
    public async Task OnGetAsync_IdNull_ReturnNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(null);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_SeasonNull_ReturnNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(1);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_IdNotFound_ReturnNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        Helpers.SeedSite(context);
        Helpers.SeedSeason(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(3);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_IdFound_ReturnPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        Helpers.SeedSite(context);
        Helpers.SeedNumSeasons(context, 3);
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnGetAsync(3);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnPostAsync_IdNull_ReturnNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnPostAsync(null);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnPostAsync_SeasonNotFound_NoSeasonDeletedReDirectToIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        Helpers.SeedSite(context);
        int numSeasons = 3;
        Helpers.SeedNumSeasons(context, numSeasons);
        var expectedResult = new RedirectToPageResult("./Index");
        int id = 5;
#pragma warning disable CS8604 // Possible null reference argument.
        var expectedSeasons = context.Seasons.Where(s => s.Id != id).ToList(); // should be all ;-)
#pragma warning restore CS8604 // Possible null reference argument.

        // act
        var actualResult = await page.OnPostAsync(id);

        // assert
        var actualSeasons = context.Seasons.AsNoTracking().ToList();

        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(actualSeasons.Count, Is.EqualTo(numSeasons));
        Assert.That(actualSeasons.Count, Is.EqualTo(expectedSeasons.Count));
        Assert.That(actualSeasons.OrderBy(s => s.Id).Select(s => s.Year),
            Is.EqualTo(expectedSeasons.OrderBy(s => s.Id).Select(s => s.Year)));
    }

    [Test]
    public async Task OnPostAsync_SeasonFound_ReDirectToIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        Helpers.SeedSite(context);
        int numSeasons = 3;
        Helpers.SeedNumSeasons(context, numSeasons);
        var expectedResult = new RedirectToPageResult("./Index");
        int id = 2;
#pragma warning disable CS8604 // Possible null reference argument.
        var expectedSeasons = context.Seasons.Where(s => s.Id != id).ToList();  // should one less here
#pragma warning restore CS8604 // Possible null reference argument.

        // act
        var actualResult = await page.OnPostAsync(id);

        // assert
        var actualSeasons = context.Seasons.AsNoTracking().ToList();

        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(actualSeasons.Count, Is.EqualTo(numSeasons - 1));
        Assert.That(actualSeasons.Count, Is.EqualTo(expectedSeasons.Count));
        Assert.That(actualSeasons.OrderBy(s => s.Id).Select(s => s.Year),
            Is.EqualTo(expectedSeasons.OrderBy(s => s.Id).Select(s => s.Year)));
    }
}
