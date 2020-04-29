﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PotatoBot.Models;

namespace DAL.Migrations
{
    [DbContext(typeof(GuildContext))]
    partial class GuildContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "5.0.0-preview.3.20181.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("PotatoBot.Models.GuildData", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal[]>("AutoRolesOnJoin")
                        .HasColumnType("numeric[]");

                    b.Property<bool>("EnableModLogs")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableWelcomeMessages")
                        .HasColumnType("boolean");

                    b.Property<string>("LeaveMessage")
                        .HasColumnType("text");

                    b.Property<List<GuildLog>>("Logs")
                        .HasColumnType("jsonb");

                    b.Property<decimal>("ModLogsChannel")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("MutedRoleId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Prefix")
                        .HasColumnType("text");

                    b.Property<string>("WelcomeMessage")
                        .HasColumnType("text");

                    b.Property<decimal>("WelcomeMessagesChannel")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("PotatoBot.Models.GuildLog", b =>
                {
                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<decimal>("AuthorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal?>("GuildDataId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<string>("TargetUser")
                        .HasColumnType("text");

                    b.HasIndex("GuildDataId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("PotatoBot.Models.GuildLog", b =>
                {
                    b.HasOne("PotatoBot.Models.GuildData", "GuildData")
                        .WithMany()
                        .HasForeignKey("GuildDataId");
                });
#pragma warning restore 612, 618
        }
    }
}
