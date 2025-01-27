using System;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Models.DTO;

public class CategoryCuDto
{

     public virtual Guid CategoryId { get; set; }
    public CategoryNames Name { get; set; } // Gör detta till en enum sen?

    public CategoryCuDto()
    {
        
    }

    public CategoryCuDto(ICategory org)
    {
        Name = org.Name;
        CategoryId = org.CategoryId;
    }

}