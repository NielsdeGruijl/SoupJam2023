using DevKit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    private Vector2 mousePos;

    [Header("World")]
    [SerializeField] Camera cam;
    [SerializeField] GameObject player;

    [Header("Sweep Attack")]
    [SerializeField] private float sweepRange;
    [SerializeField] private float sweepCooldown;
    [SerializeField] private GameObject sweepArea;

    [Header("Divine Light")]
    [SerializeField] private float chargeTime;
    [SerializeField] private float divineRange;
    [SerializeField] private float divineCooldown;
    [SerializeField] private GameObject divineArea;
    public bool inDivineState = false;

    [Header("Holy Slam")]
    [SerializeField] private float slamRange;
    [SerializeField] private GameObject slamArea;

    void Update()
    {
        transform.eulerAngles = LookAt2D.LookAtMouse(transform).eulerAngles;
    }

    public void ExecuteSweepAttack()
    {
        sweepArea.SetActive(true);
        StartCoroutine(SweepAttack());
        if (getPlayerDistance(transform.position) < sweepRange)
        {
            Vector2 heading = player.transform.position - transform.position;
            float dotProduct = Vector2.Dot(heading, transform.right.normalized);
            if (dotProduct > 0)
            {
                Debug.Log("Sweep Attack Hit Player");
            }
        }
    }

    IEnumerator SweepAttack()
    {
        yield return new WaitForSeconds(0.1f);
        sweepArea.SetActive(false);
    }
    
    public void EnterDivineState()
    {
        inDivineState = true;
    }

    public void ExecuteDivineLight()
    {
        //if (!inDivineState) return;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        divineArea.SetActive(true);
        divineArea.transform.position = mousePos;
        StartCoroutine(DivineLight());
    }

    IEnumerator DivineLight()
    {
        yield return new WaitForSeconds(chargeTime);
        if(getPlayerDistance(mousePos) < divineRange)
        {
            Debug.Log("Divine Light Hit Player");
        }
        divineArea.SetActive(false);
    }

    public void ExecuteHolySlam()
    {
        slamArea.SetActive(true);
        StartCoroutine(HolySlam());
        if(getPlayerDistance(transform.position) < slamRange)
        {
            Debug.Log("Holy Slam Hit Player");
        }
    }

    IEnumerator HolySlam()
    {
        yield return new WaitForSeconds(0.1f);
        slamArea.SetActive(false);
    }

    private float getPlayerDistance(Vector3 pos)
    {
        return ((player.transform.position + (player.transform.localScale/2)) - pos).magnitude;
    }
}
