using CarDealership.enums;

namespace CarDealership.dto;

public class OrderReviewDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    public RequestStatus Status { get; set; }
    public string? Message { get; set; }

    public bool RequiresDeliveryAddress { get; set; }
    public bool RequiresCardNumber { get; set; }

    public string? CardNumber { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
