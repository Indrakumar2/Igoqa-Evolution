using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IExportPrefixService : IMasterService
    {
        Response SaveExportPrefix(IList<ExportPrefix> model, bool returnResultSet = false);

        Response Search(Models.ExportPrefix search);

        Response Search(IList<string> prefixesNames);

        bool IsValidExportPrefixes(IList<string> exportPrefixNames, ref IList<string> exportPrefixNotExist, ref IList<ExportPrefix> exportPrefixes, ref List<MessageDetail> errorMessages);
    }
}
