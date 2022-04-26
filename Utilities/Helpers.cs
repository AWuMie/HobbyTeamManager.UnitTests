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
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

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
        //var httpRequest = new HttpRequest("", "http://example.com/", "");
        //var stringWriter = new StringWriter();
        //var httpResponse = new HttpResponse(stringWriter);
        //var httpContext = new HttpContext(httpRequest, httpResponse);

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
}
