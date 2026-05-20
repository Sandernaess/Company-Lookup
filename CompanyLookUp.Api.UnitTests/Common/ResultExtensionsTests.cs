using CompanyLookup.Api.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace CompanyLookUp.Api.UnitTests.Common
{
    [TestClass]
    public class ResultExtensionsTests
    {
        [TestMethod]
        public void ToHttpResult_Generic_When_Success_Returns_Ok()
        {
            // Arrange
            var result = Result.Success("value");

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var okResult = httpResult as Ok<string>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("value", okResult.Value);
        }

        [TestMethod]
        public void ToHttpResult_Generic_When_Validation_Returns_BadRequest()
        {
            // Arrange
            var result = Result.Failure<string>("Invalid input.", ErrorType.Validation);

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void ToHttpResult_Generic_When_NotFound_Returns_NotFound()
        {
            // Arrange
            var result = Result.Failure<string>("Not found.", ErrorType.NotFound);

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void ToHttpResult_Generic_When_Conflict_Returns_Conflict()
        {
            // Arrange
            var result = Result.Failure<string>("Conflict.", ErrorType.Conflict);

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.Conflict, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void ToHttpResult_Generic_When_Unauthorized_Returns_Unauthorized()
        {
            // Arrange
            var result = Result.Failure<string>("Unauthorized.", ErrorType.Unauthorized);

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, statusCodeResult.StatusCode);
        }

        // Non-generic Result tests
        [TestMethod]
        public void ToHttpResult_When_Success_Returns_Ok()
        {
            // Arrange
            var result = Result.Success();

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.OK, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void ToHttpResult_When_Validation_Returns_BadRequest()
        {
            // Arrange
            var result = Result.Failure("Invalid input.", ErrorType.Validation);

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void ToHttpResult_When_NotFound_Returns_NotFound()
        {
            // Arrange
            var result = Result.Failure("Not found.", ErrorType.NotFound);

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void ToHttpResult_When_Conflict_Returns_Conflict()
        {
            // Arrange
            var result = Result.Failure("Conflict.", ErrorType.Conflict);

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.Conflict, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void ToHttpResult_When_Unauthorized_Returns_Unauthorized()
        {
            // Arrange
            var result = Result.Failure("Unauthorized.", ErrorType.Unauthorized);

            // Act
            var httpResult = result.ToHttpResult();

            // Assert
            var statusCodeResult = httpResult as IStatusCodeHttpResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, statusCodeResult.StatusCode);
        }
    }
}
