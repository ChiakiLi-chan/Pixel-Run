using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum Speeds { Slow = 0, Normal= 1, Fast= 2, Faster=3, Fastest=4}
public enum Gamemodes{Cube = 0, Ship = 1, Ball = 2, UFO = 3, Wave = 4, Robot = 5, Spider = 6}

public enum PadType{Yellow = 0, Violet = 1, Blue = 2}

public enum PortalValues{
    Normal = 0, Slow = 1, Fast = 2, Faster = 3, Fastest = 4,
    Cube = 5, Ship = 6, Ball = 7, UFO = 8, Wave = 9, Robot = 10, Spider = 11 
}
public enum GamemodesV2
{
    CubeNormal = 0, CubeSlow = 1, CubeFast = 2, CubeFaster = 3, CubeFastest = 4,
    ShipNormal = 5, ShipSlow = 6, ShipFast = 7, ShipFaster = 8, ShipFastest = 9,
    BallNormal = 10, BallSlow = 11, BallFast = 12, BallFaster = 13, BallFastest = 14,
    UFONormal = 15, UFOSlow = 16, UFOFast = 17, UFOFaster = 18, UFOFastest = 19,
    WaveNormal = 20, WaveSlow = 21, WaveFast = 22, WaveFaster = 23, WaveFastest = 24,
    RobotNormal = 25, RobotSlow = 26, RobotFast = 27, RobotFaster = 28, RobotFastest = 29,
    SpiderNormal = 30, SpiderSlow = 31, SpiderFast = 32, SpiderFaster = 33, SpiderFastest = 34
}


public class Movement : MonoBehaviour
{
    public Speeds CurrentSpeed;
    public Gamemodes CurrentGamemode;
    public GamemodesV2 CurrentGamemodeV2;
    public SpriteChanger spriteChanger;
    public int nextGameMode;

    [System.NonSerialized] public int[] screenHeightValues =  {11, 10, 8, 10, 10, 11 ,9};
    [System.NonSerialized] public float yLastPortal = -2.3f;

    public float[] SpeedValues = {8.6f, 10.4f, 12.96f, 15.6f, 19.27f};
    public float GroundCheckRadius;
    public float fadeDuration = .2f;
    private float startVolume;


    public LayerMask GroundMask;
    public Transform Sprite;
    public Transform MapCompleteAnimationTarget;
    public Transform playerSprite;
    public CameraFollow2 Cam;
    public ResetManager resetManager;
    public Respawner respawner;

    public AttemptCounter attemptcounter;
    
    Rigidbody2D rb;

    public int Gravity = 1;
    public bool clickProcessed = false;
    public bool MapComplete = false;
    public bool isOnBooster = false;
    public bool camfreeze = false;
    public bool isPaused = false;
    public bool isComplete = false;
    public bool isOnBRing = false;
    public bool isOnPRing = false;
    public bool isOnYRing = false;
    public bool isDead = false;
    public Vector3 initialPosition;
    public Quaternion initialRotation;

    private AudioSource mapMusicSource;
    private AudioSource mapCompleteSFX;
    private AudioSource deathSound;

    public int[,] table = new int[34, 11];

    public GameObject LevelCompleteCanvas;
    public GameObject AttemptCanvas;
    public GameObject MCM;

    void Start()
    {
        
        rb= GetComponent<Rigidbody2D>();
        mapMusicSource = GameObject.Find("MapMusic").GetComponent<AudioSource>();
        mapCompleteSFX = GameObject.Find("MapCompleteSFX").GetComponent<AudioSource>();
        startVolume = mapMusicSource.volume;

        GameObject targetObject = GameObject.FindWithTag("MCATarget");
        LevelCompleteCanvas = GameObject.FindWithTag("LVLCMPCanvas");
        spriteChanger.ChangeSprite(0);
                
        LevelCompleteCanvas.SetActive(false);

        if (targetObject != null)
            MapCompleteAnimationTarget = targetObject.transform;
        else
            Debug.LogWarning("MCATarget not found");
    }

