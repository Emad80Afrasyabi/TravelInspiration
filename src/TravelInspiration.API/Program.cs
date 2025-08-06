using TravelInspiration.API;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddHttpClient();
   
builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
else app.UseExceptionHandler();

app.UseStatusCodePages();

app.Run();