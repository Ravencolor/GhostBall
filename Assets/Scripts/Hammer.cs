using UnityEngine;

public class Hammer : MonoBehaviour
{
    public float angleMax = 45f;
    public float vitesse = 2f;
    public float forceImpact = 50f;
    public AudioClip Audio;

    private float angleInitial;
    private float yRotation;  // Pour garder la direction initiale de l'objet

    void Start()
    {
        angleInitial = transform.eulerAngles.z;
        yRotation = transform.eulerAngles.y;  // Sauvegarde de la rotation initiale en Y
    }

    void Update()
    {
        float angle = angleInitial + Mathf.Sin(Time.time * vitesse) * angleMax;
        transform.rotation = Quaternion.Euler(0, yRotation, angle);  // Garder la direction initiale
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("bonk");
        PlayerController controller = other.gameObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                rb.AddForce(direction * forceImpact, ForceMode.Impulse);
                controller.GetComponent<PlayerController>().PlaySound(Audio);
            }
        }
    }
}
