using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public float speed;
    public Animator _anim;
    
    public Animator PolesAnimator;

    private Animator _globalAnim;
    
    public float leftScale;
    public Transform CameraTarget;

    public Transform arrow;

    private ArtPiece activeArtPiece;

    private AudioSource _audioSource;

    public AudioClip[] enterPaintingSound;
    public AudioClip leavePaintingSound;
    
    // Start is called before the first frame update
    void Start()
    {
        leftScale = _anim.gameObject.transform.localScale.x;

        _globalAnim = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
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

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (activeArtPiece != null)
            {
                // fadeout of everything
                
                _globalAnim.SetTrigger("FadeOut");
                
                PolesAnimator.SetTrigger("FadeOut");
                
                arrow.gameObject.SetActive(false);
                
                var follow = Camera.main.GetComponent<FollowPlayer>();
                follow.target = activeArtPiece.CameraTarget;

                var uiAnim = GameObject.FindWithTag("UI").GetComponent<Animator>();
                uiAnim.SetTrigger("UIEnter");

                activeArtPiece.ArtWorkShaderDispatcher.CanDraw = true;
                
                _audioSource.PlayOneShot(enterPaintingSound[Random.Range(0, enterPaintingSound.Length)]);
            }
        }
        
        _anim.gameObject.transform.localScale = scale;
    }

    public void ExitPainting()
    {
        _globalAnim.SetTrigger("FadeIn");
                
        PolesAnimator.SetTrigger("FadeIn");
                
        arrow.gameObject.SetActive(true);
                
        var follow = Camera.main.GetComponent<FollowPlayer>();
        follow.target = CameraTarget;

        var uiAnim = GameObject.FindWithTag("UI").GetComponent<Animator>();
        uiAnim.SetTrigger("UIExit");

        activeArtPiece.ArtWorkShaderDispatcher.CanDraw = false;
        
        _audioSource.PlayOneShot(leavePaintingSound);
    }
    
    public void EnableArrow(ArtPiece piece)
    {
        activeArtPiece = piece;
        arrow.gameObject.SetActive(true);
    }
    
    public void DisableArow()
    {
        activeArtPiece = null;
        arrow.gameObject.SetActive(false);
    }

    public void SetColorYellow()
    {
        if (activeArtPiece != null)
        {
            activeArtPiece.ArtWorkShaderDispatcher.paintColor = new Color(1, 1, 0.5f);
        }
    }
    
    public void SetColorOrange()
    {
        if (activeArtPiece != null)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#FB9F3BFF", out color))
            {
                activeArtPiece.ArtWorkShaderDispatcher.paintColor = color;
            }
        }
    }
    
    public void SetColorRed()
    {
        if (activeArtPiece != null)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#FF7C74FF", out color))
            {
                activeArtPiece.ArtWorkShaderDispatcher.paintColor = color;
            }
        }
    }
    
    public void SetColorGreen()
    {
        if (activeArtPiece != null)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#9FFF74FF", out color))
            {
                activeArtPiece.ArtWorkShaderDispatcher.paintColor = color;
            }
        }
    }
    
    public void SetColorBlue()
    {
        if (activeArtPiece != null)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#8AECFFFF", out color))
            {
                activeArtPiece.ArtWorkShaderDispatcher.paintColor = color;
            }
        }
    }
    
    public void SetColorPurple()
    {
        if (activeArtPiece != null)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#BE8AFFFF", out color))
            {
                activeArtPiece.ArtWorkShaderDispatcher.paintColor = color;
            }
        }
    }
    
    public void SetColorWhite()
    {
        if (activeArtPiece != null)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#FFFFFFFF", out color))
            {
                activeArtPiece.ArtWorkShaderDispatcher.paintColor = color;
            }
        }
    }
    
    public void SetColorBlack()
    {
        if (activeArtPiece != null)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#000000FF", out color))
            {
                activeArtPiece.ArtWorkShaderDispatcher.paintColor = color;
            }
        }
    }

    public void SetBrushScaleModifier(float scale)
    {
        if (activeArtPiece != null)
        {
            activeArtPiece.ArtWorkShaderDispatcher.brushScaleModifier = scale;
        }
    }
    
    public void SetActiveToolToPaintBrush()
    {
        if (activeArtPiece != null)
        {
            activeArtPiece.ArtWorkShaderDispatcher.activeTool = ArtTools.PaintBrush;
        }
    }
    
    public void SetActiveToolToEyeDrop()
    {
        if (activeArtPiece != null)
        {
            activeArtPiece.ArtWorkShaderDispatcher.activeTool = ArtTools.EyeDrop;
        }
    }
}
