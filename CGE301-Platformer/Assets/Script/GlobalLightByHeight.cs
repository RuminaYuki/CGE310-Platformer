using System.Reflection;
using UnityEngine;

public class GlobalLightByHeight : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private bool autoFindPlayerByTag = true;

    [Header("Height Range")]
    [SerializeField] private float minHeight = -5f;
    [SerializeField] private float maxHeight = 20f;

    [Header("Intensity Range")]
    [SerializeField] private float minIntensity = 0.3f;
    [SerializeField] private float maxIntensity = 1.2f;
    [SerializeField] private float smoothSpeed = 5f;

    private Component lightComponent;
    private PropertyInfo intensityProperty;
    private float currentIntensity;

    private void Awake()
    {
        TryResolveLightComponent();
        TryResolveTarget();
    }

    private void Update()
    {
        if (target == null)
        {
            TryResolveTarget();
            if (target == null) return;
        }

        if (intensityProperty == null || lightComponent == null)
        {
            TryResolveLightComponent();
            if (intensityProperty == null || lightComponent == null) return;
        }

        float t = Mathf.InverseLerp(minHeight, maxHeight, target.position.y);
        float desiredIntensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        currentIntensity = Mathf.Lerp(currentIntensity, desiredIntensity, smoothSpeed * Time.deltaTime);
        intensityProperty.SetValue(lightComponent, currentIntensity);
    }

    private void TryResolveTarget()
    {
        if (target != null || !autoFindPlayerByTag) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    private void TryResolveLightComponent()
    {
        Component[] components = GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            Component c = components[i];
            if (c == null) continue;

            PropertyInfo prop = c.GetType().GetProperty("intensity");
            if (prop == null || !prop.CanWrite || prop.PropertyType != typeof(float)) continue;

            lightComponent = c;
            intensityProperty = prop;
            currentIntensity = (float)intensityProperty.GetValue(lightComponent);
            return;
        }
    }
}
