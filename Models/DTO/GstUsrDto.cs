using System;
using Configuration;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public int NrAttractions {get;  set;}
    public int NrCategories {get; set;}
    public int NrComments {get; set;}
} 

public class GstUsrInfoCommentsDto
{
    public string Content { get; set; } = null;
    public int NrAnimals { get; set; } = 0;
}

public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
    public List<GstUsrInfoCommentsDto> Comments { get; set; } = null;
}


