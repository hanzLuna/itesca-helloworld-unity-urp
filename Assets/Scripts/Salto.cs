using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salto : MonoBehaviour
{
public  Animator anim;
[SerializeField] private float jumpHeight =2f;

   public bool espacio;
    // Start is called before the first frame update
    void Start()
    {

    }
   public  void Jump()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y + jumpHeight,transform.position.z);
    }
    void Update() {
      espacio = Input.GetKeyDown(KeyCode.Space);
        anim.SetBool("isJumping",espacio);
    }
    // Update is called once per frame

}
