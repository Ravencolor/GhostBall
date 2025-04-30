using UnityEngine;

public class Kill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.dead = true;
                playerController.PlaySound(playerController.audioDictionary["Sad"]);
                Debug.Log("TUE");
            }
        }
    }
}
