import Assignments from './assignments';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchAssignmentSearchResults } from '../../../../actions/assignment/assignmentAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
    return {
        supplierPOGenetalDetails: isEmptyReturnDefault(state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo, 'object'),
        supplierPOAssignmentData: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentList)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchAssignmentSearchResults
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Assignments);