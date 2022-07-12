//import { processApiRequest,RequestPayload } from '../../../src/services/api/defaultServiceApi';
import { contractAPIConfig ,RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { contractActionTypes } from '../../constants/actionTypes';
import { FetchData } from '../../services/api/baseApiService';
import { isEmpty } from '../../utils/commonUtils';
import { getlocalizeData } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
const actions = {
    FetchChildContractsOfPrarent: (payload) => ({
        type: contractActionTypes.childContract.FETCH_CHILD_CONTRACTS_OF_PARENT,
        data: payload
    })
};

export const FetchChildContractsOfPrarent = (status) => async (dispatch, getstate) => {
    const data = {
        responseResult:null,
        selectedValue:status
    };
    dispatch(actions.FetchChildContractsOfPrarent(data));
    const state = getstate();
    const customerDataSelected=state.RootContractReducer.ContractDetailReducer.selectedCustomerData;
    const selectedParentcontractNumber = isEmpty(customerDataSelected)?'':customerDataSelected.contractNumber;
    const childContractDataUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contractSearch + contractAPIConfig.parentContractNumber + selectedParentcontractNumber; 
    let params;
    if(status === "all"){
        params = {
            parentContractNumber:selectedParentcontractNumber
        };
    }else{
        params = {
            parentContractNumber:selectedParentcontractNumber,
            contractStatus:status
        };
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(childContractDataUrl, requestPayload)
        .catch(error => { 
            // console.error(error); // To show the error details in console           
            // IntertekToaster(localConstant.contract.childContract.FETCH_CHILD_CONTRACT_FAILED, 'dangerToast projectAssignment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const data = {
            responseResult:response.result,
            selectedValue:status
        };
        dispatch(actions.FetchChildContractsOfPrarent(data));
    }
    else {        
        IntertekToaster(localConstant.contract.childContract.FETCH_CHILD_CONTRACT_FAILED, 'dangerToast assignemntSomthingWrong');
    }
};
