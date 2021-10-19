namespace PlayZMachine
{
    class Program
    {
        static async Task Main(string[] argv)
        {
            PlayZMachine.ZorkBot ZorkBot = new PlayZMachine.ZorkBot();
            throw new NotImplementedException();
            // these memory states may be too long to tweet, so we may need to throw them in storage for a period of time and expire them
            await ZorkBot.Iterate("", "");
        }
    }
}