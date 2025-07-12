

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    [RequireComponent(typeof(PhotonView))]
    public class MoveByKeys : Photon.Pun.MonoBehaviourPun
    {
        public float Speed = 10f;
        public float JumpForce = 200f;
        public float JumpTimeout = 0.5f;

        private bool isSprite;
        private float jumpingTime;
        private Rigidbody body;
        private Rigidbody2D body2d;

        public void Start()
        {
            this.isSprite = (GetComponent<SpriteRenderer>() != null);

            this.body2d = GetComponent<Rigidbody2D>();
            this.body = GetComponent<Rigidbody>();
        }
        public void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if ((Input.GetAxisRaw("Horizontal") < -0.1f) || (Input.GetAxisRaw("Horizontal") > 0.1f))
            {
                transform.position += Vector3.right * (Speed * Time.deltaTime) * Input.GetAxisRaw("Horizontal");
            }
            if (this.jumpingTime <= 0.0f)
            {
                if (this.body != null || this.body2d != null)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        this.jumpingTime = this.JumpTimeout;

                        Vector2 jump = Vector2.up * this.JumpForce;
                        if (this.body2d != null)
                        {
                            this.body2d.AddForce(jump);
                        }
                        else if (this.body != null)
                        {
                            this.body.AddForce(jump);
                        }
                    }
                }
            }
            else
            {
                this.jumpingTime -= Time.deltaTime;
            }
            if (!this.isSprite)
            {
                if ((Input.GetAxisRaw("Vertical") < -0.1f) || (Input.GetAxisRaw("Vertical") > 0.1f))
                {
                    transform.position += Vector3.forward * (Speed * Time.deltaTime) * Input.GetAxisRaw("Vertical");
                }
            }
        }
    }
}