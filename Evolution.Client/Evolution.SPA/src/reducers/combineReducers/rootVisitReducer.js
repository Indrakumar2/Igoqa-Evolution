import reduceReducers from 'reduce-reducers';
import { VisitReducer } from './../visit/visitReducer';
import { GeneralDetailReducer } from './../visit/generalDetailReducer';
import { HistoricalVisitReducer } from './../visit/historicalVisitReducer';
import { DocumentsReducer } from './../visit/documentsReducer';
import { InterCompanyDiscountsReducer } from './../visit/interCompanyDiscountsReducer';
import { NotesReducer } from './../visit/notesReducer';
import { SupplierPerformanceReducer } from './../visit/supplierPerformanceReducer';
import { TechnicalSpecialistReducer } from './../visit/technicalSpecialistReducer';
import { VisitReferenceReducer } from './../visit/visitReferenceReducer';

const initialState = {
    visitDetail:{},
    visitList:[],
    contractHoldingCompany:'',
    chCoordinators:[],
    ocCoordinators:[],
    visitStartDate: '',
    visitEndData: '',
    visitDetails: '',
    historicalVisits: [],
    visitStatus: [] ,
    selectedVisitData: {},
    supplierList: [],
    subSupplierList: [],
    visitSupplierList: [],
    finalVistList: [],
    VisitTechnicalSpecialists: [],
    rateSchedule: '',
    VisitTechnicalSpecialistTimes: [],
    VisitTechnicalSpecialistTravels: [],
    VisitTechnicalSpecialistExpenses: [],
    VisitTechnicalSpecialistConsumables: [],
    visitValidationData:{},
    defaultTechSpecRateSchedules:[],
    visitSelectedTechSpecs:[],
    isTBAVisitStatus: false,
    visitTechnicalSpecialistsGrossMargin: null,
    supplierPerformanceTypeList:[],
    selectedVisitStatus: '',
    isTechSpecRateChanged: false,
    isShowAllRates: false,
    isExpenseOpen: true,
    visitExchangeRates: []
};

export default reduceReducers (
    VisitReducer,
    GeneralDetailReducer,
    HistoricalVisitReducer,
    DocumentsReducer,
    InterCompanyDiscountsReducer,
    NotesReducer,
    SupplierPerformanceReducer,
    TechnicalSpecialistReducer,
    VisitReferenceReducer,
    initialState
);