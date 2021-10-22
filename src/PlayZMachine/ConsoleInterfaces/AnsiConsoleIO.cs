namespace PlayZMachine.ConsoleInterfaces
{
    using Spectre.Console;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using zmachine.Library;

    internal class AnsiConsoleIO : IIO
    {
        public string? ReadLine()
        {
            return AnsiConsole.Ask<string>(prompt: ">");
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
        /// Translate stored byte into key code/key press
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public System.ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey(true);
        }
    }
}
