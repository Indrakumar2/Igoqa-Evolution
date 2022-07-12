//import { ClearCompanyData, ClearUserCompanyList } from '../appLayout/appLayoutActions';
import { ClearAssignmentData } from '../../actions/assignment/assignmentAction';
import { ClearMasterData } from '../../common/masterData/masterDataActions';

const actions = {
    AboutShowModal: () => {
        return {
            type: 'ABOUT_SHOW_MODAL'            
        };
    },
    AboutHideModal: () => {
        return {
            type: 'ABOUT_HIDE_MODAL'            
        };
    }  
};
export const AboutShowModal =()=>(dispatch)=> {
    return dispatch(actions.AboutShowModal());
};
export const AboutHideModal =()=>(dispatch)=> {
    return dispatch(actions.AboutHideModal());
};
export const ClearRefreshMasterData = () => (dispatch) => {
    dispatch(ClearMasterData());//MasterAction 
    dispatch(ClearAssignmentData()); //AssignmentAction
    // dispatch(ClearCompanyData()); //CompanyAction
    // dispatch(ClearUserCompanyList());//UserCompanyList
};
