import DivisionCostCenterMargin from './divisionCostCenterMargin';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchCompanyDivision,
    FetchDivisionCostCenter,
    AddNewDivision,
    AddNewDivisionCostCentre,
    UpdateCompanyDivisionCostcentre,
    UpdateCompanyDivision,
    DeleteCompanyDivision,
    UpdateCompanyDivisionButton,
    DeleteCompanyDivisionCostCentre,
    UpdateCostCentreButton,
    UpdateDivisionNameInCostCenter,
    UpdateSelectedDivisionData,
} from '../../../viewComponents/company/companyAction';
import {
    DisplayModal,
    HideModal
} from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';

const mapStateToProps = (state) => {
    return {
        companyDivision: state.CompanyReducer.companyDetail.CompanyDivisions,
        companyDivisionCostCenter: state.CompanyReducer.companyDetail.CompanyDivisionCostCenters === null ? [] : state.CompanyReducer.companyDetail.CompanyDivisionCostCenters,
        editedDivisionCostCentre: isEmptyReturnDefault(state.CompanyReducer.editCompanyDivisionCostCenter,'object'),
        selectedCompanyCode: state.CompanyReducer.selectedCompanyCode,
        loginUser: state.appLayoutReducer.loginUser,
        EditCompanyDivisionCostcentre: state.CompanyReducer.EditCompanyDivisionCostcentre,
        isEditCompanyDivisionCostCenterUpdate: state.CompanyReducer.isEditCompanyDivisionCostCenterUpdate,
        isEditCompanyDivision: state.CompanyReducer.isEditCompanyDivision,
        divisionNames: state.appLayoutReducer.divisionName,
        isbtnDisable:state.CompanyReducer.isbtnDisable,
        pageMode:state.CommonReducer.currentPageMode,
        selectedDivisionData: state.CompanyReducer.selectedDivisionData,
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchCompanyDivision,
                FetchDivisionCostCenter,
                AddNewDivision,
                AddNewDivisionCostCentre,
                UpdateCompanyDivisionCostcentre,
                UpdateCompanyDivision,
                DeleteCompanyDivision,
                UpdateCompanyDivisionButton,
                UpdateCostCentreButton,
                DeleteCompanyDivisionCostCentre,
                DisplayModal,
                HideModal,
                UpdateDivisionNameInCostCenter,
                UpdateSelectedDivisionData,
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(DivisionCostCenterMargin);