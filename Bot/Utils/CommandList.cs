﻿using Bot.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PotatoBot.Bot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bot.Utils
{
    public class CommandArgumentData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public object Default { get; set; }
        public bool Optional { get; set; }
        public bool CatchAll { get; set; }
    }
    public class CommandData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<string> Aliases { get; set; }
        public IEnumerable<IEnumerable<CommandArgumentData>> Overloads { get; set; }
    }
    public class CommandList
    {
        public Dictionary<string, List<CommandData>> list = new Dictionary<string, List<CommandData>>();
        public CommandList(CommandsNextExtension commands)
        {
            foreach (var cmd in commands.RegisteredCommands.Values.GroupBy(i=>i.Name).Select(y=>y.First()).ToList())
            {
                var type = cmd.Module.ModuleType;
                var attributeRaw = type.GetCustomAttributes(true)
                    .FirstOrDefault(x => x is CategoryAttribute);

                var attribute = attributeRaw as CategoryAttribute;
                var cat = attribute?.Name ?? "uncategorized";


                var data = new CommandData
                {
                    Name = cmd.Name,
                    Description = cmd.Description,
                    Aliases = cmd.Aliases,
                    Overloads = cmd.Overloads.Select(o => o.Arguments.Select(a =>
                          new CommandArgumentData
                          {
                              Name = a.Name,
                              Description = a.Description,
                              Type = a.Type.Name,
                              Default = a.DefaultValue,
                              Optional = a.IsOptional,
                              CatchAll = a.IsCatchAll
                          }))
                };
                AddCommand(data, cat);
                
            }
        }

        private void AddCommand(CommandData cmd, string category)
        {
            if (list.ContainsKey(category))
            {
                list[category].Add(cmd);
            }
            else
            {
                list.Add(category, new List<CommandData> { cmd });
            }
        }
    }
}
