namespace PlayZMachine.Services
{
    using PlayZMachine.Maps;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tweetinvi;
    using Tweetinvi.Models;
    using zmachine.Library;

    public class ZorkBotService
    {
        private readonly TwitterClient userClient;

        private IAuthenticatedUser? authenticatedUser;

        

        public ZorkBotService()
        {
            userClient = new TwitterClient(
                consumerKey: "CONSUMER_KEY",
                consumerSecret: "CONSUMER_SECRET",
                accessToken: "ACCESS_TOKEN",
                accessSecret: "ACCESS_TOKEN_SECRET");
        }

        public async Task<IAuthenticatedUser?> Login()
        {
            authenticatedUser = await userClient.Users
                .GetAuthenticatedUserAsync()
                .ConfigureAwait(false);
            return authenticatedUser;
        }

        public async Task<ITweet?> Tweet(string content)
        {
            if (authenticatedUser is null)
            {
                throw new Exception("must be logged in");
            }

            return await userClient.Tweets
                .PublishTweetAsync(content)
                .ConfigureAwait(false);
        }

        public async Task Subscribe(string environment)
        {
            if (authenticatedUser is null)
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
            if (authenticatedUser is null)
            {
                throw new Exception("must be logged in");
            }

            await userClient.AccountActivity
                .UnsubscribeFromAccountActivityAsync(
                    environment: environment,
                    userId: authenticatedUser.Id)
                .ConfigureAwait(false);
        }

        public async Task<string> Iterate(Game game, CPUState? state, string? initialInput = null, bool untilInput = true)
        {
            if (!GameMap.Map.ContainsKey(game))
            {
                throw new ArgumentException(nameof(game));
            }

            (string fileName, string description, int zmachineVersion) = GameMap.Map[game];

            StaticIO io = new StaticIO(initialInput: initialInput);
            Machine machine = (state is null)
                ? new Machine(
                    io: io,
                    programFilename: fileName)
                : new Machine(
                    io: io,
                    initialState: state);

            machine.processInstruction();

            throw new NotImplementedException();
        }
    }
}
