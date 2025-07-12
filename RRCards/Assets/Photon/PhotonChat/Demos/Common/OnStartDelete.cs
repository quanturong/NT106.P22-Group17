
using UnityEngine;

namespace Photon.Chat.UtilityScripts
{
    public class OnStartDelete : MonoBehaviour
    {
        private void Start()
        {
            Destroy(this.gameObject);
        }
    }
}