
namespace PlayZMachine.Commands
{
    using PlayZMachine.ConsoleInterfaces;
    using PlayZMachine.Maps;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using zmachine.Library;
    using zmachine.Library.Enumerations;
    using zmachine.Library.Models;
    using static Spectre.Console.SelectionPromptExtensions;

    public class LocalGameCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            AnsiConsole.MarkupLine("[underline red]ZorkBot[/] Welcome to an implementation of the Infocom Z-machine based largely on Mark's!");

            SelectionPrompt<string>? prompt = new SelectionPrompt<string>();
            prompt
                    .Title("Games [green]available[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more games)[/]");
            foreach (Game gameValue in Enum.GetValues(typeof(Game)))
            {
                if (!GameMap.Map.ContainsKey(gameValue) || GameMap.Map[gameValue].zmachineVersion > Machine.CurrentVersion)
                {
                    continue;
                }
                ISelectionItem<string>? choice = prompt.AddChoice(GameMap.Map[gameValue].fileName);
            }
            string? gameFile = AnsiConsole.Prompt<string>(prompt: prompt);

            AnsiConsoleIO io = new AnsiConsoleIO();
            Machine machine = new Machine(
                io: io,
                programFilename: Path.Combine(Directory.GetCurrentDirectory(), gameFile),
                breakpointTypes: new Dictionary<BreakpointType,BreakpointAction> { });

            BreakpointType breakpointEncountered = BreakpointType.None;
            while (!machine.Finished)
            {
                machine.DebugWrite("" + machine.InstructionCounter + " : ");

                InstructionInfo instructionInfo = machine.processInstruction();
                breakpointEncountered = instructionInfo.BreakpointType;
                if (breakpointEncountered != BreakpointType.None)
                {
                    machine.DebugWrite($"Breakpoint reached: {breakpointEncountered}");
                    break;
                }
            }

            // dropped out due breakpoint or termination
            switch (breakpointEncountered)
            {
                case BreakpointType.None:
                    // no break occurred
                    return 0;
                case BreakpointType.Complete:
                    return 0;
                case BreakpointType.InputRequired:
                    // previously this was used to break out of a status loop, but we should not be breaking for input any longer
                    machine.IO.WriteLine("Input required breakpoint reached unexpectedly");
                    return 6;
                case BreakpointType.DivisionByZero:
                    machine.DebugWrite("Division by zero.");
                    return 5;
                case BreakpointType.Error:
                    machine.DebugWrite("Unhandled error encountered.");
                    return 4;
                case BreakpointType.Unimplemented:
                    machine.DebugWrite("Requied but unimplemented opcode encountered.");
                    return 3;
                case BreakpointType.Terminate:
                    machine.DebugWrite("Terminate Breakpoint encountered.");
                    return 2;
                default:
                    machine.DebugWrite("Invalid breakpoint encountered");
                    return 255;
            }
        }
    }
}
