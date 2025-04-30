using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Audio;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count = 0;
    public TextMeshProUGUI countText;
    public GameObject WinText;
    private float movementX;
    private float movementY;
    private bool isGrounded;
    AudioSource audioSource;
    private Vector3 lastCheckpoint;
    private Vector3 offset = new Vector3(0, -0.35f, 0);

    public float speed = 5f;
    public float jumpForce = 10f;
    private float originalSpeed;
    public SkinnedMeshRenderer[] MeshR;

    // Gestion de la caméra
    public Transform cameraTransform;
    public float sensitivity = 2f;
    public float maxYAngle = 80f;
    private Vector2 lookInput;
    private float rotationX = 0f;
    private float rotationY = 0f;
    public GameObject Ghost;
    private Animator ghostAnim;
    private Transform ghostTransform;
    private float mouseY;
    private float mouseX;
    public bool dead;
    private bool respawn = false;

    public List<AudioClip> audioClips; 
    public Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

    private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");
    private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
    private static readonly int SurprisedState = Animator.StringToHash("Base Layer.surprised");
    private static readonly int AttackState = Animator.StringToHash("Base Layer.attack_shift");
    private static readonly int DissolveState = Animator.StringToHash("Base Layer.dissolve");
    private static readonly int AttackTag = Animator.StringToHash("Attack");

    private float Dissolve_value = 1;

    void Start()
    {
        SetCountText();
        foreach (AudioClip clip in audioClips)
        {
            audioDictionary[clip.name] = clip;
        }
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
        isGrounded = true;
        audioSource = GetComponent<AudioSource>();
        ghostAnim = Ghost.GetComponent<Animator>();
        ghostTransform = Ghost.transform;
        lastCheckpoint = transform.position;
        dead = false;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

        ghostAnim.SetFloat("Speed", movementVector.magnitude);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (dead)
        {
            gameObject.tag = "Untagged";
            PlayerDissolve();
        }
        if(respawn)
        {
            PlayerRespawn();
        }
        // Gestion de la rotation caméra/joueur
        mouseX = lookInput.x * sensitivity;
        mouseY = lookInput.y * sensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        cameraTransform.position = transform.position - (rotation * Vector3.forward * 5f);
        cameraTransform.LookAt(transform.position);
        ghostTransform.rotation = Quaternion.Euler(0, rotationY, 0);
        ghostTransform.position = transform.position + offset;

    }

    private void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * movementY + right * movementX) * speed;
        rb.AddForce(movement);
    }

    public void OnJump(InputValue value)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    public void OnInteract(InputValue value)
    {
        PlaySound(audioDictionary["cat"]);
        Debug.Log("Cat");
    }


    public void UpdateCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpoint = checkpointPosition;
        Debug.Log("Checkpoint mis à jour : " + lastCheckpoint);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("SpeedBoostZone"))
        {
            SpeedBoostZone boost = other.GetComponent<SpeedBoostZone>();
            if (boost != null)
            {
                speed = originalSpeed * boost.boostMultiplier;
            }
        }
        else if (other.gameObject.CompareTag("SlowZone"))
        {
            SlowZone slow = other.GetComponent<SlowZone>();
            if (slow != null)
            {
                speed = originalSpeed * slow.slowMultiplier;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBoostZone") || other.gameObject.CompareTag("SlowZone"))
        {
            speed = originalSpeed;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }


    private void PlayerDissolve()
    {
        Dissolve_value -= Time.deltaTime;
        for (int i = 0; i < MeshR.Length; i++)
        {
            MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
        }
        if (Dissolve_value <= 0)
        {
            dead = false;
            Respawn();
        }
    }
    private void PlayerRespawn()
    {
        for (int i = 0; i < MeshR.Length; i++)
        {
            Dissolve_value += Time.deltaTime;
            MeshR[i].material.SetFloat("_Dissolve", Dissolve_value);
        }
        if (Dissolve_value >= 1)
        {
            respawn = false;
        }
    }
    public void Respawn()
    {
        transform.position = lastCheckpoint;
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.tag = "Player";
        respawn = true;

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    public void SetWinText()
    {
        WinText.SetActive(true);
        countText.transform.position = new Vector3(390, 300, 0);
    }
}
