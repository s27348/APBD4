using LegacyApp;

namespace LegacyAppTests;

public class UserServiceTests
{
    [Fact]
    public void AdddUser_Should_Return_False_When_Email_Without_At_And_Dot()
    {
        //Arrange
        string firstname = "John";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "doe";
        var service = new UserService();

        //Act
        bool result = service.AddUser(firstname, lastName, email, birthDate, clientId);

        //Assert
        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AdddUser_Should_Return_False_When_Age_Lower_Than_21()
    {
        //Arrange
        string firstname = "John";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(2010, 1, 1);
        int clientId = 1;
        string email = "j.doe@abc.com";
        var service = new UserService();

        //Act
        bool result = service.AddUser(firstname, lastName, email, birthDate, clientId);

        //Assert
        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AdddUser_Should_Return_False_When_Client_Isnt_Important_And_Credit_Limit_Lower_Than_500()
    {
        // Arrange
        var clientRepository = new FakeClientRepository();
        var creditService = new FakeUserCreditService();
        var userService = new UserService(clientRepository, creditService);
    
        // Act
        var result = userService.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1990, 1, 1), 1);
    
        //Assert
        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AdddUser_Should_Return_False_When_First_Name_Is_Null_Or_Empty()
    {
        //Arrange
        string firstname = "";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "j.doe@abc.com";
        FakeClientRepository clientRepository = new FakeClientRepository();
        FakeUserCreditService creditService = new FakeUserCreditService();
        var service = new UserService(clientRepository, creditService);
    
        //Act
        bool result = service.AddUser(firstname, lastName, email, birthDate, clientId);
    
        //Assert
        Assert.Equal(false, result);
    }
}