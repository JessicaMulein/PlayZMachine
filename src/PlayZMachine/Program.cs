namespace PlayZMachine
{
    using Spectre.Cli;

    using PlayZMachine.Commands;
    using PlayZMachine.Services;

    class Program
    {
        private static ZorkBotService ZorkBot;

        static Program()
        {
            ZorkBot = new ZorkBotService();
        }

        static async Task Main(string[] argv)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
#if DEBUG
                config.PropagateExceptions();
                config.ValidateExamples();
#endif
                config
                    .AddCommand<ListGamesCommand>("games")
                    .WithAlias("g")
                    .WithDescription("List games");

                config
                    .AddCommand<IterateGameCommand>("play")
                    .WithAlias("p")
                    .WithDescription("Play a game")
                    .WithExample(new[] { "zork1" });
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