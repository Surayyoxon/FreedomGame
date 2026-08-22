using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ExternalConnection : MonoBehaviour
{
    [Header("Connection")]
    [SerializeField] private Transform villageCenter;

    [Header("Visual")]
    [SerializeField] private float lineHeight = 1f;
    [SerializeField] private float lineWidth = 0.12f;

    private LineRenderer line;
    private bool disconnected;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = 2;
        line.useWorldSpace = true;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        // Materialni kod orqali yaratamiz
        Shader shader =
            Shader.Find("Universal Render Pipeline/Unlit");

        if (shader == null)
        {
            shader = Shader.Find("Unlit/Color");
        }

        if (shader != null)
        {
            Material material =
                new Material(shader);

            material.color =
                new Color(0.1f, 0.7f, 1f, 1f);

            line.material = material;
        }
        else
        {
            Debug.LogWarning(
                "Line Renderer uchun shader topilmadi."
            );
        }
    }

    private void Start()
    {
        if (villageCenter == null)
        {
            Debug.LogError(
                "Village Center ulanmagan!"
            );

            return;
        }

        UpdateLine();
    }

    private void Update()
    {
        if (disconnected)
            return;

        if (villageCenter == null)
            return;

        UpdateLine();

        if (DependencyManager.Instance != null)
        {
            if (DependencyManager.Instance
                .GetIndependence() >= 100f)
            {
                Disconnect();
            }
        }
    }

    private void UpdateLine()
    {
        Vector3 start =
            transform.position +
            Vector3.up * lineHeight;

        Vector3 end =
            villageCenter.position +
            Vector3.up * lineHeight;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    private void Disconnect()
    {
        disconnected = true;

        line.enabled = false;

        Debug.Log(
            "External Supply connection disconnected!"
        );
    }
}