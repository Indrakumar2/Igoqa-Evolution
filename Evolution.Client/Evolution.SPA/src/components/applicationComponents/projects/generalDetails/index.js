import GeneralDetails from './generalDetails';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { 
    FetchProjectCompanyCostCenter,
    FetchProjectCompanyDivision,
    FetchProjectCompanyOffices,
    UpdateProjectGeneralDetails 
} from '../../../../actions/project/generalDetailsAction';
import { FetchModuleLogo } from '../../../../common/masterData/masterDataActions';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps=(state)=>{
    return{
		projectCompanyDivision: state.RootProjectReducer.ProjectDetailReducer.projectCompanyDivision,
		projectCompanyCostCenter: state.RootProjectReducer.ProjectDetailReducer.projectCompanyCostCenter,
		projectCompanyOffices: state.RootProjectReducer.ProjectDetailReducer.projectCompanyOffices,
		businessUnit: state.masterDataReducer.businessUnit,
        industrySector: state.masterDataReducer.industrySector,
        projectLogo: state.masterDataReducer.logo,
        projectCoordinator: state.RootProjectReducer.ProjectDetailReducer.projectCoordinator,
        masterCoordinatorData :  state.masterDataReducer.coordinators,
        projectMICoordinator : state.RootProjectReducer.ProjectDetailReducer.projectMICoordinator,
		managedServiceType: isEmptyReturnDefault(state.masterDataReducer.managedServiceType),
		generalDetails: isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo,'object'),
        // interactionMode: state.CommonReducer.interactionMode,
        loggedInUser: state.appLayoutReducer.loginUser,
        projectMode:state.RootProjectReducer.ProjectDetailReducer.projectMode
    };
};

const mapDispatchToProps=(dispatch)=>{
return{
    actions:bindActionCreators(
        {
			FetchProjectCompanyCostCenter,
			FetchProjectCompanyDivision,
			FetchProjectCompanyOffices,
            UpdateProjectGeneralDetails,
            FetchModuleLogo,
        },dispatch
    )
};
};
export default connect(mapStateToProps,mapDispatchToProps)(GeneralDetails);