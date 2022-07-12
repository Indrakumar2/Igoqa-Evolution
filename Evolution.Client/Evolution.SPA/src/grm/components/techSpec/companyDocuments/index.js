import CompanyDocuments from './companyDocuments';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';

const mapStateToProps = (state) => {
    return {
        CompanyDocumentsData: []
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

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(CompanyDocuments));