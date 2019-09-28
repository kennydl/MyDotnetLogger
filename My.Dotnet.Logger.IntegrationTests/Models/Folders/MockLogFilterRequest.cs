using My.Dotnet.Logger.TableStorage.Models;

namespace My.Dotnet.Logger.IntegrationTests.Models.Folders
{
    internal class MockLogFilterRequest : LogFilterRequest
    {
        public MockLogFilterRequest()
        {
            TableName = SetupFixture.Fixture.TableName;
        }
    }
}
