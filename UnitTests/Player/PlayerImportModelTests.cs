using HobbyTeamManager.Data;
using HobbyTeamManager.Pages.Players;
using HobbyTeamManager.UnitTests.Utilities;
using HobbyTeamManager.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HobbyTeamManager.UnitTests.UnitTests.Player;

[TestFixture]
internal class PlayerImportModelTests
{
    private const string _playerNoJsonData = "This is not a JSON file with Player data";
    private const string _playerJsonCorrectData =
        "{ \"Players\": [ { \"FirstName\": \"Name\", \"Emailaddress\": \"email@address.com\", \"MembershipTypeId\": 1 } ] }";
    private const string _playerJsonNoFirstName =
        "{ \"Players\": [ { \"Emailaddress\": \"email@address.com\", \"MembershipTypeId\": 1 } ] }";
    private const string _playerJsonIncorrectEmail =
        "{ \"Players\": [ { \"FirstName\": \"Name\", \"Emailaddress\": \"emailaddresscom\", \"MembershipTypeId\": 1 } ] }";
    private const string _playerJsonMembershipIdZero =
        "{ \"Players\": [ { \"FirstName\": \"Name\", \"Emailaddress\": \"email@address.com\", \"MembershipTypeId\": 0 } ] }";
    private const string _playerJsonMembershipIdFour =
        "{ \"Players\": [ { \"FirstName\": \"Name\", \"Emailaddress\": \"email@address.com\", \"MembershipTypeId\": 4 } ] }";

    [Test]
    public void OnPostUpload_FormFileJsonIsNull_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<ImportModel>(context);
        var expectedResult = new PageResult();

        // act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var actualResult = page.OnPostUpload(null);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ViewData[ImportModel.errorMessage], Is.EqualTo("Keine Datei gefunden!"));
    }

    [Test]
    public void OnPostUpload_FormFileJsonIsEmpty_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<ImportModel>(context);

        var fileName = "dummy.txt";
        IFormFile formFile = MockFormFileFromJsonString("", fileName);
        var expectedResult = new PageResult();

        // act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var actualResult = page.OnPostUpload(formFile);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ViewData[ImportModel.errorMessage], Is.EqualTo($"Datei \"{fileName}\" konnte nicht gelesen werden!"));
    }

    [Test]
    public void OnPostUpload_FormFileJsonIsNotConvertible_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<ImportModel>(context);
        var fileName = "dummy.txt";
        IFormFile formFile = MockFormFileFromJsonString(_playerNoJsonData, fileName);
        var expectedResult = new PageResult();

        // act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var actualresult = page.OnPostUpload(formFile);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        
        // assert
        Assert.That(actualresult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ViewData[ImportModel.errorMessage], Is.EqualTo($"Datei \"{fileName}\" konnte nicht gelesen werden!"));

        // Newtonsoft.Json.JsonReaderException
        //var ex = Assert.Throws<Newtonsoft.Json.JsonReaderException>(() => page.OnPostUpload(formFile));
        //Assert.That(ex.Message, Is.EqualTo("Unexpected character encountered while parsing value: T. Path '', line 0, position 0."));
    }

    [Test]
    [TestCase(_playerJsonNoFirstName)]
    [TestCase(_playerJsonIncorrectEmail)]
    [TestCase(_playerJsonMembershipIdZero)]
    [TestCase(_playerJsonMembershipIdFour)]
    public void OnPostUpload_PlayerFromJsonNotAllPropertiesOK_ReturnsPage(string jsonString)
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<ImportModel>(context);
        var fileName = "dummy.txt";
        IFormFile formFile = MockFormFileFromJsonString(jsonString, fileName);
        var expectedResult = new PageResult();

        // act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var actualresult = page.OnPostUpload(formFile);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // assert
        Assert.That(actualresult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ViewData[ImportModel.errorMessage], Is.EqualTo("Nicht alle Spielerdaten erfüllen die Erfordernisse!"));
    }

    [Test]
    public void OnPostUpload_FormFileJsonIsConvertible_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<ImportModel>(context);

        var fileName = "dummy.txt";
        IFormFile formFile = MockFormFileFromJsonString(_playerJsonCorrectData, fileName);

        var expectedResult = new PageResult();

        // act
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var actualresult = page.OnPostUpload(formFile);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // assert
        Assert.That(actualresult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ViewData[ImportModel.successMessage], Is.EqualTo($"Datei \"{fileName}\" geladen"));
        Assert.That(page.Players.Count, Is.EqualTo(1));
        Assert.That(page.Players[0].FirstName, Is.EqualTo("Name"));
    }

    [Test]
    public async Task OnPostSaveAsync_ModelStateInvalid_ReturnsPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        var page = Helpers.PageFromDummyHttpContext<ImportModel>(context);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        page.ModelState.AddModelError("Error.Text", "Some error text.");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnPostSaveAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.Players, Is.Null);
    }

    [Test]
    public async Task OnPostSaveAsync_ImportedDataWithDublicatedMailAddress_ReturnsPage()
    {
        // arrange
        List<Models.Player> players = new List<Models.Player>();
        string firstName = "FirstName1";
        string lastName = "LastName1";
        var player = new Models.Player()
        {
            FirstName = firstName,
            LastName = lastName,
            Emailaddress = $"{firstName}@{lastName}.com",
            MembershipTypeId = 1,
        };
        players.Add(player);

        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, 1);
        var page = Helpers.PageFromDummyHttpContext<ImportModel>(context);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Miscellaneous.SetSessionStringFromObject<List<Models.Player>>(players, page.HttpContext);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var expectedResult = new PageResult();

        // act
        var actualResult = await page.OnPostSaveAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
        Assert.That(page.ViewData[ImportModel.errorMessage], Is.EqualTo("Doppelte Emailadressen: \"FirstName1@LastName1.com\" , !"));
    }

    [Test]
    public async Task OnPostSaveAsync_ImportedDataOk_ReturnsIndexPage()
    {
        // arrange
        using var context = new HobbyTeamManagerContext(Helpers.TestDbContextOptions());
        Helpers.SeedAllMembershipTypes(context);
        Helpers.SeedNumPlayers(context, 1);

        List<Models.Player> players = new List<Models.Player>();
        string firstName = "FirstName2";
        string lastName = "LastName2";
        var player = new Models.Player()
        {
            FirstName = firstName,
            LastName = lastName,
            Emailaddress = $"{firstName}@{lastName}.com",
            MembershipTypeId = 1,
        };
        players.Add(player);

        var page = Helpers.PageFromDummyHttpContext<ImportModel>(context);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Miscellaneous.SetSessionStringFromObject<List<Models.Player>>(players, page.HttpContext);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        var expectedResult = new RedirectToPageResult("./Index");

        // act
        var actualResult = await page.OnPostSaveAsync();

        // assert
        Assert.That(actualResult, Is.TypeOf(expectedResult.GetType()));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        player = context.Players.Find(2);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        Assert.That(player, Is.Not.Null);
    }

    private IFormFile MockFormFileFromJsonString(string jsonString, string fileName)
    {
        var bytes = Encoding.UTF8.GetBytes(jsonString);
        return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", fileName);
    }
}
