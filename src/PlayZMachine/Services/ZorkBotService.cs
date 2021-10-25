using PlayZMachine.Enumerations;
using PlayZMachine.Maps;
using Tweetinvi;
using Tweetinvi.Models;
using zmachine.Library.Models;
using zmachine.Library.Models.IO;

namespace PlayZMachine.Services;

public class ZorkBotService
{
    private readonly TwitterClient userClient;

    private IAuthenticatedUser? authenticatedUser;


    public ZorkBotService()
    {
        this.userClient = new TwitterClient(
            "CONSUMER_KEY",
            "CONSUMER_SECRET",
            "ACCESS_TOKEN",
            "ACCESS_TOKEN_SECRET");
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

        await this.userClient.AccountActivity
            .SubscribeToAccountActivityAsync(
                environment)
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
                environment,
                this.authenticatedUser.Id)
            .ConfigureAwait(false);
    }

    public async Task<string> Iterate(Game game, CPUState? state, string? initialInput = null, bool untilInput = true)
    {
        if (!GameMap.Map.ContainsKey(game))
        {
            throw new ArgumentException(nameof(game));
        }

        (string? fileName, string? description, int zmachineVersion) = GameMap.Map[game];

        StaticIO io = new StaticIO(initialInput);
        Machine machine = state is null
            ? new Machine(
                io,
                fileName)
            : new Machine(
                io,
                state);

        machine.processInstruction();

        throw new NotImplementedException();
    }
}