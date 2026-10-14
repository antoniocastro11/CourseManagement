namespace CourseManagementAPI.UnitTests;

public class MapperTests
{
    [Fact]
    public void Profiles_ConfiguracaoValida() => TestDb.CreateConfig().AssertConfigurationIsValid();
}
