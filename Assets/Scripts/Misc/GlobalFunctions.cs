using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFunctions
{
    public static float EvenlyCenteredValueAround0(int numOfItems, int i)
    {
        return ((numOfItems - 1) * -20) + (40 * i);
    }
}
