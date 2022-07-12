using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Master.Domain.Models;
using System.Collections.Generic;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface IEmailPlaceholderRepository : IGenericRepository<EmailPlaceHolder>
    {
        IList<EmailPlaceholder> Search(EmailPlaceholder search);
    }
}
