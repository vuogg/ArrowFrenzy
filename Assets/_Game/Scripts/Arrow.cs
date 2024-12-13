using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 velocity;
    public LayerMask reflectLayers;
    public float arrowSpeed = 10f;

    public void Launch(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
    }

    void Update()
    {
        //chuyen huong mui ten sau khi nay
        transform.position += velocity * Time.deltaTime;

        if (velocity != Vector3.zero) 
        { 
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        //Raycast kiem tra va cham
        RaycastHit hit;
        if (Physics.Raycast(transform.position, velocity.normalized, out hit, velocity.magnitude * Time.deltaTime + 0.1f, reflectLayers))
        {
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Buff"))
            {
                Reflect(hit);
            }
            else if (hit.collider.CompareTag("Target"))
            {
                //gay sat thuong cho target
                DamageTarget(hit.collider.gameObject);
                //tu huy mui ten sau khi trung target
                Destroy(gameObject);
            }
        }
    }

    void Reflect(RaycastHit hit)
    {
        //vector phap tuyen va phan xa
        //Phap tuyen tai diem va cham
        Vector3 normal = hit.normal;

        //vector phan xa
        velocity = Vector3.Reflect(velocity, normal);

        //dich chuyen 1 chut de khong va cham lap lai
        //transform.position = hit.point + normal * 0.01f;
    }

    void DamageTarget(GameObject target)
    {
        Target targetScript = target.GetComponent<Target>();
        if (targetScript != null)
        {
            targetScript.TakeDamage(1);
        }
    }
}
