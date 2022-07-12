import CustomInput from './inputControlls';
import { connect } from 'react-redux';
const mapStateToProps = (state) => {    
    return {
        pageMode:state.CommonReducer.currentPageMode
    };
};
export default connect(mapStateToProps)(CustomInput);