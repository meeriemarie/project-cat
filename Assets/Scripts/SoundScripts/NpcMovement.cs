using UnityEngine;

public class NPCFootstepLoop : MonoBehaviour
{
    public string startFootstepEvent = "Play_concrete_footsteps_6752";
    public string stopFootstepEvent = "Stop_concrete_footsteps_6752";

  private Vector3 lastPosition;
    private float checkInterval = 0.1f;
    private float checkTimer = 0f;
    private bool isPlaying = false;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            float distance = Vector3.Distance(transform.position, lastPosition);
            bool isMoving = distance > 0.001f;

            if (isMoving && !isPlaying)
            {
                AkUnitySoundEngine.PostEvent(startFootstepEvent, gameObject);
                isPlaying = true;
            }
            else if (!isMoving && isPlaying)
            {
                AkUnitySoundEngine.PostEvent(stopFootstepEvent, gameObject);
                isPlaying = false;
            }

            lastPosition = transform.position;
            checkTimer = 0f;
        }
    }
}