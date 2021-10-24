namespace PlayZMachine.Maps;

public static class GameMap
{
    public static IReadOnlyDictionary<Game, (string fileName, string description, int zmachineVersion)> Map
        => new Dictionary<Game, (string fileName, string description, int zmachineVersion)>
        {
            {Game.Zork1, ("ZORK1.DAT", "Zork 1: The Final Underground", 3)},
            {
                Game.Hitchhiker, ("hhgg.z5", "Hitchhiker's Guide to the Galaxy", 3)
            }, // technically z5, but seems playable on 3 and was included in repo of V3 player
            {Game.Endless, ("nameless.z8", "Endless/Nameless - 2012. Z8 test game", 8)}
        };
}