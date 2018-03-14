using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class procces_gaming : MonoBehaviour {

    public int score = 0;
    public Text gb_word;
    public Text gb_score;
    public Text gb_finish_score;
    public Image[] cubes;

    public GameObject panel_lose;
    string[] words = new string[] { "RED", "GREEN", "BLUE" };
	void Start ()
    {
        gb_word.text = generate_word();
	}
	 
    public void NextColor(string color)
    {
        if (color == gb_word.text)
        {
            score++;
            gb_score.text = score.ToString();
            gb_word.text = generate_word();
        }
        else
        {
            panel_lose.SetActive(true);
            gb_finish_score.text += " " + score.ToString();
        }
    }   
    string generate_word()
    {
        return words[Random.Range(0, 3)];
    }
    public void retry()
    {
        Menu menu = new Menu();
        menu.loadGame();
    }
	
}
