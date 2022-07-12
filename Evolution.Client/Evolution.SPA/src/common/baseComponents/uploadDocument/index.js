import UploadDocument from './uploadDocument';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { FetchDocumentUniqueName,RemoveDocumentsFromDB } from '../../commonAction';
import { bindActionCreators } from 'redux';
import { isEmptyReturnDefault } from '../../../utils/commonUtils';
import { UploadDocumentDetails } from './uploadDocumentAction';
import { UpdateSensitiveDetails, IsRemoveDocument } from '../../../grm/actions/techSpec/SensitiveDocumentsAction';
const mapStateToProps = (state) => {
    return {
        loggedInUser: state.appLayoutReducer.loginUser,
        currentPage:state.CommonReducer.currentPage,
        selectedEpinNo:state.RootTechSpecReducer.TechSpecDetailReducer.selectedEpinNo,
        uploadDocument: isEmptyReturnDefault(state.UploadDocumentReducer.uploadDocument),
    };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {          
            FetchDocumentUniqueName,
            RemoveDocumentsFromDB,
            UploadDocumentDetails,
            UpdateSensitiveDetails,
            IsRemoveDocument
        }, 
        dispatch
      ),
    };
  };
export default withRouter(connect(mapStateToProps,mapDispatchToProps)(UploadDocument));
