using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Identity.API.Data;

public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext(options) { }
