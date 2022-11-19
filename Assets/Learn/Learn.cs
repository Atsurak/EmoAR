using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Learn : MonoBehaviour
{
    public void Cow(){
        SceneManager.LoadScene("Cow"); 
    }
    public void Tiger(){
        SceneManager.LoadScene("Tiger");
    }
    public void Dog(){
        SceneManager.LoadScene("Dog");
    }
}
