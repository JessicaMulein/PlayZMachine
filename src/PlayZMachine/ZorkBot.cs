namespace PlayZMachine
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tweetinvi;
    using Tweetinvi.Models;

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

        public async Task<string> Iterate(Game game, string state, string initialInput, bool untilInput = true)
        {
            if (!this.Games.ContainsKey(game))
            {
                return null;
            }

            (string file, string description) = this.Games[game];

            TwitterIO io = new TwitterIO(initialInput: "");
            zmachine.Machine machine = new zmachine.Machine(
                io: io,
                filename: file);

            machine.processInstruction();
            
            throw new NotImplementedException();
        }
    }
}
