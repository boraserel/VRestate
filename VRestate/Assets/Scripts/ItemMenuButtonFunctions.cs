using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMenuButtonFunctions : MonoBehaviour
{
    public GameObject BathroomMenuPanel;
    public GameObject KitchenMenuPanel;
    public GameObject ConstructionMenuPanel;
    public GameObject ItemMenuPanel;

    public SizeMenuFunctions sizeMenu;
    public GameObject settingsMenu;
    public MeshRenderer rend;
    public Material material;
    bool isInteractionOpen = false;

    public  static string clickedButtonName = "";
    public string leftClickedButtonName;

    [SerializeField] private Camera mainCamera;
    public GameObject cameraRig;
    Vector3 camerarigpos;
    Quaternion camerarigrotation;
    Vector3 cameraZoom;
    Vector3 camerapos;
    public float movementSpeed;
    public float movementTime;
    public float cameraRotationAmount;
     bool followmouse = true;
    //public Vector3 cameraZoomAmount;

    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;

    Vector3 rotateStartPosition;
    Vector3 rotateCurrentPosition;
    





    public float cameraScrollSpeed = 20f;
    
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    public float cameraMinY = 1.5f;
    public float cameraMaxY = 30f;
    public static PlaceableObject objectToPlace;

    
   

    public GameObject ObjectFollowsMouse;
    public static GameObject buildingSystemObjectFollowMouse;
    public GameObject Selected3DObject;
    public bool isButtonClicked = true;


    // Kitchen
    public GameObject CabinetBase1;
    public static Vector3 CabinetBase1Size; 
    public GameObject CabinetBase2;
    public static Vector3 CabinetBase2Size;
    public GameObject CabinetBaseCorner;
    public static Vector3 CabinetBaseCornerSize;
    public GameObject CabinetBaseSink;
    public static Vector3 CabinetBaseSinkSize;
    public GameObject CabinetTall;
    public static Vector3 CabinetTallSize;
    public GameObject CabinetWall1;
    public static Vector3 CabinetWall1Size;
    public GameObject CabinetWall2;
    public static Vector3 CabinetWall2Size;
    public GameObject Stove;
    public static Vector3 StoveSize;

    public bool cabinetBase1SizeSelected = false;
    public bool cabinetBase2SizeSelected = false;
    public bool cabinetBaseCornerSizeSelected = false;
    public bool cabinetBaseSinkSizeSelected = false;
    public bool cabinetTallSizeSelected = false;
    public bool cabinetWall1SizeSelected = false;
    public bool cabinetWall2SizeSelected = false;
    public bool stoveSizeSelected = false;

    //Construction
    public GameObject Floor;
    public static Vector3 FloorSize;


    public GameObject Wall;
    public static Vector3 WallSize;
    public GameObject Door1;
    //public static Vector3 Door1Size;
    public GameObject Door2;
    //public static Vector3 Door2Size;
    public GameObject Window;
    public GameObject Frame;
    
    public GameObject Rail;
    public static Vector3 RailSize;



    // Dynamic Wall Creation
    bool creatingWall = false;
    bool deleteAfterWallHold = false;
    GameObject startPole;
    GameObject endPole;
    GameObject wall;
    public GameObject wallPrefab;

    // Dynamic Wall Creation
    bool creatingFloor = false;
    bool deleteAfterFloorHold = false;
    GameObject startFloor;
    GameObject endFloor;
    GameObject floor;
    public GameObject floorPrefab;



    /*Vector3 prevMousePosition;
    public float sizingFactor = 0.5f;
    Vector3 minimumScale = new Vector3(0.2f, 1.0f, 0.2f);*/


    //Bathroom
    public GameObject Bathtub;
    public static Vector3 BathtubSize;
    public GameObject Shower;
    public static Vector3 ShowerSize;
    public GameObject Vanity3;
    public static Vector3 Vanity3Size;
    public GameObject Toilet;
    public static Vector3 ToiletSize;
    public GameObject Vanity1;
    public static Vector3 Vanity1Size;
    public bool foundcount = false;

    public bool bathtubSizeSelected = false;
    public bool showerSizeSelected = false;
    public bool vanity3SizeSelected = false;
    public bool toiletSizeSelected = false;
    public bool vanity1SizeSelected = false;











    public GameObject cube;
    public GameObject InteractionCanvas;
    //public int[] itemModelCount = new int[70];
    /* itemModelCount indexes:
     * 0 -> CabinetBase1
     * 1 -> CabinetBase2
     * 2 -> CabinetBaseCorner
     * 3 -> CabinetBaseSink
     * 4 -> CabinetTall
     * 5 -> CabinetWall1
     * 6 -> CabinetWall2
     * 7 -> CabinetBase1
     * 9 -> WallPrefab
     * 
     * 15 -> Stove
     * 20 -> Floor
     * 
     * 
     */
    
   
    Vector3 mousepos;
    public void Start()
    {
        camerarigpos = cameraRig.transform.position;
        camerarigrotation = cameraRig.transform.rotation;
        cameraZoom = mainCamera.transform.localPosition;
        camerapos = mainCamera.transform.localPosition;


        CabinetBase1.transform.localScale = new Vector3(1f, 1f, 1f);
        CabinetBase1Size = CabinetBase1.GetComponent<MeshRenderer>().bounds.size;
        CabinetBase2.transform.localScale = new Vector3(1f, 1f, 1f);
        CabinetBase2Size = CabinetBase2.GetComponent<MeshRenderer>().bounds.size;
        CabinetBaseCorner.transform.localScale = new Vector3(1f, 1f, 1f);
        CabinetBaseCornerSize = CabinetBaseCorner.GetComponent<MeshRenderer>().bounds.size;
        CabinetBaseSink.transform.localScale = new Vector3(1f, 1f, 1f);
        CabinetBaseSinkSize = CabinetBaseSink.GetComponent<MeshRenderer>().bounds.size;
        CabinetTall.transform.localScale = new Vector3(1f, 1f, 1f);
        CabinetTallSize = CabinetTall.GetComponent<MeshRenderer>().bounds.size;
        CabinetWall1.transform.localScale = new Vector3(1f, 1f, 1f);
        CabinetWall1Size = CabinetWall1.GetComponent<MeshRenderer>().bounds.size;
        CabinetWall2.transform.localScale = new Vector3(1f, 1f, 1f);
        CabinetWall2Size = CabinetWall2.GetComponent<MeshRenderer>().bounds.size;

        Stove.transform.localScale = new Vector3(1f, 1f, 1f);
        StoveSize = Stove.GetComponent<MeshRenderer>().bounds.size;

        Floor.transform.localScale = new Vector3(0.2f, 0.1f, 0.2f);
        FloorSize = Floor.GetComponent<MeshRenderer>().bounds.size;


        Wall.transform.localScale = new Vector3(1f, 1f, 1f);
        WallSize = Wall.GetComponent<MeshRenderer>().bounds.size;

        Bathtub.transform.localScale = new Vector3(1f, 1f, 1f);
        BathtubSize = Bathtub.GetComponent<MeshRenderer>().bounds.size;
        Shower.transform.localScale = new Vector3(1f, 1f, 1f);
        ShowerSize = Shower.GetComponent<MeshRenderer>().bounds.size;
        Vanity3.transform.localScale = new Vector3(100f, 100f, 100f);
        Vanity3Size = Vanity3.GetComponent<MeshRenderer>().bounds.size;
        Toilet.transform.localScale = new Vector3(1f, 1f, 1f);
        ToiletSize = Toilet.GetComponent<MeshRenderer>().bounds.size;
        Vanity1.transform.localScale = new Vector3(1f, 1f, 1f);
        Vanity1Size = Vanity1.GetComponent<MeshRenderer>().bounds.size;


        Door1.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
        Door2.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
        Window.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
        Frame.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
        Rail.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
        //RailSize = Rail.GetComponent<MeshRenderer>().bounds.size;
    }
    //Ana kategoriden Bathroom butonuna t�kland���nda
    public void BathroomButtonClicked()
    {
        
        BathroomMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-660f, 127f);
        
        ItemMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(1500f, 127f);
    }

    //Ana kategoriden Kitchen butonuna t�kland���nda
    public void KitchenButtonClicked()
    {

        KitchenMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-660f, 127f);

        ItemMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(3660f, 127f);
    }

    //Ana kategoriden Construction butonuna t�kland���nda
    public void ConstructionButtonClicked()
    {

        ConstructionMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-660f, 127f);

        ItemMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-2820f, 127f);
    }

    //Bathroom alt kategoriden Back butonuna t�kland���nda
    public void BathroomBackButtonClicked()
    {
       
        BathroomMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(1500f, 127f);

        ItemMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-660f, 127f);
    }

    //Kitchen alt kategoriden Back butonuna t�kland���nda
    public void KitchenBackButtonClicked()
    {

        KitchenMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(3660f, 127f);

        ItemMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-660f, 127f);
    }

    //Construction alt kategoriden Back butonuna t�kland���nda
    public void ConstructionBackButtonClicked()
    {

        ConstructionMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-2820f, 127f);

        ItemMenuPanel.GetComponent<RectTransform>().localPosition = new Vector3(-660f, 127f);
    }


    //buton ismine g�re bu metottan prefab olu�tur
    public void ItemMenuButtonsLeftClicked() 
    {
        leftClickedButtonName = EventSystem.current.currentSelectedGameObject.name;
        /*for (int i = 0; i < itemModelCount.Length; i++)
        {
            itemModelCount[i] = 0;
        }*/
        foundcount = false;
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");

        if (leftClickedButtonName == "Cabinet_Base_1" && cabinetBase1SizeSelected == true)
        {
            if (isButtonClicked == true)
            {                
                Destroy(ObjectFollowsMouse);               
            }
            if(isButtonClicked == false)
            {
                isButtonClicked =true;
            }

            //ObjectFollowsMouse = Instantiate(CabinetBase1, new Vector3(4f, 2f, -18f), Quaternion.Euler( new Vector3(0, 90, 0)));
            ObjectFollowsMouse = Instantiate(CabinetBase1, new Vector3(4f, 2f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();

            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[0]= 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Cabinet_Base_2" && cabinetBase2SizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(CabinetBase2, new Vector3(4f, 2f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();

            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[1] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Cabinet_Base_Corner" && cabinetBaseCornerSizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(CabinetBaseCorner, new Vector3(4f, 2f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[2] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Cabinet_Base_Sink" && cabinetBaseSinkSizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(CabinetBaseSink, new Vector3(4f, 2f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[3] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Cabinet_Tall" && cabinetTallSizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(CabinetTall, new Vector3(4f, 2f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[4] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Cabinet_Wall_1" && cabinetWall1SizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(CabinetWall1, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[5] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Cabinet_Wall_2" && cabinetWall2SizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(CabinetWall2, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[6] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Wall")
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(wallPrefab, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[9] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Stove" && stoveSizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Stove, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[15] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Floor")
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Floor, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[20] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Bathtub" && bathtubSizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Bathtub, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[30] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Shower" && showerSizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Shower, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[31] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Sink")
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Vanity3, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[32] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Toilet" && toiletSizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Toilet, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[33] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Vanity_1" && vanity1SizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Vanity1, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[34] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Vanity_3" && vanity3SizeSelected == true)
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Vanity3, new Vector3(4f, 10f, -18f), Quaternion.Euler(new Vector3(-90, 180, 0)));
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[35] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Door_1")
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Door1, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[36] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
            {
                ObjectFollowsMouse.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().isTrigger = true;
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size = ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);

            }
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Door_2")
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Door2, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[37] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
            {
                ObjectFollowsMouse.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().isTrigger = true;
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size = ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);

            }
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Window")
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Window, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[38] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
            {
                ObjectFollowsMouse.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().isTrigger = true;
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size = ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);

            }
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;

        }
        else if (leftClickedButtonName == "Frame")
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Frame, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[39] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
            {
                ObjectFollowsMouse.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().isTrigger = true;
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size = ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);

            }
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }
        else if (leftClickedButtonName == "Rail")
        {
            if (isButtonClicked == true)
            {
                Destroy(ObjectFollowsMouse);
            }
            if (isButtonClicked == false)
            {
                isButtonClicked = true;
            }

            ObjectFollowsMouse = Instantiate(Rail, new Vector3(4f, 10f, -18f), Quaternion.identity);
            objectToPlace = ObjectFollowsMouse.GetComponent<PlaceableObject>();
            ObjectFollowsMouse.AddComponent<ObjectDrag>();
            ObjectFollowsMouse.layer = LayerIgnoreRaycast;
            //itemModelCount[40] = 1;
            foundcount = true;
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
            {
                ObjectFollowsMouse.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().isTrigger = true;
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size = ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);

            }
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
            buildingSystemObjectFollowMouse = ObjectFollowsMouse;
        }


        /// BURADAK� KODLARI (LAYER + (IF ELSE'�N ���N�) + BUILDINGSYSTEMOBJECTFOLLOWMOUSE) �LK BA�TA SIZE G�RME ZORUNLULU�U ���N HER B�R LEFTCLICKEDBUTTONNAME METHODUNUN ���NE EKLED�M
        
        //int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        //ObjectFollowsMouse.layer = LayerIgnoreRaycast;
        /*if (ObjectFollowsMouse.name == "NewDoor1(Clone)" || ObjectFollowsMouse.name == "NewDoor2(Clone)" || ObjectFollowsMouse.name == "NewWindow2(Clone)" || ObjectFollowsMouse.name == "Frame(Clone)" || ObjectFollowsMouse.name == "Rail(Clone)")
        {
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
            {
                ObjectFollowsMouse.transform.GetChild(i).gameObject.AddComponent<BoxCollider>() ;
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().isTrigger = true;
                ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size = ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);

            }
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;

        }
        else
        {
            ObjectFollowsMouse.AddComponent<BoxCollider>();
            ObjectFollowsMouse.GetComponent<BoxCollider>().size = ObjectFollowsMouse.GetComponent<BoxCollider>().size - new Vector3(0.1f, 0.1f, 0.1f);
            ObjectFollowsMouse.GetComponent<BoxCollider>().isTrigger = true;
        }*/
       

      
        //buildingSystemObjectFollowMouse = ObjectFollowsMouse;



    }
  
    public void ItemMenuButtonsRightClicked()
    {
        //CabinetBase1.transform.localScale = new Vector3(5f, 5f, 5f);
        //CabinetBase1.transform.localScale = new Vector3(1f, 1f, 1f);
        //clickedButtonName = EventSystem.current.currentSelectedGameObject.name;

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            int indexofbutton = 0;
            foreach (var go in raycastResults)
            {
                if (indexofbutton == 1)
                {
                     clickedButtonName = go.gameObject.name;
                }
                //Debug.Log(go.gameObject.name, go.gameObject);
                indexofbutton++;
            }
        }
        //clickedButtonName = "Cabinet_Base_2";
        Debug.Log(clickedButtonName);



    }


    // Dynamic Wall Creation metotlar�
    ///////////////////////////////////////////////////////////////////////////////////////////////
    void SetStart()
    {
        creatingWall = true;
        deleteAfterWallHold = false;
        followmouse = false;
        startPole = Instantiate(wallPrefab, new Vector3(4f, 10f, -18f), ObjectFollowsMouse.transform.rotation);
        endPole = Instantiate(wallPrefab, new Vector3(1000f, 10f, 1000f), startPole.transform.rotation);
        wall = (GameObject)Instantiate(wallPrefab, new Vector3(1000f, 10f, 1000f), Quaternion.identity);
        startPole.AddComponent<BoxCollider>();
        endPole.AddComponent<BoxCollider>();
        wall.AddComponent<BoxCollider>();

        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        startPole.layer = LayerIgnoreRaycast;
        endPole.layer = LayerIgnoreRaycast;
        wall.layer = LayerIgnoreRaycast;

        ObjectFollowsMouse.transform.position = new Vector3(1000, 1000, 1000);

        //Vector3 mousepos = getWorldPoint();
        //startPole.transform.position = new Vector3(mousepos.x, mousepos.y + wallPrefab.transform.localScale.y / 2, mousepos.z);

        RaycastHit pos = getWorldPoint();

        BoxCollider b = startPole.GetComponent<BoxCollider>();

        Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(pos);
        //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
        Vector3 objectposition = new Vector3(a.x + 0.25f, a.y + wallPrefab.transform.localScale.y / 2, a.z + 0.25f);
        startPole.transform.position = objectposition;
        
        //startPole.transform.position = new Vector3(mousepos.x, mousepos.y, mousepos.z);

        //Destroy(ObjectFollowsMouse);

    }
    void setEnd()
    {
        creatingWall = false;

        // Vector3 mousepos = getWorldPoint();
        // endPole.transform.position = new Vector3(mousepos.x, mousepos.y + wallPrefab.transform.localScale.y / 2, mousepos.z);

        RaycastHit pos = getWorldPoint();

        BoxCollider b = endPole.GetComponent<BoxCollider>();

        Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(pos);
        //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
        Vector3 objectposition = new Vector3(a.x + 0.25f, a.y + wallPrefab.transform.localScale.y / 2, a.z + 0.25f);
        endPole.transform.position = objectposition;
        startPole.AddComponent<Rigidbody>();
        startPole.GetComponent<Rigidbody>().isKinematic = true;
        endPole.AddComponent<Rigidbody>();
        endPole.GetComponent<Rigidbody>().isKinematic = true;
        wall.AddComponent<Rigidbody>();
        wall.GetComponent<Rigidbody>().isKinematic = true;



        //endPole.transform.position = new Vector3(mousepos.x, mousepos.y, mousepos.z);

        /*for (int i = 0; i < itemModelCount.Length; i++)
        {
            itemModelCount[i] = 0;
        }*/
        foundcount = false;
        Destroy(ObjectFollowsMouse);
        followmouse = true;
       
        int LayerDefault = LayerMask.NameToLayer("Default");
        startPole.layer = LayerDefault;
        endPole.layer = LayerDefault;
        wall.layer = LayerDefault;



    }
    void adjust()
    {
        //Vector3 mousepos = getWorldPoint();
        //endPole.transform.position = new Vector3(mousepos.x, mousepos.y + wallPrefab.transform.localScale.y / 2, mousepos.z);

        RaycastHit pos = getWorldPoint();

        BoxCollider b = endPole.GetComponent<BoxCollider>();

        Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(pos);
        //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
        Vector3 objectposition = new Vector3(a.x + 0.25f, a.y + wallPrefab.transform.localScale.y / 2, a.z + 0.25f);
        endPole.transform.position = objectposition;



        // endPole.transform.position = new Vector3(mousepos.x, mousepos.y, mousepos.z);
        startPole.transform.LookAt(endPole.transform.position);
        endPole.transform.LookAt(startPole.transform.position);
        //endPole.transform.rotation = new Quaternion(startPole.transform.rotation.x, startPole.transform.rotation.y, startPole.transform.rotation.z, startPole.transform.rotation.w);
       
        float distance = Vector3.Distance(startPole.transform.position, endPole.transform.position);
        //Debug.Log(distance);
        wall.transform.position = startPole.transform.position + distance / 2 * startPole.transform.forward;
        wall.transform.rotation = startPole.transform.rotation;
        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, (2*distance-1f)*(startPole.transform.localScale.z));
        //wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, (2*distance-1)*startPole.transform.localScale.z);
    }

    ///////////////////////////////////////////////
    void SetStartFloor()
    {
        creatingFloor = true;
        deleteAfterFloorHold = false;
        followmouse = false;
        startFloor = Instantiate(floorPrefab, new Vector3(4f, 10f, -18f), ObjectFollowsMouse.transform.rotation);
        endFloor = Instantiate(floorPrefab, new Vector3(1000f, 10f, 1000f), startFloor.transform.rotation);
        floor = (GameObject)Instantiate(floorPrefab, new Vector3(1000f, 10f, 1000f), Quaternion.identity);
        startFloor.AddComponent<BoxCollider>();
        endFloor.AddComponent<BoxCollider>();
        floor.AddComponent<BoxCollider>();

        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        startFloor.layer = LayerIgnoreRaycast;
        endFloor.layer = LayerIgnoreRaycast;
        floor.layer = LayerIgnoreRaycast;

        ObjectFollowsMouse.transform.position = new Vector3(1000, 1000, 1000);

        //Vector3 mousepos = getWorldPoint();
        //startPole.transform.position = new Vector3(mousepos.x, mousepos.y + wallPrefab.transform.localScale.y / 2, mousepos.z);

        RaycastHit pos = getWorldPoint();

        BoxCollider b = startFloor.GetComponent<BoxCollider>();

        Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(pos);
        //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
        Vector3 objectposition = new Vector3(a.x + 0.25f, a.y , a.z + 0.25f);
        startFloor.transform.position = objectposition;

        //startPole.transform.position = new Vector3(mousepos.x, mousepos.y, mousepos.z);

        //Destroy(ObjectFollowsMouse);

    }
    void setEndFloor()
    {
        creatingFloor = false;

        // Vector3 mousepos = getWorldPoint();
        // endPole.transform.position = new Vector3(mousepos.x, mousepos.y + wallPrefab.transform.localScale.y / 2, mousepos.z);

        RaycastHit pos = getWorldPoint();

        BoxCollider b = endFloor.GetComponent<BoxCollider>();

        Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(pos);
        //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
        Vector3 objectposition = new Vector3(a.x + 0.25f, a.y , a.z + 0.25f);
        endFloor.transform.position = objectposition;
        startFloor.AddComponent<Rigidbody>();
        startFloor.GetComponent<Rigidbody>().isKinematic = true;
        endFloor.AddComponent<Rigidbody>();
        endFloor.GetComponent<Rigidbody>().isKinematic = true;
        floor.AddComponent<Rigidbody>();
        floor.GetComponent<Rigidbody>().isKinematic = true;



        //endPole.transform.position = new Vector3(mousepos.x, mousepos.y, mousepos.z);

        /*for (int i = 0; i < itemModelCount.Length; i++)
        {
            itemModelCount[i] = 0;
        }*/
        foundcount = false;
        Destroy(ObjectFollowsMouse);
        followmouse = true;

        int LayerDefault = LayerMask.NameToLayer("Default");
        startFloor.layer = LayerDefault;
        endFloor.layer = LayerDefault;
        floor.layer = LayerDefault;



    }
    void adjustFloor()
    {
        //Vector3 mousepos = getWorldPoint();
        //endPole.transform.position = new Vector3(mousepos.x, mousepos.y + wallPrefab.transform.localScale.y / 2, mousepos.z);

        RaycastHit pos = getWorldPoint();

        BoxCollider b = endFloor.GetComponent<BoxCollider>();

        Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(pos);
        //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
        Vector3 objectposition = new Vector3(a.x + 0.25f, a.y, a.z + 0.25f);
        endFloor.transform.position = objectposition;



        // endPole.transform.position = new Vector3(mousepos.x, mousepos.y, mousepos.z);
        startFloor.transform.LookAt(endFloor.transform.position);
        endFloor.transform.LookAt(startFloor.transform.position);
        //endPole.transform.rotation = new Quaternion(startPole.transform.rotation.x, startPole.transform.rotation.y, startPole.transform.rotation.z, startPole.transform.rotation.w);

        float distance = Vector3.Distance(startFloor.transform.position, endFloor.transform.position);
        //Debug.Log(distance);
        floor.transform.position = startFloor.transform.position + distance / 2 * startFloor.transform.forward;
        floor.transform.rotation = startFloor.transform.rotation;
        floor.transform.localScale = new Vector3(floor.transform.localScale.x, floor.transform.localScale.y, (2 * distance - 1f) * (startFloor.transform.localScale.z));
        //wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, (2*distance-1)*startPole.transform.localScale.z);
    }

    RaycastHit getWorldPoint()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            return hit;
        }
        return hit;
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////

    public void Update()
    {
        //Debug.Log(MouseInputUIBlocker.BlockedByUI + " BLOCKEDBYUI");

        
        /*bool foundcount = false;
        for (int i = 0; i < itemModelCount.Length; i++)
        {
            if (itemModelCount[i] == 1)
            {
                foundcount = true;
            }
        }*/
        //KAMERA HAREKET ETMES�
        if (Input.GetKey(KeyCode.UpArrow))
        {
            camerarigpos += (cameraRig.transform.forward * movementSpeed);
        }
            
        if (Input.GetKey(KeyCode.DownArrow))
        {
            camerarigpos += (cameraRig.transform.forward * -movementSpeed);
        }
               
        if (Input.GetKey(KeyCode.RightArrow))
        {
            camerarigpos += (cameraRig.transform.right * movementSpeed);
        }
                   
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            camerarigpos += (cameraRig.transform.right *- movementSpeed);
        }

        //KAMERA D�ND�RMES�
        if (Input.GetKey(KeyCode.Q))
        {
            camerarigrotation *= Quaternion.Euler(Vector3.up * -cameraRotationAmount);
            
        }
        if (Input.GetKey(KeyCode.E))
        {
            camerarigrotation *= Quaternion.Euler(Vector3.up * cameraRotationAmount);
            
        }

        //Mouse �zerinden hareket etme ve rotation yap�lmas�
        if (MouseInputUIBlocker.BlockedByUI == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                float entry;
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            if (Input.GetMouseButton(1))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                float entry;
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);
                    camerarigpos = cameraRig.transform.position + dragStartPosition - dragCurrentPosition;
                }
            }
            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;
                Vector3 difference = rotateStartPosition - rotateCurrentPosition;
                rotateStartPosition = rotateCurrentPosition;
                camerarigrotation *= Quaternion.Euler(Vector3.up * (-difference.x /5f));
            }
        }
        //KAMERA HAREKET ETMES� (MOUSE �LE)
        /*if (MouseInputUIBlocker.BlockedByUI == false)
        {
            if (Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                camerarigpos += (cameraRig.transform.forward * movementSpeed);
            }

            if (Input.mousePosition.y <= panBorderThickness)
            {
                camerarigpos += (cameraRig.transform.forward * -movementSpeed);
            }

            if (Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                camerarigpos += (cameraRig.transform.right * movementSpeed);
            }

            if (Input.mousePosition.x <= panBorderThickness)
            {
                camerarigpos += (cameraRig.transform.right * -movementSpeed);
            }
        }*/

        //KAMERANIN SINIRLARI
        camerarigpos.x = Mathf.Clamp(camerarigpos.x, -panLimit.x, panLimit.x);
        cameraZoom.y = Mathf.Clamp(cameraZoom.y, cameraMinY, cameraMaxY);
        camerarigpos.z = Mathf.Clamp(camerarigpos.z, -panLimit.y-30, panLimit.y-40);

        //cameraRig.transform.position = Vector3.Lerp(cameraRig.transform.position, camerarigpos, Time.deltaTime * movementTime);
        cameraRig.transform.position = camerarigpos;
        cameraRig.transform.rotation = Quaternion.Lerp(camerarigrotation, cameraRig.transform.rotation,  Time.deltaTime * movementTime );
        

        //SE��LEN OBJE OLU�UP MOUSEU TAK�P ED�YOR
        if (foundcount)
        {
            InteractionCanvas.SetActive(false);
          
           

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                //Debug.Log(raycastHit.point);
                if (ObjectFollowsMouse != null)
                {
                    if (ObjectFollowsMouse.name == "Cabinet_Wall_1(Clone)" || ObjectFollowsMouse.name == "Cabinet_Wall_2(Clone)")
                    {
                        if (followmouse == true)
                        {
                            //ObjectFollowsMouse.transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y + 1.5f, raycastHit.point.z);
                            RaycastHit raycastHitObject = raycastHit;

                            BoxCollider b = ObjectFollowsMouse.GetComponent<BoxCollider>();

                            Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(raycastHitObject);
                            //Debug.Log(a + " aaa");
                            //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
                            Vector3 objectposition = Vector3.zero;
                            float gridposx = 0;
                            float gridposy = 0;
                            float gridposz = 0;
                            if (ObjectFollowsMouse.name == "Cabinet_Wall_1(Clone)")
                            {
                                gridposx = sizeMenu.CabinetWall1x;
                                gridposy = sizeMenu.CabinetWall1y;
                                gridposz = sizeMenu.CabinetWall1z;
                            }
                            else if (ObjectFollowsMouse.name == "Cabinet_Wall_2(Clone)")
                            {
                                gridposx = sizeMenu.CabinetWall2x;
                                gridposy = sizeMenu.CabinetWall2y;
                                gridposz = sizeMenu.CabinetWall2z;
                            }
                            if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 0f)
                            {
                                //Debug.Log("1. if");
                                //hit.point = new Vector3(hit.point.x + 0.25f, hit.point.y, hit.point.z - 0.25f);
                                objectposition = new Vector3(a.x - 0.25f - 0.25f, a.y + 2.5f - gridposy / 200, a.z + gridposz / 200);
                                //Debug.Log(" abc " + objectposition);
                                //objectposition = new Vector3(a.x - b.size.x, a.y, a.z + b.size.z);
                                //Debug.Log(" abc " + objectposition);
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 90f)
                            {
                                //Debug.Log("2. if");
                                //hit.point = new Vector3(hit.point.x - 0.25f, hit.point.y, hit.point.z - 0.25f);
                                objectposition = new Vector3(a.x  + gridposz / 200, a.y + 2.5f - gridposy / 200, a.z + 0.25f + 0.25f);
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 180f)
                            {
                                //Debug.Log("3. if");
                                //hit.point = new Vector3(hit.point.x - 0.25f, hit.point.y, hit.point.z + 0.25f);
                                objectposition = new Vector3(a.x +0.25f + 0.25f, a.y + 2.5f - gridposy / 200, a.z - gridposz / 200 );
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 270f)
                            {
                                //Debug.Log("4. if");
                                //hit.point = new Vector3(hit.point.x + 0.25f, hit.point.y, hit.point.z + 0.25f);
                                objectposition = new Vector3(a.x  - gridposz / 200, a.y + 2.5f - gridposy / 200, a.z - 0.25f - 0.25f);
                            }
                            //Vector3 objectposition = new Vector3(a.x + BuildingSystem.posx / 200, a.y + 2f, a.z + BuildingSystem.posz / 200);
                            ObjectFollowsMouse.transform.position = objectposition;
                            if (ObjectFollowsMouse.GetComponent<PlaceableObject>().isColliding == true)
                            {
                                ObjectFollowsMouse.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                                for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
                                {
                                    rend = ObjectFollowsMouse.transform.GetChild(i).GetComponent<MeshRenderer>();
                                    for (int j = 0; j < rend.materials.Length; j++)
                                    {
                                        rend.materials[j].color = Color.red;
                                    }
                                }
                            }
                            else
                            {
                                ObjectFollowsMouse.transform.GetComponent<MeshRenderer>().material.color = Color.white;
                                for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
                                {
                                    rend = ObjectFollowsMouse.transform.GetChild(i).GetComponent<MeshRenderer>();
                                    for (int j = 0; j < rend.materials.Length; j++)
                                    {
                                        rend.materials[j].color = Color.white;
                                    }
                                }
                            }
                        }
                        

                    }
                    else if ( ObjectFollowsMouse.name == "CubeWall(Clone)" || ObjectFollowsMouse.name == "NewDoor1(Clone)" || ObjectFollowsMouse.name == "NewDoor2(Clone)" || ObjectFollowsMouse.name == "NewWindow2(Clone)" || ObjectFollowsMouse.name == "Floor(Clone)" || ObjectFollowsMouse.name == "Frame(Clone)" || ObjectFollowsMouse.name == "Rail(Clone)")
                    {
                        if (followmouse == true)
                        {
                            //bir �nceki buydu
                            //ObjectFollowsMouse.transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y+ wallPrefab.transform.localScale.y /2, raycastHit.point.z);
                            //Vector3 pos = raycastHit.point;
                            RaycastHit raycastHitObject = raycastHit;
                            BoxCollider b = ObjectFollowsMouse.GetComponent<BoxCollider>();
                            
                            Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(raycastHitObject);
                            //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
                            Vector3 objectposition = Vector3.zero;
                            if (ObjectFollowsMouse.name == "CubeWall(Clone)")
                             {
                                
                                objectposition = new Vector3(a.x + 0.25f, a.y + wallPrefab.transform.localScale.y / 2, a.z + 0.25f);
                            }
                            if (ObjectFollowsMouse.name == "NewDoor1(Clone)")
                            {
                                 objectposition = new Vector3(a.x + 0.25f, a.y + wallPrefab.transform.localScale.y / 2, a.z + 0.25f);
                            }
                            if (ObjectFollowsMouse.name == "NewDoor2(Clone)")
                            {
                                 objectposition = new Vector3(a.x + 0.25f, a.y + wallPrefab.transform.localScale.y / 2, a.z + 0.25f);
                            }
                            if (ObjectFollowsMouse.name == "NewWindow2(Clone)")
                            {
                                 objectposition = new Vector3(a.x + 0.25f, a.y + wallPrefab.transform.localScale.y / 2, a.z + 0.25f);
                            }                          
                            if (ObjectFollowsMouse.name == "Floor(Clone)")
                            {
                                objectposition = new Vector3(a.x + 0.25f, a.y, a.z + 0.25f);
                            }
                            if (ObjectFollowsMouse.name == "Frame(Clone)")
                            {
                                objectposition = new Vector3(a.x + 0.25f, a.y + wallPrefab.transform.localScale.y / 2, a.z + 0.25f);
                            }
                            if (ObjectFollowsMouse.name == "Rail(Clone)")
                            {
                                objectposition = new Vector3(a.x + 0.25f, a.y + 0.5f, a.z + 0.25f);
                            }
                            ObjectFollowsMouse.transform.position = objectposition;



                            //ObjectFollowsMouse.transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z);
                        }
                    }
                    else
                    {
                        if (followmouse == true)
                        {
                            //ObjectFollowsMouse.transform.position = raycastHit.point;
                            RaycastHit raycastHitObject = raycastHit;

                            BoxCollider b = ObjectFollowsMouse.GetComponent<BoxCollider>();

                            Vector3 a = BuildingSystem.current.SnapCoordinateToGrid(raycastHitObject);
                            //Debug.Log(a + " aaa");
                            //Debug.Log(b.size.x + " " + b.size.z + " SIZEEEE");
                            Vector3 objectposition= Vector3.zero;
                            //Debug.Log(ObjectFollowsMouse.transform.eulerAngles.y + " obje rotation y");
                            float gridposx = 0;
                            float gridposy = 0;
                            float gridposz = 0;
                            if (ObjectFollowsMouse.name == "Cabinet_Base_1(Clone)")
                            {                               
                                 gridposx = sizeMenu.CabinetBase1x;
                                 gridposy = sizeMenu.CabinetBase1y;
                                 gridposz = sizeMenu.CabinetBase1z;
                            }
                            else if (ObjectFollowsMouse.name == "Cabinet_Base_2(Clone)")
                            {
                                gridposx = sizeMenu.CabinetBase2x;
                                gridposy = sizeMenu.CabinetBase2y;
                                gridposz = sizeMenu.CabinetBase2z;
                            }
                            else if (ObjectFollowsMouse.name == "Cabinet_Base_Corner(Clone)")
                            {
                                gridposx = sizeMenu.CabinetBaseCornerx;
                                gridposy = sizeMenu.CabinetBaseCornery;
                                gridposz = sizeMenu.CabinetBaseCornerz;
                            }
                            else if (ObjectFollowsMouse.name == "Cabinet_Base_Sink(Clone)")
                            {
                                gridposx = sizeMenu.CabinetBaseSinkx;
                                gridposy = sizeMenu.CabinetBaseSinky;
                                gridposz = sizeMenu.CabinetBaseSinkz;
                            }
                            else if (ObjectFollowsMouse.name == "Cabinet_Tall(Clone)")
                            {
                                gridposx = sizeMenu.CabinetTallx;
                                gridposy = sizeMenu.CabinetTally;
                                gridposz = sizeMenu.CabinetTallz;
                            }
                            else if (ObjectFollowsMouse.name == "Stove(Clone)")
                            {
                                gridposx = sizeMenu.Stovex;
                                gridposy = sizeMenu.Stovey;
                                gridposz = sizeMenu.Stovez;
                            }
                            else if (ObjectFollowsMouse.name == "Bathtub(Clone)")
                            {
                                gridposx = sizeMenu.Bathtubx;
                                gridposy = sizeMenu.Bathtuby;
                                gridposz = sizeMenu.Bathtubz;
                            }
                            else if (ObjectFollowsMouse.name == "Shower(Clone)")
                            {
                                gridposx = sizeMenu.Showerx;
                                gridposy = sizeMenu.Showery;
                                gridposz = sizeMenu.Showerz;
                            }
                            else if (ObjectFollowsMouse.name == "Toilet(Clone)")
                            {
                                gridposx = sizeMenu.Toiletx;
                                gridposy = sizeMenu.Toilety;
                                gridposz = sizeMenu.Toiletz;
                            }
                            else if (ObjectFollowsMouse.name == "Vanity_1(Clone)")
                            {
                                gridposx = sizeMenu.Vanity1x;
                                gridposy = sizeMenu.Vanity1y;
                                gridposz = sizeMenu.Vanity1z;
                            }
                            else if (ObjectFollowsMouse.name == "Vanity_3(Clone)")
                            {
                                gridposx = sizeMenu.Vanity2x;
                                gridposy = sizeMenu.Vanity2z;
                                gridposz = sizeMenu.Vanity2y;
                            }
                            if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 0f)
                            {
                                //Debug.Log("1. if");
                                //hit.point = new Vector3(hit.point.x + 0.25f, hit.point.y, hit.point.z - 0.25f);
                                objectposition = new Vector3(a.x - gridposx / 200, a.y, a.z + gridposz / 200);
                                //Debug.Log(" abc " + objectposition);
                                //objectposition = new Vector3(a.x - b.size.x, a.y, a.z + b.size.z);
                                //Debug.Log(" abc " + objectposition);
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 90f)
                            {
                                //Debug.Log("2. if");
                                //hit.point = new Vector3(hit.point.x - 0.25f, hit.point.y, hit.point.z - 0.25f);
                                objectposition = new Vector3(a.x + gridposz / 200, a.y, a.z + gridposx / 200);
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 180f)
                            {
                                //Debug.Log("3. if");
                                //hit.point = new Vector3(hit.point.x - 0.25f, hit.point.y, hit.point.z + 0.25f);
                                objectposition = new Vector3(a.x + gridposx / 200, a.y, a.z - gridposz / 200);
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 270f)
                            {
                                //Debug.Log("4. if");
                                //hit.point = new Vector3(hit.point.x + 0.25f, hit.point.y, hit.point.z + 0.25f);
                                objectposition = new Vector3(a.x - gridposz / 200, a.y, a.z - gridposx / 200);
                            }


                            /*if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 0f)
                            {
                                Debug.Log("1. if");
                                //hit.point = new Vector3(hit.point.x + 0.25f, hit.point.y, hit.point.z - 0.25f);
                                objectposition = new Vector3(a.x - BuildingSystem.posx / 200, a.y, a.z + BuildingSystem.posz / 200);
                                //Debug.Log(" abc " + objectposition);
                                //objectposition = new Vector3(a.x - b.size.x, a.y, a.z + b.size.z);
                                //Debug.Log(" abc " + objectposition);
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 90f)
                            {
                                Debug.Log("2. if");
                                //hit.point = new Vector3(hit.point.x - 0.25f, hit.point.y, hit.point.z - 0.25f);
                                objectposition = new Vector3(a.x + BuildingSystem.posz / 200, a.y, a.z + BuildingSystem.posx / 200);
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 180f)
                            {
                                Debug.Log("3. if");
                                //hit.point = new Vector3(hit.point.x - 0.25f, hit.point.y, hit.point.z + 0.25f);
                                objectposition = new Vector3(a.x + BuildingSystem.posx / 200, a.y, a.z - BuildingSystem.posz / 200);
                            }
                            else if (ObjectFollowsMouse.tag == "3DModel" && ObjectFollowsMouse.transform.eulerAngles.y == 270f)
                            {
                                Debug.Log("4. if");
                                //hit.point = new Vector3(hit.point.x + 0.25f, hit.point.y, hit.point.z + 0.25f);
                                objectposition = new Vector3(a.x - BuildingSystem.posz / 200, a.y, a.z - BuildingSystem.posx / 200);
                            }*/

                            //Vector3 objectposition = new Vector3(a.x + BuildingSystem.posx / 200, a.y, a.z + BuildingSystem.posz / 200);
                            ObjectFollowsMouse.transform.position = objectposition;

                            if (ObjectFollowsMouse.GetComponent<PlaceableObject>().isColliding == true)
                            {
                                ObjectFollowsMouse.transform.GetComponent<MeshRenderer>().material.color = Color.red;
                                for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
                                {
                                    rend = ObjectFollowsMouse.transform.GetChild(i).GetComponent<MeshRenderer>();
                                    for (int j = 0; j < rend.materials.Length; j++)
                                    {
                                        rend.materials[j].color = Color.red;
                                    }
                                }
                            }
                            else
                            {
                                ObjectFollowsMouse.transform.GetComponent<MeshRenderer>().material.color = Color.white;
                                for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
                                {
                                    rend = ObjectFollowsMouse.transform.GetChild(i).GetComponent<MeshRenderer>();
                                    for (int j = 0; j < rend.materials.Length; j++)
                                    {
                                        rend.materials[j].color = Color.white;
                                    }
                                }
                            }
                        }
                    }
                }
                
            }


           
            if (MouseInputUIBlocker.BlockedByUI == false)
            {
                //Rotate
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (ObjectFollowsMouse.name == "Vanity_3(Clone)")
                    {
                        ObjectFollowsMouse.transform.Rotate(0, 0, 90f, Space.Self);
                    }
                    else
                    {
                        ObjectFollowsMouse.transform.Rotate(0, 90f, 0, Space.Self);
                    }
                }
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (ObjectFollowsMouse.name == "Vanity_3(Clone)")
                    {
                        ObjectFollowsMouse.transform.Rotate(0, 0, -90f, Space.Self);
                    }
                    else
                    {
                        ObjectFollowsMouse.transform.Rotate(0, -90f, 0, Space.Self);
                    }
                }

                // Sol t�k koyma
                if (ObjectFollowsMouse.name == "CubeWall(Clone)")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log("START");
                        SetStart();
                    }
                    else if ((Input.GetMouseButtonUp(0)))
                    {
                        if (deleteAfterWallHold == false)
                        {
                            //Debug.Log("END");
                            setEnd();
                        }
                    }
                    else
                    {
                        
                        //Debug.Log("ELSE");
                        if (creatingWall == true && deleteAfterWallHold== false)
                        {
                            //Debug.Log("ADJUST");
                            adjust();
                        }
                    }


                }
                else if (ObjectFollowsMouse.name == "Floor(Clone)")
                {

                    if (Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log("START");
                        SetStartFloor();
                    }
                    else if ((Input.GetMouseButtonUp(0)))
                    {
                        if (deleteAfterFloorHold == false)
                        {
                            //Debug.Log("END");
                            setEndFloor();
                        }
                    }
                    else
                    {

                        //Debug.Log("ELSE");
                        if (creatingFloor == true && deleteAfterFloorHold == false)
                        {
                            //Debug.Log("ADJUST");
                            adjustFloor();
                        }
                    }

                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (ObjectFollowsMouse.GetComponent<PlaceableObject>().isColliding == false)
                        {
                            //cabinetbase1count = 0;

                            //ObjectFollowsMouse.AddComponent<BoxCollider>();
                            int LayerDefault = LayerMask.NameToLayer("Default");
                            ObjectFollowsMouse.layer = LayerDefault;
                            //Debug.Log(ObjectFollowsMouse.GetComponent<MeshRenderer>().bounds.size);
                            //Debug.Log(cube.GetComponent<MeshRenderer>().bounds.size);


                            //B�yle yap�nca NewDoor2 modeli kendi i�inde collision veriyor prefab� d�zenle
                            /*if (ObjectFollowsMouse.name == "NewDoor1(Clone)" || ObjectFollowsMouse.name == "NewDoor2(Clone)" || ObjectFollowsMouse.name == "NewWindow2(Clone)" || ObjectFollowsMouse.name == "Frame(Clone)" || ObjectFollowsMouse.name == "Rail(Clone)")
                            {
                                ObjectFollowsMouse.AddComponent<Rigidbody>();
                                ObjectFollowsMouse.GetComponent<Rigidbody>().isKinematic = true;
                                for (int i = 0; i < ObjectFollowsMouse.transform.childCount; i++)
                                {
                                    ObjectFollowsMouse.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                                    ObjectFollowsMouse.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = true;                                 

                                }

                            }
                            else
                            {
                                ObjectFollowsMouse.AddComponent<Rigidbody>();
                                ObjectFollowsMouse.GetComponent<Rigidbody>().isKinematic = true;
                            }*/
                            ObjectFollowsMouse.AddComponent<Rigidbody>();
                            ObjectFollowsMouse.GetComponent<Rigidbody>().isKinematic = true;

                            isButtonClicked = false;
                            /*for (int i = 0; i < itemModelCount.Length; i++)
                            {
                                itemModelCount[i] = 0;
                            }*/
                            foundcount = false;
                        }
                        else
                        {
                            Debug.Log("OBJE KOYULAMAZ");
                        }
                    }
                }
                
              
                // Sa� t�k delete
                if (Input.GetMouseButtonDown(1))
                {
                    //cabinetbase1count = 0;
                    /*for (int i = 0; i < itemModelCount.Length; i++)
                    {
                        itemModelCount[i] = 0;
                    }*/
                    foundcount = false;
                    if (ObjectFollowsMouse.name == "CubeWall(Clone)")
                    {

                        if(creatingWall== false)
                        {
                            Destroy(ObjectFollowsMouse);
                            followmouse = true;
                        }
                        else
                        {
                            deleteAfterWallHold = true;
                            Destroy(startPole);
                            Destroy(endPole);
                            Destroy(wall);
                            Destroy(ObjectFollowsMouse);
                            followmouse = true;
                        }

                        
                    }
                    else if (ObjectFollowsMouse.name == "Floor(Clone)")
                    {

                        if (creatingFloor == false)
                        {
                            Destroy(ObjectFollowsMouse);
                            followmouse = true;
                        }
                        else
                        {
                            deleteAfterFloorHold = true;
                            Destroy(startFloor);
                            Destroy(endFloor);
                            Destroy(floor);
                            Destroy(ObjectFollowsMouse);
                            followmouse = true;
                        }


                    }
                    else
                    {
                        Destroy(ObjectFollowsMouse);
                    }
                }
            }
            
        }
        else
        {
            
            // B�T�N MODELLERE 3DModel TAG'I EKLE


            
            if (MouseInputUIBlocker.BlockedByUI == false)
            {
                 float camerascroll = Input.GetAxis("Mouse ScrollWheel");
                 cameraZoom.y -= camerascroll* cameraScrollSpeed* 100f* Time.deltaTime;
                 mainCamera.transform.localPosition = cameraZoom;
            }


            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                //Debug.Log(MouseInputUIBlocker.BlockedByUI + " blockedbyUI");
                //Debug.Log(raycastHit.point);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log(raycastHit.collider.tag);
                    Debug.Log(sizeMenu.transform.position);
                    Debug.Log(settingsMenu.active);
                    Debug.Log(IsMouseHoveringOnButton.onButton);
                    //Debug.Log(sizeMenu.transform.position + " sizemenuposition");
                    if (raycastHit.collider.tag == "3DModel" && (sizeMenu.transform.position == new Vector3(3960f,3540f,3000f)) && settingsMenu.active == false && IsMouseHoveringOnButton.onButton == false)
                    {
                        //isInteractionOpen = true;
                        Selected3DObject = raycastHit.transform.gameObject;
                        //ObjectFollowsMouse = Selected3DObject;  // BURA CLIENTTAN SONRA EKLEND�
                        Debug.Log(Selected3DObject + "selected 3d object");
                        InteractionCanvas.SetActive(true);

                        // InteractionCanvas.transform.SetParent(raycastHit.transform);

                        //InteractionCanvas.transform.position = new Vector3(raycastHit.transform.position.x, raycastHit.transform.position.y + raycastHit.transform.gameObject.GetComponent<MeshRenderer>().bounds.size.y + 0.4f, raycastHit.transform.position.z);


                        //InteractionCanvas.transform.position = new Vector3(raycastHit.transform.position.x, raycastHit.transform.position.y + 4f, raycastHit.transform.position.z);  // World spaceteyse buralar a��lacak

                    }

                    else
                    {
                        if (MouseInputUIBlocker.BlockedByUI == false && IsMouseHoveringOnButton.onButton == false)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                InteractionCanvas.SetActive(false);
                                //isInteractionOpen=false;
                            }
                        }
                    }

                }
            }
        }
    }
}
