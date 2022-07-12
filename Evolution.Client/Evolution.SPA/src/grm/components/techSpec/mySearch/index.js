import MySearch from './mySearch';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { FetchMySearchData } from '../../../actions/techSpec/mySearchActions';
import { grmSearchMatsterData } from '../../../../common/masterData/masterDataActions';

const mapStateToProps = (state) => {
    return {
        mySearchData: state.RootTechSpecReducer.TechSpecDetailReducer.mySearchData
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                grmSearchMatsterData,
                FetchMySearchData,
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(MySearch));