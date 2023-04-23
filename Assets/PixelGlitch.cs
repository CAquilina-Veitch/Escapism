using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelGlitch : MonoBehaviour
{
    Image img;
    float totalTime = 1;
    public float delay = 0.5f;
    public bool isGlitching;

    public void FadeOut()
    {
        
        StartCoroutine(GlitchOut());
    }
    public void DoBoth()
    {
        
        StartCoroutine(Both());
    }
    public void Glitching(bool to)
    {
        if (!isGlitching && to)
        {
            isGlitching = to;
            StartCoroutine(Glitch());
        }
        isGlitching = to;
    }
    private void OnEnable()
    {
        img = GetComponent<Image>();
    }
    IEnumerator GlitchOut()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D(img.sprite.texture.width, img.sprite.texture.height, img.sprite.texture.format, false);
        Graphics.CopyTexture(img.sprite.texture,texture);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        img.sprite = sprite;

        List<Vector2Int> temp = new List<Vector2Int>();
        List<Vector2Int> tempto = new List<Vector2Int>();

        for (int x = 0; x <= 10; x++)
        {
            for (int y = 0; y <= 10; y++)
            {
                Vector2Int coord = new Vector2Int(x, y);
                temp.Add(coord);
            }
        }
        int amnt = temp.Count;
        while (temp.Count > 0)
        {
            int i = Random.Range(0, temp.Count);
            Vector2Int coord = temp[i];
            tempto.Add(coord);
            temp.RemoveAt(i);

            Vector2Int xBounds = new Vector2Int(texture.width/10*coord.x, texture.width / 10 * (coord.x+1));
            Vector2Int yBounds = new Vector2Int(texture.height / 10 * coord.y, texture.height / 10 * (coord.y + 1)); ;

            for(int x = xBounds.x; x < xBounds.y; x++)
{
                for (int y = yBounds.x; y < yBounds.y; y++)
                {

                    texture.SetPixel(x, y, Color.clear);
                }
            }


            texture.Apply();






            yield return new WaitForSeconds(totalTime / (float)amnt);

            
            
        }


    }
    IEnumerator Glitch()
    {
        yield return new WaitForEndOfFrame();


        Texture2D texture = new Texture2D(img.sprite.texture.width, img.sprite.texture.height, img.sprite.texture.format, false);
        Graphics.CopyTexture(img.sprite.texture, texture);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        img.sprite = sprite;


        


        while (isGlitching)
        {
            Vector2Int coord = new Vector2Int(Random.Range(0, 10), Random.Range(0, 10));

            Vector2Int xBounds = new Vector2Int(texture.width / 10 * coord.x, texture.width / 10 * (coord.x + 1));
            Vector2Int yBounds = new Vector2Int(texture.height / 10 * coord.y, texture.height / 10 * (coord.y + 1)); ;

            Color[] pixelColors = new Color[(xBounds.y - xBounds.x) * (yBounds.y - yBounds.x)];
            Color rand = new Color(Random.Range(0, 1f),Random.Range(0, 1f),Random.Range(0, 1f),Random.Range(0, 1f));
            int i = 0;
            for (int x = xBounds.x; x < xBounds.y; x++)
            {
                for (int y = yBounds.x; y < yBounds.y; y++)
                {
                    pixelColors[i] = texture.GetPixel(x, y);
                    texture.SetPixel(x, y, rand);
                    i++;
                }
            }

            texture.Apply();

            yield return new WaitForSeconds(delay);



            i = 0;
            for (int x = xBounds.x; x < xBounds.y; x++)
            {
                for (int y = yBounds.x; y < yBounds.y; y++)
                {
                    texture.SetPixel(x, y, pixelColors[i]);
                    i++;
                }
            }

            texture.Apply();







        }


    }
    IEnumerator Both()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(img.sprite.texture.width, img.sprite.texture.height, img.sprite.texture.format, false);
        Graphics.CopyTexture(img.sprite.texture, texture);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        img.sprite = sprite;



        List<Vector2Int> temp = new List<Vector2Int>();
        List<Vector2Int> tempto = new List<Vector2Int>();

        for (int x = 0; x <= 10; x++)
        {
            for (int y = 0; y <= 10; y++)
            {
                Vector2Int coord = new Vector2Int(x, y);
                temp.Add(coord);
            }
        }
        int amnt = temp.Count;
        while (temp.Count > 0)
        {
            int i = Random.Range(0, temp.Count);
            Vector2Int coord = temp[i];
            tempto.Add(coord);
            temp.RemoveAt(i);

            Vector2Int xBounds = new Vector2Int(texture.width / 10 * coord.x, texture.width / 10 * (coord.x + 1));
            Vector2Int yBounds = new Vector2Int(texture.height / 10 * coord.y, texture.height / 10 * (coord.y + 1)); ;

            for (int x = xBounds.x; x < xBounds.y; x++)
            {
                for (int y = yBounds.x; y < yBounds.y; y++)
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }

            texture.Apply();




            coord = new Vector2Int(Random.Range(0, 10), Random.Range(0, 10));


            Color[] pixelColors = new Color[(xBounds.y - xBounds.x) * (yBounds.y - yBounds.x)];
            Color rand = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            i = 0;
            for (int x = xBounds.x; x < xBounds.y; x++)
            {
                for (int y = yBounds.x; y < yBounds.y; y++)
                {
                    pixelColors[i] = texture.GetPixel(x, y);
                    texture.SetPixel(x, y, rand);
                    i++;
                }
            }

            texture.Apply();




            yield return new WaitForSeconds(totalTime / (float)amnt);

            i = 0;
            for (int x = xBounds.x; x < xBounds.y; x++)
            {
                for (int y = yBounds.x; y < yBounds.y; y++)
                {
                    texture.SetPixel(x, y, pixelColors[i]);
                    i++;
                }
            }

            texture.Apply();

        }


    }


}
