using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Jobs;
using Unity.Collections;

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

    [SerializeField]
    private List<List<ObjData>> batches;

    [HideInInspector]
    public Bounds BoundingBox;
    private void Start()
    {
        PlaceObjs();
    }

    private void Update()
    {
        Redraw();
    }

    public void Redraw()
    {
        if (batches != null)
        {
            foreach (var batch in batches)
            {
                Graphics.DrawMeshInstanced(prefabTemplate.GetComponentInChildren<MeshFilter>().sharedMesh, 0, prefabTemplate.GetComponentInChildren<MeshRenderer>().sharedMaterial, batch.Select((a) => a.matrix).ToList());
            }
        }
        
    }

    public void PlaceObjs()
    {
        if (batches == null)
        {
            Debug.Log("Making Array");
            batches = new List<List<ObjData>>();
        }

        if (batches.Count > 0)
        {
            Debug.Log("Clearing Array");
            batches.Clear();
        }

        Debug.Log("Randomising Positions...");
        List<Vector3> PointList = ReRoll();
        Debug.Log("Randomising Positions Done");

        PointList = RaycastPositions(PointList);

        
        List<ObjData> currBatch = new List<ObjData>();

        int batchIndexNum = 0;
        for (int i = 0; i < PointList.Count; i++)
        {

            Vector3 position = new Vector3(Random.Range(-(discDiameter / 2), (discDiameter / 2)), 0.0f, Random.Range(-(discDiameter / 2), (discDiameter / 2)));
            float randScale = Random.Range(minScale, maxScale);
            foreach (Transform item in prefabTemplate.transform)
            {

                //Apply prefabs positions AFTER raycast for offset
                currBatch.Add(new ObjData( PointList[i] + prefabTemplate.transform.position, item.localScale * randScale, item.rotation)); ;
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


        Redraw();
    }

    public List<Vector3> ReRoll()
    {
        List<Vector3> PointList = new List<Vector3>();

        float stepSizeX = transform.localScale.x / density;
        float stepSizeZ = transform.localScale.z / density;
        for (float i = -(density / 2); i < density / 2; i++) //have inital position been in center, rather than up and left
        {
            for (float j = -(density / 2); j < density / 2; j++)
            {
                //Get random point in cell.
                float randX = Random.Range(i * stepSizeX, (i + 1) * stepSizeX);
                float randZ = Random.Range(j * stepSizeZ, (j + 1) * stepSizeZ);

                Vector3 point = new Vector3(randX, transform.position.y, randZ);
                
                //Apply local position
                PointList.Add(transform.position  + point);

            }
        }

        return PointList;
    }
    public LayerMask mask = -1;
    public List<Vector3> RaycastPositions(List<Vector3> posList)
    {

        //This chunk doesn't work
        //It is a COPY PASTE of the docs
        //Nice job unity. Single threaded it is


        //// Perform a single raycast using RaycastCommand and wait for it to complete
        //// Setup the command and result buffers
        //var results = new NativeArray<RaycastHit>(posList.Count, Allocator.TempJob);
        //var commands = new NativeArray<RaycastCommand>(posList.Count, Allocator.TempJob);



        //Vector3 direction = Vector3.down;

        //for (int i = 0; i < posList.Count; i++)
        //{            
        //    commands[i] = new RaycastCommand(transform.position, Vector3.down, Mathf.Infinity, mask);
        //}

        //Debug.Log("Scheduling Raycasts...");
        //// Schedule the batch of raycasts
        //JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, commands.Length, default(JobHandle));

        //// Wait for the batch processing job to complete
        //handle.Complete();
        //Debug.Log("RayCasts Complete");

        List<Vector3> returnList = new List<Vector3>();

        //// Copy the result. If batchedHit.collider is null there was no hit
        //RaycastHit batchedHit = results[0];

        Debug.Log("Scheduling Raycasts...");
        for (int i = 0; i < posList.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(posList[i], Vector3.down, out hit, Mathf.Infinity))
            {
                returnList.Add(hit.point);
            }
        }
        Debug.Log("RayCasts Complete");

        return returnList;
    }

    private void OnDrawGizmos()
    {
       
    }
}
