using LiveCodeCICD;
using LiveCodeCICD.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AnimalCountingDatabase.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(1 == 1);
        }
        [Fact]
        public async Task IntegrationTest()
        {
            //Create db 
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder
                // Uncomment the following line if you want to print generated
                // SQL statements on the console.
                // .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            var context = new ApplicationDbContext(optionsBuilder.Options);

            //DElete database
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            //create controller
            var controller = new CustomersController(context);
            //Add Customer
            await controller.Add(new Customer { Name = "Alex" });
            //Assert result

            var result = await controller.GetAll();
            Assert.Equal(1, result.Count());
        }
    }
}