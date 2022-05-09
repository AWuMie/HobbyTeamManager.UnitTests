using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Players;
using HobbyTeamManager.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests.Player;

[TestFixture]
internal class PlayerDetailsModelTests
{
    [Test]
    public async Task OnGetAsync_ParameterIdEqualsNull_ReturnsNotFound()
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
    public async Task OnGetAsync_NoPlayerWithId_ReturnsNotFound()
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
    public async Task OnGetAsync_PlayerFoundWithId_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, 1);
        var page = new DetailsModel(context);
        var expectedResult = new PageResult();
        int id = 1;

        // act
        var actualResult = await page.OnGetAsync(id);

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Player.Id, Is.EqualTo(id));
    }
}
