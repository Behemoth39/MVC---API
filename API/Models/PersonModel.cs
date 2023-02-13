using System.ComponentModel.DataAnnotations;

namespace westcoasteducation.api.Models;

public class PersonModel
{
    [Key]
    public int Id { get; set; }
    public int Age { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
