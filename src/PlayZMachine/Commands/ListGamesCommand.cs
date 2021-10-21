
namespace PlayZMachine.Commands
{
    using Spectre.Console;
    using Spectre.Console.Cli;
    using static Spectre.Console.MultiSelectionPromptExtensions;
    using static Spectre.Console.SelectionPromptExtensions;

    public class ListGamesCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            var prompt = new SelectionPrompt<string>();
            prompt
                    .Title("What's your [green]favorite fruit[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoice("Apple");
            var fruit = AnsiConsole.Prompt<string>(prompt: prompt);
            return 0;
        }
    }
}
