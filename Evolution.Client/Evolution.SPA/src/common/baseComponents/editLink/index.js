import EditLink from './editLink';
import { connect } from 'react-redux';

const mapStateToProps = (state) => {
    return {
        projectInteractionMode: state.CommonReducer.interactionMode,
        interactionMode: state.CommonReducer.interactionMode,
        contractInteractionMode:state.RootContractReducer.ContractDetailReducer.interactionMode,
        pageMode: state.CommonReducer.currentPageMode
    };
};

export default connect(mapStateToProps, null)(EditLink);