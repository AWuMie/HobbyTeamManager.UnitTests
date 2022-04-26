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
public class SiteEditModelTests
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
        var page = new EditModel(context);
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
        var page = new EditModel(context);
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
        var page = new EditModel(context);
        Helpers.SeedNumSites(context, 3);
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnGetAsync(3);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<EditModel>(context);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        page.Site = new Models.Site();
        page.ModelState.AddModelError("Error.Text", "Some error text.");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ConfirmationModeOptions, Is.Not.Null);
        Assert.That(page.MenuPositionOptions, Is.Not.Null);
    }

    [Test]
    public async Task OnPostAsync_ModelStateValid_ReturnsToIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<EditModel>(context);
        Helpers.SeedNumSites(context, 1);
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        page.Site = context.Sites.First();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
        var expectedResult = new RedirectToPageResult("./Index");

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Site.Id, Is.EqualTo(1));
    }
}
