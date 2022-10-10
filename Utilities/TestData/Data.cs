using Bogus;
using System.Collections.Generic;

namespace TestData
{
    public static class Data
    {
        public static IList<Team> GenerateTeams(int count, int memberCount)
        {
            Faker<Team> teamGenerator = new Faker<Team>()
                .StrictMode(true)
                .RuleFor(x => x.Name, f => f.Name.JobDescriptor())
                .RuleFor(x => x.Members, _ => GeneratePeople(memberCount));

            return teamGenerator.Generate(count);
        }

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

        public static IList<Image> GenerateImages(int count)
        {
            Faker<Image> generator = new Faker<Image>();
            generator.RuleFor(x => x.Url, f => f.Image.PicsumUrl());
            return generator.Generate(count);
        }

        public static IList<Product> GenerateProducts(int count)
        {
            Faker<Product> generator = new Faker<Product>()
                .StrictMode(true)
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 100));

            return generator.Generate(count);
        }
    }
}