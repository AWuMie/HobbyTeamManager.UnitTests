using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Players;
using HobbyTeamManager.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests.Player;

[TestFixture]
internal class PlayerDeleteModelTests
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
    public async Task OnGetAsync_PlayerNull_ReturnNotFound()
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
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, 1);
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
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, 1);
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnGetAsync(1);

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
    public async Task OnPostAsync_PlayerNotFound_NoPlayerDeletedReDirectToIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        Helpers.SeedAllMembershipTypes(context);
        int numPlayers = 3;
        Helpers.SeedNumPlayers(context, numPlayers);
        var expectedResult = new RedirectToPageResult("./Index");
        int id = 5;
#pragma warning disable CS8604 // Possible null reference argument.
        var expectedPlayers = context.Players.Where(p => p.Id != id).ToList(); // should be all ;-)
#pragma warning restore CS8604 // Possible null reference argument.

        // act
        var actualResult = await page.OnPostAsync(id);

        // assert
        var actualPlayers = context.Players.AsNoTracking().ToList();

        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(actualPlayers.Count, Is.EqualTo(numPlayers));
        Assert.That(actualPlayers.Count, Is.EqualTo(expectedPlayers.Count));
        Assert.That(actualPlayers.OrderBy(p => p.Id).Select(p => p.FirstName),
            Is.EqualTo(expectedPlayers.OrderBy(p => p.Id).Select(p => p.FirstName)));
    }

    [Test]
    public async Task OnPostAsync_PlayerFound_ReDirectToIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new DeleteModel(context);
        Helpers.SeedAllMembershipTypes(context);
        int numPlayers = 3;
        Helpers.SeedNumPlayers(context, numPlayers);
        var expectedResult = new RedirectToPageResult("./Index");
        int id = 2;
#pragma warning disable CS8604 // Possible null reference argument.
        var expectedPlayers = context.Players.Where(p => p.Id != id).ToList();  // should one less here
#pragma warning restore CS8604 // Possible null reference argument.

        // act
        var actualResult = await page.OnPostAsync(id);

        // assert
        var actualPlayers = context.Players.AsNoTracking().ToList();

        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(actualPlayers.Count, Is.EqualTo(numPlayers - 1));
        Assert.That(actualPlayers.Count, Is.EqualTo(expectedPlayers.Count));
        Assert.That(actualPlayers.OrderBy(p => p.Id).Select(p => p.FirstName),
            Is.EqualTo(expectedPlayers.OrderBy(p => p.Id).Select(p => p.FirstName)));
    }
}
