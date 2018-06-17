using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterRotateMovement : Photon.PunBehaviour
{
    //character model found in https://www.assetstore.unity3d.com/en/#!/content/3012

    private Transform VRCamera;

    private Vector3 moveDirection   = Vector3.zero;
    private Vector3 rotateDirection = Vector3.zero;
    private CharacterController controller;
    private Animator anim;
    public float gravity = 40f;

    public float JumpSpeed = 8.0f;
    public float SpeedDecrease = 0.5f;
    public float SpeedIncrease = 1.5f;
    public float Speed = 6.0f;
    public Transform CharacterGO;

    bool isInSwipeArea;

    private GameState GameState;// = GameState.Start;
    public SpeedController MySpeedController;

    // Use this for initialization
    void Start()
    {
        if (photonView.isMine)
        {
            VRCamera = Camera.main.transform;
            moveDirection = transform.forward;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= Speed;

            UIManager.Instance.ResetScore();
            UIManager.Instance.SetStatus(Constants.StatusTapToStart);

            GameState = GameState.Playing;

            anim = CharacterGO.GetComponent<Animator>();
            controller = GetComponent<CharacterController>();

            anim.SetBool(Constants.AnimationStarted, true);
            var instance = GameManager.Instance;
            instance.GameState = GameState.Playing;

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
                        var instance = GameManager.Instance;
                        instance.GameState = GameState.Playing;

                        UIManager.Instance.SetStatus(string.Empty);
                    }
                    break;
                case GameState.Playing:
                    VRCamera = Camera.main.transform; //Should we update? == main.camera atm
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
                    GameManager.Instance.GameState = GameState.Playing;
                    break;

                case GameState.SpeedUp:
                    MySpeedController.SpeedAlter(SpeedIncrease, 2f);
                    GameManager.Instance.GameState = GameState.Playing;
                    break;

                case GameState.Dead:
                    anim.SetBool(Constants.AnimationStarted, false);
                    if (Input.GetMouseButtonUp(0))
                    {
                        //restart
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            GameManager.Instance.Die();
        }
    }

    private void DetectJumpOrSwipeLeftRight()
    {
        
        var vrRot = VRCamera.rotation.eulerAngles;  //charachter 
        var myRot = transform.rotation.eulerAngles; //hmd
        var angleVert = Mathf.DeltaAngle(vrRot.x, myRot.x);
        var angleHori= Mathf.DeltaAngle(vrRot.y, myRot.y);
        rotateDirection.y = angleHori/180f;
        transform.Rotate(rotateDirection, Time.deltaTime*2);
        //Debug.Log(angleHori);

        if (angleVert > 20 && controller.isGrounded)
        {
            moveDirection.y = JumpSpeed;
            anim.SetBool(Constants.AnimationJump, true);
            GameManager.Instance.GameState = GameState.SpeedUp;

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
    public void Slow()
    {
        this.GameState = GameState.Slow;
    }

    public void SpeedUp()
    {
        this.GameState = GameState.SpeedUp;
    }
}
