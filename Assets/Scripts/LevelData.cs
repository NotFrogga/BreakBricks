using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelData
{
    static int? lastLevelLoaded = 0;

    public static void setLastLevelLoaded(Level level)
    {
        if (lastLevelLoaded < level.levelLoaded)
        {
            lastLevelLoaded = level.levelLoaded;
        }
    }

    public static int? getLastLevelLoaded()
    {
        return lastLevelLoaded;
    }
}
