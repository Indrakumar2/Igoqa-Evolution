import EmailTemplates from './emailTemplates';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchPlaceholders, UpdateEmailTemplate, UpdateEmailTemplateType,UpdateCompanyEmailTemplate } from '../../../viewComponents/company/companyAction';

const mapStateToProps = (state) => {
    return {
        companyEmailTemplates: state.CompanyReducer.companyDetail.CompanyEmailTemplates,
        emailTemplate: state.CompanyReducer.emailTemplate,
        emailTemplateType:state.CompanyReducer.emailTemplateType,
        editorPlaceholders: state.CompanyReducer.editorPlaceholders,
        pageMode: state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchPlaceholders,
                UpdateEmailTemplate,
                UpdateEmailTemplateType,
                UpdateCompanyEmailTemplate
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(EmailTemplates);