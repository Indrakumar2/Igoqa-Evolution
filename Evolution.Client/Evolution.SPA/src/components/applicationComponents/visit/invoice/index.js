import Invoice from './invoice';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';

const mapStateToProps = (state) => { 
    return {
        InvoiceData:[]
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
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(Invoice));