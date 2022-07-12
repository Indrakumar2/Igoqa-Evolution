using Evolution.Common.Models.Responses;
using Evolution.ILearn.Domain.Models;
using System.Collections.Generic;

namespace Evolution.ILearn.Domain.Interfaces
{
    public interface ILearnInterface
    {
        Response Save(List<LearnData> learnDatas);

        Response AddCompantancyData();

        Response AddInternalTrainingData();

        Response AddCertificateData(); //D1172

        Response AddTrainingData(); //D1172
    }
}
