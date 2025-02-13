using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;

public class AttractionServiceDb : IAttractionService
{
    private readonly AttractionDbRepos _attractionDbRepos;
    private readonly AddressDbRepos _addressDbRepos;
    private readonly ILogger<AttractionServiceDb> _logger;    
    
    public AttractionServiceDb(AttractionDbRepos attractionDbRepos, AddressDbRepos addressDbRepos, ILogger<AttractionServiceDb> logger)
    {
        _attractionDbRepos = attractionDbRepos;
        _addressDbRepos = addressDbRepos;
        _logger = logger;
    }

    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat) => _attractionDbRepos.ReadItemAsync(id, flat);
    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _attractionDbRepos.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id) => _attractionDbRepos.DeleteItemAsync(id);
    public Task<ResponseItemDto<IAttraction>> PostAttractionAsync(AttractionCuDto item) => _attractionDbRepos.CreateItemAsync(item);
    public Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto item) => _attractionDbRepos.UpdateItemAsync(item);

    public async Task<ResponseItemDto<IAttraction>> CreateAttractionWithAddressAsync(AttractionCuDto newAttraction)
    {
        try
        {
            // Kontrollera att adressen existerar
            var address = await _addressDbRepos.ReadItemAsync(newAttraction.AddressId, true);
            if (address?.Item == null)
            {
                throw new ArgumentException($"Address with id {newAttraction.AddressId} does not exist");
            }

            // Skapa sevärdheten
            var result = await _attractionDbRepos.CreateItemAsync(newAttraction);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating attraction with address: {ex.Message}");
            throw;
        }
    }
}


/*
using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;
using System;
using System.Threading.Tasks;

namespace Services;

public class AttractionServiceDb : IAttractionService
{
    private readonly AttractionDbRepos _attractionDbRepos;
    private readonly AddressDbRepos _addressDbRepos; 
    private readonly ILogger<AttractionServiceDb> _logger;

    public AttractionServiceDb(AttractionDbRepos attractionDbRepos, AddressDbRepos addressDbRepos, ILogger<AttractionServiceDb> logger)
    {
        _attractionDbRepos = attractionDbRepos;
        _addressDbRepos = addressDbRepos; 
        _logger = logger;
    }

    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat) 
        => _attractionDbRepos.ReadItemAsync(id, flat);

    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) 
        => _attractionDbRepos.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);

    public Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id) 
        => _attractionDbRepos.DeleteItemAsync(id);

    public async Task<ResponseItemDto<IAttraction>> PostAttractionAsync(AttractionCuDto item)
    {
        // Kontrollera att adressen existerar
        var address = await _addressDbRepos.ReadItemAsync(item.AddressId, true);
        if (address?.Item == null)
        {
            throw new ArgumentException($"Address with id {item.AddressId} does not exist");
        }

        // Skapa sevärdheten
        return await _attractionDbRepos.CreateItemAsync(item);
    }

    public async Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto item)
    {
        // Kontrollera att adressen existerar
        var address = await _addressDbRepos.ReadItemAsync(item.AddressId, true);
        if (address?.Item == null)
        {
            throw new ArgumentException($"Address with id {item.AddressId} does not exist");
        }

        // Uppdatera sevärdheten
        return await _attractionDbRepos.UpdateItemAsync(item);
    }
}

*/