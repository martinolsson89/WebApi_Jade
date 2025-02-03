using System;
using Configuration;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public int NrAttractions {get;  set;}
    public int NrCategories {get; set;}
    public int NrAddresses {get; set;}
} 


public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
}


