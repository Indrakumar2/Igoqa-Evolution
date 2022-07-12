using Evolution.Common.Models.Messages;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IActiveDirectoryService
    {
        bool IsValidADUsers(List<KeyValuePair<string, string>> usernames, out List<ValidationMessage> validationMessage);
    }
}
