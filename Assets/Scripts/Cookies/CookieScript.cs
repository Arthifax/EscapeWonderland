using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CookieScript : MonoBehaviour
{
    [SerializeField] private int cookieOrderNumber;
    [SerializeField] private CookieGameManager gameManager;
    [SerializeField] private GameObject quizCard;
    [SerializeField] private TMP_Text quizText;


    public void CheckCookie()
    {
        if (gameManager.currentQuestion == cookieOrderNumber)
        {
            if (cookieOrderNumber == 0)
            {
                quizCard.SetActive(true);
            }
            else if (cookieOrderNumber == 1)
            {
                quizCard.SetActive(true);
            }
            else if (cookieOrderNumber == 2)
            {
                quizCard.SetActive(true);
            }
            else if (cookieOrderNumber == 3)
            {
                quizCard.SetActive(true);
            }
            else if (cookieOrderNumber == 4)
            {
                quizCard.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}
