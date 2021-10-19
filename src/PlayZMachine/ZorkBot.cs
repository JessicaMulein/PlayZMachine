namespace PlayZMachine
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tweetinvi;
    using Tweetinvi.Models;
    using zmachine;

    public class ZorkBot
    {
        private readonly TwitterClient userClient;

        private IAuthenticatedUser? authenticatedUser;

        private readonly IReadOnlyDictionary<Game, (string, string)> Games = new Dictionary<Game, (string, string)>()
        {
            { Game.Zork1, ("ZORK1.DAT", "Zork 1: The Final Underground") },
            { Game.Hitchhiker, ("hhgg2.z5", "Hitchhiker's Guide to the Galaxy") },
        };

        public ZorkBot()
        {
            this.userClient = new TwitterClient(
                consumerKey: "CONSUMER_KEY",
                consumerSecret: "CONSUMER_SECRET",
                accessToken: "ACCESS_TOKEN",
                accessSecret: "ACCESS_TOKEN_SECRET");
        }

        public async Task<IAuthenticatedUser?> Login()
        {
            this.authenticatedUser = await this.userClient.Users
                .GetAuthenticatedUserAsync()
                .ConfigureAwait(false);
            return this.authenticatedUser;
        }

        public async Task<ITweet?> Tweet(string content)
        {
            if (this.authenticatedUser is null)
            {
                throw new Exception("must be logged in");
            }

            return await this.userClient.Tweets
                .PublishTweetAsync(content)
                .ConfigureAwait(false);
        }

        public async Task Subscribe(string environment)
        {
            if (this.authenticatedUser is null)
            {
                throw new Exception("must be logged in");
            }

            await userClient.AccountActivity
                .SubscribeToAccountActivityAsync(
                    environment: environment)
                .ConfigureAwait(false);
        }

        public async Task Unsubscribe(string environment = "sandbox")
        {
            if (this.authenticatedUser is null)
            {
                throw new Exception("must be logged in");
            }

            await this.userClient.AccountActivity
                .UnsubscribeFromAccountActivityAsync(
                    environment: environment,
                    userId: this.authenticatedUser.Id)
                .ConfigureAwait(false);
        }

        public async Task<string> Iterate(Game game, CPUState state, string initialInput, bool untilInput = true)
        {
            if (!this.Games.ContainsKey(game))
            {
                throw new ArgumentException(nameof(game));
            }

            (string file, string description) = this.Games[game];

            StaticIO io = new zmachine.StaticIO(initialInput: initialInput);
            zmachine.Machine machine = (state is null)
                ? new zmachine.Machine(
                    io: io,
                    programFilename: file)
                : new zmachine.Machine(
                    io: io,
                    initialState: state);

            machine.processInstruction();

            throw new NotImplementedException();
        }
    }
}
