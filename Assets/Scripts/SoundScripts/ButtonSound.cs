using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void OnClick()
    {
        AkUnitySoundEngine.PostEvent("Play_tap_notification_180637", gameObject);
    }
}
