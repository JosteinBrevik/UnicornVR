using UnityEngine;

public class CactusPlayerMovement : Photon.PunBehaviour
{
    void Update()
    {
        if (photonView.isMine)
        {
            var x = Input.GetAxis("HorizontalUI") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("VerticalUI") * Time.deltaTime * 3.0f;
            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);
        }



    }
}