using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static HeapMethod.DistanceMaxHeap;

public class HeapMethod : MonoBehaviour
{
    [SerializeField] GameObject processing;
    [SerializeField] GameObject start;
    [SerializeField] GameObject manager;

    [SerializeField] TMP_InputField coordX;
    [SerializeField] TMP_InputField coordY;
    [SerializeField] TMP_InputField resultsField;
    [SerializeField] TMP_InputField yearField;

    [SerializeField] public GameObject incidencePrefab;

    [SerializeField] public GameObject outputWindow;

    public List<GameObject> incidencesDisplay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessData() // runs heap sort and updates GUI
    {
        start.GetComponent<Button>().interactable = false;
        processing.SetActive(true);
        outputWindow.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        DistanceMaxHeap heap = new DistanceMaxHeap(Convert.ToInt32(resultsField.text),
            Convert.ToDouble(coordX.text), Convert.ToDouble(coordY.text),
            Convert.ToInt32(yearField.text)); // create new heap with updated coords, # results, and year

        for (int i = 0; i < 16512; i++)
        {
            heap.insert(manager.GetComponent<Manager>().incidences[i], i + 1); // insert all incidences into heap
        }

        display(ref heap); // display heap on the GUI

        processing.SetActive(false);
        start.GetComponent<Button>().interactable = true;
    }


    public class DistanceMaxHeap // heap with max capacity, removes largest element to keep elements with smallest distances
    {
        public class Incidence // incidence object stores CancerIncidence with distance and incidence #
        {
            public CancerIncidence c;
            public double distance;
            public int incidenceNumber;

            public void Assign(ref CancerIncidence a, CancerIncidence b) // essentially a = b, meant for CancerIncidence's
            {
                a = new CancerIncidence();
                a.setAgeAdjustedCI_Lower(b.getAgeAdjustedCI_Lower());
                a.setAgeAdjustedCI_Upper(b.getAgeAdjustedCI_Upper());
                a.setCrudeRate(b.getCrudeRate());
                a.setCrudeRate_CI_Lower(b.getCrudeRate_CI_Lower());
                a.setCrudeRate_CI_Upper(b.getCrudeRate_CI_Upper());
                a.setYear(b.getYear());
                a.setGender(b.getGender());
                a.setRace(b.getRace());
                a.setEventType(b.getEventType());
                a.setPopulation(b.getPopulation());
                a.setAffectedArea(b.getAffectedArea());
                a.setCount(b.getCount());
                a.setLocationX(b.getLocationX());
                a.setLocationY(b.getLocationY());
            }

            public Incidence(CancerIncidence _c, double _distance, int _incidenceNumber) // constructor
            {
                Assign(ref c, _c);
                distance = _distance;
                incidenceNumber = _incidenceNumber;
            }
        };

        public Incidence[] distances; // stores incidences
        public int size;
        int capacity;
        double[] source = new double[2]; // store source coords inputted by user
        int year; // store year inputted by user
        public GameObject incidencePre; // dropdown prefab

        public DistanceMaxHeap(int _capacity, double _x, double _y, int _year) // max heap constructor
        {
            capacity = _capacity;
            size = 0;
            distances = new Incidence[capacity];
            source[0] = _x;
            source[1] = _y;
            year = _year;
        }

        public void extractMax() // remove largest element and heapify
            // (does not update size since it is only used once the heap's capacity is max)
        {
            int rootPos = 0;

            Assign(ref distances[rootPos], distances[size - 1]); // set first element equal to last element
            distances[size - 1].distance = 0; // set last element distance to 0

            while (true)
            {
                if (2 * rootPos + 1 < size - 1) // if first child exists
                {
                    int tempPos = 0;
                    if (2 * rootPos + 2 < size - 1) // if second child exists
                    {
                        tempPos = (distances[2 * rootPos + 1].distance > distances[2 * rootPos + 2].distance) ?
                            2 * rootPos + 1 : 2 * rootPos + 2;  // set tempPos to position of greater child;
                    }
                    else
                    {
                        tempPos = 2 * rootPos + 1; // set tempPos equal to position of left child
                    }

                    Swap(ref distances[tempPos], ref distances[rootPos]); // swap parent with greater child

                    rootPos = tempPos; // update rootPos
                }
                else // if no children
                {
                    break; // break loop
                }
            }
        }

