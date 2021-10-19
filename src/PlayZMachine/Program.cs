using Spectre.Cli;

namespace PlayZMachine
{
    class Program
    {
        private static PlayZMachine.ZorkBot ZorkBot;
        
        static Program()
        {
            ZorkBot = new PlayZMachine.ZorkBot();
        }

        static async Task Main(string[] argv)
        {
            var app = new CommandApp<IterateGameCommand>();
            app.Configure(config =>
            {
#if DEBUG
                config.PropagateExceptions();
                config.ValidateExamples();
#endif
            });

            // these memory states may be too long to tweet, so we may need to throw them in storage for a period of time and expire them
            var output = await ZorkBot.Iterate(
                game: Game.Zork1,
                state: null,
                initialInput: "",
                untilInput: true);
        }
    }
}