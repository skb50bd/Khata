using System.ComponentModel.DataAnnotations;

using Domain;

namespace DTOs;

public class ServiceDto
{
    public int Id { get; set; }
    public bool IsRemoved { get; set; }
    public string Name { get; set; }

    public int OutletId { get; set; }

    public OutletDto Outlet { get; set; }

    public string Description { get; set; }

    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    public Metadata Metadata { get; set; }
}