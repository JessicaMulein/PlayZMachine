
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
                breakpointTypes: new BreakpointType[] { BreakpointType.Terminate, BreakpointType.Complete });
            BreakpointType breakpointEncountered = BreakpointType.None;
            var encounteredEnd = new BreakpointType[] { BreakpointType.Terminate, BreakpointType.Complete }.Contains(breakpointEncountered);
            while (!machine.Finished && !encounteredEnd)
            {
                machine.DebugWrite("" + machine.InstructionCounter + " : ");

                breakpointEncountered = machine.processInstruction();
                if (breakpointEncountered != BreakpointType.None)
                {
                    machine.DebugWrite($"Breakpoint reached: {breakpointEncountered}");
                    // this may be an InputRequired for example
                    // drop out of the loop to evaluate it
                    break;
                }
            }

            // dropped out due breakpoint or termination
            switch (breakpointEncountered)
            {
                case BreakpointType.None:
                    // no break occurred, resume normal operation
                    return 0;
                case BreakpointType.InputRequired:
                    // previously this was used to break out of a status loop, but we should not be breaking for input any longer
                    machine.IO.WriteLine("Input required breakpoint reached unexpectedly");
                    return 4;
                case BreakpointType.Complete:
                    return 0;
                case BreakpointType.Terminate:
                    machine.DebugWrite("Terminate Breakpoint encountered.");
                    return 2;
                default:
                    machine.DebugWrite("Invalid breakpoint encountered");
                    return 1;
            }

        }
    }
}
