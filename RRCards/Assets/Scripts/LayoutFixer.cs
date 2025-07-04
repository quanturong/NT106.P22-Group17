using UnityEngine;
using UnityEngine.UI;

public class LayoutFixer : MonoBehaviour
{
    public void ResetLayout()
    {
        var layoutGroup = GetComponent<VerticalLayoutGroup>();
        var sizeFitter = GetComponent<ContentSizeFitter>();

        if (layoutGroup != null)
        {
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;
        }
        if (sizeFitter != null)
        {
            sizeFitter.enabled = false;
            sizeFitter.enabled = true;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
