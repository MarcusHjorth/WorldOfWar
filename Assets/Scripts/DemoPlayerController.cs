using System;
using UnityEngine;

public class DemoPlayerController : MonoBehaviour
{
    public float attackDamage = 10f;
    public float reach = 4f;

    private Health _health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = GetComponent<Health>();

        _health.OnDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Hit();
        }
    }

    void Hit()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, reach))
        {
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.Damage(attackDamage);
            }
        }
    }

    void OnDie()
    {
        Debug.Log("Player died");
    }

    private void OnDrawGizmos()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Gizmos.DrawLine(cameraRay.origin, cameraRay.origin + cameraRay.direction * reach);
    }
}