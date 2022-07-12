import Payroll from './payroll';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchPayrollData,
    FetchPayrollPeriodName,
    AddNewPayroll,
    AddNewPayrollPeriodName,
    FetchCostSaleReference,
    PayrollPopupClear,
    DeletePayrollType,
    UpdateCompanyPayrollButton,
    UpdateCompanyPayroll,
    DeletePayrollPeriodName,
    UpdatePayrollPeriodName,
    TogglePayrollPeriodNameButton,
    UpdateOverrideCostSaleReference,
    UpdatePayrollPeriodNameInPayrollPeriod,
    updateAvgTSHourlyCost,
    UpdateSelectedPayrollData,
    EditPayrollPeriodName
} from '../../../viewComponents/company/companyAction';
import { FetchCurrency,FetchPayrolls } from '../../../appLayout/appLayoutActions';
import {
    DisplayModal,
    HideModal
} from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault, isEmpty } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
        PayrollData: isEmptyReturnDefault(state.CompanyReducer.companyDetail.CompanyPayrolls),
        CostOfSaleReference: state.CompanyReducer.CostOfSaleReference,
        //PayrollPeriodName:state.CompanyReducer.companyDetail.CompanyPayrollPeriods,
        PayrollPeriodName: isEmptyReturnDefault(state.CompanyReducer.companyDetail.CompanyPayrollPeriods),
        selectedCompanyCode: state.CompanyReducer.selectedCompanyCode,
        loginUser: state.appLayoutReducer.loginUser,
        isEditCompanyPayroll: state.CompanyReducer.isEditCompanyPayroll,
        currency: state.masterDataReducer.currencyMasterData,
        editedPairollPeriodName: state.CompanyReducer.editPayrollPeriodName,
        showButton: state.CompanyReducer.showButton,
        masterPayrolls: isEmptyReturnDefault(state.appLayoutReducer.payrolls),
        exportPrefixes: state.appLayoutReducer.exportPrefixes,
        isCOSEmailOverrideAllow: !isEmpty(state.CompanyReducer.companyDetail.CompanyInfo) ?state.CompanyReducer.companyDetail.CompanyInfo.isCOSEmailOverrideAllow:false,        
        avgTSHourlyCost: state.CompanyReducer.companyDetail.CompanyInfo?state.CompanyReducer.companyDetail.CompanyInfo.avgTSHourlyCost:'',
        pageMode:state.CommonReducer.currentPageMode,
        selectedPayrollData:state.CompanyReducer.selectedPayrollData
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchPayrollData,
                FetchPayrollPeriodName,
                AddNewPayroll,
                AddNewPayrollPeriodName,
                FetchCostSaleReference,
                PayrollPopupClear,
                DeletePayrollType,
                UpdateCompanyPayrollButton,
                DisplayModal,
                HideModal,
                UpdateCompanyPayroll,
                FetchCurrency,
                DeletePayrollPeriodName,
                UpdatePayrollPeriodName,
                TogglePayrollPeriodNameButton,
                UpdateOverrideCostSaleReference,
                FetchPayrolls,
                UpdateSelectedPayrollData,
                UpdatePayrollPeriodNameInPayrollPeriod,
                EditPayrollPeriodName,
                updateAvgTSHourlyCost
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Payroll);