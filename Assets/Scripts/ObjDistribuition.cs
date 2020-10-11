using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.UI;

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
    public float density = 10;

    public float CellSize = 5.0f;

    //public LayerMask GrassMask;
    public string GrassExclusionTag;

    public LayerMask RayCastToHit;
    public GameObject prefabTemplate;

    
    [SerializeField]
    private List<List<ObjData>> batches;

    private List<Mesh> MeshCells;

    [HideInInspector]
    public Bounds BoundingBox;
    private void Start()
    {
        PlaceObjMesh();
    }

    private void Update()
    {
        RedrawMesh();
    }

    public void RedrawGPUI()
    {
        if (batches != null)
        {
            foreach (var batch in batches)
            {
                Graphics.DrawMeshInstanced(prefabTemplate.GetComponentInChildren<MeshFilter>().sharedMesh, 0, prefabTemplate.GetComponentInChildren<MeshRenderer>().sharedMaterial, batch.Select((a) => a.matrix).ToList());
            }
        }
        
    }

    public void RedrawMesh()
    {
        foreach (var item in MeshCells)
        {
            Graphics.DrawMesh(item, Vector3.zero, Quaternion.identity, prefabTemplate.GetComponentInChildren<MeshRenderer>().sharedMaterial, 0);
        }
    }

    public void PlaceObjMesh()
    {
        if (MeshCells == null)
        {
            Debug.Log("Making Array");
            MeshCells = new List<Mesh>();
        }

        if (MeshCells.Count > 0)
        {
            Debug.Log("Clearing Array");
            MeshCells.Clear();
        }

        List<List<CombineInstance>> MeshCellInstances = new List<List<CombineInstance>>();

        int numOfColumns = Mathf.CeilToInt(transform.localScale.x / CellSize);
        int numOfRows = Mathf.CeilToInt(transform.localScale.z / CellSize);
        Vector2 startPos = new Vector2(transform.position.x - (transform.localScale.x / 2), transform.position.z - (transform.localScale.z / 2));

        for (int i = 0; i < numOfColumns * numOfRows; i++)
        {
            List<CombineInstance> temp = new List<CombineInstance>();
            MeshCellInstances.Add(temp);
        }

        
        Debug.Log("Randomising Positions...");
        List<Vector3> PointList = ReRoll();
        Debug.Log("Randomising Positions Done");

        PointList = RaycastPositions(PointList);

        Debug.Log("Batching Mesh...");
        for (int i = 0; i < PointList.Count; i++)
        {
            float xLocal = PointList[i].x - startPos.x;
            int col = Mathf.FloorToInt(xLocal / CellSize);

            float yLocal = PointList[i].z - startPos.y;
            int row = Mathf.FloorToInt(yLocal / CellSize);

            int iIndex = row * numOfColumns + col;

            float randScale = Random.Range(minScale, maxScale);
            foreach (Transform item in prefabTemplate.transform)
            {
                CombineInstance newInstance = new CombineInstance();
                newInstance.mesh = item.GetComponent<MeshFilter>().sharedMesh;
                newInstance.transform = Matrix4x4.TRS(PointList[i] + prefabTemplate.transform.position + item.transform.position, item.rotation, item.localScale * randScale);
                MeshCellInstances[iIndex].Add(newInstance);
            }
        }
        Debug.Log("Batching Mesh Done");

        foreach (var item in MeshCellInstances)
        {
            Mesh newMesh = new Mesh();
            newMesh.CombineMeshes(item.ToArray());
            MeshCells.Add(newMesh);
        }
    }

    public void PlaceObjsGPUI()
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


        RedrawGPUI();
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
            if (Physics.Raycast(posList[i], Vector3.down, out hit, Mathf.Infinity, RayCastToHit.value))
            {
                if (hit.collider.gameObject.tag != GrassExclusionTag)
                {
                    returnList.Add(hit.point);
                }
            }
        }
        Debug.Log("RayCasts Complete");

        return returnList;
    }

    private void OnDrawGizmos()
    {
       
    }
}
