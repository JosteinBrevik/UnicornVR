using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Com.DefaultCompany.UnicornVR;

public class CharacterRotateMovement : Photon.PunBehaviour
{
    //character model found in https://www.assetstore.unity3d.com/en/#!/content/3012

    private Transform VRCamera;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 rotateDirection = Vector3.zero;
    private CharacterController controller;
    private Animator anim;
    public float gravity = 40f;
    public PlayerManager playerManager;
    public float JumpSpeed = 8.0f;
    public float SpeedDecrease = 0.5f;
    public float SpeedIncrease = 1.5f;
    public float Speed = 6.0f;
    public Transform CharacterGO;

    public GameState GameState;// = GameState.Start;
    public PlayerManager MyPlayerManager;
    public SpeedController MySpeedController;

    // Use this for initialization
    void Start()
    {
        if (photonView.isMine)
        {
            var newCamera = playerManager.gameObject.GetComponent<CameraWork>().GetTransform();
            if (newCamera == null) Debug.Log("VRCAMERA = NULL :( ");
            VRCamera = newCamera;//Camera.main.transform;
            moveDirection = transform.forward;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= Speed;

            UIManager.Instance.ResetScore();
            UIManager.Instance.SetStatus(Constants.StatusTapToStart);

            GameState = GameState.Playing;

            anim = CharacterGO.GetComponent<Animator>();
            controller = GetComponent<CharacterController>();

            anim.SetBool(Constants.AnimationStarted, true);
            GameState = GameState.Playing;

            UIManager.Instance.SetStatus(string.Empty);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            switch (GameState)
            {
                case GameState.Start:
                    if (true)//Input.GetMouseButtonUp(0))
                    {
                        anim.SetBool(Constants.AnimationStarted, true);
                        GameState = GameState.Playing;

                        UIManager.Instance.SetStatus(string.Empty);
                    }
                    break;
                case GameState.Playing:
                    VRCamera = playerManager.gameObject.GetComponent<CameraWork>().GetTransform();
                    UIManager.Instance.IncreaseScore(0.01f);
                    CheckHeight();

                    DetectJumpOrSwipeLeftRight();

                    // Get HMD directions
                    var currYSpeed = moveDirection.y; //dont add moment to jump
                    moveDirection = VRCamera.TransformDirection(Vector3.forward) * Speed;
                    moveDirection.y = currYSpeed;

                    //apply gravity
                    moveDirection.y -= gravity * Time.deltaTime;

                    //move the player
                    controller.Move(moveDirection * Time.deltaTime);

                    Debug.Log(Speed);

                    //rotate the player?                    
                    //Vector3 movement = new Vector3(moveDirection.x, 0.0f, moveDirection.z) * Time.deltaTime;
                    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.05f);
                    //transform.Rotate(rotateDirection, Time.deltaTime * 1f);


                    break;
                case GameState.Slow:
                    MySpeedController.SpeedAlter(SpeedDecrease, 3.0f);
                    GameState = GameState.Playing;
                    break;

                case GameState.Dead:
                    anim.SetBool(Constants.AnimationStarted, false);
                    if (true)//(Input.GetMouseButtonUp(0))
                    {
                        //restart
                        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        MySpeedController.SpeedAlter(0.1f, 10f);
                        MyGameManager.Instance.LeaveRoom();
                    }
                    break;
                default:
                    break;
            }
        }

    }

    private void CheckHeight()
    {
        if (transform.position.y < -10 || transform.position.y > 10)
        {
            //GameManager.Instance.Die();
            UIManager.Instance.SetStatus(Constants.StatusDeadTapToStart);
            GameState = GameState.Dead;
        }
    }

    private void DetectJumpOrSwipeLeftRight()
    {

        var vrRot = VRCamera.rotation.eulerAngles;  //charachter 
        var myRot = transform.rotation.eulerAngles; //hmd
        var angleVert = Mathf.DeltaAngle(vrRot.x, myRot.x);
        var angleHori = Mathf.DeltaAngle(vrRot.y, myRot.y);
        rotateDirection.y = angleHori / 180f;
        transform.Rotate(rotateDirection, Time.deltaTime * 2);
        //Debug.Log(angleHori);

        if (angleVert > 20 && controller.isGrounded)
        {
            moveDirection.y = JumpSpeed;
            anim.SetBool(Constants.AnimationJump, true);
            MySpeedController.SpeedAlter(SpeedIncrease, 2f);

        }
        else
        {
            anim.SetBool(Constants.AnimationJump, false);
        }

    }

    public void Die()
    {
        UIManager.Instance.SetStatus(Constants.StatusDeadTapToStart);
        this.GameState = GameState.Dead;
    }


    void OnTriggerEnter(Collider other)
    {


        if (!photonView.isMine)
        {
            return;
        }


        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (other.name.Contains("modelBox"))
        {
            MySpeedController.SpeedAlter(SpeedDecrease, 2f);
            Destroy(other.gameObject);
        }
        else if ((other.name.Contains("candy")))
        {
            MySpeedController.SpeedAlter(SpeedIncrease, 1f);
            Destroy(other.gameObject);
        }
    }

    public GameState GetGameState()
    {
        return this.GameState;
    }
}
