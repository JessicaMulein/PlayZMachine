using PlayZMachine.Commands;
using PlayZMachine.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PlayZMachine;

internal class Program
{
    private static readonly ZorkBotService ZorkBot;

    static Program()
    {
        ZorkBot = new ZorkBotService();
    }

    private static async Task Main(string[] args)
    {
        CommandApp app = new CommandApp();
        app.Configure(config =>
        {
#if DEBUG
            config.PropagateExceptions();
            config.ValidateExamples();
#endif
            config
                .AddCommand<LogWatchCommand>("watch")
                .WithAlias("w")
                .WithDescription("Continuous log monitoring of the Twitter Bot");

            config
                .AddCommand<ListGamesCommand>("games")
                .WithAlias("g")
                .WithDescription("List games");

            config
                .AddCommand<LocalGameCommand>("local")
                .WithAlias("l")
                .WithDescription("Play a game locally");
        });

        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine(
            "[underline red]ZorkBot[/] Welcome to an implementation of the Infocom Z-machine based largely on Mark's!");
        AnsiConsole.MarkupLine(" [red]-[/] [green]Background Twitter Bot[/]");
        AnsiConsole.MarkupLine(" [red]-[/] [blue]Local Game[/]");
        AnsiConsole.MarkupLine(" [red]-[/] [purple]Admin Console[/]");
        AnsiConsole.MarkupLine("---------------------------------------------------------------------------");

        // these memory states may be too long to tweet, so we may need to throw them in storage for a period of time and expire them
        await app.RunAsync(args);
    }
}