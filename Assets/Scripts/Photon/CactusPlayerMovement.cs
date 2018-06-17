using UnityEngine;

public class CactusPlayerMovement : Photon.PunBehaviour
{
    void Update()
    {
        if (photonView.isMine)
        {
            //var x = Input.GetAxis("HorizontalUI") * Time.deltaTime * 12.0f;
            //var z = GvrViewer.Instance.HeadPose.Orientation * Vector3.up;
            //var dir = Camera.main.transform.TransformDirection(Vector3.forward) * 8.0f;

            //transform.rotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles[1], Vector3.up);
            transform.Rotate(Vector3.up, Time.deltaTime * Camera.main.transform.rotation.eulerAngles[1]);
        }



    }
}