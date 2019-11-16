using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Interface_Controller : MonoBehaviour {

    public TMP_Text scoreText;
    public TMP_Text LevelOfDifficulty;
    public TMP_Text gameOvertext;

    private float _level = 1;
    private float _score;
    private bool _pause;

    private void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SCORE_CHANGE, Score);
        Messenger<float>.AddListener(GameEvent.LEVEL, Level);
        Messenger<bool>.AddListener(GameEvent.GAMEOVER, GameOver);        
    }
    private void Start()
    {
        Level(_level);
    }

    private void Score(float score)
    {
        _score = score;
        scoreText.text = "Score " + string.Format("{0:0}", _score);
    }

    private void Level(float level)
    {
        _level = level;
        LevelOfDifficulty.text = "Level Of Difficulty " + string.Format("{0:0.00}", _level);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    private void GameOver(bool gOver)
    {
        if (gOver)  //Возможно в дальнейшем будут другие вварианты
        {
            string maxScoreText = "";
            float saveScore = PlayerPrefs.GetFloat("Player");

            if (saveScore > 0) maxScoreText = "Your MAX Score: " + saveScore;
            if(saveScore < _score) PlayerPrefs.SetFloat("Player", _score);

            if (gameOvertext) gameOvertext.gameObject.SetActive(true);
            gameOvertext.text = "Game Over \n" +
                "Your Score: " + string.Format("{0:0}", _score) + "\n" +
                maxScoreText;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GamePause()
    {
        if (_pause)
        {
            Time.timeScale = 1;
            _pause = false;
        }
        else
        {
            Time.timeScale = 0;
            _pause = true;
        }
    }

    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.SCORE_CHANGE, Score);
        Messenger<float>.RemoveListener(GameEvent.LEVEL, Level);
        Messenger<bool>.AddListener(GameEvent.GAMEOVER, GameOver);
    }
}
