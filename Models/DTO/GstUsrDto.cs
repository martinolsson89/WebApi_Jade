using System;
using Configuration;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public int NrSeededAttractions {get;  set;}
    public int NrUnseededAttractions {get; set;}
    public int NrSeededCategories {get; set;}
    public int NrUnseededCategories {get; set;}
    public int NrSeededComments {get; set;}
    public int NrUnseededComments {get; set;}
    public int NrSeededAddresses {get; set;}
} 

public class GstUsrInfoCommentsDto
{
}

public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
}
