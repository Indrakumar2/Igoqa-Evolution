import WonLostReport from './wonLostReport';
import {
    bindActionCreators
} from 'redux';
import {
    withRouter
} from 'react-router-dom';
import {
    isEmptyReturnDefault
} from '../../../../utils/commonUtils';
import {
    FetchCordinators,
    FetchWonLostDetails
} from '../../../actions/techSpec/wonLostAction';
import {
    connect
} from 'react-redux';
import { FetchDispositionType   } from '../../../actions/techSpec/preAssignmentAction';

const mapStateToProps = (state) => {    
    return {
        companyList: isEmptyReturnDefault(state.appLayoutReducer.companyList),
        companyCoordinator: isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectCoordinator),
        wonLostDatas: isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.wonLostDatas),
        selectedCompany: state.appLayoutReducer.selectedCompany,        
        dispositionType: state.RootTechSpecReducer.TechSpecDetailReducer.dispositionType        
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators({
            FetchCordinators,
            FetchWonLostDetails,
            FetchDispositionType
        },
            dispatch

        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(WonLostReport));