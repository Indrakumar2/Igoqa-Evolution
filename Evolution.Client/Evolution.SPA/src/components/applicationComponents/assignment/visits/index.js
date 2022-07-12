import Visits from './visits';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchAssignmentVisits } from '../../../../actions/visit/visitAction';
const mapStateToProps = (state) => {
    return {
        //selectedVisitStatus:state.rootVisitReducer.selectedVisitStatus ,
        visitData:state.rootVisitReducer.visitList
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {            
                FetchAssignmentVisits
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Visits);
