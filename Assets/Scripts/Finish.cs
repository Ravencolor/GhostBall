using UnityEngine;

public class Finish : MonoBehaviour

{
    public AudioClip Audio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // V�rifie que c'est bien le joueur
        {   

            PlayerController player = other.GetComponent<PlayerController>();
            player.speed = 0;
            player.SetWinText();
            player.PlaySound(Audio);
        }
    }

}
