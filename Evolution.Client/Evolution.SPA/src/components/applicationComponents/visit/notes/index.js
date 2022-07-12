import Notes from './notes';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmpty, isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { FetchVisitNotes, AddUpdateVisitNotes,EditVisitNotes } from '../../../../actions/visit/notesAction';
import {
    isCoordinatorCompany,
    isOperatorCompany 
} from '../../../../selectors/visitSelector';

const mapStateToProps = (state) => { 
    const visitInfo = isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo, 'object');    
    const isOCCompany = isOperatorCompany(visitInfo.visitOperatingCompanyCode, state.appLayoutReducer.selectedCompany);
    return {        
        visitInfo: visitInfo,
        VisitNotes: isEmpty(state.rootVisitReducer.visitDetails.VisitNotes) ? [] :
            state.rootVisitReducer.visitDetails.VisitNotes,
        loggedInUser: state.appLayoutReducer.username,
        loggedInUserName: state.appLayoutReducer.loginUser,
        currentPage: state.CommonReducer.currentPage,
        isTBAVisitStatus: state.rootVisitReducer.isTBAVisitStatus,
        pageMode:state.CommonReducer.currentPageMode,
        visitId: state.rootVisitReducer.visitDetails.VisitInfo ?
        state.rootVisitReducer.visitDetails.VisitInfo.visitId : null,        
        isOperatorApporved: (isOCCompany && [ 'O','A' ].includes(visitInfo.visitStatus) ? true : false)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchVisitNotes,
                AddUpdateVisitNotes,
                EditVisitNotes,
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(Notes));