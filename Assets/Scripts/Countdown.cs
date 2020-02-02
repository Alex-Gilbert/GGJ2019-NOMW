using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text text;

    public int minutes;
    public int seconds;

    private AudioSource _audio;

    public AudioClip whistle;

    private void Start()
    {
        text.text = seconds < 10 ? $"{minutes}:0{seconds}" : $"{minutes}:{seconds}";
        _audio = GetComponent<AudioSource>();
    }

    public void StartCountDown()
    {
        text.text = seconds < 10 ? $"{minutes}:0{seconds}" : $"{minutes}:{seconds}";
        StartCoroutine(Begin());
    }
    
    
    IEnumerator Begin()
    {
        while (seconds > 0 || minutes > 0)
        {
            seconds -= 1;
            if (seconds < 0)
            {
                seconds = 59;
                minutes -= 1;

                if (minutes == 0)
                {
                    var audioMaster = GameObject.FindWithTag("AudioMaster");
                    if (audioMaster != null)
                    {
                        var a = audioMaster.GetComponent<AudioMaster>();
                        a.StartLastMinute();
                    }
                }
            }

            text.text = seconds < 10 ? $"{minutes}:0{seconds}" : $"{minutes}:{seconds}";
            yield return new WaitForSeconds(1.0f);
        }
        
        var am = GameObject.FindWithTag("AudioMaster");
        if (am != null)
        {
            var a = am.GetComponent<AudioMaster>();
            a.StartGameOver();
        }
        
        _audio.PlayOneShot(whistle);
        
        yield return new WaitForSeconds(1.5f);
        
        GameObject.FindGameObjectWithTag("MonaLisa").GetComponent<ArtPiece>().Done();
        GameObject.FindGameObjectWithTag("StarryNight").GetComponent<ArtPiece>().Done();
        GameObject.FindGameObjectWithTag("Jesus").GetComponent<ArtPiece>().Done();
        
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        yield return null;
    }
}
