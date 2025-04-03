using Altcha.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace AltchaTest.Server.Data;

internal class ExampleDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<VerifiedChallenge> VerifiedChallenges { get; set; }
}
