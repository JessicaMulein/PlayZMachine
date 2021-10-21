
namespace PlayZMachine.Commands
{
    using Spectre.Console;
    using Spectre.Console.Cli;
    using PlayZMachine.Maps;
    using static Spectre.Console.SelectionPromptExtensions;
    using zmachine.Library;
    using System.Diagnostics;

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
            var gameFile = AnsiConsole.Prompt<string>(prompt: prompt);

            int numInstructionsProcessed = 0;
            AnsiConsole.Status()
                .Start(status: $"[grey]SELECTED:[/] {gameFile}", ctx =>
                {
                    ctx.Spinner(Spinner.Known.Dots8Bit);
                    ctx.SpinnerStyle(Style.Parse("yellow"));

                    ConsoleIO io = new ConsoleIO();
                    Machine machine = new Machine(
                        io: io,
                        programFilename: Path.Combine(Directory.GetCurrentDirectory(), gameFile));

                    ctx.SpinnerStyle(Style.Parse("green"));
                    ctx.Status("Data loaded");

                    while (!machine.Finished)
                    {
                        if (machine.DebugEnabled)
                        {
                            Debug.Write("" + numInstructionsProcessed + " : ");
                        }

                        ctx.Status($"i:{numInstructionsProcessed}> ");
                        machine.processInstruction();
                        ++numInstructionsProcessed;
                    }

                    Debug.WriteLine("Instructions processed: " + numInstructionsProcessed);
                });

            return 0;
        }
    }
}
