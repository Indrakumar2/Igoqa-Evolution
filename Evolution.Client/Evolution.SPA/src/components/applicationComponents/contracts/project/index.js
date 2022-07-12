import Project from './project';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { FetchContractProjects } from '../../../../actions/contracts/contractProjectAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
       contractprojectDetail:isEmptyReturnDefault(state.ContractProjectReducer.ContractProjects),
       selectedProjectStatusValue:state.ContractProjectReducer.selectedProjectStatus
    };
};
    const mapDispatchToProps = dispatch => {
        return {
            actions: bindActionCreators(
                {
                    FetchContractProjects
                },
                dispatch
            ),
        };
    };
export default connect(mapStateToProps, mapDispatchToProps)(Project);
