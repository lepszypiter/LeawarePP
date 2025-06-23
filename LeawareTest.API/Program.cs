using LeawareTest.API.BackgroundServices;
using LeawareTest.Application;
using LeawareTest.Infrastructure;
using LeawareTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.RegisterLeawareTestInfrastructureServices(builder.Configuration);
builder.Services.RegisterLeawareTestApplication();

builder.Services.Configure<EmailCheckSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddHostedService<EmailCheckService>();


var app = builder.Build();

UpdateDb(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.Run();


void UpdateDb(WebApplication webApplication)
{
    using var scope = webApplication.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception e)
    {
        logger.LogError(e, e.Message);
        throw;
    }
}

