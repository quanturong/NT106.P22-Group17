using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CleanSpinningWheel : MonoBehaviour
{
    [Header("Wheel Settings")]
    public int numberOfSections = 8;
    public string[] prizeNames = { "100 Xu", "Thẻ Cào", "500 Xu", "Skin VIP", "1000 Xu", "Kim Cương", "JACKPOT!", "Lượt Chơi" };

    [Header("Animation")]
    public float spinDuration = 3f;
    public float minRotations = 5f;
    public float maxRotations = 10f;

    [Header("Auto Setup")]
    public bool setupOnStart = true;

    private bool isSpinning = false;
    private float currentRotation = 0f;
    private GameObject wheelObject;
    private Button spinButton;
    private Text resultText;

    void Start()
    {
        if (setupOnStart)
        {
            SetupWheel();
        }
    }

    void SetupWheel()
    {
        // Tạo Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // EventSystem
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Background
        CreateBackground(canvas);

        // Title
        CreateTitle(canvas);

        // Wheel
        CreateWheel(canvas);

        // Button
        CreateButton(canvas);

        // Result text
        CreateResult(canvas);

        Debug.Log("Clean Wheel setup complete!");
    }

    void CreateBackground(Canvas canvas)
    {
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(canvas.transform, false);
        Image bgImg = bg.AddComponent<Image>();
        bgImg.color = new Color(0.2f, 0.3f, 0.8f);

        RectTransform bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
    }

    void CreateTitle(Canvas canvas)
    {
        GameObject title = new GameObject("Title");
        title.transform.SetParent(canvas.transform, false);
        Text titleText = title.AddComponent<Text>();
        titleText.text = "VÒNG QUAY MAY MẮN";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 30;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.fontStyle = FontStyle.Bold;

        RectTransform titleRect = title.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.85f);
        titleRect.anchorMax = new Vector2(1, 0.95f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;
    }

    void CreateWheel(Canvas canvas)
    {
        // Main wheel container - TĂNG KÍCH THƯỚC
        wheelObject = new GameObject("Wheel");
        wheelObject.transform.SetParent(canvas.transform, false);
        RectTransform wheelRect = wheelObject.AddComponent<RectTransform>();
        wheelRect.anchorMin = new Vector2(0.5f, 0.5f);
        wheelRect.anchorMax = new Vector2(0.5f, 0.5f);
        wheelRect.sizeDelta = new Vector2(400, 400); // Tăng từ 300 lên 400
        wheelRect.anchoredPosition = new Vector2(0, 20);

        // Wheel background
        Image wheelBg = wheelObject.AddComponent<Image>();
        wheelBg.sprite = CreateSimpleCircle();
        wheelBg.color = Color.black;

        // Create 8 colored sections
        Color[] colors = {
            new Color(1f, 0.4f, 0.4f),      // Đỏ
            new Color(0.4f, 0.8f, 1f),      // Xanh da trời
            new Color(0.4f, 1f, 0.4f),      // Xanh lá
            new Color(1f, 1f, 0.4f),        // Vàng
            new Color(1f, 0.4f, 1f),        // Hồng
            new Color(0.6f, 0.4f, 1f),      // Tím
            new Color(1f, 0.6f, 0.2f),      // Cam
            new Color(0.4f, 1f, 1f)         // Cyan
        };

        for (int i = 0; i < numberOfSections; i++)
        {
            CreateColoredSection(i, colors[i % colors.Length]);
        }

        // Center circle - TĂNG KÍCH THƯỚC
        GameObject center = new GameObject("Center");
        center.transform.SetParent(wheelObject.transform, false);
        Image centerImg = center.AddComponent<Image>();
        centerImg.sprite = CreateSimpleCircle();
        centerImg.color = Color.white;
        RectTransform centerRect = center.GetComponent<RectTransform>();
        centerRect.sizeDelta = new Vector2(50, 50); // Tăng từ 40 lên 50
        centerRect.anchoredPosition = Vector2.zero;

        // Pointer
        CreatePointer(canvas);
    }

    void CreateColoredSection(int index, Color color)
    {
        GameObject section = new GameObject($"Section_{index}");
        section.transform.SetParent(wheelObject.transform, false);

        Image sectionImg = section.AddComponent<Image>();
        sectionImg.sprite = CreateWedgeSprite();
        sectionImg.color = color;

        RectTransform sectionRect = section.GetComponent<RectTransform>();
        sectionRect.sizeDelta = new Vector2(380, 380); // Tăng từ 280 lên 380
        sectionRect.anchoredPosition = Vector2.zero;

        // Rotate section to position
        float angle = (360f / numberOfSections) * index;
        section.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // Add text
        AddSectionText(section, index);
    }

    void AddSectionText(GameObject section, int index)
    {
        GameObject textObj = new GameObject($"Text_{index}");
        textObj.transform.SetParent(section.transform, false);

        Text sectionText = textObj.AddComponent<Text>();
        sectionText.text = prizeNames[index % prizeNames.Length];
        sectionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        sectionText.fontSize = 12; // Tăng từ 8 lên 12
        sectionText.color = Color.white;
        sectionText.alignment = TextAnchor.MiddleCenter;
        sectionText.fontStyle = FontStyle.Bold;

        // Add outline for better readability
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2, 2); // Tăng outline

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(70, 25); // Tăng kích thước text

        // Position text in center of wedge section
        float anglePerSection = 360f / numberOfSections;
        float middleAngle = anglePerSection * 0.5f;
        float distance = 110f; // Tăng từ 80f lên 110f

        // Calculate position using trigonometry
        float radians = middleAngle * Mathf.Deg2Rad;
        float x = Mathf.Sin(radians) * distance;
        float y = Mathf.Cos(radians) * distance;

        textRect.anchoredPosition = new Vector2(x, y);

        // Rotate text to be upright and readable
        if (middleAngle > 90f && middleAngle < 270f)
        {
            // For bottom sections, flip text to keep it readable
            textRect.localRotation = Quaternion.Euler(0, 0, -middleAngle + 180f);
        }
        else
        {
            // For top sections, normal rotation
            textRect.localRotation = Quaternion.Euler(0, 0, -middleAngle);
        }
    }

    void CreatePointer(Canvas canvas)
    {
        GameObject pointer = new GameObject("Pointer");
        pointer.transform.SetParent(canvas.transform, false);

        Image pointerImg = pointer.AddComponent<Image>();
        pointerImg.sprite = CreateBetterTriangle();
        pointerImg.color = new Color(0.95f, 0.2f, 0.2f);

        RectTransform pointerRect = pointer.GetComponent<RectTransform>();
        pointerRect.sizeDelta = new Vector2(40, 50);

        // FIX: Tính toán chính xác vị trí pointer
        // Wheel radius = 200 (400/2), pointer nên ở edge của wheel
        float wheelRadius = 200f;
        float pointerOffset = 30f; // Khoảng cách từ edge vào trong
        pointerRect.anchoredPosition = new Vector2(0, wheelRadius - pointerOffset + 20); // +20 là wheel position offset

        // Add shadow effect
        GameObject shadow = new GameObject("PointerShadow");
        shadow.transform.SetParent(pointer.transform, false);
        Image shadowImg = shadow.AddComponent<Image>();
        shadowImg.sprite = CreateBetterTriangle();
        shadowImg.color = new Color(0, 0, 0, 0.3f);

        RectTransform shadowRect = shadow.GetComponent<RectTransform>();
        shadowRect.sizeDelta = new Vector2(40, 50);
        shadowRect.anchoredPosition = new Vector2(2, -2);
        shadowRect.SetAsFirstSibling();
    }

    void CreateButton(Canvas canvas)
    {
        GameObject buttonObj = new GameObject("SpinButton");
        buttonObj.transform.SetParent(canvas.transform, false);

        Button button = buttonObj.AddComponent<Button>();
        Image buttonImg = buttonObj.AddComponent<Image>();
        buttonImg.sprite = CreateSimpleCircle();
        buttonImg.color = new Color(0.95f, 0.2f, 0.2f); // Màu đỏ giống pointer

        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(180, 60); // Tăng kích thước button
        buttonRect.anchoredPosition = new Vector2(0, -150); // Điều chỉnh vị trí

        // Button text
        GameObject buttonText = new GameObject("ButtonText");
        buttonText.transform.SetParent(buttonObj.transform, false);
        Text btnText = buttonText.AddComponent<Text>();
        btnText.text = "QUAY NGAY!";
        btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        btnText.fontSize = 20; // Tăng font size
        btnText.color = Color.white;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.fontStyle = FontStyle.Bold;

        // Add outline cho button text
        Outline btnOutline = buttonText.AddComponent<Outline>();
        btnOutline.effectColor = Color.black;
        btnOutline.effectDistance = new Vector2(2, 2);

        RectTransform btnTextRect = buttonText.GetComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.offsetMin = Vector2.zero;
        btnTextRect.offsetMax = Vector2.zero;

        spinButton = button;
        spinButton.onClick.AddListener(SpinWheel);
    }

    void CreateResult(Canvas canvas)
    {
        // Container
        GameObject resultContainer = new GameObject("ResultContainer");
        resultContainer.transform.SetParent(canvas.transform, false);

        RectTransform containerRect = resultContainer.AddComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(400, 50); // Tăng kích thước
        containerRect.anchoredPosition = new Vector2(0, -220); // Điều chỉnh vị trí

        // Background
        GameObject resultBg = new GameObject("ResultBg");
        resultBg.transform.SetParent(resultContainer.transform, false);

        Image bgImage = resultBg.AddComponent<Image>();
        bgImage.sprite = CreateSimpleCircle();
        bgImage.color = new Color(1, 1, 1, 0.9f); // Tăng opacity

        RectTransform bgRect = resultBg.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // Text
        GameObject resultTextObj = new GameObject("ResultText");
        resultTextObj.transform.SetParent(resultContainer.transform, false);

        Text result = resultTextObj.AddComponent<Text>();
        result.text = "Nhấn nút để quay!";
        result.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        result.fontSize = 16; // Tăng font size
        result.color = Color.black;
        result.alignment = TextAnchor.MiddleCenter;
        result.fontStyle = FontStyle.Bold;

        RectTransform textRect = resultTextObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        resultText = result;
    }

    // Simple sprite creation methods
    Sprite CreateWedgeSprite()
    {
        int size = 128;
        Texture2D texture = new Texture2D(size, size);

        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 2;
        float innerRadius = 8f;
        float anglePerSection = 360f / numberOfSections;

        // Clear texture
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }

        // Draw wedge/pie slice
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2 pos = new Vector2(x, y);
                Vector2 dir = pos - center;
                float distance = dir.magnitude;

                if (distance <= radius && distance >= innerRadius)
                {
                    // Calculate angle from top (0 degrees at top, clockwise)
                    float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                    if (angle < 0) angle += 360f;

                    // Check if point is within wedge angle
                    // Add small offset to prevent gaps
                    if (angle <= anglePerSection + 1f)
                    {
                        texture.SetPixel(x, y, Color.white);
                    }
                }
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    Sprite CreateSimpleCircle()
    {
        int size = 64;
        Texture2D texture = new Texture2D(size, size);

        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 1;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2 pos = new Vector2(x, y);
                float distance = Vector2.Distance(pos, center);

                if (distance <= radius)
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    Sprite CreateBetterTriangle()
    {
        int size = 64;
        Texture2D texture = new Texture2D(size, size);

        // Clear texture
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }

        // Draw better triangle pointing down
        int centerX = size / 2;
        int topY = size - 8;
        int bottomY = 8;
        int width = 20;

        for (int y = bottomY; y <= topY; y++)
        {
            // Calculate triangle width at this height
            float progress = (float)(y - bottomY) / (topY - bottomY);
            int currentWidth = Mathf.RoundToInt(width * (1f - progress));

            for (int x = centerX - currentWidth; x <= centerX + currentWidth; x++)
            {
                if (x >= 0 && x < size && y >= 0 && y < size)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    Sprite CreateSimpleTriangle()
    {
        int size = 32;
        Texture2D texture = new Texture2D(size, size);

        // Clear
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }

        // Draw triangle
        for (int y = 5; y < size - 5; y++)
        {
            int width = (size - y) / 2;
            for (int x = size / 2 - width / 2; x < size / 2 + width / 2; x++)
            {
                if (x >= 0 && x < size)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    // Spinning logic
    public void SpinWheel()
    {
        if (isSpinning) return;
        StartCoroutine(SpinAnimation());
    }

    IEnumerator SpinAnimation()
    {
        isSpinning = true;

        if (spinButton != null)
            spinButton.interactable = false;

        if (resultText != null)
            resultText.text = "Đang quay...";

        // Calculate target
        float rotations = Random.Range(minRotations, maxRotations);
        float targetRotation = currentRotation + (rotations * 360f);
        float startRotation = currentRotation;

        float elapsedTime = 0f;

        // Animate
        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / spinDuration;

            // Easing
            float easeProgress = 1f - Mathf.Pow(1f - progress, 3f);
            currentRotation = Mathf.Lerp(startRotation, targetRotation, easeProgress);

            if (wheelObject != null)
            {
                wheelObject.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
            }

            yield return null;
        }

        // Final
        currentRotation = targetRotation;
        if (wheelObject != null)
        {
            wheelObject.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }

        // Result
        int winningSection = CalculateResult();
        ShowResult(winningSection);

        if (spinButton != null)
            spinButton.interactable = true;

        isSpinning = false;
    }

    int CalculateResult()
    {
        // Normalize rotation angle
        float normalizedRotation = currentRotation % 360f;
        if (normalizedRotation < 0) normalizedRotation += 360f;

        float anglePerSection = 360f / numberOfSections; // 45° cho 8 sections

        // FIX: Logic đơn giản và chính xác
        // Khi wheel quay clockwise, section tương ứng với rotation
        // Thêm offset nửa section để pointer chỉ vào center của section
        float adjustedRotation = (normalizedRotation + (anglePerSection / 2f)) % 360f;

        int sectionIndex = Mathf.FloorToInt(adjustedRotation / anglePerSection);
        sectionIndex = sectionIndex % numberOfSections;

        return sectionIndex;
    }

    void ShowResult(int sectionIndex)
    {
        string prize = prizeNames[sectionIndex % prizeNames.Length];

        if (resultText != null)
        {
            resultText.text = $"Chúc mừng! Bạn trúng: {prize}";
        }

        // DEBUG: Log để kiểm tra
        Debug.Log($"Rotation: {currentRotation:F1}°, Section Index: {sectionIndex}, Prize: {prize}");
    }
}