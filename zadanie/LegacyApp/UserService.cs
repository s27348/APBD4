using System;

namespace LegacyApp
{
    public interface IClientLimitService
    {
        int GetCreditLimit(string lastName, DateTime birthdate);
    }
    
    public interface IClientRepository
    {
        Client GetById(int idClient);
    }
    public class UserService
    {
        private const int MinimumAge = 21;
        private const double MinimumCreditLimit = 500;
        private IClientRepository _clientRepository;

        private IClientLimitService _creditService; 

        [Obsolete]

        public UserService()
        {
            _clientRepository = new ClientRepository();
            _creditService = new UserCreditService();
        }
        
        public UserService(IClientRepository clientRepositiory, IClientLimitService creditService)
        {
            _clientRepository = clientRepositiory;
            _creditService = creditService;
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (ValidateUserInput(firstName, lastName, email, dateOfBirth) == false)
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);

            var user = CreateUser(client, firstName, lastName, email, dateOfBirth);

            SetCreditLimit(client, user);

            if (IsCreditLimitSufficient(user) == false)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private bool ValidateUserInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            { 
                age--;
            }
            
            return age >= MinimumAge;
        }

        private User CreateUser(Client client, string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }

        private void SetCreditLimit(Client client, User user)
        {
            if (client.Type == "VeryImportantClient")
                user.HasCreditLimit = false;
            else if (client.Type == "ImportantClient")
            {
                int creditLimit = _creditService.GetCreditLimit(user.LastName, user.DateOfBirth) * 2;
                user.HasCreditLimit = true;
                user.CreditLimit = creditLimit;
            }
            else
            {
                int creditLimit = _creditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.HasCreditLimit = true;
                user.CreditLimit = creditLimit;
            }
        }

        private bool IsCreditLimitSufficient(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= MinimumCreditLimit;
        }
    }
}