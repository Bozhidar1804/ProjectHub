using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using ProjectHub.Data.Models.Enums;
using ProjectHub.Data;
using ProjectHub.Services.Data.Interfaces;
using ProjectHub.Services.Data;
using ProjectHub.Data.Models;
using ProjectHub.Web.ViewModels.Candidature;
using static ProjectHub.Common.GeneralApplicationConstants;

namespace ProjectHub.Services.Tests
{
    [TestFixture]
    public class CandidatureServiceTests
    {
        private ICandidatureService candidatureService;
        private ProjectHubDbContext dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProjectHubDbContext>()
                .UseInMemoryDatabase("CandidatureServiceTestDb")
                .Options;

            dbContext = new ProjectHubDbContext(options);
            candidatureService = new CandidatureService(dbContext);

            // Create test data
            ApplicationUser user1 = new ApplicationUser { Id = Guid.NewGuid(), UserName = "user1", Email = "user1@example.com", FullName = "User1" };
            ApplicationUser user2 = new ApplicationUser { Id = Guid.NewGuid(), UserName = "user2", Email = "user2@example.com", FullName = "User2" };
            Project project = new Project { Id = Guid.NewGuid(), Name = "Test Project", CreatorId = user1.Id, Description = "Project's Description" };

            dbContext.Users.AddRange(user1, user2);
            dbContext.Projects.Add(project);
            dbContext.SaveChanges();

            List<CandidatureContentModel> content = new List<CandidatureContentModel>
            {
                new CandidatureContentModel { Question = Question1, Answer = "Test Answer 1" },
                new CandidatureContentModel { Question = Question2, Answer = "Test Answer 1" },
                new CandidatureContentModel { Question = Question3, Answer = "Test Answer 1" },
                new CandidatureContentModel { Question = Question4, Answer = "Test Answer 1" },
            };
            string serializedContent = JsonConvert.SerializeObject(content);

            // Create test candidatures
            dbContext.Candidatures.Add(new Candidature
            {
                Id = Guid.NewGuid(),
                Content = serializedContent,
                ProjectId = project.Id,
                ApplicantId = user1.Id,
                Status = CandidatureStatus.Pending,
                IsDeleted = false,
                ApplicationDate = DateTime.UtcNow
            });
            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [Test]
        public async System.Threading.Tasks.Task CreateCandidatureAsync_WithValidData_ShouldCreateCandidature()
        {
            // Arrange
            var model = new CandidatureCreateInputModel
            {
                ProjectId = dbContext.Projects.First().Id,
                Answer1 = "Test Answer 1",
                Answer2 = "Test Answer 2",
                Answer3 = "Test Answer 3",
                Answer4 = "Test Answer 4"
            };
            string userId = dbContext.Users.First().Id.ToString();

            // Act
            bool result = await candidatureService.CreateCandidatureAsync(model, userId);

            // Assert
            Assert.IsTrue(result);
            Assert.That(dbContext.Candidatures.Count(), Is.EqualTo(2));
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllCandidaturesAsync_WithValidUserId_ShouldReturnCandidatures()
        {
            // Arrange
            string userId = dbContext.Users.First().Id.ToString();

            // Act
            IEnumerable<CandidatureIndexViewModel> candidatures = await candidatureService.GetAllCandidaturesAsync(userId);

            // Assert
            Assert.IsNotNull(candidatures);
            Assert.That(candidatures.Count(), Is.EqualTo(1));
        }

        [Test]
        public async System.Threading.Tasks.Task GetCandidatureByIdAsync_WithValidId_ShouldReturnCandidature()
        {
            // Arrange
            string candidatureId = dbContext.Candidatures.First().Id.ToString();

            // Act
            Candidature candidature = await candidatureService.GetCandidatureByIdAsync(candidatureId);

            // Assert
            Assert.IsNotNull(candidature);
            Assert.That(candidature.Id.ToString(), Is.EqualTo(candidatureId));
        }

        [Test]
        public async System.Threading.Tasks.Task UpdateCandidatureAsync_WithValidData_ShouldUpdateCandidature()
        {
            // Arrange
            Candidature candidature = dbContext.Candidatures.First();
            candidature.Status = CandidatureStatus.Approved;

            // Act
            await candidatureService.UpdateCandidatureAsync(candidature);
            dbContext.Entry(candidature).Reload();

            // Assert
            Assert.That(candidature.Status, Is.EqualTo(CandidatureStatus.Approved));
        }

        [Test]
        public async System.Threading.Tasks.Task GetCandidatureDetailsAsync_WithValidId_ShouldReturnDetails()
        {
            // Arrange
            string candidatureId = dbContext.Candidatures.First().Id.ToString();

            // Act
            CandidatureDetailsViewModel details = await candidatureService.GetCandidatureDetailsAsync(candidatureId);

            // Assert
            Assert.IsNotNull(details);
            Assert.That(details.Id, Is.EqualTo(candidatureId));
        }
    }
}
