using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator.SetTrigger("isClosed");
    }

    //private void OnTriggerEnter(Collider other)
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("isOpen");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("isClosed");
        }
    }

}


