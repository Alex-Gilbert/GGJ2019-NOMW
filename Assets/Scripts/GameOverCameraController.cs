using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCameraController : MonoBehaviour
{
    public Transform[] Targets;

    private Transform target;
    
    [Range(0,1)]
    public float speed;

    private int i = 0;
    
    private void Start()
    {
        Cursor.visible = true;
        
        target = Targets[0];
        
        transform.position = target.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var myPos = transform.position;
        var targetPos = target.position;

        var newPos = Vector3.Lerp(myPos, targetPos, speed);

        transform.position = newPos;
    }

    public void Next()
    {
        i++;
        i %= Targets.Length;

        target = Targets[i];
    }
    
    public void Prev()
    {
        i--;
        i %= Targets.Length;
        target = Targets[i];
    }

    public void Restart()
    {
        Destroy(GameObject.FindWithTag("MonaLisa"));
        Destroy(GameObject.FindWithTag("StarryNight"));
        Destroy(GameObject.FindWithTag("Jesus"));
        Destroy(GameObject.FindWithTag("AudioMaster"));

        SceneManager.LoadScene(0);
    }
}
