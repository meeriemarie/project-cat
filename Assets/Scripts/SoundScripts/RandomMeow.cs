using UnityEngine;

public class RandomMeow : MonoBehaviour
{
    public AK.Wwise.Event Play_RandomMeow; 

    public void PlayRandomSound()
    {
        Play_RandomMeow.Post(gameObject);
    }
}
