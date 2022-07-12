import AssignmentReferenceType from './referenceType';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchReferenceTypeDetails,
    AddReferenceTypeDetails,
    UpdateReferenceTypeDetails,
    DeleteReferenceTypeDetails,
    FetchLanguges,
    SaveReferenceTypeDetails,
    CancelReferenceTypeChanges
} from '../../../../../actions/admin/referenceTypeAction';
import { DisplayModal, HideModal } from '../../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../../utils/commonUtils';

const mapStateToProps = (state) => {
    return {
        assignmentRefTypeData:isEmptyReturnDefault(state.rootAdminReducer.referenceTypeData),
        launguageList:isEmptyReturnDefault(state.rootAdminReducer.languageData),
        currentPage: state.CommonReducer.currentPage,    
        selectedLanguageVersion:"English"
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchReferenceTypeDetails,
                AddReferenceTypeDetails,
                UpdateReferenceTypeDetails,
                DeleteReferenceTypeDetails,
                FetchLanguges,
                SaveReferenceTypeDetails,
                CancelReferenceTypeChanges,
                DisplayModal,
                HideModal
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(AssignmentReferenceType);