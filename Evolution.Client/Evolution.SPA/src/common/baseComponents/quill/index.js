import QuillComponent from './quill';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { UpdateCompanyEmailTemplate } from '../../../components/viewComponents/company/companyAction';
import { DisplayModal, HideModal } from '../customModal/customModalAction';

const mapStateToProps = (state) => {
    return {
        editorPlaceholders: state.CompanyReducer.editorPlaceholders
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                UpdateCompanyEmailTemplate,
                DisplayModal, 
                HideModal
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(QuillComponent);