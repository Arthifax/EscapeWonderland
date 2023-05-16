using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CookieGameManager : MonoBehaviour
{
    public int currentQuestion = 0;
    public GameObject questionCard;
    [SerializeField] private TMP_Text quizText;
    [SerializeField] private TMP_Text storyText;
    public string[] quizQuestions;
    public string[] correctAnswers;


    public void CheckQuizAnswer(string answer)
    {
        if (answer == correctAnswers[currentQuestion])
        {
            Debug.Log("Bingo");
            storyText.text = "Nice!";
            currentQuestion++;
            questionCard.SetActive(false);
        }
        else if(answer != correctAnswers[currentQuestion])
        {
            Debug.Log("Boo!");
            storyText.text = "Uh oh";
        }
    }
}
