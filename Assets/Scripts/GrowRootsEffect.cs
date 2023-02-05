using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRootsEffect : MonoBehaviour
{

    public List<MeshRenderer> growRootMeshes;
    public float timeToGrow = 5;
    public float refreshhRate = 0.05f;
    [Range(0, 1)]
    public float minGrow = 0.2f;
    [Range(0, 1)]
    public float maxGrow = 0.97f;

    private List<Material> growMaterials = new List<Material>();
    private bool fullyGrown;

    private int index;
    private bool stop;
    // Start is called before the first frame update
    void Start()
    {

        if (stop)
        {
            return;
        }

        for (int i = 0; i < growRootMeshes.Count; i++)
        {
            for (int j = 0; j < growRootMeshes[i].materials.Length; j++)
            {
                if (growRootMeshes[i].materials[j].HasProperty("_Grow"))
                {
                    growRootMeshes[i].materials[j].SetFloat("_Grow", minGrow);
                    growMaterials.Add(growRootMeshes[i].materials[j]);
                }
            }
        }
    }


    private float time = 0;
    // Update is called once per frame
    void Update()
    {
        if (stop) { return; }
        StartCoroutine(Stop());
        for (int i = 0; i < growMaterials.Count; i++)
        {
            StartCoroutine(GrowRoots(growMaterials[i]));
        }

    }

    public IEnumerator Stop()
    {
        yield return new WaitForSeconds(1 * timeToGrow);
        stop = true;
    }




    public IEnumerator GrowRoots(Material mat)
    {
        float growValue = mat.GetFloat("_Grow");

        if (!fullyGrown)
        {
            while (growValue < maxGrow)
            {
                growValue += 1 / (timeToGrow / refreshhRate);
                mat.SetFloat("_Grow", growValue);

                yield return new WaitForSeconds(refreshhRate);
            }
        }
        else
        {
            while (growValue > minGrow)
            {
                growValue -= 1 / (timeToGrow / refreshhRate);
                mat.SetFloat("_Grow", growValue);

                yield return new WaitForSeconds(refreshhRate);
            }
        }

        if (growValue >= maxGrow)
        {
            fullyGrown = true;
        }
        else
        {
            fullyGrown = false;
        }
    }

}
