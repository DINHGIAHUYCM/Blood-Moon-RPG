using UnityEngine;
using System.Collections;

public class EarthquakeSimulation : MonoBehaviour
{
    public Camera[] cameras; // Mảng chứa các camera
    public float earthquakeDuration = 2f; // Thời gian mô phỏng động đất
    public float earthquakeInterval = 5f; // Khoảng thời gian giữa các lần mô phỏng động đất
    public float shakeIntensity = 0.1f; // Cường độ rung
    public AudioClip earthquakeSound; // Âm thanh động đất
    [Range(0f, 1f)] public float startVolume = 0f; // Âm lượng khi bắt đầu động đất
    [Range(0f, 1f)] public float maxVolume = 1f; // Âm lượng tối đa giữa thời gian động đất

    private bool isEarthquakeActive = false; // Biến để kiểm tra xem đang có động đất hay không
    private Vector3[] playerPositions; // Mảng lưu trữ vị trí của các đối tượng có tag "Player"

    private void Start()
    {
        // Gọi hàm Mô phỏng động đất sau mỗi khoảng thời gian
        StartCoroutine(SimulateEarthquakeLoop());
    }

    private IEnumerator SimulateEarthquakeLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(earthquakeInterval);
            yield return StartCoroutine(SimulateEarthquake());
            yield return new WaitForSeconds(10f); // Chờ 10 giây trước khi bắt đầu động đất tiếp theo
        }
    }

    private IEnumerator SimulateEarthquake()
    {
        // Bắt đầu động đất chỉ khi không có động đất nào khác đang diễn ra và player không di chuyển
        if (!isEarthquakeActive && !ArePlayersMoving())
        {
            yield return StartCoroutine(ShakeCameras());
            PlayEarthquakeSound(startVolume);
        }
    }

    private IEnumerator ShakeCameras()
    {
        isEarthquakeActive = true; // Báo hiệu rằng đang có động đất

        foreach (Camera camera in cameras)
        {
            if (camera != null)
            {
                yield return StartCoroutine(ShakeCamera(camera));
            }
        }

        yield return new WaitForSeconds(earthquakeDuration);

        isEarthquakeActive = false; // Kết thúc động đất

        // Phát âm thanh động đất với âm lượng tối đa
        PlayEarthquakeSound(maxVolume);
    }

    private IEnumerator ShakeCamera(Camera camera)
    {
        Vector3 originalPosition = camera.transform.position;

        float elapsed = 0f;

        while (elapsed < earthquakeDuration)
        {
            // Tính toán vị trí mới của camera trong thời gian động đất
            float xOffset = Random.Range(-shakeIntensity, shakeIntensity);
            float yOffset = Random.Range(-shakeIntensity, shakeIntensity);
            Vector3 shakeOffset = new Vector3(xOffset, yOffset, 0f);

            camera.transform.position = originalPosition + shakeOffset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Reset vị trí của camera sau khi kết thúc động đất
        camera.transform.position = originalPosition;
    }

    private void PlayEarthquakeSound(float volume)
    {
        if (earthquakeSound != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = earthquakeSound;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource, earthquakeSound.length);
        }
    }

    private void SavePlayerPositions()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        playerPositions = new Vector3[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                playerPositions[i] = players[i].transform.position;
            }
        }
    }

private bool ArePlayersMoving()
{
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    if (players == null || players.Length == 0)
    {
        // If there are no players, consider them not moving
        return false;
    }

    for (int i = 0; i < players.Length; i++)
    {
        if (players[i] != null)
        {
            // Ensure the player object is not null before accessing its properties
            if (players[i].transform != null && playerPositions != null && playerPositions.Length > i)
            {
                // Check if the player's position has changed
                if (Vector3.Distance(players[i].transform.position, playerPositions[i]) > 0.01f)
                {
                    return true;
                }
            }
        }
    }

    return false;
}

}
