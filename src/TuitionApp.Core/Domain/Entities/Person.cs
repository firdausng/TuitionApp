namespace TuitionApp.Core.Domain.Entities
{
    public abstract class Person: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
