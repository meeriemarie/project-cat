using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogChaser : ChaserBase
{
    public float moveSpeed = 6f;
    public float dashForce = 12f;
    public float dashCooldown = 2f;
    public float dashRange = 5f;

    private Rigidbody _rb;
    private bool _canDash = true;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (CanSeePlayer())
        {
            Chase();
        }
    }

    protected override void Chase()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        _rb.MovePosition(_rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        if (_canDash && Vector3.Distance(transform.position, player.position) < dashRange)
        {
            StartCoroutine(Dash(direction));
        }

    }

    private IEnumerator Dash(Vector3 dir)
    {
        _canDash = false;
        _rb.AddForce(dir * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
}

