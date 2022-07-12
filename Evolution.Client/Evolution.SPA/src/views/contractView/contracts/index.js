import Contract from './contracts';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ShowLoader,HideLoader } from '../../../common/commonAction';

const mapStateToProps = (state) => {
    return {      
        loader: state.CommonReducer.loader,    
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {               
                ShowLoader,
                HideLoader              
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Contract);