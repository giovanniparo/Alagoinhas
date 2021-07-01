using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject lanternLight;
    [SerializeField] private Transform[] lanternPositions; //0-Right 1-Down 2-Left 3-Up

    public bool isBusy = false;
    public bool onTarget = false;
    public bool lanternOn = false;
    public Vector3 currentLightDirection;

    private Animator animator;
    private Rigidbody2D rb2D;
    private Light2D light2D;

    private float defaultMoveSpeed;
    private Vector2 move;
    private Vector3 moveFromScriptTarget;
    private Pickup currentPickUp;
    private bool moving = false;
    private bool onPickUp = false;
    private bool lookingFor = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        light2D = lanternLight.GetComponent<Light2D>();
        light2D.enabled = false;
        defaultMoveSpeed = moveSpeed;
        currentLightDirection = lanternLight.transform.up;
    }

    private void Update()
    {
        if (!isBusy && !GameManager.instance.GetGameBusy())
        {
            ProcessInput();
        }
        else if((transform.position - moveFromScriptTarget).magnitude <= 0.1f)
        {
            StopMovement();
            moveSpeed = defaultMoveSpeed;
            onTarget = true;
        }
        if (lanternOn)
            UpdateLanternBattery();

        UpdateAnimator();
        UpdateLanternLight();
        if(lookingFor)
        {
            lookingFor = false;
            StopMovement();
        }
    }

    private void FixedUpdate()
    {
        if (move.magnitude != 0.0f)
        {
            rb2D.MovePosition((Vector2)transform.position + move * moveSpeed * Time.deltaTime);
            SoundManager.instance.PlayWalkSFX();
        }
    }

    private void UpdateLanternBattery()
    {
        //Update GameManager value for battery timer
        GameManager.instance.UpdateBatteryTimer(Time.deltaTime);
        //Update UI interface
        UIManager.instance.UpdateLanternBatteryUI(GameManager.instance.GetBatteryPercentage());
        //Check if battery is empty, turn it off if true
        if(GameManager.instance.GetBatteryPercentage() <= 0.001f)
        {
            lanternOn = false;
            light2D.enabled = lanternOn;
        }
    }

    private void ProcessInput()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.CloseGame();
        }

        if (Input.GetKeyDown(KeyCode.E) && onPickUp && !isBusy)
        {
            Interact();
        }

        if(Input.GetKeyDown(KeyCode.Space) && Inventory.instance.CheckInventory("Lanterna"))
        {
            lanternOn = !lanternOn;
            SoundManager.instance.PlaySFX("Lantern");
            light2D.enabled = lanternOn;
        }
    }

    public void MoveFromScript(Vector3 moveTarget, float moveSpeed = -1.0f)
    {
        onTarget = false;
        moveFromScriptTarget = moveTarget;
        if(moveSpeed > 0.0f)
            this.moveSpeed = moveSpeed;
        move = (moveTarget - transform.position).normalized;
    }

    public void LookTo(Direction lookToDirection)
    {
        lookingFor = true;

        switch (lookToDirection)
        {
            case Direction.Right:
                move = new Vector2(1.0f, 0.0f);
                break;
            case Direction.Down:
                move = new Vector2(0.0f, -1.0f);
                break;
            case Direction.Left:
                move = new Vector2(-1.0f, 0.0f);
                break;
            case Direction.Up:
                move = new Vector2(0.0f, 1.0f);
                break;
        }
    }

    private void Interact()
    {
        if(currentPickUp != null)
        {
            isBusy = true;
            if(currentPickUp.pickableItem.itemName == "CheckedCar")
            {
                SpeechManager.instance.ClearEmote();
                StartCoroutine(CheckingCarCoroutine());
            }
            else if(currentPickUp.pickableItem.itemName == "Lanterna" && Inventory.instance.CheckInventory("Lanterna"))
            {
                GameManager.instance.ResetLanternBattery();
                currentPickUp.ItemPicked();
                isBusy = false;
            }
            else if(currentPickUp.pickableItem.itemName == "LampBook")
            {
                StartCoroutine(CheckingLampBookCoroutine());
            }
            else
            {
                Inventory.instance.AddItem(currentPickUp.pickableItem);
                UIManager.instance.UpdateItemUIImages(currentPickUp.pickableItem);
                SoundManager.instance.PlaySFX("Item");
                currentPickUp.ItemPicked();
                isBusy = false;
            }

            currentPickUp = null;
        }
    }

    private void UpdateLanternLight()
    {
        if (move.magnitude != 0.0f)
        {
            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                if (move.x > 0.0f)
                {
                    lanternLight.transform.position = lanternPositions[0].position;
                    lanternLight.transform.up = lanternPositions[0].transform.up;
                }
                else
                {
                    lanternLight.transform.position = lanternPositions[2].position;
                    lanternLight.transform.up = lanternPositions[2].transform.up;
                }
            }
            else
            {
                if (move.y > 0.0f)
                {
                    lanternLight.transform.position = lanternPositions[3].position;
                    lanternLight.transform.up = lanternPositions[3].transform.up;
                }
                else
                {
                    lanternLight.transform.position = lanternPositions[1].position;
                    lanternLight.transform.up = lanternPositions[1].transform.up;
                }
            }
            currentLightDirection = lanternLight.transform.up;
        }
    }

    private void UpdateAnimator()
    {
        if (move.magnitude != 0.0f)
            moving = true;
        else
            moving = false;

        animator.SetBool("lantern", lanternOn);
        animator.SetBool("move", moving);
        if (move.magnitude != 0.0f)
        {
            animator.SetFloat("moveX", move.x);
            animator.SetFloat("moveY", move.y);
        }
    }

    public void SetPickUp(bool onPickUp, Pickup currentPickUp = null)
    {
        this.onPickUp = onPickUp;
        this.currentPickUp = currentPickUp;
    }

    public void StopMovement()
    {
        move = Vector2.zero;
        moving = false;
    }

    IEnumerator CheckingLampBookCoroutine()
    {
        SpeechManager.instance.ClearEmote();
        SpeechManager.instance.Create(transform, new Vector3(-0.25f, 1.75f, 0), "\"Folhas em um Oceano de Sangue\"");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        isBusy = false;
    }

    IEnumerator CheckingCarCoroutine()
    {
        GameManager.instance.SetGameBusy(true);

        if(Inventory.instance.CheckInventory("Bateria") && Inventory.instance.CheckInventory("Chaves") && Inventory.instance.CheckInventory("Gasolina"))
        {
            SpeechManager.instance.Create(transform, new Vector3(0, 1.75f, 0), "Hora de voltar pra casa...");
            while (SpeechManager.instance.currentSpeechBubbleScript != null)
                yield return null;
            SpeechManager.instance.Create(transform, new Vector3(0, 1.75f, 0), "Mas primeiro...");
            while (SpeechManager.instance.currentSpeechBubbleScript != null)
                yield return null;
            SpeechManager.instance.Create(transform, new Vector3(0, 1.75f, 0), "Preciso passar na delegacia.");
            while (SpeechManager.instance.currentSpeechBubbleScript != null)
                yield return null;
            GameManager.instance.EndGame();
        }
        else if (Inventory.instance.CheckInventory("CheckedCar"))
        {
            SpeechManager.instance.Create(transform, new Vector3(0, 1.75f, 0), "Preciso encontrar as peças!");
            yield return new WaitForSeconds(3.2f);
        }
        else
        {
            Inventory.instance.AddItem(currentPickUp.pickableItem);
            SpeechManager.instance.Create(transform, new Vector3(-0.5f, 1.75f, 0), "Um carro?.... Aqui?");
            while(SpeechManager.instance.currentSpeechBubbleScript != null)
                yield return null;
            SpeechManager.instance.Create(transform, new Vector3(-0.5f, 1.75f, 0), "Eu posso usá-lo para passar pela névoa!");
            while (SpeechManager.instance.currentSpeechBubbleScript != null)
                yield return null;
            SpeechManager.instance.Create(transform, new Vector3(-0.5f, 1.75f, 0), "Parece estar faltando algumas peças...");
            while (SpeechManager.instance.currentSpeechBubbleScript != null)
                yield return null;
            UIManager.instance.ShowObjectives();
        }

        GameManager.instance.SetGameBusy(false);
        isBusy = false;
    }
}
