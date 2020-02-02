using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextRoom : MonoBehaviour
{
    // Start is called before the first frame update
    public string levelToLoad;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "player_char")
        {
            print("collide");
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
