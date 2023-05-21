using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimateSprite : MonoBehaviour
{
    [SerializeField] private float current = -0.1f;
    [SerializeField] private float target = 0f;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] Material mat;
    [SerializeField] GameObject background;

    private void Start()
    {
        mat = GetComponent<Image>().material;
    }

    private void Update()
    {
        current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
        mat.SetFloat("_FadeAmount", current);
    }

    public void DissolveSprite()
    {
        target = 1f;
    }
}
