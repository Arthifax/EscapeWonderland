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
    [SerializeField] private TMP_Text storyText;
    

    public void CheckCookie()
    {
        if (gameManager.currentQuestion == cookieOrderNumber)
        {
            Debug.Log("Question Number: " + cookieOrderNumber);
            gameManager.currentQuestion++;
            if (cookieOrderNumber == 1)
            {
                quizCard.SetActive(true);
                quizText.text = "Question Number: " + cookieOrderNumber;
            }
            else if (cookieOrderNumber == 2)
            {
                quizCard.SetActive(true);
                quizText.text = "Question Number: " + cookieOrderNumber;
            }
            else if (cookieOrderNumber == 3)
            {
                quizCard.SetActive(true);
                quizText.text = "Question Number: " + cookieOrderNumber;
            }
            else if (cookieOrderNumber == 4)
            {
                quizCard.SetActive(true);
                quizText.text = "Question Number: " + cookieOrderNumber;
            }
            else if (cookieOrderNumber == 5)
            {
                quizCard.SetActive(true);
                quizText.text = "Question Number: " + cookieOrderNumber;
            }
            gameObject.SetActive(false);
        }
    }

    public void CheckQuizAnswer()
    {
        if (gameManager.currentQuestion == 1)
        {
            quizCard.SetActive(true);
            quizText.text = "Question Number: " + cookieOrderNumber;
        }
        else if (gameManager.currentQuestion == 2)
        {
            quizCard.SetActive(true);
            quizText.text = "Question Number: " + cookieOrderNumber;
        }
        else if (gameManager.currentQuestion == 3)
        {
            quizCard.SetActive(true);
            quizText.text = "Question Number: " + cookieOrderNumber;
        }
        else if (gameManager.currentQuestion == 4)
        {
            quizCard.SetActive(true);
            quizText.text = "Question Number: " + cookieOrderNumber;
        }
        else if (gameManager.currentQuestion == 5)
        {
            quizCard.SetActive(true);
            quizText.text = "Question Number: " + cookieOrderNumber;
        }
    }
}
