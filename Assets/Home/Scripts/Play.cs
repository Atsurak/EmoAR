using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene("GameScene"); 
    }
    public void HomeScreen(){
        SceneManager.LoadScene("HomeScene");
    }
    public void LearnScreen(){
        SceneManager.LoadScene("Learn");
    }

}
