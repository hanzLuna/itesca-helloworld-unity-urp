using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Player : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMesh;

    [SerializeField]
    float moveSpeed = 2f;

    [SerializeField] 
    float jumpHeight =5f;

    Animator anim;

    int score;
    AudioSource  po;
    
    Rigidbody rb;
    GameInputs gameInputs;
   
   ///////PARA DETECTAR EL RAYO//////
    [SerializeField] 
    Color rayColor = Color.magenta;
    [SerializeField] 
    float rayDistance = 5;

    [SerializeField] 
    LayerMask groundLayer; //layer del suelo paa saber que es esto

    [SerializeField] 
    Transform rayTransform;
    
    void Awake()
    {
        gameInputs = new GameInputs();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody >();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameInputs.Land.Jump.performed += _=> Jump();
        po = GetComponent<AudioSource>();
    }

    void Jump()
    {

        if(IsGrounding)
        {
             rb.AddForce(Vector3.up * jumpHeight,ForceMode.Impulse);
        }
        
         
    }

    void FixedUpdate() //detecta la fisica
    {
        
    }

    bool IsGrounding => Physics.Raycast(rayTransform.position,-transform.up,rayDistance,groundLayer); //dice si esta tocando o no algo
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

    void Movement()
    {
       
        if(IsMoving)
        {
            transform.Translate( Vector3.forward * Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.LookRotation(new Vector3(Axis.x,0f,Axis.y));
            //anim.SetBool("isRunning",true);
        }   
        
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

    void OnDrawGizmosSelected() 
    {
        //gizmo es como las lineas que forman un objeto, la apertura de la camara etc.
        Gizmos.color = rayColor;
        Gizmos.DrawRay(rayTransform.position, -transform.up * rayDistance);
    }
}
