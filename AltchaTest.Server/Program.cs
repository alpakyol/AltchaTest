using AltchaTest.Server.Data;
using Ixnas.AltchaNet;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* ALTCHA */

var selfHostedKeyBase64 = "35Indc5qCcf5j4ykwn/znlubzXW3nBkod5PCHg6EQKN3unjrYCRxgqD2uoH8VKV8IbDvnagj0JcBAAzadW83wQ==";
var selfHostedKey = Convert.FromBase64String(selfHostedKeyBase64!);

// Initialize database using EF Core with SQLite in-memory.
var sqliteConnection = new SqliteConnection("datasource=:memory:");
sqliteConnection.Open();

// Add challenge store.
builder.Services.AddDbContext<ExampleDbContext>(options =>
                                                    options.UseSqlite(sqliteConnection));
builder.Services.AddScoped<IAltchaCancellableChallengeStore, AltchaChallengeStore>();

// Add Altcha services.
builder.Services.AddScoped(sp => Ixnas.AltchaNet.Altcha.CreateServiceBuilder()
                                       .UseSha256(selfHostedKey)
                                       //.UseInMemoryStore()
                                       .UseStore(sp.GetService<IAltchaCancellableChallengeStore>)
                                       .SetExpiry(AltchaExpiry.FromSeconds(60))
                                       .Build());
builder.Services.AddScoped(_ => Ixnas.AltchaNet.Altcha.CreateSolverBuilder()
                                      .Build());

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

/* END ALTCHA */

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
        builder
        .WithOrigins("https://localhost:62450")
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

// Create database table.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExampleDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
