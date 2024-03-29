﻿using Microsoft.EntityFrameworkCore;
using TournamentManager.Domain;

namespace TournamentManager.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Poule> Poules { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Set> Sets { get; set; }
}
