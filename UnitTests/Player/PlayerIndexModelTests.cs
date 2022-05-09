using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Players;
using HobbyTeamManager.UnitTests.Utilities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests.Player;

[TestFixture]
internal class PlayerIndexModelTests
{
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public async Task OnGetAsync_WhenCalled_PlayerListFilled(int numPlayers)
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, numPlayers);
        var page = new IndexModel(context);

        // act
        await page.OnGetAsync();

        // assert
        Assert.That(page.Player.Count, Is.EqualTo(numPlayers));
    }
}
