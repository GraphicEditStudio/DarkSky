using UnityEngine;
using Platinio.TweenEngine;


public class Door : MonoBehaviour
{
    //bool _isDoorOpen = false;
    Vector3 _doorClosedPos;
    Vector3 _doorOpenPos;
    public GameObject door;
    public GameObject doorUI;

    private void Awake()
    {
        _doorClosedPos = door.transform.position;
        _doorOpenPos = new Vector3(door.transform.position.x, 5f, door.transform.position.z);
        doorUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            doorUI.SetActive(true);
        }


    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        if (other.CompareTag("Player") && Input.GetKey("e"))
        {
            OpeningDoor();
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorUI.SetActive(false);
            CloseingDoor();

        }
    }



    void OpeningDoor()
    {
        doorUI.SetActive(false);
        door.transform.Move(_doorOpenPos, 2.0f);
    }


    void CloseingDoor()
    {
        doorUI.SetActive(false);
        door.transform.Move(_doorClosedPos, 2.0f);
    }

}
