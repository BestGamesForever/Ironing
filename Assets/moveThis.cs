using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveThis : MonoBehaviour
{
    Vector3 startPos;
    Vector3 EndPos;
    public Camera renderCam;
    public float turnSpeed;
    bool ismouseUp;
    private void Start()
    {
        startPos = transform.localPosition;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ismouseUp = false;
            startPos = renderCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        }
        if (Input.GetMouseButton(0))
        {
            EndPos = renderCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            Vector3 direction = EndPos - startPos;
            transform.localPosition = new Vector3(Mathf.Clamp((transform.localPosition.x + direction.x / 20),-2.5f,1.5f), 
                -.3f, 
               Mathf.Clamp((transform.localPosition.z + direction.z / 20),1.35f,11.5f));
            Quaternion targetRotation = Quaternion.LookRotation((EndPos - startPos));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        if (Input.GetMouseButtonUp(0))
        {
            ismouseUp = true;
            StartCoroutine(IdleOfIron());
        }
    }
    IEnumerator IdleOfIron()
    {
        float elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime;
            Vector3 targetPos = new Vector3(transform.localPosition.x, .5f, transform.localPosition.z);
            Quaternion targetRotation = Quaternion.Euler(-90, 45, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, .5f / elapsedTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, .5f / elapsedTime);
            yield return null;
        }
    }
}
