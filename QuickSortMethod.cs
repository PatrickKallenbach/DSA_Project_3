using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static QuickSortMethod.DistanceContainer;

public class QuickSortMethod : MonoBehaviour
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
        processing.SetActive(true);
        start.GetComponent<Button>().interactable = false;
        outputWindow.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        DistanceContainer container = new DistanceContainer(Convert.ToInt32(resultsField.text),
            Convert.ToDouble(coordX.text), Convert.ToDouble(coordY.text),
            Convert.ToInt32(yearField.text)); // create new container with updated coords, # results, and year

        for (int i = 0; i < 16512; i++)
        {
            container.insert(manager.GetComponent<Manager>().incidences[i], i + 1); // insert all incidences into container
        }

        display(ref container); // display container on the GUI

        processing.SetActive(false);
        start.GetComponent<Button>().interactable = true;
    }


    public class DistanceContainer // heap with max capacity, removes largest element to keep elements with smallest distances
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

        public DistanceContainer(int _capacity, double _x, double _y, int _year) // max heap constructor
        {
            capacity = _capacity;
            size = 0;
            distances = new Incidence[capacity];
            source[0] = _x;
            source[1] = _y;
            year = _year;
        }

        public void quickSort(ref Incidence[] distances, int low, int high) {
            if (low < high) {
                double pivot = distances[high].distance;
                int i = low;

                for (int j = low; j < high; j++) {
                    if (distances[j].distance < pivot) {
                        Swap(ref distances[i], ref distances[j]);
                        i++;
                    }
                }
                Swap(ref distances[i], ref distances[high]);

                quickSort(ref distances, low, i - 1);
                quickSort(ref distances, i + 1, high);
            }
        }

        public void insert(CancerIncidence element, int incidenceNumber) {
            if (element.getYear() == year) // if year of incidence matches user specified year
            {
                double distance = Math.Sqrt(Math.Pow(source[0] - element.getLocationX(), 2) + Math.Pow(source[1] - element.getLocationY(), 2));

                if (size < capacity)
                {
                    distances[size++] = new Incidence(element, distance, incidenceNumber);
                    if (size != 1)
                    {
                        if (distances[size - 1].distance < distances[size - 2].distance)
                        {
                            quickSort(ref distances, 0, size - 1);
                        }
                    }
                }
                else
                {
                    if (distance < distances[capacity - 1].distance)
                    {
                        distances[capacity - 1] = new Incidence(element, distance, incidenceNumber);
                        if (distances[size - 1].distance < distances[size - 2].distance)
                        {
                            quickSort(ref distances, 0, size - 1);
                        }
                    }
                }
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

    public void display(ref DistanceContainer container) // displays elements in GUI
    {
        outputWindow.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100 * container.size); // set size of scroll window to fit incidences

        for (int i = 0; i < container.size; i++)
        {
            GameObject temp = Instantiate(incidencePrefab); // create new dropdown object to show data
            temp.transform.SetParent(outputWindow.transform, true); // move dropdown to content window
            temp.SetActive(true);

            RectTransform rt = temp.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - (100 * i) + 10); // set to correct position in list

            TMP_Dropdown dropdown = temp.GetComponent<TMP_Dropdown>();

            dropdown.options.Add(new TMP_Dropdown.OptionData(Convert.ToString(i + 1) + ": Incidence " + Convert.ToString(container.distances[i].incidenceNumber)));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Year: " + Convert.ToString(container.distances[i].c.getYear())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Distance: " + Convert.ToString(container.distances[i].distance)));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Location: " + Convert.ToString(container.distances[i].c.getLocationX()) + ", " + Convert.ToString(container.distances[i].c.getLocationY())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Event Type: " + Convert.ToString(container.distances[i].c.getEventType())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Race: " + Convert.ToString(container.distances[i].c.getRace())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Gender: " + Convert.ToString(container.distances[i].c.getGender())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Population: " + Convert.ToString(container.distances[i].c.getPopulation())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Count: " + Convert.ToString(container.distances[i].c.getCount())));
            dropdown.options.Add(new TMP_Dropdown.OptionData("Crude Rate: " + Convert.ToString(container.distances[i].c.getCrudeRate())));
            // ^^ assign values of dropdown "options" (altered dropdown to display info rather than choose options) to values of incidences

            incidencesDisplay.Add(temp); // add incidence to display
        }
    }
}