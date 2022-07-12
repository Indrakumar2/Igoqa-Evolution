import OperationalDocuments from './operationalDocuments';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';

const mapStateToProps = (state) => {
    return {
        OperationalDocumentsData: []
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
             
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(OperationalDocuments));