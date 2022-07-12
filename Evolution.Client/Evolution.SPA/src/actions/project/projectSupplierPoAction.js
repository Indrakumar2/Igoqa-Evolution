import { projectActionTypes } from '../../constants/actionTypes';
import { projectAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData,isEmpty ,parseValdiationMessage } from '../../utils/commonUtils';
import { ShowLoader, HideLoader } from '../../common/commonAction';
const localConstant = getlocalizeData();

const actions = {
    FetchSupplierPo: (payload) => ({
        type: projectActionTypes.FETCH_SUPPLIERPO,
        data: payload
    }),
};
export const FetchSupplierPo = () => async (dispatch, getstate) => {
    const state = getstate();
    dispatch(ShowLoader());
    const supplierPOPrejectNo = isEmpty(state.RootProjectReducer.ProjectDetailReducer.projectDetail)?'':state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo.projectNumber;
    const projectSupplierPo = projectAPIConfig.baseUrl + projectAPIConfig.supplierPo;
    const  params = {
        SupplierPOProjectNumber:supplierPOPrejectNo,
        ModuleName:"SPO"
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(projectSupplierPo, requestPayload)
        .catch(error => {      
            // console.error(error); // To show the error details in console      
            // IntertekToaster(localConstant.project.PROJECT_AUPPLIERPO_NOT_FETCHED, 'dangerToast projectAssignment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchSupplierPo(response.result));
        dispatch(HideLoader());
    }
    else if (response.code === "11" || response.code === "41" || response.code === "31") {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast SupplierPOWentWrong');
        dispatch(HideLoader());
    }
    else {        
        IntertekToaster(localConstant.project.PROJECT_AUPPLIERPO_NOT_FETCHED, 'dangerToast assignemntSomthingWrong');
        dispatch(HideLoader());
    }
};