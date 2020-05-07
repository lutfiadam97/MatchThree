using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Instance sebagai global access
    public static GameManager instance;
    int playerScore;
    public Text scoreText;

    // singleton, Singleton merupakan design pattern dimana 
    //membatasi instance dari suatu Class hanya satu saja. 
    //Karena instance nya hanya dibatasi satu saja, 
    //Singleton menyediakan global point of access. 
    //Dengan cara ini semua dapat mengakses instance nya.
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    //Update score dan ui
    public void GetScore(int point)
    {
        playerScore += point;
        scoreText.text = playerScore.ToString();
    }
}
