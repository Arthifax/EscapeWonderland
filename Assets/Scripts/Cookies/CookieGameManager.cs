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
    [SerializeField] GameObject gameProps;
    [SerializeField] GameObject endButton;
    public string[] quizQuestions;
    public string[] correctAnswers;

    


    public void CheckQuizAnswer(string answer)
    {
        if (answer == correctAnswers[currentQuestion])
        {
            Debug.Log("Bingo");
            storyText.text = "Nice!";
            currentQuestion++;
            if(currentQuestion == correctAnswers.Length)
            {
                EndQuiz();
            }
            questionCard.SetActive(false);
        }
        else if(answer != correctAnswers[currentQuestion])
        {
            Debug.Log("Boo!");
            storyText.text = "Uh oh";
        }
    }

    public void EndQuiz()
    {
        storyText.text = "Alice: Dit is hem! Deze gaat mij kleiner maken!";
        gameProps.SetActive(false);
        endButton.SetActive(true);
    }
}
