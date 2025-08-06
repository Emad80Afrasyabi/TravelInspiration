using ExternalDestinationSearch.API.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseHttpsRedirection();
 
List<DestinationDto> destinations = 
[
    new DestinationDto(name: "Antwerp, Belgium") { Description = "", ImageName = "antwerp.jpg" },
    new DestinationDto(name: "San Francisco, USA") { Description = "", ImageName = "sanfranciso.jpg" },
    new DestinationDto(name: "Sydney, Australia") { Description = "", ImageName = "sydney.jpg" },
    new DestinationDto(name: "Paris, France") { Description = "", ImageName = "paris.jpg" },
    new DestinationDto(name: "New Delhi, India") { Description = "", ImageName = "newdelhi.jpg" },
    new DestinationDto(name: "Tokyo, Japan") { Description = "", ImageName = "tokyo.jpg" },
    new DestinationDto(name: "Cape Town, South Africa") { Description = "", ImageName = "capetown.jpg" },
    new DestinationDto(name: "Barcelona, Spain") { Description = "", ImageName = "barcelona.jpg" },
    new DestinationDto(name: "Toronto, Canada") { Description = "", ImageName = "toronto.jpg" }
];

app.MapGet(pattern: "/destinations", (string? searchFor, HttpContext context) =>
{ 
    List<DestinationDto> filteredDestinations = destinations.Where(destination => searchFor == null ||
                                                                                  destination.Name.Contains(searchFor) || 
                                                                                  (destination.Description != null && destination.Description.Contains(searchFor))).ToList();

    foreach (DestinationDto destination in filteredDestinations)
        destination.ImageRootUri = $"{context.Request.Scheme}://{context.Request.Host}/images/";
    
    return Results.Ok(filteredDestinations);
});

app.Run();
