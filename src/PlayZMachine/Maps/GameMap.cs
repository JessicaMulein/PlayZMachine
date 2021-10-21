namespace PlayZMachine.Maps
{
    public static class GameMap
    {
        public static IReadOnlyDictionary<Game, (string, string)> Map => new Dictionary<Game, (string, string)>()
        {
            { Game.Zork1, ("ZORK1.DAT", "Zork 1: The Final Underground") },
            { Game.Hitchhiker, ("hhgg.z5", "Hitchhiker's Guide to the Galaxy") },
        };
    }
}
