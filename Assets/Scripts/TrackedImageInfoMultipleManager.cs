using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoMultipleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private Button infoButton;

    [SerializeField]
    private Text imageTrackedText;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text instructionText;

    [Header("The length of this list must match the number of images in Reference Image Library")]
    [SerializeField]
    private GameObject[] arObjectsToPlace;

    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f,0.1f,0.1f);


    private ARTrackedImageManager m_TrackedImageManager;
    private IReferenceImageLibrary refLibrary;

    private int refImageCount;
    private Dictionary<string, GameObject> arObjects;

    void Awake()
    {
        dismissButton.onClick.AddListener(Dismiss);
        infoButton.onClick.AddListener(Info);
       
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        
        // встановлю всі GameObject у словник
       
    }

    private void Start()
    {
        refLibrary = m_TrackedImageManager.referenceLibrary;
        refImageCount = refLibrary.count;
        LoadObjectDictionary();
    }

    void LoadObjectDictionary()
    {
        arObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < refImageCount; i++)
        {
            GameObject newOverlay = new GameObject();
            newOverlay = arObjectsToPlace[i];
            //check if the object is prefab and need to be instantiated
            if (arObjectsToPlace[i].gameObject.scene.rootCount == 0)
            {
                newOverlay = (GameObject)Instantiate(arObjectsToPlace[i], transform.localPosition, Quaternion.identity);
            }

            arObjects.Add(refLibrary[i].name, newOverlay);
            newOverlay.SetActive(false);
        }


    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void Dismiss() => welcomePanel.SetActive(false);

    private void Info()
    {
        welcomePanel.SetActive(true);

        
        if (imageTrackedText.text == "Куб")
        {
            titleText.text = "Куб";
            instructionText.text = "Кубом називається правильний многогранник, кожна грань якого є квадратом."
                +" Куб можна назвати об'ємний, тривимірним або навіть 3D квадратом. Куб має 8 вершин, граней 6, 12 ребер."
                +" Куб - це дивовижна геометрична фігура, в яку можна заховати або вписати інші фігури: октаедр, тетраедр, ікосаедр та інші."
                +"\n\nФормула. Об'єм куба через довжину ребра а:\nV = a^3\n\nФормула. Площа поверхні куба через довжину ребра а:\nS = 6*a^2" ;
           // instructionText.alignment = TextAnchor.MiddleCenter;
        } else if(imageTrackedText.text == "Циліндр")
        {
            titleText.text = "Циліндр";
            instructionText.text = "Циліндр - це геометричне тіло, обмежене циліндричною поверхнею і двома площинами (основами циліндра)."
                + " Циліндрична поверхня -поверхня, одержувана при русі прямої(утворює L) паралельно самій собі, уздовж плоскої кривої направляючої."
                + "\n\nФормула.Об'єм циліндра: \nV = π*r^2*h=\n= π*(d^2/4)*h \n,де r -радіус основи, h - висота циліндра, d - діаметр основи."
                + "\n\nФормула.Повна площа поверхні циліндра: \nS = 2*π*r*(h + r)";
           // instructionText.alignment = TextAnchor.MiddleCenter;
        }
        else if(imageTrackedText.text == "Октаедр")
        {
            titleText.text = "Октаедр";
            
            instructionText.text = "Октаедр це геометричне тіло з восьми граней, кожна з яких - правильний трикутник"
                + " Правильний октаедр складений з восьми рівносторонніх трикутників. Кожна вершина октаедра є вершиною чотирьох трикутників. Отже, сума плоских кутів при кожній вершині дорівнює 240 °. "
                + "\n\nФормула. Об'єм октаедра:\nV= (а^3/3) * √2"
                + "\n\nФормула. Площа поверхні октаедра :\nS = 2*а^2*√3";


        //    instructionText.alignment = TextAnchor.MiddleCenter;

        }
        else
        {
            titleText.text = "Інструкція";
            instructionText.text = "1.Наведіть камеру телефона на мітку-зображення;"
                +"\n2.Після зчитування з'явится відпвідна геометрична модель;"
                +"\n3.Для детальної інформацію необхідно натиснути кнопку \"Детальніше\".";
         //   instructionText.alignment = TextAnchor.MiddleCenter;
        }
        
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var addedImage in eventArgs.added)
        {
            //  ActivateTrackedObject(addedImage.referenceImage.name);
            UpdateARImage(addedImage);
        }

        foreach (var update in eventArgs.updated)
        {
            UpdateARImage(update);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            //arObjects[trackedImage.name].SetActive(false);
            Destroy(trackedImage.gameObject);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        //if tracked image tracking state is comparable to tracking
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            imageTrackedText.text = trackedImage.referenceImage.name;
            if (imageTrackedText.text == "Octaedr")
            {
                imageTrackedText.text = "Октаедр";
            } else if (imageTrackedText.text == "Cube")
            {
                imageTrackedText.text = "Куб";

            } else if (imageTrackedText.text == "Cylinder")
            {
                imageTrackedText.text = "Циліндр";

            }
            //set the image tracked ar object to active 
            arObjects[trackedImage.referenceImage.name].SetActive(true);
            arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
            arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
        }
        else //if tracked image tracking state is limited or none 
        {
            //deactivate the image tracked ar object 
            arObjects[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    void ActivateTrackedObject(string imageName)
    {
      
        Debug.Log("Tracked the target: " + imageName);
        arObjects[imageName].SetActive(true);
        // Give the initial image a reasonable default scale
        arObjects[imageName].transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
    }

   /* void AssignGameObject(string name, Vector3 newPosition)
    {
        if(arObjectsToPlace != null)
        {
            GameObject goARObject = arObjects[name];
            goARObject.SetActive(true);
            goARObject.transform.position = newPosition;
            goARObject.transform.localScale = scaleFactor;
            foreach(GameObject go in arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if(go.name != name)
                {
                    go.SetActive(false);
                }
            } 
        }
    }
   */
}
