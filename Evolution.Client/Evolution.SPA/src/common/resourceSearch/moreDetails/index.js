import { MoreDetails } from './moreDetails';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';

const mapStateToProps = (state) => {
    return {
        taxonomyCategory: state.masterDataReducer.techSpecCategory,
        taxonomySubCategory: state.masterDataReducer.techSpecSubCategory,
        taxonomyServices: state.masterDataReducer.techSpecServices,
        assignmentTypes: state.rootAssignmentReducer.assignmentType,
    };
};

export default connect(mapStateToProps, null)(MoreDetails);