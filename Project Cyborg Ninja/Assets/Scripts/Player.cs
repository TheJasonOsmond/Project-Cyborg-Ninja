using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Controls stats and allowed actions
//Children are different playable "classes"
public class Player : MonoBehaviour
{
    //Initialize Objects
    public HealthBar healthbar;

    //Default Player Stats
    private string playerName = "OZ3N";
    protected float maxHealth;
    protected float currentHealth;
    protected int currentLevel = 0;
    protected float invulnerableLength = 1f;

    //Player States
    public bool isInvulnerable = false;

    //Movement
    protected float baseMoveSpeed = 5f;
    protected float moveSpeedMod = 1f;
    public float moveSpeed;
    public float attackMoveSpeedMod = 0.3f;

    //Dashing
    //[HideInInspector] public float dashSpeedMod = 12f;
    //[HideInInspector] public float dashDuration= 0.5f;
    //[HideInInspector] public float dashCooldown = 2f;
    [HideInInspector] int maxDashCount = 2;
    [HideInInspector] int dashCount;

    public float dashSpeedMod = 12f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 2f;


    public int dashCounter
    {
        get
        {
            return dashCount;
        }
        set
        {
            dashCount = value;
            if (dashCount < 0)
                dashCount = 0;
            else if (dashCount > maxDashCount)
                dashCount = maxDashCount;
        }
    }






    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);

        moveSpeed = baseMoveSpeed * moveSpeedMod;
        dashCount = maxDashCount;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public abstract IEnumerator Fire();

    //public abstract IEnumerator Attack();
}
