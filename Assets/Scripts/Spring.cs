using UnityEngine;

public class Spring : MonoBehaviour
{
    public float bounceForce = 10f;
    public float scaleMultiplier = 2f;
    public float resetTime = 0.5f;
    private Vector3 originalScale;
    public AudioClip Audio;

    void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                //// Augmente la taille
                //transform.localScale = new Vector3(originalScale.x, originalScale.y * scaleMultiplier, originalScale.z);

                // Pousse le joueur en l'air
                playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                other.GetComponent<PlayerController>().PlaySound(Audio);

                // Rétablit la taille après un délai
                Invoke("ResetScale", resetTime);
            }
        }
    }

    private void ResetScale()
    {
        transform.localScale = originalScale;
    }
}