        public void insert(CancerIncidence d, int incidenceNumber)
        { // insert will automatically remove largest element if capacity is full
            double distance = Math.Sqrt(Math.Pow(source[0] - d.getLocationX(), 2) + Math.Pow(source[1] - d.getLocationY(), 2));
            // ^ set distance from source to incidence

            if (d.getYear() == year) // if year of incidence matches user specified year
            {
                if (size < capacity) // if heap is not full yet
                {
                    distances[size] = new Incidence(d, distance, incidenceNumber); // add new element
                    size++; // increment size
                    heapifyUp(); // heapify last element upwards
                }
                else if (distances[0].distance > distance) // if heap is full, then if the max distance in heap is larger than current distance
                {
                    extractMax(); // remove maximum element

                    distances[size - 1] = new Incidence(d, distance, incidenceNumber); // insert new element into last position
                    heapifyUp(); // heapify the last element upwards
                }
            }
        }

        public void heapifyUp() // used in insert function
        {
            int tempPos = size - 1; // set tempPos to last element
            int parentPos = (tempPos - 1) / 2; // set parentPos to parent of tempPos

            while (tempPos > 0 && distances[tempPos].distance > distances[parentPos].distance)
            { // while tempPos is not root and distance at tempPos is greater than distance at parentPos
                Swap(ref distances[tempPos], ref distances[parentPos]); // swap incidences (Swap explained later)

                tempPos = parentPos; // set tempPos equal to parentPos
                parentPos = (parentPos - 1) / 2; // set parentPos = parent element
            }
        }

        public void Swap(ref Incidence a, ref Incidence b) // swaps two Incidence object values
        {
            double ADistance = a.distance;
            int AIncidenceNumber = a.incidenceNumber;
            double AAgeAdjustedRate = a.c.getAgeAdjustedRate();
            double AAgeAdjustedCI_Lower = a.c.getAgeAdjustedCI_Lower();
            double AAgeAdjustedCI_Upper = a.c.getAgeAdjustedCI_Upper();
            double ACrudeRate = a.c.getCrudeRate();
            double ACrudeRate_CI_Lower = a.c.getCrudeRate_CI_Lower();
            double ACrudeRate_CI_Upper = a.c.getCrudeRate_CI_Upper();
            int AYear = a.c.getYear();
            string AGender = a.c.getGender();
            string ARace = a.c.getRace();
            string AEventType = a.c.getEventType();
            int APopulation = a.c.getPopulation();
            string AAffectedArea = a.c.getAffectedArea();
            int ACount = a.c.getCount();
            double ALocationX = a.c.getLocationX();
            double ALocationY = a.c.getLocationY();

            a.distance = b.distance;
            a.incidenceNumber = b.incidenceNumber;
            a.c.setAgeAdjustedRate(b.c.getAgeAdjustedRate());
            a.c.setAgeAdjustedCI_Lower(b.c.getAgeAdjustedCI_Lower());
            a.c.setAgeAdjustedCI_Upper(b.c.getAgeAdjustedCI_Upper());
            a.c.setCrudeRate(b.c.getCrudeRate());
            a.c.setCrudeRate_CI_Lower(b.c.getCrudeRate_CI_Lower());
            a.c.setCrudeRate_CI_Upper(b.c.getCrudeRate_CI_Upper());
            a.c.setYear(b.c.getYear());
            a.c.setGender(b.c.getGender());
            a.c.setRace(b.c.getRace());
            a.c.setEventType(b.c.getEventType());
            a.c.setPopulation(b.c.getPopulation());
            a.c.setAffectedArea(b.c.getAffectedArea());
            a.c.setCount(b.c.getCount());
            a.c.setLocationX(b.c.getLocationX());
            a.c.setLocationY(b.c.getLocationY());

            b.distance = ADistance;
            b.incidenceNumber = AIncidenceNumber;
            b.c.setAgeAdjustedRate(AAgeAdjustedRate);
            b.c.setAgeAdjustedCI_Lower(AAgeAdjustedCI_Lower);
            b.c.setAgeAdjustedCI_Upper(AAgeAdjustedCI_Upper);
            b.c.setCrudeRate(ACrudeRate);
            b.c.setCrudeRate_CI_Lower(ACrudeRate_CI_Lower);
            b.c.setCrudeRate_CI_Upper(ACrudeRate_CI_Upper);
            b.c.setYear(AYear);
            b.c.setGender(AGender);
            b.c.setRace(ARace);
            b.c.setEventType(AEventType);
            b.c.setPopulation(APopulation);
            b.c.setAffectedArea(AAffectedArea);
            b.c.setCount(ACount);
            b.c.setLocationX(ALocationX);
            b.c.setLocationY(ALocationY);
        }

