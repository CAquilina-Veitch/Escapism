using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelGlitch : MonoBehaviour
{
    public Sprite img;
    float totalTime = 1;
    public float delay = 0.5f;
    public bool isGlitching;

    public void FadeOut(bool to)
    {
        GetReady();
        StartCoroutine(GlitchOut(to));
    }
    public void DoBoth(bool to)
    {
        GetReady();
        StartCoroutine(Both(to));
    }
    public void Glitching(bool to)
    {
        GetReady();
        if (!isGlitching && to)
        {
            isGlitching = to;
            StartCoroutine(Glitch());
        }
        isGlitching = to;
    }
    public void GetReady()
    {
        if (img == null)
        {
            if (TryGetComponent<Image>(out Image i))
            {
                img = i.sprite;
            }
            else
            {if(TryGetComponent<Animator>(out Animator u)){
                u.StopPlayback();
            }
                
                img = GetComponent<SpriteRenderer>().sprite;
            }
        }

    }
    private void OnEnable()
    {
        GetReady();
    }
    IEnumerator GlitchOut(bool to)
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D(img.texture.width, img.texture.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(img.texture,texture);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        img = sprite;

        List<Vector2Int> temp = new List<Vector2Int>();
       // List<Vector2Int> tempto = new List<Vector2Int>();

        for (int x = 0; x <= 10; x++)
        {
            for (int y = 0; y <= 10; y++)
            {
                Debug.Log(x+" "+y);
                Vector2Int coord = new Vector2Int(x, y);
                temp.Add(coord);
            }
        }
        int amnt = temp.Count;
        while (temp.Count > 0)
        {
            int i = Random.Range(0, temp.Count);
            Vector2Int coord = temp[i];
            //tempto.Add(coord);
            temp.RemoveAt(i);

            Vector2Int xBounds = new Vector2Int(texture.width/10*coord.x, texture.width / 10 * (coord.x+1));
            Vector2Int yBounds = new Vector2Int(texture.height / 10 * coord.y, texture.height / 10 * (coord.y + 1)); ;

            for(int x = xBounds.x; x < xBounds.y; x++)
{
                for (int y = yBounds.x; y < yBounds.y; y++)
                {
                    Debug.Log(x + " " + y);
                    texture.SetPixel(x, y, Color.clear);
                    texture.Apply();
                }
            }


            texture.Apply();






            yield return new WaitForSeconds(totalTime / (float)amnt);

            
            
        }
        gameObject.SetActive(to);

    }

    IEnumerator Glitch()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = new Texture2D(img.texture.width, img.texture.height, img.texture.format, false);
        Graphics.CopyTexture(img.texture, texture);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        img = sprite;


        


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
    IEnumerator Both(bool to)
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(img.texture.width, img.texture.height, img.texture.format, false);
        Graphics.CopyTexture(img.texture, texture);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        img = sprite;



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

        gameObject.SetActive(to);

    }


}
