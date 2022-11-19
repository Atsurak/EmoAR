using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type of the Question
[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    AUDIO,
    VIDEO
}

// Game Status Description
[SerializeField]
public enum GameStatus
{
    PLAYING,
    NEXT
}

// Data Structure for Storing Question Data
[System.Serializable]
public class Question
{
    public string questionInfo;         //question text
    public QuestionType questionType;   //question type
    public Sprite questionImage;        //image for Image Type Question
    public AudioClip audioClip;         //audio for audio Type Question
    public UnityEngine.Video.VideoClip videoClip;   //video for video type Qns
    public List<string> options;        //options to select
    public string correctAns;           //correct option i.e" Answer
}

public class QuizManager : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private QuizGameUI quizGameUI; // QuizGameUI Script
    [SerializeField] private List<QuizDataScriptable> quizDataList; // Scriptable Object
    [SerializeField] private float timeInSeconds; // time for each section
#pragma warning restore 649

    private string currentCategory = "";
    private int correctAnswerCount = 0;
    private List<Question> questions;
    private Question selectedQuetion = new Question(); // select a Question
    private int gameScore;
    private int lifesRemaining;
    private float currentTime;
    private QuizDataScriptable dataScriptable;

    private GameStatus gameStatus = GameStatus.NEXT;

    public GameStatus GameStatus { get { return gameStatus; } }

    public List<QuizDataScriptable> QuizData { get => quizDataList; }

    public void StartGame(int categoryIndex, string category)
    {
        currentCategory = category;
        correctAnswerCount = 0;
        gameScore = 0;
        lifesRemaining = 3;
        currentTime = timeInSeconds;
        
        // set the Questions Data for current Quiz
        questions = new List<Question>();
        dataScriptable = quizDataList[categoryIndex];
        questions.AddRange(dataScriptable.questions);

        //select a question and change status to Playing
        SelectQuestion();
        gameStatus = GameStatus.PLAYING;
    }

    // randomly select's a question
    private void SelectQuestion()
    {
        //get a random number
        int val = UnityEngine.Random.Range(0, questions.Count);
        //set the selectedQuetion
        selectedQuetion = questions[val];
        //send the question to quizGameUI
        quizGameUI.SetQuestion(selectedQuetion);
        // remove that question from the questions list
        questions.RemoveAt(val);
    }

    private void Update()
    {
        if (gameStatus == GameStatus.PLAYING)
        {
            currentTime -= Time.deltaTime;
            SetTime(currentTime);
        }
    }

    void SetTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);    //set the time value
        quizGameUI.TimerText.text = time.ToString("mm':'ss");//convert time to Time format

        if (currentTime <= 0)
        {
            //Game Over
            GameEnd();
        }
    }

    // check whether the selected option is correct
    public bool Answer(string selectedOption)
    {
        //set default to false
        bool correct = false;
        //if selected answer is the correctAns
        if (selectedQuetion.correctAns == selectedOption)
        {
            correctAnswerCount++;
            correct = true;
            gameScore += 50;
            quizGameUI.ScoreText.text = "Score:" + gameScore;
        }
        else
        {
            lifesRemaining--;
            quizGameUI.ReduceLife(lifesRemaining);

            if (lifesRemaining == 0)
            {
                GameEnd();
            }
        }

        if (gameStatus == GameStatus.PLAYING)
        {
            if (questions.Count > 0)
            {
                //call SelectQuestion method again after 1s
                Invoke("SelectQuestion", 0.4f);
            }
            else
            {
                GameEnd();
            }
        }
        //return the value of correct bool
        return correct;
    }

    private void GameEnd()
    {
        gameStatus = GameStatus.NEXT;
        quizGameUI.GameOverPanel.SetActive(true);
        //to save only the highest score compare the current score with saved score and if more save the new score
        //if correctAnswerCount > PlayerPrefs.GetInt(currentCategory)
        PlayerPrefs.SetInt(currentCategory, correctAnswerCount); //save score for this quiz or current Category
    }
}