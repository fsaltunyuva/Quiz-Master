using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Scoring")]
    [SerializeField] Slider progressBar;

    public bool isComplete;
    
    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update(){
        timerImage.fillAmount = timer.fillFraction;

        if(timer.loadNextQuestion) {
            if (progressBar.value == progressBar.maxValue) {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion) {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }
    
    public void OnAnswerSelected(int index) {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Puan: " + scoreKeeper.calculateScore(); 
    }

    void DisplayAnswer(int index) {
        Image buttonImage;

        if(index == currentQuestion.getCorrectAnswerIndex()) {
            questionText.text = "Doğru Cevap!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswer();
        }

        else {
                correctAnswerIndex = currentQuestion.getCorrectAnswerIndex();
                string correctAnswer = currentQuestion.getAnswer(correctAnswerIndex);
                questionText.text = "Yanlış Cevap. Doğru cevap :\n" + correctAnswer;
                buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
                buttonImage.sprite = correctAnswerSprite;
        }
    }

    void GetNextQuestion() {
        
        if(questions.Count > 0) {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionSeen();
        }

    }

    void GetRandomQuestion() {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if(questions.Contains(currentQuestion)) {
            questions.Remove(currentQuestion);
        }

    }

    void DisplayQuestion(){

        questionText.text = currentQuestion.getQuestion();

        for(int i = 0; i < answerButtons.Length; i++) {

            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.getAnswer(i);

        }
    }

    void SetButtonState(bool state) {

        for(int i = 0; i < answerButtons.Length; i++) {
            Button button  = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites() {
        for(int i = 0; i < answerButtons.Length; i++) {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

}
