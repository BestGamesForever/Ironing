using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using Deform;

public class SendRay : MonoBehaviour
{
 
    Transform cloth;
    ObiCloth temporaryObi;
    public GameObject[] obiClothes;
    int index;
    bool ishit;
    public GameObject parentObj;
    CellularNoiseDeformer deformerTransform, deformerTransform1, deformerTransform2;
    public float yoffset;
    private void Start()
    {
        index = -1;
        ishit = true;
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
        Ray ray = new Ray(transform.position+new Vector3(1.5f,yoffset,0), Vector3.down);
        Debug.DrawRay(transform.position + new Vector3(1.5f, yoffset, 0), Vector3.down, Color.blue, 1);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo,1))
        {
            Debug.Log(" hitinfo.collider.name" + hitinfo.collider.name);
            if ((hitinfo.collider.name== "Obi Cloth"|| hitinfo.collider.name == "Thirt" || hitinfo.collider.name == "Obi Cloth (1)") &&ishit)
            {
                cloth = hitinfo.transform;
                temporaryObi = cloth.GetComponent<ObiCloth>();
                StartCoroutine(Ironit());
            }
            if ((hitinfo.collider.name == "Cellular Noise") && ishit)
            {
                Debug.Log("Hit");
                 deformerTransform = hitinfo.collider.GetComponent<CellularNoiseDeformer>();
                 StartCoroutine(hitdeformer());                
            }
            if ((hitinfo.collider.name == "Cellular Noise1")&&ishit)
            {
                deformerTransform1 = hitinfo.collider.GetComponent<CellularNoiseDeformer>();
                StartCoroutine(hitdeformer1());
            }
            if ((hitinfo.collider.name == "Cellular Noise2")&& ishit)
            {
                deformerTransform2 = hitinfo.collider.GetComponent<CellularNoiseDeformer>();
                StartCoroutine(hitdeformer2());
            }
        }
    }
    IEnumerator hitdeformer()
    {       
        deformerTransform.OffsetSpeedVector = Vector4.one * 1.01f;
        deformerTransform.transform.root.GetComponent<MoveSolver>().Yposition();
        float elapsedtime = 0;
        while (ishit)
        {
           // Debug.Log(deformerTransform.MagnitudeVector);
            elapsedtime += Time.deltaTime * .4f;
            deformerTransform.MagnitudeScalar = Mathf.Lerp(deformerTransform.MagnitudeScalar, 0, elapsedtime/50);
            yield return null;
        }       
       
        deformerTransform.OffsetSpeedVector = Vector4.zero;      
    }
    IEnumerator hitdeformer1()
    {

        deformerTransform1.OffsetSpeedVector = Vector4.one * 1.01f;
        deformerTransform1.transform.root.GetComponent<MoveSolver>().Yposition();

        float elapsedtime = 0;
        while (ishit)
        {
            // Debug.Log(deformerTransform.MagnitudeVector);
            elapsedtime += Time.deltaTime * .4f;
            deformerTransform1.MagnitudeScalar = Mathf.Lerp(deformerTransform1.MagnitudeScalar, 0, elapsedtime / 50);
            yield return null;
            deformerTransform1.transform.parent.localPosition = Vector3.Lerp(deformerTransform1.transform.parent.localPosition,
                new Vector3(deformerTransform1.transform.parent.localPosition.x, deformerTransform1.transform.parent.localPosition.y, -10f), elapsedtime/100);
        }
        deformerTransform1.OffsetSpeedVector = Vector4.zero;
    }
    IEnumerator hitdeformer2()
    {
        deformerTransform2.OffsetSpeedVector = Vector4.one * 1.01f;
        deformerTransform2.transform.root.GetComponent<MoveSolver>().Yposition();
        float elapsedtime = 0;
        while (ishit)
        {
            // Debug.Log(deformerTransform.MagnitudeVector);
            elapsedtime += Time.deltaTime * .4f;
            deformerTransform2.MagnitudeScalar = Mathf.Lerp(deformerTransform2.MagnitudeScalar, 0, elapsedtime / 50 );
            deformerTransform2.transform.parent.localPosition = Vector3.Lerp(deformerTransform2.transform.parent.localPosition,
               new Vector3(deformerTransform2.transform.parent.localPosition.x, deformerTransform2.transform.parent.localPosition.y, -10f), elapsedtime/1000);
            yield return null;
        }
        deformerTransform2.OffsetSpeedVector = Vector4.zero;
    }
    IEnumerator Ironit()
    {        
        Debug.Log("Ironhit");
        yield return new WaitForSeconds(.2f);
        foreach (Transform child in cloth)
        {
            child.gameObject.SetActive(false);
        }
        float elapsedtime = 0;
        while (elapsedtime<=1)
        {           
            temporaryObi.stretchingScale = Mathf.Lerp(1.07f, 1, 1 / elapsedtime);
            temporaryObi.maxBending = Mathf.Lerp(1f, 0, 1 / elapsedtime);
            yield return null;
        }      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name== "Obi Cloth"|| other.gameObject.name == "Obi Cloth (1)")
        {
            StartCoroutine(hitTrigger());
        }     
    }
   
    IEnumerator hitTrigger()
    {     
        yield return new WaitForSeconds(1f);
        ishit = false;
        index++;
        Debug.Log($"index{index}");
        obiClothes[index].SetActive(false);
        yield return new WaitForSeconds(1f);
        obiClothes[index + 1].SetActive(true);
        ishit = true;
        Debug.Log("Ishit" + ishit);
    }    
    IEnumerator RiseIron()
    {
        float elapsedtime = 0;
        yoffset = 2f;
        while (elapsedtime<=.2f)
        {
            elapsedtime += Time.deltaTime;
          
            Quaternion targetrot = Quaternion.Euler(0, 0, 18.5F);
            parentObj.transform.rotation = Quaternion.Slerp(parentObj.transform.rotation, targetrot, 1/elapsedtime);
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
}
