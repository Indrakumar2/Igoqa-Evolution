import HyperLink from './hyperLink';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { GetSelectedProfile, GetSelectedDraftProfile ,FetchSavedDraftProfile } from '../../../actions/techSpec/techSpecSearchActions';
import { ARSSearchPanelStatus } from '../../../../actions/assignment/assignedSpecialistsAction';
import { GetMyTaskARSSearch } from '../../../../actions/assignment/arsSearchAction';
// import { UpdateCurrentPage ,UpdateCurrentModule,SetCurrentPageMode } from '../../../../common/commonAction';
const mapStateToProps = (state) => {
  return {
    
  };
};
const mapDispatchToProps = dispatch => {
  return {
    actions: bindActionCreators(
      {
        GetSelectedProfile,
        GetSelectedDraftProfile,
        FetchSavedDraftProfile,
        ARSSearchPanelStatus,
        GetMyTaskARSSearch,
        
      },
      dispatch
    ),
  };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(HyperLink));