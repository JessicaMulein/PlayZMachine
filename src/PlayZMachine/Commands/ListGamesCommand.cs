
namespace PlayZMachine.Commands
{
    using PlayZMachine.Maps;
    using Spectre.Console;
    using Spectre.Console.Cli;

    public class ListGamesCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            AnsiConsole.MarkupLine("");
            AnsiConsole.MarkupLine("[green]GAMES[/][red]:[/]");
            AnsiConsole.MarkupLine("");
            int startingIndex = 6;
            string[] colors = new string[] { "red", "orange", "yellow" , "green", "blue", "indigo", "violet" };
            string[] gameFiles = GameMap.Map.Values.Select(pair => pair.Item1).ToArray();
            int longestGameFile = gameFiles.Select(fileName => fileName.Length).Max();
            int longestGameName = Enum.GetNames(typeof(Game)).Select(gameName => gameName.Length).Max();
            foreach (var pair in GameMap.Map)
            {
                AnsiConsole.MarkupLine(String.Format(" [{0}]- {1}[/] > {2} : {3}", colors[startingIndex], pair.Value.Item1.ToString().PadRight(longestGameFile), pair.Key.ToString().PadRight(longestGameName), pair.Value.Item2));
                startingIndex = (startingIndex + 1) % colors.Length;
            }
            return 0;
        }
    }
}
