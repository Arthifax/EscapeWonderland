using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class CheckQuizInput : MonoBehaviour
{

    public TMP_Text questionText;
    public TMP_Text inputFieldText;
    public int currentQuestion = 1;
    public int maxQuestions;
    public List<String> questions;
    public List<String> answers;
    public List<GameObject> pictures;
    public GameObject victoryScreen;

    private void Start()
    {
        questionText.text = questions[0];
        maxQuestions = questions.Count;
    }
    
    public void CheckQuestion()
    {
        Debug.Log(inputFieldText.text);
        if (answers[currentQuestion].Equals(inputFieldText.text))
        {
            if (answers[currentQuestion].Equals(inputFieldText.text) && currentQuestion == maxQuestions - 1)
            {
                Debug.Log("Done!");
                pictures[currentQuestion].SetActive(true);
                questionText.text = "Je hebt gewonnen!";
                victoryScreen.SetActive(true);
            }
            else
            {
                currentQuestion++;
                Debug.Log(currentQuestion);
                pictures[currentQuestion-1].SetActive(true);
                questionText.text = questions[currentQuestion];
                inputFieldText.text = "";
            }
        }

        /*if (!answers[currentQuestion].Equals(inputFieldText.text))
        {
            Debug.Log("Wrong Input!");
        }
        
        if(currentQuestion <= maxQuestions)
        {
            Debug.Log("You win!");
            currentQuestion++;
            pictures[currentQuestion-1].SetActive(true);
            questionText.text = "You Win!";
            victoryScreen.SetActive(true);
        }*/
    }
}
