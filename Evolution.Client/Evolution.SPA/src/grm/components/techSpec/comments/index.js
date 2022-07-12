import Comments from './comments';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { isEmptyReturnDefault, isUndefined,getlocalizeData } from '../../../../utils/commonUtils';
import { withRouter } from 'react-router-dom';
import {
    AddComments,EditComments
} from '../../../actions/techSpec/commentsAction';

import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { userTypeCheck } from '../../../../selectors/techSpechSelector';
import { applicationConstants } from '../../../../constants/appConstants';
const localConstant = getlocalizeData();
const mapStateToProps = (state) => {
    return { 
        commentsData:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistNotes),
        loginUser:state.appLayoutReducer.loginUser,
        epin:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo) 
        ? null : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin,
        pageMode:state.CommonReducer.currentPageMode,
        activities:state.appLayoutReducer.activities,
        draftDataToCompare: state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare,
        draftDataNotes: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.draftDataToCompare.technicalSpecialistNotes),
        selectedDraftNotes: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistNotes),
        isRCUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.RC }),
        isRMUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.RM }),
        isTMUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.TM }),
        isTSUserTypeCheck:userTypeCheck( { array:isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE)),param:localConstant.techSpec.userTypes.TS }),
        selectedProfileDetails: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo,'object'),//SystemRole based UserType relevant Quick Fixes 
        assignedToUser:isUndefined(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo)?'':state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDraft.TechnicalSpecialistInfo.assignedToUser
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                
                AddComments,
                DisplayModal,
                 HideModal,
                 EditComments,//D661 issue8
            },
            dispatch
        ),
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Comments));
