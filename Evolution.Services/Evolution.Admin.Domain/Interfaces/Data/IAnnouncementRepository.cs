using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Admin.Domain.Models.Admins;

namespace Evolution.Admin.Domain.Interfaces.Data
{
    public interface IAnnouncementRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Announcement>
    {
        IList<DomainModel.Announcement> Search(DomainModel.Announcement searchModel);
    }
}
