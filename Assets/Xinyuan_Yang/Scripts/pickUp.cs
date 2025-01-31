/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pickUp : MonoBehaviour
{

    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count= 0;
        setCountText();
        winTextObject.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 10;
            setCountText();
        }
        if (other.gameObject.CompareTag("PickDown"))
        {
            other.gameObject.SetActive(false);
            count -= 10;
            setCountText();
        }

    }
   
    void setCountText()
    {
        
        countText.text = "Score: " + count.ToString();
        if (count >= 100)
        {
            winTextObject.SetActive(true);
        }
    }
}
*/
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class pl : GenericBehaviour
{
	//public static PlayerMoveCode Instance;

	public float walkSpeed = 0.15f;
	public float runSpeed = 1.0f;
	public float sprintSpeed = 2.0f;
	public float speedDampTime = 0.1f;
	public float jumpHeight = 1.5f;
	public float jumpIntertialForce = 10f;
	private float speed, speedSeeker;
	private int jumpBool;
	private int groundedBool;
	private bool jump;
	private bool isColliding;
	public bool isLock;

	public Text text;
	public Image Victory;
	public Image Defeat;
	public int a = 0;

	//  private void Awake()
	//  {
	//Instance = this;
	//  }
	void Start()
	{
		jumpBool = Animator.StringToHash("Jump");
		groundedBool = Animator.StringToHash("Grounded");
		behaviourManager.GetAnim.SetBool(groundedBool, true);

		behaviourManager.SubscribeBehaviour(this);
		behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);
		speedSeeker = runSpeed;

		//Hide mouse
		Time.timeScale = 1;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Donuts"))
		{
			other.GetComponent<AudioSource>().enabled = true;
			a = a + 10;
			text.text = "Score��" + a.ToString();
			StartCoroutine("Wait", other);
			//other.gameObject.SetActive(false);

		}
		if (other.gameObject.tag.Equals("RiceBalls"))
		{
			other.GetComponent<AudioSource>().enabled = true;
			StartCoroutine("Wait", other);
			Defeat.gameObject.SetActive(true);
		}

	}
	IEnumerator Wait(Collider other)
	{
		yield return new WaitForSeconds(1f);
		other.gameObject.SetActive(false);
	}
	void Update()
	{
		if (a == 100)
		{
			Victory.gameObject.SetActive(true);

		}


		if (!jump && Input.GetKeyDown(KeyCode.Space) && behaviourManager.IsCurrentBehaviour(this.behaviourCode) && !behaviourManager.IsOverriding())
		{
			jump = true;
		}


		//Press ESC pause
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isLock = !isLock;
			if (isLock)
			{
				Time.timeScale = 0;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}


	public override void LocalFixedUpdate()
	{
		MovementManagement(behaviourManager.GetH, behaviourManager.GetV);


		JumpManagement();
	}


	void JumpManagement()
	{
		if (jump && !behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.IsGrounded())
		{
			behaviourManager.LockTempBehaviour(this.behaviourCode);
			behaviourManager.GetAnim.SetBool(jumpBool, true);

			if (behaviourManager.GetAnim.GetFloat(speedFloat) > 0.1)
			{

				GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0f;

				RemoveVerticalVelocity();

				float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
				velocity = Mathf.Sqrt(velocity);
				behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
			}
		}

		else if (behaviourManager.GetAnim.GetBool(jumpBool))
		{
			if (!behaviourManager.IsGrounded() && !isColliding && behaviourManager.GetTempLockStatus())
			{
				behaviourManager.GetRigidBody.AddForce(transform.forward * jumpIntertialForce * Physics.gravity.magnitude * sprintSpeed, ForceMode.Acceleration);
			}

			if ((behaviourManager.GetRigidBody.velocity.y < 0) && behaviourManager.IsGrounded())
			{
				behaviourManager.GetAnim.SetBool(groundedBool, true);

				GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;

				jump = false;
				behaviourManager.GetAnim.SetBool(jumpBool, false);
				behaviourManager.UnlockTempBehaviour(this.behaviourCode);
			}
		}
	}


	void MovementManagement(float horizontal, float vertical)
	{
		if (behaviourManager.IsGrounded())
			behaviourManager.GetRigidBody.useGravity = true;


		else if (!behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.GetRigidBody.velocity.y > 0)
		{
			RemoveVerticalVelocity();
		}

		Rotating(horizontal, vertical);


		Vector2 dir = new Vector2(horizontal, vertical);
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude;

		speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
		speed *= speedSeeker;
		if (behaviourManager.IsSprinting())
		{
			speed = sprintSpeed;
		}

		behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
	}


	private void RemoveVerticalVelocity()
	{
		Vector3 horizontalVelocity = behaviourManager.GetRigidBody.velocity;
		horizontalVelocity.y = 0;
		behaviourManager.GetRigidBody.velocity = horizontalVelocity;
	}

	Vector3 Rotating(float horizontal, float vertical)
	{
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

		forward.y = 0.0f;
		forward = forward.normalized;

		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection;
		targetDirection = forward * vertical + right * horizontal;

		if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
			behaviourManager.GetRigidBody.MoveRotation(newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}
		if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}


	private void OnCollisionStay(Collision collision)
	{
		isColliding = true;
		if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
		{
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		isColliding = false;
		GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
		GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
	}
}
