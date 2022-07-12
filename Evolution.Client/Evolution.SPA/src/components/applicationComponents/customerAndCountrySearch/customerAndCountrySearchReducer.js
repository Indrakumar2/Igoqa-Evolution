import { customerCountrySearch } from '../../../constants/actionTypes';

const initialState = {
    customerName: "",    
    countryMasterData: [],
    selectedCustomerData: [],
    customerContract: [],
    defaultCustomerName:'',
    defaultCustomerId:'',
    customerList:[],
    selectedCountryName:'',
    reportsCustomerName:"",
    selectedReportCountryName:'',
    defaultReportCustomerName:'',
    
};

export const CustomerAndCountrySearch = (state = initialState, action) => {
    const { type, data } = action;   
    switch (type) {
        case customerCountrySearch.SET_DEFAULT_CUSTOMER_NAME:
            state = {
                ...state,
                defaultCustomerName: data.customerName || '',
                defaultCustomerId:   data.customerId || ''          
            };
            return state;

            case customerCountrySearch.SET_DEFAULT_REPORT_CUSTOMER_NAME:  
            state = {
                ...state,
                defaultReportCustomerName: data.customerName || ''             
            };
            return state;
     
        // case contractActionTypes.SHOW_FIXED_MODAL:
        //     state = {
        //         ...state,
        //         isShowModal: !state.isShowModal,
        //         customerList: []

        //     };
        //     return state;
        // case contractActionTypes.HIDE_FIXED_MODAL:
        //     state = {
        //         ...state,
        //         isShowModal: !state.isShowModal,
        //         //customerName:''
        //     };
        //     return state;
        // case companyActionTypes.COUNTRY:
        //     state = {
        //         ...state,
        //         countryMasterData: data
        //     };
        //     return state;
        case customerCountrySearch.FETCH_CUSTOMER_COUNTRY_LIST:   
            state = {
                ...state,
                customerList: data
            };
            return state;
        case customerCountrySearch.SELECTED_CUSTOMER_NAME:       
            state = {
                ...state,
                customerName: data[0].customerName,
                customerList:[],
                selectedCustomerData: [ ...data ],
            };
            return state;  

            case customerCountrySearch.UPDATE_REPORT_CUSTOMER:
                state = {
              
                    ...state,
                    reportsCustomerName: data[0].customerName,
                    defaultReportCustomerName: data[0].customerName,
    
                }; 
               return state;
            case customerCountrySearch.CLEAR_CUSTOMER_DATA:
                state = {
                    ...state,
                    reportsCustomerName: '',
                    customerList:[],
                    defaultReportCustomerName:''
    
                };
                return state;

        case customerCountrySearch.CLEAR_DATA:
            state = {
                ...state,
                customerContract: [],
                selectedCustomerData: [],
                customerName: '',
                defaultCustomerName: '',
                defaultCustomerId:'',
                selectedCountryName:'',
                //contractStatus: 'Open',
                customerList:[]
            };
            return state;
        case customerCountrySearch.SET_SELECTED_COUNTRY_NAME:
            state = {
                ...state,
                selectedCountryName:data
            };
            return state;

            case customerCountrySearch.SET_REPORT_SELECTED_COUNTRY_NAME:
                state = {
                    ...state,
                    selectedReportCountryName:data
                };
                return state;
         case customerCountrySearch.CLEAR_REPORTS_DATA:
            state = {
                ...state,
                selectedReportCountryName:'',
                reportsCustomerName: '',
                defaultReportCustomerName:'',

            };
            return state;
        default:
            return state;
    }
};