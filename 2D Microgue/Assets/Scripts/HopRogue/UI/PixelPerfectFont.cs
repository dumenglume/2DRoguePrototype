using UnityEngine;

namespace HopRogue
{
public class PixelPerfectFont : MonoBehaviour
{
    public Font[] fonts;

    void Start()
    {
        for (int i = 0; i < fonts.Length; i++)
        {
            fonts[i].material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}
}