import VisitReference from './visitReference';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, } from '../../../../utils/commonUtils';
import {
    FetchVisitReference,
    //FetchReferencetypes,
    AddVisitReference,
    UpdateVisitReference,
    DeleteVisitReference
} from '../../../../actions/visit/visitReferenceAction';
import {
    DisplayModal,
    HideModal
} from '../../../../common/baseComponents/customModal/customModalAction';

const mapStateToProps = (state) => {
    return {        
        VisitReferences: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitReferences),
        visitReferenceTypes: state.rootVisitReducer.visitReferenceTypes,
        loggedInUser: state.appLayoutReducer.loginUser,
        currentPage: state.CommonReducer.currentPage,
        isTBAVisitStatus: state.rootVisitReducer.isTBAVisitStatus,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchVisitReference,
                //FetchReferencetypes,
                AddVisitReference,
                UpdateVisitReference,
                DeleteVisitReference,
                DisplayModal,
                HideModal
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(VisitReference));