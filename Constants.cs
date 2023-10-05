public class Constants
{
    public static float SCORE_RADIUS = 3f;
    public static float STRUCTURE_SIZE = 3f;

    public static float WIND_APMPLITUDE = 1.5f;
    public static float WIND_FREQUENCY = 1.11f;

    public static int HIGHLIGHT_POOL_SIZE = 10;
}

public enum STRUCTURE_CATEGORY{
    RESIDENTIAL,
    NATURE,
    INDUSTRY,
    ENTERTAINMENT
}


public enum STRUCTURE_ID{
    HOUSE1, HOUSE2, HOUSE3, HOUSE4, HOUSE5, HOUSE6, HOUSE7,
    PARK1, PARK2, PARK3, PARK4, PARK5, PARK6, PARK7,
    STARTER1, STARTER2, STARTER3, STARTER4, STARTER5, STARTER6, STARTER7
}

public enum THEME{
    GREEN,
    DESERT,
    COLD
}

public enum GAMEPLAY_STATE{
    PLACEMENT,  //NOTE - Gameplay UI
    MenuORUI,
    PHOTO,
    SettingsUI,
    NewGameUI,
    LoadGameUI
}

public enum GAME_STATUS{
    GameInProgress, GameWon, GameLost
}

public enum PROP_ID{
    WATER_PROP_1, WATER_PROP_2, WATER_PROP_3, WATER_PROP_4, WATER_PROP_5, WATER_PROP_6,
    ISLAND_PROP_1, ISLAND_PROP_2, ISLAND_PROP_3, ISLAND_PROP_4, ISLAND_PROP_5, ISLAND_PROP_6,
    WATER_TREE, ISLAND_TREE
}