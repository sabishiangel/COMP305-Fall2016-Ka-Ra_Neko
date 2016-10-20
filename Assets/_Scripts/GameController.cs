using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/*Author: Ashley Tjon-Hing
Program: COMP305 FALL 2016
Student #: 300744476
Date: October 19th 2016
Last Revised: October 20th 2016*/

public class GameController : MonoBehaviour
{
    //Private Instance Variables


    //Public Instance Variables (Testing)
    public int score = 0;


    [Header("Labels")]
    public Text livesLabel;
    public Text powerLabel;
    public Text scoreLabel;
    public Text gameOverLabel;
    public Text finalLabel;
    public Text victoryLabel;
    public Button restartButton;
    public Button instructions;

    [Header("Game Objects")]
    public GameObject KaRa;
    public GameObject Good_Vibes;
    public GameObject bad_guy;
    public GameObject bad_boss;

    [Header("Music")]
    public AudioSource endMusic;
    public AudioSource winMusic;

    // Use this for initialization
    void Start()
    {

        gameOverLabel.gameObject.SetActive(false);
        finalLabel.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        this.victoryLabel.gameObject.SetActive(false);
        this.instructions.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void endGame()
    {
        this.scoreLabel.gameObject.SetActive(false);
        this.livesLabel.gameObject.SetActive(false);
        this.powerLabel.gameObject.SetActive(false);
        this.instructions.gameObject.SetActive(true);
        this.gameOverLabel.gameObject.SetActive(true);
        this.finalLabel.gameObject.SetActive(true);
        this.finalLabel.text = "Score: " + this.score;
        this.restartButton.gameObject.SetActive(true);
        this.KaRa.SetActive(false);
        this.Good_Vibes.SetActive(false);
        this.bad_guy.SetActive(false);
        this.bad_boss.SetActive(false);
        endMusic.Play();
        endMusic.loop = true;
    }

    public void winGame()
    {
        this.scoreLabel.gameObject.SetActive(false);
        this.livesLabel.gameObject.SetActive(false);
        this.powerLabel.gameObject.SetActive(false);
        this.finalLabel.gameObject.SetActive(true);
        this.instructions.gameObject.SetActive(true);
        this.victoryLabel.gameObject.SetActive(true);
        this.finalLabel.text = "Score: " + this.score;
        this.restartButton.gameObject.SetActive(true);
        this.KaRa.SetActive(false);
        this.Good_Vibes.SetActive(false);
        this.bad_guy.SetActive(false);
        this.bad_boss.SetActive(false);
        winMusic.Play();
        winMusic.loop = true;
    }

    public void restart_click()
    {
        SceneManager.LoadScene("Play");
    }

}
