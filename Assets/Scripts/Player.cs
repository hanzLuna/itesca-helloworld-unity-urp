using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMesh;

    [SerializeField]
    float moveSpeed = 2f;

    [SerializeField] private float jumpHeight =2f;

    Animator anim;

    int score;
    AudioSource  po;
     
    GameInputs gameInputs;
    public  void Jump()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y + jumpHeight,transform.position.z);
         anim.SetBool("isJumping",true);
         
    }

    
    void Awake()
    {
        gameInputs = new GameInputs();
        anim = GetComponent<Animator>();
      
    }
    // Start is called before the first frame update
    void Start()
    {
        
        po = GetComponent<AudioSource>();
    }

    

    void OnEnable() {
        gameInputs.Enable();
    }

    void OnDisable() {
        gameInputs.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        //movement 3d
        Movement();
        anim.SetFloat("move", AxisMagnitudeAbs);
         
        
    }
void FixedUpdate()
{
     if ( anim.GetBool("isJumping"))
    {
        anim.SetBool("isJumping",false);
    }
     if ( anim.GetBool("isRunning"))
    {
        anim.SetBool("isRunning",false);
    }
}
    void Movement()
    {
       
        if(IsMoving)
        {
            transform.Translate( Vector3.forward * Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.LookRotation(new Vector3(Axis.x,0f,Axis.y));
            anim.SetBool("isRunning",true);
        }   
        var movem = new Vector3
        {
            x = Axis.x,
            z = Axis.y
        }.normalized;
    }
    /// <summary>
    /// Retunrs the axis with H input and V Input.
    /// </summary>
    /// <returns></returns>
     Vector2 Axis =>gameInputs.Land.Move.ReadValue<Vector2>();

    /// <summary>
    /// Check if player is moving with inputs H and V.
    /// </summary>
    bool IsMoving => AxisMagnitudeAbs > 0;


    /// <summary>
    /// Returns the magnitude of the Axis with inputs H and V.
    /// </summary>
    /// <returns></returns>
    float AxisMagnitudeAbs => Mathf.Abs(Axis.magnitude);

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Collectable"))
        {
            score++;
            po.Play();
            textMesh.text = $"Score: {score}";
            Destroy(other.gameObject);
        }   
    }
}
