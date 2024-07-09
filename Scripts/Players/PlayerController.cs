using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public bool FacingLeft { get; private set; }

    public float moveSpeed = 1f;
    public float mobileCursorRange = 1f;
    [SerializeField] private GameObject SelectedShadow;
    [SerializeField] private GameObject mobileCursor;
    [SerializeField] MonoBehaviour currentActiveWeapon;
    [SerializeField] Camera cam;
    [SerializeField] FixedJoystick joystick;
    private PlayerControls playerControls;
    private PlayerHealth playerHealth;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private GettingPushed gettingPushed;
    private MouseFollow mouseFollow;
    private PhotonView view;

    private Vector2 movement;
    private Vector2 cursorMovement;
    private GameObject SelectedPlayer;
    private GameObject selected;
    public float weaponRange;
    public bool mobileAiming = false;
    

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerHealth = GetComponent<PlayerHealth>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        gettingPushed = GetComponent<GettingPushed>();
        mouseFollow = GetComponentInChildren<MouseFollow>();
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        transform.GetChild(0).GetComponent<ActiveWeapon>().enabled = true;
        currentActiveWeapon = GetComponentInChildren<ActiveWeapon>().CurrentActiveWeapon;
        weaponRange = GetComponentInChildren<ActiveWeapon>().weaponRange;
        
        playerControls.Combat.Select.performed += _ => OnClick();
        playerControls.Combat.AimAttack.performed += _ => SpawnMobileCursor();
        playerControls.Combat.AimAttack.canceled += _ => RemoveMobileCursor();

        

        if (view.IsMine)
        {
        }
        if (!view.IsMine)
        {
            Destroy(GetComponent<Rigidbody2D>());
        }
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }


    private void Update()
    {
        if (view.IsMine)
        {
            PlayerInput();
        }

    }

    private void FixedUpdate()
    {
        if (view.IsMine && !playerHealth.IsDead)
        {
            Move();
            if ((currentActiveWeapon as IWeapon).GetPlayingAttackAnim())
            { return; }
            if (selected && SelectedPlayerInRange())
                mouseFollow.FaceEnemy(SelectedPlayer);
            else if (mobileAiming)
                mouseFollow.FaceMobileCursor(mobileCursor);
            else
                mouseFollow.FaceMouse();

            AdjustPlayerFacingDirection();
            
        }
    }

    private void PlayerInput()
    {
        if (view.IsMine)
        {
            cursorMovement = playerControls.Combat.AimAttack.ReadValue<Vector2>();
            movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
            if (playerControls.Combat.Dash.IsPressed() || JoystickDash())
                Dash();
        }
    }

    private void Move()
    {
        mobileCursor.transform.localPosition = cursorMovement * mobileCursorRange;
        if (gettingPushed.beingKnockedBack)
        {
            return;
        }
        transform.position = (Vector2)transform.position + movement * moveSpeed * Time.deltaTime;
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = cam.WorldToScreenPoint(transform.position);
        if (selected && SelectedPlayerInRange())
        {
            if (SelectedPlayer.transform.position.x < playerScreenPoint.x)
            {
                mySpriteRenderer.flipX = true;
                FacingLeft = true;
            }
            else if (SelectedPlayer.transform.position.x > playerScreenPoint.x)
            {
                mySpriteRenderer.flipX = false;
                FacingLeft = false;
            }
            return;
        }

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            FacingLeft = true;
        }
        else if (mousePos.x > playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = false;
            FacingLeft = false;
        }
    }

    public void Dash()
    {
        if (view.IsMine && GameSceneManager.instance.gameStarted)
        {
            view.RPC("Dash", RpcTarget.All);
        }
    }

    public void OnClick()
    {
        var rayHit = Physics2D.GetRayIntersection(cam.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider || SelectedPlayer == rayHit.collider.gameObject ||
            rayHit.collider.gameObject == this.gameObject
            ||!rayHit.collider.GetComponent<PlayerHealth>())
        {
            SelectedPlayer = null;
            Destroy(selected);
            Debug.Log("Null");
        }

        else if (SelectedPlayer == null)
        {
            SelectedPlayer = rayHit.collider.gameObject;
            Destroy(selected);
            selected = Instantiate(SelectedShadow, SelectedPlayer.transform);
            Debug.Log(SelectedPlayer.name);
        }
    }

    public bool SelectedPlayerInRange()
    {
        if (selected)
        {
            if (Vector2.Distance(SelectedPlayer.transform.position, transform.position) < weaponRange + 0.6f)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public void SpawnMobileCursor()
    {
        mobileAiming = true;
        mobileCursor.SetActive(true);
    }

    public void RemoveMobileCursor()
    {
        mobileAiming = false;
        mobileCursor.transform.localPosition = Vector3.zero;
        mobileCursor.SetActive(false);
    }

    public bool JoystickDash()
    {
        if (Vector2.Distance(joystick.Direction,Vector2.zero) >= 1f)
        //if (Vector2.Distance(Gamepad.current.leftStick.ReadValue(), Vector2.zero) >= 1f)
            return true;
        else
            return false;
    }

    public void toggleShowScoreboard()
    {
        Scoreboard.instance.showing = !Scoreboard.instance.showing;
    }
}
