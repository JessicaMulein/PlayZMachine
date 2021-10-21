
namespace PlayZMachine.Commands
{
    using Spectre.Console;
    using Spectre.Console.Cli;
    using PlayZMachine.Maps;
    using static Spectre.Console.SelectionPromptExtensions;

    public class LocalGameCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            AnsiConsole.MarkupLine("[underline red]ZorkBot[/] Welcome to an implementation of the Infocom Z-machine based largely on Mark's!");

            var prompt = new SelectionPrompt<string>();
            prompt
                    .Title("Games [green]available[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more games)[/]");
            foreach (Game gameValue in Enum.GetValues(typeof(Game)))
            {
                var choice = prompt.AddChoice(GameMap.Map[gameValue].Item1);
            }
            var game = AnsiConsole.Prompt<string>(prompt: prompt);
            AnsiConsole.MarkupLine($"[grey]SELECTED:[/] {game}");
            return 0;
        }
    }
}
