﻿using PotatoBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PotatoBot.Models
{
    public enum LogAction
    {
        Kick, Ban, Warn, Mute, Unmute, Unban
    }
    public class GuildLog
    {
        public GuildData GuildData { get; set; }
        public LogAction Action { get; set; }
        public string TargetUser { get; set; }
        public string Reason { get; set; }
        public ulong AuthorId { get; set; }
        public DateTime Date { get; set; }
    }

    public enum WarnAction
    {
        Kick, Ban, Mute1d, Mute2d, Mute3d, Mute7d
    }

    public class GuildData
    {
        public ulong Id { get; set; }
        public string Prefix { get; set; }

        [Column(TypeName = "jsonb")]
        public List<GuildLog> Logs { get; set; } = new List<GuildLog>();

        public List<Warn> Warns { get; set; }

        /*
        public bool EnableWarnAction { get; set; }
        public int WarnActionTreshold { get; set; }
        public WarnAction WarnActionType { get; set; }
        */

        public bool EnableModLogs { get; set; }
        public ulong ModLogsChannel { get; set; }

        public bool EnableMessageLogs { get; set; }
        public ulong MessageLogsChannel { get; set; }

        public ulong MutedRoleId { get; set; }

        public ulong[] AutoRolesOnJoin { get; set; }

        public bool EnableWelcomeMessages { get; set; }
        public ulong WelcomeMessagesChannel { get; set; }
        public string WelcomeMessage { get; set; } = "Welcome, **{user}**! You are the {members}. member.";
        public string LeaveMessage { get; set; } = "**{user}** left :(";
    }

}
