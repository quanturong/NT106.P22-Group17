using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpinWheel))]
public class SpinWheelEditor : Editor
{
    private SerializedProperty sectionsCountProp;
    private SerializedProperty sectionsProp;
    private SerializedProperty wheelRadiusProp;
    private SerializedProperty spinDurationProp;
    private SerializedProperty maxSpinSpeedProp;
    private SerializedProperty spinCurveProp;
    private SerializedProperty wheelContainerProp;

    private void OnEnable()
    {
        sectionsCountProp = serializedObject.FindProperty("sectionsCount");
        sectionsProp = serializedObject.FindProperty("sections");
        wheelRadiusProp = serializedObject.FindProperty("wheelRadius");
        spinDurationProp = serializedObject.FindProperty("spinDuration");
        maxSpinSpeedProp = serializedObject.FindProperty("maxSpinSpeed");
        spinCurveProp = serializedObject.FindProperty("spinCurve");
        wheelContainerProp = serializedObject.FindProperty("wheelContainer");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(wheelRadiusProp);

        // Xử lý số lượng ô
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sectionsCountProp);
        if (EditorGUI.EndChangeCheck())
        {
            // Đảm bảo số lượng ô tối thiểu là 2
            if (sectionsCountProp.intValue < 2)
                sectionsCountProp.intValue = 2;

            // Điều chỉnh danh sách sections theo số lượng mới
            AdjustSectionsList(sectionsCountProp.intValue);
        }

        // Hiển thị danh sách sections
        EditorGUILayout.PropertyField(sectionsProp, true);

        // Hiển thị các thành phần UI
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spinButton"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("resultText"));

        // Phần cài đặt spinning
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Spin Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(spinDurationProp);
        EditorGUILayout.PropertyField(maxSpinSpeedProp);
        EditorGUILayout.PropertyField(spinCurveProp);

        serializedObject.ApplyModifiedProperties();

        // Nút tái tạo wheel
        if (GUILayout.Button("Rebuild Wheel"))
        {
            SpinWheel spinWheel = (SpinWheel)target;
            if (spinWheel.wheelContainer != null)
            {
                DestroyImmediate(spinWheel.wheelContainer.gameObject);
                spinWheel.wheelContainer = null;
            }

            // Gọi phương thức RebuildWheel() thay vì Awake()
            spinWheel.RebuildWheel();
        }
    }

    private void AdjustSectionsList(int newCount)
    {
        int currentCount = sectionsProp.arraySize;

        if (newCount > currentCount)
        {
            // Tạo thêm sections mới
            for (int i = currentCount; i < newCount; i++)
            {
                sectionsProp.InsertArrayElementAtIndex(sectionsProp.arraySize);

                // Thiết lập giá trị mặc định cho section mới
                SerializedProperty newSection = sectionsProp.GetArrayElementAtIndex(i);
                newSection.FindPropertyRelative("sectionName").stringValue = "Ô " + (i + 1);

                // Chọn màu ngẫu nhiên cho section mới
                Color randomColor = new Color(
                    Random.Range(0.2f, 1f),
                    Random.Range(0.2f, 1f),
                    Random.Range(0.2f, 1f)
                );
                newSection.FindPropertyRelative("color").colorValue = randomColor;
                newSection.FindPropertyRelative("probability").floatValue = 1f;
                newSection.FindPropertyRelative("reward").stringValue = "Phần thưởng " + (i + 1);
            }
        }
        else if (newCount < currentCount)
        {
            // Xóa bớt sections
            for (int i = currentCount - 1; i >= newCount; i--)
            {
                sectionsProp.DeleteArrayElementAtIndex(i);
            }
        }
    }
}