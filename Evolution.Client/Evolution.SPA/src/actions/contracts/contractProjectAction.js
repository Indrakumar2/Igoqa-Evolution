//import { processApiRequest,RequestPayload } from '../../../src/services/api/defaultServiceApi';
import { contractAPIConfig ,RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { contractActionTypes } from '../../constants/actionTypes';
import { FetchData } from '../../services/api/baseApiService';
import { isEmpty } from '../../utils/commonUtils';
const actions = {
    FetchContractProjects: (payload) => ({
        type: contractActionTypes.FETCH_CONTRACT_PROJECTS,
        data: payload
    })
};

export const FetchContractProjects = (status) => async (dispatch, getstate) => {    
    const data = {
        responseResult:null,
        selectedValue:status
    };
    if(status === ''){
        const data = {
            responseResult:[],
            selectedValue:status
        };
        dispatch(actions.FetchContractProjects(data));
        return false;
    }
    dispatch(actions.FetchContractProjects(data));
    const state = getstate();
    const selectedcontractNumber = isEmpty(state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo.contractNumber)?'':state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo.contractNumber; //Changes for D-1014
    const projectDataUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.projects + contractAPIConfig.contractNumber + encodeURIComponent(selectedcontractNumber);
   
    let params = {};
    if(selectedcontractNumber){
        if(status === "all"){
            params = {
                contractProjectNumber:selectedcontractNumber
            };
        }else{
            params = {
                contractProjectNumber:selectedcontractNumber,
                projectStatus:status
            };
        }
    }
    if(!isEmpty(params)){
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(projectDataUrl, requestPayload)
            .catch(error => {  
                // console.error(error); // To show the error details in console          
                // IntertekToaster('Fetch Project  Something went wrong', 'dangerToast projectAssignment');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            const data = {
                responseResult:response.result,
                selectedValue:status
            };
            dispatch(actions.FetchContractProjects(data));
            return response.result;
        }
        else {        
            IntertekToaster('Fetch Project  Something went wrong', 'dangerToast assignemntSomthingWrong');
        }
    }
};
