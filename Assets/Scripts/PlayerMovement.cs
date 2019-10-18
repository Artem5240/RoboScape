using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public PowerUpController PUController;
    public Animator SkinAnimator;
    public GameManager GM;
    CapsuleCollider selfCollider;
    Rigidbody rb;

    public delegate void OnPowerupUse(PowerUpController.PowerUp.Type type);
    public static event OnPowerupUse PowerUpUseEvent;

    public float JumpSpeed = 12;

    int laneNumber = 1,             // Начальная дорожка
        lanesCount = 2;             // всего дорожек

    public float FirstLanePos,          // Позиция нулевой линии
                 LaneDistance,          // Расстояние между линиями
                 SideSpeed;             // Скорость перемещения с одной линии на другую 

    bool isRolling = false;
    bool isImmortal = false;

    Vector3 ccCenterNorm = new Vector3(0, .96f, 0),
            ccCenterRoll = new Vector3(0, .4f, .85f);

    float ccHeightNorm = 2,
          ccHeightRoll = .4f;

    bool wannaJump = false;

    Vector3 startPosition;
    Vector3 rbVelocity;

	void Start ()
    {
        selfCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        SwipeController.SwipeEvent += CheckInput;
	}

    public void Respawn()
    {
        StopAllCoroutines();
        isImmortal = false;
        isRolling = false;
        wannaJump = false;
        StopRolling();
    }

    public void Pause()
    {
        rbVelocity = rb.velocity;
        rb.isKinematic = true;
        SkinAnimator.speed = 0;
    }

    public void UnPause()
    {
        rb.isKinematic = false;
        rb.velocity = rbVelocity;
        SkinAnimator.speed = 1;
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, Physics.gravity.y * 4, 0), ForceMode.Acceleration);

        if (wannaJump && isGrounded())                // Условие прыжка
        {
            SkinAnimator.SetTrigger("jumping");
            rb.AddForce(new Vector3(0, JumpSpeed, 0), ForceMode.Impulse);
            wannaJump = false;
        }
    }

    void Update ()
    {
        if (isGrounded())
            SkinAnimator.ResetTrigger("falling");
        else if (rb.velocity.y < -8)
            SkinAnimator.SetTrigger("falling");

        Vector3 newPos = transform.position;        // Новый вектор на основе текущей позиции
        // Движение в стороны
        newPos.z = Mathf.Lerp(newPos.z, FirstLanePos + (laneNumber * LaneDistance), Time.deltaTime * SideSpeed);
        transform.position = newPos;
	}

    void CheckInput(SwipeController.SwipeType type)
    {
        if (isGrounded() && GM.CanPlay && !isRolling)
        {
            if (type == SwipeController.SwipeType.UP)
                wannaJump = true;
            else if (type == SwipeController.SwipeType.DOWN)
                StartCoroutine(DoRoll());
        }

        int sign = 0;

        if (!GM.CanPlay || isRolling)
            return;

        if (type == SwipeController.SwipeType.LEFT)
            sign = -1;
        else if (type == SwipeController.SwipeType.RIGHT)
            sign = 1;
        else
            return;

        laneNumber += sign;
        laneNumber = Mathf.Clamp(laneNumber, 0, lanesCount);
    }
    // Находимся ли на поверхности
    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.05f);
    }

    IEnumerator DoRoll()
    {
        float rollDuration = 1.5f;
        float cdDuration = .3f;

        isRolling = true;
        SkinAnimator.SetBool("rolling", true);

        selfCollider.center = ccCenterRoll;
        selfCollider.height = ccHeightRoll;

        while (rollDuration > 0)
        {
            if (GM.CanPlay)
                rollDuration -= Time.deltaTime;
            yield return null;
        }

        StopRolling();

        while (cdDuration > 0)
        {
            if (GM.CanPlay)
                cdDuration -= Time.deltaTime;
            yield return null;
        }

        isRolling = false;
    }

    void StopRolling()
    {
        SkinAnimator.SetBool("rolling", false);

        selfCollider.center = ccCenterNorm;
        selfCollider.height = ccHeightNorm;
    }

    private void OnCollisionEnter(Collision collision)        // Смерть при столкновении
    { //Если врезался или упал - смерть
        if ((!collision.gameObject.CompareTag("Trap") &&
             !collision.gameObject.CompareTag("DeathPlane")) || 
             !GM.CanPlay)
            return;

        if (isImmortal && !collision.gameObject.CompareTag("DeathPlane"))
        {
            collision.collider.isTrigger = true;
            return;
        }

        StartCoroutine(Death());
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Coin":
                GM.AddCoins(1);
                AudioManager.Instance.PlayCoinEffect();
                break;

            case "MultiPU":
                PowerUpUseEvent(PowerUpController.PowerUp.Type.MUILTIPLIER);
                break;

            case "ImmortalPU":
                PowerUpUseEvent(PowerUpController.PowerUp.Type.IMMORTALITY);
                break;

            case "CoinsSpawnPU":
                PowerUpUseEvent(PowerUpController.PowerUp.Type.COINS_SPAWN);
                break;

            default: return;
        }

        Destroy(other.gameObject);
    }

    IEnumerator Death()
    {
        GM.CanPlay = false;
        PUController.ResetAllPowerUps();

        SkinAnimator.SetTrigger("death");

        yield return new WaitForSeconds(2);

        SkinAnimator.ResetTrigger("death");
        GM.ShowResult();
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        laneNumber = 1;
    }

    public void ImmortalityOn()
    {
        isImmortal = true;
    }

    public void ImmortalityOff()
    {
        isImmortal = false;
    }

}
