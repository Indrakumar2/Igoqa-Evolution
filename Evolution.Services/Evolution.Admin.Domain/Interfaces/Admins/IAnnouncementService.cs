using Evolution.Admin.Domain.Models.Admins;
using Evolution.Common.Models.Responses;

namespace Evolution.Admin.Domain.Interfaces.Admins
{
    public interface IAnnouncementService
    {
        Response GetAnnouncement(Announcement searchModel);
    }
}
