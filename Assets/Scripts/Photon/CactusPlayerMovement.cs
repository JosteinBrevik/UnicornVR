using UnityEngine;

public class CactusPlayerMovement : Photon.PunBehaviour
{
    void Update()
    {
        if (photonView.isMine)
        {
            var x = Input.GetAxis("HorizontalUI") * Time.deltaTime * 12.0f;
            var z = GvrViewer.Instance.HeadPose.Orientation * Vector3.up;

            //transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z[2]);
        }



    }
}