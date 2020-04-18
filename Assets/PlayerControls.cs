using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float MaxMoveSpeed = 8; 
    
    private CharacterController controllerComponent;
    private Animator animatorComponent;
    private Vector3 moveSpeed;

    private float grabCooldown;
    
    private static readonly int GrabParam = Animator.StringToHash("grab");
    private static readonly int WalkSpeedParam = Animator.StringToHash("walk speed");

    private void Start()
    {
        animatorComponent = GetComponent<Animator>();
        controllerComponent = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateWalk();

        if (Input.GetKeyDown(KeyCode.Space)) Grab();
        grabCooldown -= Time.deltaTime;
    }

    private void UpdateWalk()
    {
        float ySpeed = moveSpeed.y;
        moveSpeed.y = 0;
        
        Vector3 target = MaxMoveSpeed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        moveSpeed = Vector3.MoveTowards(moveSpeed, target, Time.deltaTime * 30);

        animatorComponent.SetFloat(WalkSpeedParam, moveSpeed.magnitude);

        if (moveSpeed.magnitude > 0.1f) transform.LookAt(transform.position + moveSpeed);

        moveSpeed.y = ySpeed + Physics.gravity.y * Time.deltaTime;
        controllerComponent.Move(moveSpeed * Time.deltaTime);
    }

    private void Grab()
    {
        if (grabCooldown > 0) return;
        
        animatorComponent.SetTrigger(GrabParam);
        Debug.Log("Grab");

        grabCooldown = 5f/6;
    }

    public void GrabAnimationCallback()
    {
        
    }
}
