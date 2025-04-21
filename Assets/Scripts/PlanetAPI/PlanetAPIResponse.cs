using System;
using System.Collections.Generic;

[Serializable]
public class Family
{
    public string scientificNameWithoutAuthor;
    public string scientificNameAuthorship;
    public string scientificName;
}

[Serializable]
public class Gbif
{
    public string id;
}

[Serializable]
public class Genus
{
    public string scientificNameWithoutAuthor;
    public string scientificNameAuthorship;
    public string scientificName;
}

[Serializable]
public class Powo
{
    public string id;
}

[Serializable]
public class PredictedOrgan
{
    public string image;
    public string filename;
    public string organ;
    public double score;
}

[Serializable]
public class Query
{
    public string project;
    public List<string> images;
    public List<string> organs;
    public bool includeRelatedImages;
    public bool noReject;
    public string type;
}

[Serializable]
public class Result
{
    public double score;
    public Species species;
    public Gbif gbif;
    public Powo powo;
}

[Serializable]
public class APIResponse
{
    public Query query;
    public List<PredictedOrgan> predictedOrgans;
    public string language;
    public string preferedReferential;
    public string bestMatch;
    public List<Result> results;
    public string version;
    public int remainingIdentificationRequests;
}

[Serializable]
public class Species
{
    public string scientificNameWithoutAuthor;
    public string scientificNameAuthorship;
    public Genus genus;
    public Family family;
    public List<string> commonNames;
    public string scientificName;
}