namespace HealthyJuices.Shared.Dto
{
    public record CompanyDto(long Id, string Name, string Comment, string PostalCode, string City, string Street, double Latitude, double Longitude);
}