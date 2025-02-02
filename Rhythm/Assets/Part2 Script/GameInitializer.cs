using UnityEngine;
using TMPro;

public class GameInitializer : MonoBehaviour
{
    public GameObject player;
    public GameObject restartButton;
    public Transform cameraTarget;
    public GameObject victoryFlag;
    public GameObject Hint;
    public float cameraMoveSpeed = 20.0f;
    private Vector3 initialCameraPosition;
    public CameraFollow cameraFollowScript;
    public GameObject beatBar;
    public TextMeshProUGUI scoreText;
    public float targetOrthographicSize = 25f;
    private float initialOrthographicSize;
    public GameObject HealthBar;
    void Start()
    {
        beatBar.SetActive(false);
        victoryFlag.SetActive(false);
        if (!GameSetting.IsRestarting)
        {
            initialOrthographicSize = Camera.main.orthographicSize;
            restartButton.SetActive(false);
            scoreText.enabled = false;
            Hint.SetActive(false);
            HealthBar.SetActive(false);
            initialCameraPosition = Camera.main.transform.position;
            Time.timeScale = 0f;
            if (cameraFollowScript != null) 
            {
                cameraFollowScript.enabled = false;
            }
            StartCoroutine(MoveCameraToTarget(cameraTarget.position, () => {
                StartCoroutine(MoveCameraBack(initialCameraPosition, () => {
                    ResumeGame();
                }));
            }));
        }
    }

    System.Collections.IEnumerator MoveCameraToTarget(Vector3 targetPosition, System.Action onCompleted = null)
    {
        float startTime = Time.unscaledTime;
        
        float journeyLength = Vector3.Distance(Camera.main.transform.position, targetPosition);
        float journey = 0f;
        while (Vector3.Distance(Camera.main.transform.position, new Vector3(targetPosition.x, targetPosition.y, initialCameraPosition.z)) > 0.01f)
        {
            float distCovered = (Time.unscaledTime - startTime) * cameraMoveSpeed;
            journey = distCovered / journeyLength;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(targetPosition.x, targetPosition.y, initialCameraPosition.z), cameraMoveSpeed * Time.unscaledDeltaTime);
            Camera.main.orthographicSize = Mathf.Lerp(initialOrthographicSize, targetOrthographicSize, journey); // 动态调整正交大小

            yield return null;
        }

        yield return new WaitForSecondsRealtime(2);
        onCompleted?.Invoke();
    }
    System.Collections.IEnumerator MoveCameraBack(Vector3 targetPosition, System.Action onCompleted = null)
    {   
        float startTime = Time.unscaledTime;
        float journeyLength = Vector3.Distance(Camera.main.transform.position, targetPosition);
        float journey = 0f;
        while (Vector3.Distance(Camera.main.transform.position, new Vector3(targetPosition.x, targetPosition.y, initialCameraPosition.z)) > 0.01f)
        {
            float distCovered = (Time.unscaledTime - startTime) * 120;
            journey = distCovered / journeyLength;
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(targetPosition.x, targetPosition.y, initialCameraPosition.z), 120 * Time.unscaledDeltaTime);
            Camera.main.orthographicSize = Mathf.Lerp(targetOrthographicSize, initialOrthographicSize, journey); // 动态调整正交大小

            yield return null;
        }
        yield return new WaitForSecondsRealtime(2);
        onCompleted?.Invoke();
    }
    void ResumeGame()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1f;
        // beatBar.SetActive(true);
        HealthBar.SetActive(true);
        restartButton.SetActive(true);
        Hint.SetActive(true);
        scoreText.enabled = true;
        Camera.main.orthographicSize = initialOrthographicSize;
        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = true;
        }
    }

}