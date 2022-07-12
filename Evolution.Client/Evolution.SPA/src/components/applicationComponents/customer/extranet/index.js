import PortalAccess from './extranet';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { FetchProjectList } from '../../../../actions/project/projectSearchAction'; 
import { 
    AddExtranetUser,
    AssignExtranetUserToContact,
    ExtranetUserAccountsList
} from '../../../viewComponents/customer/customerAction';
const mapStateToProps = (state) => {
    return {
          customerAddressData: state.CustomerReducer.customerDetail.Addresses,
          securityQuestionsMasterData: state.RootTechSpecReducer.TechSpecDetailReducer.passwordSecurityQuestionArray, 
          extranetUser:isEmptyReturnDefault(state.CustomerReducer.extranetUser,'object'),
          extranetUsers:isEmptyReturnDefault(state.CustomerReducer.extranetUsers),
          selectedCustomerCode:state.CustomerReducer.selectedCustomerCode,
          selectedCompany:state.appLayoutReducer.selectedCompany,
          companyList:state.appLayoutReducer.companyList, 
          customerProjectList:isEmptyReturnDefault(state.RootProjectReducer.ProjectDetailReducer.projectSearchList),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            { 
                AddExtranetUser,
                AssignExtranetUserToContact, 
                ExtranetUserAccountsList, 
                FetchProjectList             
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(PortalAccess);