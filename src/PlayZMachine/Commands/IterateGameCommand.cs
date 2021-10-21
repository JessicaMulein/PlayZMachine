
namespace PlayZMachine.Commands
{
    using Spectre.Console;
    using Spectre.Console.Cli;
    using static Spectre.Console.MultiSelectionPromptExtensions;
    using static Spectre.Console.SelectionPromptExtensions;

    public class IterateGameCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            return 1;
        }
    }
}
