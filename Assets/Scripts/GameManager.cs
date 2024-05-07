using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    int darkMatterCollected = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    public void NextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "Level1":
                SceneManager.LoadScene("Level2");
                break;

            case "Level2":
                SceneManager.LoadScene("Level3");
                break;

            case "Level3":
                StartCoroutine(SetVictory());
                break;
        }
    }

    public void SetGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator SetVictory()
    {
        SceneManager.LoadScene("Victory");
        yield return new WaitForSeconds(.25f);

        TMP_Text recapText = GameObject.FindGameObjectWithTag("Recap").GetComponent<TMP_Text>();
        recapText.text = $"You Found {darkMatterCollected} Dark Matter Cells";

        Cursor.visible = true;

        yield return null;
    }

    IEnumerator GameOver()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
        GameObject.FindGameObjectWithTag("Level").GetComponent<BasicLevelSequence>().StopLevel();
        GameObject.FindGameObjectWithTag("GameOver").SetActive(true);
        return null;
    }

    public void AddDarkMatter(int collected)
    {
        darkMatterCollected += collected;
    }
}
