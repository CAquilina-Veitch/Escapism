using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFunctions
{
    public static float EvenlyCenteredValueAround0(int numOfItems, int i)
    {
        return ((numOfItems - 1) * -20) + (40 * i);
    }
    public static float DeltaFPS(int FramesPerSecond)
    {
        return 1f / (float)FramesPerSecond;
    }
    public static int PhoneTextWidth(this string s, int fontsize, Font font)
    {

        if (string.IsNullOrEmpty(s))
            return 0;
        FontStyle fontstyle = FontStyle.Normal;
        int w = 0;
        font.RequestCharactersInTexture(s, fontsize, fontstyle);

        foreach (char c in s)
        {
            font.GetCharacterInfo(c, out CharacterInfo cInfo, fontsize);
            //w += cInfo.advance;
            //w += cInfo.glyphWidth;
            w += (int)cInfo.advance;
        }

        return w;
    }
    public static float specialMult(this string s, float added)
    {
        float f = (float)s.Length;
        float temp = -0.041666666666667f * f * f - 0.25f * f + 11.666666666667f;
        return temp*1.2f+added;
    }
}
