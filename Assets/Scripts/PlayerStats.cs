using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float playTime { get; private set; } = 0f;
    public int objectsCollected { get; private set; } = 0;

    private void Update()
    {
        playTime += Time.deltaTime;
    }

    public void CollectObject()
    {
        objectsCollected++;
    }
}
