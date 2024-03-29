using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraSizeController : MonoBehaviour
{
    public Camera mainCamera;
    public float sizeIncreaseAmount = 1.0f;
    public float transitionTime = 1.0f;
    public Button[] buttons;

    private float originalSize;
    private bool isButtonPressed;
    private float transitionTimer;

    private void Start()
    {
        originalSize = mainCamera.orthographicSize;

        // Map onClick events of buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i; // Save the button index to pass into the event
            buttons[i].onClick.AddListener(() => OnButtonPressed(buttonIndex));
        }
    }

    private void Update()
    {
        if (isButtonPressed)
        {
            mainCamera.orthographicSize += sizeIncreaseAmount * Time.deltaTime;
        }
    }

    private void OnButtonPressed(int buttonIndex)
    {
        if (!isButtonPressed)
        {
            isButtonPressed = true;
            transitionTimer = 0.0f;
            buttons[buttonIndex].interactable = false;
            Invoke("ResetCameraSize", transitionTime);
        }
    }

    private void ResetCameraSize()
    {
        isButtonPressed = false;
        StartCoroutine(TransitionCameraSize(originalSize, transitionTime));

        // Enable all buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    private IEnumerator TransitionCameraSize(float targetSize, float duration)
    {
        float initialSize = mainCamera.orthographicSize;
        float timer = 0.0f;

        while (timer <= duration)
        {
            float t = timer / duration;
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            timer += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
    }
}