namespace PlayZMachine
{
    using PlayZMachine.Commands;
    using PlayZMachine.Services;
    using Spectre.Console.Cli;
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

            AnsiConsole.MarkupLine("[underline red]ZorkBot[/] Welcome to an implementation of the Infocom Z-machine based largely on Mark's!");
            // these memory states may be too long to tweet, so we may need to throw them in storage for a period of time and expire them
            await app.RunAsync(args: new string[] { });
            return;
        }
    }
}