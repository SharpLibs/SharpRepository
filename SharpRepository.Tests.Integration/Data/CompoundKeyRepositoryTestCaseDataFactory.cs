using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.EfRepository;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.TestObjects;
using System.Collections.Generic;
using System.Linq;


namespace SharpRepository.Tests.Integration.Data
{
    public class CompoundKeyRepositoryTestCaseDataFactory
    {
        public static IEnumerable<TestCaseData> Build(RepositoryType[] includeType)
        {
            if (includeType.Contains(RepositoryType.InMemory))
            {
                yield return new TestCaseData(new InMemoryRepository<User, string, int>()).SetName("InMemoryRepository Test");
            }

            if (includeType.Contains(RepositoryType.Ef))
            {
                var dbPath = EfDataDirectoryFactory.Build();
                yield return new TestCaseData(new EfRepository<User, string, int>(new TestObjectContext("Data Source=" + dbPath))).SetName("EfRepository Test");
            }

            if (includeType.Contains(RepositoryType.EfCore))
            {
                var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                     .UseInMemoryDatabase("integration test")
                     .Options;

                // Create the schema in the database
                var context = new TestObjectContextCore(options);
                context.Database.EnsureCreated();
                yield return new TestCaseData(new EfCoreRepository<User, string, int>(context)).SetName("EfCoreRepository Test");
            }
        }
    }
}