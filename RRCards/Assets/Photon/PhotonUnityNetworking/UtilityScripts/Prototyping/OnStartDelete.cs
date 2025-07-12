
using UnityEngine;

namespace Photon.Pun.UtilityScripts
{
    public class OnStartDelete : MonoBehaviour
    {
        private void Start()
        {
            Destroy(this.gameObject);
        }
    }
}