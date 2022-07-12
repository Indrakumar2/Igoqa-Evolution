import Notes from './notes';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmpty } from '../../../../utils/commonUtils';
import { AddContractNotesDetails,EditContractNotesDetails } from '../../../../actions/contracts/noteAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
const mapStateToProps = (state) => {    
    return {
        contractsNotesData:isEmpty(state.RootContractReducer.ContractDetailReducer.contractDetail.ContractNotes)?
        []:state.RootContractReducer.ContractDetailReducer.contractDetail.ContractNotes,
        loggedInUser: state.appLayoutReducer.username,  
        loggedInUserName: state.appLayoutReducer.loginUser,  
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                DisplayModal,
                HideModal,
                AddContractNotesDetails,
                EditContractNotesDetails //D661 issue8
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(Notes);
