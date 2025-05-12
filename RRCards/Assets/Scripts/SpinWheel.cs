using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SpinWheel : MonoBehaviour
{
    [System.Serializable]
    public class WheelSection
    {
        public string sectionName;
        public Color color;
        public float probability = 1f;
        public string reward;
    }

    [Header("Wheel Settings")]
    public int sectionsCount = 6;
    public float wheelRadius = 2f;
    public List<WheelSection> sections = new List<WheelSection>();
    public Transform wheelContainer;
    public Button spinButton;
    public TextMeshProUGUI resultText;

    [Header("Spinning Settings")]
    public float spinDuration = 3f;
    public float maxSpinSpeed = 720f;
    public AnimationCurve spinCurve;

    private bool isSpinning = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;

    public void Awake()
    {
        if (sections.Count == 0)
        {
            Color[] defaultColors = new Color[] {
                Color.red, Color.green, Color.red, Color.green, Color.red, Color.green
            };

            for (int i = 0; i < sectionsCount; i++)
            {
                sections.Add(new WheelSection
                {
                    sectionName = (i % 2 == 0) ? "ALIVE" : "DEAD",
                    color = defaultColors[i % defaultColors.Length],
                    reward = (i % 2 == 0) ? "ALIVE" : "DEAD"
                });
            }
        }

        CreateWheel();

        if (spinButton != null)
        {
            spinButton.onClick.AddListener(StartSpin);
        }
    }

    public void RebuildWheel()
    {
        CreateWheel();
    }

    void CreateWheel()
    {
        if (wheelContainer == null)
        {
            GameObject wheelObj = new GameObject("WheelContainer");
            wheelObj.transform.SetParent(transform, false);
            wheelContainer = wheelObj.transform;
        }

        foreach (Transform child in wheelContainer)
        {
            Destroy(child.gameObject);
        }

        float anglePerSection = 360f / sections.Count;

        for (int i = 0; i < sections.Count; i++)
        {
            GameObject section = new GameObject("Section_" + i);
            section.transform.SetParent(wheelContainer, false);
            CreateSectionVisual(section, i, anglePerSection);
            CreateSectionText(section, sections[i].sectionName, i, anglePerSection);
        }

        CreatePointer();
    }

    void CreateSectionVisual(GameObject parent, int index, float anglePerSection)
    {
        GameObject visual = new GameObject("Visual");
        visual.transform.SetParent(parent.transform, false);

        MeshFilter meshFilter = visual.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = visual.AddComponent<MeshRenderer>();

        float startAngle = index * anglePerSection;
        Mesh mesh = new Mesh();

        int segments = 20;
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + i * (anglePerSection / segments);
            angle *= Mathf.Deg2Rad;

            float x = Mathf.Sin(angle) * wheelRadius;
            float y = Mathf.Cos(angle) * wheelRadius;

            vertices[i + 1] = new Vector3(x, y, 0);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = sections[index].color;
        meshRenderer.material = mat;
    }

    void CreateSectionText(GameObject parent, string text, int index, float anglePerSection)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(parent.transform, false);

        float angle = index * anglePerSection + anglePerSection / 2f;
        angle *= Mathf.Deg2Rad;

        float x = Mathf.Sin(angle) * wheelRadius * 0.6f;
        float y = Mathf.Cos(angle) * wheelRadius * 0.6f;

        textObj.transform.localPosition = new Vector3(x, y, -0.1f);
        textObj.transform.localRotation = Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg + 90);

        TextMeshPro textComponent = textObj.AddComponent<TextMeshPro>();
        textComponent.text = text;
        textComponent.fontSize = 5;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.color = Color.white;

        textObj.transform.localScale = Vector3.one * 0.1f;
    }

    void CreatePointer()
    {
        GameObject pointer = new GameObject("Pointer");
        pointer.transform.SetParent(transform, false);

        pointer.transform.localPosition = new Vector3(0, wheelRadius + 0.2f, -0.1f);

        MeshFilter meshFilter = pointer.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = pointer.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[3];
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(-0.1f, 0.2f, 0);
        vertices[2] = new Vector3(0.1f, 0.2f, 0);

        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = Color.magenta;
        meshRenderer.material = mat;
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinTheWheel());
        }
    }

    IEnumerator SpinTheWheel()
    {
        isSpinning = true;

        int result = GetRandomSectionIndex();
        float anglePerSection = 360f / sections.Count;
        float targetSectionAngle = result * anglePerSection + anglePerSection / 2f;
        int extraSpins = Random.Range(2, 4);

        targetAngle = 360f * extraSpins + targetSectionAngle;
        float startAngle = currentAngle % 360f;
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / spinDuration;
            float curveValue = spinCurve.Evaluate(t);

            currentAngle = Mathf.Lerp(startAngle, startAngle + targetAngle, curveValue);
            wheelContainer.localRotation = Quaternion.Euler(0, 0, -currentAngle);

            yield return null;
        }

        isSpinning = false;

        if (resultText != null)
        {
            resultText.text = "Kết quả: " + sections[result].sectionName + "\n" + sections[result].reward;
        }
    }

    private int GetRandomSectionIndex()
    {
        float totalProbability = 0f;
        foreach (var section in sections)
        {
            totalProbability += section.probability;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float accumulatedProbability = 0f;
        for (int i = 0; i < sections.Count; i++)
        {
            accumulatedProbability += sections[i].probability;
            if (randomValue <= accumulatedProbability)
            {
                return i;
            }
        }

        return sections.Count - 1;
    }
}
