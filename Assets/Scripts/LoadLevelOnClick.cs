using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnClick : MonoBehaviour
{
    public void Clicked(string levelName){
        SceneManager.LoadScene(levelName);
    }
}
