 import { dashBoardActionTypes } from '../../constants/actionTypes';
import { homeAPIConfig,RequestPayload, masterData } from '../../apiConfig/apiConfig';
 import { isEmpty } from '../../utils/commonUtils';
 import { getlocalizeData,parseValdiationMessage } from '../../utils/commonUtils'; 
 import { PostData } from '../../services/api/baseApiService';
 import IntertekToaster from '../../common/baseComponents/intertekToaster';
 import { ShowLoader,HideLoader } from '../../common/commonAction';
import { applicationConstants } from '../../constants/appConstants';
 const localConstant = getlocalizeData();
const actions ={
    DashboardDocumentationDetails : (payload) => {
        return {
            type: dashBoardActionTypes.FETCH_DOCUMENTATION_DETAILS,
            data: payload
        };
    },
};

export const DashboardDocumentationDetails = (data) => async (dispatch,getstate) =>{
    dispatch(ShowLoader());
    const DashboardDocumentationUrl = homeAPIConfig.homeBaseURL + masterData.systemSettings;

    const requestPayload = new RequestPayload(applicationConstants.documentationContants);
    const response = await PostData(DashboardDocumentationUrl, requestPayload)
        .catch(error => {   
            // console.error(error); // To show the error details in console         
            // IntertekToaster(localConstant.contract.childContract.FETCH_CHILD_CONTRACT_FAILED, 'dangerToast projectAssignment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response && response.code === "1") {
        const documentationData = [];
        // ITK D 1100 changes.
        if(response.result && response.result.length > 0){ 
            for(let i=0 ; i<applicationConstants.documentationContants.length/2 ; i++)
            { 
                const desc = response.result.filter(x => x.keyName === ( i>0 ? 'DescText'+i : 'DescText'));
                const url = response.result.filter(x => x.keyName === (i>0 ? 'URLText'+i : 'URLText')); 
                if(desc && desc.length > 0 && !isEmpty(desc[0].keyValue ) && url && url.length > 0 && !isEmpty(url[0].keyValue)){
                    documentationData.push({ name :desc[0].keyValue, description :  url[0].keyValue  });
                }
            } 
        }
        dispatch(actions.DashboardDocumentationDetails(documentationData));
        dispatch(HideLoader());
    }
    else if(response && response.code && (response.code === "11" || response.code === "41")){
        IntertekToaster(parseValdiationMessage(response), 'warningToast dadshboarddocedetails');
        dispatch(HideLoader());
    }
    else {        
        IntertekToaster(localConstant.contract.childContract.FETCH_CHILD_CONTRACT_FAILED, 'dangerToast assignemntSomthingWrong');
        dispatch(HideLoader());
    }
};