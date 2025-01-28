using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;
using Microsoft.Extensions.Azure;

namespace Models.DTO;

//DTO is a DataTransferObject, can be instanstiated by the controller logic
//and represents a, fully instantiable, subset of the Database models
//for a specific purpose.

//These DTO are simplistic and used to Update and Create objects
public class AttractionCuDto
{
    public virtual Guid? ZooId { get; set; }

    public string Description { get; set; }

    public Guid CategoryId { get; set; }

    public AttractionCuDto() { }
    public AttractionCuDto(IAttraction org)
    {
        ZooId = org.AttractionId;
        Description = org.Description;

        CategoryId = org.Category.CategoryId;
    }
}