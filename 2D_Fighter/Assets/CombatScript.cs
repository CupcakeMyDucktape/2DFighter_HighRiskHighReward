using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour {

    [SerializeField] float m_speed = 1.0f;
    [SerializeField] float m_jumpForce = 2.0f;

    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayer;


    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    public bool Active;

    // Use this for initialization
    void Start() {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    void Update() {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0) {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if (inputX < 0) {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
    
        //Attack
        else if (Input.GetKeyDown("p")) {
            Attack();
        }

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded) {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    void Attack() {
        //Play Attack animation
        m_animator.SetTrigger("Attack");

        //Detect Enemies in range of the attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        //Deals damage to the enemies
        foreach (Collider enemy in hitEnemies) {
            Debug.Log("We hit " + enemy.name);
        }
    }
    void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;

        //Draws a circle to show where the attack will hit.
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
