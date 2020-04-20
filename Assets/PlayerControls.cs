using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float MaxMoveSpeed = 8;

    public AudioSource DashSound;
    public AudioSource StepSound;

    private CharacterController controllerComponent;
    private Animator animatorComponent;
    private PlayerGrabTrigger grabTrigger;

    private Vector3 moveSpeed;
    private float grabCooldown;
    private float dashingTimeLeft;

    private static readonly int GrabParam = Animator.StringToHash("grab");
    private static readonly int WalkSpeedParam = Animator.StringToHash("walk speed");

    private void Start()
    {
        animatorComponent = GetComponent<Animator>();
        controllerComponent = GetComponent<CharacterController>();

        grabTrigger = GetComponentInChildren<PlayerGrabTrigger>();
    }

    private void Update()
    {
        if (MainMenu.IsGameStarted)
        {
            UpdateWalk();

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.Z)) Grab();
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.X)) Dash(false);
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.X)) Dash(true);
            grabCooldown -= Time.deltaTime;
        }

        animatorComponent.SetFloat(WalkSpeedParam, new Vector3(moveSpeed.x,0,moveSpeed.z).magnitude);
    }

    private void Dash(bool holding)
    {
        if (dashingTimeLeft < (holding ? -.4f : -.2f))
        {
            dashingTimeLeft = .3f;
            DashSound.Play();
        }
    }

    public void StepAnimationCallback()
    {
        if (dashingTimeLeft > 0) return;

        if (StepSound.pitch < 1) StepSound.pitch = Random.Range(1.05f, 1.15f);
        else StepSound.pitch = Random.Range(0.9f, 0.95f);
        StepSound.Play();
    }

    private void UpdateWalk()
    {
        float ySpeed = moveSpeed.y;
        moveSpeed.y = 0;
        if (dashingTimeLeft <= 0)
        {
            Vector3 target = MaxMoveSpeed *
                             new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 300);

            if (moveSpeed.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveSpeed),
                    Time.deltaTime * 720);
            }
        }
        else
        {
            moveSpeed = MaxMoveSpeed * 5 * moveSpeed.normalized;
        }

        dashingTimeLeft -= Time.deltaTime;

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime;
        controllerComponent.Move(moveSpeed * Time.deltaTime);
    }

    private void Grab()
    {
        if (grabCooldown > 0) return;

        if (grabTrigger.GrabbedObject != null)
        {
            grabTrigger.Release();
            return;
        }

        animatorComponent.SetTrigger(GrabParam);

        grabCooldown = .5f;
    }

    public void GrabAnimationCallback()
    {
        grabTrigger.Grab();
    }
}