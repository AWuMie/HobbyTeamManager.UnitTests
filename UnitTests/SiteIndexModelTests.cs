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
        SeedNumSites(context, numSites);
        var page = new IndexModel(context);

        // act
        await page.OnGetAsync();

        // assert
        Assert.That(page.Site.Count, Is.EqualTo(numSites));
    }

    private void SeedNumSites(HobbyTeamManagerContext context, int num)
    {
        var sites = new List<Site>();

        for (int i = 0; i < num; i++)
        {
            var site = new Site()
            {
                Name = "Name" + (i + 1).ToString(),
                Headline = "Headline" + (i + 1).ToString(),
                ConfirmationModeId = 1,
                MenuPositionId = 1,
            };
            sites.Add(site);
        }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        context.Sites.AddRange(sites);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();
    }
}
