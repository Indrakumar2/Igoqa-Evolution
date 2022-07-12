import { projectActionTypes } from '../../constants/actionTypes';
import { assignmentAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../services/api/baseApiService';
import { getlocalizeData,isEmpty,parseValdiationMessage } from '../../utils/commonUtils';
import { required } from '../../utils/validator';
const localConstant = getlocalizeData();

const actions = {
    FetchAssignments: (payload) => ({
        type: projectActionTypes.FETCH_ASSIGNMENT,
        data: payload
    }),
};
export const FetchAssignments = (status) => async (dispatch, getstate) => {
    const data = {
        responseResult:null,
        selectedValue:status
    };
    if(status === ''){
        const data = {
            responseResult:[],
            selectedValue:status
        };
        dispatch(actions.FetchAssignments(data));
        return false;
    }
    dispatch(actions.FetchAssignments(data));
    const state = getstate();
    const contractProjectNumber = state.RootProjectReducer.ProjectDetailReducer.selectedProjectNo;
    if (!required(contractProjectNumber) && contractProjectNumber >0) {
    // const projectAssignments = projectAPIConfig.baseUrl + projectAPIConfig.assignments;
        const projectAssignments=assignmentAPIConfig.assignment;
        let params;
        if(status === "all"){
            params = {
                assignmentProjectNumber:contractProjectNumber,
                ModuleName:"ASGMNT"
            };
        }
        else if(status === "No"){
            params = {
                assignmentProjectNumber:contractProjectNumber,
                assignmentStatus:'P',
                ModuleName:"ASGMNT"
            };
        } else {
            params = {
                assignmentProjectNumber:contractProjectNumber,
                assignmentStatus:'C',
                ModuleName:"ASGMNT"
            };
        }
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(projectAssignments, requestPayload)
            .catch(error => {   
                // console.error(error); // To show the error details in console 
                // IntertekToaster(localConstant.project.assignments.PROJECT_ASSIGNEMNT_NOT_FETCHED, 'dangerToast projectAssignment');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            const data = {
                responseResult:isEmpty(response.result)?[]:response.result,
                selectedValue:status
            };
            dispatch(actions.FetchAssignments(data));
        }
        else if (response && response.code && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast AsgntFetchWentWrong');
        }
        else {
            IntertekToaster(localConstant.project.assignments.PROJECT_ASSIGNEMNT_NOT_FETCHED, 'dangerToast assignemntSomthingWrong');
        }
    }
};