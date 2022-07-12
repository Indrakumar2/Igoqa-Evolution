import TechSpecMyTask from './techSpecMytask';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { 
    FetchTechSpecMytaskData,
    FetchMyTaskAssignUsers,
    UpdateMyTaskReassign
} from '../../../actions/techSpec/techSpecMytaskActions';
import { TechSpecClearSearch } from '../../../actions/techSpec/techSpecSearchActions';
import {   
    grmSearchMatsterData
} from '../../../../common/masterData/masterDataActions';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { 
    DisplayModal, 
    HideModal
} from '../../../../common/baseComponents/customModal/customModalAction';
import {    
    Dashboardrefresh,
    FetchDashboardCount
  } from '../../../../components/viewComponents/dashboard/dahboardActions';

const mapStateToProps = (state) => {
    return {       
        techSpecMytaskData:state.RootTechSpecReducer.TechSpecDetailReducer.techSpecMytaskData,
        isARSSearch:state.rootAssignmentReducer.isARSSearch,
        techSpecAssignUser: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.techSpecMytaskAssignUsers),
    }; 
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {   grmSearchMatsterData,        
                FetchTechSpecMytaskData,
                TechSpecClearSearch,
                FetchMyTaskAssignUsers,
                UpdateMyTaskReassign,
                DisplayModal, 
                HideModal,
                Dashboardrefresh,
                FetchDashboardCount
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(TechSpecMyTask));