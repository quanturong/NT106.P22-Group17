using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

#pragma warning disable 0618

namespace TMPro.Examples
{
    public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler
    {
        public RectTransform TextPopup_Prefab_01;

        private RectTransform m_TextPopup_RectTransform;
        private TextMeshProUGUI m_TextPopup_TMPComponent;

        private TextMeshProUGUI m_TextMeshPro;
        private Canvas m_Canvas;
        private Camera m_Camera;

        private bool isHoveringObject;
        private int m_selectedWord = -1;
        private int m_selectedLink = -1;
        private int m_lastIndex = -1;

        private Matrix4x4 m_matrix;

        private TMP_MeshInfo[] m_cachedMeshInfoVertexData;

        void Awake()
        {
            m_TextMeshPro = GetComponent<TextMeshProUGUI>();
            m_Canvas = GetComponentInParent<Canvas>();
            m_Camera = m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_Canvas.worldCamera;

            m_TextPopup_RectTransform = Instantiate(TextPopup_Prefab_01);
            m_TextPopup_RectTransform.SetParent(m_Canvas.transform, false);
            m_TextPopup_TMPComponent = m_TextPopup_RectTransform.GetComponentInChildren<TextMeshProUGUI>();
            m_TextPopup_RectTransform.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        }

        void ON_TEXT_CHANGED(Object obj)
        {
            if (obj == m_TextMeshPro)
            {
                m_cachedMeshInfoVertexData = m_TextMeshPro.textInfo.CopyMeshInfoVertexData();
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => isHoveringObject = true;

        public void OnPointerExit(PointerEventData eventData) => isHoveringObject = false;

        public void OnPointerClick(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData) { }

        void LateUpdate()
        {
            if (!isHoveringObject)
            {
                if (m_lastIndex != -1)
                {
                    RestoreCachedVertexAttributes(m_lastIndex);
                    m_lastIndex = -1;
                }
                return;
            }

            int charIndex = TMP_TextUtilities.FindIntersectingCharacter(m_TextMeshPro, Input.mousePosition, m_Camera, true);

            if (charIndex == -1 || charIndex != m_lastIndex)
            {
                RestoreCachedVertexAttributes(m_lastIndex);
                m_lastIndex = -1;
            }

            if (charIndex != -1 && charIndex != m_lastIndex && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                m_lastIndex = charIndex;
                int materialIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].vertexIndex;

                Vector3[] vertices = m_TextMeshPro.textInfo.meshInfo[materialIndex].vertices;
                Vector2 charMidBaseline = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2;
                Vector3 offset = charMidBaseline;

                for (int i = 0; i < 4; i++)
                    vertices[vertexIndex + i] -= offset;

                m_matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 1.5f);

                for (int i = 0; i < 4; i++)
                    vertices[vertexIndex + i] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex + i]);

                for (int i = 0; i < 4; i++)
                    vertices[vertexIndex + i] += offset;

                Color32[] vertexColors = m_TextMeshPro.textInfo.meshInfo[materialIndex].colors32;
                Color32 highlightColor = new Color32(255, 255, 192, 255);

                for (int i = 0; i < 4; i++)
                    vertexColors[vertexIndex + i] = highlightColor;

                TMP_MeshInfo meshInfo = m_TextMeshPro.textInfo.meshInfo[materialIndex];
                int lastVertexIndex = vertices.Length - 4;
                meshInfo.SwapVertexData(vertexIndex, lastVertexIndex);

                m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }
        }

        void RestoreCachedVertexAttributes(int index)
        {
            if (index == -1 || index > m_TextMeshPro.textInfo.characterCount - 1) return;

            int materialIndex = m_TextMeshPro.textInfo.characterInfo[index].materialReferenceIndex;
            int vertexIndex = m_TextMeshPro.textInfo.characterInfo[index].vertexIndex;

            Vector3[] srcVertices = m_cachedMeshInfoVertexData[materialIndex].vertices;
            Vector3[] dstVertices = m_TextMeshPro.textInfo.meshInfo[materialIndex].vertices;
            for (int i = 0; i < 4; i++)
                dstVertices[vertexIndex + i] = srcVertices[vertexIndex + i];

            Color32[] srcColors = m_cachedMeshInfoVertexData[materialIndex].colors32;
            Color32[] dstColors = m_TextMeshPro.textInfo.meshInfo[materialIndex].colors32;
            for (int i = 0; i < 4; i++)
                dstColors[vertexIndex + i] = srcColors[vertexIndex + i];

            // ✅ Vector4[] to Vector2[] conversion for UVs0
            Vector4[] srcUVs0_4D = m_cachedMeshInfoVertexData[materialIndex].uvs0;
            Vector2[] dstUVs0 = new Vector2[srcUVs0_4D.Length];
            for (int i = 0; i < srcUVs0_4D.Length; i++)
                dstUVs0[i] = new Vector2(srcUVs0_4D[i].x, srcUVs0_4D[i].y);

            for (int i = 0; i < 4; i++)
                dstUVs0[vertexIndex + i] = new Vector2(srcUVs0_4D[vertexIndex + i].x, srcUVs0_4D[vertexIndex + i].y);

            Vector2[] srcUVs2 = m_cachedMeshInfoVertexData[materialIndex].uvs2;
            Vector2[] dstUVs2 = m_TextMeshPro.textInfo.meshInfo[materialIndex].uvs2;
            for (int i = 0; i < 4; i++)
                dstUVs2[vertexIndex + i] = srcUVs2[vertexIndex + i];

            int lastIndex = (srcVertices.Length / 4 - 1) * 4;
            for (int i = 0; i < 4; i++)
            {
                dstVertices[lastIndex + i] = srcVertices[lastIndex + i];
                dstColors[lastIndex + i] = srcColors[lastIndex + i];
                dstUVs0[lastIndex + i] = new Vector2(srcUVs0_4D[lastIndex + i].x, srcUVs0_4D[lastIndex + i].y);
                dstUVs2[lastIndex + i] = srcUVs2[lastIndex + i];
            }

            m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
}
