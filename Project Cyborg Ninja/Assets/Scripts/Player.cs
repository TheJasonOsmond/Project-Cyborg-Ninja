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
    protected float _dashSpeed = 14f;
    protected float _dashLength = 0.2f;
    protected float _dashCooldown = 0.5f;
    protected int _dashCount = 1;

    
    


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);

        moveSpeed = baseMoveSpeed * moveSpeedMod;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public abstract IEnumerator Fire();

    //public abstract IEnumerator Attack();
}