        public void Assign(ref Incidence a, Incidence b) // assigns values of b to a (a = b)
        {
            a = new Incidence(b.c, b.distance, b.incidenceNumber);
            a.c.setAgeAdjustedCI_Lower(b.c.getAgeAdjustedCI_Lower());
            a.c.setAgeAdjustedCI_Upper(b.c.getAgeAdjustedCI_Upper());
            a.c.setCrudeRate(b.c.getCrudeRate());
            a.c.setCrudeRate_CI_Lower(b.c.getCrudeRate_CI_Lower());
            a.c.setCrudeRate_CI_Upper(b.c.getCrudeRate_CI_Upper());
            a.c.setYear(b.c.getYear());
            a.c.setGender(b.c.getGender());
            a.c.setRace(b.c.getRace());
            a.c.setEventType(b.c.getEventType());
            a.c.setPopulation(b.c.getPopulation());
            a.c.setAffectedArea(b.c.getAffectedArea());
            a.c.setCount(b.c.getCount());
            a.c.setLocationX(b.c.getLocationX());
            a.c.setLocationY(b.c.getLocationY());
        }
    };

    public void display(ref DistanceMaxHeap heap) // displays elements in GUI
    {
        DistanceMaxHeap.Incidence[] incidenceSort = new DistanceMaxHeap.Incidence[heap.size]; // initialize list of sorted incidences

        outputWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100 * heap.size); // set size of scroll window to fit incidences

        int tempSize = heap.size;
        for (int i = 0; i < tempSize; i++)
        {
            heap.Assign(ref incidenceSort[heap.size - 1], heap.distances[0]); // set incidence sort element to max element in heap
            heap.extractMax(); // remove max element from heap
            heap.size--; // decrease size (for internal workings of extractMax())
        }

        for (int i = 0; i < tempSize; i++)
        {
            GameObject temp = Instantiate(incidencePrefab); // create new dropdown object to show data
            temp.transform.SetParent(outputWindow.transform, true); // move dropdown to content window
            temp.SetActive(true);

            RectTransform rt = temp.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - (100 * i) + 10); // set to correct position in list

            TMP_Dropdown dropdown = temp.GetComponent<TMP_Dropdown>();

            dropdown.options.Add(new TMP_Dropdown.OptionData(Convert.ToString(i + 1) + ": Incidence " + Convert.ToString(incidenceSort[i].incidenceNumber)));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Year: " + Convert.ToString(incidenceSort[i].c.getYear())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Distance: " + Convert.ToString(incidenceSort[i].distance)));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Location: " + Convert.ToString(incidenceSort[i].c.getLocationX()) + ", " + Convert.ToString(incidenceSort[i].c.getLocationY())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Event Type: " + Convert.ToString(incidenceSort[i].c.getEventType())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Race: " + Convert.ToString(incidenceSort[i].c.getRace())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Gender: " + Convert.ToString(incidenceSort[i].c.getGender())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Population: " + Convert.ToString(incidenceSort[i].c.getPopulation())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Count: " + Convert.ToString(incidenceSort[i].c.getCount())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Crude Rate: " + Convert.ToString(incidenceSort[i].c.getCrudeRate())));
            // ^^ assign values of dropdown "options" (altered dropdown to display info rather than choose options) to values of incidences

            incidencesDisplay.Add(temp); // add incidence to display
        }
    }
}