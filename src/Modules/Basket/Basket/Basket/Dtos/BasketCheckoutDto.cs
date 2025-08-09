namespace Basket.Basket.Dtos
{
    public record BasketCheckoutDto
    (
         string UserName,
         Guid CustomerId,
         decimal TotalPrice,
        string FirstName,
        string LastName,
        string EmailAddress,
        string AddressLine,
        string Country,
        string State,
        string ZipCode,
        string CardNumber,
        string CardName,
        string Expiration,
        string Cvv,
        int PaymentMethod
    );
}
