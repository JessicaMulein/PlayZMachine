using Spectre.Console;
using zmachine.Library.Interfaces;

namespace PlayZMachine.ConsoleInterfaces;

internal class AnsiConsoleIO : IIO
{
    public string? ReadLine()
    {
        return AnsiConsole.Ask<string>("");
    }

    public void Write(string str)
    {
        AnsiConsole.Markup(str);
    }

    public void WriteLine(string str)
    {
        AnsiConsole.MarkupLine(str);
    }

    /// <summary>
    ///     Translate stored byte into key code/key press
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey(true);
    }
}