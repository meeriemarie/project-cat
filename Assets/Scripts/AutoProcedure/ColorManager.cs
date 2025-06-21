using UnityEngine;

public static class ColorManager
{
    public static void ApplyRandomPastelColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        float hue = Random.Range(0f, 1f);
        float saturation = Random.Range(0.4f, 0.6f); // Pastel range
        float value = Random.Range(0.85f, 1f);

        Color pastelColor = Color.HSVToRGB(hue, saturation, value);
        renderer.material.color = pastelColor;
    }
}
