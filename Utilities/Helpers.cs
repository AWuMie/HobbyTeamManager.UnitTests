using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HobbyTeamManager.Data;
using HobbyTeamManager.Pages;
using Microsoft.AspNetCore.Http;
using HobbyTeamManager.UnitTests.UnitTests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using HobbyTeamManager.Models;

namespace HobbyTeamManager.UnitTests.Utilities;

// //////////
// See there:
// https://docs.microsoft.com/en-us/aspnet/core/test/razor-pages-tests?view=aspnetcore-6.0
// //////////
public static class Helpers
{
    public static DbContextOptions<HobbyTeamManagerContext> TestDbContextOptions()
    {
        // Create a new service provider to create a new in-memory database.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance using an in-memory database and 
        // IServiceProvider that the context should resolve all of its 
        // services from.
        var builder = new DbContextOptionsBuilder<HobbyTeamManagerContext>()
            .UseInMemoryDatabase("InMemoryDb")
            .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    public static T? PageFromDummyHttpContext<T>(HobbyTeamManagerContext context)
        where T : BasePageModel
    {
        var httpContext = new DefaultHttpContext()
        {
            Session = new DummySession(),
        };
        var dic = new Dictionary<string, StringValues>();
        httpContext.Request.Form = new FormCollection(dic);

        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        var pageContext = new PageContext(actionContext) { ViewData = viewData };

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        T page = (T?)Activator.CreateInstance(typeof(T), new object[] { context });
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        page.PageContext = pageContext;
        page.TempData = tempData;
        page.Url = new UrlHelper(actionContext);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        return page;
    }

    public static void SeedNumSites(HobbyTeamManagerContext context, int num)
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

    public static void SeedNumSeasons(HobbyTeamManagerContext context, int num)
    {
        var seasons = new List<Season>();

        for (int i = 0; i < num; i++)
        {
            var season = new Season()
            {
                Year = 2020 + (i + 1),
                StartMonth = 8,
                MatchOnDay = 0,
                SiteId = 1,
            };
            seasons.Add(season);
        }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        context.Seasons.AddRange(seasons);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();
    }

    public static void SeedAllMembershipTypes(HobbyTeamManagerContext context)
    {
        var mstMember = new MembershipType()
        {
            Name = MembershipType.Member,
        };
        var mstGuest = new MembershipType()
        {
            Name = MembershipType.Guest,
        };
        var mstEx = new MembershipType()
        {
            Name = MembershipType.Ex,
        };
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        context.MembershipTypes.Add(mstMember);
        context.MembershipTypes.Add(mstGuest);
        context.MembershipTypes.Add(mstEx);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();
    }

    public static void SeedNumPlayers(HobbyTeamManagerContext context, int num)
    {
        var players = new List<Player>();

        for (int i = 0; i < num; i++)
        {
            string firstName = "FirstName" + (i + 1).ToString();
            string lastName = "LastName" + (i + 1).ToString();

            var player = new Player()
            {
                FirstName = firstName,
                LastName = lastName,
                Emailaddress = $"{firstName}@{lastName}.com",
                MembershipTypeId = 1,
            };
            players.Add(player);
        }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        context.Players.AddRange(players);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        context.SaveChanges();
    }
}
