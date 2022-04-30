using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Sites;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Threading.Tasks;
using HobbyTeamManager.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HobbyTeamManager.UnitTests.UnitTests;

[TestFixture]
internal class SiteCreateModelTests
{
    [Test]
    public void OnGet_WhenCalled_InitializesDropdownListsAndReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Utilities.Helpers.TestDbContextOptions());
        var page = new CreateModel(context);
        var expectedResult = new PageResult();

        // act
        var actualResult = page.OnGet();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ConfirmationModeOptions, Is.Not.Null);
        Assert.That(page.MenuPositionOptions, Is.Not.Null);
    }

    [Test]
    public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<CreateModel>(context);
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
        var page = Helpers.PageFromDummyHttpContext<CreateModel>(context);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        page.Site = new Models.Site();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var expectedResult = new RedirectToPageResult("./Index");

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Site.Id, Is.EqualTo(1));
    }
}
