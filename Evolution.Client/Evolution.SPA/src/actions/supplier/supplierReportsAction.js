import { getlocalizeData,isEmpty } from '../../utils/commonUtils';
import { RequestPayload, supplierAPIConfig } from '../../apiConfig/apiConfig';
import { FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { supplierActionTypes } from '../../constants/actionTypes';
const localConstant = getlocalizeData();

const actions = {
    fetchSupplierVisitPerformanceReport: (payload) => ({
        type: supplierActionTypes.FETCH_SUPPLIER_VISIT_PERFORMANCE_REPORT,
        data: payload
    }),
    UpdateReportCustomer: (payload) => ({
        type: supplierActionTypes.UPDATE_REPORT_CUSTOMER,
        data: payload

    }),
    ClearCustomerData: (payload) =>({
       type: supplierActionTypes.CLEAR_CUSTOMER_DATA,
       data: payload
    }),
};

export const UpdateReportCustomer =(data)=> async(dispatch,getstate) =>
{
    dispatch(actions.UpdateReportCustomer(data));
};

export const ClearCustomerData =(data) => async(dispatch,getstate) =>
{
     dispatch(actions.ClearCustomerData());
};

export const fetchSupplierVisitPerformanceReport = (data) => async(dispatch,getstate) => {
   
    const state = getstate();
    const supplierSearchUrl = supplierAPIConfig.supplierBaseUrl + supplierAPIConfig.supplierVisitPerformanceSearch;
    const params = data;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(supplierSearchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SUPPLIER_SEARCH_LIST_VALIDATION, 'warningToast supplierSearchFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            if(response.result.length>0)
            {  
            dispatch(actions.fetchSupplierVisitPerformanceReport(response.result));
            return response;
            }
            else {
                IntertekToaster(localConstant.validationMessage.SUPPLIER_VISIT_PERFORMANCE_NO_DATA_FOUND,'dangerToast fetchSupplierSearchFailure');
            }
        }  
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchSupplierSearchFailure');
    }

};