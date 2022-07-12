import Notes from './notes';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { AddCompanyNotesDetails,
       ShowButtonHandler,
       EditCompanyNotesDetails } from '../../../viewComponents/company/companyAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
const mapStateToProps = (state) => {    
    return {
        companyNotesData:state.CompanyReducer.companyDetail==={}?[]: state.CompanyReducer.companyDetail.CompanyNotes,
        loggedInUser: state.appLayoutReducer.username,    
        loggedInUserName: state.appLayoutReducer.loginUser,  
        interactionMode: state.CommonReducer.interactionMode,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {       
                AddCompanyNotesDetails,
                ShowButtonHandler,
                DisplayModal,
                HideModal,
                EditCompanyNotesDetails //D661 issue8
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Notes);
