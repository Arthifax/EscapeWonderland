using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSprite : MonoBehaviour
{

    public void DissolveSprite()
    {
        Material mat = GetComponent<Renderer>().material;
        mat.SetFloat("_FadeAmount", 1.0f);
    }
}
