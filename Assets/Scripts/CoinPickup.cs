using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Reward")]
    [SerializeField] private int moneyAmount = 10;

    [Header("Animation")]
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float floatHeight = 0.15f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Sound")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float soundVolume = 0.8f;

    private Vector3 startPosition;
    private bool collected = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime
        );

        float y =
            startPosition.y +
            Mathf.Sin(Time.time * floatSpeed) *
            floatHeight;

        transform.position = new Vector3(
            transform.position.x,
            y,
            transform.position.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (ResourceManager.Instance == null)
            return;

        collected = true;

        ResourceManager.Instance.AddMoney(
            moneyAmount
        );

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(
                pickupSound,
                transform.position,
                soundVolume
            );
        }

        Debug.Log(
            "Coin collected! +" +
            moneyAmount +
            " Money"
        );

        Destroy(gameObject);
    }
}