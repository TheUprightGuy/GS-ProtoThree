using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ObjData
{

    public Vector3 pos;

    public Vector3 scale;

    public Quaternion rot;

   
    public Matrix4x4 matrix

    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);
        }
    }

    public ObjData(Vector3 pos, Vector3 scale, Quaternion rot)
    {
        this.pos = pos;
        this.scale = scale;
        this.rot = rot;
    }

}
    [ExecuteAlways]
    public class ObjDistribuition : MonoBehaviour
    {

    public float minScale = 1.0f;
    public float maxScale = 1.0f;

    //Defines size of radius
    public float discDiameter = 5;
    public float density = 10;

    public GameObject prefabTemplate;

    private List<List<ObjData>> batches = new List<List<ObjData>>();

    private void Start()
    {
        PlaceObjs(transform.position);
    }
    private void Update()
    {
        Redraw();
    }

    private void Redraw()
    {
        foreach (var batch in batches)
        {
            Graphics.DrawMeshInstanced(prefabTemplate.GetComponentInChildren<MeshFilter>().sharedMesh, 0, prefabTemplate.GetComponentInChildren<MeshRenderer>().sharedMaterial, batch.Select((a) => a.matrix).ToList());
        }
    }


    public void PlaceObjs(Vector3 pos)
    {
        List<Vector3> PointList = ReRoll();

        int batchIndexNum = 0;

        List<ObjData> currBatch = new List<ObjData>();

        for (int i = 0; i < PointList.Count; i++)
        {

            Vector3 position = new Vector3(Random.Range(-(discDiameter / 2), (discDiameter / 2)), 0.0f, Random.Range(-(discDiameter / 2), (discDiameter / 2)));
            float randScale = Random.Range(minScale, maxScale);
            foreach (Transform item in prefabTemplate.transform)
            {

                currBatch.Add(new ObjData(pos + PointList[i], item.localScale * randScale, item.rotation));
                batchIndexNum++;
            }

            if (batchIndexNum >= 1000)
            {
                batches.Add(currBatch);
                currBatch = new List<ObjData>();
                batchIndexNum = 0;
            }

        }
        if (batchIndexNum < 1000)
        {
            batches.Add(currBatch);
        }

    }

    public List<Vector3> ReRoll()
    {
        List<Vector3> PointList = new List<Vector3>();

        float stepSize = discDiameter / density;
        for (float i = -(density / 2); i < density / 2; i++)
        {
            for (float j = -(density / 2); j < density / 2; j++)
            {
                //Get random point in cell.
                float randX = Random.Range(i * stepSize, (i + 1) * stepSize);
                float randZ = Random.Range(j * stepSize, (j + 1) * stepSize);

                Vector3 point = new Vector3(randX, 0.0f, randZ);
                PointList.Add(point);

                ////Restrict to within circle
                //if (Vector3.Distance(point, Vector3.zero) < discDiameter)
                //{
                //    PointList.Add(point);
                //}
            }
        }

        return PointList;
    }


    private void OnDrawGizmos()
    {
        foreach (var batch in batches)
        {
            foreach (var item in batch)
            {
                Gizmos.DrawSphere(item.pos, 1.0f);
            }
        }
    }
}
