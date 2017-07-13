using UnityEngine;

public class VersusPlayerMovement : MonoBehaviour
{
    public PlayerHealth playerHpScript;
    public float speed = 4;
    public float fire_rate;
    public float ArrowDirection_X;
    public float ArrowDirection_Y;
    public GameObject Arrow;
    public GameObject Bow;
    public GameObject Spear;
    public GameObject ChargedSpear;
    public GameObject Projectile;
    public GameObject EnergyBubble;
    public GameObject SpearEquippedImage;
    public GameObject SpearUnequippedImage;
    public GameObject BowEquippedImage;
    public GameObject BowUnequippedImage;
    public float ChargeTime = 1.5f;
    public bool chargedArrow;
    public bool spearAttack = false;

    public string button_HorizontalAxis;
    public string button_VerticalAxis;
    public string button_Fire1;
    public string button_Fire2;
    public string button_chooseBow;
    public string button_chooseSpear;
    

    private Rigidbody2D rb2d;
    private float nextFire = 0f;
    private float direction_X;
    private float direction_Y;
    //private float Arrow_face_ypos;
    private SpriteRenderer CharSprite;
    private float ChargingStartTime;
    //private Vector3 ArrowPosition;

    Animator anim;
    public bool facingRight = true;
    public bool BowUse = true;
    public bool SpearUse = false;
    bool shooting = false;
    bool EnableRelease = false;

