using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField]
    private int damage = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().TakeDamage(damage);
            Dismiss();
        }
    }

    private void Start() {

        transform.GetComponent<Rigidbody2D>().velocity =  ( 
            GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position -
            GameObject.FindGameObjectWithTag("FinalBoss").GetComponent<FinalBoss>().playerCheck.transform.position).normalized * 5f;

    }

    private void Dismiss()
    {
        Destroy(gameObject);
    }

}
