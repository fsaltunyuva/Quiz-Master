using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private int correctAnswers = 0;
    private int questionsSeen = 0;

    public int getCorrectAnswers() {
        return correctAnswers;
    }

    public void IncrementCorrectAnswer(){
        correctAnswers++;
    }

    public int getQuestionSeen() {
        return questionsSeen;
    }

    public void IncrementQuestionSeen() {
        questionsSeen++;
    }

    public int calculateScore(){
        return Mathf.RoundToInt(correctAnswers / (float) questionsSeen * 100);
    }
}
