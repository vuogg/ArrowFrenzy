using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : GameUnit
{
    [SerializeField] private BoxCollider col;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trailRenderer;

    private Vector3 velocity;

    public int maxReflects = 10;
    public int currentReflects;
    public Target target;
    public LayerMask reflectLayers;
    public LayerMask buffLayers;
    public LayerMask targetLayers;
    public float arrowSpeed = 10f;

    bool isStuck = false;

    private void FixedUpdate()
    {
        //PredictRaycast();

        if (!isStuck)
        {
            Vector3 nextPoint = TF.position + arrowSpeed * Time.fixedDeltaTime * velocity.normalized;

            RaycastHit hit;
            if (Physics.Raycast(TF.position, velocity.normalized, out hit, arrowSpeed * Time.fixedDeltaTime + 0.1f, reflectLayers | targetLayers | buffLayers))
            {
                Debug.DrawRay(TF.position, velocity.normalized, Color.red, arrowSpeed * Time.fixedDeltaTime + 0.1f);
                if ((reflectLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    Reflect(hit);
                    return;
                }
                else if ((targetLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    HitTarget(hit);
                    return;
                }
                else if ((buffLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    HandleBuff(hit);
                    return;
                }
            }

            TF.position = nextPoint;

            if (velocity != Vector3.zero)
            {
                TF.rotation = Quaternion.LookRotation(velocity);
            }

            CheckOutOfBounds();
        }
    }

    //private void FixedUpdate()
    //{
    //    if (!isStuck)
    //    {
    //        Vector3 nextPoint = TF.position + arrowSpeed * Time.fixedDeltaTime * velocity.normalized;

    //        RaycastHit hit;

    //        // Tia PredictRaycast dài hơn
    //        if (Physics.Raycast(TF.position, velocity.normalized, out hit, (arrowSpeed * Time.fixedDeltaTime) * 2f, targetLayers))
    //        {
    //            Target potentialTarget = Cache.GetTarget(hit.collider.gameObject);

    //            // Kiểm tra điều kiện slow motion
    //            if (LevelManager.Instance.currentLevel.activeTargets.Count == 1 && potentialTarget != null && potentialTarget.hp == 1)
    //            {
    //                TriggerSlowMotion();
    //            }
    //        }

    //        // Tia Raycast gốc
    //        if (Physics.Raycast(TF.position, velocity.normalized, out hit, arrowSpeed * Time.fixedDeltaTime + 0.1f, reflectLayers | targetLayers | buffLayers))
    //        {
    //            Debug.DrawRay(TF.position, velocity.normalized, Color.red, arrowSpeed * Time.fixedDeltaTime + 0.1f);
    //            if ((reflectLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
    //            {
    //                Reflect(hit);
    //                return;
    //            }
    //            else if ((targetLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
    //            {
    //                HitTarget(hit);
    //                return;
    //            }
    //            else if ((buffLayers.value & (1 << hit.collider.gameObject.layer)) != 0)
    //            {
    //                HandleBuff(hit);
    //                return;
    //            }
    //        }

    //        TF.position = nextPoint;

    //        if (velocity != Vector3.zero)
    //        {
    //            TF.rotation = Quaternion.LookRotation(velocity);
    //        }

    //        CheckOutOfBounds();
    //    }
    //}

    private void CheckOutOfBounds()
    {
        if(TF.position.x < -13f || TF.position.x > 13f || TF.position.z < -18f || TF.position.z > 18f)
        {
            SimplePool.Despawn(this);
            UnregisterArrow();
        }
    }

    public void OnInit()
    {
        isStuck = false;
        currentReflects = 0;
        rb.isKinematic = false;
        col.enabled = true;
        //LevelManager.Instance.currentLevel = null;
    }    

    public void Launch(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
    }

    private void HandleBuff(RaycastHit hit)
    {
        Buff buff = Cache.GetBuff(hit.collider);
        if (buff != null)
        {
            buff.ArrowMultiply(velocity.normalized);
            if (hit.collider.CompareTag(Constants.TAG_GREENBUFF))
            {
                buff.ChangeAnim(Constants.ANIM_SHAKING);
                Reflect(hit);
            }
            else if (hit.collider.CompareTag(Constants.TAG_YELLOWBUFF))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void HitTarget(RaycastHit hit)
    {
        ParticlePool.Play(ParticleType.Blood, hit.point, Quaternion.LookRotation(hit.normal));

        OnHit targetScript = Cache.GetOnHit(hit.collider);
        if (targetScript != null)
        {
            targetScript.TakeDamage(1);
        }

        StickToTarget(hit);
        UnregisterArrow();
    }

    //private void HitTarget(RaycastHit hit)
    //{
    //    ParticlePool.Play(ParticleType.Blood, hit.point, Quaternion.LookRotation(hit.normal));

    //    OnHit targetScript = Cache.GetOnHit(hit.collider);
    //    if (targetScript != null)
    //    {
    //        targetScript.TakeDamage(1);
    //    }

    //    StickToTarget(hit);

    //    if (LevelManager.Instance.currentLevel.activeTargets.Count == 1 &&
    //        LevelManager.Instance.currentLevel.activeTargets[0].hp <= 0)
    //    {
    //        // Sau khi target cuối cùng bị hạ
    //        StartCoroutine(RestoreTime());
    //    }

    //    UnregisterArrow();
    //}

    void Reflect(RaycastHit hit)
    {
        //vector phap tuyen va phan xa
        //Phap tuyen tai diem va cham
        Vector3 normal = hit.normal;

        //vector phan xa
        velocity = Vector3.Reflect(velocity, normal);

        //dich chuyen 1 chut de khong va cham lap lai
        TF.position = hit.point + normal * 0.09f;

        currentReflects++;
        if (currentReflects >= maxReflects)
        {
            SimplePool.Despawn(this);
            UnregisterArrow();
        }
    }

    private void StickToTarget(RaycastHit hit)
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;

        float depth = 0.45f;
        Vector3 hitPosition = hit.point - TF.forward * depth;


        TF.SetParent(hit.collider.transform);
        //huong mui ten sau khi gam vao
        TF.SetPositionAndRotation(hitPosition, Quaternion.LookRotation(velocity.normalized));
        isStuck = true;
    }

    //private void PredictRaycast()
    //{
    //    Vector3 currentPosition = TF.position;
    //    Vector3 predictedVelocity = velocity.normalized;
    //    float step = 0.1f;
    //    Ray ray = new(currentPosition, predictedVelocity);

    //    if (Physics.Raycast(ray, out RaycastHit hit, step, targetLayers))
    //    {
    //        Debug.DrawRay(currentPosition, predictedVelocity, Color.black, step);
    //        //Target target = hit.collider.GetComponent<Target>();
    //        if (LevelManager.Instance.currentLevel.ShouldTriggerSlowMotion())
    //        {
    //            //timeManager.DoSlowMotion();
    //            //TimeManager.Instance.DoSlowMotion();
    //            StartSlowMotion();
    //            return;
    //        }
    //    }
    //}

    //    //RaycastHit predictHit;
    //    //if (Physics.Raycast(TF.position, velocity.normalized, out predictHit, arrowSpeed * Time.fixedDeltaTime + 0.5f, targetLayers))
    //    //{
    //    //    Debug.DrawRay(TF.position, velocity.normalized, Color.black, arrowSpeed * Time.fixedDeltaTime + 0.5f);
    //    //    if (LevelManager.Instance.currentLevel.ShouldTriggerSlowMotion())
    //    //    {
    //    //        StartSlowMotion();
    //    //        return;
    //    //    }
    //    //}
    //}

    public void RegisterArrow()
    {
        LevelManager.Instance.currentLevel.RegisterArrow(this);
    }

    public void UnregisterArrow()
    {
        LevelManager.Instance.currentLevel.UnregisterArrow(this);
    }

    //private void StartSlowMotion()
    //{
    //    Time.timeScale = 0.2f;
    //    StartCoroutine(IEResetTimeScale());
    //}

    //private IEnumerator IEResetTimeScale()
    //{
    //    yield return new WaitForSecondsRealtime(2f);
    //    //yield return Cache.GetWFS(2f);
    //    Time.timeScale = 1f;
    //}

    private void TriggerSlowMotion()
    {
        Time.timeScale = 0.2f; // Làm chậm thời gian
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Điều chỉnh FixedUpdate để khớp với Time.timeScale
    }

    // Hàm khôi phục thời gian sau khi mũi tên găm vào target
    private IEnumerator RestoreTime()
    {
        float restoreDuration = 2f;
        float elapsed = 0f;

        while (elapsed < restoreDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(0.2f, 1f, elapsed / restoreDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // Khôi phục giá trị mặc định
    }
}
