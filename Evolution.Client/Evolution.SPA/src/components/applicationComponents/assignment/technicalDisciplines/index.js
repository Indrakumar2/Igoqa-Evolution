import TechnicalDiscipilines from './technicalDisciplines';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmpty } from '../../../../utils/commonUtils';
import { 
    AddNewClassification,
    DeleteClassification
} from '../../../../actions/assignment/technicalDesciplinesActions';
import { 
    DisplayModal, 
    HideModal 
} from '../../../../common/baseComponents/customModal/customModalAction';

const mapStateToProps = (state) => {
    return {
        assignmentClassificationGrid:isEmpty(state.rootAssignmentReducer.assignmentDetail.assignmentClassificationData)?[]:state.rootAssignmentReducer.assignmentDetail.assignmentClassificationData
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                DisplayModal,
                HideModal,
                AddNewClassification,
                DeleteClassification
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps,mapDispatchToProps)(TechnicalDiscipilines);