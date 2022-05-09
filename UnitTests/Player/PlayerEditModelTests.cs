using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Players;
using HobbyTeamManager.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests.Player;

[TestFixture]
internal class PlayerEditModelTests
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
    public async Task OnGetAsync_PlayerNull_ReturnNotFound()
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
    public async Task OnGetAsync_PlayerIdNotFound_ReturnNotFound()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new EditModel(context);
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, 1);
        var expectedResult = new NotFoundResult();

        // act
        var actualResult = await page.OnGetAsync(3);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
    }

    [Test]
    public async Task OnGetAsync_PlayerIdFound_ReturnPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new EditModel(context);
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, 1);
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnGetAsync(1);

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
        page.Player = new Models.Player();
        page.ModelState.AddModelError("Error.Text", "Some error text.");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.MembershipTypeSL, Is.Not.Null);
    }

    [Test]
    public async Task OnPostAsync_ModelStateValid_ReturnsToIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<EditModel>(context);
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, 1);
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        page.Player = context.Players.First();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
        var expectedResult = new RedirectToPageResult("./Index");

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Player.Id, Is.EqualTo(1));
    }
}
