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
internal class PlayerCreateModelTests
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
        Assert.That(page.MembershipTypeSL, Is.Not.Null);
    }

    [Test]
    public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<CreateModel>(context);
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
        var page = Helpers.PageFromDummyHttpContext<CreateModel>(context);
        Helpers.SeedAllMembershipTypes(context);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        page.Player = new Models.Player();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var expectedResult = new RedirectToPageResult("./Index");

        // act
        var actualResult = await page.OnPostAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Player.Id, Is.EqualTo(1));
    }
}
