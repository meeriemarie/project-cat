using UnityEngine;

public class DogBark : MonoBehaviour
{
    public string barkEvent = "Play_RandomBark";
    public float minDelay =0.1f;
    public float maxDelay =0.2f;

    private void Start()
    {
        StartCoroutine(BarkRoutine());
    }

    System.Collections.IEnumerator BarkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            AkUnitySoundEngine.PostEvent(barkEvent, gameObject);
        }
    }
}
