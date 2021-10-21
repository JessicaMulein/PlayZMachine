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
            return AnsiConsole.Ask<string>(prompt: "");
        }
        public void Write(string str)
        {
            AnsiConsole.Markup(str); // write/writeline came reversed in the fork.... -JM
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
            char[] key = new char[1];
            //string str = AnsiConsole.

            if ((key[0] >= 'a' && key[0] <= 'z') || (key[0] >= '0' && key[0] <= '9'))
            {
                char[]? ucase = key.ToString().ToUpperInvariant().ToCharArray();
                ConsoleKey console = (ConsoleKey)Enum.Parse(
                    enumType: typeof(ConsoleKey),
                    value: new ReadOnlySpan<char>(
                        array: ucase,
                        start: 0,
                        length: 1),
                    ignoreCase: true);
                return new ConsoleKeyInfo(
                    keyChar: ucase[0],
                    key: console,
                    shift: false,
                    alt: false,
                    control: false);
            }
            else if (key[0] >= 'A' && key[0] <= 'Z')
            {
                ConsoleKey console = (ConsoleKey)Enum.Parse(
                    enumType: typeof(ConsoleKey),
                    value: new ReadOnlySpan<char>(
                        array: key,
                        start: 0,
                        length: 1),
                    ignoreCase: false);
                return new ConsoleKeyInfo(
                    keyChar: key[0],
                    key: console,
                    shift: true,
                    alt: false,
                    control: false);
            }
            else if (key[0] == ' ')
            {
                return new ConsoleKeyInfo(
                    keyChar: key[0],
                    key: ConsoleKey.Spacebar,
                    shift: false,
                    alt: false,
                    control: false);
            }

            throw new NotImplementedException();
        }
    }
}
