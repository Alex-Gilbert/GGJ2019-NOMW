using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MovieController : MonoBehaviour
{

    private VideoPlayer _player;
    public VideoClip scene2;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<VideoPlayer>();
    }

    [ContextMenu("Switch")]
    public void SwitchToScene2()
    {
        //_player.Stop();
        _player.clip = scene2;
        _player.frame = 0;
        _player.isLooping = false;
    }

    private void Update()
    {
        if (_player.isPaused)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
