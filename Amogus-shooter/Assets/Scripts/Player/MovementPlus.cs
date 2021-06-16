using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine.UI;

public class MovementPlus : NetworkBehaviour
{
    Rigidbody rb;

    public float jumpBoosts = 5.0f;
    public float dashBoosts = 3.0f;
    int jumpCount = 1;
    int dashCount = 1;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    void Update()
    {
        Jump();
        Dash();
        JumpCountCheck();
    }

    public void Dash()
    {
        if (Input.GetKey(KeyCode.W) && dashCount == 1 && Input.GetKeyDown(KeyCode.C))
        {
            rb.AddRelativeForce(Vector3.forward * dashBoosts, ForceMode.Impulse);
            dashCount = 0;
            StartCoroutine(ResetDash());
        }
        else if (Input.GetKey(KeyCode.A) && dashCount == 1 && Input.GetKeyDown(KeyCode.C))
        {
            rb.AddRelativeForce(Vector3.left * dashBoosts, ForceMode.Impulse);
            dashCount = 0;
            StartCoroutine(ResetDash());
        }
        else if (Input.GetKey(KeyCode.S) && dashCount == 1 && Input.GetKeyDown(KeyCode.C))
        {
            rb.AddRelativeForce(Vector3.back * dashBoosts, ForceMode.Impulse);
            dashCount = 0;
            StartCoroutine(ResetDash());
        }
        else if (Input.GetKey(KeyCode.D) && dashCount == 1 && Input.GetKeyDown(KeyCode.C))
        {
            rb.AddRelativeForce(Vector3.right * dashBoosts, ForceMode.Impulse);
            dashCount = 0;
            StartCoroutine(ResetDash());
        }
        //else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.C) && dashCount == 1)
        //{
        //    Quaternion rotation = Quaternion.Euler(0, -45, 0);
        //    Vector3 direction = rotation * Vector3.forward;
        //    rb.AddRelativeForce(direction * dashBoosts, ForceMode.Impulse);
        //    dashCount = 0;
        //    StartCoroutine(ResetDash());
        //}
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(2.5f);
        dashCount = 1;
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 1)
        {
            rb.AddForce(Vector3.up * jumpBoosts, ForceMode.Impulse);
            jumpCount -= 1;
        }
    }

    public void JumpCountCheck()
    {
        if (jumpCount > 1)
        {
            jumpCount = 1;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        jumpCount += 1;
    }
}
