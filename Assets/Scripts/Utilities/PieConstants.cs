public static class PieConstants
{
    public static class Layers
    {
        public const int LevelBounds = 0;
        public const int Death = 6;
        public const int Ground = 9;
    }

    public static class Tags
    {
        public const string Player = "Player";
        public const string Scoop = "Scoop";
    }

    public enum ShipAttackType
    { 
        Circle,// launches projectiles from all around the shpi
        Sweep,// aiming a certain cardinal direction, launches projectiles one by one in an arcing motion
        Targeting// aims at the player and fires for a certain number of projectiles
    }
}

