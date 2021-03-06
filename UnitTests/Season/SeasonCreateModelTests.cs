using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Seasons;
using HobbyTeamManager.UnitTests.Utilities;
using HobbyTeamManager.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests.Season;

[TestFixture]
internal class SeasonCreateModelTests
{
    [Test]
    public void OnGet_WhenCalled_InitializesDropdownListsAndReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = new CreateModel(context);
        var expectedResult = new PageResult();

        // act
        var actualResult = page.OnGet();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.YearSL, Is.Not.Null);
        Assert.That(page.MonthSL, Is.Not.Null);
        Assert.That(page.WeekDaySL, Is.Not.Null);
    }

    [Test]
    public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<CreateModel>(context);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        page.Season = new Models.Season();
        page.ModelState.AddModelError("Error.Text", "Some error text.");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.YearSL, Is.Not.Null);
        Assert.That(page.MonthSL, Is.Not.Null);
        Assert.That(page.WeekDaySL, Is.Not.Null);
    }

    [Test]
    public async Task OnPostAsync_ModelStateValid_ReturnsToIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<CreateModel>(context);

        var site = new Models.Site();
        site.Id = 1;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        context.Sites.Add(site);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();
        context.Entry(site).State = EntityState.Detached;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Miscellaneous.SetSessionStringFromObject<Models.Site>(site, page.HttpContext);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        page.Season = new Models.Season();
        // arbitrary values!
        page.SelectedYear = 2022;
        page.SelectedMonth = 8;
        page.SelectedWeekDay = 5;

        var expectedResult = new RedirectToPageResult("./Index");

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Season.Id, Is.EqualTo(1));
        Assert.That(page.Season.MatchDays, Is.Not.Null);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Assert.That(page.Season.MatchDays.Count, Is.AtLeast(52));
        Assert.That(page.Season.MatchDays.Count, Is.AtMost(53));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}
