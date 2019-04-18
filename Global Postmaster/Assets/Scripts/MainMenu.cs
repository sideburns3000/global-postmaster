using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // this function is called when the "New Game" button is clicked
    public void NewGame()
    {
        StartCoroutine(Manager.instance.LoadAsyncScene("LetterSorting"));
    }
    
    // this function is called when the "Quit" button is clicked
    public void Quit()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
