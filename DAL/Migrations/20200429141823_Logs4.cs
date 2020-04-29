﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using PotatoBot.Models;

namespace DAL.Migrations
{
    public partial class Logs4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<GuildLog>>(
                name: "Logs",
                table: "Guilds",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logs",
                table: "Guilds");
        }
    }
}
