using HobbyTeamManager.Data;
using HobbyTeamManager.Models;
using HobbyTeamManager.Pages.Seasons;
using HobbyTeamManager.UnitTests.Utilities;
using HobbyTeamManager.Utilities;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests.Season;

[TestFixture]
internal class SeasonIndexModelTests
{
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public async Task OnGetAsync_WhenCalled_SiteListFilled(int numSeasons)
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());

        var site = new Site();
        site.Id = 1;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        context.Sites.Add(site);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();

        Helpers.SeedNumSeasons(context, numSeasons);

        var selection = from s in context.Seasons select s;
        var expectedSeasons = selection.ToList();

        var page = Helpers.PageFromDummyHttpContext<IndexModel>(context);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Miscellaneous.SetSessionStringFromObject<Site>(site, page.HttpContext);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // act
        await page.OnGetAsync();

        // assert
        Assert.That(page.Season.Count, Is.EqualTo(numSeasons));
        Assert.That(page.Season.Count, Is.EqualTo(expectedSeasons.Count));
    }
}