    public int DFATable(int x, int y)
    {
        table = new int[35, 12]
        {
        	{ 0, 1, 2, 3, 4, 0, 5, 10, 15, 20, 25, 30 },
            { 0, 1, 2, 3, 4, 1, 6, 11, 16, 21, 26, 31 },
            { 0, 1, 2, 3, 4, 2, 7, 12, 17, 22, 27, 32 },
            { 0, 1, 2, 3, 4, 3, 8, 13, 18, 23, 28, 33 },
            { 0, 1, 2, 3, 4, 4, 9, 14, 19, 24, 29, 34 },
            { 5, 6, 7, 8, 9, 0, 5, 10, 15, 20, 25, 30 },
            { 5, 6, 7, 8, 9, 1, 6, 11, 16, 21, 26, 31 },
            { 5, 6, 7, 8, 9, 2, 7, 12, 17, 22, 27, 32 },
            { 5, 6, 7, 8, 9, 3, 8, 13, 18, 23, 28, 33 },
            { 5, 6, 7, 8, 9, 4, 9, 14, 19, 24, 29, 34 },
            { 10, 11, 12, 13, 14, 0, 5, 10, 15, 20, 25, 30 },
            { 10, 11, 12, 13, 14, 1, 6, 11, 16, 21, 26, 31 },
            { 10, 11, 12, 13, 14, 2, 7, 12, 17, 22, 27, 32 },
            { 10, 11, 12, 13, 14, 3, 8, 13, 18, 23, 28, 33 },
            { 10, 11, 12, 13, 14, 4, 9, 14, 19, 24, 29, 34 },
            { 15, 16, 17, 18, 19, 0, 5, 10, 15, 20, 25, 30 },
            { 15, 16, 17, 18, 19, 1, 6, 11, 16, 21, 26, 31 },
            { 15, 16, 17, 18, 19, 2, 7, 12, 17, 22, 27, 32 },
            { 15, 16, 17, 18, 19, 3, 8, 13, 18, 23, 28, 33 },
            { 15, 16, 17, 18, 19, 4, 9, 14, 19, 24, 29, 34 },
            { 20, 21, 22, 23, 24, 0, 5, 10, 15, 20, 25, 30 },
            { 20, 21, 22, 23, 24, 1, 6, 11, 16, 21, 26, 31 },
            { 20, 21, 22, 23, 24, 2, 7, 12, 17, 22, 27, 32 },
            { 20, 21, 22, 23, 24, 3, 8, 13, 18, 23, 28, 33 },
            { 20, 21, 22, 23, 24, 4, 9, 14, 19, 24, 29, 34 },
            { 25, 26, 27, 28, 29, 0, 5, 10, 15, 20, 25, 30 },
            { 25, 26, 27, 28, 29, 1, 6, 11, 16, 21, 26, 31 },
            { 25, 26, 27, 28, 29, 2, 7, 12, 17, 22, 27, 32 },
            { 25, 26, 27, 28, 29, 3, 8, 13, 18, 23, 28, 33 },
            { 25, 26, 27, 28, 29, 4, 9, 14, 19, 24, 29, 34 },
            { 30, 31, 32, 33, 34, 0, 5, 10, 15, 20, 25, 30 },
            { 30, 31, 32, 33, 34, 1, 6, 11, 16, 21, 26, 31 },
            { 30, 31, 32, 33, 34, 2, 7, 12, 17, 22, 27, 32 },
            { 30, 31, 32, 33, 34, 3, 8, 13, 18, 23, 28, 33 },
            { 30, 31, 32, 33, 34, 4, 9, 14, 19, 24, 29, 34 }
        };
        return table[x,y];
    }

