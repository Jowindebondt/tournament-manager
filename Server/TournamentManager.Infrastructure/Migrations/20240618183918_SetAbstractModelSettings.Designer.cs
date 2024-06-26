﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TournamentManager.Infrastructure;

#nullable disable

namespace TournamentManager.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240618183918_SetAbstractModelSettings")]
    partial class SetAbstractModelSettings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PlayerPoule", b =>
                {
                    b.Property<int>("PlayersId")
                        .HasColumnType("int");

                    b.Property<int>("PoulesId")
                        .HasColumnType("int");

                    b.HasKey("PlayersId", "PoulesId");

                    b.HasIndex("PoulesId");

                    b.ToTable("PlayerPoule");
                });

            modelBuilder.Entity("TournamentManager.Domain.Game", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MatchId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Score_1")
                        .HasColumnType("int");

                    b.Property<int>("Score_2")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("TournamentManager.Domain.Match", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Player1Id")
                        .HasColumnType("int");

                    b.Property<int>("Player2Id")
                        .HasColumnType("int");

                    b.Property<int>("PouleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.HasIndex("PouleId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("TournamentManager.Domain.Member", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int>("Class")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("TournamentManager.Domain.Player", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("TournamentManager.Domain.Poule", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoundId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoundId");

                    b.ToTable("Poules");
                });

            modelBuilder.Entity("TournamentManager.Domain.Round", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("TournamentManager.Domain.RoundSettings", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("nvarchar(34)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RoundId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoundId")
                        .IsUnique()
                        .HasFilter("[RoundId] IS NOT NULL");

                    b.ToTable("RoundSettings");

                    b.HasDiscriminator<string>("Discriminator").HasValue("RoundSettings");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TournamentManager.Domain.Tournament", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sport")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("TournamentManager.Domain.TournamentSettings", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TournamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId")
                        .IsUnique()
                        .HasFilter("[TournamentId] IS NOT NULL");

                    b.ToTable("TournamentSettings");

                    b.HasDiscriminator<string>("Discriminator").HasValue("TournamentSettings");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TournamentManager.Domain.TableTennisRoundSettings", b =>
                {
                    b.HasBaseType("TournamentManager.Domain.RoundSettings");

                    b.Property<int>("BestOf")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("TableTennisRoundSettings");
                });

            modelBuilder.Entity("TournamentManager.Domain.TableTennisSettings", b =>
                {
                    b.HasBaseType("TournamentManager.Domain.TournamentSettings");

                    b.Property<int>("Handicap")
                        .HasColumnType("int");

                    b.Property<int>("TournamentType")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("TableTennisSettings");
                });

            modelBuilder.Entity("PlayerPoule", b =>
                {
                    b.HasOne("TournamentManager.Domain.Player", null)
                        .WithMany()
                        .HasForeignKey("PlayersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TournamentManager.Domain.Poule", null)
                        .WithMany()
                        .HasForeignKey("PoulesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TournamentManager.Domain.Game", b =>
                {
                    b.HasOne("TournamentManager.Domain.Match", "Match")
                        .WithMany("Games")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("TournamentManager.Domain.Match", b =>
                {
                    b.HasOne("TournamentManager.Domain.Player", "Player1")
                        .WithMany("MatchesAsPlayer1")
                        .HasForeignKey("Player1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TournamentManager.Domain.Player", "Player2")
                        .WithMany("MatchesAsPlayer2")
                        .HasForeignKey("Player2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TournamentManager.Domain.Poule", "Poule")
                        .WithMany("Matches")
                        .HasForeignKey("PouleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player1");

                    b.Navigation("Player2");

                    b.Navigation("Poule");
                });

            modelBuilder.Entity("TournamentManager.Domain.Member", b =>
                {
                    b.HasOne("TournamentManager.Domain.Player", "Player")
                        .WithMany("Members")
                        .HasForeignKey("PlayerId");

                    b.HasOne("TournamentManager.Domain.Tournament", "Tournament")
                        .WithMany("Members")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentManager.Domain.Poule", b =>
                {
                    b.HasOne("TournamentManager.Domain.Round", "Round")
                        .WithMany("Poules")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Round");
                });

            modelBuilder.Entity("TournamentManager.Domain.Round", b =>
                {
                    b.HasOne("TournamentManager.Domain.Tournament", "Tournament")
                        .WithMany("Rounds")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentManager.Domain.RoundSettings", b =>
                {
                    b.HasOne("TournamentManager.Domain.Round", "Round")
                        .WithOne("Settings")
                        .HasForeignKey("TournamentManager.Domain.RoundSettings", "RoundId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Round");
                });

            modelBuilder.Entity("TournamentManager.Domain.TournamentSettings", b =>
                {
                    b.HasOne("TournamentManager.Domain.Tournament", "Tournament")
                        .WithOne("Settings")
                        .HasForeignKey("TournamentManager.Domain.TournamentSettings", "TournamentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("TournamentManager.Domain.Match", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("TournamentManager.Domain.Player", b =>
                {
                    b.Navigation("MatchesAsPlayer1");

                    b.Navigation("MatchesAsPlayer2");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("TournamentManager.Domain.Poule", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("TournamentManager.Domain.Round", b =>
                {
                    b.Navigation("Poules");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("TournamentManager.Domain.Tournament", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Rounds");

                    b.Navigation("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
