import CalendarScheduleDetailsReport from './calendarScheduleDetailsReport';
import { bindActionCreators } from 'redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { connect } from 'react-redux';
import { FetchCalendarScheduleDetails, FetchResourceInfo, ClearSearchData, FetchContractHoldingCoordinator } from '../../../actions/techSpec/reportsAction';
import {
    FetchSupplierSearchList,
    ClearSupplierSearchList,
} from '../../../../actions/supplierPO/supplierPOSearchAction';
// import { 
//     FetchCoordinatorForContractHoldingCompany, 
//     FetchCoordinatorForOperatingCompany
//  } from '../../../../actions/assignment/generalDetailsActions';
import { ClearData } 
from  '../../../../../src/components/applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import arrayUtil from '../../../../utils/arrayUtil';

const mapStateToProps = (state) => {    
    return {
        companyList: isEmptyReturnDefault(state.appLayoutReducer.companyList),
        calendarSchedulesData:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.calendarSchedulesData),
        defaultCustomerName:state.CustomerAndCountrySearch.defaultCustomerName,
        supplierList: state.rootSupplierPOReducer.supplierList,
        //operatingCompanyCoordinators: isEmptyReturnDefault(state.rootAssignmentReducer.operatingCompanyCoordinators).length >0 ? state.rootAssignmentReducer.operatingCompanyCoordinators.filter(x=>x.isActive === true) : [],
        //contractHoldingCompanyCoordinators: isEmptyReturnDefault(state.rootAssignmentReducer.contractHoldingCompanyCoordinators).length >0 ? state.rootAssignmentReducer.contractHoldingCompanyCoordinators.filter(x=>x.isActive === true) : [],
        companyBasedTSData:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.companyBasedTSData),
        coordinators:isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.coordinators) && arrayUtil.sort(state.RootTechSpecReducer.TechSpecDetailReducer.coordinators,'displayName','asc')
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators({
            FetchCalendarScheduleDetails,
            FetchSupplierSearchList,
            ClearSupplierSearchList,
            FetchContractHoldingCoordinator,
            ClearData,
            FetchResourceInfo,
            ClearSearchData,
        },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(CalendarScheduleDetailsReport));