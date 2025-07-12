using UnityEngine;

namespace Photon.Pun.Demo.Procedural
{
    public class ProceduralController : MonoBehaviour
    {
        private Camera cam;

        #region UNITY

        public void Awake()
        {
            cam = Camera.main;
        }
        public void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            if (h >= 0.1f)
            {
                cam.transform.position += Vector3.right * 10.0f * Time.deltaTime;
            }
            else if (h <= -0.1f)
            {
                cam.transform.position += Vector3.left * 10.0f * Time.deltaTime;
            }

            if (v >= 0.1f)
            {
                cam.transform.position += Vector3.forward * 10.0f * Time.deltaTime;
            }
            else if (v <= -0.1f)
            {
                cam.transform.position += Vector3.back * 10.0f * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                cam.transform.position += Vector3.up * 10.0f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                cam.transform.position += Vector3.down * 10.0f * Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Block b = hit.transform.GetComponent<Block>();

                    if (b != null)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            WorldGenerator.Instance.DecreaseBlockHeight(b.ClusterId, b.BlockId);
                        }
                        else if (Input.GetMouseButtonDown(1))
                        {
                            WorldGenerator.Instance.IncreaseBlockHeight(b.ClusterId, b.BlockId);
                        }
                    }
                }
            }
        }

        #endregion
    }
}