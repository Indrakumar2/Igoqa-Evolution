import MySearchAnchor from './mySearchAnchor';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { FetchQuickSearchData } from '../../../../actions/techSpec/quickSearchAction';
import { FetchPreAssignment } from '../../../../actions/techSpec/preAssignmentAction';

const mapStateToProps = (state) => {
    return {
    };
  };
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {
          FetchQuickSearchData,
          FetchPreAssignment,
        },
        dispatch
      ),
    };
};

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(MySearchAnchor));