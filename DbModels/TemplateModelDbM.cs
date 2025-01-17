using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;

namespace DbModels;

[Table("MusicGroups", Schema = "supusr")]
public class MusicGroupDbM : MusicGroup
{
    [Key]
    public override Guid Id { get; set; }

    [Required]
    public  override string Name { get; set;}
}