﻿namespace PlayZMachine
{
    using PlayZMachine.Commands;
    using PlayZMachine.Services;
    using Spectre.Cli;
    using Spectre.Console;

    internal class Program
    {
        private static readonly ZorkBotService ZorkBot;

        static Program()
        {
            ZorkBot = new ZorkBotService();
        }

        private static async Task Main(string[] argv)
        {
            CommandApp? app = new CommandApp();
            app.Configure(config =>
            {
#if DEBUG
                config.PropagateExceptions();
                config.ValidateExamples();
#endif
                config
                    .AddCommand<ListGamesCommand>("games")
                    .WithAlias("g")
                    .WithDescription("List games");

                config
                    .AddCommand<IterateGameCommand>("play")
                    .WithAlias("p")
                    .WithDescription("Play a game")
                    .WithExample(new[] { "zork1" });
            });

            AnsiConsole.Markup("[underline red]Hello[/] World!");

            // these memory states may be too long to tweet, so we may need to throw them in storage for a period of time and expire them
            string? output = await ZorkBot.Iterate(
                game: Game.Zork1,
                state: null,
                initialInput: "",
                untilInput: true);
        }
    }
}