using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestCoastEducation.web.Models;

namespace WestCoastEducation.web.Data;

public class WestCoastEducationContext : IdentityDbContext
{
    public WestCoastEducationContext(DbContextOptions options) : base(options) { }
}
