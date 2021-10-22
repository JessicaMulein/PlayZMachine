
namespace PlayZMachine.Commands
{
    using Spectre.Console;
    using Spectre.Console.Cli;
    using PlayZMachine.Maps;
    using static Spectre.Console.SelectionPromptExtensions;
    using zmachine.Library;
    using System.Diagnostics;
    using PlayZMachine.ConsoleInterfaces;
    using zmachine.Library.Enumerations;

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

            AnsiConsoleIO io = new AnsiConsoleIO();
            Machine machine = new Machine(
                io: io,
                programFilename: Path.Combine(Directory.GetCurrentDirectory(), gameFile),
                breakpointTypes: new BreakpointType[] { BreakpointType.InputRequired, BreakpointType.Terminate });
            BreakpointType breakpointEncountered = BreakpointType.None;
            while (!machine.Finished && (breakpointEncountered != BreakpointType.Terminate))
            {
                // primarily stay in the status log
                AnsiConsole.Status()
                    .Start(status: $"[grey]SELECTED:[/] {gameFile}", ctx =>
                    {
                        ctx.Spinner(Spinner.Known.Dots8Bit);
                        ctx.SpinnerStyle(Style.Parse("yellow"));


                        ctx.SpinnerStyle(Style.Parse("green"));
                        ctx.Status("Data loaded");

                        while (!machine.Finished)
                        {
                            machine.DebugWrite("" + machine.InstructionCounter + " : ");

                            ctx.Status($"i:{machine.InstructionCounter}> ");
                            breakpointEncountered = machine.processInstruction();
                            if (breakpointEncountered != BreakpointType.None)
                            {
                                machine.DebugWrite($"Breakpoint reached: {breakpointEncountered}");
                                // this may be an InputRequired for example
                                // drop out of the loop to evaluate it
                                break;
                            }
                        }
                    });

                // dropped out due to input required, breakpoint (future), or termination
                switch (breakpointEncountered)
                {
                    case BreakpointType.None:
                        // no break occurred, resume normal operation
                        break;
                    case BreakpointType.InputRequired:
                        machine.DebugWrite("Input Breakpoint encountered.");
                        // re process the input instruction, skipping the input break, allowing the single input operation 
                        // basically disable breaking until at least this instruction
                        machine.BreakAfter = Math.Max(
                            machine.BreakAfter,
                            machine.InstructionCounter + 1);
                        breakpointEncountered = machine.processInstruction();
                        break;
                    case BreakpointType.Terminate:
                        machine.DebugWrite("Terminate Breakpoint encountered.");
                        return 1;
                }

                // resume loop normally
            }

            return 0;
        }
    }
}
