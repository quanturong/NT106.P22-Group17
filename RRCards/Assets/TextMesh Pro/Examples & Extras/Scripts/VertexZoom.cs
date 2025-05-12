using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace TMPro.Examples
{
    public class VertexZoom : MonoBehaviour
    {
        public float AngleMultiplier = 1.0f;
        public float SpeedMultiplier = 1.0f;
        public float CurveScale = 1.0f;

        private TMP_Text m_TextComponent;
        private bool hasTextChanged;

        void Awake()
        {
            m_TextComponent = GetComponent<TMP_Text>();
        }

        void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        }

        void Start()
        {
            StartCoroutine(AnimateVertexColors());
        }

        void ON_TEXT_CHANGED(Object obj)
        {
            if (obj == m_TextComponent)
                hasTextChanged = true;
        }

        IEnumerator AnimateVertexColors()
        {
            m_TextComponent.ForceMeshUpdate();
            TMP_TextInfo textInfo = m_TextComponent.textInfo;

            Matrix4x4 matrix;
            TMP_MeshInfo[] cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();

            List<float> modifiedCharScale = new List<float>();
            List<int> scaleSortingOrder = new List<int>();

            hasTextChanged = true;

            while (true)
            {
                if (hasTextChanged)
                {
                    cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
                    hasTextChanged = false;
                }

                int characterCount = textInfo.characterCount;
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                modifiedCharScale.Clear();
                scaleSortingOrder.Clear();

                for (int i = 0; i < characterCount; i++)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible)
                        continue;

                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;

                    Vector3[] sourceVertices = cachedMeshInfoVertexData[materialIndex].vertices;
                    Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;
                    Vector3 offset = charMidBasline;

                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    for (int j = 0; j < 4; j++)
                        destinationVertices[vertexIndex + j] = sourceVertices[vertexIndex + j] - offset;

                    float randomScale = Random.Range(1f, 1.5f);
                    modifiedCharScale.Add(randomScale);
                    scaleSortingOrder.Add(modifiedCharScale.Count - 1);

                    matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * randomScale);

                    for (int j = 0; j < 4; j++)
                        destinationVertices[vertexIndex + j] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + j]);

                    for (int j = 0; j < 4; j++)
                        destinationVertices[vertexIndex + j] += offset;

                    // ✅ FIX DÒNG 112: Vector4[] to Vector2[]
                    Vector4[] sourceUVs0_4D = cachedMeshInfoVertexData[materialIndex].uvs0;
                    Vector2[] sourceUVs0 = new Vector2[sourceUVs0_4D.Length];
                    for (int j = 0; j < sourceUVs0_4D.Length; j++)
                        sourceUVs0[j] = new Vector2(sourceUVs0_4D[j].x, sourceUVs0_4D[j].y);

                    Vector4[] uvs0_4D = textInfo.meshInfo[materialIndex].uvs0;
                    Vector2[] destinationUVs0 = new Vector2[uvs0_4D.Length];
                    for (int j = 0; j < uvs0_4D.Length; j++)
                    {
                        destinationUVs0[j] = new Vector2(uvs0_4D[j].x, uvs0_4D[j].y);
                    }


                    for (int j = 0; j < 4; j++)
                        destinationUVs0[vertexIndex + j] = sourceUVs0[vertexIndex + j];

                    Color32[] sourceColors32 = cachedMeshInfoVertexData[materialIndex].colors32;
                    Color32[] destinationColors32 = textInfo.meshInfo[materialIndex].colors32;

                    for (int j = 0; j < 4; j++)
                        destinationColors32[vertexIndex + j] = sourceColors32[vertexIndex + j];
                }

                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    scaleSortingOrder.Sort((a, b) => modifiedCharScale[a].CompareTo(modifiedCharScale[b]));
                    textInfo.meshInfo[i].SortGeometry(scaleSortingOrder);

                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;

                    // ✅ FIX FINAL UVS CONVERSION
                    Vector4[] uvs4D = textInfo.meshInfo[i].uvs0;
                    Vector2[] uvs2D = new Vector2[uvs4D.Length];
                    for (int j = 0; j < uvs4D.Length; j++)
                        uvs2D[j] = new Vector2(uvs4D[j].x, uvs4D[j].y);

                    textInfo.meshInfo[i].mesh.uv = uvs2D;
                    textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;

                    m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
