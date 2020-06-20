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

    private void Dismiss()
    {
        Destroy(gameObject);
    }

}
