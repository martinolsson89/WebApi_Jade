using System;
using Configuration;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public string Title { get; set; }
    public int NrSeededAttractions {get;  set;}
    public int NrUnseededAttractions {get; set;}
    public int NrSeededCategories {get; set;}
    public int NrUnseededCategories {get; set;}
    public int NrSeededComments {get; set;}
    public int NrUnseededComments {get; set;}
    public int NrSeededAddresses {get; set;}
}

public class GstUsrInfoAttractionsDto
{
    public string City { get; set;}
    public string Country { get; set; }
    public int NrAttractions {get; set;} = 0;
}

public class GstUsrInfoCategoriesDto
{
    public string CategoryName { get; set; } 
    public int NrAttractions {get; set;} = 0;
}


public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
    public List<GstUsrInfoAttractionsDto> Attractions { get; set; } = null;
    public List<GstUsrInfoCategoriesDto> Categories { get; set; } = null;
}