    //void Start()
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetFloat("Shooting", 0);
        CharSprite = GetComponent<SpriteRenderer>();
        CharSprite.sortingOrder = 8;
        Bow.SetActive(true);
        ChargedSpear.SetActive(false);
        Arrow.SetActive(false);
        Spear.SetActive(false);
        BowUse = true;
        SpearUse = false;
        ArrowDirection_X = 0;
        ArrowDirection_Y = -1;
        //Arrow_face_ypos = Arrow.transform.position.y - transform.position.y;
    }

    void OnEnable()
    {
        CharSprite.sortingOrder = 8;
        ChargedSpear.SetActive(false);
        Arrow.SetActive(false);
        ArrowDirection_X = 0;
        ArrowDirection_Y = -1;
        EnableRelease = false;
        if (SpearUse)
            speed = 3;

        spearAttack = false;
    }

    void Update()
    {
        transform.position = transform.position.y * Vector3.up + transform.position.x * Vector3.right + Vector3.forward * transform.position.y;
        if (!(chargedArrow && EnableRelease) && speed != 8)
        {
            if (Input.GetButtonDown(button_chooseBow))
            {
                SpearUse = false;
                BowUse = true;
                speed = 4;
                ChargeTime = 1.5f;
                Spear.SetActive(false);
                Bow.SetActive(true);
                SpearUnequippedImage.SetActive(true);
                SpearEquippedImage.SetActive(false);
                BowUnequippedImage.SetActive(false);
                BowEquippedImage.SetActive(true);
            }
            if (Input.GetButtonDown(button_chooseBow))
            {
                BowUse = false;
                SpearUse = true;
                speed = 3;
                ChargeTime = 0.9f;
                Bow.SetActive(false);
                Arrow.SetActive(false);
                Spear.SetActive(true);
                SpearUnequippedImage.SetActive(false);
                SpearEquippedImage.SetActive(true);
                BowUnequippedImage.SetActive(true);
                BowEquippedImage.SetActive(false);
                chargedArrow = false;
            }
        }

        SetCharacterSpriteOrder();

        if (Input.GetButtonDown(button_Fire1) && Time.time > nextFire)
        {
            if (BowUse)
            {
                Arrow.SetActive(true);
            }
            else if (SpearUse)
            {
                if (speed != 8)
                {
                    ChargedSpear.SetActive(true);
                }
            }

            chargedArrow = false;
            EnableRelease = true;
            ChargingStartTime = Time.time;
        }

        if (Input.GetButton(button_Fire1) && EnableRelease)
        {
            if (Time.time - ChargingStartTime >= ChargeTime)
            {
                if (BowUse)
                    Instantiate(EnergyBubble, Bow.transform.position, transform.rotation);
                else if (SpearUse)
                {
                    speed = 8;
                }

                ChargingStartTime = Time.time;
                chargedArrow = true;
            }
        }

        if (Input.GetButtonUp(button_Fire1))
        {
            if (EnableRelease && BowUse)
            {
                Instantiate(Projectile, Arrow.transform.position, Arrow.transform.rotation);
                nextFire = Time.time + fire_rate;
                Arrow.SetActive(false);
            }
            else if (SpearUse)
            {
                ChargedSpear.SetActive(false);
                if (EnableRelease && chargedArrow)
                {
                    spearAttack = true;
                    chargedArrow = false;
                }
            }
            EnableRelease = false;
        }

    }


    void FixedUpdate()
    {
        float move_vertical = Input.GetAxisRaw(button_VerticalAxis);
        float move_horizontal = Input.GetAxisRaw(button_HorizontalAxis);

        if (BowUse)
            UseBow(move_horizontal, move_vertical);
        else if (SpearUse)
            UseSpear(move_horizontal, move_vertical);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    void UseBow(float move_horizontal, float move_vertical)
    {


        if (Input.GetButton(button_Fire1) || Input.GetButton(button_Fire2) || Input.GetButtonDown(button_Fire1) || Input.GetButtonDown(button_Fire2))
            shooting = true;
        else
            shooting = false;


        if (!shooting && !EnableRelease)
        {
            anim.SetFloat("Shooting", 0);
            anim.SetFloat("MoveUp", move_vertical);
            anim.SetFloat("MoveRight", Mathf.Abs(move_horizontal));
            direction_X = move_horizontal;
            direction_Y = move_vertical;

            if (move_horizontal != 0 || move_vertical != 0)
            {
                if (move_vertical == 0)
                {
                    ArrowDirection_X = move_horizontal;
                    ArrowDirection_Y = 0;
                }
                else if (move_horizontal == 0)
                {
                    ArrowDirection_Y = move_vertical;
                    ArrowDirection_X = 0;
                }
            }
        }
        else
        {
            anim.SetFloat("Shooting", 1);
            if (move_vertical == 0 && move_horizontal == 0)
                anim.SetFloat("isIdle", 1);
            else
                anim.SetFloat("isIdle", 0);
        }

        Vector2 movement = new Vector2(move_horizontal, move_vertical);

        if ((move_horizontal > 0 || ArrowDirection_Y != 0) && !facingRight && !shooting)
        {
            Flip();
        }
        else if (move_horizontal < 0 && ArrowDirection_Y == 0 && facingRight && !shooting)
        {
            Flip();
        }

        rb2d.MovePosition(rb2d.position + movement * speed * Time.deltaTime);

        //if (Input.GetButton("Fire1") || Input.GetButton("Fire2") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonUp("Fire1")) //testing getbuttonup
        //{ }
        // else
        //   shooting = false;
    }

    void UseSpear(float move_horizontal, float move_vertical)
    {

        if (Input.GetButton(button_Fire1) || Input.GetButton(button_Fire2) || Input.GetButtonDown(button_Fire1) || Input.GetButtonDown(button_Fire2))
            shooting = true;
        else
            shooting = false;

        if (!chargedArrow && !spearAttack)
        {
            if (!shooting && !EnableRelease)
            {
                anim.SetFloat("Shooting", 0);
                anim.SetFloat("MoveUp", move_vertical);
                anim.SetFloat("MoveRight", Mathf.Abs(move_horizontal));
                direction_X = move_horizontal;
                direction_Y = move_vertical;

                if (move_horizontal != 0 || move_vertical != 0)
                {
                    if (move_vertical == 0)
                    {
                        ArrowDirection_X = move_horizontal;
                        ArrowDirection_Y = 0;
                    }
                    else if (move_horizontal == 0)
                    {
                        ArrowDirection_Y = move_vertical;
                        ArrowDirection_X = 0;
                    }
                }
            }
            else
            {
                anim.SetFloat("Shooting", 1);
                if (move_vertical == 0 && move_horizontal == 0)
                    anim.SetFloat("isIdle", 1);
                else
                    anim.SetFloat("isIdle", 0);
            }

            Vector2 movement = new Vector2(move_horizontal, move_vertical);
            if ((move_horizontal > 0 || ArrowDirection_Y != 0) && !facingRight && !shooting)
            {
                Flip();
            }
            else if (move_horizontal < 0 && ArrowDirection_Y == 0 && facingRight && !shooting)
            {
                Flip();
            }
            rb2d.MovePosition(rb2d.position + movement * speed * Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Shooting", 1);
            anim.SetFloat("isIdle", 0);
            Vector2 movement = new Vector2(ArrowDirection_X, ArrowDirection_Y);
            if (spearAttack)
            {
                rb2d.MovePosition(rb2d.position + movement * speed * Time.deltaTime);
            }
        }
    }

    void SetCharacterSpriteOrder()
    {
        if (direction_Y == 1 && direction_X == 0)
        {
            //Arrow.transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
            //ArrowPosition = Arrow.transform.position;
            //ArrowPosition.y = transform.position.y;
            //Arrow.transform.position = ArrowPosition; 
            CharSprite.sortingOrder = 10;
        }
        else if (direction_Y == -1 && direction_X == 0)
        {
            //Arrow.transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
            //ArrowPosition = Arrow.transform.position;
            //ArrowPosition.y = transform.position.y + Arrow_face_ypos;
            //Arrow.transform.position = ArrowPosition;
            CharSprite.sortingOrder = 8;
        }
        else if (direction_X != 0 && direction_Y == 0)
        {
            //Arrow.transform.rotation = Quaternion.AngleAxis(90f, Vector3.forward);
            //if (direction_X == -1)
            //   CharSprite.sortingOrder = 8;
            //else
            CharSprite.sortingOrder = 10;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (SpearUse && spearAttack)
        {
            if (other.CompareTag("Obstacle") || other.CompareTag("EnemyGiant")) //|| other.CompareTag("Enemy")) //|| other.CompareTag("EnemyFast"))
            {
                spearAttack = false;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (playerHpScript.damaged && playerHpScript.vulnerable)
        {
            spearAttack = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (SpearUse) //&& spearAttack)
        {
            if ((other.CompareTag("Obstacle") || other.CompareTag("EnemyGiant")) && 8 == speed && !chargedArrow) // || other.CompareTag("Enemy")) //|| other.CompareTag("EnemyFast"))
            {
                speed = 3;
            }
        }

    }
}
