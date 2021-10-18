﻿namespace PlayZMachine
{
    using Tweetinvi;
    using Tweetinvi.Client;
    using Tweetinvi.Models;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ZorkBot
    {
        private readonly TwitterClient userClient;

        private IAuthenticatedUser? authenticatedUser;

        private readonly IReadOnlyDictionary<string, (string, string)> Games = new Dictionary<string, (string, string)>()
        {
            { "zork1", ("ZORK1.DAT", "Zork 1: The Final Underground") },
            { "hhgg2", ("hhgg2.z5", "Hitchhiker's Guide to the Galaxy") },
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

        public async Task Unsubscribe(string environment)
        {
            if (this.authenticatedUser is null)
            {
                throw new Exception("must be logged in");
            }

            await this.userClient.AccountActivity
                .UnsubscribeFromAccountActivityAsync(
                    environment: "sandbox",
                    userId: this.authenticatedUser.Id)
                .ConfigureAwait(false);
        }

        public async Task<string> Iterate(string game, string state)
        {
            if (!this.Games.ContainsKey(game))
            {
                return null;
            }

            (string file, string description) = this.Games[game];

            //ReadData("ZORK1.DAT");

            //Memory memory = new Memory(128 * 1024); //128k main memory block
            //memory.load("ZORK1.DAT");
            //memory.dumpHeader();
            zmachine.Machine machine = new zmachine.Machine(file);

            int numInstructionsProcessed = 0;
            while (!machine.isFinished())
            {
                if (machine.debug)
                    Debug.Write("" + numInstructionsProcessed + " : ");
                machine.processInstruction();
                ++numInstructionsProcessed;
            }
            Debug.WriteLine("Instructions processed: " + numInstructionsProcessed);
        }
    }
}
