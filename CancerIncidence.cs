using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancerIncidence // CancerIncidence class derived from Bridges class definition, converted to C# by Patrick
{
    [SerializeField] double age_adjusted_rate;       // expected cancer rate, adjusted for age
    [SerializeField] double[] age_adjusted_rate_ci = new double[2];  // confidence interval-lower,upper
    [SerializeField] double crude_rate;              // cancer rate adjusted by population
    [SerializeField] double[] crude_rate_ci = new double[2];     // confidence interval

    [SerializeField] int count;                  // incidence count
    [SerializeField] int year;                   // reporting year
    [SerializeField] int population;             // population of this area

    [SerializeField] string gender;               // gender (male, female, male and female
    [SerializeField] string race;
    [SerializeField] string event_type;               // incidence, mortality
    [SerializeField] string affected_area;            // location, typically, state

    [SerializeField] double[] loc = new double[2];           // location (cartesian coords)

    public CancerIncidence()
    {
        loc[0] = loc[1] = 0.0;
        age_adjusted_rate_ci[0] = age_adjusted_rate_ci[1] = 0.0;
        crude_rate_ci[0] = crude_rate_ci[1] = 0.0;
    }

    public double getAgeAdjustedRate() { return age_adjusted_rate; }
    public void setAgeAdjustedRate(double aar) { age_adjusted_rate = aar; }

    public double getAgeAdjustedCI_Lower() { return age_adjusted_rate_ci[0]; }
    public void setAgeAdjustedCI_Lower(double ci_l) { age_adjusted_rate_ci[0] = ci_l; }

    public double getAgeAdjustedCI_Upper() { return age_adjusted_rate_ci[1]; }
    public void setAgeAdjustedCI_Upper(double ci_u) { age_adjusted_rate_ci[1] = ci_u; }

    public double getCrudeRate() { return crude_rate; }
    public void setCrudeRate(double cr) { crude_rate = cr; }

    public double getCrudeRate_CI_Lower() { return crude_rate_ci[0]; }
    public void setCrudeRate_CI_Lower(double cr_l) { crude_rate_ci[0] = cr_l; }

    public double getCrudeRate_CI_Upper() { return crude_rate_ci[1]; }
    public void setCrudeRate_CI_Upper(double cr_u) { crude_rate_ci[1] = cr_u; }

    public int getYear() { return year; }
    public void setYear(int y) { year = y; }

    public string getGender() { return gender; }
    public void setGender(string g) { gender = g; }

    public string getRace() { return race; }
    public void setRace(string r) { race = r; }

    public string getEventType() { return event_type; }
    public void setEventType(string et) { event_type = et; }

    public int getPopulation() { return population; }
    public void setPopulation(int pop) { population = pop; }

    public string getAffectedArea() { return affected_area; }
    public void setAffectedArea(string area) { affected_area = area; }

    public int getCount() { return count; }
    public void setCount(int c) { count = c; }

    public double getLocationX() { return loc[0]; }
    public void setLocationX(double locX) { loc[0] = locX; }

    public double getLocationY() { return loc[1]; }
    public void setLocationY(double locY) { loc[1] = locY; }
}

/*

public class CancerIncidence : MonoBehaviour
{
	[SerializeField] double age_adjusted_rate;       // expected cancer rate, adjusted for age
    [SerializeField] double[] age_adjusted_rate_ci = new double[2];  // confidence interval-lower,upper
    [SerializeField] double crude_rate;              // cancer rate adjusted by population
    [SerializeField] double[] crude_rate_ci = new double[2];     // confidence interval

	[SerializeField] int count;                  // incidence count
	[SerializeField] int year;                   // reporting year
    [SerializeField] int population;             // population of this area

	[SerializeField] string gender;               // gender (male, female, male and female
	[SerializeField] string race;
	[SerializeField] string event_type;               // incidence, mortality
    [SerializeField] string affected_area;            // location, typically, state

    [SerializeField] double[] loc = new double[2];           // location (cartesian coords


	void Start()
	{
        loc[0] = loc[1] = 0.0;
        age_adjusted_rate_ci[0] = age_adjusted_rate_ci[1] = 0.0;
        crude_rate_ci[0] = crude_rate_ci[1] = 0.0;
    }

	void Update()
	{

	}

	public double getAgeAdjustedRate()
	{
		return age_adjusted_rate;
	}

	public void setAgeAdjustedRate(double aar)
	{
		age_adjusted_rate = aar;
	}

	public double getAgeAdjustedCI_Lower()
	{
		return age_adjusted_rate_ci[0];
	}
	public void setAgeAdjustedCI_Lower(double ci_l)
	{
		age_adjusted_rate_ci[0] = ci_l;
	}

	public double getAgeAdjustedCI_Upper()
	{
		return age_adjusted_rate_ci[1];
	}

	public void setAgeAdjustedCI_Upper(double ci_u)
	{
		age_adjusted_rate_ci[1] = ci_u;
	}
	public double getCrudeRate()
	{
		return crude_rate;
	}
	public void setCrudeRate(double cr)
	{
		crude_rate = cr;
	}

	public double getCrudeRate_CI_Lower()
	{
		return crude_rate_ci[0];
	}

	public void setCrudeRate_CI_Lower(double cr_l)
	{
		crude_rate_ci[0] = cr_l;
	}
	public double getCrudeRate_CI_Upper()
	{
		return crude_rate_ci[1];
	}

	public void setCrudeRate_CI_Upper(double cr_u)
	{
		crude_rate_ci[1] = cr_u;
	}

	public int getYear()
	{
		return year;
	}

	public void setYear(int y)
	{
		year = y;
	}
	c string getGender()
	{
		return gender;
	}
	public void setGender(string g)
	{
		gender = g;
	}

	public string getRace()
	{
		return race;
	}
	public void setRace(string r)
	{
		race = r;
	}
	public string getEventType()
	{
		return event_type;
	}
	public void setEventType(string et)
	{
		event_type = et;
	}

	public int getPopulation()
	{
		return population;
	}

	public void setPopulation(int pop)
	{
		population = pop;
	}

	public string getAffectedArea()
	{
		return affected_area;
	}

	public void setAffectedArea(string area)
	{
		affected_area = area;
	}

	public int getCount()
	{
		return count;
	}
	public void setCount(int c)
	{
		count = c;
	}
	public double getLocationX()
	{
		return loc[0];
	}
	public void setLocationX(double locX)
	{
		loc[0] = locX;
	}

	public double getLocationY()
	{
		return loc[1];
	}
	public void setLocationY(double locY)
	{
		loc[1] = locY;
	}
}
*/