import Documentation from './documentation';

import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { DashboardDocumentationDetails } from '../../../../actions/dashboard/documentationAction';
const mapStateToProps = (state) => {
    return {
      documentationDetails:state.documentationReducer.documentationDetails
    };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
            DashboardDocumentationDetails
        }, 
        dispatch
      ),
    };
  };

export default connect(mapStateToProps,mapDispatchToProps)(Documentation);