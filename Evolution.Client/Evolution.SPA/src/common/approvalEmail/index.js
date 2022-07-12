import ApprovalEmail from './approvalEmail';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { FetchDocumentUniqueName } from '../../common/commonAction';
import { AddAttachedDocuments } from '../../actions/visit/visitAction';
import { isEmptyReturnDefault } from '../../utils/commonUtils';
import { StaticRouter } from 'react-router-dom';

const mapStateToProps = (state) => {
    return {
        loggedInUser:state.appLayoutReducer.userName,
        attachedDocs: isEmptyReturnDefault(state.rootVisitReducer.attachedFiles)
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchDocumentUniqueName,
                AddAttachedDocuments
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(ApprovalEmail);