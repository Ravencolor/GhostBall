using UnityEngine;

public class Saw : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 100f;
    public float range = 1.41f;
    private bool movingForward = true;
    private float startLocalZ;
    private Transform parentTransform;

    void Start()
    {
        parentTransform = transform.parent;
        startLocalZ = transform.localPosition.z;
    }

    void Update()
    {
        // Mouvement sur l'axe Z relatif à l'objet parent
        float step = moveSpeed * Time.deltaTime;
        if (movingForward)
        {
            transform.localPosition += new Vector3(0, 0, step);
            if (transform.localPosition.z >= startLocalZ + range)
                movingForward = false;
        }
        else
        {
            transform.localPosition -= new Vector3(0, 0, step);
            if (transform.localPosition.z <= startLocalZ - range)
                movingForward = true;
        }

        // Rotation sur l'axe X sans impacter le déplacement
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0, Space.Self);
    }
}
