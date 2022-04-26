using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Sites;
using HobbyTeamManager.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests;

[TestFixture]
public class SiteDeleteModelTests
{
    [Test]
    public async Task OnGetAsync_IdNull_ReturnNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new EditModel(context);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(null);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_SiteNull_ReturnNotFound()
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
        Helpers.SeedNumSites(context, 3);
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
    public async Task OnPostAsync_SiteNotFound_NoSiteDeletedReDirecttoIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        int numSites = 3;
        Helpers.SeedNumSites(context, numSites);
        var expectedResult = new RedirectToPageResult("./Index");
        int id = 5;
#pragma warning disable CS8604 // Possible null reference argument.
        var expectedSites = context.Sites.Where(s => s.Id != id).ToList();
#pragma warning restore CS8604 // Possible null reference argument.

        // act
        var actualResult = await page.OnPostAsync(id);

        // assert
        var actualSites = context.Sites.AsNoTracking().ToList();

        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(actualSites.Count, Is.EqualTo(numSites));
        Assert.That(actualSites.Count, Is.EqualTo(expectedSites.Count));
        Assert.That(actualSites.OrderBy(s => s.Id).Select(s => s.Name),
            Is.EqualTo(expectedSites.OrderBy(s => s.Id).Select(s => s.Name)));
    }

    [Test]
    public async Task OnPostAsync_SiteFound_ReDirecttoIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        int numSites = 3;
        Helpers.SeedNumSites(context, numSites);
        var expectedResult = new RedirectToPageResult("./Index");
        int id = 2;
#pragma warning disable CS8604 // Possible null reference argument.
        var expectedSites = context.Sites.Where(s => s.Id != id).ToList();
#pragma warning restore CS8604 // Possible null reference argument.

        // act
        var actualResult = await page.OnPostAsync(id);

        // assert
        var actualSites = context.Sites.AsNoTracking().ToList();

        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(actualSites.Count, Is.EqualTo(numSites - 1));
        Assert.That(actualSites.Count, Is.EqualTo(expectedSites.Count));
        Assert.That(actualSites.OrderBy(s => s.Id).Select(s => s.Name),
            Is.EqualTo(expectedSites.OrderBy(s => s.Id).Select(s => s.Name)));
    }
}
