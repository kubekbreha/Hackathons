namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;

#if UNITY_EDITOR
    using Input = InstantPreviewInput;
#endif

    public class HelloARController : MonoBehaviour
    {
        
        public Camera FirstPersonCamera;
        public GameObject DetectedPlanePrefab;
        public GameObject AndyPlanePrefab;
        public GameObject AndyPointPrefab;
        private const float k_ModelRotation = 180.0f;
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();
        private bool m_IsQuitting = false;
        //--------------------------------------------------------

        private TrackableHit hit;


        // Material for wall.
        public Material transparentMaterial;
        public Material visibleMaterial;

        //list of corners for one wall
        List<GameObject> listOfWallCorners = new List<GameObject>();
        List<GameObject> listOfWallCornersIndicators = new List<GameObject>();
        List<GameObject> listOfTokensPositions = new List<GameObject>();

        private Vector3 firstPositionV3 = Vector3.zero;
        private Vector3 lastPositionV3 = Vector3.zero;

        private Vector3 firstScale = Vector3.zero;
        private Vector3 lastScale = Vector3.zero;

        public GameObject prefabToken;
        public GameObject prefabMonster;
        GameObject monster;


        private bool InputAndroidPrefab = false;
        private bool InputToken = false;
        private bool sideWalls = false;
        private bool movingMonster = false;
        private int positionCounter = 0;

        //private float wallThickness = 0.05F;
        private float wallThickness = 0.05F;
        private float wallHeight = 30F;


        //--------------------------------------------------------
        public Rigidbody Point;
        public Transform hrac;
        Rigidbody RigidPrefab;

        Vector3 xMinusVector;
        Vector3 zPlusVector;
        Vector3 xPlusVector;
        Vector3 zMinusVector;
        Vector3 xMinusVectorChange;
        Vector3 randomV3;

        int row1 = 0;
        int row2 = 0;
        int i = 0;
        float randomX;
        float randomZ;

        private GameObject[] walls;

        bool collide;



        public void Update()
        {
            
            if(movingMonster == true){

                Vector3 target = listOfTokensPositions[positionCounter + 1].transform.position;
                    

                if (positionCounter > listOfTokensPositions.Count - 1)
                {
                    movingMonster = false;
                }

                float distance = Vector3.Distance(target, monster.transform.position);

                if (distance < 0.2F )
                    positionCounter++;

               
                var dir = (target - monster.transform.position).normalized;

                // Then add the direction * the speed to the current position:
                monster.transform.position += dir * 0.2F * Time.deltaTime;
                monster.transform.LookAt(target);
                monster.transform.Rotate(0, 90, 0);


            }



            _UpdateApplicationLifecycle();

            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    break;
                }
            }

            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Choose the Andy model for the Trackable that got hit.
                    GameObject prefab;
                    if (hit.Trackable is FeaturePoint)
                    {
                        prefab = AndyPointPrefab;
                    }
                    else
                    {
                        prefab = AndyPlanePrefab;
                    }

                    if(sideWalls == true){
                        wallThickness = 0.6F;
                    }

                    //if(InputAndroidPrefab == true)
                    //{
                    //    var andyObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);
                    //    andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                    //    var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                    //    andyObject.transform.parent = anchor.transform;
                    //    return;
                    //}


                    if (InputToken == true)
                    {
                        Debug.Log("add token");
                        var andyObject = Instantiate(prefabToken, new Vector3(hit.Pose.position.x, hit.Pose.position.y + 0.2F, hit.Pose.position.z), hit.Pose.rotation);
                        andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                        andyObject.transform.parent = anchor.transform;

                        listOfTokensPositions.Add(andyObject);

                        if(listOfTokensPositions.Count >= 15){
                            SpawnMonster();              
                        }

                        return;
                    }



                    if (listOfWallCorners.Count == 0)
                    {
                        GameObject firstPosition = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        firstPosition.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Pose.position.z);
                        firstPosition.transform.localScale = new Vector3(wallThickness, wallHeight, wallThickness);
                        firstPosition.GetComponent<Renderer>().material = transparentMaterial;
                        listOfWallCorners.Add(firstPosition);
                        firstPosition.gameObject.tag = "Wall";


                        GameObject firstPositionInd = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        firstPositionInd.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y + 0.05F, hit.Pose.position.z);
                        firstPositionInd.transform.localScale = new Vector3(wallThickness, 0.1F, wallThickness);
                        firstPositionInd.GetComponent<Renderer>().material = visibleMaterial;
                        listOfWallCornersIndicators.Add(firstPositionInd);


                        firstPositionV3 = firstPosition.transform.position;
                        firstScale = firstPosition.transform.localScale;
                        return;
                    }

                    if (listOfWallCorners.Count != 0)
                    {
                        GameObject secondPosition = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        secondPosition.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Pose.position.z);
                        secondPosition.transform.localScale = new Vector3(wallThickness, wallHeight, wallThickness);
                        secondPosition.GetComponent<Renderer>().material = transparentMaterial;
                        listOfWallCorners.Add(secondPosition);
                        secondPosition.gameObject.tag = "Wall";

                        GameObject secondPositionInd = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        secondPositionInd.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y + 0.05F, hit.Pose.position.z);
                        secondPositionInd.transform.localScale = new Vector3(wallThickness, 0.1F, wallThickness);
                        secondPositionInd.GetComponent<Renderer>().material = visibleMaterial;
                        listOfWallCornersIndicators.Add(secondPositionInd);


                        GenerateWall(listOfWallCorners[listOfWallCorners.Count - 2], listOfWallCorners[listOfWallCorners.Count - 1]);
                        GenerateWall(listOfWallCornersIndicators[listOfWallCornersIndicators.Count - 2], listOfWallCornersIndicators[listOfWallCornersIndicators.Count - 1]);

                       

                        GameObject thirdPosition = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        thirdPosition.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Pose.position.z);
                        thirdPosition.transform.localScale = new Vector3(wallThickness, wallHeight, wallThickness);
                        thirdPosition.GetComponent<Renderer>().material = transparentMaterial;
                        listOfWallCorners.Add(thirdPosition);
                        thirdPosition.gameObject.tag = "Wall";

                        GameObject thirdPositionInd = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        thirdPositionInd.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y + 0.05F, hit.Pose.position.z);
                        thirdPositionInd.transform.localScale = new Vector3(wallThickness, 0.1F, wallThickness);
                        thirdPositionInd.GetComponent<Renderer>().material = visibleMaterial;
                        listOfWallCornersIndicators.Add(thirdPositionInd);

                        lastPositionV3 = thirdPosition.transform.position;
                        lastScale = thirdPosition.transform.localScale;

                    } 
                }
            }
        }


        private void SpawnMonster(){
            Debug.Log("spawn monster");

            monster = Instantiate(prefabMonster, listOfTokensPositions[0].transform.position, hit.Pose.rotation);
            monster.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
            monster.transform.parent = anchor.transform;

            movingMonster = true;
        }


        public void ConnectFirstAndLast(){
            if(listOfWallCorners.Count == 0){
                return;
            }

            Debug.Log("In ConnectFirstAndLast()");
            GameObject first = GameObject.CreatePrimitive(PrimitiveType.Cube);
            first.transform.position = firstPositionV3;
            first.transform.localScale = firstScale;
            first.GetComponent<Renderer>().material = transparentMaterial;
            listOfWallCorners.Add(first);
            first.gameObject.tag = "Wall";

            GameObject last = GameObject.CreatePrimitive(PrimitiveType.Cube);
            last.transform.position = lastPositionV3;
            last.transform.localScale = lastScale;
            last.GetComponent<Renderer>().material = transparentMaterial;
            listOfWallCorners.Add(last);
            last.gameObject.tag = "Wall";



            GameObject firstInd = GameObject.CreatePrimitive(PrimitiveType.Cube);
            firstInd.transform.position = new Vector3(firstPositionV3.x, firstPositionV3.y, firstPositionV3.z);
            firstInd.transform.localScale = new Vector3(firstScale.x,0.1F,firstScale.z);
            firstInd.GetComponent<Renderer>().material = visibleMaterial;
            listOfWallCornersIndicators.Add(firstInd);

            GameObject lastInd = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lastInd.transform.position = new Vector3(lastPositionV3.x, lastPositionV3.y, lastPositionV3.z);
            lastInd.transform.localScale = new Vector3(lastScale.x, 0.1F, lastScale.z);;
            lastInd.GetComponent<Renderer>().material = visibleMaterial;
            listOfWallCornersIndicators.Add(lastInd);


            GenerateWall(first, last);
            GenerateWall(firstInd, lastInd);


            firstPositionV3 = Vector3.zero;
            lastPositionV3 = Vector3.zero;
            CleanWallsList();
        }


        private void GenerateWall(GameObject begin, GameObject end){
            begin.transform.LookAt(end.transform.position);
            end.transform.LookAt(begin.transform.position);
            float distance = Vector3.Distance(begin.transform.position, end.transform.position);
            end.transform.position = end.transform.position + distance / 2 * end.transform.forward;
            end.transform.rotation = end.transform.rotation;
            end.transform.localScale = new Vector3(end.transform.localScale.x, end.transform.localScale.y, distance);
        }

        public void CleanWallsList()
        {
            Debug.Log("In CleanWallsList()");
            listOfWallCorners.Clear();
            listOfWallCornersIndicators.Clear();
        }

        public void ChangeInput()
        {
            if(InputAndroidPrefab == true){
                InputAndroidPrefab = false;
            }else{
                InputAndroidPrefab = true;
            }
            Debug.Log("InputAndroidPrefab -> " + InputAndroidPrefab);
        }


        public void ChangeInputToken()
        {
            if (InputToken == true)
            {
                InputToken = false;
            }
            else
            {
                InputToken = true;
            }
            Debug.Log("InputToken -> " + InputToken);
        }



        public void CallStart()
        {










            //xMinusVector = new Vector3(-0.25f, 0.0f, 0.0f);
            //zPlusVector = new Vector3(0.0f, 0.0f, 0.25f);
            //xPlusVector = new Vector3(0.25f, 0.0f, 0.0f);
            //zMinusVector = new Vector3(0.0f, 0.0f, -0.25f);

            //walls = GameObject.FindGameObjectsWithTag("Wall");

            //Debug.Log(hrac);
            //Vector3 startPosition = new Vector3(hrac.transform.position.x, hit.Pose.position.y, hrac.transform.position.z);


            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = startPosition;
            //cube.transform.localScale = new Vector3(0.01F, 1F, 0.01F);


            //Debug.Log(startPosition);
            //renderPoints(startPosition);
        }

        public void renderPoints(Vector3 startPos)
        {
            collide = false;
            int x = 0;
            Vector3 startPosition = startPos;

            collide = blockedFront(startPosition + zPlusVector);
           
            if (collide == false)
            {
                RigidPrefab = Instantiate(Point, startPosition + zPlusVector, Point.rotation) as Rigidbody;
                RigidPrefab.tag = "Point";
                renderPoints(startPosition + zPlusVector);

            }

            collide = false;

            collide = blockedFront(startPosition + xMinusVector);
            if (collide == false)
            {
                RigidPrefab = Instantiate(Point, startPosition + xMinusVector, Point.rotation) as Rigidbody;
                RigidPrefab.tag = "Point";
                renderPoints(startPosition + xMinusVector);

            }

            collide = false;

            collide = blockedFront(startPosition + zMinusVector);
            if (collide == false)
            {
                RigidPrefab = Instantiate(Point, startPosition + zMinusVector, Point.rotation) as Rigidbody;
                RigidPrefab.tag = "Point";
                renderPoints(startPosition + zMinusVector);

            }

            collide = false;

            collide = blockedFront(startPosition + xPlusVector);
            if (collide == false)
            {
                RigidPrefab = Instantiate(Point, startPosition + xPlusVector, Point.rotation) as Rigidbody;
                RigidPrefab.tag = "Point";
                renderPoints(startPosition + xPlusVector);
            }
        }


        public bool blockedFront(Vector3 vector)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                Collider helpCol = walls[i].GetComponent<Collider>();
                if (helpCol.ClosestPointOnBounds(vector) == vector)
                {
                    Debug.Log("Blocked");
                    return true;
                }
            }

            GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
            for (int j = 0; j < points.Length; j++)
            {
                Collider helpCol2 = points[j].GetComponent<Collider>();
                if (helpCol2.ClosestPointOnBounds(vector) == vector)
                {
                    Debug.Log("Blocked");
                    return true;
                }

            }

            Debug.Log("NOT - Blocked");
            return false;
        }



        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                Debug.Log("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                Debug.Log("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

  
        private void _DoQuit()
        {
            Application.Quit();
        }

    
    }
}
