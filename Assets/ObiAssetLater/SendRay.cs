using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using Deform;

public class SendRay : MonoBehaviour
{
    [SerializeField] float speed;
    public List<Transform> cloths;
    Transform cloth;
    Transform clothParent;
    ObiCloth temporaryObi;
    public GameObject[] obiClothes;
    int index;
    bool ishit;
    bool isParent, isParent2, isParent3;
    public GameObject parentObj;

    public Deform.Masking.BoxMask deformerTransform, deformerTransform1, deformerTransform2;
    public float yoffset;

    private void Start()
    {
        cloths = new List<Transform>();
        yoffset = 2;
        index = -1;
        ishit = true;
        isParent = false;
        isParent2 = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ishit = true;
            StartCoroutine(DescentIron());
        }
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(RiseIron());
            ishit = false;
        }
        if (Input.GetMouseButton(0))
        {
            ishit = true;
        }
        Ray ray = new Ray(transform.position + new Vector3(1.5f, yoffset, 0), Vector3.down);
        Debug.DrawRay(transform.position + new Vector3(1.5f, yoffset, 0), Vector3.down, Color.blue, 1);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, 1))
        {
           // Debug.Log(" hitinfo.collider.name" + hitinfo.collider.name);
            if ((hitinfo.collider.name == "Box Mask") && ishit)
            {
                TemporarySpeed._offsetvector = .5f;
                StartCoroutine(hitend());
                Debug.Log("Hit");
                deformerTransform = hitinfo.collider.GetComponent<Deform.Masking.BoxMask>();
                deformerTransform.GetComponent<MoveSolver>().Speed = -speed;              
                isParent = true;                            
            }
            if ((hitinfo.collider.name == "Box MaskSecond") && ishit)
            {
                TemporarySpeed._offsetvector = .5f;
                StartCoroutine(hitend());
                cloths.Clear();
                cloths.Add(hitinfo.collider.transform);
                if (isParent)
                {
                    deformerTransform.GetComponent<MoveSolver>().Speed = 0f;
                    isParent = false;
                }
                deformerTransform1 = hitinfo.collider.GetComponent<Deform.Masking.BoxMask>();               
                isParent2 = true;
                StartCoroutine(hitdeformer());               
            }
            if ((hitinfo.collider.name == "Box MaskThird") && ishit)
            {
                TemporarySpeed._offsetvector = .5f;
                StartCoroutine(hitend());
                cloths.Clear();
                cloths.Add(hitinfo.collider.transform);
                if (isParent2)
                {
                    deformerTransform1.GetComponent<MoveSolver>().Speed = 0f;
                    isParent2 = false;
                    deformerTransform.GetComponent<MoveSolver>().Speed = 0f;
                    isParent = false;
                }               
                deformerTransform2 = hitinfo.collider.GetComponent<Deform.Masking.BoxMask>();
                isParent3 = true;
                StartCoroutine(hitdeformer());
            }
        }
    }
    IEnumerator hitdeformer()
    {       
        float elapsedtime = 0;
        while (isParent2)
        {
            elapsedtime += Time.deltaTime * .4f;
            Bounds bounds = cloths[0].GetComponent<Deform.Masking.BoxMask>().OuterBounds;
            bounds.center = Vector3.Lerp(bounds.center, new Vector3(1.19f, -21f, 0), Time.deltaTime / 50);
            bounds.extents = Vector3.Lerp(bounds.extents, new Vector3(.46f, 4.6f, 2), Time.deltaTime / 50);
            cloths[0].GetComponent<Deform.Masking.BoxMask>().OuterBounds = bounds;

            Bounds bounds2 = cloths[0].GetComponent<Deform.Masking.BoxMask>().InnerBounds;
            bounds2.center = Vector3.Lerp(bounds.center, new Vector3(1.19f, -21f, 0), Time.deltaTime / 50);
            bounds2.extents = Vector3.Lerp(bounds.extents, new Vector3(.46f, 5.7f, 2), Time.deltaTime / 50);
            cloths[0].GetComponent<Deform.Masking.BoxMask>().InnerBounds = bounds2;
            yield return null;
        }
        while (isParent3)
        {
            elapsedtime += Time.deltaTime * .4f;
            Bounds bounds = cloths[0].GetComponent<Deform.Masking.BoxMask>().OuterBounds;
            bounds.center = Vector3.Lerp(bounds.center, new Vector3(1.19f, -22f, 0), Time.deltaTime / 50);
            bounds.extents = Vector3.Lerp(bounds.extents, new Vector3(.46f, 11f, 2), Time.deltaTime / 50);
            cloths[0].GetComponent<Deform.Masking.BoxMask>().OuterBounds = bounds;

            Bounds bounds2 = cloths[0].GetComponent<Deform.Masking.BoxMask>().InnerBounds;
            bounds2.center = Vector3.Lerp(bounds2.center, new Vector3(1.19f, -22f, 0), Time.deltaTime / 50);
            bounds2.extents = Vector3.Lerp(bounds2.extents, new Vector3(0.45f, 5.7f, 2), Time.deltaTime / 50);
            cloths[0].GetComponent<Deform.Masking.BoxMask>().InnerBounds = bounds2;
            yield return null;
        }
    }
   
    IEnumerator RiseIron()
    {
        if (isParent)
        {
            deformerTransform.GetComponent<MoveSolver>().Speed = 0f;
            isParent = false;
        }
        if (isParent2)
        {
           //deformerTransform1.GetComponent<MoveSolver>().Speed = 0f;
            isParent2 = false;
        }
        if (isParent3)
        {
            //deformerTransform1.GetComponent<MoveSolver>().Speed = 0f;
            isParent3 = false;
        }
        float elapsedtime = 0;
        yoffset = 2f;
        while (elapsedtime <= .2f)
        {
            elapsedtime += Time.deltaTime;

            Quaternion targetrot = Quaternion.Euler(0, 0, 18.5f);
            parentObj.transform.rotation = Quaternion.Slerp(parentObj.transform.rotation, targetrot, 1 / elapsedtime);
            yield return null;
        }
      
    }
    IEnumerator DescentIron()
    {
        float elapsedtime = 0;
        yoffset = 0f;
        while (elapsedtime <= .2f)
        {
            elapsedtime += Time.deltaTime;
            Quaternion targetrot = Quaternion.Euler(0, 0, -27);
            parentObj.transform.rotation = Quaternion.Slerp(parentObj.transform.rotation, targetrot, 1 / elapsedtime);
            yield return null;
        }
    }
    IEnumerator hitend()
    {
        yield return new WaitForSeconds(.25f);
        TemporarySpeed._offsetvector = 0;
    }
}
