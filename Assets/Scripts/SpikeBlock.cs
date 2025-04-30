using UnityEngine;
using System.Collections;

public class SpikeBlock : MonoBehaviour
{
    public float damage = 10f;
    public Transform childObject;
    public float attractionSpeed = 5f;
    public float returnSpeed = 2f;
    private Vector3 initialPosition;

    void Start()
    {
        if (childObject != null)
        {
            initialPosition = childObject.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (childObject != null)
            {
                StartCoroutine(AttractAndReturnChild());
            }
        }
    }

    private IEnumerator AttractAndReturnChild()
    {
        while (Vector3.Distance(childObject.position, transform.position) > 0.1f)
        {
            childObject.position = Vector3.MoveTowards(childObject.position, transform.position, attractionSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (Vector3.Distance(childObject.position, initialPosition) > 0.1f)
        {
            childObject.position = Vector3.MoveTowards(childObject.position, initialPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
