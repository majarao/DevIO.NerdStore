using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Identity.API.Data;

public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : IdentityDbContext(options) { }
