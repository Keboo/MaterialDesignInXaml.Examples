using System.Collections.Generic;
using Bogus;

namespace TestData
{
    public static class Data
    {
        public static IList<Person> GeneratePeople(int count)
        {
            Faker<Person> generator = new Faker<Person>()
                .StrictMode(true)
                .RuleFor(x => x.ID, f => f.IndexGlobal)
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .RuleFor(x => x.DOB, f => f.Person.DateOfBirth);

            return generator.Generate(count);
        }
    }
}