    void FixedUpdate()
    {
        if (isDead) return;
        if (isComplete) return;
    

        if (TouchingWall())
        {
            Die();
            StartCoroutine(RespawnAfterDelay(2f));  
        }
        else if(MapComplete)
        {
            StartSpinAndMove();
            isComplete = true;
        }
        else
        {
            Invoke(CurrentGamemodeV2.ToString(), 0);
            transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
            
            if(isOnBooster)
            {
                if (Input.GetMouseButton(0))
                {
                    if(isOnBRing)
                    {
                        Gravity *= -1;
                        rb.velocity = Vector3.zero;
                        isOnBooster = false;   
                    }
                    else if(isOnYRing)
                    {
                        rb.velocity = Vector2.up * 24.5876f *Gravity;
                        isOnBooster = false;
                    }
                    else if(isOnPRing)
                    {
                        rb.velocity = Vector2.up * 19.1269f *Gravity;
                        isOnBooster = false;
                    }
                }
            }
        }    
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused; 
        Time.timeScale = isPaused ? 0f : 1f; 

        if (isPaused)
        {
            if (mapMusicSource != null) mapMusicSource.Pause();
            Debug.Log("Game paused");
        }
        else
        {
            if (mapMusicSource != null) mapMusicSource.UnPause();
            Debug.Log("Game unpaused");
        }
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.down * Gravity * 0.5f, Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0 , GroundMask);
    }

    bool TouchingWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right*0.55f), Vector2.up * 0.8f + (Vector2.right*GroundCheckRadius),0, GroundMask);
    }

    

    void Cube()
    {
        Generic.createGameMode(rb, this, true, 19.5269f, 9.057f, true, false, 409.1f);
        spriteChanger.ChangeSprite(0);
    }

    void Ship()
    {
        rb.gravityScale = 2.93f * (Input.GetMouseButton(0) ? -1 : 1) * Gravity;
        Generic.LimitYVelocity(9.95f,rb);
        transform.rotation= Quaternion.Euler(0, 0, rb.velocity.y * 2);
        spriteChanger.ChangeSprite(1);
    }
    
    void Ball()
    {
        spriteChanger.ChangeSprite(2);
        Generic.createGameMode(rb,this,true,0,6.2f,false,true);
    }

    void UFO()
    {
        spriteChanger.ChangeSprite(3);
        Generic.createGameMode(rb,this,false,10.841f,4.1483f,false,false,0, 10.841f);
    }

    void Wave()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if(Input.GetMouseButton(0))
            spriteRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);
        else if(OnGround())
            spriteRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        else
            spriteRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, -45f);
        spriteChanger.ChangeSprite(4);
        rb.gravityScale= 0;
        rb.velocity = new Vector2(0, SpeedValues[(int)CurrentSpeed] * (Input.GetMouseButton(0) ? 1 : -1) * Gravity);
    }
    
    float robotXstart = -100;
    bool onGroundProcessed;
    bool gravityFlipped;

    void Robot()
    {
        spriteChanger.ChangeSprite(5);
        if(!Input.GetMouseButton(0))
            clickProcessed = false;

        if(OnGround() && !clickProcessed && Input.GetMouseButton(0))
        {
            gravityFlipped = false;
            clickProcessed = true;
            robotXstart = transform.position.x;
            onGroundProcessed = true;
        }

        if(Mathf.Abs(robotXstart - transform.position.x) <= 3)
        {
            if (Input.GetMouseButton(0) && onGroundProcessed && !gravityFlipped)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.up * 10.4f * Gravity;
                return;
            }
        }
        else if(Input.GetMouseButton(0))
            onGroundProcessed = false;

        rb.gravityScale = 8.62f*Gravity;
        Generic.LimitYVelocity(23.66f, rb);
    }

    void Spider()
    {
        spriteChanger.ChangeSprite(6);
        Generic.createGameMode(rb,this,true,238.29f,6.2f,false,true,0,238.29f);
    }


    void CubeNormal(){
        CurrentSpeed = Speeds.Normal;
        Cube();
    }

    void CubeSlow(){
        CurrentSpeed = Speeds.Slow;
        Cube();
    }

    void CubeFast(){
        CurrentSpeed = Speeds.Fast;
        Cube();
    }

    void CubeFaster(){
        CurrentSpeed = Speeds.Faster;
        Cube();
    }

    void CubeFastest(){
        CurrentSpeed = Speeds.Fastest;
        Cube();
    }

    void ShipNormal(){
        CurrentSpeed = Speeds.Normal;
        Ship();
    }

    void ShipSlow(){
        CurrentSpeed = Speeds.Slow;
        Ship();
    }

    void ShipFast(){
        CurrentSpeed = Speeds.Fast;
        Ship();
    }

    void ShipFaster(){
        CurrentSpeed = Speeds.Faster;
        Ship();
    }

    void ShipFastest(){
        CurrentSpeed = Speeds.Fastest;
        Ship();
    }

    void BallNormal(){
        CurrentSpeed = Speeds.Normal;
        Ball();
    }

    void BallSlow(){
        CurrentSpeed = Speeds.Slow;
        Ball();
    }

    void BallFast(){
        CurrentSpeed = Speeds.Fast;
        Ball();
    }

    void BallFaster(){
        CurrentSpeed = Speeds.Faster;
        Ball();
    }

    void BallFastest(){
        CurrentSpeed = Speeds.Fastest;
        Ball();
    }

    void UFONormal(){
        CurrentSpeed = Speeds.Normal;
        UFO();
    }

    void UFOSlow(){
        CurrentSpeed = Speeds.Slow;
        UFO();
    }

    void UFOFast(){
        CurrentSpeed = Speeds.Fast;
        UFO();
    }

    void UFOFaster(){
        CurrentSpeed = Speeds.Faster;
        UFO();
    }

    void UFOFastest(){
        CurrentSpeed = Speeds.Fastest;
        UFO();
    }

    void WaveNormal(){
        CurrentSpeed = Speeds.Normal;
        Wave();
    }

    void WaveSlow(){
        CurrentSpeed = Speeds.Slow;
        Wave();
    }

    void WaveFast(){
        CurrentSpeed = Speeds.Fast;
        Wave();
    }

    void WaveFaster(){
        CurrentSpeed = Speeds.Faster;
        Wave();
    }

    void WaveFastest(){
        CurrentSpeed = Speeds.Fastest;
        Wave();
    }

    void RobotNormal(){
        CurrentSpeed = Speeds.Normal;
        Robot();
    }

    void RobotSlow(){
        CurrentSpeed = Speeds.Slow;
        Robot();
    }

    void RobotFast(){
        CurrentSpeed = Speeds.Fast;
        Robot();
    }

    void RobotFaster(){
        CurrentSpeed = Speeds.Faster;
        Robot();
    }

    void RobotFastest(){
        CurrentSpeed = Speeds.Fastest;
        Robot();
    }

    void SpiderNormal(){
        CurrentSpeed = Speeds.Normal;
        Spider();
    }

    void SpiderSlow(){
        CurrentSpeed = Speeds.Slow;
        Spider();
    }

    void SpiderFast(){
        CurrentSpeed = Speeds.Fast;
        Spider();
    }

    void SpiderFaster(){
        CurrentSpeed = Speeds.Faster;
        Spider();
    }

    void SpiderFastest(){
        CurrentSpeed = Speeds.Fastest;
        Spider();
    }

    public void InteractionBetweenPad(PadType padType, int gravity, int State, float yPad)
    {
        switch(State)
        {
            case 0:
                Debug.Log("Pad Hit");
                switch((int)padType)
                {
                    case 0:
                        rb.velocity = Vector2.up * 27.5876f *Gravity;
                        break;
                    case 1:
                        rb.velocity = Vector2.up * 19.1269f *Gravity;
                        break;
                    case 2:
                        Gravity = gravity;
                        rb.gravityScale = Mathf.Abs(rb.gravityScale)* (int)gravity;
                        gravityFlipped = true;
                        break;
                }
                break;
            case 1:
                break;
        }
    }

    public void ChangeThroughPortal(PortalValues portalValue, int gravity, int State, float yPortal)
    {
        transform.rotation = Quaternion.identity;
        switch(State)
        {
            case 0:
                /*Gravity = gravity;
                rb.gravityScale = Mathf.Abs(rb.gravityScale)* (int)gravity;
                gravityFlipped = true;
                */
                Gravity *= -1;
                break;
            case 1:
                nextGameMode = DFATable((int)CurrentGamemodeV2, (int)portalValue);
                switch(nextGameMode)
                {
                    case 0:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.CubeNormal;
                        break;
                    case 1:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.CubeSlow;
                        break;
                    case 2:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.CubeFast;
                        break;
                    case 3:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.CubeFaster;
                        break;
                    case 4:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.CubeFastest;
                        break;
                    case 5:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.ShipNormal;
                        transform.rotation = Quaternion.identity;
                        break;
                    case 6:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.ShipSlow;
                        break;
                    case 7:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.ShipFast;
                        break;
                    case 8:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.ShipFaster;
                        break;
                    case 9:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.ShipFastest;
                        break;
                    case 10:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.BallNormal;
                        break;
                    case 11:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.BallSlow;
                        break;
                    case 12:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.BallFast;
                        break;
                    case 13:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.BallFaster;
                        break;
                    case 14:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.BallFastest;
                        break;
                    case 15:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.UFONormal;
                        break;
                    case 16:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.UFOSlow;
                        break;
                    case 17:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.UFOFast;
                        break;
                    case 18:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.UFOFaster;
                        break;
                    case 19:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.UFOFastest;
                        break;
                    case 20:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.WaveNormal;
                        break;
                    case 21:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.WaveSlow;
                        break;
                    case 22:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.WaveFast;
                        break;
                    case 23:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.WaveFaster;
                        break;
                    case 24:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.WaveFastest;
                        break;
                    case 25:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.RobotNormal;
                        break;
                    case 26:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.RobotSlow;
                        break;
                    case 27:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.RobotFast;
                        break;
                    case 28:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.RobotFaster;
                        break;
                    case 29:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.RobotFastest;
                        break;
                    case 30:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.SpiderNormal;
                        break;
                    case 31:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.SpiderSlow;
                        break;
                    case 32:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.SpiderFast;
                        break;
                    case 33:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.SpiderFaster;
                        break;
                    case 34:
                        yLastPortal = yPortal;
                        CurrentGamemodeV2 = GamemodesV2.SpiderFastest;
                        break; 
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Die();
            StartCoroutine(RespawnAfterDelay(2f));
        }
    } 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalScript portal = collision.gameObject.GetComponent<PortalScript>();
        PadScript pad = collision.gameObject.GetComponent<PadScript>();
        TransporterScript transporter = collision.gameObject.GetComponent<TransporterScript>();
        if(portal)
            portal.initiatePortal(this);
        else if (pad)
            pad.initiatePad(this);
        else if(collision.gameObject.CompareTag("YellowRing"))
        {
            isOnBooster = true;
            isOnYRing = true;           
        }
        else if(collision.gameObject.CompareTag("PurpleRing"))
        {
            isOnBooster = true;
            isOnPRing = true;
        }
        else if(collision.gameObject.CompareTag("BlueRing"))
        {
            isOnBooster = true;
            isOnBRing = true;
        }
        else if (collision.gameObject.CompareTag("CameraStopper"))
        {
            Cam.FreezeCamera();
        }   
        else if (collision.gameObject.CompareTag("MapComplete"))
        {
            MapComplete = true;
            AttemptCanvas.SetActive(false);
            MCM.SetActive(false);
        }      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("YellowRing"))
        {
            isOnBooster = false;
            isOnYRing = false;
        }
        else if (collision.gameObject.CompareTag("PurpleRing"))
        {
            isOnBooster = false;
            isOnPRing = false;
        }
        else if (collision.gameObject.CompareTag("BlueRing"))
        {
            isOnBooster = false;
            isOnBRing = false;
        }

    }

    private void attemptUpdate()
    {
        attemptcounter.IncrementAttempts();
    }

    public void Die()
    {
        camfreeze =true;
        isDead = true;
        StopMovement();
        mapMusicSource.Stop();
        playerSprite = transform.Find("PlayerSprite");
        deathSound = GameObject.Find("deathSound").GetComponent<AudioSource>();

        if (deathSound != null)
            deathSound.Play();
        if (playerSprite != null)
            playerSprite.gameObject.SetActive(false);
    }


    public void StopMovement()
    {
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(0f, currentVelocity.y); 
        rb.isKinematic = true;
        rb.angularVelocity = 0f;
    }


    IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }
    
    public void Respawn()
    {
        resetManager.ResetAllObjects();
        MCM.SetActive(true);
        AttemptCanvas.SetActive(true);
        attemptUpdate();
        CurrentGamemodeV2 = GamemodesV2.CubeNormal;
        Gravity = 1;
        camfreeze = false;
        isDead = false;
        playerSprite = transform.Find("PlayerSprite");

        if (playerSprite != null)
            playerSprite.gameObject.SetActive(true);

        respawner.RespawnPlayer();
        
        //transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.isKinematic = false; 
        mapMusicSource.time = 0f;
        mapMusicSource.Play();
        mapMusicSource.volume  = startVolume;
    }

    public void RespawnButton(){        
        LevelCompleteCanvas.SetActive(false);
        isComplete = false;
        MapComplete = false;
        Cam.UnfreezeCamera();
        Respawn(); 
        mapMusicSource.volume  = startVolume;
    }

    public void StartSpinAndMove()
    {
        StartCoroutine(FadeOutRoutine());
        mapCompleteSFX.Play();
        Generic.createGameMode(rb, this, true, 19.5269f, 9.057f, true, false, 409.1f);
        StartCoroutine(SpinAndMoveRoutine());   
    }

    private IEnumerator SpinAndMoveRoutine()
    {
        float elapsedTime = 0f;
        
        Vector3 startPosition = transform.position;
        float startRotation = transform.rotation.eulerAngles.z;

        float moveDuration = 2.2f;
        elapsedTime = 0f;
        Vector3 targetPosition = MapCompleteAnimationTarget.position;
        float height = 5f; // Adjust this for the height of the arc

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            float arcHeight = Mathf.Sin(Mathf.PI * t) * height;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t) + Vector3.up * arcHeight;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        LevelCompleteCanvas.SetActive(true);
    }
    private IEnumerator FadeOutRoutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            float targetVolume = startVolume /2f;
            mapMusicSource.volume = Mathf.Lerp(startVolume, targetVolume, t); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    

}
