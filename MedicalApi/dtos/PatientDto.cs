public class PatientDto
 // defaults zeby nie wywalalo bledow pzy kompilacji
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime Birthdate { get; set; } = default!;
}