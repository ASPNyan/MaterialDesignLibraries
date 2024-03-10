using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExampleSite.Data;

public class IdentityDb(DbContextOptions<IdentityDb> options) : IdentityDbContext<User>(options);