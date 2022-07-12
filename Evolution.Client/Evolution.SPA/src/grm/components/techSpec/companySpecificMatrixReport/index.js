import CompanySpecificMatrixReport from './companySpecificMatrixReport';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { connect } from 'react-redux';
import { FetchCompanySpecificMatrixDetails, ClearSearchData } from '../../../actions/techSpec/reportsAction';
import { ShowLoader, HideLoader } from '../../../../common/commonAction';

const mapStateToProps = (state) => {   
    return {
        companyList: isEmptyReturnDefault(state.appLayoutReducer.userRoleCompanyList),
        resourceTaxonomiesData:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.companySpecificMatrixReportData)
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators({
            ShowLoader,
            HideLoader,
            FetchCompanySpecificMatrixDetails,
            ClearSearchData
        },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(CompanySpecificMatrixReport));