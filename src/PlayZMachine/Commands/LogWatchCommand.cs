
namespace PlayZMachine.Commands
{
    using Spectre.Console;
    using Spectre.Console.Cli;

    public class LogWatchCommand : Command
    {
        public override int Execute(CommandContext context)
        {
            AnsiConsole.MarkupLine("");
            AnsiConsole.Status()
                .Start(status: "Bot Log", ctx =>
                {
                    ctx.Spinner(Spinner.Known.Dots8Bit);
                    ctx.SpinnerStyle(Style.Parse("yellow"));

                    while(true)
                    {
                        Thread.Sleep(1000);
                        DateTime date = DateTime.Now;
                        AnsiConsole.MarkupLine($"{date.ToString()}");
                    }
                });
            return 0;
        }
    }
}
