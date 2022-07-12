using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Google.Model.Models;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GoogleModel = Evolution.Google.Model;

namespace Evolution.TechSpecialist.GeoCoordinate.Extractor
{
    public class SyncService
    {
        private readonly GoogleModel.Interfaces.IMongoGeoCoordinateService _mongoGeoCoordinateService = null;
        private readonly GoogleModel.Interfaces.IGeoCoordinateService _googleCoordinateService = null;
        private readonly AppEnvVariableBaseModel _syncSetting = null;
        private readonly IAppLogger<SyncService> _logger = null;
        private readonly ITechnicalSpecialistContactService _tsContactService = null;

        public SyncService(GoogleModel.Interfaces.IMongoGeoCoordinateService mongoGeoCoordinateService,
                            GoogleModel.Interfaces.IGeoCoordinateService googleCoordinateService,
                            ITechnicalSpecialistContactService tsContactService,
                            IOptions<AppEnvVariableBaseModel> syncSetting,
                            IAppLogger<SyncService> logger)
        {
            _mongoGeoCoordinateService = mongoGeoCoordinateService;
            _googleCoordinateService = googleCoordinateService;
            _tsContactService = tsContactService;
            _syncSetting = syncSetting.Value;
            _logger = logger;
        }

        public void PrformExtractOperation()
        {
            TimerCallback callback = new TimerCallback(Tick);

            // create a second timer tick
            System.Threading.Timer stateTimer = new System.Threading.Timer(callback, null, 0, (_syncSetting.TaskRunIntervalInMinute * 60000));

            Console.WriteLine("Don't Press Enter Key Oterwise System Will Shutdown.");

            Console.ReadLine();
        }

        private void Tick(Object stateInfo)
        {
            try
            {
                Console.Clear();

                Console.WriteLine("Don't Press Enter Key Otherwise System Will Shutdown.");

                this.Start();
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                //Console.WriteLine(ex.ToFullString());
            }
        }

        private void Start()
        {
            PrintMessage("Geo Coordinate Extract Started", null);

            IList<TechnicalSpecialistContactInfo> tsContacts = null;
            var addToBeSync = this.GetContactsToSync(ref tsContacts);

            PrintMessage(string.Format("Total no. of address found ({0}) to be sync.", addToBeSync?.Count), null);

            var searchResponse = this._mongoGeoCoordinateService.SearchAndSyncToMongo(addToBeSync);
            
            var tsIds = tsContacts.Join(searchResponse,
                                        tsc => new { tsc.Country, tsc.County, tsc.City, tsc.PostalCode },
                                        mc => new { mc.Country, County = mc.State, mc.City, PostalCode = mc.Zip },
                                        (tsc, mc)=> tsc.Id)?.Distinct().ToList();            
            var response = this._tsContactService.UpdateContactSyncStatus(tsIds);

            if (response.Code == ResponseType.Success.ToId())
                PrintMessage("Geo Coordinate Extract Completed.", null);
            else
                PrintMessage(string.Format("Geo Coordinate Extract could not completed because of below error.{0} {1}", Environment.NewLine, response.Description), null);
        }

        private void PrintMessage(string message, object document)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0}  DateTime :- {1}", message, DateTime.Now.ToString()));
            if (document != null)
            {
                Console.WriteLine("Document Detail :-");
                Console.WriteLine(document?.ToText());
            }
            _logger.LogInformation("", string.Format("{0} $ DateTime :- {1}", message, DateTime.Now.ToString()), document?.ToText());
        }

        private IList<MongoGeoCoordinateInfo> GetContactsToSync(ref IList<TechnicalSpecialistContactInfo> tsContacts)
        {
            IList<MongoGeoCoordinateInfo> result = null;

            var response = this._tsContactService.Get(new TechnicalSpecialistContactInfo()
            {
                ContactType = ContactType.PrimaryAddress.ToString(),
                IsGeoCordinateSync = false
            },100);

            if (response?.Code == ResponseType.Success.ToId())
            {
                if (response.Result != null)
                {
                    tsContacts = response.Result
                                         .Populate<IList<TechnicalSpecialistContactInfo>>();
                    var searchModels = tsContacts.Select(x => new MongoGeoCoordinateInfo()
                    {
                        City = x.City,
                        State = x.County,
                        Country = x.Country,
                        Zip = x.PostalCode
                    }).ToList();

                    result = searchModels;
                }
            }
            return result;
        }
    }
}