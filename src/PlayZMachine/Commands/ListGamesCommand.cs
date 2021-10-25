using PlayZMachine.Enumerations;
using PlayZMachine.Maps;
using Spectre.Console;
using Spectre.Console.Cli;
using zmachine.Library.Models;

namespace PlayZMachine.Commands;

public class ListGamesCommand : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine("[green]GAMES[/][red]:[/]");
        int startingIndex = 6;
        string[] colors = { "red", "orange", "yellow", "green", "blue", "indigo", "violet" };
        string[] gameFiles = GameMap.Map.Values.Select(pair => pair.Item1).ToArray();
        int longestGameFile = gameFiles.Select(fileName => fileName.Length).Max();
        int longestGameName = Enum.GetNames(typeof(Game)).Select(gameName => gameName.Length).Max();
        foreach (KeyValuePair<Game, (string fileName, string description, int zmachineVersion)> pair in GameMap.Map)
        {
            if (!GameMap.Map.ContainsKey(pair.Key) || pair.Value.zmachineVersion > Machine.CurrentVersion)
            {
                continue;
            }

            AnsiConsole.MarkupLine(string.Format(" [{0}]- {1}[/] > {2} : {3}",
                colors[startingIndex],
                pair.Value.fileName.PadRight(longestGameFile),
                pair.Key.ToString().PadRight(longestGameName),
                pair.Value.description));

            startingIndex = (startingIndex + 1) % colors.Length;
        }

        return 0;
    }
}