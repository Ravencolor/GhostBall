using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player; 
    public float sensitivity = 2f; 
    public float distanceFromPlayer = 5f; 
    public float maxYAngle = 80f; 

    private Vector2 lookInput;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // Initialise la position de la caméra à une certaine distance du joueur
        transform.position = player.position - transform.forward * distanceFromPlayer;
    }

    void LateUpdate()
    {
        // Appliquer la rotation de la caméra avec la souris
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        // Rotation horizontale (autour du joueur)
        rotationY += mouseX;

        // Rotation verticale (haut/bas), avec une limite
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);

        // Appliquer la rotation
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.position = player.position - (rotation * Vector3.forward * distanceFromPlayer);
        transform.LookAt(player.position);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
