using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // Thêm dòng này

public class Defeat : MonoBehaviour
{
    public Image victoryImage;
    public float duration = 1f; // Thời gian pop-up
    public Button lobbyButton; // Kéo button từ Inspector vào

    private void Start()
    {
        StartCoroutine(PopAndShineLoop());

        // Gán sự kiện click cho nút
        if (lobbyButton != null)
            lobbyButton.onClick.AddListener(LoadLobbyScene);
    }

    IEnumerator PopAndShineLoop()
    {
        float time = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        while (true)
        {
            float loopTime = 0f;
            while (loopTime < 1f)
            {
                loopTime += Time.deltaTime;
                float alpha = Mathf.PingPong(loopTime * 2f, 1f);
                Color color = victoryImage.color;
                color.a = alpha;
                victoryImage.color = color;
                yield return null;
            }
        }
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
