using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Controls stats and allowed actions
//Children are different playable "classes"
public abstract class Player : MonoBehaviour
{
    //Initialize Objects
    [Header("Objects")]
    public HealthBar healthbar;
    public PlayerControllerNew playerController;

    //Default Player Stats
    private string playerName = "OZ3N";
    protected float maxHealth = 100;
    protected float currentHealth;
    protected int currentLevel = 0;
    protected float invulnerableLength = 1f;

    //Player States
    [Header("Player States")]
    public bool isInvulnerable = false;

    //Movement
    [Header("Movement")]
    public float moveSpeed;
    public float attackMoveSpeedMod = 0.3f;
    protected float baseMoveSpeed = 5f;
    protected float moveSpeedMod = 1f;

    //Dashing
    [Header("Dashing")]
    public float dashSpeedMod = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    int maxDashCount = 2;
    int dashCount;
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

    public abstract IEnumerator Fire();

    public abstract IEnumerator Attack();
}
