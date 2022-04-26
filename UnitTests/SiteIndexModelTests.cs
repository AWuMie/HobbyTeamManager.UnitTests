using HobbyTeamManager.Data;
using HobbyTeamManager.Models;
using HobbyTeamManager.Pages.Sites;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using HobbyTeamManager.UnitTests.Utilities;

namespace HobbyTeamManager.UnitTests.UnitTests;

[TestFixture]
public class SiteIndexModelTests
{
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public async Task OnGetAsync_WhenCalled_SiteListFilled(int numSites)
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        Helpers.SeedNumSites(context, numSites);
        var page = new IndexModel(context);

        // act
        await page.OnGetAsync();

        // assert
        Assert.That(page.Site.Count, Is.EqualTo(numSites));
    }
}
