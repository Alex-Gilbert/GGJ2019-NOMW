using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public float speed;
    public Animator _anim;

    public float leftScale;
    
    // Start is called before the first frame update
    void Start()
    {
        leftScale = _anim.gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        
        GetComponent<Rigidbody2D>().velocity = new Vector2(h, 0) * speed;
        _anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));

        var scale = _anim.gameObject.transform.localScale;
        
        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            scale.x = -leftScale;
        }
        
        if (GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            scale.x = leftScale;
        }

        _anim.gameObject.transform.localScale = scale;
    }
}
