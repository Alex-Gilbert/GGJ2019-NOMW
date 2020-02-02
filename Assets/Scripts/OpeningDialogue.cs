using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OpeningDialogue : MonoBehaviour
{
    public Text text;
    public string[] openingSpeech;
    public float lettersPerSecond;

    private AudioSource audio;
    public AudioClip[] mumbles;

    public MoveScript playerMoveScript;

    public Countdown Countdown;
    
    private float playerSpeed;
    
    private void Start()
    {
        Cursor.visible = false;
        
        text.text = "";
        audio = GetComponent<AudioSource>();
        StartCoroutine(Dialogue());

        playerSpeed = playerMoveScript.speed;
        playerMoveScript.speed = 0;
    }

    IEnumerator Dialogue()
    {
        for (int i = 0; i < openingSpeech.Length; ++i)
        {
            audio.PlayOneShot(mumbles[Random.Range(0, mumbles.Length)]);
                
            var s = openingSpeech[i];

            for (int j = 0; j <= s.Length; ++j)
            {
                text.text = s.Substring(0, j);
                yield return new WaitForSeconds(1.0f / lettersPerSecond);
            }
            
            while (!Input.GetMouseButtonDown(0))
                yield return null;
        }
        
        playerMoveScript.speed = playerSpeed;

        var audioMaster = GameObject.FindWithTag("AudioMaster");
        if (audioMaster != null)
        {
            var a = audioMaster.GetComponent<AudioMaster>();
            a.FadeInLayerTwo();
        }
        
        Countdown.StartCountDown();
        gameObject.SetActive(false);
    }
}
