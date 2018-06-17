using UnityEngine;

public enum SpeedState
{
    SpeedUnaltered,
    SpeedAltered
}

public class SpeedController : MonoBehaviour
{
    private SpeedState state = SpeedState.SpeedUnaltered;
    public CharacterRotateMovement CharacterRotateMovement;
    private float prevSpeed;

    public void SpeedAlter(float newSpeedMult, float duartion)
    {
        if (state == SpeedState.SpeedUnaltered)
        {
            {
                state = SpeedState.SpeedAltered;
                prevSpeed = CharacterRotateMovement.Speed;
                CharacterRotateMovement.Speed *= newSpeedMult;
                UIManager.Instance.SetSpeed(prevSpeed);
                Invoke("ResetSpeed", duartion);
            }
        }
    }

    private void ResetSpeed()
    {
        CharacterRotateMovement.Speed = prevSpeed;
        state = SpeedState.SpeedUnaltered;
        UIManager.Instance.SetSpeed(prevSpeed);
    }

}