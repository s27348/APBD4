using LegacyApp;

namespace LegacyAppTests;

public class FakeUserCreditService : IClientLimitService
{
        public int GetCreditLimit(string lastName, DateTime birthdate)
        {
            return 200;
        }
}